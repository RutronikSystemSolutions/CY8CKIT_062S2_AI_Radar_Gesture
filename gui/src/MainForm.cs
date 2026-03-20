using NumSharp;
using System.IO.Ports;

namespace RadarSensorGesture
{
    public partial class MainForm : Form
    {
        private USBCommunication usbCommunication = new USBCommunication();
        private GestureProcessor gestureProcessor = new GestureProcessor();
        private GestureProcessorOptimized gestureProcessorOptimized = new GestureProcessorOptimized();
        private DataLogger dataLogger = new DataLogger();
        private GestureClassificator classificator = new GestureClassificator();

        private bool phaseMode = false;
        private bool showAzimuth = true;
        private bool showElevation = true;

        public MainForm()
        {
            InitializeComponent();

            usbCommunication.OnNewConnectionState += UsbCommunication_OnNewConnectionState;
            usbCommunication.OnNewFrame += UsbCommunication_OnNewFrame;
            usbCommunication.OnNewRadarConfiguration += UsbCommunication_OnNewRadarConfiguration;

            gestureProcessor.OnNewTimeSignal += GestureProcessor_OnNewTimeSignal;
            gestureProcessor.OnNewMagnitudeValue += GestureProcessor_OnNewMagnitudeValue;
            gestureProcessor.OnNewPoint += GestureProcessor_OnNewPoint;
            gestureProcessor.OnNewPhaseDiff += GestureProcessor_OnNewPhaseDiff;
            gestureProcessor.OnNewDistance += GestureProcessor_OnNewDistance;

            gestureProcessorOptimized.OnNewTimeSignal += GestureProcessor_OnNewTimeSignal;
            gestureProcessorOptimized.OnNewMagnitudeValue += GestureProcessor_OnNewMagnitudeValue;
            gestureProcessorOptimized.OnNewPhaseDiff += GestureProcessor_OnNewPhaseDiff;
            gestureProcessorOptimized.OnNewPoint += GestureProcessor_OnNewPoint;

            dataLogger.OnNewRecordedCount += DataLogger_OnNewRecordedCount;
            dataLogger.OnNewState += DataLogger_OnNewState;

            classificator.OnNewGesture += Classificator_OnNewGesture;
        }

        private void Classificator_OnNewGesture(object sender, Gesture gesture)
        {
            detectedActionView.SignalGesture(gesture);
        }

        private void DataLogger_OnNewState(object sender, bool state)
        {
            stopRecordingButton.Enabled = state;
            startRecordingButton.Enabled = !state;
            storeToFileButton.Enabled = !state;
        }

        private void GestureProcessor_OnNewDistance(object sender, int antennaIndex, double distance)
        {
            distanceOverTimeView.updateData(antennaIndex, distance);
        }

        private void DataLogger_OnNewRecordedCount(object sender, int count)
        {
            recordCountTextBox.Text = count.ToString();
        }

        private void GestureProcessor_OnNewPhaseDiff(object sender, double azimuth, double elevation)
        {
            phaseDifferenceView.UpdateData(azimuth, elevation);
            classificator.feed(azimuth, elevation);

            if (phaseMode)
                gestureView.AddPoint(azimuth, elevation, 1);
        }

        private void GestureProcessor_OnNewPoint(object sender, double x, double y, double magnitude)
        {
            if (phaseMode == false)
                gestureView.AddPoint(x, y, magnitude);
        }

        private void GestureProcessor_OnNewMagnitudeValue(object sender, int antennaIndex, double magnitude)
        {
            magnitudeOverTime.updateData(antennaIndex, magnitude);
        }

        private void GestureProcessor_OnNewTimeSignal(object sender, int antennaIndex, double[] signal)
        {
            rawSignalView.updateData(antennaIndex, signal);
        }

        private void UsbCommunication_OnNewRadarConfiguration(object sender, RadarConfiguration configuration)
        {
            gestureProcessor.SetRadarConfiguration(configuration);
            gestureProcessorOptimized.SetRadarConfiguration(configuration);
            rawSignalView.SetRadarConfiguration(configuration);
            magnitudeOverTime.SetRadarConfiguration(configuration);
            dataLogger.SetRadarConfiguration(configuration);
            distanceOverTimeView.SetRadarConfiguration(configuration);
        }

        /// <summary>
        /// Event: new radar frame is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="frame"></param>
        private void UsbCommunication_OnNewFrame(object sender, ushort[] frame)
        {
            gestureProcessor.ProcessFrame(frame);
            //gestureProcessorOptimized.ProcessFrame(frame);
            dataLogger.Store(frame);
        }

        /// <summary>
        /// Event: new connection state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void UsbCommunication_OnNewConnectionState(object sender, USBCommunication.ConnectionState state)
        {
            switch (state)
            {
                case USBCommunication.ConnectionState.Connected:
                    connectDisconnectButton.Text = "Disconnect";
                    comPortStatusTextBox.Text = "Connected";
                    comPortStatusTextBox.BackColor = Color.Green;
                    comPortComboBox.Enabled = false;
                    break;
                case USBCommunication.ConnectionState.Iddle:
                    connectDisconnectButton.Text = "Connect";
                    comPortStatusTextBox.Text = "Iddle";
                    comPortStatusTextBox.BackColor = Color.LightGray;
                    comPortComboBox.Enabled = true;
                    break;
                case USBCommunication.ConnectionState.Error:
                    connectDisconnectButton.Text = "Connect";
                    comPortStatusTextBox.Text = "Error";
                    comPortStatusTextBox.BackColor = Color.Red;
                    comPortComboBox.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Event handler: happens once at first load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the possible com ports
            string[] serialPorts = SerialPort.GetPortNames();
            comPortComboBox.DataSource = serialPorts;
        }

        /// <summary>
        /// User click on the "Connect" /"Disconnect" button, the connection state will be changed accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectDisconnectButton_Click(object sender, EventArgs e)
        {
            if (usbCommunication.IsConnected())
            {
                // Disconnect
                usbCommunication.Disconnect();
            }
            else
            {
                if ((comPortComboBox.SelectedIndex < 0) || (comPortComboBox.SelectedIndex >= comPortComboBox.Items.Count)) return;

                var selectedItem = comPortComboBox.Items[comPortComboBox.SelectedIndex];
                if (selectedItem != null)
                {
                    string? portName = selectedItem.ToString();
                    if (portName != null) usbCommunication.SetPortName(portName);
                }
            }
        }

        private void startRecordingButton_Click(object sender, EventArgs e)
        {
            dataLogger.Start();
        }

        private void stopRecordingButton_Click(object sender, EventArgs e)
        {
            dataLogger.Stop();
        }

        private void storeToFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Numpy file (.npy)|*.npy";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dataLogger.LogToFile(saveFileDialog.FileName);
        }

        private void rawValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rawValuesToolStripMenuItem.Checked = !rawValuesToolStripMenuItem.Checked;
            mainSplitContainer.Panel1Collapsed = !rawValuesToolStripMenuItem.Checked;
        }

        private void phaseModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            phaseModeToolStripMenuItem.Checked = !phaseModeToolStripMenuItem.Checked;
            phaseMode = phaseModeToolStripMenuItem.Checked;
            if (phaseMode)
            {
                gestureView.SetPhaseMode();
            }
            else
            {
                gestureView.SetMMode();
            }
        }

        private void azimuthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            azimuthToolStripMenuItem.Checked = !azimuthToolStripMenuItem.Checked;
            showAzimuth = azimuthToolStripMenuItem.Checked;

            phaseDifferenceView.SetDisplayMode(showAzimuth, showElevation);
        }

        private void elevationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elevationToolStripMenuItem.Checked = !elevationToolStripMenuItem.Checked;
            showElevation = elevationToolStripMenuItem.Checked;

            phaseDifferenceView.SetDisplayMode(showAzimuth, showElevation);
        }

        private void holdonLongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            holdonLongToolStripMenuItem.Checked = !holdonLongToolStripMenuItem.Checked;
            if (holdonLongToolStripMenuItem.Checked)
            {
                distanceOverTimeView.SetHoldTimeMs(2000);
                phaseDifferenceView.SetHoldTimeMs(2000);
            }
            else
            {
                distanceOverTimeView.SetHoldTimeMs(100);
                phaseDifferenceView.SetHoldTimeMs(100);
            }
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdConfigurationForm form = new ThresholdConfigurationForm(gestureProcessor);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void distanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetectionDistanceForm form = new DetectionDistanceForm(gestureProcessor);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
    }
}
