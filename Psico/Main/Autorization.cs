using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using System.Net;
using System.Net.Mail;

namespace Psico
{
    public partial class Autorization : Form
    {
        Timer timer = new Timer();
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();
        Timer timer3 = new Timer();
        Timer timer4 = new Timer();
        Timer timer5 = new Timer();
        Timer timer6 = new Timer();
        int TimerStatus1 = 0;
        int TimerStatus2 = 0;
        int TimerStatus3 = 0;
        int TimerStatus4 = 0;
        int TimerStatus5 = 0;
        int TimerStatus6 = 0;
        SqlConnection con = SQLConnectionString.GetDBConnection();
        int CheckData = 0;
        int NewPassKey = 0;
        int KolvoClickNewPass = 0;
        int CheckConnection = 0;

        public Autorization()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация под разрешение экрана
            Formalignment();

            // Разрешение на получение события нажатия сочитания клавиш
            KeyPreview = true;

            // Таймеры
            timer1.Tick += Timer_Tick1;
            timer1.Interval = 10;
            timer2.Tick += Timer_Tick2;
            timer2.Interval = 10;
            timer3.Tick += Timer_Tick3;
            timer3.Interval = 10;
            timer4.Tick += Timer_Tick4;
            timer4.Interval = 10;
            timer5.Tick += Timer_Tick5;
            timer5.Interval = 10;
            timer6.Tick += Timer_Tick6;
            timer6.Interval = 10;

            Program.checkopenzadacha = 0;
        }

        private void UpdateMainAdminStatus()
        {
            // Обнуление статуса у Главного администратора
            try
            {
                new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = 1");
            }
            catch
            {
                CreateInfo("Ошибка подключения к серверу, обратитесь к администратору!", "red", panel1);
                CheckConnection = 1;
            }
        }

        private void ClientAutorization(object sender, EventArgs e)
        {
            // Проверка на подключение к БД
            if (CheckConnection ==0)
            {
                // Обнуление переменной отвечающей за номер пользователя
                Program.UserRole = 0;

                try
                {
                    // Подключение к БД
                    con.Open();

                    // Проверка на заполнение данных
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {
                        // Проверка на существование пользователя с указанными данными
                        Program.user = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user from users where User_Login = '" + textBox1.Text + "' and User_Password = '" + new Shifr().Shifrovka(textBox2.Text, "Pass") + "'"));

                        // Если пользователь существует
                        if (Program.user != 0)
                        {
                            // Проверка на статус пользователя
                            bool UserStatus = Convert.ToBoolean(new SQL_Query().GetInfoFromBD("select UserStatus from users where id_user = " + Program.user + ""));

                            // Если пользователь не в сети
                            if (UserStatus == false)
                            {
                                // Проверка даты окончания лицензии
                                CheckUserEndData();

                                // Если Дата окончания лицензии не сегодня
                                if (CheckData == 0)
                                {
                                    // Выбор статуса пользователя
                                    string UserRole = new SQL_Query().GetInfoFromBD("select Naim as 'Role' from Role where users_id = " + Program.user + "");

                                    // Изменение статуса пользователя на "В сети"
                                    new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=1 WHERE id_user = " + Program.user + "");

                                    // В зависимости от роли пользователя выполнить
                                    switch (UserRole)
                                    {
                                        case "Admin":
                                            // Обнуление статуса у главного администратора
                                            UpdateMainAdminStatus();

                                            // Удаление динамической созданной Panel
                                            new Autorization().CloseInfo();

                                            //Присвоение переменной роли пользователя
                                            Program.UserRole = 1;

                                            //Открытие формы администратора
                                            new administrator().Show();

                                            // Скрытие этой формы
                                            Hide();
                                            break;
                                        case "Teacher":
                                            // Удаление динамической созданной Panel
                                            new Autorization().CloseInfo();

                                            //Присвоение переменной роли пользователя
                                            Program.UserRole = 2;

                                            // Открытие формы преподавателя
                                            new TeacherStudents().Show();

                                            // Скрытие этой формы
                                            Hide();
                                            break;
                                        case "Student":
                                            // Удаление динамической созданной Panel
                                            new Autorization().CloseInfo();

                                            //Присвоение переменной роли пользователя
                                            Program.UserRole = 3;

                                            // Открытие формы студента
                                            new Anketa().Show();

                                            // Скрытие этой формы
                                            Hide();
                                            break;
                                        default:
                                            CreateInfo("Роль пользователя не зарегистрированна в программе","red",panel1);
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
                    // Отключение от БД
                    con.Close();
                }
                catch
                {
                    CreateInfo("Отсутствует подключение к БД! Обратитесь к администратору.", "red", panel1);

                    // Отключение от БД
                    con.Close();
                }
            }
            else
            {
                CreateInfo("Ошибка подключения к серверу, обратитесь к администратору!", "red", panel1);
            }
        }

        private void OpenFormRegistration(object sender, EventArgs e)
        {

            // Проверка подключения к БД
            if (CheckConnection == 0)
            {
                // Удаление динамической созданной Panel
                new Autorization().CloseInfo();

                // Открытие формы регистрации
                new Registration().Show();

                // Скрытие этой формы
                Hide();
            }
            else
            {
                CreateInfo("Ошибка подключения к серверу, обратитесь к администратору!", "red", panel1);
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            // Выход из программы
            Application.Exit();
        }

        private void CheckUserEndData()
        {
            // Выбор номера преподавателя студента
            int TeacherID = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Teacher_id from users where id_user = " + Program.user + ""));

            // Выбор даты окончания лицензии у студента
            string UserEndDate = new SQL_Query().GetInfoFromBD("select user_end_data from Teachers where id_teacher = " + TeacherID + "");

            // Отдельный выбор года
            char year1 = UserEndDate[6];
            char year2 = UserEndDate[7];
            char year3 = UserEndDate[8];
            char year4 = UserEndDate[9];
            string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

            // Отдельный выбор месяца
            char Month1 = UserEndDate[3];
            char Month2 = UserEndDate[4];
            string Month = Month1.ToString() + Month2.ToString();

            // Отдельный выбор дня
            char Day1 = UserEndDate[0];
            char Day2 = UserEndDate[1];
            string Day = Day1.ToString() + Day2.ToString();

            // Выбор сегодняшей даты
            int Nowyear = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Year(getdate()) as 'year'"));

            // Выбор сегодняшей даты
            string NowMonth = new SQL_Query().GetInfoFromBD("select Month(getdate()) as 'Month'");
            if (Convert.ToInt32(NowMonth) < 10)
            {
                NowMonth = "0" + NowMonth;
            }

            // Выбор сегодняшей даты
            string NowDay = new SQL_Query().GetInfoFromBD("select Day(getdate()) as 'Day'");
            if (Convert.ToInt32(NowDay) < 10)
            {
                NowDay = "0" + NowDay;
            }

            // Преобразование собранных данных в даты
            DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
            DateTime UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

            // Сравнение даты окончания лицензии с сегодняшней датой
            if (NowData > UserData)
            {
                // Пользователь может войти
                CheckData = 1;
            }
            else
            {
                // Пользователю отказано в доступе
                CheckData = 0;
            }
        }

        private void Formalignment()
        {
            // Адаптация разрешения экрана
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

        private void ShowHidePassword(object sender, EventArgs e)
        {
            // Показ/скрытие пароля
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else textBox2.UseSystemPasswordChar = true;
        }

        private void RestorePassword(object sender, EventArgs e)
        {
            // Проверка подключение к БД
            if (CheckConnection ==0)
            {
                // Обнуление попыток ввода кода подтверждения изменения пароля
                KolvoClickNewPass = 0;

                // Динамическое Создание Panel
                Panel panel = new Panel();
                panel.Name = "RestorePassword";
                panel.Size = new Size(panel1.Width, panel1.Height);
                panel.Location = new Point(0, 0);
                panel.BackColor = Color.White;
                panel1.Controls.Add(panel);
                panel.BringToFront();

                // Динамическое Создание Label
                Label label = new Label();
                label.Name = "Title";
                label.Text = "Восстановление пароля";
                label.Font = new Font(label.Font.FontFamily, 20);
                label.Size = new Size(320, 30);
                label.Location = new Point(panel.Width / 2 - label.Width / 2, label1.Location.Y);
                panel.Controls.Add(label);

                // Динамическое Создание Textbox
                TextBox textBox3 = new TextBox();
                textBox3.Name = "Login";
                textBox3.Size = new Size(373, 31);
                textBox3.Font = new Font(textBox3.Font.FontFamily, 16);
                textBox3.MaxLength = 50;
                textBox3.BackColor = Color.PowderBlue;
                textBox3.KeyPress += ZapretRus;
                textBox3.Enter += TextBox3_Enter;
                textBox3.Leave += TextBox3_Leave;
                textBox3.Location = new Point(textBox1.Location.X, textBox1.Location.Y + 30);
                new ToolTip().SetToolTip(textBox3, "Ваш логин");
                panel.Controls.Add(textBox3);

                // Динамическое Создание Label
                Label label2 = new Label();
                label2.Name = "label6";
                label2.Text = "Логин: ";
                label2.Font = new Font(label2.Font.FontFamily, 14);
                label2.Size = new Size(320, 30);
                label2.Location = new Point(textBox3.Location.X, textBox3.Location.Y);
                panel.Controls.Add(label2);

                // Динамическое Создание Textbox
                TextBox textBox4 = new TextBox();
                textBox4.Name = "Mail";
                textBox4.Size = new Size(373, 31);
                textBox4.Font = new Font(textBox4.Font.FontFamily, 16);
                textBox4.MaxLength = 150;
                textBox4.BackColor = Color.PowderBlue;
                textBox4.KeyPress += ZapretRus;
                textBox4.Enter += TextBox4_Enter;
                textBox4.Leave += TextBox4_Leave;
                textBox4.Location = new Point(textBox2.Location.X, textBox2.Location.Y + 30);
                new ToolTip().SetToolTip(textBox4, "Почта, на которую придёт код подтверждения");
                panel.Controls.Add(textBox4);

                // Динамическое Создание Label
                Label label3 = new Label();
                label3.Name = "label7";
                label3.Text = "Почта: ";
                label3.Font = new Font(label3.Font.FontFamily, 14);
                label3.Size = new Size(320, 30);
                label3.Location = new Point(textBox4.Location.X, textBox4.Location.Y);
                panel.Controls.Add(label3);

                // Динамическое Создание Button
                Button button5 = new Button();
                button5.Name = "BackToAutorization";
                button5.Text = "Назад";
                button5.Font = new Font(button5.Font.FontFamily, 13);
                button5.Size = new Size(140, 35);
                button5.BackColor = Color.PowderBlue;
                button5.ForeColor = Color.Black;
                button5.Location = new Point(30, button2.Location.Y);
                button5.Click += BackToAutorization;
                button5.FlatStyle = FlatStyle.Flat;
                panel.Controls.Add(button5);

                // Динамическое Создание Button
                Button button6 = new Button();
                button6.Name = "SentKey";
                button6.Text = "Выслать код подтверждения";
                button6.Font = new Font(button6.Font.FontFamily, 13);
                button6.Size = new Size(300, 35);
                button6.BackColor = Color.PowderBlue;
                button6.ForeColor = Color.Black;
                button6.Location = new Point(panel.Width - button6.Width - 30, button1.Location.Y);
                button6.Click += SentKey;
                button6.FlatStyle = FlatStyle.Flat;
                panel.Controls.Add(button6);
            }
            else
            {
                CreateInfo("Ошибка подключения к серверу, обратитесь к администратору!", "red", panel1);
            }
        }

        private void BackToAutorization(object sender, EventArgs e)
        {
            // Удаление динамически созданных Panel
            try
            {
                (panel1.Controls["RestorePassword"] as Panel).Dispose();
            }
            catch{}
            try
            {
                (panel1.Controls["BDSettings"] as Panel).Dispose();
            }
            catch{}
        }

        private void SentKey(object sender, EventArgs e)
        {
            // Проверка на ввод данных
            if (((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text != "" && ((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text != "")
            {
                // Подключение к БД
                con.Open();

                // Проверка на существование пользователя по указанным данным
                Program.user = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + ((panel1.Controls["RestorePassword"] as Panel).Controls["Login"] as TextBox).Text + "' " +
                                                                                    "and User_Mail = '" + new Shifr().Shifrovka(((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text, "Mail") + "'"));

                // Если пользователь существует
                if (Program.user != 0)
                {
                    try
                    {
                        // Генерация кода подтверждения
                        NewPassKey = Convert.ToInt32(GetKey());

                        // Отправка Кода подтверждения на указанную почту
                        MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", ((panel1.Controls["RestorePassword"] as Panel).Controls["Mail"] as TextBox).Text,
                                                           "Код подтверждения для изменения пароля в программе Psico", NewPassKey.ToString());
                        SmtpClient client = new SmtpClient("smtp.yandex.ru");
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                        client.EnableSsl = true;
                        client.Send(mail);

                        // Перерисовка формы
                        (panel1.Controls["RestorePassword"] as Panel).Controls["Sentkey"].Dispose();
                        (panel1.Controls["RestorePassword"] as Panel).Controls["label6"].Dispose();
                        (panel1.Controls["RestorePassword"] as Panel).Controls["label7"].Dispose();
                        (panel1.Controls["RestorePassword"] as Panel).Controls["Login"].Dispose();
                        (panel1.Controls["RestorePassword"] as Panel).Controls["Mail"].Dispose();

                        // Динимическое создание textbox
                        TextBox textBox5 = new TextBox();
                        textBox5.Name = "Key";
                        textBox5.Size = new Size(373, 31);
                        textBox5.Font = new Font(textBox5.Font.FontFamily, 16);
                        textBox5.BackColor = Color.PowderBlue;
                        textBox5.Enter += TextBox5_Enter;
                        textBox5.Leave += TextBox5_Leave;
                        textBox5.Location = new Point(textBox1.Location.X + 10, textBox1.Location.Y + 30);
                        new ToolTip().SetToolTip(textBox5, "Код подтверждения");
                        (panel1.Controls["RestorePassword"] as Panel).Controls.Add(textBox5);

                        // Динимическое создание label
                        Label label8 = new Label();
                        label8.Name = "label8";
                        label8.Text = "Код: ";
                        label8.Font = new Font(label8.Font.FontFamily, 14);
                        label8.Size = new Size(320, 30);
                        label8.Location = new Point(textBox5.Location.X, textBox5.Location.Y);
                        (panel1.Controls["RestorePassword"] as Panel).Controls.Add(label8);

                        // Динимическое создание textbox
                        TextBox textBox6 = new TextBox();
                        textBox6.Name = "NewPassword";
                        textBox6.Size = new Size(373, 31);
                        textBox6.Font = new Font(textBox2.Font.FontFamily, 16);
                        textBox6.MaxLength = 50;
                        textBox6.BackColor = Color.PowderBlue;
                        textBox6.KeyPress += ZapretRus;
                        textBox6.Enter += TextBox6_Enter;
                        textBox6.Leave += TextBox6_Leave;
                        textBox6.Location = new Point(textBox2.Location.X + 10, textBox2.Location.Y + 30);
                        new ToolTip().SetToolTip(textBox6, "Новый пароль");
                        (panel1.Controls["RestorePassword"] as Panel).Controls.Add(textBox6);

                        // Динимическое создание label
                        Label label9 = new Label();
                        label9.Name = "label9";
                        label9.Text = "Новый пароль: ";
                        label9.Font = new Font(label9.Font.FontFamily, 14);
                        label9.Size = new Size(320, 30);
                        label9.Location = new Point(textBox6.Location.X, textBox6.Location.Y);
                        (panel1.Controls["RestorePassword"] as Panel).Controls.Add(label9);

                        // Динимическое создание button
                        Button button7 = new Button();
                        button7.Name = "GetNewPassword";
                        button7.Text = "Изменить пароль";                        
                        button7.Size = new Size(300, 35);
                        button7.Font = new Font(button7.Font.FontFamily, 13);
                        button7.BackColor = Color.PowderBlue;
                        button7.ForeColor = Color.Black;
                        button7.Location = new Point((panel1.Controls["RestorePassword"] as Panel).Width - button7.Width - 30, button1.Location.Y);
                        button7.Click += GetNewPassword;
                        (panel1.Controls["RestorePassword"] as Panel).Controls.Add(button7);
                    }
                    catch
                    {
                        CreateInfo("Ошибка отправки кода подтверждения на почту, проверьте подключение к интернету","red",panel1);
                    }                    
                }
                else
                {
                    CreateInfo("Пользователя с указанными данными не существует!", "red", panel1);
                }

                // Отключение от БД
                con.Close();
            }
            else
            {
                CreateInfo("Необходимо заполнить все поля!", "red", panel1);
            }
        }

        private void GetNewPassword(object sender, EventArgs e)
        {
            // Проверка количества попыток
            if (KolvoClickNewPass < 6)
            {
                // Проверка на ввод данных
                if ((panel1.Controls["RestorePassword"] as Panel).Controls["Key"].Text != "" && (panel1.Controls["RestorePassword"] as Panel).Controls["NewPassword"].Text != "")
                {
                    // Проверка на корректность указанного кода подтверждения
                    if ((panel1.Controls["RestorePassword"] as Panel).Controls["Key"].Text == NewPassKey.ToString())
                    {
                        // Подключение к БД
                        con.Open();

                        // Изменение старого пароля на указанный
                        new SQL_Query().UpdateOneCell("UPDATE users SET User_Password='" + new Shifr().Shifrovka((panel1.Controls["RestorePassword"] as Panel).Controls["NewPassword"].Text, "Pass") + "' WHERE id_user = " + Program.user + "");

                        // Удаление динамической созданной Panel
                        try
                        {
                            (panel1.Controls["RestorePassword"] as Panel).Dispose();
                        }
                        catch
                        {
                        }
                        
                        // Вывод сообщения
                        CreateInfo("Вы успешно изменили пароль", "lime", panel1);

                        // Отключение от БД
                        con.Close();
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Вы указали неверный код подтверждения", "red", panel1);

                        //Очистка введённых данных
                        (panel1.Controls["RestorePassword"] as Panel).Controls["Key"].Text = "";
                        (panel1.Controls["RestorePassword"] as Panel).Controls["NewPassword"].Text = "";
                    }

                    // Увеличение количества попыток
                    KolvoClickNewPass++;
                }
                else
                {
                    CreateInfo("Необходимо заполнить все поля", "red", panel1);
                }
            }
            else
            {
                // Вывод сообщения
                CreateInfo("Превышен лимит по вводу кода подтверждения для изменения пароля","red",panel1);

                // Удаление динамической созданной Panel
                try
                {
                    (panel1.Controls["RestorePassword"] as Panel).Dispose();
                }
                catch
                {
                }
            }
        }

        public static string GetKey(int x = 6)
        {
            // Генерация кода подтверждения изменения пароля
            string key = "";
            var r = new Random();
            while (key.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsDigit(c))
                    key += c;
            }
            return key;
        }

        public void CreateInfo(string labelinfo, string color, Panel MainPanel)
        {
            // Удаление динамической созданной Panel
            CloseInfo();

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
            CloseInfo();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            CloseInfo();
        }
        
        public void CloseInfo()
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

        private void ZapretRus(object sender, KeyPressEventArgs e)
        {
            // Запрет на ввод русских букв
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar == '@')
            {
            }
            else
            {
                CreateInfo("Возможно вводить только цифры и латинские буквы!", "red", panel1);
                e.Handled = true;
            }
        }

        private void HotKeys(object sender, KeyEventArgs e)
        {
            // Сочитание клавиш Ctrl + Shift + D
            if (e.Control && e.Shift && e.KeyCode == Keys.D)
            {
                // Открытие формы изменения строки подключения
                CreateWindowForBDSettings();
            }


            // Сочитание клавиш Ctrl + Shift + R
            if (e.Control && e.Shift && e.KeyCode == Keys.R)
            {
                string CheckUsers = new SQL_Query().GetInfoFromBD("select id_user from users");

                if (CheckUsers == "0")
                {
                    // Открытие формы добавление первого пользователя
                    new AddUser().Show();
                    Hide();
                }
            }
        }

        private void CreateWindowForBDSettings()
        {
            // Динамическое создание Panel
            Panel panel = new Panel()
            {
                Name = "BDSettings",
                Size = new Size(panel1.Width, panel1.Height),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            panel1.Controls.Add(panel);
            panel.BringToFront();

            // Динамическое создание Label
            for (int labelkolvo = 0; labelkolvo < 4; labelkolvo++)
            {
                Label label = new Label();
                switch (labelkolvo)
                {
                    case 0:
                        label.Name = "Title";
                        label.Text = "Настройка подключения";
                        label.Font = new Font(label.Font.FontFamily, 20);
                        label.Location = new Point(0,0);
                        break;

                    case 1:
                        label.Name = "ExampleBDLabel";
                        label.Text = "Пример строки подключения";
                        label.Font = new Font(label.Font.FontFamily, 14);
                        label.Location = new Point(0,panel.Height/6);
                        break;

                    case 2:
                        label.Name = "BDLabel";
                        label.Text = "Cтрока подключения";
                        label.Font = new Font(label.Font.FontFamily, 14);
                        label.Location = new Point(0,panel.Height/6+80);
                        break;
                    case 3:
                        label.Name = "KeyForCreateNewConStringLabel";
                        label.Text = "Код подтверждения";
                        label.Font = new Font(label.Font.FontFamily, 14);
                        label.Location = new Point(0, panel.Height / 6 + 160);
                        break;
                }
                label.Size = new Size(panel.Width, 40);
                label.TextAlign = ContentAlignment.MiddleCenter;
                panel.Controls.Add(label);
            }

            // Динамическое создание TextBox
            for (int textboxkolvo = 0; textboxkolvo < 3; textboxkolvo++)
            {
                TextBox textBox = new TextBox();
                switch (textboxkolvo)
                {
                    case 0:
                        textBox.Name = "ExampleBDTextbox";
                        textBox.Text = "Data Source=Место хранения;initial catalog=Название БД;Persist Security info=True;User ID=Логин;Password=Пароль;";
                        textBox.ReadOnly = true;
                        textBox.Location = new Point(10,panel.Height/6+40);
                        break;

                    case 1:
                        textBox.Name = "BDTextbox";
                        textBox.Location = new Point(10,panel.Height/6+120);
                        break;

                    case 2:
                        textBox.Name = "KeyForCreateNewConStringTextbox";
                        textBox.Location = new Point(10, panel.Height / 6 + 200);
                        break;
                }
                textBox.Size = new Size(panel.Width-20, 30);
                textBox.Font = new Font(textBox.Font.FontFamily, 14);
                textBox.BackColor = Color.PowderBlue;
                panel.Controls.Add(textBox);
            }

            // Динамическое создание Button
            for (int buttonkolvo = 0; buttonkolvo < 2; buttonkolvo++)
            {
                Button button = new Button();
                switch (buttonkolvo)
                {                    
                    case 0:
                        button.Name = "ButtonForBackToAutorization";
                        button.Text = "НАЗАД";
                        button.Size = new Size(button2.Width,button2.Height);
                        button.Location = new Point(button2.Location.X,button2.Location.Y);
                        button.Click += BackToAutorization;
                        break;

                    case 1:
                        button.Name = "ButtonForGetNewConString";
                        button.Text = "Изменить подключение к серверу";
                        button.Size = new Size(330, button1.Height);
                        button.Location = new Point(panel.Width-button.Width-22, button1.Location.Y);
                        button.Click += GetNewConString;
                        break;
                }
                button.FlatStyle = FlatStyle.Flat;
                button.Font = new Font(button.Font.FontFamily, 13);
                button.BackColor = Color.PowderBlue;
                button.ForeColor = Color.Black;
                panel.Controls.Add(button);
            }
        }

        private void GetNewConString(object sender, EventArgs e)
        {
            // Проверка на ввод данных
            if ((panel1.Controls["BDSettings"] as Panel).Controls["BDTextbox"].Text !="" && (panel1.Controls["BDSettings"] as Panel).Controls["KeyForCreateNewConStringTextbox"].Text != "")
            {
                ////Запись строки подключения к БД
                File.WriteAllText("C:\\System\\ConnectionString.txt", (panel1.Controls["BDSettings"] as Panel).Controls["BDTextbox"].Text);

                //Вывод сообщения
                DialogResult result = MessageBox.Show("Необходимо перезапустить программу для обновления подключения!", "Внимание!",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // При нажатии на "ОК"
                if (result == DialogResult.OK)
                {
                    // Закрытие приложения
                    Application.Exit();
                }
            }
            else
            {
                // Вывод сообщения
                CreateInfo("Необходимо заполнить все поля","red",panel1);
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
            catch { }
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
            catch { }
        }

        private void Timer_Tick5(object sender, EventArgs e)
        {
            try
            {
                switch (TimerStatus5)
                {
                    case 1:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left - 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left <= 80)
                        {
                            timer5.Stop();
                        }
                        break;

                    case 2:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left + 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label8"] as Label).Left >= 150)
                        {
                            timer5.Stop();
                        }
                        break;
                }
            }
            catch { }
        }

        private void Timer_Tick6(object sender, EventArgs e)
        {
            try
            {
                switch (TimerStatus6)
                {
                    case 1:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left - 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left <= 0)
                        {
                            timer6.Stop();
                        }
                        break;

                    case 2:
                        ((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left = ((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left + 2;

                        if (((panel1.Controls["RestorePassword"] as Panel).Controls["label9"] as Label).Left >= 150)
                        {
                            timer6.Stop();
                        }
                        break;
                }
            }
            catch { }
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

        private void TextBox5_Leave(object sender, EventArgs e)
        {
            TimerStatus5 = 2;

            timer5.Start();
        }

        private void TextBox5_Enter(object sender, EventArgs e)
        {
            TimerStatus5 = 1;

            timer5.Start();
        }

        private void TextBox6_Leave(object sender, EventArgs e)
        {
            TimerStatus6 = 2;

            timer6.Start();
        }

        private void TextBox6_Enter(object sender, EventArgs e)
        {
            TimerStatus6 = 1;

            timer6.Start();
        }
    }
}
