using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF4 : AbstractMOP
    {
        private static UF4 instance;
        private UF4(int pd)
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

            int j, count1, count2, nx;
            double sum1, sum2, yj, hj;

            sum1 = sum2 = 0.0;
            count1 = count2 = 0;
            nx = sp.Length;         // dimension of decision space

            for (j = 2; j <= nx; j++)
            {
                yj = sp[j - 1] - Math.Sin(6.0 * Math.PI * sp[0] + j * Math.PI / nx);
                hj = Math.Abs(yj) / (1.0 + Math.Exp(2.0 * Math.Abs(yj)));
                if (j % 2 == 0)
                {
                    sum2 += hj;
                    count2++;
                }
                else
                {
                    sum1 += hj;
                    count1++;
                } // end if/else
            } // end for
            obj[0] = sp[0] + 2.0 * sum1 / (double)count1;
            obj[1] = 1.0 - sp[0] * sp[0] + 2.0 * sum2 / (double)count2;

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
                domain[i, 0] = -2;
                domain[i, 1] = 2;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static UF4 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF4(pd);
                instance.name = "UF4";
            }
            return instance;
        }
    }
}
