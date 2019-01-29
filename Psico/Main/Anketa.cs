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

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1366)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
            }
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
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
            if (richTextBox3.Text == "Место работы и стаж")
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
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (
                (richTextBox1.Text != "") && (richTextBox1.Text != "ФИО") &&
                (richTextBox2.Text != "") && (richTextBox2.Text != "Образование") &&
                (richTextBox3.Text != "") && (richTextBox3.Text != "Место работы и стаж") &&
                (richTextBox4.Text != "") && (richTextBox4.Text != "Год обучения") &&
                (richTextBox5.Text != "") && (richTextBox5.Text != "Возраст")
                )
            {
                Vstuplenie vstuplenie = new Vstuplenie();
                vstuplenie.Show();
                Close();
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
