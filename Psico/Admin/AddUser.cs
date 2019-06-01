using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class AddUser : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        Timer timer = new Timer();
        int UserSelectedStatus = 0;
        int Admin = 0;
        int teacher = 0;
        int student = 0;

        public AddUser()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Открытие главной формы администратора
            OpenMainForm();
        }

        private void AddUsers(object sender, EventArgs e)
        {
            // Объявление переменных
            int TeacherId = 0;
            int checkteacherUniqueNaim = 0;
            string LoginCheck;

            // Добавление пользователя в зависимости от указанной роли
            switch (comboBox1.SelectedIndex)
            {
                // Добавление администратора
                case 0:
                    // Проверка на ввод данных
                    if ((panel1.Controls["textbox1"] as TextBox).Text != "" && (panel1.Controls["textbox2"] as TextBox).Text != "" && (panel1.Controls["textbox3"] as TextBox).Text != "" &&
                        (panel1.Controls["textbox4"] as TextBox).Text != "" && (panel1.Controls["EndDataMask1"] as MaskedTextBox).MaskCompleted == true)
                    {
                        // Проверка на существование указанного уникального имени в БД
                        checkteacherUniqueNaim = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher from Teachers where Unique_Naim='" + (panel1.Controls["textbox4"].Text) + "'"));

                        // Если указанное уникальное имя не зарегистрированно в БД
                        if (checkteacherUniqueNaim == 0)
                        {
                            // Проверка на существование указанного логина в БД
                            LoginCheck = new SQL_Query().GetInfoFromBD("select User_Mail from users where User_Login='" + (panel1.Controls["textbox1"].Text) + "'");

                            // Если указанный логин не существует в БД
                            if (LoginCheck == "0")
                            {
                                // Проверка на существование указанной почты
                                string CheckMail = "";
                                CheckMail = new CheckExistMail().CheckMail(panel1.Controls["textbox3"].Text);

                                // Если указанная почта существует
                                if (CheckMail == "Почтовый ящик существует")
                                {
                                    // Выбор сегодняшней даты
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

                                    // Выбор отдельно введённого года
                                    char year1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[0];
                                    char year2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[1];
                                    char year3 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[2];
                                    char year4 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[3];
                                    string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

                                    // Выбор отдельно введённого месяца
                                    char Month1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[5];
                                    char Month2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[6];
                                    string Month = Month1.ToString() + Month2.ToString();

                                    // Выбор отдельно введённого дня
                                    char Day1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[8];
                                    char Day2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[9];
                                    string Day = Day1.ToString() + Day2.ToString();

                                    // Проверка на корректность указаннного месяца и дня
                                    if (Convert.ToInt16(Month) != 00 && Convert.ToInt16(Day) != 00)
                                    {
                                        // Проверка на указанный месяц
                                        if (Convert.ToInt32(Month) <= 12)
                                        {
                                            // Проверка на указанный день
                                            if (Convert.ToInt32(Day) <= 28)
                                            {
                                                // Объявление сегодняшней и пользовательской даты
                                                DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
                                                DateTime UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

                                                // Если пользовательская дата больше сегодняшней
                                                if (UserData > NowData)
                                                {
                                                    try
                                                    {
                                                        // Подключение к БД
                                                        con.Open();

                                                        // Запись пользователя как преподавателя
                                                        SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                                        StrPrc1.CommandType = CommandType.StoredProcedure;
                                                        StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel1.Controls["textbox4"] as TextBox).Text);
                                                        StrPrc1.Parameters.AddWithValue("@User_End_Data", (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text);
                                                        StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", 999);
                                                        StrPrc1.ExecuteNonQuery();

                                                        // Выбор номера преподавателя
                                                        TeacherId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel1.Controls["textbox4"] as TextBox).Text + "'"));

                                                        // Запись пользователя 
                                                        SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                        StrPrc2.CommandType = CommandType.StoredProcedure;
                                                        StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox1"] as TextBox).Text);
                                                        StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka((panel1.Controls["textbox2"] as TextBox).Text, "Pass"));
                                                        StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka((panel1.Controls["textbox3"] as TextBox).Text, "Mail"));
                                                        StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                                        StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                                        StrPrc2.ExecuteNonQuery();

                                                        // Выбор номера пользователя
                                                        int UserId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox1"] as TextBox).Text + "'"));

                                                        // Запись информации о роли пользователя
                                                        SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                        StrPrc3.CommandType = CommandType.StoredProcedure;
                                                        StrPrc3.Parameters.AddWithValue("@Naim", "Admin");
                                                        StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                        StrPrc3.Parameters.AddWithValue("@Dostup_id", 1);
                                                        StrPrc3.ExecuteNonQuery();

                                                        // Отключение от БД
                                                        con.Close();

                                                        // Вывод сообщения
                                                        MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                        // Открытие главной формы администратора
                                                        OpenMainForm();
                                                    }
                                                    catch
                                                    {
                                                        CreateInfo("Сообщение с вашим паролем не было отправлено!", "red", panel1);
                                                        con.Close();
                                                    }
                                                }
                                                else
                                                {
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
                    break;

                // Добавление преподавателя
                case 1:

                    // Проверка на ввод данных
                    if ((panel1.Controls["textbox10"] as TextBox).Text != "" && (panel1.Controls["textbox6"] as TextBox).Text != "" && (panel1.Controls["textbox7"] as TextBox).Text != "" && 
                        (panel1.Controls["textbox8"] as TextBox).Text != "" && (panel1.Controls["textbox9"] as TextBox).Text != "" && (panel1.Controls["EndDataMask2"] as MaskedTextBox).MaskCompleted == true)
                    {
                        //Проверка на существование указанного уникального имени в БД
                        checkteacherUniqueNaim = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher from Teachers where Unique_Naim='" + (panel1.Controls["textbox10"].Text) + "'"));

                        // Если указанное уникальное имя не зарегистрированно в БД
                        if (checkteacherUniqueNaim == 0)
                        {
                            // Проверка на существование указанного логина в БД
                            LoginCheck = new SQL_Query().GetInfoFromBD("select User_Mail from users where User_Login='" + (panel1.Controls["textbox6"].Text) + "'");

                            // Если указанный логин не зарегистрирован в БД
                            if (LoginCheck == "0")
                            {
                                // Проверка указанной почты на существование
                                string checkmail = "";
                                checkmail = new CheckExistMail().CheckMail(panel1.Controls["textbox8"].Text);

                                // Если указанная почта существует
                                if (checkmail == "Почтовый ящик существует")
                                {
                                    //Выбор сегодняшней даты из БД
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

                                    // Выбор отдельно введённого года
                                    char year1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[0];
                                    char year2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[1];
                                    char year3 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[2];
                                    char year4 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[3];
                                    string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

                                    // Выбор отдельно введённого месяца
                                    char Month1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[5];
                                    char Month2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[6];
                                    string Month = Month1.ToString() + Month2.ToString();

                                    // Выбор отдельно введённого дня
                                    char Day1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[8];
                                    char Day2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[9];
                                    string Day = Day1.ToString() + Day2.ToString();

                                    // Проверка на корректность указаннного месяца и дня
                                    if (Convert.ToInt16(Month) != 00 && Convert.ToInt16(Day) != 00)
                                    {
                                        // Проверка указанного месяца
                                        if (Convert.ToInt32(Month) <= 12)
                                        {
                                            // Проверка указанного дня
                                            if (Convert.ToInt32(Day) <= 28)
                                            {
                                                // Объявление сегодняшней и пользовательской даты
                                                DateTime NowData = DateTime.Parse("" + NowDay + "/" + NowMonth + "/" + Nowyear + "");
                                                DateTime UserData = DateTime.Parse("" + Day + "/" + Month + "/" + Year + "");

                                                // Если пользовательская дата больше сегодняшней
                                                if (UserData > NowData)
                                                {
                                                    // Регистрация
                                                    try
                                                    {
                                                        // Подключение к БД
                                                        con.Open();

                                                        // Запись данных о новом преподавателе
                                                        SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                                        StrPrc1.CommandType = CommandType.StoredProcedure;
                                                        StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel1.Controls["textbox10"] as TextBox).Text);
                                                        StrPrc1.Parameters.AddWithValue("@User_End_Data", (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text);
                                                        StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", Convert.ToInt32((panel1.Controls["textbox9"] as TextBox).Text));
                                                        StrPrc1.ExecuteNonQuery();

                                                        // Выбор номера нового преподавателя
                                                        TeacherId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel1.Controls["textbox10"] as TextBox).Text + "'"));

                                                        // Запись данных о новом пользователе
                                                        SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                        StrPrc2.CommandType = CommandType.StoredProcedure;
                                                        StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox6"] as TextBox).Text);
                                                        StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka((panel1.Controls["textbox7"] as TextBox).Text, "Pass"));
                                                        StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka((panel1.Controls["textbox8"] as TextBox).Text, "Mail"));
                                                        StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                                        StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                                        StrPrc2.ExecuteNonQuery();

                                                        // Выбор номера пользователя 
                                                        int UserId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox6"] as TextBox).Text + "'"));

                                                        // Запись данных о роли нового пользователя
                                                        SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                        StrPrc3.CommandType = CommandType.StoredProcedure;
                                                        StrPrc3.Parameters.AddWithValue("@Naim", "Teacher");
                                                        StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                        StrPrc3.Parameters.AddWithValue("@Dostup_id", 2);
                                                        StrPrc3.ExecuteNonQuery();

                                                        // Отключение от БД
                                                        con.Close();

                                                        // Вывод сообщения
                                                        MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                        // Открытие главной формы администратора
                                                        OpenMainForm();
                                                    }
                                                    catch
                                                    {
                                                        CreateInfo("Сообщение с вашим паролем не было отправлено!", "red", panel1);
                                                        con.Close();
                                                    }
                                                }
                                                else
                                                {
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
                                    CreateInfo(checkmail,"red",panel1);
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
                    break;

                // Студент
                case 2:
                    // Проверка на ввод данных
                    if ((panel1.Controls["textbox12"] as TextBox).Text != "")
                    {
                        // Проверка на ввод данных
                        if ((panel1.Controls["textbox14"] as TextBox).Text != "" && (panel1.Controls["textbox15"] as TextBox).Text != "" && (panel1.Controls["textbox12"] as TextBox).Text != "" &&
                            (panel1.Controls["textbox13"] as TextBox).Text != "")
                        {
                            // Проверка на существование преподавателя по указанному логину
                            TeacherId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox12"] as TextBox).Text + "'"));

                            // Если преподаватель существует
                            if (TeacherId != 0)
                            {
                                // Проверка на существования пользователя с выбранным логином
                                LoginCheck = new SQL_Query().GetInfoFromBD("select User_Mail from users where User_Login='" + (panel1.Controls["textbox13"].Text) + "'");

                                // Если указанный логин не зарегистрирован в БД
                                if (LoginCheck == "0")
                                {
                                    // Проверка роли по указанному логину преподавателя
                                    string CheckRole = new SQL_Query().GetInfoFromBD("select Naim as 'RoleName' from Role where users_id = '" + TeacherId + "'");

                                    // Если роль является "Преподаватель"
                                    if (CheckRole == "Teacher")
                                    {
                                        // Выбор номера преподавателя
                                        int PrepodID = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Teacher_id as 'ID' from users where id_user = '" + TeacherId + "'"));

                                        // Выбор количества возможных регистраций студентов у преподавателя
                                        int studentkolvo = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select KolvoNeRegStudents as 'kolvostudents' from Teachers where id_teacher = '" + PrepodID + "'"));

                                        // Если количество больше 0
                                        if (studentkolvo > 0)
                                        {
                                            // Проверка указанной почты на существование
                                            string checkmail = "";
                                            checkmail = new CheckExistMail().CheckMail(panel1.Controls["textbox15"].Text);

                                            // Если указанная почта существует
                                            if (checkmail == "Почтовый ящик существует")
                                            {
                                                // Подключение к БД
                                                con.Open();

                                                // Запись данных о новом пользователе в БД
                                                SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                StrPrc2.CommandType = CommandType.StoredProcedure;
                                                StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox13"] as TextBox).Text);
                                                StrPrc2.Parameters.AddWithValue("@User_Password", new Shifr().Shifrovka((panel1.Controls["textbox14"] as TextBox).Text, "Pass"));
                                                StrPrc2.Parameters.AddWithValue("@User_Mail", new Shifr().Shifrovka((panel1.Controls["textbox15"] as TextBox).Text, "Mail"));
                                                StrPrc2.Parameters.AddWithValue("@UserStatus", 0);
                                                StrPrc2.Parameters.AddWithValue("@Teacher_id", PrepodID);
                                                StrPrc2.ExecuteNonQuery();

                                                // Объявление переменной с новым значением количества возможных регистраций у преподавателя
                                                int UpdateStudentKolvo = studentkolvo - 1;

                                                // Изменение количества регистраций студентов у преподавателя
                                                new SQL_Query().UpdateOneCell("UPDATE teachers SET KolvoNeRegStudents="+UpdateStudentKolvo+" WHERE id_teacher = " + PrepodID + "");

                                                // Выбор номера нового пользователя
                                                int UserId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox13"] as TextBox).Text + "'"));

                                                // Запись данных о роли нового пользователя в БД
                                                SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                StrPrc3.CommandType = CommandType.StoredProcedure;
                                                StrPrc3.Parameters.AddWithValue("@Naim", "Student");
                                                StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                StrPrc3.Parameters.AddWithValue("@Dostup_id", 3);
                                                StrPrc3.ExecuteNonQuery();

                                                // Отключение от БД
                                                con.Close();

                                                // Вывод сообщения
                                                MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                // Открытие главной формы администратора
                                                OpenMainForm();
                                            }
                                            else
                                            {
                                                // Вывод сообщени
                                                CreateInfo(checkmail,"red",panel1);

                                                // Отключение от БД
                                                con.Close();
                                            }
                                        }
                                        else
                                        {
                                            CreateInfo("Превышено максимально возможное число студентов у преподавателя!", "red", panel1);
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

                // Тип регистрации не выбран
                default:
                    CreateInfo("Для добавления пользователя необходимо выбрать его статус и ввести для него данные!","red", panel1);
                    break;
            }
        }

        private void UserStatusChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                // Выбран Администратор
                case 0:

                    UserSelectedStatus = 1;
                    CleanForm();

                    if (Admin == 0)
                    {
                        // Заполнение Panel1
                        for (int i = 1; i < 6; i++)
                        {
                            // Создание label
                            Label label = new Label();
                            label.Name = "label" + i.ToString();
                            switch (i)
                            {
                                case 1:
                                    label.Text = "Логин администратора:";
                                    label.Location = new Point(52, 120);
                                    break;
                                case 2:
                                    label.Text = "Пароль администратора:";
                                    label.Location = new Point(35, 160);
                                    break;
                                case 3:
                                    label.Text = "Почта администратора:";
                                    label.Location = new Point(50, 200);
                                    break;
                                case 4:
                                    label.Text = "Уникальное имя:";
                                    label.Location = new Point(115, 240);
                                    break;
                                case 5:
                                    label.Text = "Дата окончания использования:";
                                    label.Location = new Point(50, 280);
                                    break;
                            }
                            label.AutoSize = true;
                            label.Font = new Font(label.Font.Name, 16, FontStyle.Regular);
                            panel1.Controls.Add(label);

                            // Создание textbox
                            TextBox textbox = new TextBox();
                            textbox.Name = "textbox" + i.ToString();
                            switch (i)
                            {
                                case 1:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 117);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 2:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 157);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 3:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 197);
                                    textbox.MaxLength = 150;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 4:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 237);
                                    textbox.MaxLength = 30;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 5:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 237);
                                    break;
                            }
                            textbox.Font = new Font(textbox.Font.Name, 16);
                            panel1.Controls.Add(textbox);
                        }

                        //Создание maskedtextbox
                        MaskedTextBox maskedTextBox = new MaskedTextBox();
                        maskedTextBox.Name = "EndDataMask1";
                        maskedTextBox.Size = new Size(125, 40);
                        maskedTextBox.Location = new Point(404, 277);
                        maskedTextBox.Mask = "2000-00-00";
                        maskedTextBox.AsciiOnly = true;
                        maskedTextBox.Font = new Font(maskedTextBox.Font.Name, 16, FontStyle.Regular);
                        maskedTextBox.KeyPress += ZapretRusAndEng;
                        panel1.Controls.Add(maskedTextBox);
                    }

                    Admin = 1;
                    (panel1.Controls["textbox5"] as TextBox).Visible = false;
                    break;

                // Выбран Преподаватель
                case 1:

                    UserSelectedStatus = 2;
                    CleanForm();

                    if (teacher == 0)
                    {
                        // Заполнение Panel1
                        for (int i = 6; i < 12; i++)
                        {
                            // Создание label
                            Label label = new Label();
                            label.Name = "label" + i.ToString();
                            switch (i)
                            {
                                case 6:
                                    label.Text = "Логин преподавателя:";
                                    label.Location = new Point(65, 120);
                                    break;
                                case 7:
                                    label.Text = "Пароль преподавателя:";
                                    label.Location = new Point(48, 160);
                                    break;
                                case 8:
                                    label.Text = "Почта преподавателя:";
                                    label.Location = new Point(61, 200);
                                    break;
                                case 9:
                                    label.Text = "Количество студентов:";
                                    label.Location = new Point(56, 240);
                                    break;
                                case 10:
                                    label.Text = "Уникальное имя:";
                                    label.Location = new Point(116, 280);
                                    break;
                                case 11:
                                    label.Text = "Дата окончания использования:";
                                    label.Location = new Point(50, 320);
                                    break;
                            }
                            label.AutoSize = true;
                            label.Font = new Font(label.Font.Name, 16);
                            panel1.Controls.Add(label);

                            // Создание textbox
                            TextBox textbox = new TextBox();
                            textbox.Name = "textbox" + i.ToString();
                            switch (i)
                            {
                                case 6:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 117);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 7:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 157);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 8:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 197);
                                    textbox.MaxLength = 150;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 9:
                                    textbox.Size = new Size(50, 40);
                                    textbox.Location = new Point(404, 237);
                                    textbox.MaxLength = 3;
                                    textbox.KeyPress += ZapretRusAndEng;
                                    break;
                                case 10:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 277);
                                    textbox.MaxLength = 30;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 11:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 277);
                                    break;
                            }
                            textbox.Font = new Font(textbox.Font.Name, 16);
                            panel1.Controls.Add(textbox);
                        }

                        //Создание maskedtextbox
                        MaskedTextBox maskedTextBox = new MaskedTextBox();
                        maskedTextBox.Name = "EndDataMask2";
                        maskedTextBox.Size = new Size(125, 40);
                        maskedTextBox.Location = new Point(404, 317);
                        maskedTextBox.Mask = "2000-00-00";
                        maskedTextBox.AsciiOnly = true;
                        maskedTextBox.Font = new Font(maskedTextBox.Font.Name, 16, FontStyle.Regular);
                        panel1.Controls.Add(maskedTextBox);
                        maskedTextBox.KeyPress += ZapretRusAndEng;
                    }

                    teacher = 1;
                    (panel1.Controls["textbox11"] as TextBox).Visible = false;
                    break;

                // Выбран Студент
                case 2:

                    UserSelectedStatus = 3;
                    CleanForm();

                    if (student == 0)
                    {
                        // Заполнение Panel1
                        for (int i = 12; i < 16; i++)
                        {
                            // Создание label
                            Label label = new Label();
                            label.Name = "label" + i.ToString();
                            switch (i)
                            {
                                case 12:
                                    label.Text = "Логин преподавателя:";
                                    label.Location = new Point(65, 120);
                                    break;
                                case 13:
                                    label.Text = "Логин студента:";
                                    label.Location = new Point(130, 160);
                                    break;
                                case 14:
                                    label.Text = "Пароль студента:";
                                    label.Location = new Point(113, 200);
                                    break;
                                case 15:
                                    label.Text = "Почта студента:";
                                    label.Location = new Point(128, 240);
                                    break;
                            }
                            label.AutoSize = true;
                            label.Font = new Font(label.Font.Name, 16);
                            panel1.Controls.Add(label);

                            // Создание textbox
                            TextBox textbox = new TextBox();
                            textbox.Name = "textbox" + i.ToString();
                            switch (i)
                            {
                                case 12:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 117);
                                    textbox.MaxLength = 50;
                                    textbox.ShortcutsEnabled = false;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 13:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 157);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 14:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 197);
                                    textbox.MaxLength = 50;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                                case 15:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 237);
                                    textbox.MaxLength = 150;
                                    textbox.KeyPress += ZapretRus;
                                    break;
                            }
                            textbox.Font = new Font(textbox.Font.Name, 16);
                            panel1.Controls.Add(textbox);
                        }
                    }
                    student = 1;
                    break;
            }
        }

        private void CleanForm()
        {
            // Очистка формы
            switch (UserSelectedStatus)
            {
                // Если выбран тип регистрации "Админ"
                case 1:
                    if (teacher != 0)
                    {
                        (panel1.Controls["label6"] as Label).Visible = false;
                        (panel1.Controls["label7"] as Label).Visible = false;
                        (panel1.Controls["label8"] as Label).Visible = false;
                        (panel1.Controls["label9"] as Label).Visible = false;
                        (panel1.Controls["label10"] as Label).Visible = false;
                        (panel1.Controls["label11"] as Label).Visible = false;
                        (panel1.Controls["textbox6"] as TextBox).Visible = false;
                        (panel1.Controls["textbox7"] as TextBox).Visible = false;
                        (panel1.Controls["textbox8"] as TextBox).Visible = false;
                        (panel1.Controls["textbox9"] as TextBox).Visible = false;
                        (panel1.Controls["textbox10"] as TextBox).Visible = false;
                        (panel1.Controls["EndDataMask2"] as MaskedTextBox).Visible = false;
                    }
                    if (student != 0)
                    {
                        (panel1.Controls["label12"] as Label).Visible = false;
                        (panel1.Controls["label13"] as Label).Visible = false;
                        (panel1.Controls["label14"] as Label).Visible = false;
                        (panel1.Controls["label15"] as Label).Visible = false;
                        (panel1.Controls["textbox12"] as TextBox).Visible = false;
                        (panel1.Controls["textbox13"] as TextBox).Visible = false;
                        (panel1.Controls["textbox14"] as TextBox).Visible = false;
                        (panel1.Controls["textbox15"] as TextBox).Visible = false;
                    }
                    if (Admin != 0)
                    {
                        (panel1.Controls["label1"] as Label).Visible = true;
                        (panel1.Controls["label2"] as Label).Visible = true;
                        (panel1.Controls["label3"] as Label).Visible = true;
                        (panel1.Controls["label4"] as Label).Visible = true;
                        (panel1.Controls["label5"] as Label).Visible = true;
                        (panel1.Controls["textbox1"] as TextBox).Visible = true;
                        (panel1.Controls["textbox2"] as TextBox).Visible = true;
                        (panel1.Controls["textbox3"] as TextBox).Visible = true;
                        (panel1.Controls["textbox4"] as TextBox).Visible = true;
                        (panel1.Controls["EndDataMask1"] as MaskedTextBox).Visible = true;
                    }
                    break;

                // Если выбран тип регистрации "Преподаватель"
                case 2:
                    if (Admin !=0)
                    {
                        (panel1.Controls["label1"] as Label).Visible = false;
                        (panel1.Controls["label2"] as Label).Visible = false;
                        (panel1.Controls["label3"] as Label).Visible = false;
                        (panel1.Controls["label4"] as Label).Visible = false;
                        (panel1.Controls["label5"] as Label).Visible = false;
                        (panel1.Controls["textbox1"] as TextBox).Visible = false;
                        (panel1.Controls["textbox2"] as TextBox).Visible = false;
                        (panel1.Controls["textbox3"] as TextBox).Visible = false;
                        (panel1.Controls["textbox4"] as TextBox).Visible = false;
                        (panel1.Controls["EndDataMask1"] as MaskedTextBox).Visible = false;
                    }
                    if (student !=0)
                    {
                        (panel1.Controls["label12"] as Label).Visible = false;
                        (panel1.Controls["label13"] as Label).Visible = false;
                        (panel1.Controls["label14"] as Label).Visible = false;
                        (panel1.Controls["label15"] as Label).Visible = false;
                        (panel1.Controls["textbox12"] as TextBox).Visible = false;
                        (panel1.Controls["textbox13"] as TextBox).Visible = false;
                        (panel1.Controls["textbox14"] as TextBox).Visible = false;
                        (panel1.Controls["textbox15"] as TextBox).Visible = false;
                    }
                    if (teacher !=0)
                    {
                        (panel1.Controls["label6"] as Label).Visible = true;
                        (panel1.Controls["label7"] as Label).Visible = true;
                        (panel1.Controls["label8"] as Label).Visible = true;
                        (panel1.Controls["label9"] as Label).Visible = true;
                        (panel1.Controls["label10"] as Label).Visible = true;
                        (panel1.Controls["label11"] as Label).Visible = true;
                        (panel1.Controls["textbox6"] as TextBox).Visible = true;
                        (panel1.Controls["textbox7"] as TextBox).Visible = true;
                        (panel1.Controls["textbox8"] as TextBox).Visible = true;
                        (panel1.Controls["textbox9"] as TextBox).Visible = true;
                        (panel1.Controls["textbox10"] as TextBox).Visible = true;
                        (panel1.Controls["EndDataMask2"] as MaskedTextBox).Visible = true;
                    }                    
                    break;

                // Если выбран тип регистрации "Студент"
                case 3:
                    if (Admin != 0)
                    {
                        (panel1.Controls["label1"] as Label).Visible = false;
                        (panel1.Controls["label2"] as Label).Visible = false;
                        (panel1.Controls["label3"] as Label).Visible = false;
                        (panel1.Controls["label4"] as Label).Visible = false;
                        (panel1.Controls["label5"] as Label).Visible = false;
                        (panel1.Controls["textbox1"] as TextBox).Visible = false;
                        (panel1.Controls["textbox2"] as TextBox).Visible = false;
                        (panel1.Controls["textbox3"] as TextBox).Visible = false;
                        (panel1.Controls["textbox4"] as TextBox).Visible = false;
                        (panel1.Controls["EndDataMask1"] as MaskedTextBox).Visible = false;
                    }
                    if (teacher != 0)
                    {
                        (panel1.Controls["label6"] as Label).Visible = false;
                        (panel1.Controls["label7"] as Label).Visible = false;
                        (panel1.Controls["label8"] as Label).Visible = false;
                        (panel1.Controls["label9"] as Label).Visible = false;
                        (panel1.Controls["label10"] as Label).Visible = false;
                        (panel1.Controls["label11"] as Label).Visible = false;
                        (panel1.Controls["textbox6"] as TextBox).Visible = false;
                        (panel1.Controls["textbox7"] as TextBox).Visible = false;
                        (panel1.Controls["textbox8"] as TextBox).Visible = false;
                        (panel1.Controls["textbox9"] as TextBox).Visible = false;
                        (panel1.Controls["textbox10"] as TextBox).Visible = false;
                        (panel1.Controls["EndDataMask2"] as MaskedTextBox).Visible = false;
                    }
                    if (student != 0)
                    {
                        (panel1.Controls["label12"] as Label).Visible = true;
                        (panel1.Controls["label13"] as Label).Visible = true;
                        (panel1.Controls["label14"] as Label).Visible = true;
                        (panel1.Controls["label15"] as Label).Visible = true;
                        (panel1.Controls["textbox12"] as TextBox).Visible = true;
                        (panel1.Controls["textbox13"] as TextBox).Visible = true;
                        (panel1.Controls["textbox14"] as TextBox).Visible = true;
                        (panel1.Controls["textbox15"] as TextBox).Visible = true;
                    }
                    break;
            }
        }

        private void OpenMainForm()
        {
            //Открытие главной формы администратора
            new administrator().Show();
            Close();
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

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация под разрешение экрана
            FormAlignment();

            // Динамическое создание Label
            Label label = new Label();
            label.Text = "Роль пользователя:";
            label.Location = new Point(67,69);
            label.Font = new Font(label.Font.Name, 16);
            label.AutoSize = true;
            panel1.Controls.Add(label);
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
            }catch{}
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }catch { }
        }

        private void ZapretRusAndEng(object sender, KeyPressEventArgs e)
        {
            // Запрет на ввод букв
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
