using MOEAPlat.Encoding;
using System;


namespace MOEAPlat.Problems
{
    public class POL : AbstractMOP
    {
        private static POL instance;
        private POL()
        {
            this.parDimension = 2;
            Init();
        }
        public override void Evaluate(MoChromosome chromosome)
        {
            double[] sp = chromosome.realGenes;
            double[] obj = chromosome.objectivesValue;

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]);

            double A1 = 0.5 * Math.Sin(1) - 2 * Math.Cos(1) + Math.Sin(2) - 1.5 * Math.Cos(2);
            double A2 = 1.5 * Math.Sin(1) - Math.Cos(1) + 2 * Math.Sin(2) - 0.5 * Math.Cos(2);
            double B1 = 0.5 * Math.Sin(sp[0]) - 2 * Math.Cos(sp[0]) + Math.Sin(sp[1]) - 1.5 * Math.Cos(sp[1]);
            double B2 = 1.5 * Math.Sin(sp[0]) - Math.Cos(sp[0]) + 2 * Math.Sin(sp[1]) - 0.5 * Math.Cos(sp[1]);

            obj[0] = 1 + Math.Pow(A1 - B1, 2) + Math.Pow(A2 - B2, 2);
            obj[1] = Math.Pow(3 + sp[0], 2) + Math.Pow(1 + sp[1], 2);

            for (int i = 0; i < this.parDimension; i++)
                sp[i] = (sp[i] - domain[i, 0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension, 2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i, 0] = -1 * Math.PI;
                domain[i, 1] = Math.PI;
            }
            this.objDimension = 2;
            this.range = new double[objDimension, 2];
        }

        public static POL GetInstance()
        {
            if (instance == null)
            {
                instance = new POL();
                instance.name = "POL";
            }
            return instance;
        }
    }
}
