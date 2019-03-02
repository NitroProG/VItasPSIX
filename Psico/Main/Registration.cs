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
        string KeyCheck;
        string LoginCheck;
        DataGridView datagr = new DataGridView();

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
                            textbox.PasswordChar = '*';
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
                for (int i = 4; i < 8; i++)
                {
                    // Создание label
                    Label label = new Label();
                    label.Name = "label" + i.ToString();
                    switch (i)
                    {
                        case 4:
                            label.Text = "Ключ регистрации:";
                            label.Location = new Point(42, 60);
                            break;
                        case 5:
                            label.Text = "Логин преподавателя:";
                            label.Location = new Point(8, 120);
                            break;
                        case 6:
                            label.Text = "Почта преподавателя:";
                            label.Location = new Point(8, 180);
                            break;
                        case 7:
                            label.Text = "Количество студентов:";
                            label.Location = new Point(2, 240);
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
                            textbox.Location = new Point(250, 57);
                            textbox.PasswordChar = '*';
                            textbox.ShortcutsEnabled = false;
                            break;
                        case 5:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 117);
                            break;
                        case 6:
                            textbox.Size = new Size(230, 40);
                            textbox.Location = new Point(250, 177);
                            break;
                        case 7:
                            textbox.Size = new Size(50, 40);
                            textbox.Location = new Point(250, 237);
                            textbox.MaxLength = 3;
                            textbox.KeyPress += NumberCheck;
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
                        try
                        {
                            // Динамическое создание таблицы
                            datagr.Name = "datagrview";
                            datagr.Location = new Point(0, 0);
                            datagr.Size = new Size(300,300);
                            SqlDataAdapter da1 = new SqlDataAdapter("select * from users where User_Login = '" + (panel3.Controls["textbox1"] as TextBox).Text + "'", con);
                            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                            DataSet ds1 = new DataSet();
                            da1.Fill(ds1, "users");
                            datagr.DataSource = ds1.Tables[0];
                            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                            panel2.Controls.Add(datagr);
                            datagr.Visible = false;

                            if (Convert.ToInt32(datagr.Rows[0].Cells[4].Value) != 0)
                            {
                                if ((panel3.Controls["textbox2"] as TextBox).Text != "" && (panel3.Controls["textbox3"] as TextBox).Text != "")
                                {
                                    //Выбор данных из БД
                                    SqlCommand CheckStudentLogin = new SqlCommand("select Student_Mail from students where Student_Login='"+(panel3.Controls["textbox2"].Text)+"'", con);
                                    SqlDataReader dr = CheckStudentLogin.ExecuteReader();

                                    try
                                    {
                                        // Запись данных из БД
                                        dr.Read();
                                        LoginCheck = dr["Student_Mail"].ToString();
                                        dr.Close();
                                    }
                                    catch
                                    {
                                        LoginCheck = "";
                                        dr.Close();
                                    }

                                    if (LoginCheck =="")
                                    {
                                        // изменение данных в БД
                                        SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                                        StrPrc1.CommandType = CommandType.StoredProcedure;
                                        StrPrc1.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr.Rows[0].Cells[0].Value));
                                        StrPrc1.Parameters.AddWithValue("@User_Login", datagr.Rows[0].Cells[1].Value.ToString());
                                        StrPrc1.Parameters.AddWithValue("@User_Password", datagr.Rows[0].Cells[2].Value.ToString());
                                        StrPrc1.Parameters.AddWithValue("@User_Mail", datagr.Rows[0].Cells[3].Value.ToString());
                                        StrPrc1.Parameters.AddWithValue("@Kolvo_students", Convert.ToInt32(datagr.Rows[0].Cells[4].Value) - 1);
                                        StrPrc1.Parameters.AddWithValue("@isadmin", Convert.ToInt32(datagr.Rows[0].Cells[5].Value));
                                        StrPrc1.ExecuteNonQuery();

                                        string password = GetPassword();

                                        try
                                        {
                                            // Отправка пароля по почте
                                            MailMessage mail = new MailMessage("ProgrammPsicotest", (panel3.Controls["textbox3"] as TextBox).Text, "Пароль для программы Psico", password);
                                            SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                            client.Port = 587;
                                            client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                            client.EnableSsl = true;
                                            client.Send(mail);

                                            // Запись данных В БД
                                            SqlCommand StrPrc2 = new SqlCommand("Students_add", con);
                                            StrPrc2.CommandType = CommandType.StoredProcedure;
                                            StrPrc2.Parameters.AddWithValue("@Student_Login", (panel3.Controls["textbox2"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@Student_Password", password);
                                            StrPrc2.Parameters.AddWithValue("@Student_Mail", (panel3.Controls["textbox3"] as TextBox).Text);
                                            StrPrc2.Parameters.AddWithValue("@users_id", Convert.ToInt32(datagr.Rows[0].Cells[0].Value));
                                            StrPrc2.ExecuteNonQuery();

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
                                    MessageBox.Show("Заполнены не все поля для успешной регистрации! Пожалуйста заполните все необходимые поля для регистрации.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Превышено максимально возможное число студентов у преподавателя!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Введённый логин преподавателя не существует! Убедитесь в правильности ввода логина и попытайтесь снова.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            (panel3.Controls["textbox1"] as TextBox).Text = "";
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
                                SqlCommand CheckUserLogin = new SqlCommand("select User_Mail from users where User_Login='" + (panel3.Controls["textbox5"].Text) + "'", con);
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
                                    string password = GetPassword();

                                    // Регистрация
                                    try
                                    {
                                        // Отправка пароля по почте
                                        MailMessage mail = new MailMessage("ProgrammPsicotest", (panel3.Controls["textbox6"] as TextBox).Text, "Пароль для программы Psico", password);
                                        SmtpClient client = new SmtpClient("smtp.yandex.ru");
                                        client.Port = 587;
                                        client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                                        client.EnableSsl = true;
                                        client.Send(mail);

                                        // Запись данных В БД
                                        SqlCommand StrPrc1 = new SqlCommand("users_add", con);
                                        StrPrc1.CommandType = CommandType.StoredProcedure;
                                        StrPrc1.Parameters.AddWithValue("@User_Login", (panel3.Controls["textbox5"] as TextBox).Text);
                                        StrPrc1.Parameters.AddWithValue("@User_Password", password);
                                        StrPrc1.Parameters.AddWithValue("@User_Mail", (panel3.Controls["textbox6"] as TextBox).Text);
                                        StrPrc1.Parameters.AddWithValue("@Kolvo_students", (panel3.Controls["textbox7"] as TextBox).Text);
                                        StrPrc1.Parameters.AddWithValue("@isadmin", 0);
                                        StrPrc1.ExecuteNonQuery();

                                        // Генерация нового ключа активации
                                        string key = GetKey();

                                        // Изменение ключа активации программы
                                        SqlCommand StrPrc2 = new SqlCommand("defend_update", con);
                                        StrPrc2.CommandType = CommandType.StoredProcedure;
                                        StrPrc2.Parameters.AddWithValue("@id_defend", 1);
                                        StrPrc2.Parameters.AddWithValue("@DefenderKey", key);
                                        StrPrc2.ExecuteNonQuery();

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
                        (panel3.Controls["textbox4"] as TextBox).Visible = false;
                        (panel3.Controls["textbox5"] as TextBox).Visible = false;
                        (panel3.Controls["textbox6"] as TextBox).Visible = false;
                        (panel3.Controls["textbox7"] as TextBox).Visible = false;

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
                            (panel3.Controls["textbox4"] as TextBox).Visible = true;
                            (panel3.Controls["textbox5"] as TextBox).Visible = true;
                            (panel3.Controls["textbox6"] as TextBox).Visible = true;
                            (panel3.Controls["textbox7"] as TextBox).Visible = true;
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
