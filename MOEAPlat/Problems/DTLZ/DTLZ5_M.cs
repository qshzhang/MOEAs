using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class DTLZ5_M : AbstractMOP
    {
        private static DTLZ5_M instance;
        //final private double alpha = 1;
        private readonly int K = 10;

        private DTLZ5_M(int pd)
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

            double[] theta = new double[objDimension - 1];
            double g = 0.0;
            int k = parDimension - objDimension + 1;

            for (int i = parDimension - k; i < parDimension; i++)
                g += (sp[i] - 0.5) * (sp[i] - 0.5);

            double t = Math.PI / (4.0 * (1.0 + g));

            theta[0] = sp[0] * Math.PI / 2.0;
            for (int i = 1; i < (objDimension - 1); i++)
                theta[i] = t * (1.0 + 2.0 * g * sp[i]);

            for (int i = 0; i < objDimension; i++)
                obj[i] = 1.0 + g;

            for (int i = 0; i < objDimension; i++)
            {
                for (int j = 0; j < objDimension - (i + 1); j++)
                    obj[i] *= Math.Cos(theta[j]);
                if (i != 0)
                {
                    int aux = objDimension - (i + 1);
                    obj[i] *= Math.Sin(theta[aux]);
                } // if
            } //for

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

        public static DTLZ5_M getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new DTLZ5_M(pd);
                instance.name = "DTLZ5_" + pd;
            }
            return instance;
        }
    }
}
