namespace MOEAPlat.PlotDialog
{
    partial class MOPAdd
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
            this.txtObjectives = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtFunc = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.ucBackPanel = new MOEAPlat.UserControls.ucBackPanel();
            this.SuspendLayout();
            // 
            // txtObjectives
            // 
            this.txtObjectives.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObjectives.Location = new System.Drawing.Point(12, 103);
            this.txtObjectives.Multiline = true;
            this.txtObjectives.Name = "txtObjectives";
            this.txtObjectives.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObjectives.Size = new System.Drawing.Size(527, 208);
            this.txtObjectives.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(198, 334);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(122, 37);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Generate";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtFunc
            // 
            this.txtFunc.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFunc.Location = new System.Drawing.Point(157, 67);
            this.txtFunc.Name = "txtFunc";
            this.txtFunc.Size = new System.Drawing.Size(382, 28);
            this.txtFunc.TabIndex = 2;
            // 
            // txtType
            // 
            this.txtType.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtType.Location = new System.Drawing.Point(12, 66);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(117, 28);
            this.txtType.TabIndex = 3;
            // 
            // ucBackPanel
            // 
            this.ucBackPanel.BackColor = System.Drawing.Color.Transparent;
            this.ucBackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBackPanel.IsShowMinBtn = true;
            this.ucBackPanel.Location = new System.Drawing.Point(0, 0);
            this.ucBackPanel.Name = "ucBackPanel";
            this.ucBackPanel.Size = new System.Drawing.Size(551, 385);
            this.ucBackPanel.TabIndex = 4;
            // 
            // MOPAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 385);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.txtFunc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtObjectives);
            this.Controls.Add(this.ucBackPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MOPAdd";
            this.Text = "MOPAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtObjectives;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtFunc;
        private System.Windows.Forms.TextBox txtType;
        private UserControls.ucBackPanel ucBackPanel;
    }
}