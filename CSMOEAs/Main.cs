using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using MOEAPlat.Algorithms;
using MOEAPlat.Problems;
using MOEAPlat.PlotDialog;
using System.Reflection;
using MOEAPlat.Common;

namespace MOEAPlat
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach(Type ty in types)
            {
                if (ty.FullName.Split('.').Length > 3 || ty.FullName.IndexOf("<>c") != -1) continue;
                if (ty.FullName.IndexOf("MultiObjectiveSolver") != -1) continue;
                if (ty.FullName.IndexOf("MultiObjectiveProblem") != -1) continue;
                if (ty.FullName.IndexOf("AbstractMOP") != -1) continue;
                if (ty.FullName.IndexOf("MOEAPlat.Algorithms") != -1)
                {
                    this.comBoxAlg.Items.Add(ty.FullName.Substring(ty.FullName.LastIndexOf(".")+1));
                }
                else if(ty.FullName.IndexOf("MOEAPlat.Problems") != -1)
                {
                    this.comBoxMOP.Items.Add(ty.FullName.Substring(ty.FullName.LastIndexOf(".") + 1));
                }
            }
            //this.comBoxAlg.SelectedIndex = 0;
            //this.comBoxMOP.SelectedIndex = 0;
        }

        public void coordinateplot(List<double[]> list)
        {
            Mchart.Series.Clear();
            //this.Mchart.Series[0].ChartType = SeriesChartType.Line;
            Mchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Mchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            Mchart.ChartAreas[0].AxisX.Title = "Objective No.";
            Mchart.ChartAreas[0].AxisY.Title = "Objective Value";

            if(list.Count > 0)
            {
                Mchart.ChartAreas[0].AxisX.Maximum = list[0].Length;
            }
            
            Mchart.ChartAreas[0].AxisX.Minimum = 1;
            //Mchart.ChartAreas[0].AxisX.Interval = (10 - 0) / 10;

            for (int i = 0; i < list.Count; i++)
            {
                Mchart.Series.Add(""+(i));
                Mchart.Series[i].ChartType = SeriesChartType.Line;
                for (int j = 0; j < list[i].Length; j++)
                {
                    this.Mchart.Series[i].Points.AddXY(j+1, list[i][j]);
                }
                
            }
        }

        public void IGDCurve(List<PairRelation> list)
        {
            GenChart.Series[0].LegendText = "IGD Curve";
            //this.Mchart.Series[0].ChartType = SeriesChartType.Line;
            GenChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            GenChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            GenChart.ChartAreas[0].AxisX.Title = "Generation";
            GenChart.ChartAreas[0].AxisY.Title = "IGD";

            GenChart.ChartAreas[0].AxisX.Maximum = list.Count;
            GenChart.ChartAreas[0].AxisX.Minimum = 1;
            //Mchart.ChartAreas[0].AxisX.Interval = (10 - 0) / 10;

            this.GenChart.Series[0].ChartType = SeriesChartType.Spline;
            this.GenChart.Series[0].Points.Clear();
            this.GenChart.Series[0].MarkerStyle = MarkerStyle.Square;
            this.GenChart.Series[0].Color = Color.Red;
            //GenChart.Series.Add("IGDCurve");

            foreach (PairRelation p in list)
            {
                this.GenChart.Series[0].Points.AddXY(p.pos, p.val);
            }
        }

        public void plot(List<double[]> list, int type)
        {
            if (list.Count == 0) return;

            Mchart.ChartAreas[0].AxisX.Title = "x";
            Mchart.ChartAreas[0].AxisY.Title = "y";
            Mchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Mchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            Mchart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            if (Mchart.Series.Count > 2)
            {
                this.Mchart.Series.Clear();
                this.Mchart.Series.Add("POF");
            }

            if (type == 1)
            {
                if (Mchart.Series.Count > 1)
                    this.Mchart.Series.RemoveAt(1);
                this.Mchart.Series.Add("Algorithm");
                Mchart.Series[1].ChartType = SeriesChartType.Point;
            }
            else
            {

                this.Mchart.Series[0].Points.Clear();
                this.Mchart.Series[0].ChartType = SeriesChartType.Spline;
                this.Mchart.Series[0].LegendText = "POF";

                Mchart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(Tool.MaxArray(list, 0));
                Mchart.ChartAreas[0].AxisX.Minimum = Math.Floor(Tool.MinArray(list, 1));
            }


            for (int i = 0; i < list.Count; i++)
            {
                this.Mchart.Series[type].Points.AddXY(list[i][0], list[i][1]);
            }

        }


        private void btnExecute_Click(object sender, EventArgs e)
        {

            InitGlobalValue();


            Assembly assembly = Assembly.GetExecutingAssembly();
            MultiObjectiveSolver impl = (MultiObjectiveSolver)assembly.CreateInstance("MOEAPlat.Algorithms."+GlobalValue.AlgName);
            impl.div = GlobalValue.Popsize;
            impl.TotalItrNum = GlobalValue.MaxGeneration;
            impl.neighbourSize = 20;

            //if ((string)this.combDimension.SelectedItem == "") this.txtObjDimension.Text = "0";
            int obj = GlobalValue.Dimension;
            object[] parameters = null;
            if (obj != 0)
            {
                parameters = new object[1];
                parameters[0] = obj;
            }

            Type t = Type.GetType("MOEAPlat.Problems." + GlobalValue.MOPName, true);

            MultiObjectiveProblem problem;

            try
            {
                problem = (MultiObjectiveProblem)t.InvokeMember("GetInstance", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod,
                null, null, parameters);
            }
            catch (Exception ex)
            {
                problem = (MultiObjectiveProblem)t.InvokeMember("GetInstance", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod,
                null, null, null);
            }

            //if(problem.getObjectiveSpaceDimension() > 3 && problem.getName().IndexOf("DTLZ") == -1)
            //{
            //    MessageBox.Show("only support 2-objective problems");
            //    return;
            //}

            //MultiObjectiveProblem problem = (MultiObjectiveProblem)assembly.CreateInstance("MOEAPlat.Problems."+this.comBoxMOP.SelectedItem, 
            //    true, BindingFlags.Default, null, parameters, null, null); ;
            
            //MultiObjectiveProblem problem = Problems.WFG4_M.GetInstance(15);
            impl.Solve(problem);

            List<double[]> list = FileTool.ReadData("obj");
            if (problem.GetObjectiveSpaceDimension() == 2)
            {
                List<double[]> pof = POF.POF.GetPOF(problem.GetName());
                plot(pof, 0);
                plot(list, 1);
            }
            else
            {
                Mchart.Legends.Clear();
                coordinateplot(list);
            }

            IGDCurve(FileTool.ReadIndicatorData("igdCurve"));
            table(list);
            list.Clear();
        }

        protected void table(List<double[]> list)
        {
            this.DataShow.Rows.Clear();
            this.DataShow.Columns.Clear();
            this.DataShow.Columns.Add("Seq.", "Seq.");

            for (int i = 0; i < list[0].Length; i++)
            {
                DataShow.Columns.Add("f" + (i + 1), "f" + (i + 1));
            }

            for (int i = 0; i < list.Count; i++)
            {
                DataShow.Rows.Add();
                for (int j = 0; j < list[0].Length; j++)
                {
                    DataShow.Rows[i].Cells["f" + (j + 1)].Value = list[i][j];
                }
                DataShow.Rows[i].Cells["Seq."].Value = i + 1;

            }
        }

        private void btnAddObj_Click(object sender, EventArgs e)
        {
            MOPAdd frm = new MOPAdd();
            frm.ShowDialog();
        }

        private void comBoxMOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.combDimension.Items.Clear();
            this.combPopsize.Items.Clear();
            string mop = this.comBoxMOP.SelectedItem.ToString();
            if (mop.IndexOf("DTLZ")!=-1 || mop.IndexOf("WFG") != -1)
            {
                this.labelDimension.Text = "Obj Dimension:";
                this.labelPopsize.Text = "Division:";

                
                foreach(int em in Controller.dictionary["DTLZ"].objs)
                {
                    this.combDimension.Items.Add(em);
                }
                
                foreach (int em in Controller.dictionary["DTLZ"].division)
                {
                    this.combPopsize.Items.Add(em);
                }

            }
            else
            {
                this.labelDimension.Text = "Decision Dimension:";
                this.labelPopsize.Text = "Popsize:";

                this.combDimension.Items.Clear();
                this.combDimension.Items.Add(Controller.dictionary[mop].decisions);
                this.combPopsize.Items.Clear();
                this.combPopsize.Items.Add(Controller.dictionary[mop].popsize);
                this.txtGeneration.Text = Controller.dictionary[mop].maxGeneration.ToString();
            }
            this.combDimension.SelectedIndex = 0;
            this.combPopsize.SelectedIndex = 0;
        }

        private void comBoxAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            string alg = this.comBoxAlg.SelectedItem.ToString();
            string mop = this.comBoxMOP.SelectedItem.ToString();
            if (alg.IndexOf("VaEA")!=-1 || alg.IndexOf("BiGE")!=-1 || 
                alg.IndexOf("NSGA2") != -1 || alg.IndexOf("SPEA2") != -1){
                if(mop.IndexOf("DTLZ")!=-1 || mop.IndexOf("WFG") != -1)
                {
                    int objs = Convert.ToInt32(combDimension.SelectedItem);
                    combPopsize.Items.Clear();
                    combPopsize.Items.Add(Controller.dictPopsize[objs]);
                }
            }
        }

        private void InitGlobalValue()
        {
            GlobalValue.MOPName = this.comBoxMOP.SelectedItem.ToString();
            GlobalValue.Dimension = Convert.ToInt32(this.combDimension.SelectedItem);
            GlobalValue.AlgName = this.comBoxAlg.SelectedItem.ToString();
            GlobalValue.MaxGeneration = Convert.ToInt32(this.txtGeneration.Text);
            GlobalValue.Popsize = Convert.ToInt32(this.combPopsize.SelectedItem);
            GlobalValue.AggressionFunction = this.combAggFunction.SelectedItem.ToString();
            GlobalValue.IsShowProcess = this.combDisplayProcess.SelectedItem.ToString().Equals("True") ? true : false;
            GlobalValue.IsNormalization = this.combNormalization.SelectedItem.ToString().Equals("True") ? true : false;
            GlobalValue.CrossoverType = this.combCrossoverType.SelectedItem.ToString();
        }
    }
}
