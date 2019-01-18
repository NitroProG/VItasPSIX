using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Psico
{
    public partial class Autorization : Form
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Введите логин";
            richTextBox2.Text = "Введите пароль";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
            con.Open(); // подключение к БД

            SqlCommand sc = new SqlCommand("Select * from users where[User_Login] = '" + richTextBox1.Text + "' and[User_Password] = '" + richTextBox2.Text + "'and [isadmin]='1'", con); //выбор данных из таблицы БД 
            SqlDataReader dr;
            dr = sc.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                count += 1;
            }
            dr.Close();

            if (count == 1)
            {
                administrator administrator = new administrator();
                administrator.Show();
                Hide();
            }
            else
            {
                SqlCommand sc1 = new SqlCommand("Select * from users where[User_Login] = '" + richTextBox1.Text + "' and[User_Password] = '" + richTextBox2.Text + "'and [isadmin]='0'", con); //выбор данных из таблицы БД 
                SqlDataReader dr1;
                dr1 = sc1.ExecuteReader();
                int count1 = 0;
                while (dr1.Read())
                {
                    count1 += 1;
                }
                dr1.Close();

                SqlCommand sc2 = new SqlCommand("select id_user from users where[User_Login] = '" + richTextBox1.Text + "' and[User_Password] = '" + richTextBox2.Text + "'", con);
                Program.user = sc2.ExecuteScalar().ToString();

                if (count1 == 1)
                {
                    
                    Anketa anketa = new Anketa();
                    anketa.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Данные введены не верно");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox1.Text == "Введите логин")
            {
                richTextBox1.Text = "";
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox2.Text == "Введите пароль")
            {
                richTextBox2.Text = "";
            }
        }
    }
}
