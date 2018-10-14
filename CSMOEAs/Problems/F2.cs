using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class F2 : AbstractMOP
    {
        private static F2 instance;

        private F2(int pd)
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

            double g = 0, tp;
            for (int i = 1; i < parDimension; i++)
            {
                tp = sp[i] - Math.Sin(0.5 * Math.PI * sp[i]);
                g += tp * tp - Math.Cos(2 * Math.PI * tp);
            }
            g = 2 * Math.Sin(0.5 * Math.PI * sp[0]) * (parDimension - 1 + g);
            obj[0] = (1 + g) * (1 - sp[0]);
            obj[1] = 0.5 * (1 + g) * (sp[0] + Math.Sqrt(sp[0]) * Math.Pow(Math.Cos(4 * Math.PI * sp[0]), 2));

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            // TODO Auto-generated method stub
            //parDimension = 3;
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = 0;
                domain[i, 1] = 1;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static F2 GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new F2(pd);
                instance.name = "F2";
            }
            return instance;
        }
    }
}
