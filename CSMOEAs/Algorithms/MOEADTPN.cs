using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of MOEA/D-TPN and test problems can refer to the following paper
 * 
 *  Shouyong Jiang, Shengxiang Yang, An improved multiobjective optimization evolutionary algorithm based on 
 *  decomposition for complex pareto fronts, IEEE Trans. Cybern. 46 (2) (2016) 421–437
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class MOEADTPN : MultiObjectiveSolver
    {
        protected List<MoChromosome> exterSet;

        Random random = new Random();

        protected int nr = 2;

        protected Boolean isCave = true;

        protected void Initial()
        {
            exterSet = new List<MoChromosome>();

            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            InitWeight(this.div);
            InitialPopulation();
            InitNeighbour();
        }

        protected void InitNeighbour()
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

        protected void InitWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.GetTransweight();

            this.popsize = this.weights.Count();
        }

        
        protected void UpdateNeighbours(int i, MoChromosome offSpring)
        {
            int cnt = 0;
            for (int j = 0; j < this.neighbourSize; j++)
            {
                int weightindex = neighbourTable[i][j];
                MoChromosome sol = mainpop[weightindex];

                double d = UpdateCretia(weightindex, offSpring);
                double e = UpdateCretia(weightindex, sol);

                if (isCave == true)
                {
                    if (d < e)
                    {
                        offSpring.CopyTo(mainpop[weightindex]);
                        cnt++;
                    }
                }
                else
                {
                    if (d > e)
                    {
                        offSpring.CopyTo(mainpop[weightindex]);
                        cnt++;
                    }
                }

                if (cnt >= nr) break;
            }
        }

        protected double UpdateCretia(int problemIndex, MoChromosome chrom)
        {
            return NtechScalarObj(problemIndex, chrom);
        }

        protected double NtechScalarObj(int idx, MoChromosome var)
        {
            double[] namda = this.Transweights[idx];
            double max_fun = 0;

            if (isCave == true)
            {
                max_fun = -1 * Double.MaxValue;
                for (int n = 0; n < numObjectives; n++)
                {
                    double diff = (var.objectivesValue[n] - idealpoint[n]);
                    double feval;
                    if (namda[n] == 0)
                        feval = 0.00001 * diff;
                    else
                        feval = diff * namda[n];
                    if (feval > max_fun)
                        max_fun = feval;
                }
            }
            else
            {
                max_fun = Double.MaxValue;
                for (int n = 0; n < numObjectives; n++)
                {
                    double diff = (narpoint[n] - var.objectivesValue[n]);
                    double feval;
                    if (namda[n] == 0)
                        feval = 0.00001 * diff;
                    else
                        feval = diff * namda[n];
                    if (feval < max_fun)
                        max_fun = feval;
                }
            }
            return max_fun;
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
                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offSpring = SBXCrossover(i, true);//GeneticOPDE//GeneticOPSBXCrossover
                    this.Evaluate(offSpring);
                    UpdateNeighbours(i, offSpring);
                    UpdateReference(offSpring);
                    offSpring = null;
                }
                if (this.ItrCounter == 0.7 * this.TotalItrNum)
                {
                    isCave = isConcave();
                    //isCave = false;
                    if (isCave == false)
                    {
                        exterSet.Clear();
                        for (int i = 0; i < this.mainpop.Count; i++)
                        {
                            MoChromosome offSpring = this.CreateChromosome();
                            this.mainpop[i].CopyTo(offSpring);
                            exterSet.Add(offSpring);
                        }

                        for (int i = 0; i < this.weights.Count(); i++)
                        {
                            for (int j = 0; j < this.numObjectives; j++)
                            {
                                this.weights[i][j] = 1 - this.weights[i][j];
                            }
                        }
                        GetTransweight();
                        this.neighbourTable.Clear();
                        InitNeighbour();

                        for (int i = 0; i < popsize; i++)
                        {
                            for (int j = 0; j < this.numObjectives; j++)
                            {
                                narpoint[j] = 1;
                            }
                        }
                    }
                }

                if (this.ItrCounter % 10 == 0)
                {
                    List<MoChromosome> union = new List<MoChromosome>();
                    union.AddRange(mainpop);
                    union.AddRange(exterSet);
                    frm.refereshPlot(this.ItrCounter, union);
                    frm.Refresh();
                    igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
                }

                this.ItrCounter++;
            }
            mainpop.AddRange(exterSet);
            List<MoChromosome> result = new List<MoChromosome>();
            result.AddRange(mainpop);
            mainpop.Clear();
            mainpop.AddRange(NSGA.FastNonDominatedSort(result)[0]);

            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
            Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected Boolean isConcave()
        {
            double dm = 0, de = 0;
            int cm = 0, ce = 0;

            for (int i = 0; i < this.weights.Count(); i++)
            {
                if (IsBoundary(i) == true)
                {
                    de += GetGama(i);
                    ce++;
                }
                else
                {
                    dm += GetGama(i);
                    cm++;
                }
            }

            de /= ce;
            dm /= cm;

            if (dm < 0.9 * de) return false;
            return true;

        }

        protected Boolean IsBoundary(int pos)
        {
            double[] namda = this.weights[pos];
            double val = 1.0;
            for (int i = 0; i < this.numObjectives; i++) val *= namda[i];
            double conval = Math.Pow(1d / this.numObjectives, this.numObjectives);
            if (val < 0.5 * conval) return true;
            return false;
        }

        protected double GetGama(int pos)
        {
            double dist = 0;
            for (int i = 1; i < this.neighbourSize; i++)
            {
                dist += GetDist(mainpop[pos].objectivesValue,
                        mainpop[this.neighbourTable[pos][i]].objectivesValue);
            }
            return dist / (this.neighbourSize - 1);
        }

        protected double GetDist(double[] v1, double[] v2)
        {
            double dist = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dist += Math.Pow(v1[i] - idealpoint[i] - v2[i] - idealpoint[i], 2);
            }
            return Math.Sqrt(dist);
        }
    }
}
