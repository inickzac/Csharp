namespace CIPHER
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ciperLFSRRadioBut = new System.Windows.Forms.RadioButton();
            this.GeffeRadioBut = new System.Windows.Forms.RadioButton();
            this.RC4RadioBut = new System.Windows.Forms.RadioButton();
            this.LFSR1Textbox = new System.Windows.Forms.TextBox();
            this.LFSR2Textbox = new System.Windows.Forms.TextBox();
            this.LFSR3Textbox = new System.Windows.Forms.TextBox();
            this.openFileButtom = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.cryptButtom = new System.Windows.Forms.Button();
            this.deciperButton = new System.Windows.Forms.Button();
            this.PrLabel = new System.Windows.Forms.Label();
            this.savePathTextBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.readedLabel = new System.Windows.Forms.Label();
            this.panelControlFalseInChipering = new System.Windows.Forms.Panel();
            this.RC4textBox = new System.Windows.Forms.TextBox();
            this.Savebut = new System.Windows.Forms.Button();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.OriginalTexttextBox = new System.Windows.Forms.TextBox();
            this.ProcessTextBox = new System.Windows.Forms.TextBox();
            this.originalTexr = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyOriglabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RegistrLabellabel = new System.Windows.Forms.Label();
            this.Keylabel = new System.Windows.Forms.Label();
            this.GenKeytextBox = new System.Windows.Forms.TextBox();
            this.panelControlFalseInChipering.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ciperLFSRRadioBut
            // 
            this.ciperLFSRRadioBut.AutoSize = true;
            this.ciperLFSRRadioBut.Location = new System.Drawing.Point(277, 21);
            this.ciperLFSRRadioBut.Name = "ciperLFSRRadioBut";
            this.ciperLFSRRadioBut.Size = new System.Drawing.Size(52, 17);
            this.ciperLFSRRadioBut.TabIndex = 4;
            this.ciperLFSRRadioBut.TabStop = true;
            this.ciperLFSRRadioBut.Text = "LFSR";
            this.ciperLFSRRadioBut.UseVisualStyleBackColor = true;
            this.ciperLFSRRadioBut.Click += new System.EventHandler(this.CiperLFSRRadioBut_Click);
            // 
            // GeffeRadioBut
            // 
            this.GeffeRadioBut.AutoSize = true;
            this.GeffeRadioBut.Location = new System.Drawing.Point(277, 44);
            this.GeffeRadioBut.Name = "GeffeRadioBut";
            this.GeffeRadioBut.Size = new System.Drawing.Size(94, 17);
            this.GeffeRadioBut.TabIndex = 5;
            this.GeffeRadioBut.TabStop = true;
            this.GeffeRadioBut.Text = "Схема Геффе";
            this.GeffeRadioBut.UseVisualStyleBackColor = true;
            this.GeffeRadioBut.Click += new System.EventHandler(this.GeffeRadioBut_Click);
            // 
            // RC4RadioBut
            // 
            this.RC4RadioBut.AutoSize = true;
            this.RC4RadioBut.Location = new System.Drawing.Point(277, 67);
            this.RC4RadioBut.Name = "RC4RadioBut";
            this.RC4RadioBut.Size = new System.Drawing.Size(46, 17);
            this.RC4RadioBut.TabIndex = 6;
            this.RC4RadioBut.TabStop = true;
            this.RC4RadioBut.Text = "RC4";
            this.RC4RadioBut.UseVisualStyleBackColor = true;
            this.RC4RadioBut.Click += new System.EventHandler(this.RC4RadioBut_Click);
            // 
            // LFSR1Textbox
            // 
            this.LFSR1Textbox.Location = new System.Drawing.Point(11, 15);
            this.LFSR1Textbox.MaxLength = 23;
            this.LFSR1Textbox.Name = "LFSR1Textbox";
            this.LFSR1Textbox.Size = new System.Drawing.Size(252, 20);
            this.LFSR1Textbox.TabIndex = 7;
            this.LFSR1Textbox.TextChanged += new System.EventHandler(this.LFSR1Textbox_TextChanged);
            this.LFSR1Textbox.VisibleChanged += new System.EventHandler(this.LFSR1Textbox_VisibleChanged);
            this.LFSR1Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LFSR1Textbox_KeyPress);
            // 
            // LFSR2Textbox
            // 
            this.LFSR2Textbox.Location = new System.Drawing.Point(11, 41);
            this.LFSR2Textbox.MaxLength = 31;
            this.LFSR2Textbox.Name = "LFSR2Textbox";
            this.LFSR2Textbox.Size = new System.Drawing.Size(252, 20);
            this.LFSR2Textbox.TabIndex = 8;
            this.LFSR2Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LFSR2Textbox_KeyPress);
            // 
            // LFSR3Textbox
            // 
            this.LFSR3Textbox.Location = new System.Drawing.Point(11, 67);
            this.LFSR3Textbox.MaxLength = 39;
            this.LFSR3Textbox.Name = "LFSR3Textbox";
            this.LFSR3Textbox.Size = new System.Drawing.Size(252, 20);
            this.LFSR3Textbox.TabIndex = 9;
            this.LFSR3Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LFSR3Textbox_KeyPress);
            // 
            // openFileButtom
            // 
            this.openFileButtom.Location = new System.Drawing.Point(200, 150);
            this.openFileButtom.Name = "openFileButtom";
            this.openFileButtom.Size = new System.Drawing.Size(147, 23);
            this.openFileButtom.TabIndex = 10;
            this.openFileButtom.Text = "Открыть файл";
            this.openFileButtom.UseVisualStyleBackColor = true;
            this.openFileButtom.Click += new System.EventHandler(this.OpenFileButtom_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(14, 150);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(176, 20);
            this.pathTextBox.TabIndex = 11;
            // 
            // cryptButtom
            // 
            this.cryptButtom.Location = new System.Drawing.Point(14, 176);
            this.cryptButtom.Name = "cryptButtom";
            this.cryptButtom.Size = new System.Drawing.Size(147, 23);
            this.cryptButtom.TabIndex = 12;
            this.cryptButtom.Text = "Зашифровать";
            this.cryptButtom.UseVisualStyleBackColor = true;
            this.cryptButtom.Click += new System.EventHandler(this.CryptButtom_Click);
            // 
            // deciperButton
            // 
            this.deciperButton.Location = new System.Drawing.Point(200, 176);
            this.deciperButton.Name = "deciperButton";
            this.deciperButton.Size = new System.Drawing.Size(147, 23);
            this.deciperButton.TabIndex = 13;
            this.deciperButton.Text = "Расшифровать";
            this.deciperButton.UseVisualStyleBackColor = true;
            this.deciperButton.Click += new System.EventHandler(this.DeciperButton_Click);
            // 
            // PrLabel
            // 
            this.PrLabel.AutoSize = true;
            this.PrLabel.Location = new System.Drawing.Point(10, 29);
            this.PrLabel.Name = "PrLabel";
            this.PrLabel.Size = new System.Drawing.Size(37, 13);
            this.PrLabel.TabIndex = 14;
            this.PrLabel.Text = "tyrtyrty";
            // 
            // savePathTextBox
            // 
            this.savePathTextBox.Location = new System.Drawing.Point(14, 121);
            this.savePathTextBox.Name = "savePathTextBox";
            this.savePathTextBox.Size = new System.Drawing.Size(176, 20);
            this.savePathTextBox.TabIndex = 15;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1127, 23);
            this.progressBar1.TabIndex = 16;
            // 
            // readedLabel
            // 
            this.readedLabel.AutoSize = true;
            this.readedLabel.Location = new System.Drawing.Point(131, 29);
            this.readedLabel.Name = "readedLabel";
            this.readedLabel.Size = new System.Drawing.Size(37, 13);
            this.readedLabel.TabIndex = 17;
            this.readedLabel.Text = "tyrtyrty";
            // 
            // panelControlFalseInChipering
            // 
            this.panelControlFalseInChipering.Controls.Add(this.RC4textBox);
            this.panelControlFalseInChipering.Controls.Add(this.Savebut);
            this.panelControlFalseInChipering.Controls.Add(this.GeffeRadioBut);
            this.panelControlFalseInChipering.Controls.Add(this.openFileButtom);
            this.panelControlFalseInChipering.Controls.Add(this.cryptButtom);
            this.panelControlFalseInChipering.Controls.Add(this.LFSR2Textbox);
            this.panelControlFalseInChipering.Controls.Add(this.LFSR3Textbox);
            this.panelControlFalseInChipering.Controls.Add(this.pathTextBox);
            this.panelControlFalseInChipering.Controls.Add(this.LFSR1Textbox);
            this.panelControlFalseInChipering.Controls.Add(this.savePathTextBox);
            this.panelControlFalseInChipering.Controls.Add(this.deciperButton);
            this.panelControlFalseInChipering.Controls.Add(this.RC4RadioBut);
            this.panelControlFalseInChipering.Controls.Add(this.ciperLFSRRadioBut);
            this.panelControlFalseInChipering.Location = new System.Drawing.Point(2, 12);
            this.panelControlFalseInChipering.Name = "panelControlFalseInChipering";
            this.panelControlFalseInChipering.Size = new System.Drawing.Size(375, 218);
            this.panelControlFalseInChipering.TabIndex = 18;
            // 
            // RC4textBox
            // 
            this.RC4textBox.Location = new System.Drawing.Point(11, 93);
            this.RC4textBox.Name = "RC4textBox";
            this.RC4textBox.Size = new System.Drawing.Size(252, 20);
            this.RC4textBox.TabIndex = 17;
            // 
            // Savebut
            // 
            this.Savebut.Location = new System.Drawing.Point(200, 121);
            this.Savebut.Name = "Savebut";
            this.Savebut.Size = new System.Drawing.Size(147, 23);
            this.Savebut.TabIndex = 16;
            this.Savebut.Text = "Путь сохранения файла";
            this.Savebut.UseVisualStyleBackColor = true;
            this.Savebut.Click += new System.EventHandler(this.Savebut_Click);
            // 
            // panelProgress
            // 
            this.panelProgress.Controls.Add(this.readedLabel);
            this.panelProgress.Controls.Add(this.progressBar1);
            this.panelProgress.Controls.Add(this.PrLabel);
            this.panelProgress.Location = new System.Drawing.Point(2, 597);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(1133, 48);
            this.panelProgress.TabIndex = 19;
            // 
            // OriginalTexttextBox
            // 
            this.OriginalTexttextBox.Location = new System.Drawing.Point(648, 27);
            this.OriginalTexttextBox.Multiline = true;
            this.OriginalTexttextBox.Name = "OriginalTexttextBox";
            this.OriginalTexttextBox.Size = new System.Drawing.Size(241, 564);
            this.OriginalTexttextBox.TabIndex = 20;
            // 
            // ProcessTextBox
            // 
            this.ProcessTextBox.Location = new System.Drawing.Point(895, 27);
            this.ProcessTextBox.Multiline = true;
            this.ProcessTextBox.Name = "ProcessTextBox";
            this.ProcessTextBox.Size = new System.Drawing.Size(240, 564);
            this.ProcessTextBox.TabIndex = 21;
            // 
            // originalTexr
            // 
            this.originalTexr.AutoSize = true;
            this.originalTexr.Location = new System.Drawing.Point(645, 9);
            this.originalTexr.Name = "originalTexr";
            this.originalTexr.Size = new System.Drawing.Size(113, 13);
            this.originalTexr.TabIndex = 22;
            this.originalTexr.Text = "Оригинальный текст";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(892, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Обработанный текст";
            // 
            // KeyOriglabel
            // 
            this.KeyOriglabel.AutoSize = true;
            this.KeyOriglabel.Location = new System.Drawing.Point(400, 9);
            this.KeyOriglabel.Name = "KeyOriglabel";
            this.KeyOriglabel.Size = new System.Drawing.Size(130, 13);
            this.KeyOriglabel.TabIndex = 18;
            this.KeyOriglabel.Text = "Сгенерированный ключ ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RegistrLabellabel);
            this.panel1.Controls.Add(this.Keylabel);
            this.panel1.Location = new System.Drawing.Point(5, 252);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 109);
            this.panel1.TabIndex = 20;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // RegistrLabellabel
            // 
            this.RegistrLabellabel.AutoSize = true;
            this.RegistrLabellabel.Location = new System.Drawing.Point(11, 4);
            this.RegistrLabellabel.Name = "RegistrLabellabel";
            this.RegistrLabellabel.Size = new System.Drawing.Size(161, 13);
            this.RegistrLabellabel.TabIndex = 20;
            this.RegistrLabellabel.Text = "Исходное значение регистров";
            // 
            // Keylabel
            // 
            this.Keylabel.Location = new System.Drawing.Point(8, 31);
            this.Keylabel.Name = "Keylabel";
            this.Keylabel.Size = new System.Drawing.Size(360, 64);
            this.Keylabel.TabIndex = 19;
            // 
            // GenKeytextBox
            // 
            this.GenKeytextBox.Location = new System.Drawing.Point(403, 26);
            this.GenKeytextBox.Multiline = true;
            this.GenKeytextBox.Name = "GenKeytextBox";
            this.GenKeytextBox.Size = new System.Drawing.Size(239, 565);
            this.GenKeytextBox.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 647);
            this.Controls.Add(this.KeyOriglabel);
            this.Controls.Add(this.GenKeytextBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.originalTexr);
            this.Controls.Add(this.ProcessTextBox);
            this.Controls.Add(this.OriginalTexttextBox);
            this.Controls.Add(this.panelControlFalseInChipering);
            this.Controls.Add(this.panelProgress);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelControlFalseInChipering.ResumeLayout(false);
            this.panelControlFalseInChipering.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton ciperLFSRRadioBut;
        private System.Windows.Forms.RadioButton GeffeRadioBut;
        private System.Windows.Forms.RadioButton RC4RadioBut;
        private System.Windows.Forms.TextBox LFSR1Textbox;
        private System.Windows.Forms.TextBox LFSR2Textbox;
        private System.Windows.Forms.TextBox LFSR3Textbox;
        private System.Windows.Forms.Button openFileButtom;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button cryptButtom;
        private System.Windows.Forms.Button deciperButton;
        private System.Windows.Forms.Label PrLabel;
        private System.Windows.Forms.TextBox savePathTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label readedLabel;
        private System.Windows.Forms.Panel panelControlFalseInChipering;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.Button Savebut;
        private System.Windows.Forms.TextBox RC4textBox;
        private System.Windows.Forms.TextBox OriginalTexttextBox;
        private System.Windows.Forms.TextBox ProcessTextBox;
        private System.Windows.Forms.Label originalTexr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label KeyOriglabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Keylabel;
        private System.Windows.Forms.TextBox GenKeytextBox;
        private System.Windows.Forms.Label RegistrLabellabel;
    }
}

