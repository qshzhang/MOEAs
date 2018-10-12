using MOEAPlat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Encoding
{
    public class MoChromosome : IComparable<MoChromosome>
    {
        public static double EPS = 1.2e-7;
        private double F = 0.5;
        private double CR = 1;

        public double crdistance; //crodwing distance value
        public double[,] domainInfo;
        public int parDimension; //decision variable number
        public int objectDimension; //objective number
        public double[] realGenes; //decision variable
        public int[] realBGenes; //decision variable with binary code
        public double[] objectivesValue; //objective value
        //public double[] transObjectivesValue;
        public double[] cneqValue;
        public double[] ceqValue;
        public double fitnessValue;
        public double tchVal;
        public int subProbNo;
        public double angle;
        public Boolean selected = false;
        public int rank;

        public double FVg; //SPEAR used
        public double FVl; //SPEAR used

        public double APD; //RVEA used

        public double fpr; //BiGE used
        public double fcd; //GiGE used

        public static int id_cx = 30;
        public static int id_mu = 20;


        //public void randomizeParameter()
        //{

        //}

        public void setCrowdingDistance(double d)
        {
            this.crdistance = d;
        }

        public double getCrowdingDistance()
        {
            return this.crdistance;
        }

        public double getFitnessValue(int idx)
        {
            return this.objectivesValue[idx];
        }

        public string objVectorString()
        {
            string str = "";
            for(int i = 0;i < this.objectivesValue.Length; i++)
            {
                str += objectivesValue[i];
                str += ", ";
            }
            return str.Substring(0, str.Length - 2);
        }

        public string genVectorString()
        {
            string str = "";
            for (int i = 0; i < this.realGenes.Length; i++)
            {
                str += realGenes[i];
                str += ", ";
            }
            return str.Substring(0, str.Length - 2);
        }

        public Boolean dominates(MoChromosome another)
        {
            Boolean flag = false;
            for (int i = 0; i < objectDimension; i++)
            {
                if (this.objectivesValue[i] > another.objectivesValue[i]) return false;
                if (this.objectivesValue[i] < another.objectivesValue[i]) flag = true;
            }
            return flag;
        }

        public Boolean contrained_dominates(MoChromosome another)
        {
            Boolean m1 = NSGA.isFeasibleSolutions(this);
            Boolean m2 = NSGA.isFeasibleSolutions(another);

            if (m1 == true && m2 == false) return true;
            if (m1 == false && m2 == true) return false;
            if (m1 == false && m2 == false)
            {
                if (NSGA.getCVValue(this) < NSGA.getCVValue(another)) return true;
                else return false;
            }

            Boolean flag = false;
            for (int i = 0; i < objectDimension; i++)
            {
                if (this.objectivesValue[i] > another.objectivesValue[i]) return false;
                if (this.objectivesValue[i] < another.objectivesValue[i]) flag = true;
            }
            return flag;
        }

        public Boolean IsCrowdedComparisonOperatorBetter(MoChromosome otherIndividual)
        {
            if (getRank() < otherIndividual.getRank())
            {
                return true;
            }
            if (getRank() == otherIndividual.getRank()
                  && getCrowdingDistance() > otherIndividual.getCrowdingDistance())
            {
                return true;
            }

            return false;
        }

        public void copyTo(MoChromosome copyto)
        {
            //if(copyto.objectivesValue == null)
            //{
            //    copyto.objectivesValue = new double[this.objectDimension];
            //}
            //if(copyto.realGenes == null)
            //{
            //    copyto.realGenes = new double[this.parDimension];
            //}
            //if(copyto.domainInfo == null)
            //{
            //    copyto.domainInfo = new double[this.parDimension, 2];
            //}
            copyto.domainInfo = this.domainInfo;
            copyto.fitnessValue = this.fitnessValue;
            copyto.tchVal = this.tchVal;
            copyto.subProbNo = this.subProbNo;
            copyto.objectDimension = this.objectDimension;
            copyto.parDimension = this.parDimension;

            Array.Copy(this.domainInfo, 0, copyto.domainInfo, 0, 2 * this.parDimension);
            Array.Copy(this.objectivesValue, copyto.objectivesValue, copyto.objectivesValue.Length);
            Array.Copy(this.realGenes, copyto.realGenes, copyto.realGenes.Length);
            Array.Copy(this.ceqValue, copyto.ceqValue, copyto.ceqValue.Length);
            Array.Copy(this.cneqValue, copyto.cneqValue, copyto.cneqValue.Length);
        }

        //Real code
        public void mutate(double rate, Random random)
        {
            double rnd, delta1, delta2, mut_pow, deltaq;
            double y, yl, yu, val, xy;
            double eta_m = id_mu;

            //long tick = DateTime.Now.Ticks;
            //Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //Random random = new Random();
            for (int j = 0; j < parDimension; j++)
            {
                if (random.NextDouble() <= rate)
                {
                    y = realGenes[j];
                    yl = 0;
                    yu = 1;

                    delta1 = (y - yl) / (yu - yl);
                    delta2 = (yu - y) / (yu - yl);

                    rnd = random.NextDouble();
                    mut_pow = 1.0 / (eta_m + 1.0);
                    if (rnd <= 0.5)
                    {
                        xy = 1.0 - delta1;
                        val = 2.0 * rnd + (1.0 - 2.0 * rnd)
                                * (Math.Pow(xy, (eta_m + 1.0)));
                        deltaq = Math.Pow(val, mut_pow) - 1.0;
                    }
                    else
                    {
                        xy = 1.0 - delta2;
                        val = 2.0 * (1.0 - rnd) + 2.0 * (rnd - 0.5)
                                * (Math.Pow(xy, (eta_m + 1.0)));
                        deltaq = 1.0 - (Math.Pow(val, mut_pow));
                    }
                    y = y + deltaq * (yu - yl);
                    if (y < yl)
                        y = yl;
                    if (y > yu)
                        y = yu;
                    realGenes[j] = y;
                }
            }
            return;
        }

        public void setRank(int rank)
        {
            this.rank = rank;
        }

        public int getRank()
        {
            return rank;
        }

        //Real code
        public void SBX(MoChromosome p1, MoChromosome p2, Random random)
        {
            double rand;
            double y1, y2, yl, yu;
            double c1, c2;
            double alpha, beta, betaq;
            double eta_c = id_cx;

            MoChromosome parent1 = p1;
            MoChromosome parent2 = p2;
            int numVariables = p1.parDimension;

            //long tick = DateTime.Now.Ticks;
            //Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //Random random = new Random();

            if (random.NextDouble() <= 1.0)
            {
                for (int i = 0; i < numVariables; i++)
                {
                    if (random.NextDouble() <= 0.5)
                    {
                        if (Math.Abs(parent1.realGenes[i] - parent2.realGenes[i]) > EPS)
                        {
                            if (parent1.realGenes[i] < parent2.realGenes[i])
                            {
                                y1 = parent1.realGenes[i];
                                y2 = parent2.realGenes[i];
                            }
                            else
                            {
                                y1 = parent2.realGenes[i];
                                y2 = parent1.realGenes[i];
                            }
                            yl = 0;
                            yu = 1;
                            rand = random.NextDouble();
                            beta = 1.0 + (2.0 * (y1 - yl) / (y2 - y1));
                            alpha = 2.0 - Math.Pow(beta, -(eta_c + 1.0));
                            if (rand <= (1.0 / alpha))
                            {
                                betaq = Math.Pow((rand * alpha),
                                        (1.0 / (eta_c + 1.0)));
                            }
                            else
                            {
                                betaq = Math.Pow((1.0 / (2.0 - rand * alpha)),
                                        (1.0 / (eta_c + 1.0)));
                            }
                            c1 = 0.5 * ((y1 + y2) - betaq * (y2 - y1));
                            beta = 1.0 + (2.0 * (yu - y2) / (y2 - y1));
                            alpha = 2.0 - Math.Pow(beta, -(eta_c + 1.0));
                            if (rand <= (1.0 / alpha))
                            {
                                betaq = Math.Pow((rand * alpha),
                                        (1.0 / (eta_c + 1.0)));
                            }
                            else
                            {
                                betaq = Math.Pow((1.0 / (2.0 - rand * alpha)),
                                        (1.0 / (eta_c + 1.0)));
                            }
                            c2 = 0.5 * ((y1 + y2) + betaq * (y2 - y1));
                            if (c1 < yl)
                                c1 = yl;
                            if (c2 < yl)
                                c2 = yl;
                            if (c1 > yu)
                                c1 = yu;
                            if (c2 > yu)
                                c2 = yu;
                            if (random.NextDouble() <= 0.5)
                            {
                                realGenes[i] = c2;
                            }
                            else
                            {
                                realGenes[i] = c1;
                            }
                        }
                        else
                        {
                            realGenes[i] = parent1.realGenes[i];
                        }
                    }
                    else
                    {
                        realGenes[i] = parent1.realGenes[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < numVariables; i++)
                {
                    realGenes[i] = parent1.realGenes[i];
                }
            }
        }

        //Real code
        public void DE(MoChromosome p, MoChromosome p1, MoChromosome p2, Random random)
        {
            int D = parDimension;

            //long tick = DateTime.Now.Ticks;
            //Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //Random random = new Random();
            double jrandom = Math.Floor(random.NextDouble() * D);

            for (int index = 0; index < D; index++)
            {
                double value = 0;
                if (random.NextDouble() < CR || index == jrandom)
                    value = p.realGenes[index]
                            + F
                            * (p1.realGenes[index] - p2.realGenes[index]);
                else
                    value = p.realGenes[index];

                double high = 1;
                double low = 0;
                if (value > high)
                    value = high;
                else if (value < low)
                    value = low;

                realGenes[index] = value;
            }
        }

        //Binary code
        public void UniformCrossover(MoChromosome p1, MoChromosome p2, Random random)
        {
            for(int i = 0;i < this.parDimension; i++)
            {
                if(random.NextDouble() < 0.5)
                {
                    realBGenes[i] = p1.realBGenes[i];
                }
                else
                {
                    realBGenes[i] = p2.realBGenes[i];
                }
            }
        }

        //Binary code
        public void Bmutate(Random random)
        {
            for (int i = 0; i < this.parDimension; i++)
            {
                if(random.NextDouble() < 1.0 / this.parDimension)
                {
                    realBGenes[i] = realBGenes[i] == 0 ? 1 : 0;
                }
            }
        }

        public Boolean equals(Object obj)
        {
            if (!this.Equals(obj))
			    return false;
            if (this == obj)
                return true;
            MoChromosome another = (MoChromosome)obj;
            Boolean equals = true;
            for (int i = 0; i < realGenes.Length; i++)
            {
                if (Math.Abs(another.realGenes[i] - this.realGenes[i]) > 1000 * Double.MinValue)
                    equals = false;
            }
            return equals;
        }

        public int CompareTo(MoChromosome o)
        {
            if (this.tchVal < o.tchVal) return -1;
            if (this.tchVal > o.tchVal) return 1;

            return 0;
        }
    }
}
