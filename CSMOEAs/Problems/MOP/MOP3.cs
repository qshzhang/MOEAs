using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class MOP3 : AbstractMOP
    {
        private static MOP3 instance;

        private MOP3()
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
            for (int j = 2; j <= parDimension; j++)
            {
                ti = sp[j - 1] - Math.Sin(0.5 * Math.PI * sp[0]);
                ti = Math.Abs(ti);
                g += ti / (1 + Math.Exp(5 * ti));
            }
            g = 10 * Math.Sin(Math.PI * sp[0] / 2) * g;

            obj[0] = (1 + g) * Math.Cos(Math.PI * sp[0] / 2);
            obj[1] = (1 + g) * Math.Sin(Math.PI * sp[0] / 2);

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

        public static MOP3 getInstance()
        {
            if (instance == null)
            {
                instance = new MOP3();
                instance.name = "MOP3";
            }
            return instance;
        }
    }
}
