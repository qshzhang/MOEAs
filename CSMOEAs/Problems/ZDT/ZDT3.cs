using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class ZDT3 : AbstractMOP
    {
        private static ZDT3 instance;
        private ZDT3(int pd)
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

            obj[0] = sp[0];
            double g = gf(sp);
            double part = sp[0] / g;
            double part2 = (1 - Math.Sqrt(part) - part
                    * Math.Sin(10 * Math.PI * sp[0]));
            obj[1] = g * part2 + 1;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        private double gf(double[] point)
        {
            double sum = 0;
            for (int i = 1; i < parDimension; i++)
                sum += point[i];
            return 1 + 9 * sum / (parDimension - 1);
        }

        public override void Init()
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

        public static ZDT3 GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new ZDT3(pd);
                instance.name = "ZDT3";
            }
            return instance;
        }
    }
}
