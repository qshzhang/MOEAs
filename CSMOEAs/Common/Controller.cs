using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class Controller
    {
        public class MOPControl
        {
            public string name;
            public int decisions;
            public List<int> objs;
            public int popsize;
            public List<int> division;
            public List<string> alg;
            public int maxGeneration = 0;
        }

        public static Dictionary<int, int> dictPopsize = new Dictionary<int, int>()
        {
            { 2,100},{ 3, 91},{ 5,210},{ 8,156},{ 10,275},{ 15,135}
        };

        public static Dictionary<string, MOPControl> dictionary = new Dictionary<string, MOPControl>()
        {
            { "CDP", new MOPControl { name="CDP", decisions=5, popsize=23, maxGeneration=500} },
            { "F1", new MOPControl{ name="F1", decisions=30, popsize=100, maxGeneration=500} },
            { "F2", new MOPControl{ name="F2", decisions=30, popsize=100, maxGeneration=500}},
            { "F3", new MOPControl{ name="F3", decisions=30, popsize=100, maxGeneration=500}},
            { "F4", new MOPControl{ name="F4", decisions=30, popsize=23, maxGeneration=500}},
            { "F5", new MOPControl{ name="F5", decisions=30, popsize=23, maxGeneration=500}},
            { "F6", new MOPControl{ name="F6", decisions=30, popsize=23, maxGeneration=500}},
            { "mF4", new MOPControl{ name="mF4", decisions=30, popsize=23, maxGeneration=500}},
            { "POL", new MOPControl{ name="POL", decisions=2, popsize=100, maxGeneration=500}},
            { "SCH", new MOPControl{ name="SCH", decisions=1, popsize=100, maxGeneration=500}},
            { "TF2", new MOPControl{ name="TF2", decisions=20, popsize=100, maxGeneration=500}},
            { "TF4", new MOPControl{ name="TF4", decisions=20, popsize=100, maxGeneration=500}},
            { "TF5", new MOPControl{ name="TF5", decisions=20, popsize=100, maxGeneration=500}},
            { "ZDT1", new MOPControl{ name="ZDT1", decisions=10, popsize=100, maxGeneration=500}},
            { "ZDT2", new MOPControl{ name="ZDT2", decisions=10, popsize=100, maxGeneration=500}},
            { "ZDT3", new MOPControl{ name="ZDT3", decisions=10, popsize=100, maxGeneration=500}},
            { "ZDT4", new MOPControl{ name="ZDT4", decisions=10, popsize=100, maxGeneration=500}},
            { "ZDT6", new MOPControl{ name="ZDT6", decisions=10, popsize=100, maxGeneration=500}},

            { "MOP1", new MOPControl{ name="MOP1", decisions=10, popsize=300}},
            { "MOP2", new MOPControl{ name="MOP2", decisions=10, popsize=300}},
            { "MOP3", new MOPControl{ name="MOP3", decisions=10, popsize=300}},
            { "MOP4", new MOPControl{ name="MOP4", decisions=10, popsize=300}},
            { "MOP5", new MOPControl{ name="MOP5", decisions=10, popsize=300}},
            { "MOP6", new MOPControl{ name="MOP6", decisions=10, popsize=30}},
            { "MOP7", new MOPControl{ name="MOP7", decisions=10, popsize=30}},

            { "UF1", new MOPControl{ name="UF1", decisions=30, popsize=300}},
            { "UF2", new MOPControl{ name="UF2", decisions=30, popsize=300}},
            { "UF3", new MOPControl{ name="UF3", decisions=30, popsize=300}},
            { "UF4", new MOPControl{ name="UF4", decisions=30, popsize=300}},
            { "UF5", new MOPControl{ name="UF5", decisions=30, popsize=300}},
            { "UF6", new MOPControl{ name="UF6", decisions=30, popsize=300}},
            { "UF7", new MOPControl{ name="UF7", decisions=30, popsize=300}},
            { "UF8", new MOPControl{ name="UF8", decisions=30, popsize=30}},
            { "UF9", new MOPControl{ name="UF9", decisions=30, popsize=30}},
            { "UF10", new MOPControl{ name="UF10", decisions=30, popsize=30}},

            { "DTLZ", new MOPControl{ name="DTLZ", objs=new List<int>(){ 2,3,5,8,10,15}, division=new List<int>(){ 100,12,6,3,3,2} }},
            { "WFG", new MOPControl{ name="WFG", objs=new List<int>(){ 2,3,5,8,10,15}, division=new List<int>(){ 100,12,6,3,3,2} }},

            { "SRN", new MOPControl{ name="SRN", decisions=2, popsize=200}},
            { "TNK", new MOPControl{ name="TNK", decisions=2, popsize=200}},
            { "OSY", new MOPControl{ name="OSY", decisions=2, popsize=200}},
            { "CTP1", new MOPControl{ name="CTP1", decisions=2, popsize=200}},
        };

    }
}
