using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    internal class ArrayUtils
    {
        public static void scaleInPlace(double[] array, double factor)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= factor;
            }
        }

        public static double getAverage(double[] array)
        {
            double sum = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                sum += array[i];
            }
            return sum / array.Length;
        }

        public static System.Numerics.Complex getAverage(System.Numerics.Complex[] array)
        {
            System.Numerics.Complex sum = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                sum += array[i];
            }
            return sum / array.Length;
        }

        public static void offsetInPlace(double[] array, double offset)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] += offset;
            }
        }

        public static void offsetInPlace(System.Numerics.Complex[] array, System.Numerics.Complex offset)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] += offset;
            }
        }

        public static void getBiggestOfMatrix(System.Numerics.Complex[,] matrix, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;
            double max = matrix[0, 0].Magnitude;
            int rowCount = matrix.GetLength(0);
            int coloCount = matrix.GetLength(1);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < coloCount; col++)
                {
                    if (matrix[row, col].Magnitude > max)
                    {
                        rowIndex = row;
                        colIndex = col;
                        max = matrix[row, col].Magnitude;
                    }
                }
            }
        }

        public static System.Numerics.Complex[] FftShift(System.Numerics.Complex[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            System.Numerics.Complex[] values2 = new System.Numerics.Complex[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }
    }
}
