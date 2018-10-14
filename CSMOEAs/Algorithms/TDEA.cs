using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of Theta-DEA and test problems can refer to the following paper
 * 
 *     Yuan Yuan, Hua Xu, Bo Wang, Xin Yao, A new dominance relation-based evolutionary algorithm for 
 *     many-objective optimization, IEEE Trans. Evol. Comput. 20 (1) (2016) 16–37
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class TDEA : MultiObjectiveSolver
    {
        Random random = new Random();

        //public List<MoChromosome> mainpop = new List<MoChromosome>();

        public void Initial()
        {
            this.idealpoint = new double[this.numObjectives];
            this.narpoint = new double[this.numObjectives];

            for (int i = 0; i < numObjectives; i++)
            {
                idealpoint[i] = Double.MaxValue;
                narpoint[i] = Double.MinValue;
            }

            InitWeight(this.div);
            InitialPopulation();
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
                mainpop.Add(chromosome);
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

                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i);//GeneticOPDE//GeneticOPSBXCrossover
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
            List<MoChromosome> result = new List<MoChromosome>();

            List<List<MoChromosome>> associatedSolution = Clustering(pop);

            for (int i = 0; i < this.weights.Count(); i++)
            {
                associatedSolution[i].Sort();
            }

            int cnt = 0;
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

        protected List<List<MoChromosome>> Clustering(List<MoChromosome> pop)
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
                    dt = GetAngle(j, pop[i], GlobalValue.IsNormalization);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                pop[i].tchVal = PbiScalarObj(pos, pop[i], GlobalValue.IsNormalization);
                pop[i].subProbNo = pos;
                associatedSolution[pos].Add(pop[i]);
            }
            return associatedSolution;
        }

    }
}
