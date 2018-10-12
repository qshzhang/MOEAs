using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class ZDT6 : AbstractMOP
    {
        private static ZDT6 instance;
        private ZDT6(int pd)
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

            obj[0] = (1.0 - Math.Exp(-4.0 * sp[0])
                * Math.Pow(Math.Sin(6.0 * Math.PI * sp[0]), 6));

            double g = 0, h = 0;
            g = gf(sp);
            h = 1.0 - ((obj[0] / g) * (obj[0] / g));

            obj[1] = g * h;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        private double gf(double[] point)
        {
            double sum = 0;
            for (int i = 1; i < parDimension; i++)
                sum += point[i];
            sum /= (parDimension - 1);

            return 1 + 9 * Math.Pow(sum, 0.25);
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

        public static ZDT6 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new ZDT6(pd);
                instance.name = "ZDT6";
            }
            return instance;
        }
    }
}
