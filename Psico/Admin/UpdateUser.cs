using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class UpdateUser : Form
    {
        Timer timer = new Timer();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        DateTime UserData = new DateTime();
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
            // Подключение к БД
            con.Open();

            // Проверка ввода данных
            if (textBox1.Text != "" && textBox2.Text !="" && textBox3.Text !="" && textBox4.Text !="" && maskedTextBox1.MaskCompleted == true)
            {
                // Проверка указанной почты на существование
                string CheckMail = "";
                CheckMail = new CheckExistMail().CheckMail(textBox3.Text);

                // Если указанная почта существует
                if (CheckMail == "Почтовый ящик существует")
                {
                    // Изменение данных пользователя в зависимости от его роли
                    switch (comboBox1.SelectedIndex)
                    {
                        // Изменение администратора
                        case 0:

                            // Проверка указанной даты 
                            checkData();

                            // Если указанная дата ещё не наступила
                            if (CheckData == 1)
                            {
                                // Изменение данных о преподавателе
                                SqlCommand StrPrc4 = new SqlCommand("Teachers_update", con);
                                StrPrc4.CommandType = CommandType.StoredProcedure;
                                StrPrc4.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                                StrPrc4.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                                StrPrc4.Parameters.AddWithValue("@User_End_Data", UserData);
                                StrPrc4.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                                StrPrc4.ExecuteNonQuery();

                                // Изменение данных о пользователе
                                SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                                StrPrc1.CommandType = CommandType.StoredProcedure;
                                StrPrc1.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                                StrPrc1.Parameters.AddWithValue("@User_Login", textBox1.Text);
                                StrPrc1.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka(textBox2.Text, "Pass"));
                                StrPrc1.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka(textBox3.Text, "Mail"));
                                StrPrc1.Parameters.AddWithValue("@Fam", "");
                                StrPrc1.Parameters.AddWithValue("@Imya", "");
                                StrPrc1.Parameters.AddWithValue("@Otch", "");
                                StrPrc1.Parameters.AddWithValue("@Study", "");
                                StrPrc1.Parameters.AddWithValue("@Work", "");
                                StrPrc1.Parameters.AddWithValue("@Year", "");
                                StrPrc1.Parameters.AddWithValue("@Old", "");
                                StrPrc1.Parameters.AddWithValue("@UserStatus", 0);
                                StrPrc1.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                                StrPrc1.ExecuteNonQuery();

                                // Вывод сообщения
                                CreateInfo("Данные изменены!", "lime", panel1);

                                // Заполнение datagr1
                                new SQL_Query().UpdateDatagr("select * from CreateTableForUpdateUsers", "users",datagr1);
                            }
                            break;

                        // Изменение преподавателя
                        case 1:

                            // Проверка указанной даты
                            checkData();

                            // Если указанная дата ещё не наступила
                            if (CheckData == 1)
                            {
                                // Изменение данных о преподавателе
                                SqlCommand StrPrc5 = new SqlCommand("Teachers_update", con);
                                StrPrc5.CommandType = CommandType.StoredProcedure;
                                StrPrc5.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                                StrPrc5.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                                StrPrc5.Parameters.AddWithValue("@User_End_Data", UserData);
                                StrPrc5.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                                StrPrc5.ExecuteNonQuery();

                                // Изменение данных о пользователе
                                SqlCommand StrPrc2 = new SqlCommand("users_update", con);
                                StrPrc2.CommandType = CommandType.StoredProcedure;
                                StrPrc2.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                                StrPrc2.Parameters.AddWithValue("@User_Login", textBox1.Text);
                                StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka(textBox2.Text, "Pass"));
                                StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka(textBox3.Text, "Mail"));
                                StrPrc2.Parameters.AddWithValue("@Fam", "");
                                StrPrc2.Parameters.AddWithValue("@Imya", "");
                                StrPrc2.Parameters.AddWithValue("@Otch", "");
                                StrPrc2.Parameters.AddWithValue("@Study", "");
                                StrPrc2.Parameters.AddWithValue("@Work", "");
                                StrPrc2.Parameters.AddWithValue("@Year", "");
                                StrPrc2.Parameters.AddWithValue("@Old", "");
                                StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                StrPrc2.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                                StrPrc2.ExecuteNonQuery();

                                // Вывод сообщения
                                CreateInfo("Данные изменены!", "lime", panel1);

                                // Заполнение datagr1
                                new SQL_Query().UpdateDatagr("select * from CreateTableForUpdateUsers", "users", datagr1);
                            }
                            break;

                        // Изменение студента
                        case 2:
                            // Изменение данных о пользователе
                            SqlCommand StrPrc3 = new SqlCommand("users_update", con);
                            StrPrc3.CommandType = CommandType.StoredProcedure;
                            StrPrc3.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                            StrPrc3.Parameters.AddWithValue("@User_Login", textBox1.Text);
                            StrPrc3.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka(textBox2.Text, "Pass"));
                            StrPrc3.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka(textBox3.Text, "Mail"));
                            StrPrc3.Parameters.AddWithValue("@Fam", "");
                            StrPrc3.Parameters.AddWithValue("@Imya", "");
                            StrPrc3.Parameters.AddWithValue("@Otch", "");
                            StrPrc3.Parameters.AddWithValue("@Study", "");
                            StrPrc3.Parameters.AddWithValue("@Work", "");
                            StrPrc3.Parameters.AddWithValue("@Year", "");
                            StrPrc3.Parameters.AddWithValue("@Old", "");
                            StrPrc3.Parameters.AddWithValue("@UserStatus", 0);
                            StrPrc3.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                            StrPrc3.ExecuteNonQuery();

                            // Вывод сообщения
                            CreateInfo("Данные изменены!", "lime", panel1);

                            // Заполнение datagr1
                            new SQL_Query().UpdateDatagr("select * from CreateTableForUpdateUsers", "users",datagr1);
                            break;
                    }
                }
                else
                {
                    // Вывод сообщения
                    CreateInfo(CheckMail,"red",panel1);
                }
            }
            else
            {
                // Вывод сообщения
                CreateInfo("Для изменения данных необходимо выбрать пользователя и заполнить все поля!", "red", panel1);
            }

            // Обновление формы
            datagr1.CurrentCell = null;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            maskedTextBox1.Text = "";
            textBox6.Text = "";
            checkBox1.Checked = false;

            // Отключение от БД
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

            // Заполнение datagr1
            TableFill();

            // Обновление формы
            UserTextClean();

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
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[4].Visible = false;
            datagr1.Columns[5].Visible = false;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[9].Visible = false;
            datagr1.Columns[11].Visible = false;
            datagr1.Columns[12].Visible = false;

            datagr1.Columns[1].HeaderText = "Логин пользователя";
            datagr1.Columns[7].HeaderText = "Дата окончания лицензии";
            datagr1.Columns[8].HeaderText = "Оставшиеся количество возможных регистраций студентов";
            datagr1.Columns[10].HeaderText = "Роль пользователя";

            // Адаптация под разрешение экрана
            FormAlignment();

            textBox2.UseSystemPasswordChar = true;
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            // Запись выбранных в datagr1 данных в редактируемые поля
            textBox1.Enabled = true;
            textBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Enabled = true;
            textBox2.Text = new Shifr().DeShifrovka(datagr1.CurrentRow.Cells[2].Value.ToString(),"Pass");
            textBox3.Enabled = true;
            textBox3.Text = new Shifr().DeShifrovka(datagr1.CurrentRow.Cells[3].Value.ToString(),"Mail");
            textBox4.Enabled = true;
            textBox4.Text = datagr1.CurrentRow.Cells[8].Value.ToString();
            maskedTextBox1.Enabled = true;
            maskedTextBox1.Text = datagr1.CurrentRow.Cells[7].Value.ToString();

            // Запись роли выбранного пользователя в datagr1
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

        private void GetOpenMainForm()
        {
            // Открытие главной формы администратора
            new administrator().Show();
            Close();
        }

        private void checkData()
        {
            //Выбор сегодняшней даты
            int Nowyear = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Year(getdate()) as 'year'"));
            string NowMonth = new SQL_Query().GetInfoFromBD("select Month(getdate()) as 'Month'");
            if (Convert.ToInt32(NowMonth) < 10)
            {
                NowMonth = "0" + NowMonth;
            }
            string NowDay = new SQL_Query().GetInfoFromBD("select Day(getdate()) as 'Day'");
            if (Convert.ToInt32(NowDay) < 10)
            {
                NowDay = "0" + NowDay;
            }

            // Выбор отдельно года
            char year1 = maskedTextBox1.Text[6];
            char year2 = maskedTextBox1.Text[7];
            char year3 = maskedTextBox1.Text[8];
            char year4 = maskedTextBox1.Text[9];
            string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

            // Выбор отдельно месяца
            char Month1 = maskedTextBox1.Text[3];
            char Month2 = maskedTextBox1.Text[4];
            string Month = Month1.ToString() + Month2.ToString();

            // Выбор отдельно день
            char Day1 = maskedTextBox1.Text[0];
            char Day2 = maskedTextBox1.Text[1];
            string Day = Day1.ToString() + Day2.ToString();

            // Проверка указанного года на корректность
            if (Convert.ToInt16(Year) < 2100)
            {
                // Проверка указанного месяца и дня на корректность
                if (Convert.ToInt16(Month) != 00 && Convert.ToInt16(Day) != 00)
                {
                    // Проверка указанного месяца н корректность
                    if (Convert.ToInt32(Month) <= 12)
                    {
                        // Проверка указанного дня на корректность
                        if (Convert.ToInt32(Day) <= 28)
                        {
                            // Преобразование выбранных данных в даты
                            DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
                            UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

                            // Если указанная дата ещё не наступила
                            if (UserData > NowData)
                            {
                                CheckData = 1;
                            }
                            else
                            {
                                CheckData = 0;
                                CreateInfo("Выбранная вами дата окончания уже прошла!", "red", panel1);
                            }
                        }
                        else
                        {
                            CreateInfo("Максимально возможный для выбора день - 28!", "red", panel1);
                        }
                    }
                    else
                    {
                        CreateInfo("Некорректно введён месяц окончания!", "red", panel1);
                    }
                }
                else
                {
                    CreateInfo("Некорректно введён месяц или день окончания!", "red", panel1);
                }
            }
            else
            {
                CreateInfo("Максимально возможный год это 2099!","red", panel1);
            }
        }

        private void FormAlignment()
        {
            // Адаптация под разрешение экрана
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

        }

        private void VisiblePassword(object sender, EventArgs e)
        {
            // Показ/Скрытие пароля
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else textBox2.UseSystemPasswordChar = true;
        }

        private void TableFilter(object sender, EventArgs e)
        {
            // Объявление переменных
            int ComboboboxAll = 0;
            string role = "";

            // Выбор роли пользователя
            switch (comboBox2.SelectedItem)
            {
                case "Администратор":
                    role = "Admin";
                    break;

                case "Преподаватель":
                    role = "Teacher";
                    break;

                case "Студент":
                    role = "Student";
                    break;
                case "Всё":
                    ComboboboxAll = 1;
                    break;
            }

            // Заполнение datagr1
            TableFill();

            // Фильтр datagr1
            if (ComboboboxAll !=1 )
            {
                for (int i = 0; i < datagr1.Rows.Count; i++)
                {
                    if (datagr1.Rows[i].Cells[10].Value.ToString() != role)
                    {
                        datagr1.Rows[i].Visible = false;
                    }
                }
            }

            // Обновление формы
            UserTextClean();
        }

        private void InfoFinder(object sender, EventArgs e)
        {
            // Заполнение datagr1
            TableFill();

            // Поиск в datagr1
            int Find;
            for (int x = 0; x < datagr1.Rows.Count; x++)
            {
                Find = 0;

                for (int y = 0; y < datagr1.ColumnCount; y++)
                {
                    if (datagr1.Rows[x].Cells[y].Value.ToString().Contains(textBox6.Text))
                    {
                        Find = 1;
                    }
                }

                if (Find != 1)
                {
                    datagr1.Rows[x].Visible = false;
                }
            }

            // Обновление формы
            UserTextClean();
        }

        private void TableFill()
        {
            // Заполнение datagr1
            new SQL_Query().UpdateDatagr("select * from CreateTableForUpdateUsers", "CreateTableForUpdateUsers", datagr1);

            // Отключение выбора ячейки datagr1
            datagr1.CurrentCell = null;
        }

        private void UserTextClean()
        {
            // Обновление формы
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            maskedTextBox1.Text = "";
            checkBox1.Checked = false;
            comboBox1.SelectedIndex = -1;
        }

        public void CreateInfo(string labelinfo, string color, Panel MainPanel)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }

            // Создание таймера
            timer.Tick += Timer_Tick;
            timer.Interval = 5000;
            timer.Start();

            // Динамической создание Panel
            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            // Динамической создание Label
            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.Click += Label_Click;
            label.BringToFront();

            // Выбор цвета для шрифта сообщения
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

        private void Label_Click(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }
        }

        private void ZapretRusAndEng(object sender, KeyPressEventArgs e)
        {
            // Запрет ввода русских и английских букв
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только цифры!", "red", panel1);
            }
        }

        private void ZapretRus(object sender, KeyPressEventArgs e)
        {
            // Запрет ввода русских букв
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar == '@')
            {
            }
            else
            {
                CreateInfo("Возможно вводить только цифры и латинские буквы!", "red", panel1);
                e.Handled = true;
            }
        }
    }
}
