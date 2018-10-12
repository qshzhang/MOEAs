using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems.WFG
{
    public class Shapes
    {
        /**
	   * Calculate a linear shape
	   */
        public double linear(double[] x, int m)
        {
            double result = (double)1.0;
            int M = x.Length;

            for (int i = 1; i <= M - m; i++)
                result *= x[i - 1];

            if (m != 1)
                result *= (1 - x[M - m]);

            return result;
        } // linear

        /**
         * Calculate a convex shape
         */
        public double convex(double[] x, int m)
        {
            double result = (double)1.0;
            int M = x.Length;

            for (int i = 1; i <= M - m; i++)
                result *= (1 - Math.Cos(x[i - 1] * Math.PI * 0.5));

            if (m != 1)
                result *= (1 - Math.Sin(x[M - m] * Math.PI * 0.5));


            return result;
        } // convex

        /**
         * Calculate a concave shape
         */
        public double concave(double[] x, int m)
        {
            double result = (double)1.0;
            int M = x.Length;

            for (int i = 1; i <= M - m; i++)
                result *= Math.Sin(x[i - 1] * Math.PI * 0.5);

            if (m != 1)
                result *= Math.Cos(x[M - m] * Math.PI * 0.5);

            return result;
        } // concave

        /**
         * Calculate a mixed shape
         */
        public double mixed(double[] x, int A, double alpha)
        {
            double tmp;
            tmp = (double)Math.Cos((double)2.0 * A * (double)Math.PI * x[0] + (double)Math.PI * (double)0.5);
            tmp /= (2.0 * (double)A * Math.PI);

            return (double)Math.Pow(((double)1.0 - x[0] - tmp), alpha);
        } // mixed

        /**
         *  Calculate a disc shape
         */
        public double disc(double[] x, int A, double alpha, double beta)
        {
            double tmp;
            tmp = (double)Math.Cos((double)A * Math.Pow(x[0], beta) * Math.PI);

            return (double)1.0 - (double)Math.Pow(x[0], alpha) * (double)Math.Pow(tmp, 2.0);
        } // disc
    }
}
