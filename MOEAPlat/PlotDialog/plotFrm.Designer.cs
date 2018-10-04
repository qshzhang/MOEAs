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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Mchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).BeginInit();
            this.SuspendLayout();
            // 
            // Mchart
            // 
            chartArea3.Name = "ChartArea1";
            this.Mchart.ChartAreas.Add(chartArea3);
            this.Mchart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.Mchart.Legends.Add(legend3);
            this.Mchart.Location = new System.Drawing.Point(0, 0);
            this.Mchart.Name = "Mchart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.Mchart.Series.Add(series3);
            this.Mchart.Size = new System.Drawing.Size(574, 444);
            this.Mchart.TabIndex = 1;
            this.Mchart.Text = "chart1";
            // 
            // plotFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 444);
            this.Controls.Add(this.Mchart);
            this.Name = "plotFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "plotFrm";
            this.Load += new System.EventHandler(this.plotFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Mchart;
    }
}