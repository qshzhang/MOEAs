using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public class CDP : AbstractMOP
    {
        private static CDP instance;

        private CDP()
        {
            this.parDimension = 5;
            Init();
        }


        public override void Evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);


            obj[0] = 1640.2823 + 2.3573285 * sp[0] + 2.3220035 * sp[1] + 4.5688768 * sp[2] + 7.7213633 * sp[3] + 4.4559504 * sp[4];

            obj[1] = 6.5856 + 1.15 * sp[0] - 1.0427 * sp[1] + 0.9738 * sp[2] + 0.8364 * sp[3] - 0.3695 * sp[0] * sp[3] +
                    0.0861 * sp[0] * sp[4] +
                    0.3628 * sp[1] * sp[3] - 0.1106 * sp[0] * sp[0] + 0.3437 * sp[2] * sp[2] + 0.1764 * sp[3] * sp[3];

            obj[2] = -0.0551 + 0.0181 * sp[0] + 0.1024 * sp[1] + 0.0421 * sp[2] - 0.0073 * sp[0] * sp[1] + 0.024 * sp[1] * sp[2] -
                    0.0118 * sp[1] * sp[3] - 0.0204 * sp[2] * sp[3] - 0.008 * sp[2] * sp[4] - 0.0241 * sp[1] * sp[1] +
                    0.0109 * sp[3] * sp[3];

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            // TODO Auto-generated method stub
            //parDimension = 3;
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = 1;
                domain[i, 1] = 3;
            }
            this.objDimension = 3;
            this.range = new double[objDimension, 2];
        }

        public static CDP GetInstance()
        {
            if (instance == null)
            {
                instance = new CDP();
                instance.name = "CDP";
            }
            return instance;
        }
    }
}
