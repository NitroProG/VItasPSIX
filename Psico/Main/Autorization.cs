using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        SqlConnection con = DBUtils.GetDBConnection();
        int CheckData = 0;

        public Autorization()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Formalignment();
        }

        private void ClientAutorization(object sender, EventArgs e)
        {
            try
            {
                // Подключение к БД
                con.Open();

                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    // Выбор количества данных в таблице БД
                    SqlCommand GetUserId = new SqlCommand("select id_user as 'id' from users where User_Login = '" + textBox1.Text + "' and User_Password = '" + textBox2.Text + "'", con);
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
                        CheckUserEndData();

                        if (CheckData == 0)
                        {
                            // Выбор количества данных в таблице БД
                            SqlCommand GetUserRole = new SqlCommand("select Naim as 'Role' from Role where users_id = '" + Program.user + "'", con);
                            SqlDataReader dr2 = GetUserRole.ExecuteReader();

                            dr2.Read();
                            string UserRole = dr2["Role"].ToString();
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
                            CreateInfo("Внимание! Ваше время работы с программой окончено, для того чтобы продлить время обратитесь к администратору.", "red", panel1);
                            con.Close();
                        }
                    }
                    else
                    {
                        CreateInfo("Пользователя с такими данными не существует!", "red", panel1);
                        con.Close();
                    }
                }
                else
                {
                    CreateInfo("Заполнены не все поля для авторизации!", "red", panel1);
                    con.Close();
                }
            }
            catch
            {
                CreateInfo("Отсутствует подключение к БД! Обратитесь к администратору.", "red", panel1);
                con.Close();
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

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void CheckUserEndData()
        {
            // Выбор преподавателя пользователя
            SqlCommand GetTeacherID = new SqlCommand("select Teacher_id from users where id_user = " + Program.user + "", con);
            SqlDataReader dr3 = GetTeacherID.ExecuteReader();
            dr3.Read();
            int TeacherID = Convert.ToInt32(dr3["Teacher_id"].ToString());
            dr3.Close();

            // Выбор даты окончания действия программы
            SqlCommand GetUserEndDate = new SqlCommand("select user_end_data from Teachers where id_teacher = " + TeacherID + "", con);
            SqlDataReader dr4 = GetUserEndDate.ExecuteReader();
            dr4.Read();
            string UserEndDate = dr4["user_end_data"].ToString();
            dr4.Close();

            char year1 = UserEndDate[0];
            char year2 = UserEndDate[1];
            char year3 = UserEndDate[2];
            char year4 = UserEndDate[3];
            string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

            char Month1 = UserEndDate[5];
            char Month2 = UserEndDate[6];
            string Month = Month1.ToString() + Month2.ToString();

            char Day1 = UserEndDate[8];
            char Day2 = UserEndDate[9];
            string Day = Day1.ToString() + Day2.ToString();

            //Выбор данных из БД
            SqlCommand GetYear = new SqlCommand("select Year(getdate()) as 'year'", con);
            SqlDataReader dr5 = GetYear.ExecuteReader();
            dr5.Read();
            int Nowyear = Convert.ToInt32(dr5["year"]);
            dr5.Close();

            //Выбор данных из БД
            SqlCommand GetMonth = new SqlCommand("select Month(getdate()) as 'Month'", con);
            SqlDataReader dr6 = GetMonth.ExecuteReader();
            dr6.Read();
            string NowMonth = dr6["Month"].ToString();
            dr6.Close();

            if (Convert.ToInt32(NowMonth) < 10)
            {
                NowMonth = "0" + NowMonth;
            }

            //Выбор данных из БД
            SqlCommand GetDay = new SqlCommand("select Day(getdate()) as 'Day'", con);
            SqlDataReader dr7 = GetDay.ExecuteReader();
            dr7.Read();
            string NowDay = dr7["Day"].ToString();
            dr7.Close();

            if (Convert.ToInt32(NowDay) < 10)
            {
                NowDay = "0" + NowDay;
            }

            DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
            DateTime UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

            if (NowData > UserData)
            {
                CheckData = 1;
            }
            else
            {
                CheckData = 0;
            }
        }

        private void CreateInfo(string labelinfo, string color, Panel sender)
        {
            Timer timer = new Timer();
            timer.Tick += TimerTick;
            timer.Start();

            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(panel1.Width / 2 - panel.Width / 2, panel1.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            sender.Controls.Add(panel);
            panel.BringToFront();

            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;

            switch (color)
            {
                case "red":
                    label.ForeColor = Color.Red;
                    timer.Interval = 5000;
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    timer.Interval = 2000;
                    break;
                default:
                    label.ForeColor = Color.Black;
                    timer.Interval = 5000;
                    break;
            }

            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.BringToFront();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                (sender as Timer).Stop();
            }
            catch
            {

            }

            try
            {
                ((panel1.Controls["RestorePassword"] as Panel).Controls["panel"] as Panel).Dispose();
                (sender as Timer).Stop();
            }
            catch
            {

            }
        }

        private void Formalignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else textBox2.UseSystemPasswordChar = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Panel panel = new Panel();
            panel.Name = "RestorePassword";
            panel.Size = new Size(panel1.Width,panel1.Height);
            panel.Location = new Point(0, 0);
            panel.BackColor = Color.White;
            panel1.Controls.Add(panel);
            panel.BringToFront();

            Label label = new Label();
            label.Name = "Title";
            label.Text = "Восстановление пароля";
            label.Font = new Font(label.Font.FontFamily, 20);
            label.Size = new Size(320,30);
            label.Location = new Point(panel.Width/2-label.Width/2,label1.Location.Y);
            panel.Controls.Add(label);

            TextBox textBox3 = new TextBox();
            textBox3.Name = "Login";
            textBox3.Size = new Size(373,31);
            textBox3.Font = new Font(textBox3.Font.FontFamily, 16);
            textBox3.BackColor = Color.PowderBlue;
            textBox3.Location = new Point(textBox1.Location.X,textBox1.Location.Y+30);
            new ToolTip().SetToolTip(textBox3, "Ваш логин");
            panel.Controls.Add(textBox3);

            TextBox textBox4 = new TextBox();
            textBox4.Name = "Mail";
            textBox4.Size = new Size(373, 31);
            textBox4.Font = new Font(textBox4.Font.FontFamily, 16);
            textBox4.BackColor = Color.PowderBlue;
            textBox4.Location = new Point(textBox2.Location.X, textBox2.Location.Y+30);
            new ToolTip().SetToolTip(textBox4, "Почта, на которую придёт новый пароль");
            panel.Controls.Add(textBox4);

            Button button5 = new Button();
            button5.Name = "BackToAutorization";
            button5.Text = "Назад";
            button5.Font = new Font(button5.Font.FontFamily, 13);
            button5.Size = new Size(140,35);
            button5.BackColor = Color.PowderBlue;
            button5.ForeColor = Color.Black;
            button5.Location = new Point(30,button2.Location.Y);
            button5.Click += BackToAutorization;
            panel.Controls.Add(button5);

            Button button6 = new Button();
            button6.Name = "GetNewPassword";
            button6.Text = "Получить новый пароль";
            button6.Font = new Font(button6.Font.FontFamily, 13);
            button6.Size = new Size(230, 35);
            button6.BackColor = Color.PowderBlue;
            button6.ForeColor = Color.Black;
            button6.Location = new Point(panel.Width-button6.Width-30, button1.Location.Y);
            button6.Click += GetNewPassword;
            panel.Controls.Add(button6);
        }

        private void BackToAutorization(object sender, EventArgs e)
        {
            (panel1.Controls["RestorePassword"] as Panel).Dispose();
        }

        private void GetNewPassword(object sender, EventArgs e)
        {
            Program.user = 0;

            if (((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text != "" && ((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text != "")
            {
                con.Open();

                // Проверка на существование пользователя
                SqlCommand CheckUser = new SqlCommand("select id_user as 'id' from users where User_Login = '" + ((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text + "'", con);
                SqlDataReader dr1 = CheckUser.ExecuteReader();

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
                    string password = GetPassword();

                    try
                    {
                        // Отправка пароля по почте
                        MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", ((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text, 
                                                           "Пароль для программы Psico", password);
                        SmtpClient client = new SmtpClient("smtp.yandex.ru");
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                        client.EnableSsl = true;
                        client.Send(mail);


                        // Получение номера преподавателя
                        SqlCommand GetTeacherId = new SqlCommand("select Teacher_id as 'id' from users where User_Login = '" + ((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text + "'", con);
                        SqlDataReader dr2 = GetTeacherId.ExecuteReader();
                        dr2.Read();
                        int TeacherID = Convert.ToInt32(dr2["id"].ToString());
                        dr2.Close();

                        SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@id_user", Program.user);
                        StrPrc1.Parameters.AddWithValue("@User_Login", ((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text);
                        StrPrc1.Parameters.AddWithValue("@User_Password", password);
                        StrPrc1.Parameters.AddWithValue("@User_Mail", ((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text);
                        StrPrc1.Parameters.AddWithValue("@Teacher_id", TeacherID);
                        StrPrc1.ExecuteNonQuery();

                        CreateInfo("Ваш новый пароль выслан на указанную почту!", "lime", panel1);
                        (panel1.Controls["RestorePassword"] as Panel).Dispose();
                    }
                    catch
                    {
                        CreateInfo("Не удалось изменить пароль, проверьте указанную почту на существование!", "red", (panel1.Controls["RestorePassword"] as Panel));
                    }
                }
                else
                {
                    CreateInfo("Пользователя с таким логином не существует!", "red", (panel1.Controls["RestorePassword"] as Panel));
                }

                con.Close();
            }
            else
            {
                CreateInfo("Необходимо заполнить все поля!", "red", (panel1.Controls["RestorePassword"] as Panel));
            }
        }

        public static string GetPassword(int x = 6)
        {
            // Создание пароля для пользователя
            string password = "";
            var r = new Random();
            while (password.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    password += c;
            }
            return password;
        }
    }
}
