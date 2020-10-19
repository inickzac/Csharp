namespace seaBatlteСlient
{
    partial class ClientForm
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
            this.playersListListBox = new System.Windows.Forms.ListBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.registrationToServerbutton = new System.Windows.Forms.Button();
            this.shipOrientationComboBox = new System.Windows.Forms.ComboBox();
            this.letsStartButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.HorisontalradioButton = new System.Windows.Forms.RadioButton();
            this.verticalradioButton = new System.Windows.Forms.RadioButton();
            this.backButtom = new System.Windows.Forms.Button();
            this.disconnectlabel = new System.Windows.Forms.Label();
            this.Namelabel = new System.Windows.Forms.Label();
            this.waitlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playersListListBox
            // 
            this.playersListListBox.FormattingEnabled = true;
            this.playersListListBox.ItemHeight = 16;
            this.playersListListBox.Location = new System.Drawing.Point(12, 145);
            this.playersListListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.playersListListBox.Name = "playersListListBox";
            this.playersListListBox.Size = new System.Drawing.Size(392, 228);
            this.playersListListBox.TabIndex = 0;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(150, 54);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(127, 22);
            this.nameTextBox.TabIndex = 1;
            // 
            // registrationToServerbutton
            // 
            this.registrationToServerbutton.Location = new System.Drawing.Point(217, 9);
            this.registrationToServerbutton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.registrationToServerbutton.Name = "registrationToServerbutton";
            this.registrationToServerbutton.Size = new System.Drawing.Size(191, 41);
            this.registrationToServerbutton.TabIndex = 2;
            this.registrationToServerbutton.Text = "Зарегистрироваться";
            this.registrationToServerbutton.UseVisualStyleBackColor = true;
            this.registrationToServerbutton.Click += new System.EventHandler(this.regToServer_Click);
            // 
            // shipOrientationComboBox
            // 
            this.shipOrientationComboBox.FormattingEnabled = true;
            this.shipOrientationComboBox.Items.AddRange(new object[] {
            "Вертикально",
            "Горизонтально"});
            this.shipOrientationComboBox.Location = new System.Drawing.Point(283, 54);
            this.shipOrientationComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.shipOrientationComboBox.Name = "shipOrientationComboBox";
            this.shipOrientationComboBox.Size = new System.Drawing.Size(121, 24);
            this.shipOrientationComboBox.TabIndex = 3;
            this.shipOrientationComboBox.Text = "Вертикально";
            // 
            // letsStartButton
            // 
            this.letsStartButton.Location = new System.Drawing.Point(15, 9);
            this.letsStartButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.letsStartButton.Name = "letsStartButton";
            this.letsStartButton.Size = new System.Drawing.Size(191, 42);
            this.letsStartButton.TabIndex = 4;
            this.letsStartButton.Text = "Начать игру с выбранным пользователем";
            this.letsStartButton.UseVisualStyleBackColor = true;
            this.letsStartButton.Click += new System.EventHandler(this.button6_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1011, 379);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1011, 395);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // HorisontalradioButton
            // 
            this.HorisontalradioButton.AutoSize = true;
            this.HorisontalradioButton.Location = new System.Drawing.Point(527, 374);
            this.HorisontalradioButton.Margin = new System.Windows.Forms.Padding(4);
            this.HorisontalradioButton.Name = "HorisontalradioButton";
            this.HorisontalradioButton.Size = new System.Drawing.Size(115, 21);
            this.HorisontalradioButton.TabIndex = 0;
            this.HorisontalradioButton.TabStop = true;
            this.HorisontalradioButton.Text = "Вертикально";
            this.HorisontalradioButton.UseVisualStyleBackColor = true;
            this.HorisontalradioButton.CheckedChanged += new System.EventHandler(this.VerticalradioButton_CheckedChanged);
            // 
            // verticalradioButton
            // 
            this.verticalradioButton.AutoSize = true;
            this.verticalradioButton.Location = new System.Drawing.Point(713, 374);
            this.verticalradioButton.Margin = new System.Windows.Forms.Padding(4);
            this.verticalradioButton.Name = "verticalradioButton";
            this.verticalradioButton.Size = new System.Drawing.Size(130, 21);
            this.verticalradioButton.TabIndex = 1;
            this.verticalradioButton.TabStop = true;
            this.verticalradioButton.Text = "Горизонтально";
            this.verticalradioButton.UseVisualStyleBackColor = true;
            this.verticalradioButton.CheckedChanged += new System.EventHandler(this.HorizontalradioButton_CheckedChanged);
            // 
            // backButtom
            // 
            this.backButtom.Location = new System.Drawing.Point(477, 324);
            this.backButtom.Margin = new System.Windows.Forms.Padding(4);
            this.backButtom.Name = "backButtom";
            this.backButtom.Size = new System.Drawing.Size(100, 28);
            this.backButtom.TabIndex = 9;
            this.backButtom.Text = "назад";
            this.backButtom.UseVisualStyleBackColor = true;
            this.backButtom.Click += new System.EventHandler(this.BackButtom_Click);
            // 
            // disconnectlabel
            // 
            this.disconnectlabel.AutoSize = true;
            this.disconnectlabel.Location = new System.Drawing.Point(644, 22);
            this.disconnectlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.disconnectlabel.Name = "disconnectlabel";
            this.disconnectlabel.Size = new System.Drawing.Size(0, 17);
            this.disconnectlabel.TabIndex = 10;
            this.disconnectlabel.Visible = false;
            // 
            // Namelabel
            // 
            this.Namelabel.AutoSize = true;
            this.Namelabel.Location = new System.Drawing.Point(17, 54);
            this.Namelabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Namelabel.Name = "Namelabel";
            this.Namelabel.Size = new System.Drawing.Size(92, 17);
            this.Namelabel.TabIndex = 11;
            this.Namelabel.Text = "Введите имя";
            // 
            // waitlabel
            // 
            this.waitlabel.Location = new System.Drawing.Point(20, 91);
            this.waitlabel.Name = "waitlabel";
            this.waitlabel.Size = new System.Drawing.Size(384, 43);
            this.waitlabel.TabIndex = 12;
            this.waitlabel.Text = "Выбертите игрока из списка или ожидайте соединения другого игрока";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 433);
            this.Controls.Add(this.waitlabel);
            this.Controls.Add(this.Namelabel);
            this.Controls.Add(this.disconnectlabel);
            this.Controls.Add(this.backButtom);
            this.Controls.Add(this.HorisontalradioButton);
            this.Controls.Add(this.verticalradioButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.letsStartButton);
            this.Controls.Add(this.shipOrientationComboBox);
            this.Controls.Add(this.registrationToServerbutton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.playersListListBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ClientForm";
            this.Text = "client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing_1);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Client_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Client_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Client_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox playersListListBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button registrationToServerbutton;
        private System.Windows.Forms.ComboBox shipOrientationComboBox;
        private System.Windows.Forms.Button letsStartButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton HorisontalradioButton;
        private System.Windows.Forms.RadioButton verticalradioButton;
        private System.Windows.Forms.Button backButtom;
        private System.Windows.Forms.Label disconnectlabel;
        private System.Windows.Forms.Label Namelabel;
        private System.Windows.Forms.Label waitlabel;
    }
}

