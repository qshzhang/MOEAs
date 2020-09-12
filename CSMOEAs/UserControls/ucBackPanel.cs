using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MOEAPlat.UserControls
{
    public partial class ucBackPanel : UserControl
    {
        private Point frmPos;
        private Point mousePos;
        private Boolean isBeginMove = false;


        public delegate void BackPanelBtnCallback();
        public BackPanelBtnCallback OnBtnClose;
        public BackPanelBtnCallback OnBtnMinFrm;

        public bool IsShowMinBtn { get => BtnMinFrm.Visible; set => BtnMinFrm.Visible = value; }

        public ucBackPanel()
        {
            InitializeComponent();
        }

        private void BtnClose_MouseEnter(object sender, EventArgs e)
        {
            BtnClose.BackColor = System.Drawing.Color.Red;
        }

        private void BtnClose_MouseLeave(object sender, EventArgs e)
        {
            BtnClose.BackColor = System.Drawing.Color.Transparent;
        }

        private void BtnMinFrm_MouseEnter(object sender, EventArgs e)
        {
            BtnMinFrm.BackColor = System.Drawing.Color.Red;
        }

        private void BtnMinFrm_MouseLeave(object sender, EventArgs e)
        {
            BtnMinFrm.BackColor = System.Drawing.Color.Transparent;
        }

        private void ucBackPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form form = this.Parent as Form;
                if (form != null)
                {
                    isBeginMove = true;
                    frmPos = form.Location;
                    mousePos = Control.MousePosition;
                }
            }
        }

        private void ucBackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBeginMove)
            {
                Form form = this.Parent as Form;
                if (form != null)
                {
                    form.Location = frmPos + (Size)Control.MousePosition - (Size)mousePos;
                }
            }
        }

        private void ucBackPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBeginMove = false;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if(null != OnBtnClose)
            {
                OnBtnClose();
            }
        }

        private void BtnMinFrm_Click(object sender, EventArgs e)
        {
            if (null != OnBtnMinFrm)
            {
                OnBtnMinFrm();
            }
        }

        private void ucBackPanel_SizeChanged(object sender, EventArgs e)
        {
            this.BtnClose.Location = new Point(this.Width - this.BtnClose.Width - 1, 1);
            this.BtnMinFrm.Location = new Point(this.Width - this.BtnClose.Width - 3 - this.BtnMinFrm.Width, 1);
        }

        private void ucBackPanel_Paint(object sender, PaintEventArgs e)
        {
            //Pen pen = new Pen(Color.Blue, 3);
            //e.Graphics.DrawLine(pen, 0, 0, this.Width, 0);
            //e.Graphics.DrawLine(pen, 0, 0, 0, this.Height);
            //e.Graphics.DrawLine(pen, this.Width, 0, this.Width, this.Height);
            //e.Graphics.DrawLine(pen, 0, this.Height, this.Width, this.Height);
        }
    }
}
