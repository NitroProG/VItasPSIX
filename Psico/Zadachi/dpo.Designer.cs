namespace Psico
{
    partial class dpo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.hint = new System.Windows.Forms.ToolTip(this.components);
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PowderBlue;
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1366, 768);
            this.panel2.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.richTextBox2);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1345, 747);
            this.panel1.TabIndex = 2;
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.BackColor = System.Drawing.Color.PowderBlue;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.Location = new System.Drawing.Point(393, 444);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(216, 34);
            this.button5.TabIndex = 44;
            this.button5.Text = "Следующий рисунок";
            this.hint.SetToolTip(this.button5, "Показать следующий рисунок");
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.OpenNextPictureBox);
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.BackColor = System.Drawing.Color.PowderBlue;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(2, 444);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 34);
            this.button4.TabIndex = 43;
            this.button4.Text = "Посмотреть";
            this.hint.SetToolTip(this.button4, "Просмотреть данные по выбранной методике");
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.ShowListBoxInfo);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(3, 532);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(413, 20);
            this.label5.TabIndex = 42;
            this.label5.Text = "Напишите результат проверки каждой гипотезы";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.PowderBlue;
            this.richTextBox2.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox2.Location = new System.Drawing.Point(3, 555);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(606, 111);
            this.richTextBox2.TabIndex = 41;
            this.richTextBox2.Text = "";
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.listBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(3, 224);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(606, 214);
            this.listBox1.TabIndex = 40;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBoxItemChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(3, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(606, 20);
            this.label4.TabIndex = 39;
            this.label4.Text = "С помощью списка предлагаемых методик проверьте каждую гипотезу";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.richTextBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 142);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(606, 76);
            this.richTextBox1.TabIndex = 38;
            this.richTextBox1.Text = "";
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.Color.PowderBlue;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(1311, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(29, 28);
            this.button3.TabIndex = 33;
            this.button3.Text = "Х";
            this.hint.SetToolTip(this.button3, "Выход из программы");
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.ExitProgram);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.Color.PowderBlue;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("MS Reference Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(24, 698);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 35);
            this.button2.TabIndex = 32;
            this.button2.Text = "НАЗАД";
            this.hint.SetToolTip(this.button2, "Переход на главную форму");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.OpenMainForm);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(19, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1309, 64);
            this.label3.TabIndex = 5;
            this.label3.Text = "Запрос матери: \"Очень разборчив в еде. Ест плохо. Не поймёшь в каком настроении н" +
    "аходится. В чем дело?\"\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(353, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 34);
            this.label1.TabIndex = 3;
            this.label1.Text = "Задача №";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.PowderBlue;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("MS Reference Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(1172, 698);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "ПРОДОЛЖИТЬ";
            this.hint.SetToolTip(this.button1, "Переход на главную форму");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.OpenMainForm);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.TimeOnForm);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.TimeOnSeeListboxInfo);
            // 
            // hint
            // 
            this.hint.AutoPopDelay = 5000;
            this.hint.InitialDelay = 1;
            this.hint.ReshowDelay = 100;
            // 
            // dpo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1366, 768);
            this.Name = "dpo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dpo";
            this.Load += new System.EventHandler(this.FormLoad);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolTip hint;
    }
}