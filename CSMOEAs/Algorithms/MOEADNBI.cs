using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/*
 * The details of MOEA/D-NBI and test problems can refer to the following paper
 * 
 * Zhang Q, Li H, Maringer D, et al. MOEA/D with NBI-style Tchebycheff approach for 
 * portfolio management[C]// Evolutionary Computation. IEEE, 2010:1-8.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class MOEADNBI : MultiObjectiveSolver
    {
        double lambda1 = 1.0, lambda2 = 1.0;

        point P1 = new point();
        point P2 = new point();


        Random random = new Random();
        protected void initial()
        {
            this.popsize = div;
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
                updatePoint(chromosome);
                mainpop.Add(chromosome);
            }
        }

        protected void updateNeighbours(int i, MoChromosome offSpring)
        {
            for (int j = 0; j < this.neighbourSize; j++)
            {
                int weightindex = neighbourTable[i][j];
                MoChromosome sol = mainpop[weightindex];

                double d = updateCretia(weightindex, offSpring);
                double e = updateCretia(weightindex, sol);
                if (d < e)
                    offSpring.copyTo(mainpop[weightindex]);
            }
        }

        protected double updateCretia(int problemIndex, MoChromosome chrom)
        {
            return utechScalarObj(problemIndex, chrom);
        }

        protected double utechScalarObj(int idx, MoChromosome var)
        {
            double max_fun = -1 * Double.MaxValue;

            double alpha = (double)(this.popsize - idx) * 1d / (this.popsize - 1);

            point r = new point();
            r.f1 = alpha * P1.f1 + (1 - alpha) * P2.f1;
            r.f2 = alpha * P1.f2 + (1 - alpha) * P2.f2;

            max_fun = Math.Max(lambda2 * (var.objectivesValue[0] - r.f1), lambda1 * (var.objectivesValue[1] - r.f2));
            return max_fun;
        }

        protected void updatePoint(MoChromosome indiv)
        {
         // update the idealpoint.
            
            if (P1.f1 > indiv.objectivesValue[0])
            {
                P1.f1 = indiv.objectivesValue[0];
                P1.f2 = indiv.objectivesValue[1];
            }
            if (P2.f2 > indiv.objectivesValue[1])
            {
                P2.f1 = indiv.objectivesValue[0];
                P2.f2 = indiv.objectivesValue[1];
            }
            if (this.ItrCounter > 0.9 * this.TotalItrNum)
            {
                P1.f1 = 0;
                P1.f2 = 1;
                P2.f1 = 1;
                P2.f2 = 0;
            }

            lambda1 = Math.Abs(P1.f1 - P2.f1);
            lambda2 = Math.Abs(P1.f2 - P2.f2);
        }

        protected override void doSolve()
        {
            initial();
            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {
                for(int i = 0;i < this.popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i);//GeneticOPDE//GeneticOPSBXCrossover
                    this.evaluate(offspring);
                    updateReference(offspring);
                    updatePoint(offspring);
                    updateNeighbours(i, offspring);
                    offspring = null;
                }

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();
                }

                this.ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
        }
    }
}
