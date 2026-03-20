using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RadarSensorGesture
{
    public partial class ThresholdConfigurationForm : Form
    {
        private GestureProcessor? gestureProcessor;

        public ThresholdConfigurationForm()
        {
            InitializeComponent();
        }

        public ThresholdConfigurationForm(GestureProcessor processor)
        {
            InitializeComponent();

            gestureProcessor = processor;

            thresholdBotTextBox.Text = gestureProcessor.thresholdBot.ToString();
            thresholdTopTextBox.Text = gestureProcessor.thresholdTop.ToString();
        }

        private void ThresholdConfigurationForm_Load(object sender, EventArgs e)
        {

        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (gestureProcessor == null) return;

            double thresholdTop = 0;
            double thresholdBot = 0;
            try
            {
                thresholdTop = Double.Parse(thresholdTopTextBox.Text);
                thresholdBot = Double.Parse(thresholdBotTextBox.Text);

                if (thresholdBot > thresholdTop)
                {
                    throw new Exception("Threshold bot cannot be bigger as threshold top.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message));
                return;
            }

            gestureProcessor.thresholdTop = thresholdTop;
            gestureProcessor.thresholdBot = thresholdBot;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
