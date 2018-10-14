using MOEAPlat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MOEAPlat.POF
{
    public static class POF
    {
        public static List<double[]> GetPOF(string name)
        {
            List<double[]> result = new List<double[]>();

            try { 
                Type tx = typeof(POF);
                MethodInfo mf = tx.GetMethod("POF_"+name, BindingFlags.Public | BindingFlags.Static, null, new Type[] { }, null);
                return (List<double[]>)mf.Invoke(null, null);
            }catch(Exception ex)
            {
                return result;
            }
        }

        public static List<double[]> POF_F1()
        {
            List<double[]> result = new List<double[]>();
            for(int i = 0;i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = Math.Pow(1 - Math.Sqrt(arr[0]), 5);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_F2()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 0.5 * (1 - arr[0] + Math.Sqrt(1 - arr[0]) * Math.Pow(Math.Cos(4 * Math.PI * (1 - arr[0])), 2));
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_F3()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 0.5 * (1 - Math.Pow(arr[0], 0.1) + Math.Pow((1 - Math.Sqrt(arr[0])) * Math.Cos(3 * Math.PI * arr[0]), 2));
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_ZDT1()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 1 - Math.Sqrt(arr[0]);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_ZDT2()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 1 - arr[0] * arr[0];
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_ZDT3()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 2 - Math.Sqrt(arr[0]) - arr[0] * Math.Sin(10 * Math.PI * arr[0]);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_ZDT4()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_ZDT6()
        {
            return POF_ZDT2();
        }

        public static List<double[]> POF_MOP1()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_MOP2()
        {
            return POF_ZDT2();
        }

        public static List<double[]> POF_MOP3()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = Math.Sqrt(1 - arr[0] * arr[0]);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_MOP4()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 1 - Math.Sqrt(arr[0]) * Math.Pow(Math.Cos(2 * Math.PI * arr[0]), 2);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_MOP5()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_UF1()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_UF2()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_UF3()
        {
            return POF_ZDT1();
        }

        public static List<double[]> POF_UF4()
        {
            return POF_ZDT2();
        }

        public static List<double[]> POF_UF5()
        {
            return POF_UF7();
        }

        public static List<double[]> POF_UF6()
        {
            return POF_UF7();
        }

        public static List<double[]> POF_UF7()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = 1 - arr[0];
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_SCH()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 4.0 / 1000;
                arr[1] = Math.Pow(2 - Math.Sqrt(arr[0]), 2);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_TF2()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 1.0 / 1000;
                arr[1] = Math.Sqrt(1 - Math.Pow(arr[0], 5));
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_TF4()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double x = i * 1.0 / 1000;
                double[] arr = new double[2];
                arr[0] = Math.Pow(x + 0.05 * Math.Sin(6 * Math.PI * x), 2);
                arr[1] = Math.Pow(1 - x + 0.05 * Math.Sin(6 * Math.PI * x), 2);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_TF5()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double x = i * 1.0 / 1000;
                double[] arr = new double[2];
                arr[0] = Math.Pow(x + 0.05 * Math.Sin(6 * Math.PI * x), 0.2);
                arr[1] = Math.Pow(1 - x + 0.05 * Math.Sin(6 * Math.PI * x), 10);
                result.Add(arr);
            }
            return result;
        }

        public static List<double[]> POF_POL()
        {
            return FileTool.ReadData("D:\\Code\\C#\\MOEAPlat\\MOEAPlat\\POF\\APOF\\POL");
        }

        public static List<double[]> POF_WFG3_2()
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i <= 1000; i++)
            {
                double[] arr = new double[2];
                arr[0] = i * 2.0 / 1000;
                arr[1] = 4 - 2 * arr[0];
                result.Add(arr);
            }
            return result;
        }

    }
}
