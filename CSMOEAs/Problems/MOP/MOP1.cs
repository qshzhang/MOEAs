using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class MOP1 : AbstractMOP
    {
        private static MOP1 instance;

        private MOP1()
        {
            this.parDimension = 10;
            init();
        }

        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);
            
            // objection function

            double g = 0, ti = 0;
            for (int j = 3; j <= parDimension; j++)
            {
                ti = sp[j - 1] - Math.Sin(0.5 * Math.PI * sp[0]);
                g += -0.9 * ti * ti + Math.Pow(Math.Abs(ti), 0.6);
            }
            g = 2 * Math.Sin(Math.PI * sp[0]) * g;

            obj[0] = (1 + g) * sp[0];
            obj[1] = (1 + g) * (1 - Math.Sqrt(sp[0]));
            
            //

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

        public static MOP1 getInstance()
        {
            if (instance == null)
            {
                instance = new MOP1();
                instance.name = "MOP1";
            }
            return instance;
        }
    }
}
