﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private void UserControl1_Load(object sender, EventArgs e)
        {
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView2.HeaderStyle = ColumnHeaderStyle.None;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_TimesUp);
        }
        private void Timer1_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        { 
            if (timer1.Interval == 10)
            {
                timer1.Interval = 4000;
            }
            string _stkcode = Invoke(new Func<string>(() => { return comboBox1.Text; })).ToString();
            var rtn = getStkInfo(_stkcode);
            listDetails(listView1,listView2,label3,label4,label5,rtn);
        }
        private void listDetails(ListView lv1, ListView lv2, Label lb1, Label lb2, Label lb3, string[] rtn)
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
            lb3.Invoke(new MethodInvoker(() => lb3.Text = rtn[32]));
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
    }
}