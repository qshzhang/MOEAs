using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class SCH : AbstractMOP
    {
        private static SCH instance;
        private SCH()
        {
            this.parDimension = 1;
            Init();
        }
        public override void Evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            obj[0] = sp[0] * sp[0];
            obj[1] = (sp[0] - 2) * (sp[0] - 2);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = -1000;
                domain[i, 1] = 1000;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static SCH GetInstance()
        {
            if (instance == null)
            {
                instance = new SCH();
                instance.name = "SCH";
            }
            return instance;
        }
    }
}
