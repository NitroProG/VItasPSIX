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
    public partial class AddZadacha : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int AddStage;
        string SelectedForm;
        string zapros = "";
        string sved = "";
        string meropr = "";

        public AddZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        private void AddZadachaa(object sender, EventArgs e)
        {
            con.Open();

            switch (AddStage)
            {
                // Феноменология сведения
                case 0:
                    if (richTextBox1.Text != "" && richTextBox2.Text != "")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("Zadacha_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@Zapros", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@Sved", richTextBox2.Text);
                        StrPrc1.ExecuteNonQuery();

                        zapros = richTextBox1.Text;
                        sved = richTextBox2.Text;

                        // Выбор количества данных в таблице БД
                        SqlCommand GetZadachaid = new SqlCommand("select id_zadacha as 'id' from Zadacha where Zapros='" + richTextBox1.Text + "' and Sved ='"+richTextBox2.Text+"'", con);
                        SqlDataReader dr1 = GetZadachaid.ExecuteReader();
                        dr1.Read();
                        Program.NomerZadachi = Convert.ToInt32(dr1["id"].ToString());
                        dr1.Close();

                        CreateInfo("Данные успешно добавлены!","lime", panel1);

                        // Отрисовка формы
                        button1.Visible = true;
                        label2.Text = "Сведения от:";
                        label3.Text = "Описание сведений:";
                        label4.Text = "Этап Феноменология (Свободная форма)";
                        richTextBox1.Text = "";
                        richTextBox1.MaxLength = 100;
                        richTextBox2.Text = "";
                        richTextBox2.MaxLength = 2147483647;
                        ++AddStage;
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить все поля для добавления!", "red", panel1);
                    }
                    break;

                // Феноменология ответы
                case 1:
                    DialogResult result1 = MessageBox.Show("Вы уверены что хотите завершить работу с добавлением сведений на этапе феноменология (Свободная форма)?", "Внимание!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    // Если пользователь нажал ОК
                    if (result1 == DialogResult.OK)
                    {
                        // Отрисовка формы
                        label2.Text = "Вариант ответа:";
                        label3.Visible = false;
                        label4.Text = "Этап Феноменология (машинный выбор)";
                        richTextBox1.Text = "";
                        richTextBox1.MaxLength = 2147483647;
                        richTextBox2.Visible = false;
                        ++AddStage;
                    }
                    break;

                // Гипотезы
                case 2:
                    DialogResult result2 = MessageBox.Show("Вы уверены что хотите завершить работу с добавлением вариантов ответа на этапе феноменология (Машинный выбор)?", "Внимание!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    // Если пользователь нажал ОК
                    if (result2 == DialogResult.OK)
                    {
                        // Отрисовка формы
                        label2.Text = "Вариант ответа:";
                        label4.Text = "Этап Гипотезы (машинный выбор)";
                        richTextBox1.Text = "";
                        ++AddStage;
                    }
                    break;

                // Обследования
                case 3:
                    DialogResult result3 = MessageBox.Show("Вы уверены что хотите завершить работу с добавлением вариантов ответа на этапе гипотезы (Машинный выбор)?", "Внимание!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    // Если пользователь нажал ОК
                    if (result3 == DialogResult.OK)
                    {
                        // Отрисовка формы
                        label2.Text = "Короткое название обследования";
                        label2.Width = panel1.Width / 2 - 30;
                        label2.AutoSize = false;
                        label2.Location = new Point(panel1.Location.X, label2.Location.Y - 40);

                        label3.Text = "* Полное название обследования";
                        label3.Visible = true;
                        label3.Location = new Point(panel1.Location.X, label3.Location.Y - 120);

                        label5.Text = "* Данные по обследованию";
                        label5.Visible = true;
                        label5.Location = new Point(label2.Location.X + label2.Size.Width + 40, label2.Location.Y);

                        label6.Text = "Путь к файлу с первой картинкой";
                        label6.Visible = true;
                        label6.Location = new Point(panel1.Location.X, label3.Location.Y + 100);

                        label7.Text = "Путь к файлу со второй картинкой";
                        label7.Visible = true;
                        label7.Location = new Point(panel1.Location.X, label6.Location.Y + 100);

                        label4.Text = "Этап Обследования";

                        richTextBox1.Text = "";
                        richTextBox1.Size = new Size(label2.Width, label2.Height + 10);
                        richTextBox1.Location = new Point(label2.Location.X, label2.Location.Y + 30);
                        richTextBox1.MaxLength = 100;

                        richTextBox2.Text = "";
                        richTextBox2.Visible = true;
                        richTextBox2.Size = new Size(richTextBox1.Size.Width, 40);
                        richTextBox2.Location = new Point(label3.Location.X, label3.Location.Y + 30);
                        richTextBox2.MaxLength = 300;

                        richTextBox3.Visible = true;
                        richTextBox3.Size = new Size(panel2.Width / 3 + 100, panel1.Height / 2);
                        richTextBox3.Location = new Point(label5.Location.X, label5.Location.Y + 30);
                        richTextBox3.MaxLength = 2147483647;

                        richTextBox4.Visible = true;
                        richTextBox4.Size = new Size(richTextBox1.Size.Width, 40);
                        richTextBox4.Location = new Point(label3.Location.X, label6.Location.Y + 30);
                        richTextBox4.MaxLength = 2147483647;

                        richTextBox5.Visible = true;
                        richTextBox5.Size = new Size(richTextBox1.Size.Width, 40);
                        richTextBox5.Location = new Point(label3.Location.X, label7.Location.Y + 30);
                        richTextBox5.MaxLength = 2147483647;

                        ++AddStage;
                    }
                    break;

                // Диагноз
                case 4:
                    DialogResult result4 = MessageBox.Show("Вы уверены что хотите завершить работу с добавлением обследований?", "Внимание!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    // Если пользователь нажал ОК
                    if (result4 == DialogResult.OK)
                    {
                        // Отрисовка формы
                        label2.Text = "Вариант ответа:";
                        label4.Text = "Этап Диагноз (машинный выбор)";

                        richTextBox1.Text = "";
                        richTextBox1.Size = new Size(551, 86);
                        richTextBox1.Location = new Point(panel1.Width / 2 - richTextBox1.Width / 2, panel1.Height / 3 - richTextBox1.Height / 2);
                        richTextBox1.MaxLength = 2147483647;

                        label2.Location = new Point(richTextBox1.Location.X, panel1.Height / 4 - label2.Height / 2);

                        label3.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;

                        richTextBox2.Visible = false;
                        richTextBox3.Visible = false;
                        richTextBox4.Visible = false;
                        richTextBox5.Visible = false;

                        ++AddStage;
                    }
                    break;

                // Мероприятия
                case 5:
                    DialogResult result5 = MessageBox.Show("Вы уверены что хотите завершить работу с добавлением вариантов ответа на этапе диагноз (Машинный выбор)?", "Внимание!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    // Если пользователь нажал ОК
                    if (result5 == DialogResult.OK)
                    {
                        // Отрисовка формы
                        label2.Text = "Проведённые мероприятия:";
                        label4.Text = "Этап Мероприятия (Общие сведения)";

                        button1.Visible = false;

                        richTextBox1.Text = "";
                        richTextBox1.Size = new Size(richTextBox1.Width, panel1.Height / 2);
                        richTextBox1.Location = new Point(panel1.Width / 2 - richTextBox1.Width / 2, panel1.Height / 2 - richTextBox1.Height / 2);

                        label2.Location = new Point(richTextBox1.Location.X, richTextBox1.Location.Y - 35);

                        ++AddStage;
                    }
                    break;

                // Катамнез
                case 6:
                    if (richTextBox1.Text !="")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("Zadacha_update", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@id_zadacha", Program.NomerZadachi);
                        StrPrc1.Parameters.AddWithValue("@Zapros", zapros);
                        StrPrc1.Parameters.AddWithValue("@sved", sved);
                        StrPrc1.Parameters.AddWithValue("@meroprtext", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@katamneztext", "");
                        StrPrc1.ExecuteNonQuery();

                        meropr = richTextBox1.Text;

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);

                        // Отрисовка формы
                        label2.Text = "Катамнез:";
                        label4.Text = "Этап Катамнез";
                        richTextBox1.Text = "";

                        ++AddStage;
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;

                // Верные ответы
                case 7:
                    if (richTextBox1.Text !="")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("Zadacha_update", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@id_zadacha", Program.NomerZadachi);
                        StrPrc1.Parameters.AddWithValue("@Zapros", zapros);
                        StrPrc1.Parameters.AddWithValue("@sved", sved);
                        StrPrc1.Parameters.AddWithValue("@meroprtext", meropr);
                        StrPrc1.Parameters.AddWithValue("@katamneztext", richTextBox1.Text);
                        StrPrc1.ExecuteNonQuery();

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);

                        // Отрисовка формы
                        label2.Text = "Этап, для которого необходимо выбрать верный ответ:";
                        label2.AutoSize = true;
                        label2.Font = new Font(label2.Font.FontFamily, 14);
                        label2.Location = new Point(panel1.Width / 2 - label2.Width / 2 - comboBox1.Width / 2, panel1.Height / 2 - label2.Height);

                        label3.Text = "Верный ответ:";
                        label3.Font = new Font(label3.Font.FontFamily, 14);
                        label3.Visible = true;
                        label3.Location = new Point(label2.Location.X, label2.Location.Y + 50);

                        label4.Text = "Этап Верные ответы";

                        richTextBox1.Visible = false;

                        button1.Visible = true;

                        button5.Text = "Добавить задачу";

                        comboBox1.Visible = true;
                        comboBox1.Location = new Point(label2.Location.X + label2.Size.Width, label2.Location.Y);

                        comboBox2.Visible = true;
                        comboBox2.Location = new Point(label3.Location.X + label3.Size.Width, label3.Location.Y);
                        comboBox2.Size = new Size(label2.Width + comboBox1.Width - label3.Width, comboBox2.Height);

                        ++AddStage;
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;

                //Выход
                case 8:
                    MessageBox.Show("Задача успешно добавлена!","Отлично!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    administrator admin = new administrator();
                    admin.Show();
                    Close();
                    break;
            }

            con.Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            AddStage = 0;

            label4.Text = "Основные данные о задаче";

            FormAlignment();
        }

        private void AddPunkt(object sender, EventArgs e)
        {
            con.Open();

            switch (AddStage)
            {
                // Добавление сведений на этапе Феноменология
                case 1:
                    if (richTextBox1.Text != "" && richTextBox2.Text !="")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("Fenom1_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@rb", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@rbtext", richTextBox2.Text);
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();

                        // Обновление формы
                        richTextBox1.Text = "";
                        richTextBox2.Text = "";

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить все поля для успешного добавления!", "red", panel1);
                    }
                    break;

                // Добавление ответов на этапе феноменология
                case 2:
                    if (richTextBox1.Text != "")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("CBFormFill_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@FormCB", "Fenom");
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();
                        
                        // Обновление формы
                        richTextBox1.Text = "";

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        CreateInfo("необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;

                // Добавление ответов на этапе гипотезы
                case 3:
                    if (richTextBox1.Text != "")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("CBFormFill_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@FormCB", "Teor");
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();

                        // Обновление формы
                        richTextBox1.Text = "";

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;

                // Добавление обследований
                case 4:
                    if (richTextBox2.Text !="" && richTextBox3.Text !="")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("dpo_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@lb_small", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@lb", richTextBox2.Text);
                        StrPrc1.Parameters.AddWithValue("@lbtext", richTextBox3.Text);
                        StrPrc1.Parameters.AddWithValue("@lb_image", richTextBox4.Text);
                        StrPrc1.Parameters.AddWithValue("@lb_image2", richTextBox5.Text);
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();

                        // Обновление формы
                        richTextBox1.Text = "";
                        richTextBox2.Text = "";
                        richTextBox3.Text = "";
                        richTextBox4.Text = "";
                        richTextBox5.Text = "";

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить как минимум поля со знаком * для успешного добавления!", "red", panel1);
                    }
                    break;

                // Добавление ответов на этапе Диагноз
                case 5:
                    if (richTextBox1.Text != "")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("CBFormFill_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@FormCB", "Diag");
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();

                        // Обновление формы
                        richTextBox1.Text = "";

                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;

                case 6:
                    // Мероприятия
                    break;

                case 7:
                    // Катамнез
                    break;

                // Добавление верных ответов на всех этапах где нужно выбрать верные ответы
                case 8:
                    if (Convert.ToString(comboBox2.SelectedValue) != "")
                    {
                        // Запись данных В БД
                        SqlCommand StrPrc1 = new SqlCommand("vernotv_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@otv", Convert.ToString(comboBox2.SelectedValue));
                        StrPrc1.Parameters.AddWithValue("@FormVernOtv", SelectedForm);
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", Program.NomerZadachi);
                        StrPrc1.ExecuteNonQuery();

                        CreateInfo("Данные успешно добавлены!","lime", panel1);
                    }
                    else
                    {
                        CreateInfo("Необходимо заполнить поле для успешного добавления!", "red", panel1);
                    }
                    break;
            }

            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();

            switch (Convert.ToString(comboBox1.SelectedItem))
            {
                case "Феноменология":
                    SelectedForm = "Fenom";
                    break;

                case "Гипотезы":
                    SelectedForm = "Teor";
                    break;

                case "Диагноз":
                    SelectedForm = "Diag";
                    break;

                default:
                    CreateInfo("Необходимо выбрать этап, для которого хотите выбрать верный ответ!", "red", panel1);
                    break;
            }

            if (SelectedForm != "")
            {
                // Создание списка вариантов ответа 
                SqlCommand GetOtv = new SqlCommand("select CB as \"ido\" from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = '"+SelectedForm+"'", con);
                SqlDataReader dr = GetOtv.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                comboBox2.DataSource = dt;
                comboBox2.ValueMember = "ido";
            }

            con.Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
                panel1.Width = 1000;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label4.Width = panel1.Width;
            label4.TextAlign = ContentAlignment.MiddleCenter;
            label4.Location = new Point(panel1.Width / 2 - label4.Width / 2, label1.Top + 50);
            button4.Location = new Point(30, panel1.Height - 70);
            button5.Location = new Point(panel1.Width-button5.Width - 30, panel1.Height - 70);
            button1.Location = new Point(button5.Location.X, button5.Location.Y - 50);
            button1.Width = button5.Width;
            richTextBox1.Location = new Point(panel1.Width / 2 - richTextBox1.Width/2, panel1.Height / 3 - richTextBox1.Height / 2);
            label2.Location = new Point(richTextBox1.Location.X, panel1.Height / 4 - label2.Height / 2);
            richTextBox2.Location = new Point(panel1.Width / 2 - richTextBox2.Width/2, panel1.Height / 2 - richTextBox2.Height / 5);
            label3.Location = new Point(richTextBox2.Location.X, richTextBox2.Location.Y - 30);
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
            catch { }
        }
    }
}
