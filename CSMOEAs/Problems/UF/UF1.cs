using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF1 : AbstractMOP
    {
        private static UF1 instance;
        private UF1(int pd)
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

            double f1 = 0, f2 = 0;
            int cnt1 = 0, cnt2 = 2;
            for (int i = 2; i <= this.parDimension; i++)
            {
                if (i % 2 == 1)
                {
                    f1 += Math.Pow(sp[i - 1] - Math.Sin(6.0 * Math.PI * sp[0] + i * 1.0 * Math.PI / parDimension), 2);
                    cnt1++;
                }
                else
                {
                    f2 += Math.Pow(sp[i - 1] - Math.Sin(6.0 * Math.PI * sp[0] + i * 1.0 * Math.PI / parDimension), 2);
                    cnt2++;
                }
            }
            obj[0] = sp[0] + 2 * f1 / (this.parDimension / 2);
            obj[1] = 1 - Math.Sqrt(sp[0]) + 2 * f2 / (this.parDimension / 2);

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

        public static UF1 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF1(pd);
                instance.name = "UF1";
            }
            return instance;
        }
    }
}
