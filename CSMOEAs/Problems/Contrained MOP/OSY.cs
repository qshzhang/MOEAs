using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    class OSY : AbstractMOP
    {
        private static OSY instance;

        private OSY()
        {
            this.parDimension = 6;
            this.cneqNum = 6;
            this.ceqNum = 0;
            Init();
        }
        public override void Evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            obj[0] = -1 * (25 * Math.Pow(sp[0] - 2, 2) + Math.Pow(sp[1] - 2, 2) + Math.Pow(sp[2] - 1, 2) + Math.Pow(sp[3] - 4, 2) + Math.Pow(sp[4] - 1, 2));
            obj[1] = sp[0] * sp[0] + sp[1] * sp[1] + sp[2] * sp[2] + sp[3] * sp[3] + sp[4] * sp[4] + sp[5] * sp[5];

            chromosome.cneqValue[0] = 2 - sp[0] - sp[1];
            chromosome.cneqValue[1] = sp[0] + sp[1] - 6;
            chromosome.cneqValue[2] = sp[1] - sp[0] - 2;
            chromosome.cneqValue[3] = sp[0] - 3 * sp[1] - 2;
            chromosome.cneqValue[4] = Math.Pow(sp[2] - 3, 2) + sp[3] - 4;
            chromosome.cneqValue[5] = 4 - Math.Pow(sp[4] - 3, 2) - sp[5];



            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension, 2];

            domain[0, 0] = 0;
            domain[0, 1] = 10;

            domain[1, 0] = 0;
            domain[1, 1] = 10;

            domain[5, 0] = 0;
            domain[5, 1] = 10;

            domain[2, 0] = 1;
            domain[2, 1] = 5;

            domain[4, 0] = 1;
            domain[4, 1] = 5;

            domain[3, 0] = 0;
            domain[3, 1] = 6;

            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static OSY GetInstance()
        {
            if (instance == null)
            {
                instance = new OSY();
                instance.name = "OSY";
            }
            return instance;
        }
    }
}
