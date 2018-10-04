using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*
 * The details of NSGA-III and test problems can refer to the following paper
 * 
 *    Kalyanmoy Deb, Himanshu Jain, An evolutionary many-objective optimization algorithm using reference-point-based 
 *    nondominated sorting approach, part i: solving problems with box constraints, IEEE Trans. Evol. Comput. 18 (4) (2014) 577–601.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class NSGA3 : MultiObjectiveSolver
    {
        Random random = new Random();

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

        protected void initWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.popsize = this.weights.Count();
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

            string prob = mop.getName();
            if(prob.IndexOf("DTLZ") != -1)
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
                    offspring = SBXCrossover(i, true);//GeneticOPDE//GeneticOPSBXCrossover
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
            List<MoChromosome> result = new List<MoChromosome>();
            List<List<MoChromosome>> dominatedSet0 = NSGA.fastNonDominatedSort(pop);

            if (GlobalValue.IsNormalization)
            {
                updateNadirPoint(dominatedSet0[0]);
            }
            //updateNadirPoint(dominatedSet0[0]);

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

            int[] count = new int[popsize];
            for (int i = 0; i < popsize; i++) count[i] = 0;

            for (int i = 0; i < result.Count(); i++)
            {
                double dist = Double.MaxValue, dt;
                int pos = -1;
                for (int j = 0; j < this.weights.Count(); j++)
                {
                    dt = getAngle(j, result[i], GlobalValue.IsNormalization);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                count[pos]++;
            }

            List<List<MoChromosome>> associatedSolution;
            associatedSolution = new List<List<MoChromosome>>();
            for (int i = 0; i < this.weights.Count(); i++)
            {
                List<MoChromosome> temp = new List<MoChromosome>();
                associatedSolution.Add(temp);
            }

            for (int i = 0; i < dominatedSet0[cnt].Count(); i++)
            {
                double dist = Double.MaxValue, dt;
                int pos = -1;
                for (int j = 0; j < this.weights.Count(); j++)
                {
                    dt = getAngle(j, dominatedSet0[cnt][i], GlobalValue.IsNormalization);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                dominatedSet0[cnt][i].tchVal = pbiScalarObj(pos, dominatedSet0[cnt][i], GlobalValue.IsNormalization);//dist;
                dominatedSet0[cnt][i].subProbNo = pos;
                associatedSolution[pos].Add(dominatedSet0[cnt][i]);
            }

            for (int i = 0; i < this.weights.Count(); i++)
            {
                associatedSolution[i].Sort();
            }

            List<int> itr = new List<int>();
            while (result.Count < popsize)
            {
                itr.Clear();
                int min = popsize;
                for (int i = 0; i < popsize; i++)
                {
                    if (min > count[i] && associatedSolution[i].Count > 0)
                    {
                        itr.Clear();
                        min = count[i];
                        itr.Add(i);
                    }
                    else if (min == count[i] && associatedSolution[i].Count() > 0)
                    {
                        itr.Add(i);
                    }
                }

                int pos = random.Next(itr.Count());
                count[itr[pos]]++;
                result.Add(associatedSolution[itr[pos]][0]);
                associatedSolution[itr[pos]].RemoveAt(0);
            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;
        }
    }
}
