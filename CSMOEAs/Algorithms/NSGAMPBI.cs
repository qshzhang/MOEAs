using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of NSGA-MPBI and test problems can refer to the following paper
 * 
 *  Zhang Q, Zhu W, Liao B, et al. A modified PBI approach for multi-objective optimization with complex 
 *  Pareto fronts[J]. Swarm & Evolutionary Computation, 2018,  https://doi.org/10.1016/j.swevo.2018.02.001
 * 
 * If you have any questions about the codes, please contact  
 * Qisheng Zhang  at qshzhang@yeah.net 
*/

namespace MOEAPlat.Algorithms
{
    public class NSGAMPBI : MultiObjectiveSolver
    {
        private int H;
        private int baseNum;

        double threshold;

        public double Dintercept;
        public double[] sd;

        double Mr = 0.7;
        double fr = 0.05;

        Random random = new Random();
        


        protected void initial()
        {
            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];
            sd = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            initWeight(this.div);
            initialPopulation();
            initNeighbour();
            threshold = getThreshold(this.weights);
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
                    distancematrix[i, j] = distance(weights[i], weights[j]);
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

                int[] index = Sorting.sorting(val);
                int[] array = new int[this.neighbourSize];
                Array.Copy(index, array, this.neighbourSize);
                neighbourTable.Add(array);
            }
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

        protected void initWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.popsize = this.weights.Count();
            this.baseNum = this.popsize;
        }

        protected double getThreshold(List<double[]> wt)
        {
            //double min = Double.MaxValue;
            //for(int i = 0; i < wt.Count(); i++)
            //{
            //    for(int j = i + 1; j < wt.Count; j++)
            //    {
            //        double tp = distance(wt[i], wt[j]);
            //        if (min > tp) min = tp;
            //    }
            //}

            double[] center = new double[this.numObjectives];
            for (int i = 0; i < this.numObjectives; i++) center[i] = 1.0 / this.numObjectives;

            pair[] pr = new pair[wt.Count];
            for(int i = 0;i < wt.Count; i++)
            {
                pr[i] = new pair(i, -1);
            }
            for(int i = 0;i < wt.Count; i++)
            {
                pr[i].val = distance(center, wt[i]);
            }

            Array.Sort(pr);

            int len = pr.Length - 1;
            double[] ct = new double[this.numObjectives];
            for (int i = 0; i < this.numObjectives; i++) ct[i] = 0.0;
            for(int i = 0;i < this.numObjectives; i++)
            {
                for(int j = 0;j < this.numObjectives; j++)
                {
                    ct[j] += wt[pr[len - i].pos][j];
                }
            }
            for (int i = 0; i < this.numObjectives; i++) ct[i] /= this.numObjectives;


            return distance(ct, wt[pr[len-0].pos]);
        }


        protected double igdScalarObj(int idx, MoChromosome var)
        {
            double[] namda = this.weights[idx];

            double[] weight = new double[this.numObjectives];
            for (int i = 0; i < weight.Length; i++) weight[i] = 1;

            double[] obj = new double[this.numObjectives];

            for (int i = 0; i < this.numObjectives; i++)
            {
                obj[i] = var.objectivesValue[i] - this.idealpoint[i] - sd[i] -(namda[i] * Dintercept);
            }
            return pbiScalarObj(weight, obj);
        }

        protected double pbiScalarObj(double[] namda, double[] obj)
        {
            double lenv = 0, mul = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                mul += obj[i] * namda[i];
                lenv += Math.Pow(namda[i], 2);
            }
            double d1 = mul / Math.Sqrt(lenv);

            double d2 = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                d2 += Math.Pow(obj[i] - d1 * namda[i] / Math.Sqrt(lenv), 2);//
            }
            return d1 + 5 * Math.Sqrt(d2);
        }

        void updateSD(List<MoChromosome> pop)
        {
            List<MoChromosome> temp = NSGA.fastNonDominatedSort(pop)[0];
            getDintercept(temp);
            for (int i = 0; i < this.numObjectives; i++) sd[i] = Double.MaxValue;
            for (int i = 0; i < temp.Count(); i++)
            {
                double[] tp = projPoint(temp[i]);
                for (int j = 0; j < this.numObjectives; j++)
                {
                    if (sd[j] > tp[j]) sd[j] = tp[j];
                }
            }
        }
        double[] projPoint(MoChromosome var)
        {
            double[] result = new double[this.numObjectives];
            double sum = 0.0;
            for (int j = 0; j < this.numObjectives; j++)
            {
                sum += var.objectivesValue[j] - this.idealpoint[j];
            }
            for (int j = 0; j < this.numObjectives; j++)
            {
                result[j] = var.objectivesValue[j] - this.idealpoint[j] - (sum - Dintercept) / this.numObjectives;
            }
            return result;
        }

        void getDintercept(List<MoChromosome> pop)
        {
            Dintercept = Double.MaxValue;
            for (int i = 0; i < pop.Count(); i++)
            {
                double tp = 0.0;
                for (int j = 0; j < this.numObjectives; j++)
                {
                    tp += pop[i].objectivesValue[j] - this.idealpoint[j];
                }
                if (Dintercept > tp) Dintercept = tp;
            }
        }

        void updateDintercept()
        {
            double sum = 0.0;
            for (int i = 0; i < this.numObjectives; i++) sum += sd[i];
            Dintercept -= sum;
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();
            //List<List<MoChromosome>> dominatedSet0 = NSGA.fastNonDominatedSort(pop);
            List<List<MoChromosome>> dominatedSet0 = NSGA.fastConstrainedNonDominatedSort(pop);

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
                mainpop.Clear();
                mainpop.AddRange(result);
                return;
            }

            if(this.ItrCounter < Mr * this.TotalItrNum)
            {
                updateSD(pop);
                updateDintercept();
            }


            List<List<MoChromosome>> associatedSolution = clustering(dominatedSet0[cnt]);

            for (int i = 0; i < this.weights.Count(); i++)
            {
                associatedSolution[i].Sort();
            }

            cnt = 0;
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

                Boolean[] flag = new Boolean[temp.Count()];
                for (int i = 0; i < flag.Length; i++) flag[i] = false;
                while (result.Count() < popsize)
                {
                    int pos = random.Next(temp.Count());
                    while (flag[pos] == true)
                    {
                        pos = random.Next(temp.Count());
                    }

                    flag[pos] = true;
                    result.Add(temp[pos]);
                }

            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;
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
                    dt = igdScalarObj(j, pop[i]);
                    //dt = getAngle(dominatedSet0[cnt][i].objectivesValue, this.weights[j]);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                pop[i].tchVal = igdScalarObj(pos, pop[i]);
                pop[i].subProbNo = pos;
                associatedSolution[pos].Add(pop[i]);
            }
            return associatedSolution;
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

            if (GlobalValue.IsShowProcess)
            {
                frm = new plotFrm(mainpop, mop.getName());
                frm.Show();
                frm.Refresh();
            }
            while (!terminated())
            {
                List<MoChromosome> offsPop = new List<MoChromosome>();
                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    if (GlobalValue.CrossoverType.Equals("SBX"))
                    {
                        offspring = SBXCrossover(i,true);//GeneticOPDE//GeneticOPSBXCrossover
                    }
                    else
                    {
                        offspring = DECrossover(i,true);
                    }
                    this.evaluate(offspring);
                    offsPop.Add(offspring);
                    updateReference(offspring);
                }

                List<MoChromosome> Pop = new List<MoChromosome>();
                Pop.AddRange(mainpop);
                Pop.AddRange(offsPop);

                EnviromentSelection(Pop);

                if (this.ItrCounter >= Mr * this.TotalItrNum && this.ItrCounter % ((int)(this.TotalItrNum * fr)) == 0)
                {
                    adjustReferencePoints();
                }
                if(this.ItrCounter % 10 == 0)
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
                this.ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
            Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected void adjustReferencePoints()
        {
            List<double[]> wNew = new List<double[]>();

            List<List<MoChromosome>> associatedSolution = clustering(mainpop);
            

            List<double[]> wNew1 = new List<double[]>();

            for (int i = 0; i < this.weights.Count(); i++)
            {
                //sort1(associatedSolution[i]);
                if (associatedSolution[i].Count() > 0) wNew.Add(this.weights[i]);
                else wNew1.Add(this.weights[i]);
            }

            if (wNew.Count() > 0.9 * popsize) return;

            List<double[]> result = new List<double[]>();
            List<double[]> temp = new List<double[]>();


            int n = getH((int)(popsize * baseNum * 1.0 / wNew.Count()), this.numObjectives);
            temp = UniPointsGenerator.getMUniDistributedPoint(numObjectives, n);


            baseNum = temp.Count();

            double dt = threshold;
            threshold = getThreshold(temp);

            for (int i = 0; i < temp.Count(); i++)
            {
                double min = Double.MaxValue;
                for (int j = 0; j < wNew.Count(); j++)
                {
                    double tp2 = distance(temp[i], wNew[j]);
                    if (tp2 < min) min = tp2;
                }

                if (min <= dt)
                {
                    result.Add(temp[i]);
                }
            }
            weights.Clear();
            weights.AddRange(result);
            H = n;
        }

        protected double getAngle(double[] v1, double[] v2)
        {
            double sum = 0;
            double len1 = 0;
            double len2 = 0;
            for (int i = 0; i < v2.Length; i++)
            {
                sum += v1[i] - this.idealpoint[i] - sd[i] - v2[i] * Dintercept;
                len1 += Math.Pow(v1[i] - this.idealpoint[i] - sd[i] - v2[i] * Dintercept, 2);
                len2 += 1;
            }
            return Math.Acos(sum / (Math.Sqrt(len1 * len2)));
        }

        private int getH(int n, int m)
        {
            int H = 1;
            int len = 1;
            for (int i = 1; i < m; i++) len *= i;
            int len1 = 1;
            while (true)
            {
                len1 = 1;
                for (int i = H; i < H + m - 1; i++) len1 *= i;
                if (len1 / len > n) break;
                H++;
            }
            return H;
        }
    }
}
