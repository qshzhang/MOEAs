using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of VaEA and test problems can refer to the following paper
 * 
 *    Xiang Y, Zhou Y, Li M, et al. A Vector Angle-Based Evolutionary Algorithm for Unconstrained 
 *    Many-Objective Optimization[J]. IEEE Transactions on Evolutionary Computation, 2017, 21(1):131-152.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class VaEA : MultiObjectiveSolver
    {
        Random random = new Random();

        List<MoChromosome> matingpool = new List<MoChromosome>();

        List<double[]> unitVec = new List<double[]>();

        public void initial()
        {
            this.popsize = div;

            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            initialPopulation();
            initUnitVec();
        }

        protected void initialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.createChromosome();

                evaluate(chromosome);
                mainpop.Add(chromosome);
                updateReference(chromosome);
            }
        }

        private void initUnitVec()
        {
            for(int i = 0;i < this.numObjectives; i++)
            {
                double[] v = new double[this.numObjectives];
                for(int j = 0;j < this.numObjectives; j++)
                {
                    if (i == j) v[j] = 1;
                    else v[j] = 0;
                }
                unitVec.Add(v);
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
                if (mainpop[pos1].dominates(mainpop[pos2]))
                {
                    matingpool.Add(mainpop[pos1]);
                }
                else if (mainpop[pos2].dominates(mainpop[pos1]))
                {
                    matingpool.Add(mainpop[pos2]);
                }
                else
                {
                    if (random.NextDouble() < 0.5)
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

        private double[] TranObj(MoChromosome mo)
        {
            double[] v = new double[this.numObjectives];
            for (int i = 0; i < this.numObjectives; i++)
            {
                v[i] = (mo.objectivesValue[i] - this.idealpoint[i]) / (this.narpoint[i] - this.idealpoint[i]);
            }
            return v;
        }

        protected override void doSolve()
        {
            initial();
            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {
                //MatingSelection();
                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i);//GeneticOPDE//GeneticOPSBXCrossover
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

        private void EnviromentSelection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();
            List<List<MoChromosome>> dominatedSet0 = NSGA.fastNonDominatedSort(pop);

            computeFitness(ref pop);

            int cnt = 0;
            while (result.Count() + dominatedSet0[cnt].Count() <= this.popsize)
            {
                for (int r = 0; r < dominatedSet0[cnt].Count(); r++)
                {
                    dominatedSet0[cnt][r].selected = true;
                }

                result.AddRange(dominatedSet0[cnt]);
                cnt++;
            }
            if (result.Count() == this.popsize)
            {
                //return result;
                mainpop.Clear();
                mainpop.AddRange(result);
                return;
            }

            List<MoChromosome> Fl = new List<MoChromosome>();
            Fl.AddRange(dominatedSet0[cnt]);

            Boolean[] flag = new Boolean[Fl.Count];
            for (int i = 0; i < flag.Length; i++) flag[i] = false;

            Associate(ref flag, ref result, ref Fl);

            //List<double[]> P = GetVector(ref result, Fl);

            Niching(ref flag, ref result, Fl);

            mainpop.Clear();
            mainpop.AddRange(result);
        }

        private void Niching(ref Boolean[] flag, ref List<MoChromosome> result, List<MoChromosome> Fl)
        {
            while(result.Count < this.popsize)
            {
                int w = maxAngle(flag, Fl);
                int u = minAngle(flag, Fl);
                Maximum_Vector_Angle_First(w, ref flag, ref result, ref Fl);
                Worse_Elimination(u, ref flag, ref result, ref Fl);
            }
        }

        private void Maximum_Vector_Angle_First(int w, ref Boolean[] flag, ref List<MoChromosome> result, ref List<MoChromosome> Fl)
        {
            result.Add(Fl[w]);
            flag[w] = true;
            for(int i = 0;i < Fl.Count; i++)
            {
                if(flag[i] == false)
                {
                    double ag = Tool.getAngle(TranObj(Fl[i]), TranObj(Fl[w]));
                    if(ag < Fl[i].angle)
                    {
                        Fl[i].angle = ag;
                        Fl[i].subProbNo = result.Count - 1;
                    }
                }
            }
        }

        private void Worse_Elimination(int u, ref Boolean[] flag, ref List<MoChromosome> result, ref List<MoChromosome> Fl)
        {
            if(Fl[u].angle < Math.PI / (2 * (this.popsize + 1)))
            {
                int r = Fl[u].subProbNo;
                if (result[r].fitnessValue > Fl[u].fitnessValue && flag[u] == false)
                {
                    Fl[u].copyTo(result[r]);
                    flag[u] = true;
                    for (int i = 0; i < Fl.Count; i++)
                    {
                        if (flag[i] == false)
                        {
                            double ag = Tool.getAngle(TranObj(Fl[i]), TranObj(Fl[u]));
                            if (Fl[i].subProbNo != Fl[u].subProbNo)
                            {
                                if (ag < Fl[i].angle)
                                {
                                    Fl[i].angle = ag;
                                    Fl[i].subProbNo = r;
                                }
                            }
                            else
                            {
                                Fl[i].angle = ag;
                            }
                        }
                    }
                }
            }
            
        }

        private int maxAngle(Boolean[] flag, List<MoChromosome> Fl)
        {
            int pos = -1;
            double max = -1;
            for(int i = 0;i < Fl.Count; i++)
            {
                if(flag[i] == false && Fl[i].angle > max)
                {
                    max = Fl[i].angle;
                    pos = i;
                }
            }
            return pos;
        }

        private int minAngle(Boolean[] flag, List<MoChromosome> Fl)
        {
            int pos = -1;
            double min = 10;
            for (int i = 0; i < Fl.Count; i++)
            {
                if (flag[i] == false && Fl[i].angle < min)
                {
                    min = Fl[i].angle;
                    pos = i;
                }
            }
            return pos;
        }


        private void computeFitness(ref List<MoChromosome> F1)
        {
            for(int i = 0;i < F1.Count; i++)
            {
                F1[i].fitnessValue = Tool.ArraySum(TranObj(F1[i]));
            }
        }

        private void Associate(ref Boolean[] flag, ref List<MoChromosome> result, ref List<MoChromosome> Fl)
        {
            if(result.Count == 0)
            {
                //List<MoChromosome> li = Fl.OrderBy(r => r.fitnessValue).ToList();
                for(int i = 0;i < this.numObjectives; i++)
                {
                    double min = 10;
                    int pos = -1;
                    for(int j = 0;j < Fl.Count; j++)
                    {
                        double tp = Tool.getAngle(TranObj(Fl[j]), unitVec[i]);
                        if(tp < min)
                        {
                            min = tp;
                            pos = j;
                        }
                    }
                    result.Add(Fl[pos]);
                    flag[pos] = false;
                }

                pair[] li = new pair[Fl.Count];
                for(int i = 0; i < li.Length; i++)
                {
                    li[i] = new pair(i, Fl[i].fitnessValue);
                }

                li = li.OrderBy(r => r.val).ToArray();

                int cnt = 0;
                while(result.Count < 2 * this.numObjectives)
                {
                    if (flag[li[cnt].pos] == false)
                    {
                        result.Add(Fl[li[cnt].pos]);
                        flag[li[cnt].pos] = true;
                    }
                    cnt++;
                }
            }
            for(int i = 0;i < Fl.Count; i++)
            {
                if (flag[i] == true) continue;
                double minA = Double.MaxValue;
                int pos = -1;
                for(int j = 0;j < result.Count;j++)
                {
                    double tp = Tool.getAngle(TranObj(result[j]), TranObj(Fl[i]));
                    if(tp < minA)
                    {
                        minA = tp;
                        pos = j;
                    }
                }
                Fl[i].angle = minA;
                Fl[i].subProbNo = pos;
            }
        }
    }
}
