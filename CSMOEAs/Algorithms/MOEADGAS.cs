using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of MOEADGAS and test problems can refer to the following paper
 * 
 * 基于分解思想的多目标进化算法研究[D].湖南大学,2018.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class MOEADGAS : MultiObjectiveSolver
    {
        Random random = new Random();
        //public List<MoChromosome> mainpop = new List<MoChromosome>();
        int topK = 5;

        protected void initial()
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
            initNeighbour();
        }

        protected void initNeighbour()
        {
            neighbourTable = new List<int[]>(popsize);

            double[,] distancematrix = new double[popsize, popsize];
            for (int i = 0; i < popsize; i++)
            {
                distancematrix[i, i] = 0;
                for (int j = i + 1; j < popsize; j++)
                {
                    distancematrix[i, j] = Distance(weights[i], weights[j]);
                    distancematrix[j, i] = distancematrix[i, j];
                }
            }

            for (int i = 0; i < popsize; i++)
            {
                double[] val = new double[popsize];
                for (int j = 0; j < popsize; j++)
                {
                    val[j] = distancematrix[i, j];
                }

                int[] index = Sorting.Sort(val);
                int[] array = new int[this.neighbourSize];
                Array.Copy(index, array, this.neighbourSize);
                neighbourTable.Add(array);
            }
        }

        protected void initialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.CreateChromosome();

                Evaluate(chromosome);
                mainpop.Add(chromosome);
                UpdateReference(chromosome);
            }
        }

        protected void initWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.GetTransweight();

            this.popsize = this.weights.Count();
        }

        protected override void DoSolve()
        {
            initial();

            string prob = mop.GetName();
            if (prob.IndexOf("DTLZ") != -1)
            {
                igdValue.Add(QulityIndicator.QulityIndicator.DTLZIGD(mainpop, prob, this.numObjectives));
            }
            else
            {
                pofData = FileTool.ReadData(pofPath + prob);
                igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
            }

            if (GlobalValue.IsShowProcess)
            {
                frm = new plotFrm(mainpop, mop.GetName());
                frm.Show();
                frm.Refresh();
            }
            while (!Terminated())
            {
                List<MoChromosome> offsPop = new List<MoChromosome>();
                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    if (GlobalValue.CrossoverType.Equals("SBX"))
                    {
                        offspring = SBXCrossover(i, true);//GeneticOPDE//GeneticOPSBXCrossover
                    }
                    else
                    {
                        offspring = DECrossover(i, true);
                    }
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
                    if (GlobalValue.IsShowProcess)
                    {
                        frm.refereshPlot(this.ItrCounter, mainpop);
                        frm.Refresh();
                    }

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


            Common.FileTool.WritetoFile(NSGA.NonDominatedSolutions(mainpop), "gen", 1);
            Common.FileTool.WritetoFile(NSGA.NonDominatedSolutions(mainpop), "obj", 2);
            Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected double updateCretia(int problemIndex, MoChromosome chrom)
        {
            if (GlobalValue.AggressionFunction.IndexOf("PBI") != -1)
                return PbiScalarObj(problemIndex, chrom, GlobalValue.IsNormalization);
            else if (GlobalValue.AggressionFunction.IndexOf("Weight") != -1)
                return WsScalarObj(problemIndex, chrom);
            else
                return TechScalarObj(problemIndex, chrom, GlobalValue.IsNormalization);
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            mainpop.Clear();

            if (GlobalValue.IsNormalization)
            {
                UpdateNadirPoint(NSGA.NonDominatedSolutions(pop));
            }

            List<List<PairRelation>> aggr = new List<List<PairRelation>>();
            for (int i = 0; i < popsize; i++)
            {
                List<PairRelation> li = new List<PairRelation>();
                for (int j = 0; j < pop.Count(); j++)
                {
                    PairRelation pair = new PairRelation(j, updateCretia(i, pop[j]));
                    li.Add(pair);
                }
                aggr.Add(li);
            }
            for (int i = 0; i < popsize; i++)
            {
                aggr[i] = aggr[i].OrderBy(r => r.val).ToList();
            }

            List<MoChromosome> result = new List<MoChromosome>();

            int cnt = 0;
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < pop.Count(); j++)
                {
                    if (j >= topK) break;
                    if (pop[aggr[i][j].pos].selected != true)
                    {
                        mainpop.Add(pop[aggr[i][j].pos]);
                        pop[aggr[i][j].pos].selected = true;
                        //pop[aggr[i][j].pos].crdistance = dist(pop.get(aggr[i][j].pos));
                        if (j == 0) cnt++;
                        result.Add(pop[aggr[i][j].pos]);
                        //cnt++;
                        break;
                    }
                }
            }


            if (result.Count() != popsize)
            {
                int[] count = new int[popsize];
                for (int i = 0; i < popsize; i++) count[i] = 0;
                for (int i = 0; i < result.Count(); i++)
                {
                    double min = Double.MaxValue;
                    int pos = -1;
                    double temp = 0;
                    for (int j = 0; j < popsize; j++)
                    {
                        temp = this.GetAngle(j, result[i], GlobalValue.IsNormalization); //getAngle1
                        if (temp < min)
                        {
                            min = temp;
                            pos = j;
                        }
                    }
                    Distance(this.idealpoint, this.narpoint);
                    result[i].angle = min;
                    count[pos]++;
                }
                List<int> minPos = new List<int>();
                while (result.Count() < popsize)
                {
                    minPos.Clear();
                    int min = count[0];
                    minPos.Add(0);
                    for (int i = 1; i < popsize; i++)
                    {
                        if (count[i] < min)
                        {
                            min = count[i];
                            minPos.Clear();
                            minPos.Add(i);
                        }
                        else if (count[i] == min)
                        {
                            minPos.Add(i);
                        }
                    }

                    int pos = minPos[random.Next(minPos.Count())];
                    double valmin = Double.MaxValue;
                    int _pos = -1;
                    for (int i = 0; i < pop.Count(); i++)
                    {
                        if (pop[i].selected == true) continue;
                        double tp = this.GetAngle(pos, pop[i], GlobalValue.IsNormalization); //getAngle1
                        if (tp < valmin)
                        {
                            valmin = tp;
                            _pos = i;
                        }
                    }
                    count[pos]++;
                    pop[_pos].selected = true;
                    pop[_pos].angle = valmin;
                    result.Add(pop[_pos]);
                }
            }

            mainpop.Clear();
            mainpop.AddRange(result);

            for (int i = 0; i < popsize; i++)
            {
                mainpop[i].selected = false;
            }
        }
    }
}

