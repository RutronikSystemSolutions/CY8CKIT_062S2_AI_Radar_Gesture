using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class AverageComputation
    {
        public static double Compute(List<double> values, bool removeMinMax)
        {
            double sum = 0;
            double minValue = values[0];
            double maxValue = values[0];
            double dividor = values.Count;

            for(int i = 0; i < values.Count; ++i)
            {
                sum += values[i];
                if (values[i] < minValue) minValue = values[i];
                if (values[i] > maxValue) maxValue = values[i];
            }

            if (removeMinMax)
            {
                sum -= (minValue +  maxValue);
                dividor -= 2;
            }

            return sum / dividor;
        }
    }
}
