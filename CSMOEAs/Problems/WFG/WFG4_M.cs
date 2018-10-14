using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class WFG4_M : WFG.WFG
    {
        private static WFG4_M instance;

        private WFG4_M(int M) : base(M)
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
            y = t2(y, k_, M_);

            double[] x = calculate_x(y);
            for (int m = 1; m <= M_; m++)
            {
                obj[m - 1] = D_ * x[M_ - 1] + S_[m - 1] * (new WFG.Shapes()).concave(x, m);
            }

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }


        public static WFG4_M GetInstance(int M)
        {
            if (instance == null)
            {
                instance = new WFG4_M(M);
                instance.name = "WFG4_" + M;
            }
            return instance;
        }

        /**
           * WFG4 t1 transformation
           */
        public double[] t1(double[] z, int k)
        {
            double[] result = new double[z.Length];

            for (int i = 0; i < z.Length; i++)
            {
                result[i] = (new WFG.Transformations()).s_multi(z[i], 30, 10, (double)0.35);
            }

            return result;
        } // t1

        /**
        * WFG4 t2 transformation
        */
        public double[] t2(double[] z, int k, int M)
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

            int head = k + 1;
            int tail = z.Length;

            double[] subZ = subVector(z, head - 1, tail - 1);
            double[] subW = subVector(w, head - 1, tail - 1);
            result[M - 1] = (new WFG.Transformations()).r_sum(subZ, subW);

            return result;
        } // t2
    }
}
