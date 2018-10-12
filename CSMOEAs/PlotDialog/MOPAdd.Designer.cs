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
            this.SuspendLayout();
            // 
            // txtObjectives
            // 
            this.txtObjectives.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObjectives.Location = new System.Drawing.Point(12, 49);
            this.txtObjectives.Multiline = true;
            this.txtObjectives.Name = "txtObjectives";
            this.txtObjectives.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObjectives.Size = new System.Drawing.Size(536, 208);
            this.txtObjectives.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(208, 284);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(111, 37);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "生成";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtFunc
            // 
            this.txtFunc.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFunc.Location = new System.Drawing.Point(157, 13);
            this.txtFunc.Name = "txtFunc";
            this.txtFunc.Size = new System.Drawing.Size(382, 28);
            this.txtFunc.TabIndex = 2;
            // 
            // txtType
            // 
            this.txtType.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtType.Location = new System.Drawing.Point(12, 12);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(117, 28);
            this.txtType.TabIndex = 3;
            // 
            // MOPAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 342);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.txtFunc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtObjectives);
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
    }
}