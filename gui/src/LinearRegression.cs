using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class LinearRegression
    {
        public static void Compute(List<double> values, out double slope, out double intercept)
        {
            double xsum = 0;
            double ysum = 0;
            double xysum = 0;
            double xxsum = 0;
            double yysum = 0;
            double count = values.Count;

            // Default
            slope = 0;
            intercept = 0;

            if (values.Count < 2) return;

            for (int i = 0; i < values.Count; i++)
            {
                xsum += (double)i;
                ysum += values[i];
                xysum += (double)i * values[i];
                xxsum += (double)i * (double)i;
                yysum += values[i] * values[i];
            }

            double den = (count * xxsum - xsum * xsum);
            if (den == 0)
            {
                // Cannot divide by 0
                return;
            }

            slope = (count * xysum - xsum * ysum) / den;
            intercept = (ysum - slope * xsum) / count;
        }
    }
}
