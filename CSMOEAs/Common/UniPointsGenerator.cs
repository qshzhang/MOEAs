using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class UniPointsGenerator
    {
        public static List<double[]> getMUniDistributedPoint(int m, int H)
        {
            int[] buf = new int[m];
            for (int i = 0; i < m; i++) buf[i] = 0;

            List<double[]> result = new List<double[]>();
            while (true)
            {

                double[] arr = new double[m];

                for (int r = 1; r != m; ++r)
                {
                    arr[r - 1] = (buf[r] - buf[r - 1]) / (double)H;
                }
                arr[m - 1] = (H - buf[m - 1]) / (double)H;
                result.Add(arr);

                int p;
                for (p = m - 1; p != 0 && buf[p] == H; --p) ;
                if (p == 0) break;
                buf[p]++;
                for (++p; p != m; ++p)
                {
                    buf[p] = buf[p - 1];
                }
            }
            return result;
        }

        public static List<double[]> getMaUniDistributedPoint(int m, int p, int level)
        {
            //p < m
            List<double[]> result = new List<double[]>();
            result = getMUniDistributedPoint(m, p);

            List<double[]> temp = new List<double[]>();
            temp = getMUniDistributedPoint(m, p - 1);

            for (int i = 0; i < temp.Count(); i++)
            {
                for (int j = 0; j < m; j++)
                {
                    temp[i][j] = 0.5 / m + 0.5 * temp[i][j];
                }
            }
            result.AddRange(temp);
            return result;
        }
    }
}
