using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class GestureClassificator
    {
        public delegate void OnNewGestureEventHandler(object sender, Gesture gesture);
        public event OnNewGestureEventHandler? OnNewGesture;

        private System.Timers.Timer timer = new System.Timers.Timer();
        private List<double> azimuths = new List<double>();
        private List<double> elevations = new List<double>();

        private int MINIMUM_VALUE_COUNT = 7;

        public GestureClassificator()
        {
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Count = {0}", azimuths.Count));

            if (azimuths.Count > MINIMUM_VALUE_COUNT)
            {
                // Compute linear regression
                double azimuthSlope = 0;
                double azimuthIntercept = 0;
                double elevationSlope = 0;
                double elevationIntercept = 0;
                
                LinearRegression.Compute(azimuths, out azimuthSlope, out azimuthIntercept);
                LinearRegression.Compute(elevations, out elevationSlope, out elevationIntercept);

                if (Math.Abs(azimuthSlope) < 0.2 && Math.Abs(elevationSlope) < 0.2)
                {
                    System.Diagnostics.Debug.WriteLine("Click");

                    double elevationAvg = AverageComputation.Compute(elevations, true);
                    double azimuthAvg = AverageComputation.Compute(azimuths, true);

                    if (azimuthAvg < -1)
                    {
                        OnNewGesture?.Invoke(this, Gesture.LeftClick);
                    }
                    else if (azimuthAvg > 1)
                    {
                        OnNewGesture?.Invoke(this, Gesture.RightClick);
                    }
                    else
                    {
                        if (elevationAvg < -0.5)
                        {
                            OnNewGesture?.Invoke(this, Gesture.BotClick);
                        }
                        else if (elevationAvg > 0.5)
                        {
                            OnNewGesture?.Invoke(this, Gesture.TopClick);
                        }
                        else
                        {
                            OnNewGesture?.Invoke(this, Gesture.MiddleClick);
                        }
                    }

                }
            }

            timer.Stop();
            azimuths.Clear();
            elevations.Clear();
        }

        public void feed(double azimuth, double elevation)
        {
            azimuths.Add(azimuth);
            elevations.Add(elevation);

            // Restart the timer
            timer.Stop();
            timer.Start();
        }
    }
}
