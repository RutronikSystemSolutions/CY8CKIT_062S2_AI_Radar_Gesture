using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RadarSensorGesture
{
    public partial class DetectionDistanceForm : Form
    {
        private GestureProcessor? gestureProcessor;

        public DetectionDistanceForm()
        {
            InitializeComponent();
        }

        public DetectionDistanceForm(GestureProcessor processor)
        {
            InitializeComponent();
            gestureProcessor = processor;

            maxDistanceTextBox.Text = gestureProcessor.maximumDistance.ToString();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (gestureProcessor == null) return;

            double distance = 0;

            try
            {
                distance = Double.Parse(maxDistanceTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message));
                return;
            }

            gestureProcessor.maximumDistance = distance;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
