using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class WFG2_M : WFG.WFG
    {
        private static WFG2_M instance;

        private WFG2_M(int M) : base(M)
        {
            S_ = new int[M_];
            for (int i = 0; i < M_; i++)
            {
                S_[i] = 2 * (i + 1);
            }
            A_ = new int[M_ - 1];
            for (int i = 0; i < M_ - 1; i++)
            {
                A_[i] = 1;
            }
        }

        public override void Evaluate(MoChromosome chromosome)
        {
            // TODO Auto-generated method stub
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            double[] y;

            y = normalise(sp);
            y = t1(y, k_);
            y = t2(y, k_);
            y = t3(y, k_, M_);

            double[] x = calculate_x(y);
            for (int m = 1; m <= M_ - 1; m++)
            {
                obj[m - 1] = D_ * x[M_ - 1] + S_[m - 1] * (new WFG.Shapes()).convex(x, m);
            }
            obj[M_ - 1] = D_ * x[M_ - 1] + S_[M_ - 1] * (new WFG.Shapes()).disc(x, 5, (double)1.0, (double)1.0);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }


        public static WFG2_M GetInstance(int M)
        {
            if (instance == null)
            {
                instance = new WFG2_M(M);
                instance.name = "WFG2_" + M;
            }
            return instance;
        }

        /**
           * WFG2 t1 transformation
           */
        public double[] t1(double[] z, int k)
        {
            double[] result = new double[z.Length];

            Array.Copy(z, 0, result, 0, k);

            for (int i = k; i < z.Length; i++)
            {
                result[i] = (new WFG.Transformations()).s_linear(z[i], (double)0.35);
            }

            return result;
        } // t1

        /**
         * WFG2 t2 transformation
         */
        public double[] t2(double[] z, int k)
        {
            double[] result = new double[z.Length];

            Array.Copy(z, 0, result, 0, k);

            int l = z.Length - k;

            for (int i = k + 1; i <= k + l / 2; i++)
            {
                int head = k + 2 * (i - k) - 1;
                int tail = k + 2 * (i - k);
                double[] subZ = subVector(z, head - 1, tail - 1);

                result[i - 1] = (new WFG.Transformations()).r_nonsep(subZ, 2);
            }

            return result;
        } // t2

        /**
         * WFG2 t3 transformation
         */
        public double[] t3(double[] z, int k, int M)
        {
            double[] result = new double[M];
            double[] w = new double[z.Length];


            for (int i = 0; i < z.Length; i++)
            {
                w[i] = (double)1.0;
            }

            for (int i = 1; i <= M - 1; i++)
            {
                int head1 = (i - 1) * k / (M - 1) + 1;
                int tail1 = i * k / (M - 1);
                double[] subZ1 = subVector(z, head1 - 1, tail1 - 1);
                double[] subW1 = subVector(w, head1 - 1, tail1 - 1);

                result[i - 1] = (new WFG.Transformations()).r_sum(subZ1, subW1);
            }

            int l = z.Length - k;
            int head = k + 1;
            int tail = k + l / 2;

            double[] subZ = subVector(z, head - 1, tail - 1);
            double[] subW = subVector(w, head - 1, tail - 1);
            result[M - 1] = (new WFG.Transformations()).r_sum(subZ, subW);

            return result;
        } // t3
    }
}
