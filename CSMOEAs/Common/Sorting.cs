using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class Sorting
    {
        public static int[] sorting(double[] tobesorted)
        {
            int[] index = new int[tobesorted.Length];
            for (int i = 0; i < index.Length; i++)
                index[i] = i;

            for (int i = 1; i < tobesorted.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (tobesorted[index[i]] < tobesorted[index[j]])
                    {
                        // insert and break;
                        int temp = index[i];
                        for (int k = i - 1; k >= j; k--)
                        {
                            index[k + 1] = index[k];
                        }
                        index[j] = temp;
                        break;
                    }
                }
            }
            return index;
        }
    }
}
