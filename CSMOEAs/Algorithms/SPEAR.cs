using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of SPEA/R and test problems can refer to the following paper
 * 
 *    Jiang S, Yang S. A Strength Pareto Evolutionary Algorithm Based on Reference Direction for Multiobjective 
 *    and Many-Objective Optimization[J]. IEEE Transactions on Evolutionary Computation, 2017, 21(3):329-346.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class SPEAR : MultiObjectiveSolver
    {
        Random random = new Random();
        int k;

        double thetam;

        //public List<MoChromosome> mainpop = new List<MoChromosome>();

        public void initial()
        {
            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            initWeight(this.div);
            initialPopulation();
            inittheta();
        }

        protected void initWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.popsize = this.weights.Count();
            k = (int)Math.Sqrt(2 * popsize);
        }

        private void inittheta()
        {
            double[] arr = new double[this.popsize];
            for(int i = 0;i < this.popsize; i++)
            {
                double min = Double.MaxValue;
                for(int j = 0;j < popsize; j++)
                {
                    if (i == j) continue;
                    double tp = Tool.getAngle(weights[i], weights[j]);
                    if (tp < min) min = tp;
                }
                arr[i] = min;
            }
            thetam = Tool.ArrayMax(arr);
        }

        protected void initialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.createChromosome();

                evaluate(chromosome);
                updateReference(chromosome);
                mainpop.Add(chromosome);
            }
        }

        protected override void doSolve()
        {
            initial();
            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {

                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    if (GlobalValue.CrossoverType.Equals("SBX"))
                    {
                        offspring = SBXCrossover(i);//GeneticOPDE//GeneticOPSBXCrossover
                    }
                    else
                    {
                        offspring = DECrossover(i);
                    }
                    this.evaluate(offspring);
                    offsPop.Add(offspring);
                    updateReference(offspring);
                }

                List<MoChromosome> Pop = new List<MoChromosome>();
                Pop.AddRange(mainpop);
                Pop.AddRange(offsPop);

                EnviromentSelection(Pop);

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();
                }

                ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();

            GlobalRawFitness(pop);

            List<List<MoChromosome>> associatedSolution = clustering(pop);

            for(int i = 0;i < associatedSolution.Count; i++)
            {
                LocalRawFitness(associatedSolution[i]);
                for(int r = 0;r < associatedSolution[i].Count; r++)
                {
                    if(associatedSolution[i].Count == 1)
                    {
                        associatedSolution[i][r].fitnessValue = associatedSolution[i][r].FVl;
                    }
                    else
                    {
                        associatedSolution[i][r].fitnessValue = associatedSolution[i][r].FVg + associatedSolution[i][r].FVl;
                    }
                }
            }

            for (int i = 0; i < this.weights.Count(); i++)
            {
                associatedSolution[i] = associatedSolution[i].OrderBy(r => r.fitnessValue).ToList();
            }

            int cnt = 0;
            while (true)
            {
                int size = 0;
                for (int i = 0; i < this.weights.Count(); i++)
                {
                    if (associatedSolution[i].Count() > cnt)
                    {
                        size++;
                    }
                }
                if (result.Count() + size <= this.popsize)
                {
                    for (int i = 0; i < this.weights.Count(); i++)
                    {
                        if (associatedSolution[i].Count() > cnt)
                        {
                            result.Add(associatedSolution[i][cnt]);
                        }
                    }
                }
                else
                {
                    break;
                }

                if (result.Count() == this.popsize) break;
                cnt++;
            }
            if (result.Count() < this.popsize)
            {
                List<MoChromosome> temp = new List<MoChromosome>();
                for (int i = 0; i < this.weights.Count(); i++)
                {
                    if (associatedSolution[i].Count() > cnt)
                    {
                        temp.Add(associatedSolution[i][cnt]);
                    }
                }


                temp = temp.OrderBy(r => r.fitnessValue).ToList();

                int pos = 0;
                while (result.Count() < popsize)
                {
                    result.Add(temp[pos]);
                    pos++;
                }

            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;

        }

        protected void LocalRawFitness(List<MoChromosome> pop)
        {
            int[] cnt = GetSi(pop);
            for (int i = 0; i < pop.Count; i++)
            {
                pop[i].FVl = 0;
                for (int j = 0; j < pop.Count; j++)
                {
                    if (i == j) continue;
                    if (pop[j].dominates(pop[i]))
                    {
                        pop[i].FVl += cnt[j];
                    }
                }
                pop[i].FVl += pop[i].angle / (pop[i].angle + thetam);
            }
        }

        protected List<List<MoChromosome>> clustering(List<MoChromosome> pop)
        {
            List<List<MoChromosome>> associatedSolution;
            associatedSolution = new List<List<MoChromosome>>();
            for (int i = 0; i < this.weights.Count(); i++)
            {
                List<MoChromosome> temp = new List<MoChromosome>();
                associatedSolution.Add(temp);
            }

            for (int i = 0; i < pop.Count(); i++)
            {
                double dist = Double.MaxValue, dt;
                int pos = -1;
                for (int j = 0; j < this.weights.Count(); j++)
                {
                    dt = getAngle(j, pop[i]);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                //pop[i].tchVal = pbiScalarObj(pos, pop[i]);
                pop[i].angle = dist;
                pop[i].subProbNo = pos;
                associatedSolution[pos].Add(pop[i]);
            }
            return associatedSolution;
        }

        protected void GlobalRawFitness(List<MoChromosome> pop)
        {
            int[] cnt = GetSi(pop);

            List<double[]> distMatrix = DistMatrix(pop);

            for (int i = 0; i < pop.Count; i++)
            {
                pop[i].FVg = 0;
                for (int j = 0; j < pop.Count; j++)
                {
                    if (i == j) continue;
                    if (pop[j].dominates(pop[i]))
                    {
                        pop[i].FVg += cnt[j];
                    }
                }
                pop[i].FVg += (1.0 / (distMatrix[i][k] + 2));
            }
        }

        private int[] GetSi(List<MoChromosome> pop)
        {
            int[] cnt = new int[pop.Count];
            for (int i = 0; i < cnt.Length; i++) cnt[i] = 0;
            for (int i = 0; i < pop.Count; i++)
            {
                for (int j = 0; j < pop.Count; j++)
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
            for (int i = 0; i < pop.Count; i++)
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
