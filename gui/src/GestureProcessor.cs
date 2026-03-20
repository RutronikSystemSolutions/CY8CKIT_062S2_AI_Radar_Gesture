using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class GestureProcessor
    {
        #region "Events"
        public delegate void OnNewTimeSignalEventHandler(object sender, int antennaIndex, double[] signal);
        public event OnNewTimeSignalEventHandler? OnNewTimeSignal;

        public delegate void OnNewMagnitudeValueEventHandler(object sender, int antennaIndex, double magnitude);
        public event OnNewMagnitudeValueEventHandler? OnNewMagnitudeValue;

        public delegate void OnNewPointEventHandler(object sender, double x, double y, double magnitude);
        public event OnNewPointEventHandler? OnNewPoint;

        public delegate void OnNewPhaseDiffEventHandler(object sender, double azimuth, double elevation);
        public event OnNewPhaseDiffEventHandler? OnNewPhaseDiff;

        public delegate void OnNewDistanceEventHandler(object sender, int antennaIndex, double distance);
        public event OnNewDistanceEventHandler? OnNewDistance;

        #endregion

        private RadarConfiguration? radarConfiguration;

        private int fftLen = 0;
        private System.Numerics.Complex[,,]? dopplerFFTMatrix;
        private SignalWindow? window;
        private SignalWindow? complexWindow;
        private double[]? timeBuffer;
        private System.Numerics.Complex[,,]? rangeFFTMatrix;
        private System.Numerics.Complex[]? binContent;
        private int[]? maxRangeIndex;
        private double[]? maxMagnitude;
        private int[]? maxVelocityIndex;

        public double thresholdTop = 0.1;
        public double thresholdBot = 0.08;
        public double maximumDistance = 0.2;

        private bool waitingSignal = true;

        private double BinToMeters(int binIndex)
        {
            if (radarConfiguration == null)
                return double.NaN;

            double c = 299792458;
            return (double)binIndex * c / (2 * radarConfiguration.GetBandWidth());
        }

        public void SetRadarConfiguration(RadarConfiguration config)
        {
            radarConfiguration = config;

            // Allocate memory
            fftLen = (config.samplesPerChirp / 2) + 1;
            dopplerFFTMatrix = new System.Numerics.Complex[config.rxAntennas, fftLen, config.chirpsPerFrame];
            window = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.samplesPerChirp);
            complexWindow = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.chirpsPerFrame);
            timeBuffer = new double[config.samplesPerChirp];
            rangeFFTMatrix = new System.Numerics.Complex[config.rxAntennas, config.chirpsPerFrame, fftLen];
            binContent = new System.Numerics.Complex[config.chirpsPerFrame];
            maxRangeIndex = new int[config.rxAntennas];
            maxMagnitude = new double[config.rxAntennas];
            maxVelocityIndex = new int[config.rxAntennas];
        }

        /// <summary>
        /// Process a frame
        /// A frame contains all the samples of all chirps for all antennas.
        /// </summary>
        /// <param name="frame"></param>
        public void ProcessFrame(ushort[] frame)
        {
            if (radarConfiguration == null)
                return;

            if (radarConfiguration == null || timeBuffer == null ||  window == null || complexWindow == null 
                || dopplerFFTMatrix == null || rangeFFTMatrix == null || binContent == null
                || maxRangeIndex == null || maxMagnitude == null || maxVelocityIndex == null)
                return;

            for (int antennaIndex = 0; antennaIndex < radarConfiguration.rxAntennas; antennaIndex++)
            {
                // Compute all the range FFTs
                for (int chirpIdx = 0; chirpIdx < radarConfiguration.chirpsPerFrame; ++chirpIdx)
                {
                    // Extract time signal of the chirp
                    int startIndex = chirpIdx * radarConfiguration.rxAntennas * radarConfiguration.samplesPerChirp;
                    for (int sampleIndex = 0; sampleIndex < radarConfiguration.samplesPerChirp; sampleIndex++)
                    {
                        int index = startIndex + sampleIndex * radarConfiguration.rxAntennas + antennaIndex;
                        timeBuffer[sampleIndex] = frame[index];
                    }

                    // Send the first one - un-processed
                    if (chirpIdx == 0)
                    {
                        OnNewTimeSignal?.Invoke(this, antennaIndex, timeBuffer);
                    }

                    // Scale, remove average and apply window
                    // Time buffer now contains the samples of one antenna during a chirp
                    // Scale between 0 and 1
                    ArrayUtils.scaleInPlace(timeBuffer, 1.0 / 4095.0);

                    // Compute the average
                    double average = ArrayUtils.getAverage(timeBuffer);

                    // Offset
                    ArrayUtils.offsetInPlace(timeBuffer, -average);

                    // Apply windows
                    window.applyInPlace(timeBuffer);

                    // Compute real FFT
                    // Size of spectrum is (SamplesPerChirp / 2) + 1
                    System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(timeBuffer);

                    // Store it inside the matrix (will be used to compute FFT per bin and the average)
                    for (int freqIndex = 0; freqIndex < spectrum.Length; freqIndex++)
                    {
                        rangeFFTMatrix[antennaIndex, chirpIdx, freqIndex] = spectrum[freqIndex];
                    }
                }

                //// Compute average per bin
                //System.Numerics.Complex [] rangeProfileStatic = new System.Numerics.Complex[fftLen];
                //for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                //{
                //    // Get the content for this frequency bin
                //    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                //    {
                //        rangeProfileStatic[freqIndex] += rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex];
                //    }

                //    rangeProfileStatic[freqIndex] = rangeProfileStatic[freqIndex] / (fftLen);
                //}

                //// Remove it
                //for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                //{
                //    // Get the content for this frequency bin
                //    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                //    {
                //        rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex] -= rangeProfileStatic[freqIndex];
                //    }

                //    rangeProfileStatic[freqIndex] = rangeProfileStatic[freqIndex] / (fftLen);
                //}

                //// Compute energy
                //double energy = 0;
                //for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                //{
                //    // Get the content for this frequency bin
                //    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                //    {
                //        energy += rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex].Magnitude;
                //    }
                //}


                // Send the time signal of the last chirp
                // OnNewTimeSignal?.Invoke(this, antennaIndex, timeBuffer);

                double maxAmplitude = double.NaN;
                int rangeForMaxAmplitude = 0;
                int velocityForMaxAmplitude = 0;

                //OnNewMagnitudeValue?.Invoke(this, antennaIndex, energy, BinToMeters(rangeForMaxAmplitude));

                // Compute Doppler FFT
                for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        binContent[chirpIndex] = rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex];
                    }

                    // Compute average and remove it (remove 0 speed)
                    System.Numerics.Complex avgComplex = ArrayUtils.getAverage(binContent);
                    ArrayUtils.offsetInPlace(binContent, -avgComplex);

                    complexWindow.applyInPlaceComplex(binContent);

                    // Get FFT (transform in place)
                    FftSharp.FFT.Forward(binContent);

                    // Get the doppler FFT for the bin
                    System.Numerics.Complex[] dopplerFFTForBin = ArrayUtils.FftShift(binContent);

                    // Copy and check for maximum
                    for (int i = 0; i < radarConfiguration.chirpsPerFrame; i++)
                    {
                        dopplerFFTMatrix[antennaIndex, freqIndex, i] = dopplerFFTForBin[i];

                        // Only if within observed range
                        if (BinToMeters(freqIndex) < maximumDistance)
                        {
                            double magnitude = dopplerFFTForBin[i].Magnitude;
                            if (double.IsNaN(maxAmplitude) || (magnitude > maxAmplitude))
                            {
                                maxAmplitude = magnitude;
                                rangeForMaxAmplitude = freqIndex;
                                velocityForMaxAmplitude = i;
                            }
                        }
                    }
                }

                maxMagnitude[antennaIndex] = maxAmplitude;
                maxRangeIndex[antennaIndex] = rangeForMaxAmplitude;
                maxVelocityIndex[antennaIndex] = velocityForMaxAmplitude;

                if (maxAmplitude < 0.1) rangeForMaxAmplitude = 0;
                OnNewMagnitudeValue?.Invoke(this, antennaIndex, maxAmplitude);
            }

            // Compute according to the configuration
            if (radarConfiguration.rxAntennas != 3) return;

            double threshold = thresholdTop;
            if (waitingSignal == false) threshold = thresholdBot;

            if (maxMagnitude[0] > threshold && maxMagnitude[1] > threshold && maxMagnitude[2] > threshold)
            {
                waitingSignal = false;

                // Compute angle of arrival with 3 antennas
                double phaseRx1 = dopplerFFTMatrix[0, maxRangeIndex[0], maxVelocityIndex[0]].Phase;
                double phaseRx2 = dopplerFFTMatrix[1, maxRangeIndex[0], maxVelocityIndex[0]].Phase;
                double phaseRx3 = dopplerFFTMatrix[2, maxRangeIndex[0], maxVelocityIndex[0]].Phase;

                double azimuth = AngleUtils.GetAngleDiff(phaseRx1, phaseRx3);
                double elevation = AngleUtils.GetAngleDiff(phaseRx2, phaseRx3);

                elevation += Math.PI / 2;
                if (elevation > Math.PI) elevation -= 2 * Math.PI;

                OnNewPhaseDiff?.Invoke(this, azimuth, elevation);
                //OnNewDistance?.Invoke(this, 0, (maxRangeIndex[0]));
                //OnNewDistance?.Invoke(this, 1, (maxRangeIndex[1]));
                //OnNewDistance?.Invoke(this, 2, (maxRangeIndex[2]));

                OnNewDistance?.Invoke(this, 0, BinToMeters(maxRangeIndex[0]));
                OnNewDistance?.Invoke(this, 1, BinToMeters(maxRangeIndex[1]));
                OnNewDistance?.Invoke(this, 2, BinToMeters(maxRangeIndex[2]));

                // Convert to real values
                double LAMBDA_VAL = 0.0049; // 5mm
                double ANTENNA_DISTANCE = 0.0025; // 2.5mm
                azimuth = Math.Sinh((LAMBDA_VAL * azimuth) / (2 * Math.PI * ANTENNA_DISTANCE));
                elevation = Math.Sinh((LAMBDA_VAL * elevation) / (2 * Math.PI * ANTENNA_DISTANCE));

                // Convert to x,y points
                double rangeM = BinToMeters(maxRangeIndex[0]);

                double x = rangeM * Math.Sin(azimuth);
                double y = rangeM * Math.Sin(elevation);

                OnNewPoint?.Invoke(this, x, y, maxMagnitude[0]);
            }
            else
            {
                waitingSignal = true;
            }
        }
    }
}
