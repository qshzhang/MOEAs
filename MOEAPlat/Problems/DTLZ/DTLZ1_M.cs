using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class DTLZ1_M : AbstractMOP
    {
        private readonly int K = 5;
        private DTLZ1_M(int M)
        {
            parDimension = M + K - 1;
            this.objDimension = M;
            init();
        }

        public override void init()
        {
            this.domain = new double[this.parDimension,2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i,0] = 0;
                domain[i,1] = 1;
            }

            this.range = new double[objDimension,2];
        }

        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);


            double g = 0.0;
            for (int i = objDimension; i <= parDimension; i++)
                g += (sp[i - 1] - 0.5) * (sp[i - 1] - 0.5) - Math.Cos(20 * Math.PI * (sp[i - 1] - 0.5));
            // Note this is 20*PI in Deb's dtlz1 func
            g += K;
            g *= 100;

            obj[0] = 1d / 2;
            for (int i = 0; i < this.objDimension - 1; i++)
            {
                obj[0] *= sp[i];
            }
            obj[0] *= (1 + g);

            for (int i = 1; i < this.objDimension; i++)
            {
                obj[i] = 1d / 2;
                for (int j = 0; j < objDimension - i - 1; j++)
                {
                    obj[i] *= sp[j];
                }
                obj[i] *= (1 - sp[objDimension - i - 1]);
                obj[i] *= (1 + g);
            }

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public static DTLZ1_M getInstance(int M)
        {
            if (instance == null)
            {
                instance = new DTLZ1_M(M);
                instance.name = "DTLZ1_" + M;
            }
            return instance;
        }

        private static DTLZ1_M instance;
    }
}
