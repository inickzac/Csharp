using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace terapevt
{

    public partial class Form1 : Form
    {
        gentext genHtmlText = new gentext();

        public Form1()
        {
            InitializeComponent();
            refresh.Visible = false;
            webBrowser1.ScrollBarsEnabled = false;
            if (!isFirstStart.isOpenFile)
            { defaultFileSet(); }
            if (isFirstStart.isOpenFile)
            { openFile(); }
            
            
            //  defaultSet();           
        }

        void defaultFileSet()
        {
            using (StreamReader defFile = new StreamReader(Application.StartupPath.ToString() + "\\default"))
                ControlContainsText(defFile);
        }//загружаем файл по умолчанию

        void openFile()
        {
            using (System.IO.StreamReader fileRead = new System.IO.StreamReader(isFirstStart.openFile.OpenFile()))
                ControlContainsText(fileRead);
        }


        

        public void defaultSet() //стандартные установки
        {

            cripiVlaznieComboBox.Enabled = false;
            cripiSuhiecomboBox.Enabled = false;
            genPanel.AutoScroll = true;
            genHtmlText.feelingField = feelingComboBox.Text;
            feelingComboBox.SelectedIndex = 0;
            generalStateComboBox.Text = "удовлетворительное";
            foreach (CheckBox item in OnkoTableLayoutPanel.Controls.OfType<CheckBox>())
            { item.Checked = true; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
            webBrowser1.ScrollBarsEnabled = false;

            genHtmlText.dateInspection = examinationDateTimePicker.Text;
            feelingComboBox.SelectedIndex = 0;

            Koza1checkBox.Checked = true;
            Koza2checkBox.Checked = true;
            kozac6checkBox.Checked = true;

            slizistaya1CheckBox.Checked = true;
            slizistaya2checkBox.Checked = true;

            mindalini1checkBox.Checked = true;

            Limfo1checkBox.Checked = true;

            hitovid2RadioButton1.Checked = true;

            Kosti1checkBox.Checked = true;

            misci1CheckBox.Checked = true;

            legkieChastotaMaskedTextBox.Text = 16.ToString();
            legkie1CheckBox.Checked = true;

            crip1CheckBox.Checked = true;
            cripi1radioButton.Checked = true;

            pulsMaskedTextBox.Text = 70.ToString();
            puls2RadioButton.Checked = true;
            napolnenie2RadioButton.Checked = true;
            deficit2RadioButton.Checked = true;

            serdce1CheckBox.Checked = true;
            serdce2CheckBox.Checked = true;
            serdceGranica1RadioButton.Checked = true;

            Yazik1СheckBox.Checked = true;
            Yazik2СheckBox.Checked = true;

            Jivot1CheckBox.Checked = true;
            Jivot2CheckBox.Checked = true;
            Jivot3CheckBox.Checked = true;

            Pechen1CheckBox.Checked = true;
            PechenKrai1CheckBox.Checked = true;
            Selezen1CheckBox.Checked = true;

            Pochki1CheckBox.Checked = true;
            Pocolachovan1RadioButton.Checked = true;
            Moche1СheckBox.Checked = true;
            Moche2СheckBox.Checked = true;
            Stul1СheckBox.Checked = true;

            Otek1СheckBox.Checked = true;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FioTextBox_TextChanged(object sender, EventArgs e)
        {
            genHtmlText.FioField = FioTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }//фамилия

        private void SexComboBox_TextChanged(object sender, EventArgs e)//пол
        {
            genHtmlText.sexField = SexComboBox.Text;
            if (SexComboBox.SelectedIndex == 0)
            {
                MZelezaCheckBox.Enabled = false;
                genHtmlText.cell9 = "";
            }
            else { MZelezaCheckBox.Enabled = true; genHtmlText.cell9 = "Молочная железа N"; }
            SKFChanged(this, e);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)//возраст
        {
            genHtmlText.ageDateField = maskedTextBox1.Text;
            SKFChanged(this, e);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void adressTextBox_TextChanged(object sender, EventArgs e)//адрес
        {
            genHtmlText.AdressField = adressTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void examinationDateTimePicker_ValueChanged(object sender, EventArgs e)//дата приема
        {
            genHtmlText.dateInspection = examinationDateTimePicker.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void ComplainTextBox_TextChanged(object sender, EventArgs e)//жалобы
        {
            genHtmlText.complainsField = ComplainTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void AnamneztextBox_TextChanged(object sender, EventArgs e)//анамнез
        {
            genHtmlText.anamnezField = AnamneztextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void generalStateComboBox_TextChanged(object sender, EventArgs e)//общее состояние
        {
            genHtmlText.generalStateField = generalStateComboBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void TemperatureMaskedTextBox_TextChanged(object sender, EventArgs e)
        {
            genHtmlText.temperatureField = TemperatureMaskedTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }//температура

        private void WeightMaskedTextBox_TextChanged(object sender, EventArgs e)//вес
        {
            genHtmlText.weightField = WeightMaskedTextBox.Text;
            genHtmlText.IMTField = IMTGen();
            IMTLabel.Text = IMTGen();
            SKFChanged(this, e);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void growthMaskedTextBox_TextChanged(object sender, EventArgs e)//рост
        {
            genHtmlText.growthField = growthMaskedTextBox.Text;
            genHtmlText.IMTField = IMTGen();
            IMTLabel.Text = IMTGen();
            webBrowser1.DocumentText = genHtmlText.compleateText();

        }

        public string IMTGen()//вычисление ИМТ
        {
            string imt = "";
            if (WeightMaskedTextBox.Text != "" && growthMaskedTextBox.Text != "")
            {
                decimal weight = Convert.ToDecimal(WeightMaskedTextBox.Text);
                decimal growth = Convert.ToDecimal(growthMaskedTextBox.Text) / 100;
                decimal res = decimal.Round(weight / (growth * growth), 2);

                imt = res.ToString();
                IMTStringLabel.Text = IMTString(imt);
            }

            return imt;

        }

        string IMTString(string imt)//расшифровка ИМТ
        {

            double imtStr = Convert.ToDouble(imt);
            string str = "";
            if (imtStr <= 16) { str = " (Выраженный дефицит)"; }
            if (imtStr > 16 && imtStr < 18.5) { str = " (Недостаточная масса) "; }
            if (imtStr >= 18.5 && imtStr <= 24.99) { str = " (Норма) "; }
            if (imtStr >= 25 && imtStr <= 30) { str = " (ИМТ)"; }
            if (imtStr > 30 && imtStr <= 35) { str = " (НЖО 1) "; }
            if (imtStr > 35 && imtStr <= 40) { str = " (НЖО 2) "; }
            if (imtStr > 40) { str = " (НЖО 3)"; }
            return str;
        }

        private void IMTValueLabel_TextChanged(object sender, EventArgs e)
        {
            genHtmlText.IMTField = IMTValueLabel.Text + " " + IMTLabel.Text; ;

            webBrowser1.DocumentText = genHtmlText.compleateText();
        }//числовое значение ИМТ

        private void IMTStringLabel_TextChanged(object sender, EventArgs e)
        {
            genHtmlText.IMTFieldString = IMTStringLabel.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }//строковое значение ИМТ

        private void feelingComboBox_TextChanged(object sender, EventArgs e)//сознание
        {
            genHtmlText.feelingField = feelingComboBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void OncoKozaCheckBox_Click_1(object sender, EventArgs e)//онко осмотр кожа
        {
            if (OncoKozaCheckBox.Checked)
            { genHtmlText.cell1 = "Кожа N"; }
            else { genHtmlText.cell1 = "Кожа"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void PichevodCheckBox_Click(object sender, EventArgs e)//онко осмотр пищевод
        {
            if (PichevodCheckBox.Checked)
            { genHtmlText.cell2 = "Пищевод жалоб нет"; }
            else { genHtmlText.cell2 = "Пищевод жалобы есть"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void OnkoPLCheckBox_Click(object sender, EventArgs e)//онко осмотр периферические Л/У
        {
            if (OnkoPLCheckBox.Checked)
            { genHtmlText.cell3 = "Периферические лимфоузлы N"; }
            else { genHtmlText.cell3 = "Периферические лимфоузлы"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void zeludokCheckBox_Click(object sender, EventArgs e)//онко осмотр желудок
        {
            if (zeludokCheckBox.Checked)
            { genHtmlText.cell4 = "Желудок жалоб нет"; }
            else { genHtmlText.cell4 = "Желудок жалобы есть"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void GubaCheckBox_Click(object sender, EventArgs e)//онко осмотр губы
        {
            if (GubaCheckBox.Checked)
            { genHtmlText.cell5 = "Губа N"; }
            else { genHtmlText.cell5 = "Губа"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void PrKishkaCheckBox_Click(object sender, EventArgs e)//онко осмотр прямая кишка
        {
            if (PrKishkaCheckBox.Checked)
            { genHtmlText.cell6 = "Прямая кишка жалоб нет"; }
            else { genHtmlText.cell6 = "Прямая кишка жалобы есть"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void RotCheckBox_Click(object sender, EventArgs e)// онко осмотр язык и слизистая рта
        {
            if (RotCheckBox.Checked)
            { genHtmlText.cell7 = "Язык и слизистая рта N"; }
            else { genHtmlText.cell7 = "Язык и слизистая рта"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void HZelezaCheckBox_Click(object sender, EventArgs e)//онко осмотр щитовидная железа
        {
            if (HZelezaCheckBox.Checked)
            { genHtmlText.cell8 = "Щитовидная железа N"; }
            else { genHtmlText.cell8 = "Щитовидная железа"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void MZelezaCheckBox_Click(object sender, EventArgs e)//онко осмотр молочная железа
        {
            if (MZelezaCheckBox.Checked)
            { genHtmlText.cell9 = "Молочная железа N"; }
            else { genHtmlText.cell9 = "Молочная железа"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void Koza1checkBox_CheckedChanged(object sender, EventArgs e)//панель осмотра кожи
        {
            genHtmlText.kozaOsmotr = "";
            foreach (CheckBox tb in panel3.Controls.OfType<CheckBox>())
            { if (tb.Checked)
                {
                    genHtmlText.kozaOsmotr += tb.Text;
                    genHtmlText.kozaOsmotr += ", "; }
            }
            genHtmlText.kozaOsmotr = cramblDel(genHtmlText.kozaOsmotr);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void slizistaya1CheckBox_CheckedChanged(object sender, EventArgs e)//панель осмотра видимых слизистых
        {
            genHtmlText.vidimSlizist = "";
            foreach (CheckBox tb in panel4.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.vidimSlizist += tb.Text;
                    genHtmlText.vidimSlizist += ", ";
                }
            }
            genHtmlText.vidimSlizist = cramblDel(genHtmlText.vidimSlizist);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void mindalini1checkBox_CheckedChanged(object sender, EventArgs e)//панель осмотра небных миндалин
        {
            genHtmlText.mindalini = "";
            foreach (CheckBox tb in mindaliniPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.mindalini += tb.Text;
                    genHtmlText.mindalini += ", ";
                }
            }
            genHtmlText.mindalini = cramblDel(genHtmlText.mindalini);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void Limfo1checkBox_CheckedChanged(object sender, EventArgs e)//панель осмотра л/у
        {
            genHtmlText.limfoUzli = "";
            foreach (CheckBox tb in limfoUzliPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.limfoUzli += tb.Text;
                    genHtmlText.limfoUzli += ", ";
                }
            }
            genHtmlText.limfoUzli = cramblDel(genHtmlText.limfoUzli);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void hitovidRadioButton_CheckedChanged(object sender, EventArgs e)//панель осмотра щитовидной железы
        {
            if (hitovidRadioButton.Checked)

            { hitovidComboBox.Enabled = true;

                genHtmlText.hitovidZeleza = "увеличение " + hitovidComboBox.Text; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void hitovid2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (hitovid2RadioButton1.Checked)
                hitovidComboBox.Enabled = false;
            { genHtmlText.hitovidZeleza = "без изменений"; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }//если ЩЖ увеличена, то до какой степени

        string cramblDel(string strCrmb)//удаляем лишнюю запятую, если конец предложения
        {
            if (strCrmb != "")
            { strCrmb = strCrmb.Remove(strCrmb.Length - 2, 2); }
            return strCrmb;
        }

        private void Kosti1checkBox_CheckedChanged(object sender, EventArgs e)//панель осмотра костной системы
        {
            genHtmlText.kosti = "";
            foreach (CheckBox tb in kostiPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.kosti += tb.Text;
                    genHtmlText.kosti += ", ";
                }
            }
            genHtmlText.kosti = cramblDel(genHtmlText.kosti);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void misci1CheckBox_CheckedChanged(object sender, EventArgs e)//панель осмотра мышечной системы
        {
            genHtmlText.misci = "";
            foreach (CheckBox tb in misciPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.misci += tb.Text;
                    genHtmlText.misci += ", ";
                }
            }
            genHtmlText.misci = cramblDel(genHtmlText.misci);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void legkie1CheckBox_CheckedChanged(object sender, EventArgs e)//панель осмотра легких
        {
            genHtmlText.legkie = "";
            string legkieChastota = legkieHastotaLabel.Text + " " + legkieChastotaMaskedTextBox.Text + " в 1 мин ";


            foreach (CheckBox tb in perkytZvukPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.legkie += tb.Text;
                    genHtmlText.legkie += ", ";
                }
            }
            if (genHtmlText.legkie != "")
            { genHtmlText.legkie = legkiePerkytlabel.Text + " " + genHtmlText.legkie; }
            genHtmlText.legkie = cramblDel(genHtmlText.legkie);
            genHtmlText.legkie = legkieChastota + genHtmlText.legkie;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void crip1CheckBox_CheckedChanged(object sender, EventArgs e)//панель осмотра хрипы
        {
            genHtmlText.dihShum = "";
            foreach (CheckBox tb in shumPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.dihShum += tb.Text;
                    genHtmlText.dihShum += ", ";
                }
            }
            genHtmlText.dihShum = cramblDel(genHtmlText.dihShum);
            genHtmlText.dihShum += " Хрипы: ";
            if (cripi2radioButton.Checked)
            {
                cripiVlaznieComboBox.Enabled = false;
                cripiSuhiecomboBox.Enabled = true;
                genHtmlText.dihShum += cripi2radioButton.Text + " " + cripiSuhiecomboBox.Text;
            }
            if (cripi1radioButton.Checked)
            {
                cripiVlaznieComboBox.Enabled = false;
                cripiSuhiecomboBox.Enabled = false;
                genHtmlText.dihShum += cripi1radioButton.Text;
            }
            if (cripi3radioButton.Checked)
            {
                cripiVlaznieComboBox.Enabled = true;
                cripiSuhiecomboBox.Enabled = false;
                genHtmlText.dihShum += cripi3radioButton.Text + " " + cripiVlaznieComboBox.Text;
            }
            if (cripi4radioButton.Checked)
            {
                cripiVlaznieComboBox.Enabled = false;
                cripiSuhiecomboBox.Enabled = false;
                genHtmlText.dihShum += cripi4radioButton.Text;
            }

            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void pulsMaskedTextBox_TextChanged(object sender, EventArgs e)//панель осмотра кровеносная система
        {
            genHtmlText.krovSistem = pulsMaskedTextBox.Text + " ";
            if (puls1RadioButton.Checked)
            { genHtmlText.krovSistem += puls1RadioButton.Text + " "; }
            if (puls2RadioButton.Checked)
            { genHtmlText.krovSistem += puls2RadioButton.Text + " "; }
            genHtmlText.krovSistem += napolnenieLabel.Text + " ";
            if (napolnenie1RadioButton.Checked)
            { genHtmlText.krovSistem += napolnenie1RadioButton.Text + " "; }
            if (napolnenie2RadioButton.Checked)
            { genHtmlText.krovSistem += napolnenie2RadioButton.Text + " "; }
            genHtmlText.krovSistem += deficitLabel.Text + " ";
            if (deficit2RadioButton.Checked)
            { genHtmlText.krovSistem += deficit2RadioButton.Text + " "; }
            if (deficitRadioButton.Checked)
            { genHtmlText.krovSistem += deficitRadioButton.Text + " "; }
            genHtmlText.krovSistem += krovsistDavlenielabel.Text + " " + adMaskedTextBox.Text;

            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void SerdceChanged(object sender, EventArgs e)//паель осмотра сердце
        {
            genHtmlText.serdce = serdceToniLabel.Text + " ";
            foreach (CheckBox tb in serdcePanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.serdce += tb.Text;
                    genHtmlText.serdce += ", ";
                }
            }
            genHtmlText.serdce = cramblDel(genHtmlText.serdce) + " ";
            genHtmlText.serdce += serdceGranicaLabel.Text + " ";
            if (serdceGranica1RadioButton.Checked)
            { genHtmlText.serdce += serdceGranica1RadioButton.Text; }
            if (serdceGranica2RadioButton.Checked)
            { genHtmlText.serdce += serdceGranica2RadioButton.Text; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void iazikChanged(object sender, EventArgs e)//пань осмотря языка
        {

            genHtmlText.iazik = "";
            foreach (CheckBox tb in YazikPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {

                    genHtmlText.iazik += tb.Text;
                    genHtmlText.iazik += ", ";
                    if (tb.Text == Yazik4СcheckBox.Text)
                    { genHtmlText.iazik = cramblDel(genHtmlText.iazik) + " " + YazikTextBox.Text + " "; }
                }
            }
            genHtmlText.iazik = cramblDel(genHtmlText.iazik);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void ZivodChanged(object sender, EventArgs e)//панель осмотра живота
        {
            genHtmlText.zivod = "";
            foreach (CheckBox tb in JivotPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.zivod += tb.Text;
                    genHtmlText.zivod += ", ";
                    if (tb.Text == Jivot7CheckBox.Text)
                    { genHtmlText.zivod = cramblDel(genHtmlText.zivod) + " " + JivotTextBox.Text + " "; }
                }
            }
            genHtmlText.zivod = cramblDel(genHtmlText.zivod);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void pechenChanged(object sender, EventArgs e)//панель осмотра печени
        {
            genHtmlText.pechen = "";
            foreach (CheckBox tb in pehenKraiPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.pechen += tb.Text;
                    genHtmlText.pechen += ", ";
                }
            }
            if (!Pechen2CheckBox.Checked)
            {
                PechenKraiPanel.Enabled = true;
                genHtmlText.pechen += "Край: ";
                foreach (CheckBox tb in PechenKraiPanel.Controls.OfType<CheckBox>())
                {

                    if (tb.Checked)
                    {
                        genHtmlText.pechen += tb.Text;
                        genHtmlText.pechen += ", ";
                        if (tb.Text == PechenKrai3CheckBox.Text)
                        {
                            genHtmlText.pechen = cramblDel(genHtmlText.pechen) + " " + PechenKraiMaskedTextBox2
                                  .Text + " ";
                        }
                    }
                }

            }
            else { PechenKraiPanel.Enabled = false; }

            genHtmlText.pechen = cramblDel(genHtmlText.pechen);
            webBrowser1.DocumentText = genHtmlText.compleateText();

        }

        private void SelezionkaChanged(object sender, EventArgs e)//панель осмотра селезенки
        {
            genHtmlText.selizenka = "";
            foreach (CheckBox tb in SelezenPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.selizenka += tb.Text;
                    genHtmlText.selizenka += ", ";
                    if (tb.Text == Selezen2CheckBox.Text)
                    { genHtmlText.selizenka = cramblDel(genHtmlText.selizenka) + " " + SelezenMaskedTextBox2.Text + " "; }
                }
            }
            genHtmlText.selizenka = cramblDel(genHtmlText.selizenka);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void VidSystemChanged(object sender, EventArgs e)//панель осмотра выделительной системы
        {
            genHtmlText.pochki = "";
            foreach (CheckBox tb in PockiLanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.pochki += tb.Text;
                    genHtmlText.pochki += ", ";
                }
            }
            foreach (RadioButton tb in PockiLanel.Controls.OfType<RadioButton>())
            {
                if (tb.Checked)
                {
                    genHtmlText.pochki += " " + PocolachovanLabel.Text + " ";
                    genHtmlText.pochki += tb.Text;
                }
            }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void MocheispuskanieChanged(object sender, EventArgs e)//панель осмотра мочеиспускания
        {
            genHtmlText.mocheIspuskanie = "";
            foreach (CheckBox tb in MocheispusPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.mocheIspuskanie += tb.Text;
                    genHtmlText.mocheIspuskanie += ", ";
                    if (tb.Text == Moche3СheckBox.Text)
                    { genHtmlText.mocheIspuskanie = cramblDel(genHtmlText.mocheIspuskanie) + " " + MocheiTextBox.Text + " "; }
                }
            }
            genHtmlText.mocheIspuskanie = cramblDel(genHtmlText.mocheIspuskanie);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void StylChanged(object sender, EventArgs e)//панель осмотра стула
        {
            genHtmlText.styl = "";
            foreach (CheckBox tb in stylPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.styl += tb.Text;
                    genHtmlText.styl += ", ";
                    if (tb.Text == Stul4СheckBox.Text)
                    { genHtmlText.styl = cramblDel(genHtmlText.styl) + " " + StylTextBox.Text + " "; }
                }
            }
            genHtmlText.styl = cramblDel(genHtmlText.styl);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void OtekiChanged(object sender, EventArgs e)//панель осмотра отеков
        {
            genHtmlText.oteki = "";
            foreach (CheckBox tb in OtekiPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    genHtmlText.oteki += tb.Text;
                    genHtmlText.oteki += ", ";
                }
            }
            genHtmlText.oteki = cramblDel(genHtmlText.oteki);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void DiagnozTextBox_TextChanged(object sender, EventArgs e)//диагноз
        {
            genHtmlText.diagnoz = DiagnozTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void NaznachChanged(object sender, EventArgs e)//назначения
        {
            genHtmlText.naznachenia = "";
            foreach (CheckBox tb in NaznacheniaPanel.Controls.OfType<CheckBox>())
            {
                if (tb.Checked)
                {
                    if (tb.Text != Naznachenia13СheckBox.Text)
                    { genHtmlText.naznachenia += tb.Text;
                        genHtmlText.naznachenia += ", ";
                    }


                    if (tb.Text == Naznachenia13СheckBox.Text)
                    { genHtmlText.naznachenia = cramblDel(genHtmlText.naznachenia) + " " + NaznacheniaTextBox.Text + " "; }
                }
            }
            genHtmlText.naznachenia = cramblDel(genHtmlText.naznachenia);
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ReceptRichTextBox_TextChanged(object sender, EventArgs e)//рецепты
        {
            genHtmlText.recepti = ReceptRichTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void LgotaRichTextBox_TextChanged(object sender, EventArgs e)//льготные рецепты
        {
            genHtmlText.lgotnieRecepti = LgotaRichTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void RecomendRichTextBox_TextChanged(object sender, EventArgs e)//рекомендации
        {
            genHtmlText.recomendacii = RecomendRichTextBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void DispanserСomboBox_SelectedIndexChanged(object sender, EventArgs e)//группа Д осмотра
        {
            genHtmlText.gruppaNabludenia = DispanserСomboBox.Text;
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void ListNetrudosposobnostiChanged(object sender, EventArgs e)//лист нетрудоспособности
        {
            genHtmlText.listNetrudosposobnosti = "";
            if (ListNetrudosposobnostiCheckBox.Checked)
            {
                listNetrudoPanel.Enabled = true;
                genHtmlText.listNetrudosposobnosti = "<br/>Лист временной нетрудоспособности: ";
                genHtmlText.listNetrudosposobnosti = SeriaLabel.Text + " ";
                genHtmlText.listNetrudosposobnosti += SeriaTextBox.Text + " ";
                genHtmlText.listNetrudosposobnosti += NomerLabel.Text + " " + nomerbolnTextBox.Text + "<br/>";
                genHtmlText.listNetrudosposobnosti += SrokLabel.Text + " " + SrokDateTimePicker1.Text + " " + PriemLabel.Text + " " + dateTimePicker1.Text;
                genHtmlText.listNetrudosposobnosti += "<br/>" + PovtorLabel.Text + " " + PovtorDateTimePicker2.Text;
                genHtmlText.listNetrudosposobnosti += "<br/>" + "Режим: амбулаторный";
                if (ActivePoshCheckBox.Checked)
                {
                    ActivDateTimePicker2.Enabled = true;
                    genHtmlText.listNetrudosposobnosti += "<br/>" + ActivePoshCheckBox.Text + " " + ActivDateTimePicker2.Text; }
                else { ActivDateTimePicker2.Enabled = false; }

            }
            else { listNetrudoPanel.Enabled = false; }
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)//событие меню сохранения файла
        {
            SaveFileDialog saveControlToFile = new SaveFileDialog();
            saveControlToFile.AddExtension = true;
            saveControlToFile.DefaultExt = "pacient";
            if (FioTextBox.Text != "")
            {
                saveControlToFile.FileName = FioTextBox.Text;
            }
            if (saveControlToFile.ShowDialog() == DialogResult.Cancel)
                return;
            using (System.IO.StreamWriter fileWrite = new System.IO.StreamWriter(saveControlToFile.OpenFile()))
                parseControl(genPanel, fileWrite);
        }

        private void открытьВWordToolStripMenuItem_Click(object sender, EventArgs e)//открыть документ в word 
        {
            genHtmlText.OpenToWord();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//удаляем временные файлы при выходе
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath.ToString() + "\\temp");
            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                try
                { file.Delete(); }
                catch { };
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)//обнуление формы
        {
            isFirstStart.isOpenFile = false;
            Form1 newform = new Form1();
            newform.Show();
            this.Dispose(false);
            genHtmlText = new gentext();
           
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)//печать документа
        {
            webBrowser1.ShowPrintDialog();
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)//размер текста
        {
            if (maskedTextBox2.Text != "" && Convert.ToInt32(maskedTextBox2.Text) != 0)
            {
                genHtmlText.textSize = Convert.ToInt32(maskedTextBox2.Text);
                webBrowser1.DocumentText = genHtmlText.compleateText();
            }
        }

        private void saveControl(Control cn, System.IO.StreamWriter fileWrite)//сохраняем значение контролов
        {

            if (cn is CheckBox)//если выделяется, сохраняется только название
            {
                CheckBox cb = (CheckBox)cn;
                if (cb.Checked)
                { fileWrite.WriteLine("[" + cb.Name + "]" + ";"); }
            }

            if (cn is RadioButton)
            {
                RadioButton cb = (RadioButton)cn;
                if (cb.Checked)
                { fileWrite.WriteLine("[" + cb.Name + "]" + ";"); }
            }

            if (cn is TextBox || cn is ComboBox || cn is RichTextBox || cn is DateTimePicker || cn is MaskedTextBox)//если контрол содержит текст
                                                                                                                    //то сохраняем его
            { if (cn.Text != "") fileWrite.WriteLine("[" + cn.Name + "]" + "{" + cn.Text + "}" + ";"); }
        }

        private void parseControl(Panel recursPan, System.IO.StreamWriter fileWrite)//рекурсивный осмотр панелей
        {
            foreach (Control cn in recursPan.Controls)
            {
                if (cn is Panel)
                { parseControl((Panel)cn, fileWrite); }
                saveControl(cn, fileWrite);
            }
        }

        private void SKFChanged(object sender, EventArgs e)//расчет СКФ
        {
            try
            {
                int day = Int32.Parse(maskedTextBox1.Text.Substring(0, 2));
                int mounth = Int32.Parse(maskedTextBox1.Text.Substring(3, 2));
                int year = Int32.Parse(maskedTextBox1.Text.Substring(6, 4));
                var bbday = new DateTime(year, mounth, day);
                var today = DateTime.Today;
                var age = new DateTime(DateTime.Now.Subtract(bbday).Ticks).Year - 1;
                double skr = 0;
                if (SexComboBox.SelectedIndex == 0)
                    skr = Math.Round((((140 - age) * Double.Parse(WeightMaskedTextBox.Text)) / Double.Parse(SkfMaskedTextBox.Text) * 1.23), 2);
                if (SexComboBox.SelectedIndex == 1)
                    skr = Math.Round((((140 - age) * Double.Parse(WeightMaskedTextBox.Text)) / Double.Parse(SkfMaskedTextBox.Text) * 1.05), 2);
                if (skr >= 90) { SKFTextLabel.Text = "1 ст - нормальная или повышенная СКФ"; }
                if (skr > 60 && skr < 89) { SKFTextLabel.Text = "2 ст - начальное снижение СКФ"; }
                if (skr > 30 && skr < 59) { SKFTextLabel.Text = "3 ст - умеренное снижение СКФ"; }
                if (skr > 15 && skr < 29) { SKFTextLabel.Text = "4 ст - выраженное снижение СКФ"; }
                if (skr < 15) { SKFTextLabel.Text = "5 ст - почечная недостаточность"; }
                SkfValueLabel.Text = skr.ToString();
                if (WeightMaskedTextBox.Text != "")
                { genHtmlText.skfValue = skr.ToString(); }
                else { genHtmlText.skfValue = ""; }
            }

            catch { genHtmlText.skfValue = ""; };
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)//событие меню загрузить файл
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.Cancel)
                return;
            isFirstStart.isOpenFile = true;
            isFirstStart.openFile = openFile;
            Form1 newform = new Form1();
            newform.Show();
            this.Dispose(false);
            genHtmlText = new gentext();
                  
        }

        void ControlContainsText(StreamReader fileRead)//метод обработки строк сохраненного файла
        {
            while (!fileRead.EndOfStream)

            {
                string readLine = fileRead.ReadLine() + "\n";
                while (readLine[readLine.Length - 2] != ';')
                { readLine += fileRead.ReadLine() + "\n"; }
                if (readLine.LastIndexOf(']') == readLine.Length - 3)
                {
                    readLine = readLine.Replace("[", "");
                    readLine = readLine.Replace("]", "");
                    readLine = readLine.Replace(";", "");
                    readLine = readLine.Replace("\n", "");
                    openFileControl(readLine);
                }
                else
                {
                    int nameStart = readLine.IndexOf('[');
                    int nameEnd = readLine.IndexOf(']');
                    string name = readLine.Substring(nameStart + 1, nameEnd - nameStart - 1);
                    int StartText = readLine.IndexOf('{');
                    int endText = readLine.IndexOf('}');
                    string text = readLine.Substring(StartText + 1, endText - StartText - 1);
                    openFileControl(name, text);
                }

            }
        }

        void openFileControl(string name, string text = "")//загружаем значение контролов
        {
            var cnM = this.Controls.Find(name, true);
            if (cnM.Length > 0)
            {
                Control cn = cnM[0];
                if (cn is CheckBox)
                {
                    CheckBox cb = (CheckBox)cn;
                    cb.Checked = true;
                }
                if (cn is RadioButton)
                {
                    RadioButton cb = (RadioButton)cn;
                    cb.Checked = true;
                }
                if (cn is TextBox || cn is ComboBox || cn is RichTextBox || cn is DateTimePicker || cn is MaskedTextBox)
                {
                    cn.Text = text;
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComplainTextBox_TextChanged(sender,e);
            AnamneztextBox_TextChanged(sender, e);
            DiagnozTextBox_TextChanged(sender, e);
            ReceptRichTextBox_TextChanged(sender, e);
            LgotaRichTextBox_TextChanged(sender, e);
            RecomendRichTextBox_TextChanged(sender, e);
            examinationDateTimePicker_ValueChanged(sender, e);
            ListNetrudosposobnostiChanged(sender, e);
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            refresh.Checked = true;
        }

        private void refresh_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = genHtmlText.compleateText();
        }
    }

    static class isFirstStart
    { public static bool isOpenFile = false;
     public  static OpenFileDialog openFile;
    }

   
    
}