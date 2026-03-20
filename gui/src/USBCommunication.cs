using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;

namespace RadarSensorGesture
{
    internal class USBCommunication
    {
        public enum ConnectionState
        {
            Iddle,
            Connected,
            Error
        }

        #region "Constants"

        private const int WorkerReportFrame = 1;
        private const int WorkerReportRadarConfiguration = 2;

        #endregion

        #region "Events"

        public delegate void OnNewConnectionStateEventHandler(object sender, ConnectionState state);
        public event OnNewConnectionStateEventHandler? OnNewConnectionState;

        public delegate void OnNewFrameEventHandler(object sender, ushort[] frame);
        public event OnNewFrameEventHandler? OnNewFrame;

        public delegate void OnNewRadarConfigurationEventHandler(object sender, RadarConfiguration configuration);
        public event OnNewRadarConfigurationEventHandler? OnNewRadarConfiguration;

        #endregion

        #region "Members"

        /// <summary>
        /// Serial port used for the communication
        /// </summary>
        private SerialPort? port;

        /// <summary>
        /// Background worker enabling background operations
        /// </summary>
        private BackgroundWorker? worker;

        private CRC8 crc8 = new CRC8();

        private bool connected = false;

        #endregion

        /// <summary>
        /// Set the serial port name
        /// </summary>
        /// <param name="portName"></param>
        public void SetPortName(string portName)
        {
            try
            {
                port = new SerialPort
                {
                    BaudRate = 921600,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    Parity = Parity.None,
                    PortName = portName,
                    StopBits = StopBits.One,
                    ReadTimeout = 500,
                    WriteTimeout = 2000
                };
                port.Open();
            }
            catch (Exception)
            {
                OnNewConnectionState?.Invoke(this, ConnectionState.Error);
                return;
            }

            OnNewConnectionState?.Invoke(this, ConnectionState.Connected);
            connected = true;

            CreateAndStartBackgroundWorker();
        }

        public bool IsConnected()
        {
            return connected;
        }

        public void Disconnect()
        {
            if (worker == null) return;
            worker.CancelAsync();
        }

        private void CreateAndStartBackgroundWorker()
        {
            if (worker != null)
            {
                return;
            }

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (worker == null) return;

            worker.DoWork -= Worker_DoWork;
            worker.ProgressChanged -= Worker_ProgressChanged;
            worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
            worker = null;

            OnNewConnectionState?.Invoke(this, ConnectionState.Iddle);
            connected = false;
        }

        private int ReadFromPort(byte[] readBuffer, int bytesToRead)
        {
            int offset = 0;
            int remaining = bytesToRead;
            bool gotTimeout = false;

            if (port == null) return -1;

            for (; ; )
            {
                int readSize = 0;
                try
                {
                    readSize = port.Read(readBuffer, offset, remaining);
                }
                catch (System.TimeoutException)
                {
                    gotTimeout = true;
                    break;
                }
                offset += readSize;
                remaining -= readSize;
                if (remaining == 0) break;
            }

            if (gotTimeout)
            {
                return -2;
            }

            return 0;
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender == null) return;
            if (port == null) return;

            BackgroundWorker worker = (BackgroundWorker)sender;
            port.ReadExisting();

            const byte START_COMMAND = 49;
            const byte STOP_COMMAND = 50;
            const byte GET_RADAR_CONF_COMMAND = 51;

            for (; ; )
            {
                if (worker.CancellationPending)
                {
                    port.Write([STOP_COMMAND], 0, 1);
                    try
                    {
                        port.Close();
                    }
                    catch (Exception) { }
                    return;
                }

                // Empty buffer
                port.ReadExisting();
                port.Write([STOP_COMMAND], 0, 1);
                Thread.Sleep(200);
                port.ReadExisting();

                // Get radar configuration
                port.Write([GET_RADAR_CONF_COMMAND], 0, 1);

                // Wait until 34 bytes are received
                for (; ; )
                {
                    int available = port.BytesToRead;
                    if (available >= (RadarConfiguration.RADAR_CONFIGURATION_SIZE + 1)) break;
                    Thread.Sleep(10);

                    // TODO add timeout here
                }

                byte[] radarConfigurationBytes = new byte[RadarConfiguration.RADAR_CONFIGURATION_SIZE+1];
                if (ReadFromPort(radarConfigurationBytes, radarConfigurationBytes.Length) != 0)
                {
                    System.Diagnostics.Debug.WriteLine("Error reading radar configuration");
                    continue;
                }

                // Check CRC
                byte computedCrc = crc8.Crc(radarConfigurationBytes, 0, RadarConfiguration.RADAR_CONFIGURATION_SIZE);
                if (computedCrc != radarConfigurationBytes[RadarConfiguration.RADAR_CONFIGURATION_SIZE])
                {
                    // CRC error, discard the frame
                    System.Diagnostics.Debug.WriteLine("CRC error, radar configuration is wrong");
                    continue;
                }

                RadarConfiguration radarConfiguration = new RadarConfiguration(radarConfigurationBytes);
                radarConfiguration.PrintToDiag();

                worker.ReportProgress(WorkerReportRadarConfiguration, radarConfiguration);

                // +1 because last byte contains CRC over the data
                int bytesPerFrame = radarConfiguration.GetFrameSizeInBytes() + 1;
                byte[] readBuffer = new byte[bytesPerFrame];

                port.Write([START_COMMAND], 0, 1);

                for (; ; )
                {
                    if (worker.CancellationPending)
                    {
                        port.Write([STOP_COMMAND], 0, 1);
                        Thread.Sleep(200);
                        port.ReadExisting();
                        try
                        {
                            port.Close();
                        }
                        catch (Exception) { }
                        return;
                    }

                    if (ReadFromPort(readBuffer, bytesPerFrame) != 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Error reading radar frame");
                        break;
                    }

                    // Check the CRC
                    byte computed_crc = crc8.Crc(readBuffer, 0, bytesPerFrame - 1);
                    if (computed_crc != readBuffer[bytesPerFrame - 1])
                    {
                        // CRC error, discard the frame
                        System.Diagnostics.Debug.WriteLine("CRC error, discarding frame");
                        break;
                    }

                    Thread.Sleep(1);

                    // Frame is available
                    int samplesPerFrame = radarConfiguration.GetSamplesPerFrame();
                    var samples = new ushort[samplesPerFrame];
                    for (int i = 0; i < samplesPerFrame; i++)
                    {
                        samples[i] = BitConverter.ToUInt16(readBuffer, i * 2);
                    }

                    worker.ReportProgress(WorkerReportFrame, samples);
                }
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case WorkerReportFrame:
                    ushort[]? samples = e.UserState as ushort[];
                    if (samples != null)
                    {
                        OnNewFrame?.Invoke(this, samples);
                    }
                    break;
                case WorkerReportRadarConfiguration:
                    RadarConfiguration? radarConfiguration = e.UserState as RadarConfiguration;
                    if (radarConfiguration != null)
                    {
                        OnNewRadarConfiguration?.Invoke(this, radarConfiguration);
                    }
                    break;
            }
        }
    }
}
