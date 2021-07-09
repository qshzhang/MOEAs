using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using MOEAPlat.Encoding;
using BusRouteCalculator;

namespace MOEAPlat.Problems
{
    public class Buses : AbstractMOP
    {
        private static Buses instance;
        private Buses(int pd)
        {
            this.parDimension = pd;
            Init();
        }
        public override void Evaluate(MoChromosome chromosome)
        {
            string testGraph = "";
            string testTimetable = "";
            StreamReader streamReader = new StreamReader("exeterTransit_33-1-_-y10-1_graph.json");
            testGraph = streamReader.ReadToEnd();
            streamReader = new StreamReader("exeterTransit_33-1-_-y10-1_timetable.json");
            testTimetable = streamReader.ReadToEnd();
            Graph graph = JsonSerializer.Deserialize<Graph>(testGraph);
            TimeTable timeTable = JsonSerializer.Deserialize<TimeTable>(testTimetable);
            BusRoute busRoute = new BusRoute(timeTable, graph);
            BusNetwork busNetwork = new BusNetwork();
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));


            double[] sp = chromosome.realGenes;
            float[] _chromosome = new float[sp.Length];
            for (int i = 0; i < sp.Length; i++)
            {
                if (i == 0)
                {
                    _chromosome[i] = (float)Math.Round(sp[i] * 45 + 1);
                }
                else if ( i== 1)
                {
                    _chromosome[i] = (float)Math.Round(sp[i] * 3 + 1);
                }
                else
                {
                    _chromosome[i] = (float)(sp[i] * 0.65 + 0.25);
                }
            }

            double[] obj = chromosome.objectivesValue;
            float[] results = new float[obj.Length];
            results = busNetwork.SolveNetwork(_chromosome);

            for (int j = 0; j < results.Length; j++)
            {
                obj[j] = results[j];
            }

            //for (int i = 0; i < this.parDimension; i++)
            //    sp[i] = domain[i, 0] + sp[i] * (domain[i, 1] - domain[i, 0]); 

            //double g = 0, tp;
            //for (int i = 1; i < parDimension; i++)
            //{
            //    tp = sp[i] - Math.Sin(0.5 * Math.PI * sp[i]);
            //    g += tp * tp - Math.Cos(2 * Math.PI * tp);
            //}
            //g = 2 * Math.Sin(0.5 * Math.PI * sp[0]) * (parDimension - 1 + g);
            //obj[0] = (1 + g) * sp[0];
            //obj[1] = (1 + g) * Math.Pow(1 - Math.Sqrt(sp[0]), 5);

            //for (int i = 0; i < this.parDimension; i++)
            //    sp[i] = (sp[i] - domain[i,0]) / (domain[i, 1] - domain[i, 0]);
        }

        public override void Init()
        {
            this.domain = new double[this.parDimension,2];
            for (int i = 0; i < parDimension; i++)
            {
                domain[i,0] = 0;
                domain[i,1] = 1;
            }
            this.objDimension = 2;
            this.range = new double[objDimension,2];
        }

        public static Buses GetInstance(int pd)
        {
            if (instance == null)
            {
                instance = new Buses(pd);
                instance.name = "Buses";
            }
            return instance;
        }
    }
}
