using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MOEAPlat.PlotDialog
{
    public partial class MOPAdd : Form
    {
        public MOPAdd()
        {
            InitializeComponent();
            ucBackPanel.OnBtnClose += BtnClose;
            ucBackPanel.OnBtnMinFrm += BtnMinFrm;
        }

        private void BtnClose()
        {
            this.Close();
        }

        private void BtnMinFrm()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("D:\\Code\\C#\\MOEAPlat\\MOEAPlat\\Common\\template.txt", System.Text.Encoding.Default);
            string text = "";
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                text += line;
                text += System.Environment.NewLine;
            }
            sr.Close();
            if (txtFunc.Text.Length < 2) return;
            text = text.Replace("#FuncName#", txtFunc.Text);
            text = text.Replace("#ObjectiveFunctionAddRegion#", txtObjectives.Text);
            text = text.Replace("Math.sin", "Math.Sin");
            text = text.Replace("Math.cos", "Math.Cos");
            text = text.Replace("Math.acos", "Math.Acos");
            text = text.Replace("Math.sqrt", "Math.Sqrt");
            text = text.Replace("Math.pow", "Math.Pow");
            text = text.Replace("Math.abs", "Math.Abs");
            text = text.Replace("Math.exp", "Math.Exp");

            string filename = "";
            if(txtType.Text == "")
            {
                filename = "D:\\Code\\C#\\MOEAPlat\\MOEAPlat\\Problems\\" + txtFunc.Text + ".cs";
            }
            else
            {
                filename = "D:\\Code\\C#\\MOEAPlat\\MOEAPlat\\Problems\\" + txtType.Text + "\\" + txtFunc.Text + ".cs";
            }

            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(text);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }
}
