using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using System.Net;
using System.Net.Mail;

namespace Psico
{
    public partial class Registration : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        Timer timer = new Timer();
        int Strela;
        int Registr;
        int student;
        int teacher;
        int checkteacherUniqueNaim;
        string KeyCheck;
        string LoginCheck;

        public Registration()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Обнуление переменных
            Strela = 0;
            Registr = 0;
            student = 0;
            teacher = 0;

            // Динамическое создание label
            Label label = new Label();
            label.Name = "label";
            label.Text = "Выберите способ регистрации!";
            label.AutoSize = true;
            label.Location = new Point(170,130);
            label.Font = new Font(label.Font.Name,16);
            panel3.Controls.Add(label);

            // Адаптация под разрешение экрана
            Formalignment();
        }

        private void DrawStrela(object sender, PaintEventArgs e)
        {
            // Динамическая прорисовка
            if (Strela == 0)
            {
                Pen pen = new Pen(Color.DarkCyan, 6);
                pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                e.Graphics.DrawLine(pen, 250, 20, 325, 120);
                e.Graphics.DrawLine(pen, 400, 20, 325, 120);
            }
        }

        private void GetStudentInfoRegistration(object sender, EventArgs e)
        {
            // Присвоение переменной выбранного типа регистрации (Студент)
            Registr = 1;

            // Изменение цвета у выбранного типа регистрации (Студент)
            GetColorLabel();

            // Очищение Panel
            GetClearPanel1();

            // Если тип регистрации был выбран первый раз
            if (student == 0)
            {
                // Заполнение Panel
                for (int i = 1; i < 4; i++)
                {
                    // Динамическое создание label
                    Label label = new Label();
                    label.Name = "label" + i.ToString();
                    switch (i)
                    {
                        case 1:
                            label.Text = "Логин преподавателя:";
                            label.Location = new Point(8, 60);
                            break;
                        case 2:
                            label.Text = "Логин студента:";
                            label.Location = new Point(73, 120);
                            break;
                        case 3:
                            label.Text = "Почта студента:";
                            label.Location = new Point(73, 180);
                            break;
                    }
                    label.AutoSize = true;
                    label.Font = new Font(label.Font.Name, 16);
                    panel3.Controls.Add(label);

                    // Динамическое создание textbox
                    TextBox textbox = new TextBox();
                    textbox.Name = "textbox" + i.ToString();
                    switch (i)
                    {
                        case 1:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 57);
                            textbox.MaxLength = 50;
                            textbox.ShortcutsEnabled = false;
                            textbox.KeyPress += ZapretRus;
                            break;
                        case 2:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 117);
                            textbox.MaxLength = 50;
                            textbox.KeyPress += ZapretRus;
                            break;
                        case 3:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 177);
                            textbox.MaxLength = 150;
                            textbox.KeyPress += ZapretRus;
                            break;
                    }
                    textbox.Font = new Font(textbox.Font.Name, 16);
                    panel3.Controls.Add(textbox);
                }
            }

            student = 1;
        }

        private void GetTeacherInfoRegistration(object sender, EventArgs e)
        {
            // Присвоение переменной выбранного типа регистрации (Преподаватель)
            Registr = 2;

            // Изменение цвета выбранного типа регистрации (Преподаватель)
            GetColorLabel();

            // Очищение Panel
            GetClearPanel1();

            // Если тип регистрации был выбран первый раз
            if (teacher == 0)
            {
                // Заполнение Panel1
                for (int i = 4; i < 9; i++)
                {
                    // Динамическое создание label
                    Label label = new Label();
                    label.Name = "label" + i.ToString();
                    switch (i)
                    {
                        case 4:
                            label.Text = "Ключ регистрации:";
                            label.Location = new Point(42, 20);
                            break;
                        case 5:
                            label.Text = "Логин преподавателя:";
                            label.Location = new Point(8, 70);
                            break;
                        case 6:
                            label.Text = "Почта преподавателя:";
                            label.Location = new Point(6, 120);
                            break;
                        case 7:
                            label.Text = "Количество студентов:";
                            label.Location = new Point(0, 170);
                            break;
                        case 8:
                            label.Text = "Уникальное имя:";
                            label.Location = new Point(62, 220);
                            break;
                    }
                    label.AutoSize = true;
                    label.Font = new Font(label.Font.Name, 16);
                    panel3.Controls.Add(label);

                    // Динамическое создание textbox
                    TextBox textbox = new TextBox();
                    textbox.Name = "textbox" + i.ToString();
                    switch (i)
                    {
                        case 4:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 17);
                            textbox.ShortcutsEnabled = false;
                            break;
                        case 5:
                            textbox.Size = new Size(230, 40);
                            textbox.MaxLength = 50;
                            textbox.Location = new Point(250, 67);
                            textbox.KeyPress += ZapretRus;
                            break;
                        case 6:
                            textbox.Size = new Size(230, 40);
                            textbox.MaxLength = 150;
                            textbox.Location = new Point(250, 117);
                            textbox.KeyPress += ZapretRus;
                            break;
                        case 7:
                            textbox.Size = new Size(50, 40);
                            textbox.Location = new Point(250, 167);
                            textbox.MaxLength = 3;
                            textbox.KeyPress += ZapretRusAndEng;
                            break;
                        case 8:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 217);
                            textbox.MaxLength = 30;
                            textbox.KeyPress += ZapretRus;
                            break;
                    }
                    textbox.Font = new Font(textbox.Font.Name, 16);
                    panel3.Controls.Add(textbox);
                }
            }

            teacher = 1;
        }

        private void ZapretRusAndEng(object sender, KeyPressEventArgs e)
        {
            // Разрешение на ввод только цифр
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только цифры!", "red", panel1);
            }
        }

        private void ZapretRus(object sender, KeyPressEventArgs e)
        {
            // Запрет на ввод русских букв
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar=='@')
            {
            }
            else
            {
                CreateInfo("Возможно вводить только цифры и латинские буквы!", "red", panel1);
                e.Handled = true;
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            // Открытие формы авторизации
            OpenAutorizationForm();
        }

        private void GetRegistretion(object sender, EventArgs e)
        {
            // Обнуление переменных
            int TeacherId = 0;
            string EndData = "";

            // Подключение к БД
            con.Open();

            // Проверка выбранной формы регистрации
            switch (Registr)
            {
                // Регистрация студента
                case 1:
                    // Проверка на ввод данных
                    if ((panel3.Controls["textbox1"] as TextBox).Text != "")
                    {
                        // Проверка на ввод данных
                        if ((panel3.Controls["textbox2"] as TextBox).Text != "" && (panel3.Controls["textbox3"] as TextBox).Text != "")
                        {
                            // Проверка на существование преподавателя с указанным логином
                            TeacherId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox1"] as TextBox).Text + "'"));

                            // если преподаватель с указанным логином существует
                            if (TeacherId != 0)
                            {
                                // Проверка на существование пользователя с указанным логином
                                LoginCheck = new SQL_Query().GetInfoFromBD("select User_Mail from users where User_Login='" + (panel3.Controls["textbox2"].Text) + "'");

                                // Если пользователя с указанным логином не существует
                                if (LoginCheck == "0")
                                {
                                    // Проверка роли по указанному логину преподавателя
                                    string CheckRole = new SQL_Query().GetInfoFromBD("select Naim as 'RoleName' from Role where users_id = '" + TeacherId + "'");

                                    // Если указанный логин является логином преподавателя
                                    if (CheckRole == "Teacher")
                                    {
                                        // Проверка указанной почты на существование
                                        string CheckMail = "";
                                        CheckMail = new CheckExistMail().CheckMail(panel3.Controls["textbox3"].Text);

                                        // Если указанная почта существует
                                        if (CheckMail == "Почтовый ящик существует")
                                        {
                                            // Выбор номера преподавателя студента
                                            int PrepodID = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Teacher_id as 'ID' from users where id_user = '" + TeacherId + "'"));

                                            // Выбор количества доступных регистраций студентов у преподавателя
                                            int studentkolvo = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select KolvoNeRegStudents as 'kolvostudents' from Teachers where id_teacher = '" + PrepodID + "'"));

                                            // Если количество возможных регистраций студентов не равно 0 
                                            if (studentkolvo > 0)
                                            {
                                                // Генерация пароля для студента
                                                string password = GetPassword();

                                                try
                                                {
                                                    // Отправка пароля по почте
                                                    MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", (panel3.Controls["textbox3"] as TextBox).Text, "Пароль для программы Psico", password);
                                                    SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                                    client.Port = 587;
                                                    client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                                    client.EnableSsl = true;
                                                    client.Send(mail);
                                                }
                                                catch
                                                {
                                                    CreateInfo("Сообщение с вашим паролем не было отправлено, обратитесь к администратору!", "red", panel1);
                                                }

                                                // Запись данных в БД
                                                SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                StrPrc2.CommandType = CommandType.StoredProcedure;
                                                StrPrc2.Parameters.AddWithValue("@User_Login", (panel3.Controls["textbox2"] as TextBox).Text);
                                                StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka(password, "Pass"));
                                                StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka((panel3.Controls["textbox3"] as TextBox).Text, "Mail"));
                                                StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                                StrPrc2.Parameters.AddWithValue("@Teacher_id", PrepodID);
                                                StrPrc2.ExecuteNonQuery();

                                                // Изменение количества возможных регистраций студентов у преподавателя на 1
                                                new SQL_Query().UpdateOneCell("UPDATE Teachers SET KolvoNeRegStudents=" + (studentkolvo - 1) + " WHERE id_teacher = " + PrepodID + "");

                                                // Выбор номера добавленного пользователя
                                                int UserId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox2"] as TextBox).Text + "'"));

                                                // Запись данных В БД
                                                SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                StrPrc3.CommandType = CommandType.StoredProcedure;
                                                StrPrc3.Parameters.AddWithValue("@Naim", "Student");
                                                StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                StrPrc3.Parameters.AddWithValue("@Dostup_id", 3);
                                                StrPrc3.ExecuteNonQuery();

                                                // Вывод сообщения
                                                MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                // Переход на форму авторизации
                                                OpenAutorizationForm();
                                            }
                                            else
                                            {
                                                CreateInfo("Превышено максимально возможное число студентов у преподавателя!", "red", panel1);
                                            }
                                        }
                                        else
                                        {
                                            CreateInfo(CheckMail,"red",panel1);
                                        }
                                    }
                                    else
                                    {
                                        CreateInfo("Преподавателя с таким логином не существует!", "red", panel1);
                                    }
                                }
                                else
                                {
                                    CreateInfo("Пользователь с таким логином уже существует!", "red", panel1);
                                }
                            }
                            else
                            {
                                CreateInfo("Преподавателя с таким логином не существует!", "red", panel1);
                            }                       
                        }
                        else
                        {
                            CreateInfo("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.", "red", panel1);
                        }                        
                    }
                    else
                    {
                        CreateInfo("Вы не ввели логин преподавателя, без него регистрация невозможна!", "red", panel1);
                    }
                break;
                
                // Регистрация преподавателя
                case 2:
                    // Проверка на ввод данных
                    if ((panel3.Controls["textbox4"] as TextBox).Text != "")
                    {
                        // Выбор ключа активации программного продукта
                        KeyCheck = new SQL_Query().GetInfoFromBD("select DefenderKey from defender");

                        // Проверка указанного ключа активации на корректность
                        if ((panel3.Controls["textbox4"] as TextBox).Text == new Shifr().DeShifrovka(KeyCheck,"Kod"))
                        {
                            // Проверка на ввод данных
                            if ((panel3.Controls["textbox5"] as TextBox).Text != "" && (panel3.Controls["textbox6"] as TextBox).Text != "" && (panel3.Controls["textbox7"] as TextBox).Text != "")
                            {
                                // Проверка на существование указанного уникального имени в БД
                                checkteacherUniqueNaim = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher from Teachers where Unique_Naim='" + (panel3.Controls["textbox8"].Text) + "'"));

                                // Если указанное уникальное имя не зарегистрированно в БД
                                if (checkteacherUniqueNaim == 0)
                                {
                                    // Проверка на существование указанного логина в БД
                                    LoginCheck = new SQL_Query().GetInfoFromBD("select User_Mail from users where User_Login='" + (panel3.Controls["textbox5"].Text) + "'");

                                    // Если указанный логин не зарегистрирован в БД
                                    if (LoginCheck == "0")
                                    {
                                        // Проверка указанной почты на существование
                                        string CheckMail = "";
                                        CheckMail = new CheckExistMail().CheckMail(panel3.Controls["textbox6"].Text);

                                        // Если указанная почта существует
                                        if (CheckMail == "Почтовый ящик существует")
                                        {
                                            // Генерация пароля для преподавателя
                                            string password = GetPassword();

                                            try
                                            {
                                                // Отправка пароля по почте
                                                MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", (panel3.Controls["textbox6"] as TextBox).Text, "Пароль для программы Psico", password);
                                                SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                                client.Port = 587;
                                                client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                                client.EnableSsl = true;
                                                client.Send(mail);

                                                // Выбор сегодняшней даты
                                                string Day = new SQL_Query().GetInfoFromBD("select Day(getdate()) as 'Day'");
                                                if (Convert.ToInt32(Day) < 10)
                                                {
                                                    Day = "0" + Day;
                                                }
                                                string Month = (Convert.ToInt32(new SQL_Query().GetInfoFromBD("select MONTH(getdate()) as 'Month'")) + 1).ToString();
                                                if (Convert.ToInt32(Month) < 10)
                                                {
                                                    Month = "0" + Month;
                                                }
                                                string Year = new SQL_Query().GetInfoFromBD("select Year(getdate()) as 'Year'");
                                                EndData = Year + "-" + Month + "-" + Day;

                                                // Запись данных В БД
                                                SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                                StrPrc1.CommandType = CommandType.StoredProcedure;
                                                StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel3.Controls["textbox8"] as TextBox).Text);
                                                StrPrc1.Parameters.AddWithValue("@User_End_Data", EndData);
                                                StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", Convert.ToInt32((panel3.Controls["textbox7"] as TextBox).Text));
                                                StrPrc1.ExecuteNonQuery();

                                                // Выбор номера добавленного преподавателя
                                                TeacherId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel3.Controls["textbox8"] as TextBox).Text + "'"));

                                                // Запись данных В БД
                                                SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                StrPrc2.CommandType = CommandType.StoredProcedure;
                                                StrPrc2.Parameters.AddWithValue("@User_Login", (panel3.Controls["textbox5"] as TextBox).Text);
                                                StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka(password, "Pass"));
                                                StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka((panel3.Controls["textbox6"] as TextBox).Text, "Mail"));
                                                StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                                StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                                StrPrc2.ExecuteNonQuery();

                                                // Выбор номера добавленного пользователя
                                                int UserId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox5"] as TextBox).Text + "'"));

                                                // Запись данных В БД
                                                SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                StrPrc3.CommandType = CommandType.StoredProcedure;
                                                StrPrc3.Parameters.AddWithValue("@Naim", "Teacher");
                                                StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                StrPrc3.Parameters.AddWithValue("@Dostup_id", 2);
                                                StrPrc3.ExecuteNonQuery();

                                                // Генерация нового ключа активации
                                                string key = new Shifr().Shifrovka(GetKey(),"Kod");

                                                // Обновление ключа активации программного продукта
                                                new SQL_Query().UpdateOneCell("UPDATE defender SET DefenderKey='" + key + "' WHERE id_defend = 1");

                                                //Вывод сообщения
                                                MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                // Переход на форму авторизации
                                                OpenAutorizationForm();
                                            }
                                            catch
                                            {
                                                CreateInfo("Сообщение с вашим паролем не было отправлено, обратитесь к администратору!", "red", panel1);
                                            }
                                        }
                                        else
                                        {
                                            CreateInfo(CheckMail,"red",panel1);
                                        }
                                    }
                                    else
                                    {
                                        CreateInfo("Пользователь с таким логином уже существует!", "red", panel1);
                                    }
                                }
                                else
                                {
                                    CreateInfo("Введённое уникальное имя уже существует, выберите другое.", "red", panel1);
                                }
                            }
                            else
                            {
                                CreateInfo("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.", "red", panel1);
                            }
                        }
                        else
                        {
                            CreateInfo("Введённый ключ активации неверен! Пожалуйста убедитесь в правильности введённого ключа активации и попробуйде ещё раз.", "red", panel1);
                            (panel3.Controls["textbox4"] as TextBox).Text = "";
                        }
                    }
                    else
                    {
                        CreateInfo("Ключ активации не был введён, пожалуйста введите ключ активации для регистрации!", "red", panel1);
                    }
                break;
                
                // Если пользователь не выбрал форму регистрации
                default:
                    // Вывод сообщения
                    CreateInfo("Выберите способ регистрации!", "red", panel1);
                break;
            }

            con.Close();
        }

        public static string GetKey(int x=16)
        {
            // Генерация ключа активации программного продукта
            string key = "";
            var r = new Random();
            while (key.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    key += c;
            }
            return key;
        }

        public static string GetPassword(int x = 6)
        {
            // Генерация пароля для пользователя
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

        private void OpenAutorizationForm()
        {
            // Открытие формы авторизации
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void GetColorLabel()
        {
            // Визуальное изменение выбранной формы регистрации
            switch (Registr)
            {
                case 1:
                    label2.BackColor = Color.RoyalBlue;

                    label3.BackColor = Color.PowderBlue;
                    break;
                case 2:
                    label3.BackColor = Color.RoyalBlue;

                    label2.BackColor = Color.PowderBlue;
                    break;
            }
        }

        private void GetClearPanel1()
        {
            //Очищаем Panel
            switch (Registr)
            {
                // Если выбран тип регистрации "Студент"
                case 1:
                    if (teacher !=0)
                    {
                        (panel3.Controls["label4"] as Label).Visible = false;
                        (panel3.Controls["label5"] as Label).Visible = false;
                        (panel3.Controls["label6"] as Label).Visible = false;
                        (panel3.Controls["label7"] as Label).Visible = false;
                        (panel3.Controls["label8"] as Label).Visible = false;
                        (panel3.Controls["textbox4"] as TextBox).Visible = false;
                        (panel3.Controls["textbox5"] as TextBox).Visible = false;
                        (panel3.Controls["textbox6"] as TextBox).Visible = false;
                        (panel3.Controls["textbox7"] as TextBox).Visible = false;
                        (panel3.Controls["textbox8"] as TextBox).Visible = false;

                        if (student !=0)
                        {
                            (panel3.Controls["label1"] as Label).Visible = true;
                            (panel3.Controls["label2"] as Label).Visible = true;
                            (panel3.Controls["label3"] as Label).Visible = true;
                            (panel3.Controls["textbox1"] as TextBox).Visible = true;
                            (panel3.Controls["textbox2"] as TextBox).Visible = true;
                            (panel3.Controls["textbox3"] as TextBox).Visible = true;
                        }
                    }
                break;
                
                // Если выбран тип регистрации "Преподаватель"
                case 2:
                    if (student !=0)
                    {
                        (panel3.Controls["label1"] as Label).Visible = false;
                        (panel3.Controls["label2"] as Label).Visible = false;
                        (panel3.Controls["label3"] as Label).Visible = false;
                        (panel3.Controls["textbox1"] as TextBox).Visible = false;
                        (panel3.Controls["textbox2"] as TextBox).Visible = false;
                        (panel3.Controls["textbox3"] as TextBox).Visible = false;

                        if (teacher !=0)
                        {
                            (panel3.Controls["label4"] as Label).Visible = true;
                            (panel3.Controls["label5"] as Label).Visible = true;
                            (panel3.Controls["label6"] as Label).Visible = true;
                            (panel3.Controls["label7"] as Label).Visible = true;
                            (panel3.Controls["label8"] as Label).Visible = true;
                            (panel3.Controls["textbox4"] as TextBox).Visible = true;
                            (panel3.Controls["textbox5"] as TextBox).Visible = true;
                            (panel3.Controls["textbox6"] as TextBox).Visible = true;
                            (panel3.Controls["textbox7"] as TextBox).Visible = true;
                            (panel3.Controls["textbox8"] as TextBox).Visible = true;
                        }
                    }
                break;
            }

            Strela = 1;
            (panel3.Controls["label"] as Label).Visible = false;
            panel3.Invalidate();
        }

        private void Formalignment()
        {
            // Адаптация под разрешение экрана
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
            label.Font = new Font(label.Font.FontFamily, 14);
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
    }
}
