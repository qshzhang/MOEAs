using MOEAPlat.Common;
using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.QulityIndicator
{
    public static class QulityIndicator
    {

        private static List<double[]> GetList(List<MoChromosome> pop)
        {
            List<double[]> list = new List<double[]>();
            foreach (MoChromosome mo in pop)
            {
                list.Add(mo.objectivesValue);
            }
            return list;
        }

        public static double IGD(List<MoChromosome> solution, List<double[]> trueFront)
        {
            return IGD(GetList(solution), trueFront);
        }

        public static double HV(List<MoChromosome> solution, double[] refPoint, double[] miniPoin)
        {
            return HV(GetList(solution), refPoint, miniPoin);
        }

        private static double IGD(List<double[]> solution, List<double[]> trueFront)
        {
            double sum = 0.0;
            foreach (double[] v in trueFront)
            {
                int pos = -1;
                double dist = Double.MaxValue;
                for(int i = 0;i < solution.Count; i++)
                {
                    double d = QulityIndicatorToolFunction.getDist(v, solution[i]);
                    if(d < dist)
                    {
                        dist = d;
                        pos = i;
                    }
                }
                sum += dist;
            }
            return sum / trueFront.Count;
        }

        private static double HV(List<double[]> solution, double[] refPoint, double[] miniPoint)
        {
            int numberOfObjectives = refPoint.Length;
            /**
	     * Stores the maximum values of true pareto front.
	     */
            double[] maximumValues = refPoint;

            /**
             * Stores the minimum values of the true pareto front.
             */
            double[] minimumValues = miniPoint;

            /**
             * Stores the normalized front.
             */
            List<double[]> normalizedFront;

            /**
             * Stores the inverted front. Needed for minimization problems
             */
            List<double[]> invertedFront;

            // STEP 1. Obtain the maximum and minimum values of the Pareto front
            //maximumValues = QulityIndicatorToolFunction.getMaximumValues(trueFront, numberOfObjectives);
            //minimumValues = QulityIndicatorToolFunction.getMinimumValues(trueFront, numberOfObjectives);

            // STEP 2. Get the normalized front
            normalizedFront = QulityIndicatorToolFunction.getNormalizedFront(solution,
                                                        maximumValues,
                                                        minimumValues);

            // STEP 3. Inverse the pareto front. This is needed because of the original
            //metric by Zitzler is for maximization problems
            invertedFront = QulityIndicatorToolFunction.invertedFront(normalizedFront);

            // STEP4. The hypervolumen (control is passed to java version of Zitzler code)
            return QulityIndicatorToolFunction.calculateHypervolume(invertedFront, invertedFront.Count, numberOfObjectives);
        }

        public static double DTLZIGD(List<MoChromosome> solution, string prob, int objs)
        {
            List<double[]> list = GetList(solution);
            return DTLZIGD(list, prob, objs);
        }

        private static double DTLZIGD(List<double[]> solution, string prob, int objs)
        {
            List<double[]> list = new List<double[]>();
            int div = 0;
            switch (objs)
            {
                case 3:
                    div = 12;
                    break;
                case 5:
                    div = 6;
                    break;
                case 8:
                    div = 3;
                    break;
                case 10:
                    div = 3;
                    break;
                case 15:
                    div = 2;
                    break;
            }
            
            if (objs < 6) list = UniPointsGenerator.getMUniDistributedPoint(objs, div);
            else list = UniPointsGenerator.getMaUniDistributedPoint(objs, div, 2);

            List<double[]> front = new List<double[]>();

            if (prob.IndexOf("DTLZ1") != -1)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    double[] arr = new double[objs];
                    double sum = 0.0;
                    for (int j = 0; j < objs; j++)
                    {
                        sum += list[i][j];
                    }
                    for (int j = 0; j < objs; j++)
                    {
                        arr[j] = 0.5 * list[i][j] / sum;
                    }
                    front.Add(arr);
                }
            }
            else
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    double[] arr = new double[objs];
                    double sum = 0.0;
                    for (int j = 0; j < objs; j++)
                    {
                        sum += list[i][j] * list[i][j];
                    }
                    sum = Math.Sqrt(sum);
                    for (int j = 0; j < objs; j++)
                    {
                        arr[j] = list[i][j] / sum;
                    }
                    front.Add(arr);
                }
            }
            return IGD(solution, front);
        }
    }
}
