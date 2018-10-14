using MOEAPlat.Common;
using MOEAPlat.Encoding;
using MOEAPlat.PlotDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * The details of NSGA-II and test problems can refer to the following paper
 * 
 *   K. Deb, A. Pratap, S. Agarwal, T. Meyarivan, A fast and elitist multiobjective 
 *   genetic algorithm: Nsga-ii, IEEE Trans. Evol. Comput. 6 (2) (2002) 182–197
 * 
*/

namespace MOEAPlat.Algorithms
{
    public class NSGA2 : MultiObjectiveSolver
    {
        
        Random random = new Random();


        //public List<MoChromosome> mainpop = new List<MoChromosome>();

        public void initial()
        {
            this.popsize = div;
            InitialPopulation();
        }

        protected void InitialPopulation()
        {
            for (int i = 0; i < this.popsize; i++)
            {
                MoChromosome chromosome = this.CreateChromosome();

                Evaluate(chromosome);
                mainpop.Add(chromosome);
            }
        }

        protected override void DoSolve()
        {
            initial();
            frm = new plotFrm(mainpop, mop.GetName());
            frm.Show();
            frm.Refresh();
            while (!Terminated())
            {

                List<MoChromosome> offsPop = new List<MoChromosome>();

                for (int i = 0; i < popsize; i++)
                {
                    MoChromosome offspring;
                    offspring = SBXCrossover(i, false);//GeneticOPDE//GeneticOPSBXCrossover
                    this.Evaluate(offspring);
                    offsPop.Add(offspring);
                }

                List<MoChromosome> Pop = new List<MoChromosome>();
                Pop.AddRange(mainpop);
                Pop.AddRange(offsPop);

                EnviromentSelection(Pop);

                if (this.ItrCounter % 10 == 0)
                {
                    frm.refereshPlot(this.ItrCounter, mainpop);
                    frm.Refresh();
                }

                ItrCounter++;
            }
            Common.FileTool.WritetoFile(mainpop, "gen", 1);
            Common.FileTool.WritetoFile(mainpop, "obj", 2);
        }

        protected void EnviromentSelection(List<MoChromosome> pop)
        {
            List<MoChromosome> result = new List<MoChromosome>();
            //List<List<MoChromosome>> dominatedSet0 = NSGA.fastNonDominatedSort(pop);
            List<List<MoChromosome>> dominatedSet0 = NSGA.FastConstrainedNonDominatedSort(pop);

            int cnt = 0;
            while (result.Count() + dominatedSet0[cnt].Count() < this.popsize)
            {
                for (int r = 0; r < dominatedSet0[cnt].Count(); r++)
                {
                    dominatedSet0[cnt][r].selected = true;
                }

                result.AddRange(dominatedSet0[cnt]);
                cnt++;
            }
            if (result.Count() == this.popsize)
            {
                //return result;
                mainpop.Clear();
                mainpop.AddRange(result);
                return;
            }

            NSGA.CrowdingDistanceAssignment(dominatedSet0[cnt]);
            MoChromosome[] arr = NSGA.Sort(dominatedSet0[cnt]);
            int i = 0;
            while (result.Count() < popsize)
            {
                result.Add(arr[i]);
                i++;
            }
            mainpop.Clear();
            mainpop.AddRange(result);
            return;
        }
    }
}
