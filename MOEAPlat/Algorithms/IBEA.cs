using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of IBEA and test problems can refer to the following paper
 * 
 * Zitzler E, Künzli S. Indicator-Based Selection in Multiobjective Search[J]. 
 * Lecture Notes in Computer Science, 2004, 3242:832-842.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class IBEA : MultiObjectiveSolver
    {
        Random random = new Random();

        protected List<List<Double>> indicatorValues;
        protected double maxIndicatorValue;

        //public List<MoChromosome> mainpop = new List<MoChromosome>();

        public void initial()
        {
            this.popsize = div;
            initialPopulation();
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
            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {

                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i,false);//GeneticOPDE//GeneticOPSBXCrossover
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
                }

                ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();

            //List<List<MoChromosome>> dominatedSet0 = NSGA.fastNonDominatedSort(pop);

            //int cnt = 0;
            //while (result.Count() + dominatedSet0[cnt].Count() <= this.popsize)
            //{
            //    for (int r = 0; r < dominatedSet0[cnt].Count(); r++)
            //    {
            //        dominatedSet0[cnt][r].selected = true;
            //    }

            //    result.AddRange(dominatedSet0[cnt]);
            //    cnt++;
            //}
            //if (result.Count() == this.popsize)
            //{
            //    //return result;
            //    mainpop.Clear();
            //    mainpop.AddRange(result);
            //    return;
            //}

            //result.AddRange(dominatedSet0[cnt]);

            result.AddRange(pop);

            calculateFitness(ref result);

            while (result.Count() > this.popsize)
            {
                removeWorst(ref result);
            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;
        }

        public void calculateFitness(ref List<MoChromosome> solutionSet)
        {
            // Obtains the lower and upper bounds of the population
            double[] maximumValues = new double[this.numObjectives];
            double[] minimumValues = new double[this.numObjectives];

            for (int i = 0; i < this.numObjectives; i++)
            {
                maximumValues[i] = Double.MinValue;
                minimumValues[i] = Double.MaxValue;
            }

            foreach (MoChromosome solution in solutionSet)
            {
                for (int obj = 0; obj < this.numObjectives; obj++)
                {
                    double value = solution.objectivesValue[obj];
                    if (value > maximumValues[obj])
                    {
                        maximumValues[obj] = value;
                    }
                    if (value < minimumValues[obj])
                    {
                        minimumValues[obj] = value;
                    }
                }
            }

            computeIndicatorValuesHD(solutionSet, maximumValues, minimumValues);
            for (int pos = 0; pos < solutionSet.Count(); pos++)
            {
                fitness(ref solutionSet, pos);
            }
        }

        public void fitness(ref List<MoChromosome> solutionSet, int pos)
        {
            double fitness = 0.0;
            double kappa = 0.05;

            for (int i = 0; i < solutionSet.Count; i++)
            {
                if (i != pos)
                {
                    fitness += Math.Exp((-1 * indicatorValues[i][pos] / maxIndicatorValue) / kappa);
                }
            }
            solutionSet[pos].fitnessValue = fitness;
        }

        public void computeIndicatorValuesHD(List<MoChromosome> solutionSet, double[] maximumValues,
                                            double[] minimumValues)
        {
            MoChromosome A, B;
            // Initialize the structures
            indicatorValues = new List<List<Double>>();
            maxIndicatorValue = -Double.MaxValue;

            for (int j = 0; j < solutionSet.Count(); j++)
            {
                A = this.createChromosome();
                solutionSet[j].copyTo(A);

                List<Double> aux = new List<Double>();
                foreach (MoChromosome solution in solutionSet)
                {
                    B = this.createChromosome();
                    solution.copyTo(B);

                    Boolean flag = A.dominates(B);

                    double value;
                    if (flag == true) //false
                    {
                        value =
                            -calculateHypervolumeIndicator(A, B, this.numObjectives,
                                maximumValues, minimumValues);
                    }
                    else
                    {
                        value = calculateHypervolumeIndicator(B, A, this.numObjectives,
                            maximumValues, minimumValues);
                    }

                    //Update the max value of the indicator
                    if (Math.Abs(value) > maxIndicatorValue)
                    {
                        maxIndicatorValue = Math.Abs(value);
                    }
                    aux.Add(value);
                }
                indicatorValues.Add(aux);
            }
        }

        double calculateHypervolumeIndicator(MoChromosome solutionA, MoChromosome solutionB, int d,
                                                double[] maximumValues, double[] minimumValues)
        {
            double a, b, r, max;
            double volume;
            double rho = 2.0;

            r = rho * (maximumValues[d - 1] - minimumValues[d - 1]);
            max = minimumValues[d - 1] + r;

            a = solutionA.objectivesValue[d - 1];
            if (solutionB == null)
            {
                b = max;
            }
            else
            {
                b = solutionB.objectivesValue[d - 1];
            }

            if (d == 1)
            {
                if (a < b)
                {
                    volume = (b - a) / r;
                }
                else
                {
                    volume = 0;
                }
            }
            else
            {
                if (a < b)
                {
                    volume =
                        calculateHypervolumeIndicator(solutionA, null, d - 1, maximumValues, minimumValues) * (b
                            - a) / r;
                    volume +=
                        calculateHypervolumeIndicator(solutionA, solutionB, d - 1, maximumValues, minimumValues)
                            * (max - b) / r;
                }
                else
                {
                    volume =
                        calculateHypervolumeIndicator(solutionA, solutionB, d - 1, maximumValues, minimumValues)
                            * (max - a) / r;
                }
            }

            return (volume);
        }

        public void removeWorst(ref List<MoChromosome> solutionSet)
        {
            // Find the worst;
            double worst = solutionSet[0].fitnessValue;
            int worstIndex = 0;
            double kappa = 0.05;

            for (int i = 1; i < solutionSet.Count(); i++)
            {
                if (solutionSet[i].fitnessValue > worst)
                {
                    worst = solutionSet[i].fitnessValue;
                    worstIndex = i;
                }
            }

            // Update the population
            for (int i = 0; i < solutionSet.Count(); i++)
            {
                if (i != worstIndex)
                {
                    double fitness = solutionSet[i].fitnessValue;
                    fitness -= Math.Exp((-indicatorValues[worstIndex][i] / maxIndicatorValue) / kappa);
                    solutionSet[i].fitnessValue = fitness;
                }
            }

            // remove worst from the indicatorValues list
            indicatorValues.RemoveAt(worstIndex);
            foreach (List<Double> anIndicatorValues_ in indicatorValues)
            {
                anIndicatorValues_.RemoveAt(worstIndex);
            }

            solutionSet.RemoveAt(worstIndex);
        }

    }
}
