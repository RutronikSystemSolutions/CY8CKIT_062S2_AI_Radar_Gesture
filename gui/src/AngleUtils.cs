using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    public class AngleUtils
    {
        /// <summary>
        /// Get difference between two angles
        /// Result is always between -pi and pi
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public static double GetAngleDiff(double a1, double a2)
        {
            double sign = -1;
            if (a1 > a2) sign = 1;

            double angle = a1 - a2;
            double k = -sign * Math.PI * 2;
            if (Math.Abs(k + angle) < Math.Abs(angle))
            {
                return k + angle;
            }
            return angle;
        }
    }
}
