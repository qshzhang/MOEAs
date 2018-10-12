using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class SRN : AbstractMOP
    {
        private static SRN instance;

        private SRN()
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

            obj[0] = 2 + Math.Pow(sp[0] - 2, 2) + Math.Pow(sp[1] - 1, 2);
            obj[1] = 9 * sp[0] - Math.Pow(sp[1] - 1, 2);

            chromosome.cneqValue[0] = sp[0] * sp[0] + sp[1] * sp[1] - 255;
            chromosome.cneqValue[1] = sp[0] - 3 * sp[1] + 10;



            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = -20;
                domain[i, 1] = 20;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static SRN getInstance()
        {
            if (instance == null)
            {
                instance = new SRN();
                instance.name = "SRN";
            }
            return instance;
        }
    }
}
