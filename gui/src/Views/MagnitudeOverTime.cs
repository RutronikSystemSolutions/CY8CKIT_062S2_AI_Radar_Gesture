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
    public partial class MagnitudeOverTime : UserControl
    {
        private bool newData = false;

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
            Maximum = 0.2,
            Unit = "Magnitude",
            Key = "Amp",
        };

        private List<LineSeries> lines = new List<LineSeries>();
        private System.Timers.Timer timer = new System.Timers.Timer();

        public MagnitudeOverTime()
        {
            InitializeComponent();
            InitPlot();
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (newData)
            {
                newData = false;
                plotView.InvalidatePlot(true);
            }
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
                };
                lines.Add(line);
                plotView.Model.Series.Add(line);
            }

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

        public void updateData(int antennaIndex, double magnitude)
        {
            if (antennaIndex < 0 || antennaIndex >= lines.Count)
                return;

            newData = true;

            double dateTimeDouble = DateTimeAxis.ToDouble(DateTime.Now);
            lines[antennaIndex].Points.Add(new DataPoint(dateTimeDouble, magnitude));
            if (lines[antennaIndex].Points.Count > 500)
            {
                lines[antennaIndex].Points.RemoveAt(0);
            }
        }
    }
}
