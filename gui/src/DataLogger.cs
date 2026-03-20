using NumSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RadarSensorGesture
{
    internal class DataLogger
    {
        private List<ushort[]> toStoreList = new List<ushort[]>();
        private RadarConfiguration? radarConfiguration;

        public delegate void OnNewRecordedCountEventHandler(object sender, int count);
        public event OnNewRecordedCountEventHandler? OnNewRecordedCount;

        public delegate void OnNewStateEventHandler(object sender, bool state);
        public event OnNewStateEventHandler? OnNewState;

        private bool storeData = false;

        public void SetRadarConfiguration(RadarConfiguration configuration)
        {
            radarConfiguration = configuration;
            toStoreList.Clear();
        }

        public void Start()
        {
            storeData = true;
            toStoreList.Clear();
            OnNewState?.Invoke(this, storeData);
        }

        public void Stop()
        {
            storeData = false;
            OnNewState?.Invoke(this, storeData);
        }

        /// <summary>
        /// Store to file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="list"></param>
        /// <param name="rxAntennas"></param>
        /// <param name="chirpsPerFrame"></param>
        /// <param name="samplesPerChirp"></param>
        private static void StoreToFile(string fileName, List<ushort[]> list, int rxAntennas, int chirpsPerFrame, int samplesPerChirp)
        {
            // Construct and store
            var data = np.zeros((list.Count,
                rxAntennas,
                chirpsPerFrame,
                samplesPerChirp));

            for (int frameIndex = 0; frameIndex < list.Count; frameIndex++)
            {
                for (int antennaIndex = 0; antennaIndex < rxAntennas; antennaIndex++)
                {
                    // Compute all the range FFTs
                    for (int chirpIdx = 0; chirpIdx < chirpsPerFrame; ++chirpIdx)
                    {
                        // Extract time signal of the chirp
                        int startIndex = chirpIdx * rxAntennas * samplesPerChirp;
                        for (int sampleIndex = 0; sampleIndex < samplesPerChirp; sampleIndex++)
                        {
                            int index = startIndex + sampleIndex * rxAntennas + antennaIndex;
                            data.SetDouble(list[frameIndex][index], [frameIndex, antennaIndex, chirpIdx, sampleIndex]);
                        }
                    }
                }
            }

            np.save(fileName, data);
        }

        public void LogToFile(string fileName)
        {
            if (radarConfiguration == null) return;

            // Store in a background task
            Task.Run(() => StoreToFile(fileName, 
                toStoreList, 
                radarConfiguration.rxAntennas, 
                radarConfiguration.chirpsPerFrame, 
                radarConfiguration.samplesPerChirp));
        }

        public void Store(ushort[] frame)
        {
            if (storeData == false) return;
            if (radarConfiguration == null) return;

            ushort[] tmp = new ushort[frame.Length];
            for (int i = 0; i < frame.Length; ++i)
            {
                tmp[i] = frame[i];
            }

            toStoreList.Add(tmp);

            OnNewRecordedCount?.Invoke(this, toStoreList.Count);
        }
    }
}
