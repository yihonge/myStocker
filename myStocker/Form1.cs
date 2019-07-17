using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myStocker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public delegate void ShowMessage();//创建一个代理 
        public void ShowTextBox(TextBox tb, String txt)
        {
            if (tb.InvokeRequired)
            {
                ShowMessage msg;
                msg = () =>
                {
                    tb.Text = txt;
                };
                tb.Invoke(msg);
                return;
            }
            else
            {
                tb.Text = txt;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = true;
            userControl11.timer1.Start();
            userControl12.timer1.Start();
            userControl13.timer1.Start();
            toolStripButton1.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = true;
            userControl11.timer1.Stop();
            userControl12.timer1.Stop();
            userControl13.timer1.Stop();
            toolStripButton2.Enabled = false;
        }
    }
}
