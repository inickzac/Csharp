using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogBuffer
{
    public partial class Form1 : Form
    {
        LogBuffer logBuffer;

        public Form1()
        {
            InitializeComponent();
            textBox2.Text = 5.ToString();
            textBox3.Text = 10000.ToString();
            textBox4.Text = @"\log.txt";
            logBuffer  = new LogBuffer((Application.StartupPath.ToString()+textBox4.Text),Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text));
           logBuffer.MessageEvent += o => MessageBox.Show(o);
           logBuffer.ShowLogList += o => { listBox1.Items.Clear();
               o.ForEach(b => listBox1.Items.Add(b));
           };
            logBuffer.ShowQuantityitemsInBuffer += o => Invoke(new Action(() => label1.Text ="Количество строк в буфере "+ o.ToString()));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                logBuffer.Add(textBox1.Text);
                textBox1.Text = "";
                ShowLog();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void ShowLog()
        {
            listBox1.Items.Clear();
            logBuffer.readAllLog().ForEach(o => listBox1.Items.Add(o));
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            int res = 0;
            if (logBuffer!=null && Int32.TryParse(textBox2.Text,out res))
            {
                logBuffer.FlushLimit = res;
            }
          
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            int res = 0;
            if (logBuffer!=null && Int32.TryParse(textBox3.Text, out res))
            {
                logBuffer.FlushPeriodInMilliseconds = res;
            }
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logBuffer.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (logBuffer != null)
            {
                logBuffer.FileName = Application.StartupPath.ToString() + textBox4.Text;
            }
        }
    }

   
}
