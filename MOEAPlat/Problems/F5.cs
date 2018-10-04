using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class F5 : AbstractMOP
    {
        private static F5 instance;
        private F5(int pd)
        {
            this.parDimension = pd;
            init();
        }
        public override void evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]); 

            double g = 0;
		    for(int i = 2;i < parDimension;i++){
			    g += Math.Pow(sp[i] - 0.5,2);
		    }
		
		    obj[0] = (1 + g) * ((1 - sp[0]) * sp[1]);
		    obj[1] = (1 + g) * ((1 - sp[1]) * sp[0]);
		    obj[2] = (1 + g) * Math.Pow(1- sp[0] - sp[1] + 2 * sp[0] * sp[1], 6);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i,0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void init()
        {
            this.domain = new double[this.parDimension,2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i,0] = 0;
                domain[i,1] = 1;
            }
            this.objDimension = 3;
            this.range = new double[objDimension,2];
        }

        public static F5 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new F5(pd);
                instance.name = "F5";
            }
            return instance;
        }
    }
}
