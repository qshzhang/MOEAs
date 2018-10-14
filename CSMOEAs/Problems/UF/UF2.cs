using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF2 : AbstractMOP
    {
        private static UF2 instance;
        private UF2(int pd)
        {
            this.parDimension = pd;
            Init();
        }
        public override void Evaluate(MoChromosome chromosome)
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
                if (j % 2 == 0)
                {
                    yj = sp[j - 1] -
                            (0.3 * sp[0] * sp[0] * Math.Cos(24 * Math.PI * sp[0] + 4 * j * Math.PI / parDimension) + 0.6 * sp[0]) *
                            Math.Sin(6.0 * Math.PI * sp[0] + j * Math.PI / parDimension);
                    sum2 += yj * yj;
                    count2++;
                }
                else
                {
                    yj = sp[j - 1] -
                            (0.3 * sp[0] * sp[0] * Math.Cos(24 * Math.PI * sp[0] + 4 * j * Math.PI / parDimension) + 0.6 * sp[0]) *
                            Math.Cos(6.0 * Math.PI * sp[0] + j * Math.PI / parDimension);
                    sum1 += yj * yj;
                    count1++;
                }
            }

            obj[0] = sp[0] + 2.0 * sum1 / (double)count1;
            obj[1] = 1.0 - Math.Sqrt(sp[0]) + 2.0 * sum2 / (double)count2;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
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

        public static UF2 GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF2(pd);
                instance.name = "UF2";
            }
            return instance;
        }
    }
}
