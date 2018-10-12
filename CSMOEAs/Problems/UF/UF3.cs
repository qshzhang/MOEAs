using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF3 : AbstractMOP
    {
        private static UF3 instance;
        private UF3(int pd)
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
            double sum1, sum2, prod1, prod2, yj, pj;
            sum1 = sum2 = 0.0;
            count1 = count2 = 0;
            prod1 = prod2 = 1.0;


            for (int j = 2; j <= this.parDimension; j++)
            {
                yj = sp[j - 1] - Math.Pow(sp[0], 0.5 * (1.0 + 3.0 * (j - 2.0) / (this.parDimension - 2.0)));
                pj = Math.Cos(20.0 * yj * Math.PI / Math.Sqrt(j));
                if (j % 2 == 0)
                {
                    sum2 += yj * yj;
                    prod2 *= pj;
                }
                else
                {
                    sum1 += yj * yj;
                    prod1 *= pj;
                }
            }

            count1 = count2 = (this.parDimension - 2) / 2;

            obj[0] = sp[0] + 2.0 * (4.0 * sum1 - 2.0 * prod1 + 2.0) / (double)14;
            obj[1] = 1.0 - Math.Sqrt(sp[0]) + 2.0 * (4.0 * sum2 - 2.0 * prod2 + 2.0) / (double)14;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = 0;
                domain[i, 1] = 1;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static UF3 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF3(pd);
                instance.name = "UF3";
            }
            return instance;
        }
    }
}
