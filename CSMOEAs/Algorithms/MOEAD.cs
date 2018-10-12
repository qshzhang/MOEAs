using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of MOEA/D and test problems can refer to the following paper
 * 
 * Zhang Q, Li H. MOEA/D: A Multiobjective Evolutionary Algorithm Based on Decomposition[M]. IEEE Press, 2007.
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class MOEAD : MultiObjectiveSolver
    {
        Random random = new Random();
        //public List<MoChromosome> mainpop = new List<MoChromosome>();


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

            double[,] distancematrix = new double[popsize,popsize];
            for (int i = 0; i < popsize; i++)
            {
                distancematrix[i,i] = 0;
                for (int j = i + 1; j < popsize; j++)
                {
                    distancematrix[i,j] = distance(weights[i], weights[j]);
                    distancematrix[j,i] = distancematrix[i,j];
                }
            }

            for (int i = 0; i < popsize; i++)
            {
                double[] val = new double[popsize];
                for(int j = 0;j < popsize; j++)
                {
                    val[j] = distancematrix[i, j];
                }
                
                int[] index = Sorting.sorting(val);
                int[] array = new int[this.neighbourSize];
                Array.Copy(index,array,this.neighbourSize);
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

            this.getTransweight();

            this.popsize = this.weights.Count();
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
            if (GlobalValue.AggressionFunction.IndexOf("PBI") != -1)
                return pbiScalarObj(problemIndex, chrom);
            else if (GlobalValue.AggressionFunction.IndexOf("Weight") != -1)
                return wsScalarObj(problemIndex, chrom);
            else
                return techScalarObj(problemIndex, chrom);
        }

        protected override void doSolve()
        {
            initial();
            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {
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
                    this.evaluate(offspring);
                    updateReference(offspring);
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
