using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class ZDT4 : AbstractMOP
    {
        private static ZDT4 instance;
        private ZDT4(int pd)
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

            obj[0] = sp[0];

            double g = gf(sp);
            double h = 1.0 - Math.Sqrt(sp[0] / g);
            obj[1] = g * h;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        private double gf(double[] point)
        {
            double sum = 0;
            for (int i = 1; i < parDimension; i++)
                sum += (Math.Pow(point[i], 2) - (10.0 * Math.Cos(4 * Math.PI
                        * point[i])));
            return 1.0 + (10.0 * (parDimension - 1.0)) + sum;
        }

        public override void init()
        {
            this.domain = new double[this.parDimension, 2];
            domain[0, 0] = 0;
            domain[0, 1] = 1;
            for (int i = 1; i < parDimension; i++)
            {
                domain[i, 0] = -5;
                domain[i, 1] = 5;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static ZDT4 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new ZDT4(pd);
                instance.name = "ZDT4";
            }
            return instance;
        }
    }
}
