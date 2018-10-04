namespace MOEAPlat
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Mchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.DataShow = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.combNormalization = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.combCrossoverType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.combDisplayProcess = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.combAggFunction = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.combPopsize = new System.Windows.Forms.ComboBox();
            this.combDimension = new System.Windows.Forms.ComboBox();
            this.btnAddObj = new System.Windows.Forms.Button();
            this.labelDimension = new System.Windows.Forms.Label();
            this.comBoxAlg = new System.Windows.Forms.ComboBox();
            this.comBoxMOP = new System.Windows.Forms.ComboBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtGeneration = new System.Windows.Forms.TextBox();
            this.labelPopsize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.GenChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenChart)).BeginInit();
            this.SuspendLayout();
            // 
            // Mchart
            // 
            chartArea1.Name = "ChartArea1";
            this.Mchart.ChartAreas.Add(chartArea1);
            this.Mchart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Mchart.Legends.Add(legend1);
            this.Mchart.Location = new System.Drawing.Point(0, 0);
            this.Mchart.Name = "Mchart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Mchart.Series.Add(series1);
            this.Mchart.Size = new System.Drawing.Size(600, 490);
            this.Mchart.TabIndex = 0;
            this.Mchart.Text = "chart1";
            // 
            // DataShow
            // 
            this.DataShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataShow.Location = new System.Drawing.Point(0, 0);
            this.DataShow.Name = "DataShow";
            this.DataShow.ReadOnly = true;
            this.DataShow.RowTemplate.Height = 30;
            this.DataShow.Size = new System.Drawing.Size(417, 334);
            this.DataShow.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1015, 828);
            this.splitContainer1.SplitterDistance = 490;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.combNormalization);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            this.splitContainer2.Panel1.Controls.Add(this.combCrossoverType);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.combDisplayProcess);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.combAggFunction);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.combPopsize);
            this.splitContainer2.Panel1.Controls.Add(this.combDimension);
            this.splitContainer2.Panel1.Controls.Add(this.btnAddObj);
            this.splitContainer2.Panel1.Controls.Add(this.labelDimension);
            this.splitContainer2.Panel1.Controls.Add(this.comBoxAlg);
            this.splitContainer2.Panel1.Controls.Add(this.comBoxMOP);
            this.splitContainer2.Panel1.Controls.Add(this.btnExecute);
            this.splitContainer2.Panel1.Controls.Add(this.txtGeneration);
            this.splitContainer2.Panel1.Controls.Add(this.labelPopsize);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.Mchart);
            this.splitContainer2.Size = new System.Drawing.Size(1015, 490);
            this.splitContainer2.SplitterDistance = 411;
            this.splitContainer2.TabIndex = 0;
            // 
            // combNormalization
            // 
            this.combNormalization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combNormalization.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combNormalization.FormattingEnabled = true;
            this.combNormalization.Items.AddRange(new object[] {
            "True",
            "False"});
            this.combNormalization.Location = new System.Drawing.Point(220, 356);
            this.combNormalization.Name = "combNormalization";
            this.combNormalization.Size = new System.Drawing.Size(164, 28);
            this.combNormalization.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(52, 356);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "Normalization:";
            // 
            // combCrossoverType
            // 
            this.combCrossoverType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combCrossoverType.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combCrossoverType.FormattingEnabled = true;
            this.combCrossoverType.Items.AddRange(new object[] {
            "SBX",
            "DE"});
            this.combCrossoverType.Location = new System.Drawing.Point(219, 277);
            this.combCrossoverType.Name = "combCrossoverType";
            this.combCrossoverType.Size = new System.Drawing.Size(164, 28);
            this.combCrossoverType.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(50, 277);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Crossover Type:";
            // 
            // combDisplayProcess
            // 
            this.combDisplayProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combDisplayProcess.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combDisplayProcess.FormattingEnabled = true;
            this.combDisplayProcess.Items.AddRange(new object[] {
            "True",
            "False"});
            this.combDisplayProcess.Location = new System.Drawing.Point(219, 316);
            this.combDisplayProcess.Name = "combDisplayProcess";
            this.combDisplayProcess.Size = new System.Drawing.Size(164, 28);
            this.combDisplayProcess.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(50, 316);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Display Process:";
            // 
            // combAggFunction
            // 
            this.combAggFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combAggFunction.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combAggFunction.FormattingEnabled = true;
            this.combAggFunction.Items.AddRange(new object[] {
            "Weighted Sum Approach",
            "Tchebycheff Approach",
            "PBI Approach"});
            this.combAggFunction.Location = new System.Drawing.Point(219, 237);
            this.combAggFunction.Name = "combAggFunction";
            this.combAggFunction.Size = new System.Drawing.Size(170, 28);
            this.combAggFunction.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Aggregation Function:";
            // 
            // combPopsize
            // 
            this.combPopsize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combPopsize.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combPopsize.FormattingEnabled = true;
            this.combPopsize.Location = new System.Drawing.Point(219, 198);
            this.combPopsize.Name = "combPopsize";
            this.combPopsize.Size = new System.Drawing.Size(170, 28);
            this.combPopsize.TabIndex = 13;
            // 
            // combDimension
            // 
            this.combDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combDimension.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combDimension.FormattingEnabled = true;
            this.combDimension.Location = new System.Drawing.Point(219, 74);
            this.combDimension.Name = "combDimension";
            this.combDimension.Size = new System.Drawing.Size(170, 28);
            this.combDimension.TabIndex = 12;
            // 
            // btnAddObj
            // 
            this.btnAddObj.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddObj.Location = new System.Drawing.Point(23, 427);
            this.btnAddObj.Name = "btnAddObj";
            this.btnAddObj.Size = new System.Drawing.Size(143, 36);
            this.btnAddObj.TabIndex = 11;
            this.btnAddObj.Text = "Add Objection";
            this.btnAddObj.UseVisualStyleBackColor = true;
            this.btnAddObj.Click += new System.EventHandler(this.btnAddObj_Click);
            // 
            // labelDimension
            // 
            this.labelDimension.AutoSize = true;
            this.labelDimension.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDimension.Location = new System.Drawing.Point(18, 74);
            this.labelDimension.Name = "labelDimension";
            this.labelDimension.Size = new System.Drawing.Size(160, 20);
            this.labelDimension.TabIndex = 9;
            this.labelDimension.Text = "Obj/Para Dimension:";
            // 
            // comBoxAlg
            // 
            this.comBoxAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxAlg.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comBoxAlg.FormattingEnabled = true;
            this.comBoxAlg.Location = new System.Drawing.Point(219, 116);
            this.comBoxAlg.Name = "comBoxAlg";
            this.comBoxAlg.Size = new System.Drawing.Size(170, 28);
            this.comBoxAlg.TabIndex = 8;
            this.comBoxAlg.SelectedIndexChanged += new System.EventHandler(this.comBoxAlg_SelectedIndexChanged);
            // 
            // comBoxMOP
            // 
            this.comBoxMOP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxMOP.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comBoxMOP.FormattingEnabled = true;
            this.comBoxMOP.Location = new System.Drawing.Point(219, 32);
            this.comBoxMOP.Name = "comBoxMOP";
            this.comBoxMOP.Size = new System.Drawing.Size(170, 28);
            this.comBoxMOP.TabIndex = 7;
            this.comBoxMOP.SelectedIndexChanged += new System.EventHandler(this.comBoxMOP_SelectedIndexChanged);
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(236, 427);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(137, 36);
            this.btnExecute.TabIndex = 6;
            this.btnExecute.Text = "Run";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtGeneration
            // 
            this.txtGeneration.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGeneration.Location = new System.Drawing.Point(219, 157);
            this.txtGeneration.Name = "txtGeneration";
            this.txtGeneration.Size = new System.Drawing.Size(170, 28);
            this.txtGeneration.TabIndex = 4;
            // 
            // labelPopsize
            // 
            this.labelPopsize.AutoSize = true;
            this.labelPopsize.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPopsize.Location = new System.Drawing.Point(45, 198);
            this.labelPopsize.Name = "labelPopsize";
            this.labelPopsize.Size = new System.Drawing.Size(133, 20);
            this.labelPopsize.TabIndex = 3;
            this.labelPopsize.Text = "Division/Popsize:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(54, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "MaxGeneration:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Algorithms:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(125, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MOP:";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.DataShow);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GenChart);
            this.splitContainer3.Size = new System.Drawing.Size(1015, 334);
            this.splitContainer3.SplitterDistance = 417;
            this.splitContainer3.TabIndex = 0;
            // 
            // GenChart
            // 
            chartArea2.Name = "ChartArea1";
            this.GenChart.ChartAreas.Add(chartArea2);
            this.GenChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.GenChart.Legends.Add(legend2);
            this.GenChart.Location = new System.Drawing.Point(0, 0);
            this.GenChart.Name = "GenChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.GenChart.Series.Add(series2);
            this.GenChart.Size = new System.Drawing.Size(594, 334);
            this.GenChart.TabIndex = 0;
            this.GenChart.Text = "chart1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 828);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Mchart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataShow)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GenChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Mchart;
        private System.Windows.Forms.DataGridView DataShow;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.DataVisualization.Charting.Chart GenChart;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtGeneration;
        private System.Windows.Forms.Label labelPopsize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comBoxAlg;
        private System.Windows.Forms.ComboBox comBoxMOP;
        private System.Windows.Forms.Label labelDimension;
        private System.Windows.Forms.Button btnAddObj;
        private System.Windows.Forms.ComboBox combDimension;
        private System.Windows.Forms.ComboBox combPopsize;
        private System.Windows.Forms.ComboBox combDisplayProcess;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox combAggFunction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox combCrossoverType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox combNormalization;
        private System.Windows.Forms.Label label7;
    }
}

