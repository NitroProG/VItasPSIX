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
using SqlConn;

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

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            MessageBox.Show("" + Convert.ToInt32(screen.Size.Width + ""));
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
            }
            panel1.Left = Width/2 - panel1.Width/2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = DBUtils.GetDBConnection();
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

                if (count1 == 1)
                {
                    Program.user = sc2.ExecuteScalar().ToString();
                    Anketa anketa = new Anketa();
                    anketa.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Данные введены не верно");
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
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
