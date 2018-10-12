using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of SPEA2 and test problems can refer to the following paper
 * 
 *    E. Zitzler, M. Laumanns, and L. Thiele, "SPEA2: Improving the strength Pareto evolutionary algorithm 
 *    for multiobjective optimization," in Proc. Evol. Methods Des. Optimiz. Contr. Applicat. Indust. Problems, 
 *    EUROGEN, vol. 3242, no. 103, 2002, pp. 95-100.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class SPEA2 : MultiObjectiveSolver
    {
        Random random = new Random();

        int k;

        private List<MoChromosome> externalSet = new List<MoChromosome>();

        public void initial()
        {
            this.popsize = div;
            initialPopulation();
            k = (int)Math.Sqrt(2 * popsize);
        }

        protected void initialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.createChromosome();

                evaluate(chromosome);
                mainpop.Add(chromosome);
            }
        }


        protected override void doSolve()
        {
            initial();

            string prob = mop.getName();
            if (prob.IndexOf("DTLZ") != -1)
            {
                igdValue.Add(QulityIndicator.QulityIndicator.DTLZIGD(mainpop, prob, this.numObjectives));
            }
            else
            {
                pofData = FileTool.readData(pofPath + prob);
                igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
            }

            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {

                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i, false);//GeneticOPDE//GeneticOPSBXCrossover
                    this.evaluate(offspring);
                    offsPop.Add(offspring);
                }

                List<MoChromosome> Pop = new List<MoChromosome>();
                Pop.AddRange(mainpop);
                Pop.AddRange(offsPop);

                EnviromentSelection(Pop);

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();

                    if (prob.IndexOf("DTLZ") != -1)
                    {
                        igdValue.Add(QulityIndicator.QulityIndicator.DTLZIGD(mainpop, prob, this.numObjectives));
                    }
                    else
                    {
                        igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
                    }
                }

                ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
            Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            RawFitness(pop);
            List<MoChromosome> result = new List<MoChromosome>();

            for(int i = 0;i < pop.Count; i++)
            {
                if (pop[i].fitnessValue < 1) result.Add(pop[i]);
            }

            if(result.Count < popsize)
            {
                pop = pop.OrderBy(r => r.fitnessValue).ToList();
            }

            int cnt = result.Count;
            while(result.Count < popsize)
            {
                result.Add(pop[cnt]);
                cnt++;
            }
            if(result.Count > popsize)
            {
                NSGA.crowdingDistanceAssignment(result);
                result = result.OrderByDescending(r => r.crdistance).ToList();
            }

            mainpop.Clear();
            for(int r = 0;r < popsize; r++)
            {
                mainpop.Add(result[r]);
            }
        }

        protected void RawFitness(List<MoChromosome> pop)
        {
            int[] cnt = GetSi(pop);

            List<double[]> distMatrix = DistMatrix(pop);

            for (int i = 0;i < pop.Count; i++)
            {
                pop[i].fitnessValue = 0;
                for(int j = 0;j < pop.Count; j++)
                {
                    if (i == j) continue;
                    if (pop[j].dominates(pop[i]))
                    {
                        pop[i].fitnessValue += cnt[j];
                    }
                }
                pop[i].fitnessValue += (1.0 / (distMatrix[i][k] + 2));
            }
        }

        private int[] GetSi(List<MoChromosome> pop)
        {
            int[] cnt = new int[pop.Count];
            for (int i = 0; i < cnt.Length; i++) cnt[i] = 0;
            for(int i = 0;i < pop.Count; i++)
            {
                for(int j = 0;j < pop.Count; j++)
                {
                    if (i == j) continue;
                    if (pop[i].dominates(pop[j])) cnt[i]++;
                }
            }
            return cnt;
        }

        private List<double[]> DistMatrix(List<MoChromosome> pop)
        {
            List<double[]> list = new List<double[]>();
            for(int i = 0; i < pop.Count; i++)
            {
                double[] dist = new double[pop.Count];
                for (int r = 0; r < i; r++)
                {
                    dist[r] = list[r][i];
                }
                for (int j = i; j < pop.Count; j++)
                {
                    dist[j] = distance(pop[i].objectivesValue, pop[j].objectivesValue);
                }
                list.Add(dist);
            }
            for (int i = 0; i < pop.Count; i++) Array.Sort(list[i]);
            return list;
        }

    }
}
