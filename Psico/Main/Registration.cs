﻿using System;
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
    public partial class Registration : Form
    {
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

        private void Registration_Load(object sender, EventArgs e)
        {
            Strela = 0;
            Registr = 0;
            student = 0;
            teacher = 0;

            // Создание label
            Label label = new Label();
            label.Name = "label";
            label.Text = "Выберите способ регистрации!";
            label.AutoSize = true;
            label.Location = new Point(100,130);
            label.Font = new Font(label.Font.Name,16);
            panel3.Controls.Add(label);
        }

        private void DrawStrela(object sender, PaintEventArgs e)
        {
            // Рисование стрелок
            if (Strela == 0)
            {
                Pen pen = new Pen(Color.DarkCyan, 6);
                pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                e.Graphics.DrawLine(pen, 125, 20, 250, 120);
                e.Graphics.DrawLine(pen, 375, 20, 250, 120);
            }
        }

        private void GetStudentInfoRegistration(object sender, EventArgs e)
        {
            Registr = 1;

            GetColorLabel();

            GetClearPanel1();

            if (student == 0)
            {
                // Заполнение Panel1
                for (int i = 1; i < 4; i++)
                {
                    // Создание label
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

                    // Создание textbox
                    TextBox textbox = new TextBox();
                    textbox.Name = "textbox" + i.ToString();
                    switch (i)
                    {
                        case 1:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 57);
                            //textbox.PasswordChar = '*';
                            textbox.ShortcutsEnabled = false;
                            break;
                        case 2:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 117);
                            break;
                        case 3:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 177);
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
            Registr = 2;

            GetColorLabel();

            GetClearPanel1();

            if (teacher == 0)
            {
                // Заполнение Panel1
                for (int i = 4; i < 9; i++)
                {
                    // Создание label
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

                    // Создание textbox
                    TextBox textbox = new TextBox();
                    textbox.Name = "textbox" + i.ToString();
                    switch (i)
                    {
                        case 4:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 17);
                            //textbox.PasswordChar = '*';
                            textbox.ShortcutsEnabled = false;
                            break;
                        case 5:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 67);
                            break;
                        case 6:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 117);
                            break;
                        case 7:
                            textbox.Size = new Size(50, 40);
                            textbox.Location = new Point(250, 167);
                            textbox.MaxLength = 3;
                            textbox.KeyPress += NumberCheck;
                            break;
                        case 8:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 217);
                            break;
                    }
                    textbox.Font = new Font(textbox.Font.Name, 16);
                    panel3.Controls.Add(textbox);
                }
            }

            teacher = 1;
        }

        private void NumberCheck(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            OpenAutorizationForm();
        }

        private void GetRegistretion(object sender, EventArgs e)
        {
            int TeacherId = 0;

            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open();

            // Проверка выбранной формы регистрации
            switch (Registr)
            {
                // Регистрация студента
                case 1:
                    if ((panel3.Controls["textbox1"] as TextBox).Text != "")
                    {
                        // Проверка введённых данных
                        if ((panel3.Controls["textbox2"] as TextBox).Text != "" && (panel3.Controls["textbox3"] as TextBox).Text != "")
                        {
                            // Выбор количества данных в таблице БД
                            SqlCommand GetTeacherId = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox1"] as TextBox).Text + "'", con);
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
                                //Выбор данных из БД
                                SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel3.Controls["textbox2"].Text) + "'", con);
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
                                    // Выбор количества данных в таблице БД
                                    SqlCommand CheckRoleName = new SqlCommand("select Naim as 'RoleName' from Role where users_id = '" + TeacherId + "'", con);
                                    SqlDataReader dr3 = CheckRoleName.ExecuteReader();
                                    dr3.Read();
                                    string CheckRole = dr3["RoleName"].ToString();
                                    dr3.Close();

                                    if (CheckRole == "Teacher")
                                    {
                                        // Выбор количества данных в таблице БД
                                        SqlCommand GetPrepodId = new SqlCommand("select Teacher_id as 'ID' from users where id_user = '" + TeacherId + "'", con);
                                        SqlDataReader dr4 = GetPrepodId.ExecuteReader();
                                        dr4.Read();
                                        int PrepodID = Convert.ToInt32(dr4["ID"].ToString());
                                        dr4.Close();

                                        // Выбор количества данных в таблице БД
                                        SqlCommand GetKolvoStudent = new SqlCommand("select KolvoNeRegStudents as 'kolvostudents' from Teachers where id_teacher = '" + PrepodID + "'", con);
                                        SqlDataReader dr5 = GetKolvoStudent.ExecuteReader();
                                        dr5.Read();
                                        int studentkolvo = Convert.ToInt32(dr5["kolvostudents"].ToString());
                                        dr5.Close();

                                        if (studentkolvo > 0)
                                        {
                                            string password = GetPassword();

                                            // Отправка пароля по почте
                                            MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", (panel3.Controls["textbox3"] as TextBox).Text, "Пароль для программы Psico", password);
                                            SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                            client.Port = 587;
                                            client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                            client.EnableSsl = true;
                                            client.Send(mail);

                                            // Запись данных В БД
                                            SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                            StrPrc2.CommandType = CommandType.StoredProcedure;
                                            StrPrc2.Parameters.AddWithValue("@User_Login", (panel3.Controls["textbox2"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@User_Password", password);
                                            StrPrc2.Parameters.AddWithValue("@User_Mail", (panel3.Controls["textbox3"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@Teacher_id", PrepodID);
                                            StrPrc2.ExecuteNonQuery();

                                            // Выбор данных в таблице БД
                                            SqlCommand GetUniqueNaim = new SqlCommand("select Unique_Naim as 'UniqueNaim' from Teachers where id_teacher = '" + PrepodID + "'", con);
                                            SqlDataReader dr6 = GetUniqueNaim.ExecuteReader();
                                            dr6.Read();
                                            string UniqueNaim = dr6["UniqueNaim"].ToString();
                                            dr6.Close();

                                            // Изменение ключа активации программы
                                            SqlCommand StrPrc4 = new SqlCommand("Teachers_update", con);
                                            StrPrc4.CommandType = CommandType.StoredProcedure;
                                            StrPrc4.Parameters.AddWithValue("@id_teacher", PrepodID);
                                            StrPrc4.Parameters.AddWithValue("@Unique_Naim", UniqueNaim);
                                            StrPrc4.Parameters.AddWithValue("@KolvoNeRegStudents", studentkolvo - 1);
                                            StrPrc4.ExecuteNonQuery();

                                            // Выбор количества данных в таблице БД
                                            SqlCommand GetUserID = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox2"] as TextBox).Text + "'", con);
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

                                            OpenAutorizationForm();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Превышено максимально возможное число студентов у преподавателя!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Преподавателя с таким логином не существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Преподавателя с таким логином не существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }                       
                        }
                        else
                        {
                            MessageBox.Show("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Вы не ввели логин преподавателя, без него регистрация невозможна!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                break;
                
                // Регистрация преподавателя
                case 2:
                    if ((panel3.Controls["textbox4"] as TextBox).Text != "")
                    {
                        //Выбор данных из БД
                        SqlCommand GetKeyDefender = new SqlCommand("select DefenderKey from defender", con);
                        SqlDataReader dr = GetKeyDefender.ExecuteReader();

                        // Запись данных из БД
                        dr.Read();
                        KeyCheck = dr["DefenderKey"].ToString();
                        dr.Close();

                        // Проверка правильности ключа активации
                        if ((panel3.Controls["textbox4"] as TextBox).Text == KeyCheck)
                        {
                            // Проверка введённых данных
                            if ((panel3.Controls["textbox5"] as TextBox).Text != "" && (panel3.Controls["textbox6"] as TextBox).Text != "" && (panel3.Controls["textbox7"] as TextBox).Text != "")
                            {
                                //Выбор данных из БД
                                SqlCommand CheckUniqueName = new SqlCommand("select id_teacher from Teachers where Unique_Naim='" + (panel3.Controls["textbox8"].Text) + "'", con);
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

                                if (checkteacherUniqueNaim ==0)
                                {
                                    //Выбор данных из БД
                                    SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel3.Controls["textbox5"].Text) + "'", con);
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
                                        string password = GetPassword();

                                        // Регистрация
                                        try
                                        {
                                            // Отправка пароля по почте
                                            MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", (panel3.Controls["textbox6"] as TextBox).Text, "Пароль для программы Psico", password);
                                            SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                            client.Port = 587;
                                            client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                            client.EnableSsl = true;
                                            client.Send(mail);

                                            // Запись данных В БД
                                            SqlCommand StrPrc1 = new SqlCommand("Teachers_add", con);
                                            StrPrc1.CommandType = CommandType.StoredProcedure;
                                            StrPrc1.Parameters.AddWithValue("@Unique_Naim", (panel3.Controls["textbox8"] as TextBox).Text);
                                            StrPrc1.Parameters.AddWithValue("@KolvoNeRegStudents", Convert.ToInt32((panel3.Controls["textbox7"] as TextBox).Text));
                                            StrPrc1.ExecuteNonQuery();

                                            // Выбор количества данных в таблице БД
                                            SqlCommand GetTeacherId = new SqlCommand("select id_teacher as 'id' from Teachers where Unique_Naim='" + (panel3.Controls["textbox8"] as TextBox).Text + "'", con);
                                            SqlDataReader dr4 = GetTeacherId.ExecuteReader();
                                            dr4.Read();
                                            TeacherId = Convert.ToInt32(dr4["id"].ToString());
                                            dr4.Close();

                                            // Запись данных В БД
                                            SqlCommand StrPrc2 = new SqlCommand("users_add", con);
                                            StrPrc2.CommandType = CommandType.StoredProcedure;
                                            StrPrc2.Parameters.AddWithValue("@User_Login", (panel3.Controls["textbox5"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@User_Password", password);
                                            StrPrc2.Parameters.AddWithValue("@User_Mail", (panel3.Controls["textbox6"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@Teacher_id", TeacherId);
                                            StrPrc2.ExecuteNonQuery();

                                            // Выбор количества данных в таблице БД
                                            SqlCommand GetUserID = new SqlCommand("select id_user as 'id' from users where User_Login = '" + (panel3.Controls["textbox5"] as TextBox).Text + "'", con);
                                            SqlDataReader dr3 = GetUserID.ExecuteReader();
                                            dr3.Read();
                                            int UserId = Convert.ToInt32(dr3["id"].ToString());
                                            dr3.Close();

                                            // Запись данных В БД
                                            SqlCommand StrPrc3 = new SqlCommand("Role_add", con);
                                            StrPrc3.CommandType = CommandType.StoredProcedure;
                                            StrPrc3.Parameters.AddWithValue("@Naim", "Teacher");
                                            StrPrc3.Parameters.AddWithValue("@Users_id", UserId);
                                            StrPrc3.Parameters.AddWithValue("@Dostup_id", 2);
                                            StrPrc3.ExecuteNonQuery();

                                            // Генерация нового ключа активации
                                            string key = GetKey();

                                            // Изменение ключа активации программы
                                            SqlCommand StrPrc4 = new SqlCommand("defend_update", con);
                                            StrPrc4.CommandType = CommandType.StoredProcedure;
                                            StrPrc4.Parameters.AddWithValue("@id_defend", 1);
                                            StrPrc4.Parameters.AddWithValue("@DefenderKey", key);
                                            StrPrc4.ExecuteNonQuery();

                                            MessageBox.Show("Вы успешно зарегистрировались", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            OpenAutorizationForm();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("Сообщение с вашим паролем не было отправлено!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Введённое уникальное имя уже существует, выберите другое.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Введённый ключ активации неверен! Пожалуйста убедитесь в правильности введённого ключа активации и попробуйде ещё раз.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            (panel3.Controls["textbox4"] as TextBox).Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ключ активации не был введён, пожалуйста введите ключ активации для регистрации!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                break;
                
                // Если пользователь не выбрал форму регистрации
                default:
                    MessageBox.Show("Выберите способ регистрации!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                break;
            }

            //OpenAutorizationForm();
        }

        public static string GetKey(int x=16)
        {
            // Создание ключа активации
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
            //Очищаем Panel1
            switch (Registr)
            {
                // Студент
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
                
                // Учитель
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
    }
}
