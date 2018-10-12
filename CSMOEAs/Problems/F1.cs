using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class F1 : AbstractMOP
    {
        private static F1 instance;
        private F1(int pd)
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

            double g = 0, tp;
            for (int i = 1; i < parDimension; i++)
            {
                tp = sp[i] - Math.Sin(0.5 * Math.PI * sp[i]);
                g += tp * tp - Math.Cos(2 * Math.PI * tp);
            }
            g = 2 * Math.Sin(0.5 * Math.PI * sp[0]) * (parDimension - 1 + g);
            obj[0] = (1 + g) * sp[0];
            obj[1] = (1 + g) * Math.Pow(1 - Math.Sqrt(sp[0]), 5);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i,0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension,2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i,0] = 0;
                domain[i,1] = 1;
            }
            this.objDimension = 2;
            this.range = new double[objDimension,2];
        }

        public static F1 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new F1(pd);
                instance.name = "F1";
            }
            return instance;
        }
    }
}
