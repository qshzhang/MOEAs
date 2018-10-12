using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF6 : AbstractMOP
    {
        private static UF6 instance;
        private UF6(int pd)
        {
            this.parDimension = pd;
            init();
        }
        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            int count1, count2;
            double prod1, prod2;
            double sum1, sum2, yj, hj, pj;
            sum1 = sum2 = 0.0;
            count1 = count2 = 0;
            prod1 = prod2 = 1.0;

            int N_ = 2;
            double epsilon_ = 0.1;

            for (int j = 2; j <= this.parDimension; j++)
            {
                yj = sp[j - 1] - Math.Sin(6.0 * Math.PI * sp[0] + j * Math.PI / parDimension);
                pj = Math.Cos(20.0 * yj * Math.PI / Math.Sqrt(j));
                if (j % 2 == 0)
                {
                    sum2 += yj * yj;
                    prod2 *= pj;
                    count2++;
                }
                else
                {
                    sum1 += yj * yj;
                    prod1 *= pj;
                    count1++;
                }
            }
            hj = 2.0 * (0.5 / N_ + epsilon_) * Math.Sin(2.0 * N_ * Math.PI * sp[0]);
            if (hj < 0.0)
                hj = 0.0;

            obj[0] = sp[0] + hj + 2.0 * (4.0 * sum1 - 2.0 * prod1 + 2.0) / (double)count1;
            obj[1] = 1.0 - sp[0] + hj + 2.0 * (4.0 * sum2 - 2.0 * prod2 + 2.0) / (double)count2;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension, 2];
            domain[0, 0] = 0;
            domain[0, 1] = 1;
            for (int i = 1; i < parDimension; i++)
            {
                domain[i, 0] = -1;
                domain[i, 1] = 1;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static UF6 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF6(pd);
                instance.name = "UF6";
            }
            return instance;
        }
    }
}
