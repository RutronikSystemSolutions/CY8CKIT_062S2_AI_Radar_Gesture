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
    public partial class RawSignalView : UserControl
    {
        private bool newData = false;

        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Sample index"
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
            Minimum = 0,
            Maximum = 4095,
            Unit = "ADC tick",
            Key = "Amp",
        };

        private List<LineSeries> lines = new List<LineSeries>();
        private System.Timers.Timer timer = new System.Timers.Timer();

        public RawSignalView()
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
            for(int i = 0; i < configuration.rxAntennas; ++i)
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
            // Raw signals plot
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxis);
            timeModel.Axes.Add(yAxis);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        public void updateData(int antennaIndex, double[] signal)
        {
            if (antennaIndex < 0 || antennaIndex >= lines.Count)
                return;

            newData = true;

            lines[antennaIndex].Points.Clear();
            for(int i =0; i < signal.Length; ++i)
            {
                lines[antennaIndex].Points.Add(new DataPoint(i, signal[i]));
            }
        }
    }
}
