namespace MOEAPlat.UserControls
{
    partial class ucBackPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnClose = new System.Windows.Forms.Label();
            this.BtnMinFrm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.BackColor = System.Drawing.Color.Transparent;
            this.BtnClose.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClose.Location = new System.Drawing.Point(263, 0);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(30, 30);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "X";
            this.BtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            this.BtnClose.MouseEnter += new System.EventHandler(this.BtnClose_MouseEnter);
            this.BtnClose.MouseLeave += new System.EventHandler(this.BtnClose_MouseLeave);
            // 
            // BtnMinFrm
            // 
            this.BtnMinFrm.BackColor = System.Drawing.Color.Transparent;
            this.BtnMinFrm.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMinFrm.Location = new System.Drawing.Point(217, 0);
            this.BtnMinFrm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BtnMinFrm.Name = "BtnMinFrm";
            this.BtnMinFrm.Size = new System.Drawing.Size(30, 30);
            this.BtnMinFrm.TabIndex = 1;
            this.BtnMinFrm.Text = "_";
            this.BtnMinFrm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnMinFrm.Click += new System.EventHandler(this.BtnMinFrm_Click);
            this.BtnMinFrm.MouseEnter += new System.EventHandler(this.BtnMinFrm_MouseEnter);
            this.BtnMinFrm.MouseLeave += new System.EventHandler(this.BtnMinFrm_MouseLeave);
            // 
            // ucBackPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.BtnMinFrm);
            this.Controls.Add(this.BtnClose);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ucBackPanel";
            this.Size = new System.Drawing.Size(302, 259);
            this.SizeChanged += new System.EventHandler(this.ucBackPanel_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucBackPanel_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucBackPanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ucBackPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ucBackPanel_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label BtnClose;
        private System.Windows.Forms.Label BtnMinFrm;
    }
}
