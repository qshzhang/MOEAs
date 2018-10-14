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

        Common.Point2 P1 = new Common.Point2();
        Common.Point2 P2 = new Common.Point2();


        Random random = new Random();
        protected void Initial()
        {
            this.popsize = div;
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

        protected void InitWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            this.popsize = this.weights.Count();
        }

        protected void InitialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.CreateChromosome();

                Evaluate(chromosome);
                UpdateReference(chromosome);
                UpdatePoint(chromosome);
                mainpop.Add(chromosome);
            }
        }

        protected void UpdateNeighbours(int i, MoChromosome offSpring)
        {
            for (int j = 0; j < this.neighbourSize; j++)
            {
                int weightindex = neighbourTable[i][j];
                MoChromosome sol = mainpop[weightindex];

                double d = UpdateCretia(weightindex, offSpring);
                double e = UpdateCretia(weightindex, sol);
                if (d < e)
                    offSpring.CopyTo(mainpop[weightindex]);
            }
        }

        protected double UpdateCretia(int problemIndex, MoChromosome chrom)
        {
            return UTechScalarObj(problemIndex, chrom);
        }

        protected double UTechScalarObj(int idx, MoChromosome var)
        {
            double max_fun = -1 * Double.MaxValue;

            double alpha = (double)(this.popsize - idx) * 1d / (this.popsize - 1);

            Common.Point2 r = new Common.Point2();
            r.P_X = alpha * P1.P_X + (1 - alpha) * P2.P_X;
            r.P_Y = alpha * P1.P_Y + (1 - alpha) * P2.P_Y;

            max_fun = Math.Max(lambda2 * (var.objectivesValue[0] - r.P_X), lambda1 * (var.objectivesValue[1] - r.P_Y));
            return max_fun;
        }

        protected void UpdatePoint(MoChromosome indiv)
        {
         // update the idealpoint.
            
            if (P1.P_X > indiv.objectivesValue[0])
            {
                P1.P_X = indiv.objectivesValue[0];
                P1.P_Y = indiv.objectivesValue[1];
            }
            if (P2.P_Y > indiv.objectivesValue[1])
            {
                P2.P_X = indiv.objectivesValue[0];
                P2.P_Y = indiv.objectivesValue[1];
            }
            if (this.ItrCounter > 0.9 * this.TotalItrNum)
            {
                P1.P_X = 0;
                P1.P_Y = 1;
                P2.P_X = 1;
                P2.P_Y = 0;
            }

            lambda1 = Math.Abs(P1.P_X - P2.P_X);
            lambda2 = Math.Abs(P1.P_Y - P2.P_Y);
        }

        protected override void DoSolve()
        {
            Initial();
            frm = new plotFrm(mainpop, mop.GetName());
            frm.Show();
            frm.Refresh();
            while (!Terminated())
            {
                for(int i = 0;i < this.popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i);//GeneticOPDE//GeneticOPSBXCrossover
                    this.Evaluate(offspring);
                    UpdateReference(offspring);
                    UpdatePoint(offspring);
                    UpdateNeighbours(i, offspring);
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
