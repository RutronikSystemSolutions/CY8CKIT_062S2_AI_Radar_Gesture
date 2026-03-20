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
    public partial class GestureView : UserControl
    {
        private const double maxColorValue = 100;
        private const double memoryDurationMs = 1000;



        private class DataWithTimeStamp
        {
            public double x;
            public double y;
            public double magnitude;
            public double timestamp;

            public DataWithTimeStamp(double x, double y, double magnitude)
            {
                this.x = x;
                this.y = y;
                this.magnitude = magnitude;
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        private List<DataWithTimeStamp> data = new List<DataWithTimeStamp>();
        private System.Timers.Timer timer = new System.Timers.Timer();

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
            Minimum = -0.3,
            Maximum = 0.3,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "X"
        };

        /// <summary>
        /// Y Axis
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
            Minimum = -0.3,
            Maximum = 0.3,
            Unit = "Y"
        };

        private ScatterSeries detectedPresenceSeries = new ScatterSeries();

        public GestureView()
        {
            InitializeComponent();
            InitPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void SetPhaseMode()
        {
            xAxis.Maximum = Math.PI;
            xAxis.Minimum = -Math.PI;
            yAxis.Maximum = Math.PI;
            yAxis.Minimum = -Math.PI;
        }

        public void SetMMode()
        {
            xAxis.Maximum = 0.3;
            xAxis.Minimum = -0.3;
            yAxis.Maximum = 0.3;
            yAxis.Minimum = -0.3;
        }

        

        public void AddPoint(double x, double y, double magnitude)
        {
            data.Add(new DataWithTimeStamp(x, y, magnitude));
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Only show the last x seconds
            double timeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            double deadline = timeMs - memoryDurationMs;

            // First delete (points are sorted)
            int deleteCount = data.Count;
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i].timestamp > deadline)
                {
                    deleteCount = i;
                    break;
                }
            }
            data.RemoveRange(0, deleteCount);

            detectedPresenceSeries.Points.Clear();
            for (int i = 0; i < data.Count; ++i)
            {
                //detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: data[i].range));
                //detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: data.Count - i));

                // 0 -> 0ms
                // maxColorValue -> memoryDurationMs

                double zvalue = memoryDurationMs - (data[i].timestamp - deadline);
                zvalue = (zvalue * maxColorValue) / memoryDurationMs;

                detectedPresenceSeries.Points.Add(new ScatterPoint(x: data[i].x, y: data[i].y, value: zvalue));
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

            var axis1 = new LinearColorAxis();
            axis1.Key = "ColorAxis";
            axis1.Maximum = maxColorValue;
            axis1.Minimum = 0;
            axis1.Position = AxisPosition.Top;
            timeModel.Axes.Add(axis1);

            // Add series
            detectedPresenceSeries.Title = "Presence";
            detectedPresenceSeries.ColorAxisKey = "ColorAxis";

            timeModel.Series.Add(detectedPresenceSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }
    }
}
