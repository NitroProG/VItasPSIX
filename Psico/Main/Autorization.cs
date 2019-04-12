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
            string UserRole = "";

            try
            {
                // Подключение к БД
                SqlConnection con = DBUtils.GetDBConnection();
                con.Open();

                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    // Выбор количества данных в таблице БД
                    SqlCommand GetUserId = new SqlCommand("select id_user as 'id' from users where User_Login = '" + textBox1.Text + "' and User_Password = '"+textBox2.Text+"'", con);
                    SqlDataReader dr1 = GetUserId.ExecuteReader();

                    try
                    {
                        dr1.Read();
                        Program.user = Convert.ToInt32(dr1["id"].ToString());
                        dr1.Close();
                    }
                    catch
                    {
                        Program.user = 0;
                        dr1.Close();
                    }

                    if (Program.user != 0)
                    {
                        // Выбор количества данных в таблице БД
                        SqlCommand GetUserRole = new SqlCommand("select Naim as 'Role' from Role where users_id = '" + Program.user + "'", con);
                        SqlDataReader dr2 = GetUserRole.ExecuteReader();

                        dr2.Read();
                        UserRole = dr2["Role"].ToString();
                        dr2.Close();

                        switch (UserRole)
                        {
                            case "Admin":
                                    administrator FormAdmin = new administrator();
                                    FormAdmin.Show();
                                    Hide();
                                break;
                            case "Teacher":
                                    Anketa anketa1 = new Anketa();
                                    anketa1.Show();
                                    Hide();
                                break;
                            case "Student":
                                    Anketa anketa2 = new Anketa();
                                    anketa2.Show();
                                    Hide();
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователя с такими данными не существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Заполнены не все поля для авторизации!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к БД", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon. Error);
            }
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
