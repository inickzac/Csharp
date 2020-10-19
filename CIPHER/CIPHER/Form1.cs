using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIPHER
{
    public partial class Form1 : Form
    {
        readonly SaveFileDialog saveFileDial = new SaveFileDialog();
        readonly OpenFileDialog openCryptfile = new OpenFileDialog();
       
        public Form1()
        {
         
            InitializeComponent();           
            cryptButtom.Enabled = false;
            LFSR1Textbox.Text = "11111111111111111111111111111";
            LFSR2Textbox.Text = "1000100111010101000110001100100011111";
            LFSR3Textbox.Text = "010100000011101000101111110";
            RC4textBox.Text = "asb126789";
            panelProgress.Visible = false;
           // saveFileDial.DefaultExt = "crypt";
        }

          
        private void LFSR2Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = CheckEnterBinCode(e);
        }

        private void LFSR3Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = CheckEnterBinCode(e);
        }

        private void CiperLFSRRadioBut_Click(object sender, EventArgs e)
        {
            LFSR1Textbox.Enabled = true;
            LFSR2Textbox.Enabled = false;
            LFSR3Textbox.Enabled = false;
            RC4textBox.Enabled = false;
        }

        private void GeffeRadioBut_Click(object sender, EventArgs e)
        {
            RC4textBox.Enabled = false;
            LFSR1Textbox.Enabled = true;
            LFSR2Textbox.Enabled = true;
            LFSR3Textbox.Enabled = true;
        }

        private void RC4RadioBut_Click(object sender, EventArgs e)
        {
            RC4textBox.Enabled = true;
            LFSR1Textbox.Enabled = false;
            LFSR2Textbox.Enabled = false;
            LFSR3Textbox.Enabled = false;
        }

        private void LFSR1Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = CheckEnterBinCode(e);
        }

        private void OpenFileButtom_Click(object sender, EventArgs e)
        {
            
            if(openCryptfile.ShowDialog()==DialogResult.OK)
            {
                pathTextBox.Text = openCryptfile.FileName;

                    cryptButtom.Enabled = true;
               
            }

        }

        private void CryptButtom_Click(object sender, EventArgs e)
        {
            CreateAndStartThreadHiper(1);
        }

        private void DeciperButton_Click(object sender, EventArgs e)
        {
            CreateAndStartThreadHiper(2);
        }

        public async void CreateAndStartThreadHiper(int par)
        {
            try
            {
                OriginalTexttextBox.Text = "";
                ProcessTextBox.Text = "";
                GenKeytextBox.Text = "";
                Keylabel.Text = "";
                ICipher icipher=null;
                if (savePathTextBox.Text != "" || saveFileDial.ShowDialog() == DialogResult.OK)
                    {
                        savePathTextBox.Text = saveFileDial.FileName;
                    }
                    else throw new Exception("Выберите путь сохранения файла");
                    panelControlFalseInChipering.Enabled = false;
                Keylabel.Enabled = true;
                KeyOriglabel.Enabled = true;
                panelProgress.Visible = true;
                if (ciperLFSRRadioBut.Checked || GeffeRadioBut.Checked)
                { System.Collections.BitArray[] initialState = new System.Collections.BitArray[3];

                    if (ciperLFSRRadioBut.Checked)
                    {
                        initialState[0] = converToBitArray(29, LFSR1Textbox.Text);
                        initialState[1] = null;                       
                        Keylabel.Text = "LFSR1 " + LFSR1Textbox.Text;
                    }
                    if (GeffeRadioBut.Checked)
                    {
                        initialState[0] = converToBitArray(29, LFSR1Textbox.Text);
                        initialState[1] = converToBitArray(37, LFSR2Textbox.Text);
                        initialState[2] = converToBitArray(27, LFSR3Textbox.Text);                      
                        Keylabel.Text = " LFSR1 " + LFSR1Textbox.Text + " LFSR2 "+ LFSR2Textbox.Text + " LFSR3 " + LFSR3Textbox.Text;
                    }

                    icipher = new LFSR(initialState);
                }
                    if (RC4RadioBut.Checked)
                {
                    if (RC4textBox.Text == "")
                    { throw new Exception("Введите ключ rc4"); }
                    byte[] key = ASCIIEncoding.ASCII.GetBytes(RC4textBox.Text);
                    foreach(byte bt in key)
                    {
                        Keylabel.Text +=" " + Convert.ToString(bt,2);
                    }
                    icipher = new RC4(key);
                }

                 icipher.ProgressChange += progressChange;
                 icipher.secOriginalByte += (byteInt) => {
                     Action ChangeText = () => OriginalTexttextBox.Text += byteInt + " ";                               
                if (InvokeRequired) { Invoke(ChangeText); } else { ChangeText(); } };
                icipher.secChiperByte += (byteInt) => {
                    Action ChangeText = () => ProcessTextBox.Text += byteInt + " ";
                    if (InvokeRequired) { Invoke(ChangeText); } else { ChangeText(); }
                };
                icipher.secKeyBite += (byteInt) => {
                    Action ChangeText = () => GenKeytextBox.Text += byteInt + " ";
                    if (InvokeRequired) { Invoke(ChangeText); } else { ChangeText(); }
                };
                await Task.Run(() => icipher.CipherFile(pathTextBox.Text, savePathTextBox.Text, par));
                panelProgress.Visible = false; MessageBox.Show("Успешно");
                panelControlFalseInChipering.Enabled = true;
            }
            catch ( Exception ex) { MessageBox.Show(ex.Message); panelControlFalseInChipering.Enabled = true; }
        }

        System.Collections.BitArray converToBitArray(int registrLeight, string key)
        {
           
          var initialState=new System.Collections.BitArray(registrLeight);


            if (key.Length == registrLeight)
            {
                initialState=new System.Collections.BitArray(registrLeight);
                for (int i = 0; i < initialState.Count; i++)
                {
                    if (key[i] != '1' && key[i] != '0') throw new Exception("Введен неверный ключ");
                    initialState[i] = Convert.ToBoolean(key[i] - '0');
                }
            }
            else { throw new Exception("Неправильная длина ключа"); }
            return initialState;
        }

        public bool CheckEnterBinCode(KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (e.KeyChar != 48 && e.KeyChar != 49)
            {
                return true;
            }
            return false;
        }

        private void LFSR1Textbox_VisibleChanged(object sender, EventArgs e)
        {
           
        }

        private void Savebut_Click(object sender, EventArgs e)
        {
            if ( saveFileDial.ShowDialog() == DialogResult.OK)
            {
                savePathTextBox.Text = saveFileDial.FileName;
            }
        }
     
        void progressChange(long[] prog)
                 {
            
            Action vi = () =>
            {
                PrLabel.Text = prog[1].ToString(); readedLabel.Text = prog[0].ToString();
                progressBar1.Maximum = (int)prog[0]; progressBar1.Value = (int)prog[1];
            };

            if (InvokeRequired)
            { Invoke(vi); }
            else vi();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LFSR1Textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
