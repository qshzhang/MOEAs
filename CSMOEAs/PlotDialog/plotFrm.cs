using MOEAPlat.Common;
using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MOEAPlat.PlotDialog
{
    public partial class plotFrm : Form
    {
        List<double[]> list = new List<double[]>();

        List<double[]> pof = new List<double[]>();

        public plotFrm(List<MoChromosome> lst, string name)
        {
            InitializeComponent();
            ucBackPanelPlot.OnBtnClose += BtnClose;

            foreach (MoChromosome mo in lst)
            {
                double[] arr = new double[mo.objectDimension];
                Array.Copy(mo.objectivesValue, arr, mo.objectDimension);
                list.Add(arr);
            }
            if(lst[0].objectDimension < 3)
            {
                pof = POF.POF.GetPOF(name);
            }
        }

        private void BtnClose()
        {
            this.Close();
        }

        private void plotFrm_Load(object sender, EventArgs e)
        {
            Mchart.Location = new Point(3, 30);
            Mchart.Width = ucBackPanelPlot.Width - 6;
            Mchart.Height = ucBackPanelPlot.Height - 32;

            Mchart.Titles.Add("Iteration: " + 1);
            plotChart(1); 
            
            if(list[0].Length == 2)
            {
                plot(pof, 0);
            }
        }

        public void refereshPlot(int itr, List<MoChromosome> lst)
        {
            list.Clear();
            foreach (MoChromosome mo in lst)
            {
                double[] arr = new double[mo.objectDimension];
                Array.Copy(mo.objectivesValue, arr, mo.objectDimension);
                list.Add(arr);
            }
            plotChart(itr);
        }

        private void plotChart(int itr)
        {
            Mchart.Titles.Clear();
            Mchart.Titles.Add("Iteration: " + itr);

            if (list[0].Length == 2)
            {
                plot(list, 1);
                
            }
            else
            {
                Mchart.Legends.Clear();
                coordinateplot(list);
            }
        }

        public void coordinateplot(List<double[]> list)
        {
            Mchart.Series.Clear();
            //this.Mchart.Series[0].ChartType = SeriesChartType.Line;
            Mchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Mchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            Mchart.ChartAreas[0].AxisX.Title = "Objective No.";
            Mchart.ChartAreas[0].AxisY.Title = "Objective Value";

            Mchart.ChartAreas[0].AxisX.Maximum = list[0].Length;
            Mchart.ChartAreas[0].AxisX.Minimum = 1;
            //Mchart.ChartAreas[0].AxisX.Interval = (10 - 0) / 10;

            for (int i = 0; i < list.Count; i++)
            {
                Mchart.Series.Add("" + (i));
                Mchart.Series[i].ChartType = SeriesChartType.Line;
                for (int j = 0; j < list[i].Length; j++)
                {
                    this.Mchart.Series[i].Points.AddXY(j + 1, list[i][j]);
                }
            }
        }

        public void plot(List<double[]> list, int type)
        {
            if (list == null || list.Count == 0) return;
            Mchart.ChartAreas[0].AxisX.Title = "x";
            Mchart.ChartAreas[0].AxisY.Title = "y";
            Mchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Mchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            //X轴、Y轴标题
            Mchart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            if (type == 1)
            {
                if(Mchart.Series.Count > 1)
                    this.Mchart.Series.RemoveAt(1);
                this.Mchart.Series.Add("Algorithm");
                Mchart.Series[1].ChartType = SeriesChartType.Point;
            }
            else
            {
                this.Mchart.Series[0].ChartType = SeriesChartType.Spline;
                this.Mchart.Series[0].LegendText = "POF";

                Mchart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(Tool.MaxArray(list, 0));
                Mchart.ChartAreas[0].AxisX.Minimum = Math.Floor(Tool.MinArray(list, 1));

                //Mchart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(list[list.Count - 1][0]);
                //Mchart.ChartAreas[0].AxisX.Minimum = Math.Floor(list[0][0]);

                //Mchart.ChartAreas[0].AxisY.Minimum = Math.Ceiling(list[list.Count - 1][1]);
                //Mchart.ChartAreas[0].AxisY.Maximum = Math.Floor(list[0][1]);

            }

            for (int i = 0; i < list.Count; i++)
            {
                this.Mchart.Series[type].Points.AddXY(list[i][0], list[i][1]);
            }
        }
    }
}
