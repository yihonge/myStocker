using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace myStocker
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        
        public System.Timers.Timer timer1 = new System.Timers.Timer(10);
        private List<string> items = new List<string>();
        private string ini_path = System.Environment.CurrentDirectory + "//setting.ini";
        private void UserControl1_Load(object sender, EventArgs e)
        {
            listViewNF1.HeaderStyle = ColumnHeaderStyle.None;
            listViewNF2.HeaderStyle = ColumnHeaderStyle.None;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_TimesUp);
            IniFiles ini = new IniFiles(ini_path);
            var rtn = ini.ReadString("History","code","");
            comboBox1.Items.AddRange(rtn.Split(new char[] { ',' }));
        }
        private void Timer1_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        { 
            if (timer1.Interval == 10)
            {
                timer1.Interval = 4000;
            }
            string _stkcode = Invoke(new Func<string>(() => { return comboBox1.Text; })).ToString();
            if (_stkcode.Length == 6)
            {
                var rtn = getStkInfo(_stkcode);
                listDetails(listViewNF1, listViewNF2, label1, label2, label3, label5, label7, label9, label10, label12, label14, rtn);
            }
            else
            {
                return;
            }
        }
        private void listDetails(ListView lv1, ListView lv2, Label lb1, Label lb2, Label lb3, Label lb4, Label lb5, Label lb6, Label lb7, Label lb8, Label lb9, string[] rtn)
        {
            lv1.Invoke(new MethodInvoker(() =>
            {
                lv1.Items.Clear();
                lv1.BeginUpdate();
                lv1.Items.Add(new ListViewItem(new string[] { rtn[27], rtn[28] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[25], rtn[26] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[23], rtn[24] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[21], rtn[22] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[19], rtn[20] }));
                lv1.Items.Add(new ListViewItem(new string[] { "------", "-----" }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[9], rtn[10] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[11], rtn[12] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[13], rtn[14] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[15], rtn[16] }));
                lv1.Items.Add(new ListViewItem(new string[] { rtn[17], rtn[18] }));
                lv1.EndUpdate();
            }));
            if (rtn[29] != "")
            {
                string[] info = rtn[29].Split(new char[] { '|' });
                var query = info.OrderBy(o => o).Distinct();
                foreach (string itm in query)
                {
                    if (!items.Contains(reString(itm)))
                    {
                        items.Add(reString(itm));
                        string[] rt = itm.Split(new char[] { '/' });
                        lv2.Invoke(new MethodInvoker(() =>
                        {
                            lv2.BeginUpdate();
                            lv2.Items.Add(new ListViewItem(new string[] { rt[0], rt[1], rt[2], rt[3] }));
                            lv2.EndUpdate();
                        }));
                    }
                }
                lv2.Invoke(new MethodInvoker(() =>
                {
                    if (lv2.Items.Count > 1)
                    {
                        lv2.EnsureVisible(lv2.Items.Count - 1);
                    }

                }));
            }

            lb1.Invoke(new MethodInvoker(() => lb1.Text = rtn[1]));
            lb2.Invoke(new MethodInvoker(() => lb2.Text = rtn[3]));
            lb3.Invoke(new MethodInvoker(() => lb3.Text = rtn[32] + " %"));
            lb4.Invoke(new MethodInvoker(() => lb4.Text = rtn[4]));
            lb5.Invoke(new MethodInvoker(() => lb5.Text = rtn[5]));
            lb6.Invoke(new MethodInvoker(() => lb6.Text = rtn[38] + " %"));
            lb7.Invoke(new MethodInvoker(() => lb7.Text = rtn[34]));
            lb8.Invoke(new MethodInvoker(() => lb8.Text = rtn[33]));
            lb9.Invoke(new MethodInvoker(() => lb9.Text = rtn[43]+" %"));
        }
        private string reString(string _txt)
        {
            string restring = "";
            if (_txt != "")
            {
                var t = _txt.Split(new char[] { '/' });
                for (int i = 0; i < t.Length - 2; i++)
                {
                    restring += (t[i] + "/");
                }
            }
            return restring;
        }
        private string[] getStkInfo(string _stkCode)
        {
            string[] stkinfo;
            HttpCodeLib.XJHTTP xj = new HttpCodeLib.XJHTTP();
            string url = "http://qt.gtimg.cn/q=" + codeTrans(_stkCode);
            string html = xj.GetHtml(url).Html;
            stkinfo = html.Split(new char[] { '~' });
            return stkinfo;
        }
        private string codeTrans(string _code)
        {
            string codeNew = string.Empty;
            if (_code.StartsWith("600") || _code.StartsWith("601") || _code.StartsWith("603"))
            {
                codeNew = "sh" + _code;
            }
            else if (_code.StartsWith("000") || _code.StartsWith("002") || _code.StartsWith("300"))
            {
                codeNew = "sz" + _code;
            }
            return codeNew;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewNF1.Items.Clear();
            listViewNF2.Items.Clear();
        }

    }
}
