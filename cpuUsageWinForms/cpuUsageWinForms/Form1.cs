using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Geared;
using System.Diagnostics;

namespace cpuUsageWinForms
{
    public partial class Form1 : Form
    {
        Test test = new Test();
        public Form1()
        {
            InitializeComponent();
          
            cartesianChart1.Series.Add(new GLineSeries
            {
                Values = test.Values
            });
            cartesianChart1.DisableAnimations = true;
            test.cpuUsage += (o) =>
            {
                try
                {
                    Invoke(new Action(() => { label1.Text = "Загрузка ЦП: " + o.ToString() + "%"; }));
                }
                catch (ObjectDisposedException)
                {
             } };
            test.Read();

        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            test.Read();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            test.close();
        }

       
    }
}
