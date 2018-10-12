using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class FileTool
    {
        public static void WritetoFile(List<MoChromosome> col, string fileName, int type)
        {
            StreamWriter sw = new StreamWriter(fileName, false);
            foreach(MoChromosome mo in col)
            {
                //sw.Write(mo.vectorString());
                if(type == 1)
                    sw.WriteLine(mo.genVectorString());
                else
                    sw.WriteLine(mo.objVectorString());
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static void WritetoFile(List<double> col, string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName, false);
            foreach (double mo in col)
            {
                //sw.Write(mo.vectorString());
                sw.WriteLine(mo.ToString());
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static List<double[]> readData(string path)
        {
            List<double[]> list = new List<double[]>();

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader m_streamReader = new StreamReader(fs);
                m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = m_streamReader.ReadLine();
                do
                {
                    string[] split = strLine.Split(',');

                    double[] arr = new double[split.Length];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = Convert.ToDouble(split[i].Trim());
                    }
                    list.Add(arr);

                    strLine = m_streamReader.ReadLine();
                } while (strLine != null && strLine != "");
                m_streamReader.Close();
                m_streamReader.Dispose();
                fs.Close();
                fs.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return list;
            }
        }

        public static List<pair> readIndicatorData(string path)
        {
            List<pair> list = new List<pair>();

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader m_streamReader = new StreamReader(fs);
                m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = m_streamReader.ReadLine();
                int i = 1;
                do
                {
                    pair p = new pair(i, Convert.ToDouble(strLine));
                    list.Add(p);
                    strLine = m_streamReader.ReadLine();
                    i++;
                } while (strLine != null && strLine != "");
                m_streamReader.Close();
                m_streamReader.Dispose();
                fs.Close();
                fs.Dispose();
                return list;
            }
            catch
            {
                return list;
            }
            
        }
    }
}
