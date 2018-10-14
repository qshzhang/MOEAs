using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class UF8 : AbstractMOP
    {
        private static UF8 instance;
        private UF8(int pd)
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

            int j, count1, count2, count3, nx;
            double sum1, sum2, sum3, yj;

            sum1 = sum2 = sum3 = 0.0;
            count1 = count2 = count3 = 0;
            nx = sp.Length;         // dimension of decision space


            for (j = 3; j <= nx; j++)
            {
                yj = sp[j - 1] - 2.0 * sp[1] * Math.Sin(2.0 * Math.PI * sp[0] + j * Math.PI / nx);
                if (j % 3 == 1)
                {
                    sum1 += yj * yj;
                    count1++;
                }
                else if (j % 3 == 2)
                {
                    sum2 += yj * yj;
                    count2++;
                }
                else
                {
                    sum3 += yj * yj;
                    count3++;
                } // end if/else
            } // end for
            obj[0] = Math.Cos(0.5 * Math.PI * sp[0]) * Math.Cos(0.5 * Math.PI * sp[1]) + 2.0 * sum1 / (double)count1;
            obj[1] = Math.Cos(0.5 * Math.PI * sp[0]) * Math.Sin(0.5 * Math.PI * sp[1]) + 2.0 * sum2 / (double)count2;
            obj[2] = Math.Sin(0.5 * Math.PI * sp[0]);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < 2; i++)
            {
                domain[i, 0] = 0;
                domain[i, 1] = 1;
            }
            for (int i = 2; i < parDimension; i++)
            {
                domain[i, 0] = -2;
                domain[i, 1] = 2;
            }
            this.objDimension = 3;
            this.range = new double[objDimension, 2];
        }

        public static UF8 GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new UF8(pd);
                instance.name = "UF8";
            }
            return instance;
        }
    }
}
