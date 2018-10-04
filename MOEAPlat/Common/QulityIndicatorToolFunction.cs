using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class QulityIndicatorToolFunction
    {
        public static double getDist(double[] v1, double[] v2)
        {
            double dist = 0;
            for(int i = 0;i < v1.Length; i++)
            {
                dist += Math.Pow(v1[i] - v2[i], 2);
            }
            return Math.Sqrt(dist);
        }

        public static double[] getMaximumValues(List<double[]> front, int noObjectives)
        {
            double[] maximumValue = new double[noObjectives];
            for (int i = 0; i < noObjectives; i++)
                maximumValue[i] = Double.MinValue;


            foreach (double[] aFront in front)
            {
                for (int j = 0; j < aFront.Length; j++)
                {
                    if (aFront[j] > maximumValue[j])
                        maximumValue[j] = aFront[j];
                }
            }

            return maximumValue;
        } // getMaximumValues

        public static double[] getMinimumValues(List<double[]> front, int noObjectives)
        {
            double[] minimumValue = new double[noObjectives];
            for (int i = 0; i < noObjectives; i++)
                minimumValue[i] = Double.MaxValue;

            foreach (double[] aFront in front)
            {
                for (int j = 0; j < aFront.Length; j++)
                {
                    if (aFront[j] < minimumValue[j])
                        minimumValue[j] = aFront[j];
                }
            }
            return minimumValue;
        } // getMinimumValues

        public static List<double[]> getNormalizedFront(List<double[]> front,
                                            double[] maximumValue,
                                            double[] minimumValue)
        {

            List<double[]> normalizedFront = new List<double[]>();

            for (int i = 0; i < front.Count; i++)
            {
                double[] arr = new double[maximumValue.Length];
                for (int j = 0; j < front[i].Length; j++)
                {
                    arr[j] = (front[i][j] - minimumValue[j]) /
                                            (maximumValue[j] - minimumValue[j]);
                }
                normalizedFront.Add(arr);
            }
            return normalizedFront;
        } // getNormalizedFront

        public static List<double[]> invertedFront(List<double[]> front)
        {
            List<double[]> invertedFront = new List<double[]>();

            for (int i = 0; i < front.Count; i++)
            {
                double[] arr = new double[front[0].Length];

                for (int j = 0; j < front[i].Length; j++)
                {
                    if (front[i][j] <= 1.0 && front[i][j] >= 0.0)
                    {
                        arr[j] = 1.0 - arr[j];
                    }
                    else if (arr[j] > 1.0)
                    {
                        arr[j] = 0.0;
                    }
                    else if (arr[j] < 0.0)
                    {
                        arr[j] = 1.0;
                    }
                }
                invertedFront.Add(arr);
            }
            return invertedFront;
        } // invertedFront

        public static double calculateHypervolume(List<double[]> front, int noPoints, int noObjectives)
        {
            int n;
            double volume, distance;

            volume = 0;
            distance = 0;
            n = noPoints;
            while (n > 0)
            {
                int noNondominatedPoints;
                double tempVolume, tempDistance;

                noNondominatedPoints = filterNondominatedSet(front, n, noObjectives - 1);
                //noNondominatedPoints = front.length;
                if (noObjectives < 3)
                {
                    //if (noNondominatedPoints < 1)
                        //System.err.println("run-time error");

                    tempVolume = front[0][0];
                }
                else
                    tempVolume = calculateHypervolume(front,
                                                      noNondominatedPoints,
                                                      noObjectives - 1);

                tempDistance = surfaceUnchangedTo(front, n, noObjectives - 1);
                volume += tempVolume * (tempDistance - distance);
                distance = tempDistance;
                n = reduceNondominatedSet(front, n, noObjectives - 1, distance);
            }
            return volume;
        } // CalculateHypervolume

        private static int filterNondominatedSet(List<double[]> front, int noPoints, int noObjectives)
        {
            int i, j;
            int n;

            n = noPoints;
            i = 0;
            while (i < n)
            {
                j = i + 1;
                while (j < n)
                {
                    if (dominates(front[i], front[j], noObjectives))
                    {
                        /* remove point 'j' */
                        n--;
                        swap(front, j, n);
                    }
                    else if (dominates(front[j], front[i], noObjectives))
                    {
                        /* remove point 'i'; ensure that the point copied to index 'i'
                           is considered in the next outer loop (thus, decrement i) */
                        n--;
                        swap(front, i, n);
                        i--;
                        break;
                    }
                    else
                        j++;
                }
                i++;
            }
            return n;
        } // FilterNondominatedSet 

        private static Boolean dominates(double[] point1, double[] point2, int noObjectives)
        {
            int i;
            int betterInAnyObjective;

            betterInAnyObjective = 0;
            for (i = 0; i < noObjectives && point1[i] >= point2[i]; i++)
                if (point1[i] > point2[i])
                    betterInAnyObjective = 1;

            return ((i >= noObjectives) && (betterInAnyObjective > 0));
        } //Dominates

        private static void swap(List<double[]> front, int i, int j)
        {
            double[] temp;

            temp = front[i];
            front[i] = front[j];
            front[j] = temp;
        } // Swap 

        private static double surfaceUnchangedTo(List<double[]> front, int noPoints, int objective)
        {
            int i;
            double minValue, value;

            //if (noPoints < 1)
            //    System.err.println("run-time error");

            minValue = front[0][objective];
            for (i = 1; i < noPoints; i++)
            {
                value = front[i][objective];
                if (value < minValue)
                    minValue = value;
            }
            return minValue;
        } // SurfaceUnchangedTo 

        /* remove all points which have a value <= 'threshold' regarding the
           dimension 'objective'; the points referenced by
           'front[0..noPoints-1]' are considered; 'front' is resorted, such that
           'front[0..n-1]' contains the remaining points; 'n' is returned */
        private static int reduceNondominatedSet(List<double[]> front, int noPoints, int objective,
                     double threshold)
        {
            int n;
            int i;

            n = noPoints;
            for (i = 0; i < n; i++)
                if (front[i][objective] <= threshold)
                {
                    n--;
                    swap(front, i, n);
                }

            return n;
        } // ReduceNondominatedSet

    }
}
