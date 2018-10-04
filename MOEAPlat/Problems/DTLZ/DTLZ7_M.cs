using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class DTLZ7_M : AbstractMOP
    {
        private static DTLZ7_M instance;
        private readonly int K = 10;

        private DTLZ7_M(int pd)
        {

            this.objDimension = pd;
            this.parDimension = pd + K - 1;
            init();
        }

        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);


            int k = parDimension - objDimension + 1;
            double g = 0.0;
            for (int i = this.parDimension - k; i < parDimension; i++)
                g += sp[i];

            g = 1 + (9.0 * g) / k;

            Array.Copy(sp, obj, objDimension - 1);

            double h = 0.0;
            for (int i = 0; i < objDimension - 1; i++)
                h += (obj[i] / (1.0 + g)) * (1 + Math.Sin(3.0 * Math.PI * obj[i]));

            h = objDimension - h;

            obj[objDimension - 1] = (1 + g) * h;

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

            this.range = new double[objDimension, 2];
        }

        public static DTLZ7_M getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new DTLZ7_M(pd);
                instance.name = "DTLZ7_" + pd;
            }
            return instance;
        }
    }
}
