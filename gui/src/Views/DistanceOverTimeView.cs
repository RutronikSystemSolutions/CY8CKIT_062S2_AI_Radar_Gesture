using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RadarSensorGesture.Views
{
    public partial class DistanceOverTimeView : UserControl
    {
        private double lastPointTimeMs = 0;
        private bool clearByNext = false;
        private bool newData = false;
        private int holdTimeMs = 2000;

        DateTimeAxis dateTimeAxis = new DateTimeAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            StringFormat = "HH:mm:ss",
            Title = "Time",
        };

        /// <summary>
        /// Y Axis for amplitude
        /// </summary>
        private LinearAxis yAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Distance",
            Key = "Amp",
            Minimum = 0,
            // Maximum = 1,
        };

        private System.Timers.Timer timer = new System.Timers.Timer();
        private List<LineSeries> lines = new List<LineSeries>();

        public DistanceOverTimeView()
        {
            InitializeComponent();
            InitPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void SetRadarConfiguration(RadarConfiguration configuration)
        {
            plotView.Model.Series.Clear();
            lines.Clear();
            for (int i = 0; i < configuration.rxAntennas; ++i)
            {
                var line = new LineSeries
                {
                    Title = $"Antenna {i}",
                    YAxisKey = yAxis.Key,
                    LineStyle = LineStyle.None,
                    MarkerType = MarkerType.Circle
                };
                lines.Add(line);
                plotView.Model.Series.Add(line);
            }

            // TODO: compute max range

            plotView.InvalidatePlot(true);
        }

        private void InitPlot()
        {
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(dateTimeAxis);
            timeModel.Axes.Add(yAxis);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }
        public void SetHoldTimeMs(int ms)
        {
            holdTimeMs = ms;
            if (holdTimeMs <= 500)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].LineStyle = LineStyle.Solid;
                }
            }
            else
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].LineStyle = LineStyle.None;
                }
            }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            double timeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (timeMs - lastPointTimeMs > holdTimeMs)
            {
                clearByNext = true;
            }

            if (newData)
            {
                newData = false;
                plotView.InvalidatePlot(true);
            }
        }

        public void updateData(int antennaIndex, double distance)
        {
            if (antennaIndex < 0 || antennaIndex >= lines.Count)
                return;

            if (clearByNext)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].Points.Clear();
                }
                clearByNext = false;
                plotView.InvalidatePlot(true);
            }

            double dateTimeDouble = DateTimeAxis.ToDouble(DateTime.Now);
            lastPointTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            lines[antennaIndex].Points.Add(new DataPoint(dateTimeDouble, distance));
            if (lines[antennaIndex].Points.Count > 500)
            {
                lines[antennaIndex].Points.RemoveAt(0);
            }

            newData = true;
        }
    }
}
