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

            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            //MessageBox.Show("" + Convert.ToInt32(screen.Size.Width + ""));
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
            }

            // Позиционирование элементов формы пользователя
            panel1.Left = Width/2 - panel1.Width/2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open(); // подключение к БД

            // Разгранечение прав
            SqlCommand sc = new SqlCommand("Select * from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '"
                + textBox2.Text + "'and [isadmin]='1'", con); //выбор данных из таблицы БД 
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
                // Открытие формы администратора
                administrator administrator = new administrator();
                administrator.Show();
                Hide();
            }
            else
            {
                // Разгранечение прав
                SqlCommand sc1 = new SqlCommand("Select * from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '"
                    + textBox2.Text + "'and [isadmin]='0'", con); //выбор данных из таблицы БД 
                SqlDataReader dr1;
                dr1 = sc1.ExecuteReader();
                int count1 = 0;
                while (dr1.Read())
                {
                    count1 += 1;
                }
                dr1.Close();

                // Проверка на существование пользователя
                SqlCommand sc2 = new SqlCommand("select id_user from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '" + textBox2.Text + "'", con);

                if (count1 == 1) // если пользователь существует
                {
                    // Запись в переменную номер пользователя
                    Program.user = sc2.ExecuteScalar().ToString();

                    // Открытие формы анкетирования
                    Anketa anketa = new Anketa();
                    anketa.Show();
                    Hide();
                }

                // Если пользователя нет в БД
                else
                {
                    // Вывод сообщения
                    MessageBox.Show("Данные введены не верно");

                    // Очистка введённых данные
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }

            // Обновление выбранных ответов
            SqlCommand delete = new SqlCommand("delete from otvGip", con);
            delete.ExecuteNonQuery();
            SqlCommand delete1 = new SqlCommand("delete from otvDiag", con);
            delete1.ExecuteNonQuery();
            SqlCommand delete2 = new SqlCommand("delete from otvFenom", con);
            delete2.ExecuteNonQuery();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Выход из программы
            Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Очистка подсказок
            if (textBox1.Text == "Введите логин")
            {
                textBox1.Text = "";
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Очистка подсказок
            if (textBox2.Text == "Введите пароль")
            {
                textBox2.Text = "";
            }
        }
    }
}
