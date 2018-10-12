using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using MOEAPlat.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Algorithms
{
    public abstract class MultiObjectiveSolver
    {
        protected MultiObjectiveProblem mop;
        protected int ItrCounter = 1;
        public int TotalItrNum;
        public int div;
        protected int popsize;
        public int neighbourSize;
        protected int numObjectives;
        protected int parDimension;

        protected int cneqNum;
        protected int ceqNum;

        protected string pofPath = "APOF\\";

        protected List<int[]> neighbourTable;

        public double[] idealpoint;
        public double[] narpoint;
        protected List<double[]> weights;

        protected List<double[]> Transweights;

        protected Boolean stoped = false;

        public List<double> igdValue = new List<double>();

        public List<double[]> pofData = new List<double[]>();

        public List<MoChromosome> mainpop = new List<MoChromosome>();

        Random random = new Random();

        protected plotFrm frm;

        public void stop()
        {
            stoped = true;
        }

        public void solve(MultiObjectiveProblem problem)
        {
            this.setMultiObjectiveProblem(problem);
            this.numObjectives = problem.getObjectiveSpaceDimension();
            this.parDimension = problem.getParameterSpaceDimension();
            this.ceqNum = problem.getCEqNum();
            this.cneqNum = problem.getCNeqNum();
            this.doSolve();
            this.stoped = true;
        }
        public void setMultiObjectiveProblem(MultiObjectiveProblem problem)
        {
            this.mop = problem;
        }

        public void evaluate(MoChromosome chromosmoe)
        {
            MultiObjectiveProblem multiObjectiveProblem = mop;
            multiObjectiveProblem.evaluate(chromosmoe);
        }

        public MoChromosome createChromosome(int type = 0) //ref MoChromosome chromosome
        {
            MoChromosome chromosome = new MoChromosome();
            chromosome.parDimension = this.parDimension;
            chromosome.objectDimension = this.numObjectives;
            chromosome.objectivesValue = new double[this.numObjectives];
            chromosome.domainInfo = new double[this.parDimension, 2];
            chromosome.ceqValue = new double[this.ceqNum];
            chromosome.cneqValue = new double[this.cneqNum];

            //long tick = DateTime.Now.Millisecond;
            //Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            Random rm = new Random(random.Next());

            if(type == 0)
            {
                chromosome.realGenes = new double[this.parDimension];
                for (int i = 0; i < chromosome.realGenes.Length; i++)
                {
                    chromosome.realGenes[i] = rm.NextDouble();
                }
            }
            else
            {
                chromosome.realBGenes = new int[this.parDimension];
                for (int i = 0; i < chromosome.realGenes.Length; i++)
                {
                    chromosome.realBGenes[i] = rm.Next()%2;
                }
            }
            return chromosome;
        }

        protected Boolean terminated()
        {
            // condition on the iteration.
            return (this.ItrCounter > this.TotalItrNum);
        }

        public double ObjectiveLen(MoChromosome mo)
        {
            double[] arr = new double[this.numObjectives];
            for(int i = 0;i < this.numObjectives; i++)
            {
                arr[i] = mo.objectivesValue[i] - this.idealpoint[i];
            }
            return Tool.VectorLen(arr);
        }

        public double distance(double[] weight1, double[] weight2)
        {
            double sum = 0;
            for (int i = 0; i < weight1.Length; i++)
            {
                sum += Math.Pow((weight1[i] - weight2[i]), 2);
            }
            return Math.Sqrt(sum);
        }

        protected double getParDist(int idx, MoChromosome var)
        {

            double[] namda = this.weights[idx];
            double lenv = 0, mul = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                mul += ((var.objectivesValue[i] - this.idealpoint[i]) / (narpoint[i] + 1e-5 - idealpoint[i])) * namda[i];
                lenv += Math.Pow(namda[i], 2);
            }
            double d1 = mul / Math.Sqrt(lenv);

            double d2 = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                d2 += Math.Pow(((var.objectivesValue[i] - this.idealpoint[i]) / (narpoint[i] + 1e-5 - idealpoint[i])) - d1 * namda[i] / Math.Sqrt(lenv), 2);
            }
            return Math.Sqrt(d2);
        }

        protected void getTransweight()
        {
            if (this.Transweights == null)
                this.Transweights = new List<double[]>();
            else
                this.Transweights.Clear();
            for (int i = 0; i < this.weights.Count(); i++)
            {
                double[] arr = new double[numObjectives];
                for (int j = 0; j < numObjectives; j++)
                {
                    double tp = this.weights[i][j];
                    if (Math.Abs(tp) < 0.00000001)
                    {
                        arr[j] = 1.0e+6;
                    }
                    else
                    {
                        arr[j] = 1 / tp;
                    }
                }
                double sum = 0;
                for (int j = 0; j < numObjectives; j++)
                {
                    sum += arr[j];
                }
                for (int j = 0; j < numObjectives; j++)
                {
                    arr[j] = arr[j] / sum;
                }
                this.Transweights.Add(arr);
            }
        }


        protected double getAngle(int idx, MoChromosome var, Boolean flag = false)
        {
            double[] namda = this.weights[idx];
            double mul = 0.0;
            double a = 0.0;
            double b = 0.0;
            for(int i = 0;i < this.numObjectives; i++)
            {
                if(flag == true)
                {
                    mul += ((var.objectivesValue[i] - this.idealpoint[i]) / (narpoint[i] - idealpoint[i] + 1e-5)) * namda[i];
                    b += Math.Pow((var.objectivesValue[i] - this.idealpoint[i]) / (narpoint[i] - idealpoint[i] + 1e-5), 2);
                }
                else
                {
                    mul += namda[i] * (var.objectivesValue[i] - this.idealpoint[i]);
                    b += Math.Pow(var.objectivesValue[i] - this.idealpoint[i] + 1e-5, 2);
                }
                a += Math.Pow(namda[i], 2);
            }
            return Math.Acos(mul / (Math.Sqrt(a * b)));
        }

        protected double techScalarObj(int idx, MoChromosome var, Boolean flag = false)
        {
            double[] namda = this.Transweights[idx];
            double max_fun = -1 * Double.MaxValue;
            for (int n = 0; n < numObjectives; n++)
            {
                double diff = 0.0; 
                if(flag == true)
                {
                    diff = Math.Abs((var.objectivesValue[n] - idealpoint[n])/(this.narpoint[n] - this.idealpoint[n]));
                }
                else
                {
                    diff = Math.Abs(var.objectivesValue[n] - idealpoint[n]);
                }
                double feval;
                if (namda[n] == 0)
                    feval = 0.00001 * diff;
                else
                    feval = diff * namda[n];
                if (feval > max_fun)
                    max_fun = feval;
            }
            return max_fun;
        }

        protected double wsScalarObj(int idx, MoChromosome var)
        {
            double[] namda = this.weights[idx];
            double sum = 0;
            for (int n = 0; n < numObjectives; n++)
            {
                sum += (namda[n]) * (var.objectivesValue[n] - this.idealpoint[n]);
            }
            return sum;
        }

        protected double pbiScalarObj(int idx, MoChromosome var, Boolean flag = false)
        {

            double[] namda = this.weights[idx];
            double lenv = 0, mul = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                if(flag == true)
                {
                    mul += ((var.objectivesValue[i] - this.idealpoint[i])/(narpoint[i] - idealpoint[i] + 1e-5))*namda[i];
                }
                else
                {
                    mul += (var.objectivesValue[i] - this.idealpoint[i]) * namda[i];
                }
                lenv += Math.Pow(namda[i], 2);
            }
            double d1 = mul / Math.Sqrt(lenv);

            double d2 = 0;
            for (int i = 0; i < numObjectives; i++)
            {
                if (flag == true)
                {
                    d2 += Math.Pow(((var.objectivesValue[i] - this.idealpoint[i]) / (narpoint[i] - idealpoint[i] + 1e-5)) - d1 * namda[i] / Math.Sqrt(lenv), 2);
                }
                else
                {
                    d2 += Math.Pow(var.objectivesValue[i] - this.idealpoint[i] - d1 * namda[i] / Math.Sqrt(lenv), 2);
                }  
            }
            return d1 + 5 * Math.Sqrt(d2);
        }

        protected void updateReference(MoChromosome indiv)
        {//update ideal point
            for (int j = 0; j < indiv.objectivesValue.Length; j++)
            {
                if (indiv.objectivesValue[j] < idealpoint[j])
                    idealpoint[j] = indiv.objectivesValue[j];

                if (indiv.objectivesValue[j] > narpoint[j])
                {
                    narpoint[j] = indiv.objectivesValue[j];
                }
            }

        }

        protected void updateNadirPoint(List<MoChromosome> list)
        {
            List<int> li = new List<int>();
            for(int i = 0;i < this.numObjectives; i++)
            {
                double min = Double.MaxValue;
                int pos = -1;

                double[] w = new double[this.numObjectives];
                for(int j = 0; j < this.numObjectives; j++)
                {
                    w[j] = i == j ? 1 : 1e-6;
                }

                for(int j = 0;j < list.Count; j++)
                {
                    double max = Double.MinValue;
                    for(int r = 0;r < this.numObjectives; r++)
                    {
                        double tp = (list[j].objectivesValue[r] - this.idealpoint[r]) / w[r];
                        if (max < tp) max = tp;
                    }
                    if(min > max)
                    {
                        min = max;
                        pos = j;
                    }
                }
                li.Add(pos);
            }

            double[,] arr = new double[this.numObjectives, this.numObjectives];
            for(int i = 0;i < this.numObjectives; i++)
            {
                for(int j = 0;j < this.numObjectives; j++)
                {
                    arr[i, j] = list[li[i]].objectivesValue[j] - this.idealpoint[j];
                }
            }
            double[,] imatrix = Matrix.IMatrix(arr, this.numObjectives); //Matrix.InverseMatrix(arr);
            double[] u = new double[this.numObjectives];
            for (int i = 0; i < this.numObjectives; i++) u[i] = 1;
            double[] result = Matrix.MatrixMultiple(imatrix, u);

            if(result == null || (result != null && !Tool.isSatisfy(result)))
            {
                for (int i = 0; i < this.numObjectives; i++) this.narpoint[i] = Double.MinValue;
                for(int i = 0;i < this.numObjectives; i++)
                {
                    for(int j = 0;j < list.Count; j++)
                    {
                        if (list[j].objectivesValue[i] > this.narpoint[i]) this.narpoint[i] = list[j].objectivesValue[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.numObjectives; i++)
                {
                    this.narpoint[i] = 1.0 / result[i] + this.idealpoint[i];
                }
            }


        }

        public List<double[]> getObjective()
        {
            List<double[]> objs = new List<double[]>();
            foreach(MoChromosome mo in mainpop)
            {
                double[] obj = new double[this.numObjectives];
                Array.Copy(mo.objectivesValue, obj, this.numObjectives);
                objs.Add(obj);
            }
            return objs;
        }

        protected double GetObjSum(MoChromosome mo)
        {
            double sum = 0.0;
            foreach(double e in mo.objectivesValue)
            {
                sum += e;
            }
            return sum;
        }

        protected MoChromosome DECrossover(int i, Boolean flag = false)
        {//Differential Evolution
            int k, l;
            if(flag == true && random.NextDouble()<0.5)
            {
                do
                    k = neighbourTable[i][random.Next(this.neighbourSize)];
                while (k == i);
                do
                    l = neighbourTable[i][random.Next(this.neighbourSize)];
                while (l == k || l == i);
            }
            else
            {
                do
                    k = random.Next(0, popsize - 1);
                while (k == i);
                do
                    l = random.Next(0, popsize - 1);
                while (l == k || l == i);
            }



            MoChromosome chromosome1 = mainpop[k];
            MoChromosome chromosome2 = mainpop[l];

            // generic operation crossover and mutation.
            MoChromosome offSpring = this.createChromosome();
            MoChromosome current = mainpop[i];

            offSpring.DE(current, chromosome1, chromosome2, random);

            offSpring.mutate(1d / offSpring.parDimension, random);
            return offSpring;
        }

        protected MoChromosome SBXCrossover(int i, Boolean flag = false)
        {//SBXCrossover
            int k = 0;
            if (flag == true && random.NextDouble() < 0.5)
            {
                do
                    k = neighbourTable[i][random.Next(this.neighbourSize)];
                while (k == i);
            }
            else
            {
                do
                    k = random.Next(this.popsize);
                while (k == i);
            }

            MoChromosome offSpring = this.createChromosome();
            offSpring.SBX(this.mainpop[i], mainpop[k], random);

            offSpring.mutate(1d / offSpring.parDimension, random);

            offSpring.selected = false;
            //offSpring.mutate(this.randomGenerator, 1d/this.popsize);
            return offSpring;
        }

        abstract protected void doSolve();
    }
}
