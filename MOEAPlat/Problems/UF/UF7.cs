using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF7 : AbstractMOP
    {
        private static UF7 instance;
        private UF7(int pd)
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
            double sum1, sum2, yj;
            sum1 = sum2 = 0.0;
            count1 = count2 = 0;

            for (int j = 2; j <= parDimension; j++)
            {
                yj = sp[j - 1] - Math.Sin(6.0 * Math.PI * sp[0] + j * Math.PI / parDimension);
                if (j % 2 == 0)
                {
                    sum2 += yj * yj;
                    count2++;
                }
                else
                {
                    sum1 += yj * yj;
                    count1++;
                }
            }
            yj = Math.Pow(sp[0], 0.2);

            obj[0] = yj + 2.0 * sum1 / (double)count1;
            obj[1] = 1.0 - yj + 2.0 * sum2 / (double)count2;

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

        public static UF7 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF7(pd);
                instance.name = "UF7";
            }
            return instance;
        }
    }
}
