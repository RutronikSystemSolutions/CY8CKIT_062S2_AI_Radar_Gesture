using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class RadarConfiguration
    {
        public const int RADAR_CONFIGURATION_SIZE = 33;

        public UInt64 startFreq;
        public UInt64 endFreq;
        public UInt16 samplesPerChirp;
        public UInt16 chirpsPerFrame;
        public byte rxAntennas;
        public UInt32 sampleRate;
        public float chirpRepetitionTimeS;
        public float frameRepetitionTimeS;

        public RadarConfiguration(byte [] data)
        {
            startFreq = BitConverter.ToUInt64(data, 0);
            endFreq = BitConverter.ToUInt64(data, 8);
            samplesPerChirp = BitConverter.ToUInt16(data, 16);
            chirpsPerFrame = BitConverter.ToUInt16(data, 18);
            rxAntennas = data[20];
            sampleRate = BitConverter.ToUInt32(data, 21);
            chirpRepetitionTimeS = BitConverter.ToSingle(data, 25);
            frameRepetitionTimeS = BitConverter.ToSingle(data, 29);
        }

        public int GetFrameSizeInBytes()
        {
            return samplesPerChirp * chirpsPerFrame * rxAntennas * 2; // *2 because each sample is 2 bytes (16 bits)
        }

        public int GetSamplesPerFrame()
        {
            return samplesPerChirp * chirpsPerFrame * rxAntennas;
        }

        public double GetBandWidth()
        {
            return endFreq - startFreq;
        }

        public void PrintToDiag()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("startFreq: {0}\n" +
                "endFreq: {1}\n" +
                "samplesPerChirp: {2}\n" +
                "chirpsPerFrame: {3}\n" +
                "rxAntennas: {4}\n" +
                "sampleRate: {5}\n" +
                "chirpRepetitionTimeS: {6}\n" +
                "frameRepetitionTimeS: {7}\n", 
                startFreq, endFreq, samplesPerChirp, chirpsPerFrame, rxAntennas, sampleRate, chirpRepetitionTimeS, frameRepetitionTimeS));
        }
    }
}
