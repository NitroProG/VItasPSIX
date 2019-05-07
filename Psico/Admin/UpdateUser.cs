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
    public partial class UpdateUser : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        int CheckData = 0;

        public UpdateUser()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            GetOpenMainForm();
        }

        private void UpdateUsers(object sender, EventArgs e)
        {
            con.Open();

            if (textBox1.Text != "" && textBox2.Text !="" && textBox3.Text !="" && textBox4.Text !="")
            {
                switch (comboBox1.SelectedIndex)
                {
                    // Администратор
                    case 0:
                        checkData();
                        if (CheckData == 1)
                        {
                            SqlCommand StrPrc4 = new SqlCommand("Teachers_update", con);
                            StrPrc4.CommandType = CommandType.StoredProcedure;
                            StrPrc4.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                            StrPrc4.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                            StrPrc4.Parameters.AddWithValue("@User_End_Data", maskedTextBox1.Text);
                            StrPrc4.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                            StrPrc4.ExecuteNonQuery();

                            SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                            StrPrc1.CommandType = CommandType.StoredProcedure;
                            StrPrc1.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                            StrPrc1.Parameters.AddWithValue("@User_Login", textBox1.Text);
                            StrPrc1.Parameters.AddWithValue("@User_Password", textBox2.Text);
                            StrPrc1.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                            StrPrc1.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                            StrPrc1.ExecuteNonQuery();

                            CreateInfo("Данные изменены!", "lime");

                            SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                            DataSet ds1 = new DataSet();
                            da1.Fill(ds1, "users");
                            datagr1.DataSource = ds1.Tables[0];
                        }
                        break;

                    // Преподаватель
                    case 1:
                        checkData();
                        if (CheckData == 1)
                        {
                            SqlCommand StrPrc5 = new SqlCommand("Teachers_update", con);
                            StrPrc5.CommandType = CommandType.StoredProcedure;
                            StrPrc5.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                            StrPrc5.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                            StrPrc5.Parameters.AddWithValue("@User_End_Data", maskedTextBox1.Text);
                            StrPrc5.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                            StrPrc5.ExecuteNonQuery();

                            SqlCommand StrPrc2 = new SqlCommand("users_update", con);
                            StrPrc2.CommandType = CommandType.StoredProcedure;
                            StrPrc2.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                            StrPrc2.Parameters.AddWithValue("@User_Login", textBox1.Text);
                            StrPrc2.Parameters.AddWithValue("@User_Password", textBox2.Text);
                            StrPrc2.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                            StrPrc2.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                            StrPrc2.ExecuteNonQuery();

                            CreateInfo("Данные изменены!", "lime");

                            SqlDataAdapter da2 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
                            DataSet ds2 = new DataSet();
                            da2.Fill(ds2, "users");
                            datagr1.DataSource = ds2.Tables[0];
                        }                       
                        break;

                    // Студент
                    case 2:
                        SqlCommand StrPrc3 = new SqlCommand("users_update", con);
                        StrPrc3.CommandType = CommandType.StoredProcedure;
                        StrPrc3.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                        StrPrc3.Parameters.AddWithValue("@User_Login", textBox1.Text);
                        StrPrc3.Parameters.AddWithValue("@User_Password", textBox2.Text);
                        StrPrc3.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                        StrPrc3.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                        StrPrc3.ExecuteNonQuery();

                        CreateInfo("Данные изменены!", "lime");

                        SqlDataAdapter da3 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                        SqlCommandBuilder cb3 = new SqlCommandBuilder(da3);
                        DataSet ds3 = new DataSet();
                        da3.Fill(ds3, "users");
                        datagr1.DataSource = ds3.Tables[0];

                        break;
                }
            }
            else
            {
                CreateInfo("Для изменения данных необходимо выбрать пользователя и заполнить все поля!", "red");
            }

            con.Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(780,300);
            datagr1.Location = new Point(panel1.Width/2 - datagr1.Width/2, 70);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datagr1.CellClick += SelectUser;

            SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CreateTableForUpdateUsers");
            datagr1.DataSource = ds1.Tables[0];

            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionBackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionForeColor = Color.Black;
            datagr1.BackgroundColor = Color.White;

            panel1.Controls.Add(datagr1);

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.EnableHeadersVisualStyles = false;
            datagr1.ColumnHeadersHeight = 75;

            datagr1.Columns[0].Visible = false;
            datagr1.Columns[4].Visible = false;
            datagr1.Columns[5].Visible = false;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[9].Visible = false;
            datagr1.Columns[11].Visible = false;
            datagr1.Columns[12].Visible = false;

            datagr1.Columns[1].HeaderText = "Логин пользователя";
            datagr1.Columns[2].HeaderText = "Пароль пользователя";
            datagr1.Columns[3].HeaderText = "Почта пользователя";
            datagr1.Columns[7].HeaderText = "Дата окончания лицензии";
            datagr1.Columns[8].HeaderText = "Оставшиеся количество возможных регистраций студентов";
            datagr1.Columns[10].HeaderText = "Роль пользователя";

            FormAlignment();
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Enabled = true;
            textBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Enabled = true;
            textBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Enabled = true;
            textBox4.Text = datagr1.CurrentRow.Cells[8].Value.ToString();
            maskedTextBox1.Enabled = true;
            maskedTextBox1.Text = datagr1.CurrentRow.Cells[7].Value.ToString();

            switch (datagr1.CurrentRow.Cells[10].Value.ToString())
            {
                case "Admin":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "Teacher":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "Student":
                    comboBox1.SelectedIndex = 2;
                    textBox4.Enabled = false;
                    maskedTextBox1.Enabled = false;
                    break;
            }
            comboBox1.Text = datagr1.CurrentRow.Cells[10].Value.ToString();
        }

        private void WriteOnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void GetOpenMainForm()
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void checkData()
        {
            //Выбор данных из БД
            SqlCommand GetYear = new SqlCommand("select Year(getdate()) as 'year'", con);
            SqlDataReader dr3 = GetYear.ExecuteReader();
            dr3.Read();
            int Nowyear = Convert.ToInt32(dr3["year"]);
            dr3.Close();

            //Выбор данных из БД
            SqlCommand GetMonth = new SqlCommand("select Month(getdate()) as 'Month'", con);
            SqlDataReader dr4 = GetMonth.ExecuteReader();
            dr4.Read();
            string NowMonth = dr4["Month"].ToString();
            dr4.Close();

            if (Convert.ToInt32(NowMonth) < 10)
            {
                NowMonth = "0" + NowMonth;
            }

            //Выбор данных из БД
            SqlCommand GetDay = new SqlCommand("select Day(getdate()) as 'Day'", con);
            SqlDataReader dr5 = GetDay.ExecuteReader();
            dr5.Read();
            string NowDay = dr5["Day"].ToString();
            dr5.Close();

            if (Convert.ToInt32(NowDay) < 10)
            {
                NowDay = "0" + NowDay;
            }

            char year1 = maskedTextBox1.Text[0];
            char year2 = maskedTextBox1.Text[1];
            char year3 = maskedTextBox1.Text[2];
            char year4 = maskedTextBox1.Text[3];
            string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

            char Month1 = maskedTextBox1.Text[5];
            char Month2 = maskedTextBox1.Text[6];
            string Month = Month1.ToString() + Month2.ToString();

            char Day1 = maskedTextBox1.Text[8];
            char Day2 = maskedTextBox1.Text[9];
            string Day = Day1.ToString() + Day2.ToString();

            if (Convert.ToInt16(Month) != 00 && Convert.ToInt16(Day) != 00)
            {
                if (Convert.ToInt32(Month) <= 12)
                {
                    if (Convert.ToInt32(Day) <= 28)
                    {
                        DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
                        DateTime UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

                        if (UserData > NowData)
                        {
                            CheckData = 1;
                        }
                        else
                        {
                            CheckData = 0;
                            CreateInfo("Выбранная вами дата окончания уже прошла!", "red");
                        }
                    }
                    else
                    {
                        CreateInfo("Максимально возможный для выбора день - 28!", "red");
                    }
                }
                else
                {
                    CreateInfo("Некорректно введён месяц окончания!", "red");
                }
            }
            else
            {
                CreateInfo("Некорректно введён месяц или день окончания!", "red");
            }
        }

        private void CreateInfo(string labelinfo, string color)
        {
            Timer timer = new Timer();
            timer.Interval = 5000;
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
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    break;
                default:
                    label.ForeColor = Color.Black;
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

        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

        }
    }
}
