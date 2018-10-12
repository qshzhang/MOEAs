using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    class TNK : AbstractMOP
    {
        private static TNK instance;

        private TNK()
        {
            this.parDimension = 2;
            this.cneqNum = 2;
            this.ceqNum = 0;
            init();
        }
        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            obj[0] = sp[0];
            obj[1] = sp[1];

            chromosome.cneqValue[0] = -1 * (sp[0] * sp[0] + sp[1] * sp[1] - 1 - 0.1 * Math.Cos(16 * Math.Atan(sp[0] / sp[1])));
            chromosome.cneqValue[1] = Math.Pow(sp[0] - 0.5, 2) + Math.Pow(sp[1] - 0.5, 2) - 0.5;



            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = 0;
                domain[i, 1] = Math.PI;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static TNK getInstance()
        {
            if (instance == null)
            {
                instance = new TNK();
                instance.name = "TNK";
            }
            return instance;
        }
    }
}
