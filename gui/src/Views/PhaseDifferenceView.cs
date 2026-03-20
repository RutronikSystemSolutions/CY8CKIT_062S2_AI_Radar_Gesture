using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace RadarSensorGesture.Views
{
    public partial class PhaseDifferenceView : UserControl
    {
        private double lastPointTimeMs = 0;
        private bool clearByNext = false;
        private bool newData = false;

        private bool showElevation = true;
        private bool showAzimuth = true;

        private int holdTimeMs = 2000;

        DateTimeAxis dateTimeAxis = new DateTimeAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = false,
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
            Unit = "PhaseDifference",
            Key = "Amp",
            Minimum = -Math.PI,
            Maximum = Math.PI,
        };

        private LineSeries azimuthSerie = new LineSeries
        {
            Title = "Azimuth",
            Color = OxyColors.Red,
            LineStyle = LineStyle.None,
            MarkerType = MarkerType.Circle
        };

        private LineSeries elevationSerie = new LineSeries
        {
            Title = "Elevation",
            Color = OxyColors.Green,
            LineStyle = LineStyle.None,
            MarkerType = MarkerType.Circle
        };

        private System.Timers.Timer timer = new System.Timers.Timer();

        public PhaseDifferenceView()
        {
            InitializeComponent();
            InitPlot();

            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void SetHoldTimeMs(int ms)
        {
            holdTimeMs = ms;
            if (holdTimeMs <= 500)
            {
                azimuthSerie.LineStyle = LineStyle.Solid;
                elevationSerie.LineStyle = LineStyle.Solid;
            }
            else
            {
                azimuthSerie.LineStyle = LineStyle.None;
                elevationSerie.LineStyle = LineStyle.None;
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

            timeModel.Series.Add(azimuthSerie);
            timeModel.Series.Add(elevationSerie);


            Legend legend = new Legend();
            legend.LegendPosition = LegendPosition.TopRight;
            legend.LegendOrientation = LegendOrientation.Vertical;
            legend.LegendPlacement = LegendPlacement.Inside;
            legend.LegendSymbolPlacement = LegendSymbolPlacement.Left;
            timeModel.Legends.Add(legend);


            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        public void SetDisplayMode(bool azimuth, bool elevation)
        {
            showAzimuth = azimuth;
            showElevation = elevation;

            azimuthSerie.Points.Clear();
            elevationSerie.Points.Clear();
            clearByNext = false;
            plotView.InvalidatePlot(true);
        }

        public void UpdateData(double azimuth, double elevation)
        {
            if (clearByNext)
            {
                azimuthSerie.Points.Clear();
                elevationSerie.Points.Clear();
                clearByNext = false;
                plotView.InvalidatePlot(true);
            }

            double dateTimeDouble = DateTimeAxis.ToDouble(DateTime.Now);
            lastPointTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (showAzimuth)
            {
                azimuthSerie.Points.Add(new DataPoint(dateTimeDouble, azimuth));
            }
            if (showElevation)
            {
                elevationSerie.Points.Add(new DataPoint(dateTimeDouble, elevation));
            }

            if (azimuthSerie.Points.Count > 500)
            {
                azimuthSerie.Points.RemoveAt(0);
            }

            if (elevationSerie.Points.Count > 500)
            {
                elevationSerie.Points.RemoveAt(0);
            }

            newData = true;
        }
    }
}
