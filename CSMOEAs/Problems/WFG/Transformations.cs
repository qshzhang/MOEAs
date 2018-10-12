using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems.WFG
{
    public class Transformations
    {
        /**
	   * Stores a default epsilon value 
	   */
        private static double epsilon = (double)1.0e-10;

        /**
         * b_poly transformation
         * @throws JMException 
         */
        public double b_poly(double y, double alpha)
        {

            return correct_to_01((double)Math.Pow(y, alpha));
        } //b_poly


        /**
         * b_flat transformation
         */
        public double b_flat(double y, double A, double B, double C)
        {
            double tmp1 = Math.Min((double)0, (double)Math.Floor(y - B)) * A * (B - y) / B;
            double tmp2 = Math.Min((double)0, (double)Math.Floor(C - y)) * (1 - A) * (y - C) / (1 - C);

            return correct_to_01(A + tmp1 - tmp2);
        }  // b_flat      

        /**
         * s_linear transformation
         */
        public double s_linear(double y, double A)
        {
            return correct_to_01(Math.Abs(y - A) / (double)Math.Abs(Math.Floor(A - y) + A));
        } // s_linear

        /**
         * s_decept transformation
         */
        public double s_decept(double y, double A, double B, double C)
        {
            double tmp, tmp1, tmp2;

            tmp1 = (double)Math.Floor(y - A + B) * ((double)1.0 - C + (A - B) / B) / (A - B);
            tmp2 = (double)Math.Floor(A + B - y) * ((double)1.0 - C + ((double)1.0 - A - B) / B) / ((double)1.0 - A - B);

            tmp = Math.Abs(y - A) - B;

            return correct_to_01((double)1 + tmp * (tmp1 + tmp2 + (double)1.0 / B));
        } // s_decept

        /**
         * s_multi transformation
         */
        public double s_multi(double y, int A, int B, double C)
        {
            double tmp1, tmp2;

            tmp1 = ((double)4.0 * A + (double)2.0) *
                   (double)Math.PI *
                   ((double)0.5 - Math.Abs(y - C) / ((double)2.0 * ((double)Math.Floor(C - y) + C)));
            tmp2 = (double)4.0 * B *
                   (double)Math.Pow(Math.Abs(y - C) / ((double)2.0 * ((double)Math.Floor(C - y) + C))
                                         , (double)2.0);

            return correct_to_01(((double)1.0 + (double)Math.Cos(tmp1) + tmp2) / (B + (double)2.0));
        } //s_multi

        /**
         * r_sum transformation
         */
        public double r_sum(double[] y, double[] w)
        {
            double tmp1 = (double)0.0, tmp2 = (double)0.0;
            for (int i = 0; i < y.Length; i++)
            {
                tmp1 += y[i] * w[i];
                tmp2 += w[i];
            }

            return correct_to_01(tmp1 / tmp2);
        } // r_sum  

        /**
         * r_nonsep transformation
         */
        public double r_nonsep(double[] y, int A)
        {
            double tmp, denominator, numerator;

            tmp = (double)Math.Ceiling(A / (double)2.0);
            denominator = y.Length * tmp * ((double)1.0 + (double)2.0 * A - (double)2.0 * tmp) / A;
            numerator = (double)0.0;
            for (int j = 0; j < y.Length; j++)
            {
                numerator += y[j];
                for (int k = 0; k <= A - 2; k++)
                {
                    numerator += Math.Abs(y[j] - y[(j + k + 1) % y.Length]);
                }
            }

            return correct_to_01(numerator / denominator);
        } // r_nonsep

        /**
         * b_param transformation
         */
        public double b_param(double y, double u, double A, double B, double C)
        {
            double result, v, exp;

            v = A - ((double)1.0 - (double)2.0 * u) *
                    Math.Abs((double)Math.Floor((double)0.5 - u) + A);
            exp = B + (C - B) * v;
            result = (double)Math.Pow(y, exp);

            return correct_to_01(result);
        } // b_param

        /**
         */
        double correct_to_01(double a)
        {
            double min = (double)0.0;
            double max = (double)1.0;
            double min_epsilon = min - epsilon;
            double max_epsilon = max + epsilon;

            if ((a <= min && a >= min_epsilon) || (a >= min && a <= min_epsilon))
            {
                return min;
            }
            else if ((a >= max && a <= max_epsilon) || (a <= max && a >= max_epsilon))
            {
                return max;
            }
            else
            {
                return a;
            }
        } // correct_to_01      
    }
}
