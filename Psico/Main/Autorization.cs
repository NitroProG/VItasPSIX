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
                            CreateInfo("Внимание! Ваше время работы с программой окончено, для того чтобы продлить время обратитесь к администратору.", "red");
                            con.Close();
                        }
                    }
                    else
                    {
                        CreateInfo("Пользователя с такими данными не существует!", "red");
                        con.Close();
                    }
                }
                else
                {
                    CreateInfo("Заполнены не все поля для авторизации!", "red");
                    con.Close();
                }
            }
            catch
            {
                CreateInfo("Отсутствует подключение к БД! Обратитесь к администратору.", "red");
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

        private void CreateInfo(string labelinfo, string color)
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
            panel1.Controls.Add(panel);
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
    }
}
