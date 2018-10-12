using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    class CTP1 : AbstractMOP
    {
        private static CTP1 instance;

        private CTP1()
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
            obj[1] = (1 + sp[1]) * Math.Exp(-1 * sp[0] / (1 + sp[1]));

            chromosome.cneqValue[0] = 1 - obj[1] / (0.858 * Math.Exp(-0.541 * obj[0]));
            chromosome.cneqValue[1] = 1 - obj[1] / (0.728 * Math.Exp(-0.295 * obj[0]));



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

        public static CTP1 getInstance()
        {
            if (instance == null)
            {
                instance = new CTP1();
                instance.name = "CTP1";
            }
            return instance;
        }
    }
}
