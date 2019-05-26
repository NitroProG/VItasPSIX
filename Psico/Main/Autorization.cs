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
using Microsoft.Win32;

namespace Psico
{
    public partial class Autorization : Form
    {
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();
        Timer timer3 = new Timer();
        Timer timer4 = new Timer();
        SqlConnection con = DBUtils.GetDBConnection();
        int CheckData = 0;
        int TimerStatus1 = 0;
        int TimerStatus2 = 0;
        int TimerStatus3 = 0;
        int TimerStatus4 = 0;

        public Autorization()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Formalignment();

            con.Open();
            SqlCommand UpdateMainAdminStatus = new SqlCommand("UPDATE users SET UserStatus=0 WHERE id_user = 1", con);
            UpdateMainAdminStatus.ExecuteNonQuery();
            con.Close();

            timer1.Tick += Timer_Tick1;
            timer1.Interval = 10;
            timer2.Tick += Timer_Tick2;
            timer2.Interval = 10;
            timer3.Tick += Timer_Tick3;
            timer3.Interval = 10;
            timer4.Tick += Timer_Tick4;
            timer4.Interval = 10;
        }

        private void ClientAutorization(object sender, EventArgs e)
        {
            Program.UserRole = 0;

            try
            {
                // Подключение к БД
                con.Open();

                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    // Выбор количества данных в таблице БД
                    SqlCommand GetUserId = new SqlCommand("select id_user as 'id' from users where User_Login = '" + textBox1.Text + "' and User_Password = '" + new Shifr().Shifrovka(textBox2.Text,"Pass") + "'", con);
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
                        SqlCommand GetUserStatus = new SqlCommand("select UserStatus from users where id_user = '" + Program.user + "'", con);
                        SqlDataReader dr3 = GetUserStatus.ExecuteReader();
                        dr3.Read();
                        int UserStatus = Convert.ToInt32(dr3["UserStatus"]);
                        dr3.Close();

                        if (UserStatus == 0)
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

                                SqlCommand UpdateUserStatus = new SqlCommand("UPDATE users SET UserStatus=1 WHERE id_user = " + Program.user + "", con);
                                UpdateUserStatus.ExecuteNonQuery();

                                switch (UserRole)
                                {
                                    case "Admin":
                                        Program.UserRole = 1;
                                        new administrator().Show();
                                        Hide();
                                        break;
                                    case "Teacher":
                                        Program.UserRole = 2;
                                        new TeacherStudents().Show();
                                        Hide();
                                        break;
                                    case "Student":
                                        Program.UserRole = 3;
                                        new Anketa().Show();
                                        Hide();
                                        break;
                                }
                            }
                            else
                            {
                                CreateInfo("Внимание! Ваше время работы с программой окончено, для того чтобы продлить время обратитесь к администратору.", "red", panel1);
                            }
                        }
                        else
                        {
                            CreateInfo("Пользователь с указанными данными уже в сети! Обратитесь к администратору или преподавателю.", "red", panel1);
                        }
                    }
                    else
                    {
                        CreateInfo("Пользователя с такими данными не существует!", "red", panel1);
                    }
                }
                else
                {
                    CreateInfo("Заполнены не все поля для авторизации!", "red", panel1);
                }
                con.Close();
            }
            catch (Exception)
            {
                CreateInfo("Отсутствует подключение к БД! Обратитесь к администратору.", "red", panel1);
                con.Close();
            }
        }

        private void OpenFormRegistration(object sender, EventArgs e)
        {
            new Registration().Show();
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

            char year1 = UserEndDate[6];
            char year2 = UserEndDate[7];
            char year3 = UserEndDate[8];
            char year4 = UserEndDate[9];
            string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

            char Month1 = UserEndDate[3];
            char Month2 = UserEndDate[4];
            string Month = Month1.ToString() + Month2.ToString();

            char Day1 = UserEndDate[0];
            char Day2 = UserEndDate[1];
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

        private void RestorePassword(object sender, EventArgs e)
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
            textBox3.MaxLength = 50;
            textBox3.BackColor = Color.PowderBlue;
            textBox3.KeyPress += ZapretRus;
            textBox3.Enter += TextBox3_Enter;
            textBox3.Leave += TextBox3_Leave;
            textBox3.Location = new Point(textBox1.Location.X,textBox1.Location.Y+30);
            new ToolTip().SetToolTip(textBox3, "Ваш логин");
            panel.Controls.Add(textBox3);

            Label label2 = new Label();
            label2.Name = "label6";
            label2.Text = "Логин: ";
            label2.Font = new Font(label2.Font.FontFamily, 14);
            label2.Size = new Size(320, 30);
            label2.Location = new Point(textBox3.Location.X,textBox3.Location.Y);
            panel.Controls.Add(label2);

            TextBox textBox4 = new TextBox();
            textBox4.Name = "Mail";
            textBox4.Size = new Size(373, 31);
            textBox4.Font = new Font(textBox4.Font.FontFamily, 16);
            textBox4.MaxLength = 150;
            textBox4.BackColor = Color.PowderBlue;
            textBox4.KeyPress += ZapretRus;
            textBox4.Enter += TextBox4_Enter;
            textBox4.Leave += TextBox4_Leave;
            textBox4.Location = new Point(textBox2.Location.X, textBox2.Location.Y+30);
            new ToolTip().SetToolTip(textBox4, "Почта, на которую придёт новый пароль");
            panel.Controls.Add(textBox4);

            Label label3 = new Label();
            label3.Name = "label7";
            label3.Text = "Почта: ";
            label3.Font = new Font(label3.Font.FontFamily, 14);
            label3.Size = new Size(320, 30);
            label3.Location = new Point(textBox4.Location.X,textBox4.Location.Y);
            panel.Controls.Add(label3);

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

                        SqlCommand UpdatePassword = new SqlCommand("UPDATE users SET User_Password='"+password+"' WHERE id_user = "+Program.user+"", con);
                        UpdatePassword.ExecuteNonQuery();

                        CreateInfo("Ваш новый пароль выслан на указанную почту!", "lime", panel1);
                        (panel1.Controls["RestorePassword"] as Panel).Dispose();
                    }
                    catch
                    {
                        CreateInfo("Не удалось изменить пароль, проверьте указанную почту на существование!", "red", panel1);
                    }
                }
                else
                {
                    CreateInfo("Пользователя с таким логином не существует!", "red", panel1);
                }

                con.Close();
            }
            else
            {
                CreateInfo("Необходимо заполнить все поля!", "red", panel1);
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

        public void CreateInfo(string labelinfo, string color, Panel MainPanel)
        {
            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 5000;
            timer.Start();

            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.BringToFront();

            switch (color)
            {
                case "red":
                    label.ForeColor = Color.Red;
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    break;
                default:
                    label.ForeColor = Color.Black;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                (sender as Timer).Stop();
            }
            catch
            {
            }
        }

        private void ZapretRus(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == (char)Keys.Back)
            {
            }
            else
            {
                CreateInfo("Возможно вводить только цифры и латинские буквы!", "red", panel1);
                e.Handled = true;
            }
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            switch (TimerStatus1)
            {
                case 1:
                    label2.Left = label2.Left - 2;

                    if (label2.Left <= 50)
                    {
                        timer1.Stop();
                    }
                    break;

                case 2:
                    label2.Left = label2.Left + 2;

                    if (label2.Left >= 130)
                    {
                        timer1.Stop();
                    }
                    break;
            }
        }

        private void Timer_Tick2(object sender, EventArgs e)
        {
            switch (TimerStatus2)
            {
                case 1:
                    label3.Left = label3.Left - 2;

                    if (label3.Left <= 40)
                    {
                        timer2.Stop();
                    }
                    break;

                case 2:
                    label3.Left = label3.Left + 2;

                    if (label3.Left >= 130)
                    {
                        timer2.Stop();
                    }
                    break;
            }
        }

        private void Timer_Tick3(object sender, EventArgs e)
        {
            try
            {
                switch (TimerStatus3)
                {
                    case 1:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left - 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left <= 60)
                        {
                            timer3.Stop();
                        }
                        break;

                    case 2:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left + 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label6"] as Label).Left >= 130)
                        {
                            timer3.Stop();
                        }
                        break;
                }
            }
            catch{}
        }

        private void Timer_Tick4(object sender, EventArgs e)
        {
            try
            {
                switch (TimerStatus4)
                {
                    case 1:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left - 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left <= 60)
                        {
                            timer4.Stop();
                        }
                        break;

                    case 2:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left + 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label7"] as Label).Left >= 130)
                        {
                            timer4.Stop();
                        }
                        break;
                }
            }
            catch{}
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            TimerStatus1 = 1;

            timer1.Start();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            TimerStatus1 = 2;

            timer1.Start();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            TimerStatus2 = 2;

            timer2.Start();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            TimerStatus2 = 1;

            timer2.Start();
        }
        private void TextBox3_Leave(object sender, EventArgs e)
        {
            TimerStatus3 = 2;

            timer3.Start();
        }

        private void TextBox3_Enter(object sender, EventArgs e)
        {
            TimerStatus3 = 1;

            timer3.Start();
        }

        private void TextBox4_Leave(object sender, EventArgs e)
        {
            TimerStatus4 = 2;

            timer4.Start();
        }

        private void TextBox4_Enter(object sender, EventArgs e)
        {
            TimerStatus4 = 1;

            timer4.Start();
        }
    }
}
