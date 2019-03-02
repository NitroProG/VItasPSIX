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
using System.Net;
using System.Net.Mail;

namespace Psico
{
    public partial class Autorization : Form
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
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

        private void ClientAutorization(object sender, EventArgs e)
        {
            //try
            //{
                // Подключение к БД
                SqlConnection con = DBUtils.GetDBConnection();
                con.Open();

                // Разгранечение прав
                SqlCommand sc = new SqlCommand("Select * from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '"
                    + textBox2.Text + "'and [isadmin]='1'", con);
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
                    SqlCommand sc1 = new SqlCommand("Select * from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '"
                        + textBox2.Text + "'and [isadmin]='0'", con);
                    SqlDataReader dr1;
                    dr1 = sc1.ExecuteReader();

                    int count1 = 0;

                    while (dr1.Read())
                    {
                        count1 += 1;
                    }

                    dr1.Close();

                    // Проверка введённых данных
                    SqlCommand sc2 = new SqlCommand("select id_user from users where[User_Login] = '" + textBox1.Text + "' and[User_Password] = '" + textBox2.Text + "'", con);

                    // Если данные верны
                    if (count1 == 1)
                    {
                        // Запись в переменную номер пользователя
                        Program.user = sc2.ExecuteScalar().ToString();

                        try
                        {
                            MailMessage mail = new MailMessage("ProgrammPsicotest", "vit.sax@yandex.ru", "Вход пользователя", "Пользователь " + textBox1.Text + " вошёл в систему!");
                            SmtpClient client = new SmtpClient("smtp.yandex.ru");
                            client.Port = 587;
                            client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                            client.EnableSsl = true;
                            client.Send(mail);
                        }
                        catch { }

                        Anketa anketa = new Anketa();
                        anketa.Show();
                        Hide();
                    }

                    else
                    {
                        SqlCommand sc3 = new SqlCommand("Select * from students where[Student_Login] = '" + textBox1.Text + "' and[Student_Password] = '"+ textBox2.Text +"'", con);
                        SqlDataReader dr3;
                        dr3 = sc3.ExecuteReader();

                        int count3 = 0;

                        while (dr3.Read())
                        {
                            count3 += 1;
                        }

                        dr3.Close();

                        // Проверка введённых данных
                        SqlCommand sc4 = new SqlCommand("select id_students from students where[Student_Login] = '" + textBox1.Text + "' and[Student_Password] = '" + textBox2.Text + "'", con);

                        // Если данные верны
                        if (count3 == 1)
                        {
                            // Запись в переменную номер пользователя
                            Program.student = sc4.ExecuteScalar().ToString();

                            try
                            {
                                MailMessage mail = new MailMessage("ProgrammPsicotest", "vit.sax@yandex.ru", "Вход пользователя", "Пользователь " + textBox1.Text + " вошёл в систему!");
                                SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                client.Port = 587;
                                client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                client.EnableSsl = true;
                                client.Send(mail);
                            }
                            catch { }

                            Anketa anketa = new Anketa();
                            anketa.Show();
                            Hide();
                        }
                        // Если данные не верны
                        else
                        {

                            MessageBox.Show("Данные введены не верно", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            textBox1.Text = "";
                            textBox2.Text = "";
                        }
                    }
                }
            //}

            //catch
            //{
            //    MessageBox.Show("Отсутствует подключение к БД","Внимание!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //}
        }

        private void OpenFormRegistration(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Hide();
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
