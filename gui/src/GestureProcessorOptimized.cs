using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    internal class GestureProcessorOptimized
    {
        public delegate void OnNewTimeSignalEventHandler(object sender, int antennaIndex, double[] signal);
        public event OnNewTimeSignalEventHandler? OnNewTimeSignal;

        public delegate void OnNewMagnitudeValueEventHandler(object sender, int antennaIndex, double magnitude);
        public event OnNewMagnitudeValueEventHandler? OnNewMagnitudeValue;

        public delegate void OnNewPhaseDiffEventHandler(object sender, double azimuth, double elevation);
        public event OnNewPhaseDiffEventHandler? OnNewPhaseDiff;

        public delegate void OnNewPointEventHandler(object sender, double x, double y, double magnitude);
        public event OnNewPointEventHandler? OnNewPoint;

        private RadarConfiguration? radarConfiguration;
        private SignalWindow? window;
        private double[]? timeBuffer;
        private System.Numerics.Complex[,,]? rangeFFTMatrix;
        private int fftLen = 0;
        private System.Numerics.Complex[]? rangeProfileStatic;
        private double[]? energy;
        private System.Numerics.Complex[]? binContent;
        private System.Numerics.Complex[,]? dopplerFFTMatrix;
        private SignalWindow? complexWindow;
        private int[]? maxRangeIndex;
        private double[]? maxMagnitude;
        private int[]? maxVelocityIndex;

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

            fftLen = (config.samplesPerChirp / 2) + 1;
            timeBuffer = new double[config.samplesPerChirp];
            window = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.samplesPerChirp);
            rangeFFTMatrix = new System.Numerics.Complex[config.rxAntennas, config.chirpsPerFrame, fftLen];
            rangeProfileStatic = new System.Numerics.Complex[fftLen];
            energy = new double[config.rxAntennas];
            binContent = new System.Numerics.Complex[config.chirpsPerFrame];
            dopplerFFTMatrix = new System.Numerics.Complex[config.rxAntennas, config.chirpsPerFrame];
            complexWindow = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.chirpsPerFrame);
            maxRangeIndex = new int[config.rxAntennas];
            maxMagnitude = new double[config.rxAntennas];
            maxVelocityIndex = new int[config.rxAntennas];
            // Allocate memory
            /*
            fftLen = (config.samplesPerChirp / 2) + 1;
            dopplerFFTMatrix = new System.Numerics.Complex[config.rxAntennas, fftLen, config.chirpsPerFrame];
            window = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.samplesPerChirp);
            complexWindow = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, config.chirpsPerFrame);
            timeBuffer = new double[config.samplesPerChirp];
            rangeFFTMatrix = new System.Numerics.Complex[config.rxAntennas, config.chirpsPerFrame, fftLen];
            binContent = new System.Numerics.Complex[config.chirpsPerFrame];
            maxRangeIndex = new int[config.rxAntennas];
            maxMagnitude = new double[config.rxAntennas];
            maxVelocityIndex = new int[config.rxAntennas];*/
        }

        public void ProcessFrame(ushort[] frame)
        {
            if (radarConfiguration == null || timeBuffer == null
                || window == null || rangeFFTMatrix == null || rangeProfileStatic == null
                || energy == null || binContent == null || dopplerFFTMatrix == null || complexWindow == null
                || maxRangeIndex == null || maxMagnitude == null || maxVelocityIndex == null)
                return;

            double threshold = 0.5;
            int deadZone = 4;

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

                // Init to 0
                for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                {
                    rangeProfileStatic[freqIndex] = 0;
                }

                // Compute average per bin for the frame
                for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        rangeProfileStatic[freqIndex] += rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex];
                    }
                    rangeProfileStatic[freqIndex] = rangeProfileStatic[freqIndex] / (radarConfiguration.chirpsPerFrame);
                }

                // Remove it
                for (int freqIndex = 0; freqIndex < fftLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex] -= rangeProfileStatic[freqIndex];
                    }
                }

                // Compute energy
                double antennaEnergy = 0;
                for (int freqIndex = deadZone; freqIndex < fftLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        antennaEnergy += rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex].Magnitude;
                    }
                }

                OnNewMagnitudeValue?.Invoke(this, antennaIndex, antennaEnergy);
                energy[antennaIndex] = antennaEnergy;
            }

            // Compute according to the configuration
            if (radarConfiguration.rxAntennas != 3) return;

            if (energy[0] > threshold && energy[1] > threshold && energy[2] > threshold)
            {
                // Get the maximum bin index for antenna 0
                int antennaIndex = 0;
                double maxFreqEnergy = 0;
                int maxFreqIndex = 0;
                for (int freqIndex = deadZone; freqIndex < fftLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    double freqEnergy = 0;
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        freqEnergy += rangeFFTMatrix[antennaIndex, chirpIndex, freqIndex].Magnitude;
                    }
                    if (freqEnergy > maxFreqEnergy)
                    {
                        maxFreqEnergy = freqEnergy;
                        maxFreqIndex = freqIndex;
                    }
                }

                for (antennaIndex = 0; antennaIndex < radarConfiguration.rxAntennas; antennaIndex++)
                {
                    double maxAmplitude = double.NaN;
                    int rangeForMaxAmplitude = 0;
                    int velocityForMaxAmplitude = 0;

                    // Compute Doppler FFT only for this bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.chirpsPerFrame; chirpIndex++)
                    {
                        binContent[chirpIndex] = rangeFFTMatrix[antennaIndex, chirpIndex, maxFreqIndex];
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
                        dopplerFFTMatrix[antennaIndex, i] = dopplerFFTForBin[i];

                        double magnitude = dopplerFFTForBin[i].Magnitude;
                        if (double.IsNaN(maxAmplitude) || (magnitude > maxAmplitude))
                        {
                            maxAmplitude = magnitude;
                            rangeForMaxAmplitude = maxFreqIndex;
                            velocityForMaxAmplitude = i;
                        }
                    }

                    maxMagnitude[antennaIndex] = maxAmplitude;
                    maxRangeIndex[antennaIndex] = rangeForMaxAmplitude;
                    maxVelocityIndex[antennaIndex] = velocityForMaxAmplitude;
                }

                // Compute angle of arrival with 3 antennas
                double phaseRx1 = dopplerFFTMatrix[0, maxVelocityIndex[0]].Phase;
                double phaseRx2 = dopplerFFTMatrix[1, maxVelocityIndex[0]].Phase;
                double phaseRx3 = dopplerFFTMatrix[2, maxVelocityIndex[0]].Phase;

                double azimuth = AngleUtils.GetAngleDiff(phaseRx1, phaseRx3);
                double elevation = AngleUtils.GetAngleDiff(phaseRx2, phaseRx3);

                //elevation += Math.PI / 2;
                //if (elevation > Math.PI) elevation -= 2 * Math.PI;

                OnNewPhaseDiff?.Invoke(this, azimuth, elevation);

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
        }
    }
}
