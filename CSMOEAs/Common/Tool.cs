using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class Tool
    {
        public static double GetAngle(double[] v1, double[] v2)
        {
            double mul = 0.0;
            double a = 0.0;
            double b = 0.0;
            for (int i = 0; i < v1.Length; i++)
            {
                mul += v1[i] * v2[i];
                a += Math.Pow(v1[i], 2);
                b += Math.Pow(v2[i], 2);
            }
            return Math.Acos(mul / (Math.Sqrt(a * b)));
        }

        public static double ArrayMax(double[] arr)
        {
            double max = Double.MinValue;
            for(int i = 0;i < arr.Length; i++)
            {
                if (max < arr[i]) max = arr[i];
            }
            return max;
        }

        public static double ArraySum(double[] arr)
        {
            double sum = 0.0;
            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];
            }
            return sum;
        }

        public static double VectorLen(double[] arr)
        {
            double sum = 0.0;
            foreach(double e in arr)
            {
                sum += Math.Pow(e, 2);
            }
            return Math.Sqrt(sum);
        }

        public static double MaxArray(List<double[]> list, int obj)
        {
            double max = Double.MinValue;
            foreach(double[] arr in list)
            {
                if(max < arr[obj])
                {
                    max = arr[obj];
                }
            }
            return max;
        }

        public static double MinArray(List<double[]> list, int obj)
        {
            double min = Double.MaxValue;
            foreach (double[] arr in list)
            {
                if (min > arr[obj])
                {
                    min = arr[obj];
                }
            }
            return min;
        }

        public static Boolean IsSatisfy(double[] arr)
        {
            Boolean flag = true;
            foreach(double e in arr)
            {
                if (e <= 1e-5 || Double.IsNaN(e)) return false;
            }
            return flag;
        }
    }
}
