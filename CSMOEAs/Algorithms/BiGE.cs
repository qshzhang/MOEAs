using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of BiGE and test problems can refer to the following paper
 * 
 * Li M, Yang S, Liu X. Bi-goal evolution for many-objective optimization problems[J]. 
 * Artificial Intelligence, 2015, 228(C):45-65. 
 * 
*/


namespace MOEAPlat.Algorithms
{
    public class BiGE : MultiObjectiveSolver
    {
        Random random = new Random();

        List<MoChromosome> matingpool = new List<MoChromosome>();

        double radius;

        //public List<MoChromosome> mainpop = new List<MoChromosome>();

        public void Initial()
        {
            this.popsize = div;

            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            InitialPopulation();
            radius = 1.0 / Math.Pow(popsize, 1.0 / this.numObjectives);
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

        private void MatingSelection()
        {
            while (matingpool.Count < mainpop.Count)
            {
                int pos1 = random.Next() % mainpop.Count;
                int pos2 = random.Next() % mainpop.Count;
                while (pos1 == pos2)
                {
                    pos2 = random.Next() % mainpop.Count;
                }
                if(Dominate(mainpop[pos1], mainpop[pos2]))
                {
                    matingpool.Add(mainpop[pos1]);
                }else if(Dominate(mainpop[pos2], mainpop[pos1]))
                {
                    matingpool.Add(mainpop[pos2]);
                }
                else
                {
                    if(random.NextDouble() < 0.5)
                    {
                        matingpool.Add(mainpop[pos1]);
                    }
                    else
                    {
                        matingpool.Add(mainpop[pos2]);
                    }
                }
            }
        }

        protected override void DoSolve()
        {
            Initial();
            frm = new plotFrm(mainpop, mop.GetName());
            frm.Show();
            frm.Refresh();
            while (!Terminated())
            {
                //MatingSelection();
                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i, false);//GeneticOPDE//GeneticOPSBXCrossover
                    this.Evaluate(offspring);
                    offsPop.Add(offspring);
                    UpdateReference(offspring);
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
            for(int i = 0;i < pop.Count; i++)
            {
                pop[i].fpr = Tool.ArraySum(TranObj(pop[i]));
            }

            List<double[]> shareMatrix = ShareMatrix(pop);
            for(int i = 0;i < pop.Count; i++)
            {
                pop[i].fcd = Math.Pow(Tool.ArraySum(shareMatrix[i]), 0.5);
            }

            List<MoChromosome> result = new List<MoChromosome>();
            List<List<MoChromosome>> dominatedSet0 = BiGEfastNonDominatedSort(pop);

            int cnt = 0;
            while (result.Count() + dominatedSet0[cnt].Count() < this.popsize)
            {
                for (int r = 0; r < dominatedSet0[cnt].Count(); r++)
                {
                    dominatedSet0[cnt][r].selected = true;
                }

                result.AddRange(dominatedSet0[cnt]);
                cnt++;
            }

            Boolean[] flag = new Boolean[dominatedSet0[cnt].Count];
            for (int i = 0; i < flag.Length; i++) flag[i] = false;
            while(result.Count < this.popsize)
            {
                int rnd = random.Next() % flag.Length;
                if(flag[rnd] == false)
                {
                    result.Add(dominatedSet0[cnt][rnd]);
                    flag[rnd] = true;
                }
                
            }

            mainpop.Clear();
            mainpop.AddRange(result);
            return;
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
                    dist[j] = Distance(TranObj(pop[i]), TranObj(pop[j]));
                }
                list.Add(dist);
            }
            for (int i = 0; i < pop.Count; i++) Array.Sort(list[i]);
            return list;
        }

        private List<double[]> ShareMatrix(List<MoChromosome> pop)
        {
            List<double[]> list = new List<double[]>();

            List<double[]> distMatrix = DistMatrix(pop);
            for(int i = 0;i < pop.Count; i++)
            {
                double[] v = new double[pop.Count];
                for (int j = 0; j < pop.Count; j++)
                {
                    if (i == j)
                    {
                        v[j] = 0;
                        continue;
                    }
                    if(distMatrix[i][j] < radius && pop[i].fpr < pop[j].fpr)
                    {
                        v[j] = Math.Pow(0.5 * (1 - distMatrix[i][j] / radius), 2);
                    }else if(distMatrix[i][j] < radius && pop[i].fpr > pop[j].fpr)
                    {
                        v[j] = Math.Pow(1.5 * (1 - distMatrix[i][j] / radius), 2);
                    }else if(distMatrix[i][j] < radius && pop[i].fpr == pop[j].fpr)
                    {
                        v[j] = random.NextDouble();
                    }
                    else
                    {
                        v[j] = 0;
                    }
                }
                list.Add(v);
            }

            return list;
        }

        public List<List<MoChromosome>> BiGEfastNonDominatedSort(List<MoChromosome> individuals)
        {
            List<List<int>> dominationFronts = new List<List<int>>();

            Dictionary<int, List<int>> individual2DominatedIndividuals =
               new Dictionary<int, List<int>>();
            Dictionary<int, int> individual2NumberOfDominatingIndividuals =
               new Dictionary<int, int>();


            int s = 0, t = 0;
            foreach (MoChromosome individualP in individuals)
            {
                individual2DominatedIndividuals.Add(s, new List<int>());
                individual2NumberOfDominatingIndividuals.Add(s, 0);

                t = 0;
                foreach (MoChromosome individualQ in individuals)
                {
                    if (Dominate(individualP, individualQ))
                    {
                        individual2DominatedIndividuals[s].Add(t);
                    }
                    else
                    {
                        if (Dominate(individualQ, individualP))
                        {
                            individual2NumberOfDominatingIndividuals[s] =
                                  individual2NumberOfDominatingIndividuals[s] + 1;
                        }
                    }
                    t++;
                }

                if (individual2NumberOfDominatingIndividuals[s] == 0)
                {
                    // p belongs to the first front
                    individualP.SetRank(1);
                    if (dominationFronts.Count == 0)
                    {
                        List<int> firstDominationFront = new List<int>();
                        firstDominationFront.Add(s);
                        dominationFronts.Add(firstDominationFront);
                    }
                    else
                    {
                        //	            	List<MoChromosome> firstDominationFront = dominationFronts.get(0);
                        //	               firstDominationFront.add(individualP);
                        dominationFronts[0].Add(s);
                    }
                }
                s++;
            }

            int i = 1;
            while (true)
            {
                List<int> nextDominationFront = new List<int>();
                foreach (int individualP in dominationFronts[i - 1])
                {
                    foreach (int individualQ in individual2DominatedIndividuals[individualP])
                    {
                        individual2NumberOfDominatingIndividuals[individualQ] =
                              individual2NumberOfDominatingIndividuals[individualQ] - 1;
                        if (individual2NumberOfDominatingIndividuals[individualQ] == 0)
                        {
                            individuals[individualQ].SetRank(i + 1);
                            nextDominationFront.Add(individualQ);
                        }
                    }
                }
                i++;
                if (nextDominationFront.Count != 0)
                {
                    dominationFronts.Add(nextDominationFront);
                }
                else
                {
                    break;
                }
            }

            List<List<MoChromosome>> frontSet = new List<List<MoChromosome>>();
            for (int x = 0; x < dominationFronts.Count(); x++)
            {
                List<MoChromosome> it = new List<MoChromosome>();
                for (int v = 0; v < dominationFronts[x].Count(); v++)
                {
                    individuals[dominationFronts[x][v]].rank = x + 1;
                    it.Add(individuals[dominationFronts[x][v]]);
                }
                frontSet.Add(it);
            }
            return frontSet;
        }

        private Boolean Dominate(MoChromosome mo1, MoChromosome mo2)
        {
            if (mo1.fcd < mo2.fcd && mo1.fpr < mo2.fpr) return true;
            return false;
        }

        private double[] TranObj(MoChromosome mo)
        {
            double[] v = new double[this.numObjectives];
            for(int i = 0;i < this.numObjectives; i++)
            {
                v[i] = (mo.objectivesValue[i] - this.idealpoint[i]) / (this.narpoint[i] - this.idealpoint[i]);
            }
            return v;
        }

    }
}
