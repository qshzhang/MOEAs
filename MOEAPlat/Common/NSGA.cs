using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class NSGA
    {
        public static Boolean isFeasibleSolutions(MoChromosome mo)
        {
            for(int i = 0;i < mo.cneqValue.Length; i++)
            {
                if (mo.cneqValue[i] > 0) return false;
            }
            for(int j = 0;j < mo.ceqValue.Length; j++)
            {
                if (Math.Abs(mo.ceqValue[j]) > 1e-6) return false;
            }
            return true;
        }

        public static double getCVValue(MoChromosome mo)
        {
            double cv = 0;
            foreach(double e in mo.cneqValue)
            {
                cv += (e <= 0? 0 : e);
            }
            foreach(double e in mo.ceqValue)
            {
                cv += Math.Abs(e);
            }
            return cv;
        }

        public static List<List<MoChromosome>> fastConstrainedNonDominatedSort(List<MoChromosome> individuals)
        {
            List<List<int>> dominationFronts = new List<List<int>>();

            Dictionary<int, List<int>> individual2DominatedIndividuals =
               new Dictionary<int, List<int>>();
            Dictionary<int, int> individual2NumberOfDominatingIndividuals =
               new Dictionary<int, int>();


            int s = 0, t = 0;
            foreach (MoChromosome individualP in individuals)
            {
                individual2DominatedIndividuals.Add(s, new List<int>());
                individual2NumberOfDominatingIndividuals.Add(s, 0);

                t = 0;
                foreach (MoChromosome individualQ in individuals)
                {
                    if (individualP.contrained_dominates(individualQ))
                    {
                        individual2DominatedIndividuals[s].Add(t);
                    }
                    else
                    {
                        if (individualQ.contrained_dominates(individualP))
                        {
                            individual2NumberOfDominatingIndividuals[s] =
                                  individual2NumberOfDominatingIndividuals[s] + 1;
                        }
                    }
                    t++;
                }

                if (individual2NumberOfDominatingIndividuals[s] == 0)
                {
                    // p belongs to the first front
                    individualP.setRank(1);
                    if (dominationFronts.Count == 0)
                    {
                        List<int> firstDominationFront = new List<int>();
                        firstDominationFront.Add(s);
                        dominationFronts.Add(firstDominationFront);
                    }
                    else
                    {
                        //	            	List<MoChromosome> firstDominationFront = dominationFronts.get(0);
                        //	               firstDominationFront.add(individualP);
                        dominationFronts[0].Add(s);
                    }
                }
                s++;
            }

            int i = 1;
            while (true)
            {
                List<int> nextDominationFront = new List<int>();
                foreach (int individualP in dominationFronts[i - 1])
                {
                    foreach (int individualQ in individual2DominatedIndividuals[individualP])
                    {
                        individual2NumberOfDominatingIndividuals[individualQ] =
                              individual2NumberOfDominatingIndividuals[individualQ] - 1;
                        if (individual2NumberOfDominatingIndividuals[individualQ] == 0)
                        {
                            individuals[individualQ].setRank(i + 1);
                            nextDominationFront.Add(individualQ);
                        }
                    }
                }
                i++;
                if (nextDominationFront.Count != 0)
                {
                    dominationFronts.Add(nextDominationFront);
                }
                else
                {
                    break;
                }
            }

            List<List<MoChromosome>> frontSet = new List<List<MoChromosome>>();
            for (int x = 0; x < dominationFronts.Count(); x++)
            {
                List<MoChromosome> it = new List<MoChromosome>();
                for (int v = 0; v < dominationFronts[x].Count(); v++)
                {
                    individuals[dominationFronts[x][v]].rank = x + 1;
                    it.Add(individuals[dominationFronts[x][v]]);
                }
                frontSet.Add(it);
            }
            return frontSet;
        }



        public static List<List<MoChromosome>> fastNonDominatedSort(List<MoChromosome> individuals)
        {
            List<List<int>> dominationFronts = new List<List<int>>();

            Dictionary<int, List<int>> individual2DominatedIndividuals =
               new Dictionary<int, List<int>>();
            Dictionary<int, int> individual2NumberOfDominatingIndividuals =
               new Dictionary<int, int>();


            int s = 0, t = 0;
            foreach (MoChromosome individualP in individuals)
            {
                individual2DominatedIndividuals.Add(s, new List<int>());
                individual2NumberOfDominatingIndividuals.Add(s, 0);

                t = 0;
                foreach (MoChromosome individualQ in individuals)
                {
                    if (individualP.dominates(individualQ))
                    {
                        individual2DominatedIndividuals[s].Add(t);
                    }
                    else
                    {
                        if (individualQ.dominates(individualP))
                        {
                            individual2NumberOfDominatingIndividuals[s] =
                                  individual2NumberOfDominatingIndividuals[s] + 1;
                        }
                    }
                    t++;
                }

                if (individual2NumberOfDominatingIndividuals[s] == 0)
                {
                    // p belongs to the first front
                    individualP.setRank(1);
                    if (dominationFronts.Count == 0)
                    {
                        List<int> firstDominationFront = new List<int>();
                        firstDominationFront.Add(s);
                        dominationFronts.Add(firstDominationFront);
                    }
                    else
                    {
                        //	            	List<MoChromosome> firstDominationFront = dominationFronts.get(0);
                        //	               firstDominationFront.add(individualP);
                        dominationFronts[0].Add(s);
                    }
                }
                s++;
            }

            int i = 1;
            while (true)
            {
                List<int> nextDominationFront = new List<int>();
                foreach (int individualP in dominationFronts[i - 1])
                {
                    foreach (int individualQ in individual2DominatedIndividuals[individualP])
                    {
                        individual2NumberOfDominatingIndividuals[individualQ] = 
                              individual2NumberOfDominatingIndividuals[individualQ] - 1;
                        if (individual2NumberOfDominatingIndividuals[individualQ] == 0)
                        {
                            individuals[individualQ].setRank(i + 1);
                            nextDominationFront.Add(individualQ);
                        }
                    }
                }
                i++;
                if (nextDominationFront.Count != 0)
                {
                    dominationFronts.Add(nextDominationFront);
                }
                else
                {
                    break;
                }
            }

            List<List<MoChromosome>> frontSet = new List<List<MoChromosome>>();
            for (int x = 0; x < dominationFronts.Count(); x++)
            {
                List<MoChromosome> it = new List<MoChromosome>();
                for (int v = 0; v < dominationFronts[x].Count(); v++)
                {
                    individuals[dominationFronts[x][v]].rank = x + 1;
                    it.Add(individuals[dominationFronts[x][v]]);
                }
                frontSet.Add(it);
            }
            return frontSet;
        }

        public static List<MoChromosome> NonDominatedSolutions(List<MoChromosome> list)
        {
            List<MoChromosome> result = new List<MoChromosome>();
            for(int i = 0;i < list.Count; i++)
            {
                Boolean flag = true;
                for(int j = 0;j < list.Count; j++)
                {
                    if (i == j) continue;
                    if (list[j].dominates(list[i]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == true) result.Add(list[i]);
            }
            return result;
        }


        public static void crowdingDistanceAssignment(List<MoChromosome> individuals)
        {

            foreach (MoChromosome individual in individuals)
            {
                // initialize crowding distance
                individual.setCrowdingDistance(0);
            }

            int numberOfObjectives = individuals[0].objectDimension;
            for (int m = 0; m < numberOfObjectives; m++)
            {
                MoChromosome[] sortedIndividuals = individuals.ToArray();

                // sort using m-th objective value
                Array.Sort(sortedIndividuals, new FitnessValueComparator(m));

                // so that boundary points are always selected
                sortedIndividuals[0].setCrowdingDistance(Double.MaxValue);
                sortedIndividuals[sortedIndividuals.Length - 1].setCrowdingDistance(Double.MaxValue);

                // If minimal and maximal fitness value for this objective are equal,
                // do not change crowding distance 
                if (sortedIndividuals[0].getFitnessValue(m) != sortedIndividuals[sortedIndividuals.Length - 1].getFitnessValue(m))
                {
                    for (int i = 1; i < sortedIndividuals.Length - 1; i++)
                    {
                        double newCrowdingDistance = sortedIndividuals[i].getCrowdingDistance();
                        newCrowdingDistance +=
                           (sortedIndividuals[i + 1].getFitnessValue(m) - sortedIndividuals[i - 1].getFitnessValue(m))
                           / (sortedIndividuals[sortedIndividuals.Length - 1].getFitnessValue(m) - sortedIndividuals[0].getFitnessValue(m));

                        sortedIndividuals[i].setCrowdingDistance(newCrowdingDistance);
                    }
                }
            }
        }

        private class FitnessValueComparator : IComparer<MoChromosome> {


              public int indexObjective;

              public FitnessValueComparator(int indexObjective)
              {
                this.indexObjective = indexObjective;
              }

            public int Compare(MoChromosome individual1, MoChromosome individual2)
            {
                if (individual1.getFitnessValue(indexObjective) < individual2.getFitnessValue(indexObjective))
                {
                    return -1;
                }
                if (individual1.getFitnessValue(indexObjective) > individual2.getFitnessValue(indexObjective))
                {
                    return 1;
                }
                return 0;
            }
        }

        public static MoChromosome[] sort(List<MoChromosome> individuals)
        {
            MoChromosome[] result = individuals.ToArray();

            Array.Sort(result, new CrowdedComparisonOperatorComparator());

            return result;
        }

        private class CrowdedComparisonOperatorComparator : IComparer<MoChromosome>
        {

            public int Compare(MoChromosome individual1, MoChromosome individual2)
            {
                if (individual1.IsCrowdedComparisonOperatorBetter(individual2))
                {
                    return -1;
                }
                if (individual2.IsCrowdedComparisonOperatorBetter(individual1))
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}
