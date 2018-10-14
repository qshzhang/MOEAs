using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of MOEA/D-M2M and test problems can refer to the following paper
 * 
 * Liu H L, Gu F, Zhang Q. Decomposition of a Multiobjective Optimization Problem Into a Number of 
 * Simple Multiobjective Subproblems[J]. IEEE Transactions on Evolutionary Computation, 2014, 18(3):450-455.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class MOEADM2M : MultiObjectiveSolver
    {
        private int S;
        private int K;

        Random random = new Random();

        List<List<MoChromosome>> Pop = new List<List<MoChromosome>>();

        protected void Initial()
        {
            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];
            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }
            InitWeight(this.div);
            InitialPopulation();
            MoreInitial();
        }

        protected void InitWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.K = this.weights.Count();
            this.S = this.K;
            this.popsize = K * S;
        }

        protected void InitialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.CreateChromosome();
                Evaluate(chromosome);
                mainpop.Add(chromosome);
                UpdateReference(chromosome);
            }
        }

        protected void MoreInitial()
        {
            for(int i = 0;i < K; i++)
            {
                List<MoChromosome> list = new List<MoChromosome>();
                Pop.Add(list);
            }
            for(int i = 0;i < popsize; i++)
            {
                double min = Double.MaxValue;
                int pos = -1;
                for(int j = 0;j < K; j++)
                {
                    double tp = GetAngle(j, mainpop[i]);
                    if(min > tp)
                    {
                        min = tp;
                        pos = j;
                    }
                }
                Pop[pos].Add(mainpop[i]);
            }
        }

        protected MoChromosome GeneticOPDE(int pos, int i)
        {//Differential Evolution
            int k, l;

            MoChromosome chromosome1 ;
            MoChromosome chromosome2 ;

            if (Pop[pos].Count < 4)
            {
                do
                    k = random.Next(0, popsize - 1);
                while (k == i);
                do
                    l = random.Next(0, popsize - 1);
                while (l == k || l == i);

                chromosome1 = mainpop[k];
                chromosome2 = mainpop[l];
            }
            else
            {
                do
                    k = random.Next(0, Pop[pos].Count);
                while (k == i);
                do
                    l = random.Next(0, Pop[pos].Count);
                while (l == k || l == i);

                chromosome1 = Pop[pos][k];
                chromosome2 = Pop[pos][l];
            }

            // generic operation crossover and mutation.
            MoChromosome offSpring = this.CreateChromosome();
            MoChromosome current = Pop[pos][i];

            offSpring.DECrossover(current, chromosome1, chromosome2, random);

            offSpring.Mutate(1d / offSpring.parDimension, random); 
            return offSpring;
        }

        protected override void DoSolve()
        {
            Initial();

            string prob = mop.GetName();
            pofData = FileTool.ReadData(pofPath + prob);
            igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));

            frm = new plotFrm(mainpop, mop.GetName());
            frm.Show();
            frm.Refresh();
            while (!Terminated())
            {
                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0;i < K; i++)
                {
                    for(int j = 0;j < Pop[i].Count; j++)
                    {
                        MoChromosome offspring;
                        offspring = GeneticOPDE(i, j);//GeneticOPDE//GeneticOPSBXCrossover
                        this.Evaluate(offspring);
                        offsPop.Add(offspring);
                        UpdateReference(offspring);
                    }
                }

                offsPop.AddRange(mainpop);
                EnviromentSelection(offsPop);

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();
                    igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
                }

                this.ItrCounter++;
            }
            List<MoChromosome> result = new List<MoChromosome>();
            result.AddRange(mainpop);
            mainpop.Clear();
            mainpop.AddRange(NSGA.FastNonDominatedSort(result)[0]);
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
            Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            for (int i = 0; i < K; i++) Pop[i].Clear();

            for(int i = 0;i < pop.Count; i++)
            {
                double min = Double.MaxValue;
                int pos = -1;
                for (int j = 0; j < K; j++)
                {
                    double tp = GetAngle(j, pop[i]);
                    if (min > tp)
                    {
                        min = tp;
                        pos = j;
                    }
                }
                Pop[pos].Add(pop[i]);
            }

            mainpop.Clear();
            List<MoChromosome> result = new List<MoChromosome>();
            for (int i = 0;i < K; i++)
            {
                if (Pop[i].Count < S)
                {
                    int total1 = Pop[i].Count;
                    result.AddRange(Pop[i]);
                    while(total1 < S)
                    {
                        result.Add(pop[random.Next() % pop.Count]);
                        total1++;
                    }
                    continue;
                }

                List<List<MoChromosome>> dominatedSet0 = NSGA.FastNonDominatedSort(Pop[i]);

                int cnt = 0;
                int total = 0;
                while (total + dominatedSet0[cnt].Count() < this.S)
                {
                    for (int r = 0; r < dominatedSet0[cnt].Count(); r++)
                    {
                        dominatedSet0[cnt][r].selected = true;
                    }
                    total += dominatedSet0[cnt].Count();
                    result.AddRange(dominatedSet0[cnt]);
                    cnt++;
                }
                if(total < S)
                {
                    NSGA.CrowdingDistanceAssignment(dominatedSet0[cnt]);
                    MoChromosome[] arr = NSGA.Sort(dominatedSet0[cnt]);

                    for(int r = 0;r < S - total; r++)
                    {
                        result.Add(arr[r]);
                    }

                }
            }

            for (int i = 0; i < K; i++) Pop[i].Clear();
            mainpop.AddRange(result);

            for (int i = 0; i < result.Count; i++)
            {
                double min = Double.MaxValue;
                int pos = -1;
                for (int j = 0; j < K; j++)
                {
                    double tp = GetAngle(j, result[i]);
                    if (min > tp)
                    {
                        min = tp;
                        pos = j;
                    }
                }
                Pop[pos].Add(result[i]);
            }

        }
    }
}
