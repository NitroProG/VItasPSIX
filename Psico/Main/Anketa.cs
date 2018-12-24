using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Psico
{
    public partial class Anketa : Form
    {
        public Anketa()
        {
            InitializeComponent();
        }

        private void Anketa_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "ФИО";
            richTextBox2.Text = "Образование";
            richTextBox3.Text = "Место работы и стаж";
            richTextBox4.Text = "Год обучения";
            richTextBox5.Text = "Возраст";
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox1.Text == "ФИО")
            {
                richTextBox1.Text = "";
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox2.Text == "Образование")
            {
                richTextBox2.Text = "";
            }
        }

        private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox3.Text == "Местро работы и стаж")
            {
                richTextBox3.Text = "";
            }
        }

        private void richTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox4.Text == "Год обучения")
            {
                richTextBox4.Text = "";
            }
        }

        private void richTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox5.Text == "Возраст")
            {
                richTextBox5.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Autorization autorization = new Autorization();
            autorization.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (
                (richTextBox1.Text != "") &&
                (richTextBox2.Text != "") &&
                (richTextBox3.Text != "") &&
                (richTextBox4.Text != "") &&
                (richTextBox5.Text != "") 
                )
            {
                Vstuplenie vstuplenie = new Vstuplenie();
                vstuplenie.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не все поля заполнены!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
