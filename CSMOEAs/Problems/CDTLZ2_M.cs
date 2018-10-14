using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class CDTLZ2_M : AbstractMOP
    {
        private static CDTLZ2_M instance;
        private double alpha = 1;
        private int K = 10;

        private CDTLZ2_M(int pd)
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

            int k = this.parDimension - this.objDimension + 1;

            double g = 0.0;
            for (int i = this.parDimension - k; i < parDimension; i++)
            {
                g += Math.Pow(sp[i] - 0.5, 2);
            }

            obj[0] = 1;
            for (int i = 0; i < this.objDimension - 1; i++)
            {
                obj[0] *= Math.Cos(Math.Pow(sp[i], alpha) * Math.PI / 2);
            }
            obj[0] *= (1 + g);

            for (int i = 1; i < this.objDimension; i++)
            {
                obj[i] = 1;
                for (int j = 0; j < objDimension - i - 1; j++)
                {
                    obj[i] *= Math.Cos(Math.Pow(sp[j], alpha) * Math.PI / 2);
                }
                obj[i] *= Math.Sin(Math.Pow(sp[objDimension - i - 1], alpha) * Math.PI / 2);
                obj[i] *= (1 + g);
            }

            for (int i = 0; i < this.objDimension - 1; i++)
            {
                obj[i] = Math.Pow(obj[i], 4);
            }
            obj[this.objDimension - 1] = Math.Pow(obj[this.objDimension - 1], 2);

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

        public static CDTLZ2_M GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new CDTLZ2_M(pd);
                instance.name = "CDTLZ2_" + pd;
            }
            return instance;
        }
    }
}
