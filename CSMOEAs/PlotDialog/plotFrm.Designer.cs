namespace MOEAPlat.PlotDialog
{
    partial class plotFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Mchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ucBackPanelPlot = new MOEAPlat.UserControls.ucBackPanel();
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).BeginInit();
            this.SuspendLayout();
            // 
            // Mchart
            // 
            this.Mchart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.Mchart.ChartAreas.Add(chartArea1);
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            this.Mchart.Legends.Add(legend1);
            this.Mchart.Location = new System.Drawing.Point(0, 36);
            this.Mchart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Mchart.Name = "Mchart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Mchart.Series.Add(series1);
            this.Mchart.Size = new System.Drawing.Size(383, 260);
            this.Mchart.TabIndex = 1;
            this.Mchart.Text = "chart1";
            // 
            // ucBackPanelPlot
            // 
            this.ucBackPanelPlot.BackColor = System.Drawing.Color.Transparent;
            this.ucBackPanelPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBackPanelPlot.IsShowMinBtn = false;
            this.ucBackPanelPlot.Location = new System.Drawing.Point(0, 0);
            this.ucBackPanelPlot.Margin = new System.Windows.Forms.Padding(2);
            this.ucBackPanelPlot.Name = "ucBackPanelPlot";
            this.ucBackPanelPlot.Size = new System.Drawing.Size(383, 296);
            this.ucBackPanelPlot.TabIndex = 2;
            // 
            // plotFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 296);
            this.Controls.Add(this.Mchart);
            this.Controls.Add(this.ucBackPanelPlot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "plotFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "plotFrm";
            this.Load += new System.EventHandler(this.plotFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Mchart;
        private UserControls.ucBackPanel ucBackPanelPlot;
    }
}