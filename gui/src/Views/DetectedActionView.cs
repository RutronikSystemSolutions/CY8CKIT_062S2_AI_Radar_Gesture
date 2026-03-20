using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RadarSensorGesture.Views
{
    public partial class DetectedActionView : UserControl
    {
        private System.Timers.Timer timer = new System.Timers.Timer();

        public DetectedActionView()
        {
            InitializeComponent();

            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            clickCenterButton.BackColor = Color.LightGray;
            clickDownButton.BackColor = Color.LightGray;
            clickLeftButton.BackColor = Color.LightGray;
            clickRightButton.BackColor = Color.LightGray;
            clickTopButton.BackColor = Color.LightGray;

            timer.Stop();
        }

        public void SignalGesture(Gesture gesture)
        {
            clickCenterButton.BackColor = (gesture == Gesture.MiddleClick) ? Color.Green : Color.LightGray;
            clickDownButton.BackColor = (gesture == Gesture.BotClick) ? Color.Green : Color.LightGray;
            clickLeftButton.BackColor = (gesture == Gesture.LeftClick) ? Color.Green : Color.LightGray;
            clickRightButton.BackColor = (gesture == Gesture.RightClick) ? Color.Green : Color.LightGray;
            clickTopButton.BackColor = (gesture == Gesture.TopClick) ? Color.Green : Color.LightGray;

            timer.Stop();
            timer.Start();
        }
    }
}
