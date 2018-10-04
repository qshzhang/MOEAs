using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOEAPlat.Encoding;

namespace MOEAPlat.Problems
{
    public class F6 : AbstractMOP
    {
        private static F6 instance;
        private F6(int pd)
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
		    for(int i = 2;i < this.parDimension;i++){
			    g += (1 + Math.Pow(sp[i], 2) - Math.Cos(2 * Math.PI * sp[i]));
		    }
		    g /= 10;
		
		    obj[0] = Math.Pow(Math.Cos(0.5 * Math.PI * sp[0]),4) * Math.Pow(Math.Cos(0.5 * Math.PI * sp[1]), 4);
		    obj[1] = Math.Pow(Math.Cos(0.5 * Math.PI * sp[0]),4) * Math.Pow(Math.Sin(0.5 * Math.PI * sp[1]), 4);
		    obj[2] = Math.Pow((1 + g)/(1 + Math.Pow(Math.Cos(0.5 * Math.PI * sp[0]), 2)), 1/(1 + g));

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

        public static F6 getInstance(int pd)
        {
            if (instance == null)
            {
                instance = new F6(pd);
                instance.name = "F6";
            }
            return instance;
        }
    }
}
