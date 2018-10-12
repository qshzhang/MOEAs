using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*
 * The details of RVEA and test problems can refer to the following paper
 * 
 *    Ran Cheng, Yaochu Jin, Markus Olhofer, Bernhard Sendhoff, A reference vector guided evolutionary algorithm 
 *    for many-objective optimization, IEEE Trans. Evol. Comput. 20 (5) (2016) 773–791
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class RVEA : MultiObjectiveSolver
    {
        Random random = new Random();

        double thetam;
        double alpha = 2;
        double fr = 0.1;

        List<double[]> Vectors;

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
            inittheta();
        }

        private void inittheta()
        {
            double[] arr = new double[this.popsize];
            for (int i = 0; i < this.popsize; i++)
            {
                double min = Double.MaxValue;
                for (int j = 0; j < popsize; j++)
                {
                    if (i == j) continue;
                    double tp = Tool.getAngle(weights[i], weights[j]);
                    if (tp < min) min = tp;
                }
                arr[i] = min;
            }
            thetam = Tool.ArrayMax(arr);
        }

        protected void initWeight(int m)
        {
            this.weights = new List<double[]>();
            if (numObjectives < 6) this.weights = UniPointsGenerator.getMUniDistributedPoint(numObjectives, m);
            else this.weights = UniPointsGenerator.getMaUniDistributedPoint(numObjectives, m, 2);

            for(int i = 0; i < this.weights.Count; i++)
            {
                double std = Tool.VectorLen(weights[i]);
                for(int j = 0;j < this.numObjectives; j++)
                {
                    this.weights[i][j] /= std;
                }
            }

            this.popsize = this.weights.Count();

            Vectors = new List<double[]>();
            Vectors.AddRange(weights);
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

            //string prob = mop.getName();
            //if (prob.IndexOf("DTLZ") != -1)
            //{
            //    igdValue.Add(QulityIndicator.QulityIndicator.DTLZIGD(mainpop, prob, this.numObjectives));
            //}
            //else
            //{
            //    pofData = FileTool.readData(pofPath + prob);
            //    igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
            //}


            frm = new plotFrm(mainpop, mop.getName());
            frm.Show();
            frm.Refresh();
            while (!terminated())
            {

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

                Reference_Vector_Guided_Selection(Pop);
                Reference_Vector_Adaptation();
                //Reference_Vector_Adaptation(Pop);

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();

                    //if (prob.IndexOf("DTLZ") != -1)
                    //{
                    //    igdValue.Add(QulityIndicator.QulityIndicator.DTLZIGD(mainpop, prob, this.numObjectives));
                    //}
                    //else
                    //{
                    //    igdValue.Add(QulityIndicator.QulityIndicator.IGD(mainpop, pofData));
                    //}
                }

                ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
            //Common.FileTool.WritetoFile(igdValue, "igdCurve");
        }

        protected void Reference_Vector_Adaptation()
        {

            if (this.ItrCounter % ((int)(fr * this.TotalItrNum)) != 0) return;

            double[] max = new double[this.numObjectives];

            for (int i = 0; i < max.Length; i++) max[i] = Double.MinValue;

            foreach (MoChromosome mo in mainpop)
            {
                for (int i = 0; i < mo.objectDimension; i++)
                {
                    if (mo.objectivesValue[i] > max[i])
                    {
                        max[i] = mo.objectivesValue[i];
                    }
                }
            }
            Vectors.Clear();
            for (int i = 0; i < this.popsize; i++)
            {
                double[] v = new double[this.numObjectives];
                for (int j = 0; j < this.numObjectives; j++)
                {
                    v[j] = weights[i][j] * (max[j] - this.idealpoint[j]);
                }
                double sum = Tool.VectorLen(v);
                for (int j = 0; j < this.numObjectives; j++)
                {
                    v[j] /= sum;
                }
                Vectors.Add(v);
            }
        }

        protected void Reference_Vector_Adaptation(List<MoChromosome> pop)
        {
            if (this.ItrCounter % ((int)(fr * this.TotalItrNum)) != 0) return;

            double[] max = new double[this.numObjectives];

            for (int i = 0; i < max.Length; i++) max[i] = Double.MinValue;

            foreach (MoChromosome mo in pop)
            {
                for (int i = 0; i < mo.objectDimension; i++)
                {
                    if (mo.objectivesValue[i] > max[i])
                    {
                        max[i] = mo.objectivesValue[i];
                    }
                }
            }

            List<List<MoChromosome>> associatedSolution = clustering(pop);

            List<double[]> vec = new List<double[]>();

            //Vectors.Clear();
            
            for (int i = 0; i < this.popsize; i++)
            {
                double[] v = new double[this.numObjectives];
                if (associatedSolution[i].Count > 0)
                {
                    vec.Add(Vectors[i]);
                    continue;
                }
                for (int j = 0; j < this.numObjectives; j++)
                {
                    v[j] = random.NextDouble() * max[j];
                }
                double sum = Tool.VectorLen(v);
                for (int j = 0; j < this.numObjectives; j++)
                {
                    v[j] /= sum;
                }
                vec.Add(v);
            }
            Vectors.Clear();
            Vectors.AddRange(vec);
        }

        protected void Reference_Vector_Guided_Selection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();

            List<List<MoChromosome>> associatedSolution = clustering(pop);

            for(int i = 0;i < this.weights.Count; i++)
            {
                for(int j = 0;j < associatedSolution[i].Count; j++)
                {
                    double p = this.numObjectives * 1.0 * Math.Pow(this.ItrCounter * 1.0 / this.TotalItrNum, alpha) * (associatedSolution[i][j].angle / thetam);
                    associatedSolution[i][j].APD = (1 + p) * ObjectiveLen(associatedSolution[i][j]);
                }
                associatedSolution[i] = associatedSolution[i].OrderBy(r => r.APD).ToList();
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
                temp = temp.OrderBy(r => r.fitnessValue).ToList();

                int pos = 0;
                while (result.Count() < popsize)
                {
                    result.Add(temp[pos]);
                    pos++;
                }
            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;

        }

        protected double getAngle(double[] namda, MoChromosome var, Boolean flag = false)
        {
            double mul = 0.0;
            double a = 0.0;
            double b = 0.0;
            for (int i = 0; i < this.numObjectives; i++)
            {
                if(flag == true)
                {
                    mul += namda[i] * (var.objectivesValue[i] - this.idealpoint[i]);
                    mul += namda[i] * ((var.objectivesValue[i] - this.idealpoint[i])/(this.narpoint[i] - this.idealpoint[i]));
                }
                else
                {
                    b += Math.Pow(var.objectivesValue[i] - this.idealpoint[i], 2);
                }
                
                a += Math.Pow(namda[i], 2);
            }
            return Math.Acos(mul / (Math.Sqrt(a * b)));
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
                    dt = getAngle(Vectors[j], pop[i], GlobalValue.IsNormalization);
                    if (dt < dist)
                    {
                        dist = dt;
                        pos = j;
                    }
                }
                pop[i].angle = dist;
                pop[i].subProbNo = pos;
                associatedSolution[pos].Add(pop[i]);
            }
            return associatedSolution;
        }
    }
}
