using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class DTLZ4_M : AbstractMOP
    {
        private static DTLZ4_M instance;
        //final private double alpha = 1;
        private readonly int K = 10;

        private DTLZ4_M(int pd)
        {

            this.objDimension = pd;
            this.parDimension = pd + K - 1;
            Init();
        }

        public override void Evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            double alpha = 100.0;
            int k = parDimension - objDimension + 1;

            double g = 0.0;
            for (int i = parDimension - k; i < parDimension; i++)
                g += (sp[i] - 0.5) * (sp[i] - 0.5);

            for (int i = 0; i < objDimension; i++)
                obj[i] = 1.0 + g;

            for (int i = 0; i < objDimension; i++)
            {
                for (int j = 0; j < objDimension - (i + 1); j++)
                    obj[i] *= Math.Cos(Math.Pow(sp[j], alpha) * (Math.PI / 2.0));
                if (i != 0)
                {
                    int aux = objDimension - (i + 1);
                    obj[i] *= Math.Sin(Math.Pow(sp[aux], alpha) * (Math.PI / 2.0));
                } //if
            } // for

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = 0;
                domain[i, 1] = 1;
            }

            this.range = new double[objDimension, 2];
        }

        public static DTLZ4_M GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new DTLZ4_M(pd);
                instance.name = "DTLZ4_" + pd;
            }
            return instance;
        }
    }
}
