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
    public partial class AddUser : Form
    {
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
            OpenMainForm();
        }

        private void AddUsers(object sender, EventArgs e)
        {
            int TeacherId = 0;
            int checkteacherUniqueNaim = 0;
            string LoginCheck;

            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open();

            switch (comboBox1.SelectedIndex)
            {
                // Администратор
                case 0:
                    // Проверка введённых данных
                    if ((panel1.Controls["textbox1"] as TextBox).Text != "" && (panel1.Controls["textbox2"] as TextBox).Text != "" && (panel1.Controls["textbox3"] as TextBox).Text != "" &&
                        (panel1.Controls["textbox4"] as TextBox).Text != "" && (panel1.Controls["EndDataMask1"] as MaskedTextBox).MaskCompleted == true)
                    {
                        //Выбор данных из БД
                        SqlCommand CheckUniqueName = new SqlCommand("select id_teacher from Teachers where Unique_Naim='" + (panel1.Controls["textbox4"].Text) + "'", con);
                        SqlDataReader dr1 = CheckUniqueName.ExecuteReader();

                        try
                        {
                            // Запись данных из БД
                            dr1.Read();
                            checkteacherUniqueNaim = Convert.ToInt32(dr1["id_teacher"].ToString());
                            dr1.Close();
                        }
                        catch
                        {
                            checkteacherUniqueNaim = 0;
                            dr1.Close();
                        }

                        if (checkteacherUniqueNaim == 0)
                        {
                            //Выбор данных из БД
                            SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel1.Controls["textbox1"].Text) + "'", con);
                            SqlDataReader dr2 = CheckUserLogin.ExecuteReader();

                            try
                            {
                                // Запись данных из БД
                                dr2.Read();
                                LoginCheck = dr2["User_Mail"].ToString();
                                dr2.Close();
                            }
                            catch
                            {
                                LoginCheck = "";
                                dr2.Close();
                            }

                            if (LoginCheck == "")
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

                                char year1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[0];
                                char year2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[1];
                                char year3 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[2];
                                char year4 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[3];
                                string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

                                char Month1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[5];
                                char Month2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[6];
                                string Month = Month1.ToString() + Month2.ToString();

                                char Day1 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[8];
                                char Day2 = (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text[9];
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
                                                // Регистрация
                                                try
                                                {
                                                    // Запись данных В БД
                                                    SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                                    StrPrc1.CommandType = CommandType.StoredProcedure;
                                                    StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel1.Controls["textbox4"] as TextBox).Text);
                                                    StrPrc1.Parameters.AddWithValue("@User_End_Data", (panel1.Controls["EndDataMask1"] as MaskedTextBox).Text);
                                                    StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", 999);
                                                    StrPrc1.ExecuteNonQuery();

                                                    // Выбор количества данных в таблице БД
                                                    SqlCommand GetTeacherId = new SqlCommand("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel1.Controls["textbox4"] as TextBox).Text + "'", con);
                                                    SqlDataReader dr6 = GetTeacherId.ExecuteReader();
                                                    dr6.Read();
                                                    TeacherId = Convert.ToInt32(dr6["id"].ToString());
                                                    dr6.Close();

                                                    // Запись данных В БД
                                                    SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                    StrPrc2.CommandType = CommandType.StoredProcedure;
                                                    StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox1"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@User_Password", (panel1.Controls["textbox2"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@User_Mail", (panel1.Controls["textbox3"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                                    StrPrc2.ExecuteNonQuery();

                                                    // Выбор количества данных в таблице БД
                                                    SqlCommand GetUserID = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox1"] as TextBox).Text + "'", con);
                                                    SqlDataReader dr7 = GetUserID.ExecuteReader();
                                                    dr7.Read();
                                                    int UserId = Convert.ToInt32(dr7["id"].ToString());
                                                    dr7.Close();

                                                    // Запись данных В БД
                                                    SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                    StrPrc3.CommandType = CommandType.StoredProcedure;
                                                    StrPrc3.Parameters.AddWithValue("@Naim", "Admin");
                                                    StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                    StrPrc3.Parameters.AddWithValue("@Dostup_id", 1);
                                                    StrPrc3.ExecuteNonQuery();

                                                    MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                    OpenMainForm();
                                                }
                                                catch
                                                {
                                                    CreateInfo("Сообщение с вашим паролем не было отправлено!");
                                                }
                                            }
                                            else
                                            {
                                                CreateInfo("Выбранная вами дата окончания уже прошла!");
                                            }
                                        }
                                        else
                                        {
                                            CreateInfo("Максимально возможный для выбора день - 28!");
                                        }
                                    }
                                    else
                                    {
                                        CreateInfo("Некорректно введён месяц окончания!");
                                    }
                                }
                                else
                                {
                                    CreateInfo("Некорректно введён месяц или день окончания!");
                                }
                            }
                            else
                            {
                                CreateInfo("Пользователь с таким логином уже существует!");
                            }
                        }
                        else
                        {
                            CreateInfo("Введённое уникальное имя уже существует, выберите другое.");
                        }
                    }
                    else
                    {
                        CreateInfo("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.");
                    }
                    break;

                // Преподаватель
                case 1:

                    // Проверка введённых данных
                    if ((panel1.Controls["textbox10"] as TextBox).Text != "" && (panel1.Controls["textbox6"] as TextBox).Text != "" && (panel1.Controls["textbox7"] as TextBox).Text != "" && 
                        (panel1.Controls["textbox8"] as TextBox).Text != "" && (panel1.Controls["textbox9"] as TextBox).Text != "" && (panel1.Controls["EndDataMask2"] as MaskedTextBox).MaskCompleted == true)
                    {
                        //Выбор данных из БД
                        SqlCommand CheckUniqueName = new SqlCommand("select id_teacher from Teachers where Unique_Naim='" + (panel1.Controls["textbox10"].Text) + "'", con);
                        SqlDataReader dr1 = CheckUniqueName.ExecuteReader();

                        try
                        {
                            // Запись данных из БД
                            dr1.Read();
                            checkteacherUniqueNaim = Convert.ToInt32(dr1["id_teacher"].ToString());
                            dr1.Close();
                        }
                        catch
                        {
                            checkteacherUniqueNaim = 0;
                            dr1.Close();
                        }

                        if (checkteacherUniqueNaim == 0)
                        {
                            //Выбор данных из БД
                            SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel1.Controls["textbox6"].Text) + "'", con);
                            SqlDataReader dr2 = CheckUserLogin.ExecuteReader();

                            try
                            {
                                // Запись данных из БД
                                dr2.Read();
                                LoginCheck = dr2["User_Mail"].ToString();
                                dr2.Close();
                            }
                            catch
                            {
                                LoginCheck = "";
                                dr2.Close();
                            }

                            if (LoginCheck == "")
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

                                char year1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[0];
                                char year2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[1];
                                char year3 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[2];
                                char year4 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[3];
                                string Year = year1.ToString() + year2.ToString() + year3.ToString() + year4.ToString();

                                char Month1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[5];
                                char Month2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[6];
                                string Month = Month1.ToString() + Month2.ToString();

                                char Day1 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[8];
                                char Day2 = (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text[9];
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
                                                // Регистрация
                                                try
                                                {
                                                    // Запись данных В БД
                                                    SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                                    StrPrc1.CommandType = CommandType.StoredProcedure;
                                                    StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel1.Controls["textbox10"] as TextBox).Text);
                                                    StrPrc1.Parameters.AddWithValue("@User_End_Data", (panel1.Controls["EndDataMask2"] as MaskedTextBox).Text);
                                                    StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", Convert.ToInt32((panel1.Controls["textbox9"] as TextBox).Text));
                                                    StrPrc1.ExecuteNonQuery();

                                                    // Выбор количества данных в таблице БД
                                                    SqlCommand GetTeacherId = new SqlCommand("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel1.Controls["textbox10"] as TextBox).Text + "'", con);
                                                    SqlDataReader dr6 = GetTeacherId.ExecuteReader();
                                                    dr6.Read();
                                                    TeacherId = Convert.ToInt32(dr6["id"].ToString());
                                                    dr6.Close();

                                                    // Запись данных В БД
                                                    SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                                    StrPrc2.CommandType = CommandType.StoredProcedure;
                                                    StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox6"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@User_Password", (panel1.Controls["textbox7"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@User_Mail", (panel1.Controls["textbox8"] as TextBox).Text);
                                                    StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                                    StrPrc2.ExecuteNonQuery();

                                                    // Выбор количества данных в таблице БД
                                                    SqlCommand GetUserID = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox6"] as TextBox).Text + "'", con);
                                                    SqlDataReader dr7 = GetUserID.ExecuteReader();
                                                    dr7.Read();
                                                    int UserId = Convert.ToInt32(dr7["id"].ToString());
                                                    dr7.Close();

                                                    // Запись данных В БД
                                                    SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                                    StrPrc3.CommandType = CommandType.StoredProcedure;
                                                    StrPrc3.Parameters.AddWithValue("@Naim", "Teacher");
                                                    StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                                    StrPrc3.Parameters.AddWithValue("@Dostup_id", 2);
                                                    StrPrc3.ExecuteNonQuery();

                                                    MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                    OpenMainForm();
                                                }
                                                catch
                                                {
                                                    CreateInfo("Сообщение с вашим паролем не было отправлено!");
                                                }
                                            }
                                            else
                                            {
                                                CreateInfo("Выбранная вами дата окончания уже прошла!");
                                            }
                                        }
                                        else
                                        {
                                            CreateInfo("Максимально возможный для выбора день - 28!");
                                        }
                                    }
                                    else
                                    {
                                        CreateInfo("Некорректно введён месяц окончания!");
                                    }
                                }
                                else
                                {
                                    CreateInfo("Некорректно введён месяц или день окончания!");
                                }                                
                            }
                            else
                            {
                                CreateInfo("Пользователь с таким логином уже существует!");
                            }
                        }
                        else
                        {
                            CreateInfo("Введённое уникальное имя уже существует, выберите другое.");
                        }
                    }
                    else
                    {
                        CreateInfo("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.");
                    }
                    break;

                // Студент
                case 2:
                    if ((panel1.Controls["textbox12"] as TextBox).Text != "")
                    {
                        // Проверка введённых данных
                        if ((panel1.Controls["textbox14"] as TextBox).Text != "" && (panel1.Controls["textbox15"] as TextBox).Text != "" && (panel1.Controls["textbox12"] as TextBox).Text != "" &&
                            (panel1.Controls["textbox13"] as TextBox).Text != "")
                        {
                            // Проверка на существование преподавателя по его логину
                            SqlCommand GetTeacherId = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox12"] as TextBox).Text + "'", con);
                            SqlDataReader dr2 = GetTeacherId.ExecuteReader();

                            try
                            {
                                dr2.Read();
                                TeacherId = Convert.ToInt32(dr2["id"].ToString());
                                dr2.Close();
                            }

                            catch
                            {
                                TeacherId = 0;
                                dr2.Close();
                            }

                            if (TeacherId != 0)
                            {
                                // Проверка на существования пользователя с выбранным логином
                                SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel1.Controls["textbox13"].Text) + "'", con);
                                SqlDataReader dr1 = CheckUserLogin.ExecuteReader();

                                try
                                {
                                    // Запись данных из БД
                                    dr1.Read();
                                    LoginCheck = dr1["User_Mail"].ToString();
                                    dr1.Close();
                                }
                                catch
                                {
                                    LoginCheck = "";
                                    dr1.Close();
                                }

                                if (LoginCheck == "")
                                {
                                    // Проверка введённого логина преподавателя 
                                    SqlCommand CheckRoleName = new SqlCommand("select Naim as 'RoleName' from Role where users_id = '" + TeacherId + "'", con);
                                    SqlDataReader dr3 = CheckRoleName.ExecuteReader();
                                    dr3.Read();
                                    string CheckRole = dr3["RoleName"].ToString();
                                    dr3.Close();

                                    if (CheckRole == "Teacher")
                                    {
                                        // Выбор номера преподавателя в БД
                                        SqlCommand GetPrepodId = new SqlCommand("select Teacher_id as 'ID' from users where id_user = '" + TeacherId + "'", con);
                                        SqlDataReader dr4 = GetPrepodId.ExecuteReader();
                                        dr4.Read();
                                        int PrepodID = Convert.ToInt32(dr4["ID"].ToString());
                                        dr4.Close();

                                        // Проверка на возможность добавить ещё студентов для выбранного преподавателя
                                        SqlCommand GetKolvoStudent = new SqlCommand("select KolvoNeRegStudents as 'kolvostudents' from Teachers where id_teacher = '" + PrepodID + "'", con);
                                        SqlDataReader dr5 = GetKolvoStudent.ExecuteReader();
                                        dr5.Read();
                                        int studentkolvo = Convert.ToInt32(dr5["kolvostudents"].ToString());
                                        dr5.Close();

                                        if (studentkolvo > 0)
                                        {

                                            // Запись данных В БД
                                            SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                            StrPrc2.CommandType = CommandType.StoredProcedure;
                                            StrPrc2.Parameters.AddWithValue("@User_Login", (panel1.Controls["textbox13"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@User_Password", (panel1.Controls["textbox14"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@User_Mail", (panel1.Controls["textbox15"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@Teacher_id", PrepodID);
                                            StrPrc2.ExecuteNonQuery();

                                            // Выбор данных в таблице БД
                                            SqlCommand GetUniqueNaim = new SqlCommand("select Unique_Naim as 'UniqueNaim' from Teachers where id_teacher = '" + PrepodID + "'", con);
                                            SqlDataReader dr6 = GetUniqueNaim.ExecuteReader();
                                            dr6.Read();
                                            string UniqueNaim = dr6["UniqueNaim"].ToString();
                                            dr6.Close();

                                            // Изменение количества возможных студентов у преподавателя
                                            SqlCommand StrPrc4 = new SqlCommand("Teachers_update", con);
                                            StrPrc4.CommandType = CommandType.StoredProcedure;
                                            StrPrc4.Parameters.AddWithValue("@id_teacher", PrepodID);
                                            StrPrc4.Parameters.AddWithValue("@Unique_Naim", UniqueNaim);
                                            StrPrc4.Parameters.AddWithValue("@KolvoNeRegStudents", studentkolvo - 1);
                                            StrPrc4.ExecuteNonQuery();

                                            // Выбор номера добавленного пользователя
                                            SqlCommand GetUserID = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel1.Controls["textbox13"] as TextBox).Text + "'", con);
                                            SqlDataReader dr7 = GetUserID.ExecuteReader();
                                            dr7.Read();
                                            int UserId = Convert.ToInt32(dr7["id"].ToString());
                                            dr7.Close();

                                            // Запись данных В БД
                                            SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                            StrPrc3.CommandType = CommandType.StoredProcedure;
                                            StrPrc3.Parameters.AddWithValue("@Naim", "Student");
                                            StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                            StrPrc3.Parameters.AddWithValue("@Dostup_id", 3);
                                            StrPrc3.ExecuteNonQuery();

                                            MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            OpenMainForm();
                                        }
                                        else
                                        {
                                            CreateInfo("Превышено максимально возможное число студентов у преподавателя!");
                                        }
                                    }
                                    else
                                    {
                                        CreateInfo("Преподавателя с таким логином не существует!");
                                    }
                                }
                                else
                                {
                                    CreateInfo("Пользователь с таким логином уже существует!");
                                }
                            }
                            else
                            {
                                CreateInfo("Преподавателя с таким логином не существует!");
                            }
                        }
                        else
                        {
                            CreateInfo("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.");
                        }
                    }
                    else
                    {
                        CreateInfo("Вы не ввели логин преподавателя, без него регистрация невозможна!");
                    }
                    break;

                // Ничего не выбрано
                default:
                    CreateInfo("Для добавления пользователя необходимо выбрать его статус и ввести для него данные!");
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
                                    break;
                                case 2:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 157);
                                    break;
                                case 3:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 197);
                                    break;
                                case 4:
                                    textbox.Size = new Size(250, 40);
                                    textbox.Location = new Point(404, 237);
                                    textbox.MaxLength = 30;
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
                                    break;
                                case 7:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 157);
                                    break;
                                case 8:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 197);
                                    break;
                                case 9:
                                    textbox.Size = new Size(50, 40);
                                    textbox.Location = new Point(404, 237);
                                    textbox.MaxLength = 3;
                                    textbox.KeyPress += NumberCheck;
                                    break;
                                case 10:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 277);
                                    textbox.MaxLength = 30;
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
                                    textbox.ShortcutsEnabled = false;
                                    break;
                                case 13:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 157);
                                    break;
                                case 14:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 197);
                                    break;
                                case 15:
                                    textbox.Size = new Size(230, 40);
                                    textbox.Location = new Point(404, 237);
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

        private void NumberCheck(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void CleanForm()
        {
            // Очистка формы
            switch (UserSelectedStatus)
            {
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
            administrator administrator = new administrator();
            administrator.Show();
            Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void CreateInfo(string labelinfo)
        {
            Timer timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += TimerTick;
            timer.Start();

            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600,100);
            panel.Location = new Point(panel1.Width/2-panel.Width/2,panel1.Height/2-panel.Height/2);
            panel.BringToFront();
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel);

            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width,panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.ForeColor = Color.Red;
            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
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

        private void FormLoad(object sender, EventArgs e)
        {
            FormAlignment();

            Label label = new Label();
            label.Text = "Статус пользователя:";
            label.Location = new Point(67,69);
            label.Font = new Font(label.Font.Name, 16);
            label.AutoSize = true;
            panel1.Controls.Add(label);
        }
    }
}
