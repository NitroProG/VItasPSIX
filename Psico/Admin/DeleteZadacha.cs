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
    public partial class DeleteZadacha : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int Delete = 0;
        int kolvoPopitok = 0;
        string Key = "";

        public DeleteZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        public static string GetKey(int x = 8)
        {
            // Генерация ключа для удаления задачи
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

        private void DeleteZadachaa(object sender, EventArgs e)
        {
            con.Open();

            switch (Delete)
            {
                case 0:

                    CreateInfo("На почту главного администратора выслан ключ для удаления задачи, вам необходимо его указать и ещё раз нажать на кнопку удаления", "lime", panel1);

                    //Выбор данных из БД
                    SqlCommand GetMainAdminMail = new SqlCommand("select User_Mail from users where id_user = 1", con);
                    SqlDataReader dr1 = GetMainAdminMail.ExecuteReader();
                    dr1.Read();
                    string MainAdminMail = dr1["User_Mail"].ToString();
                    dr1.Close();

                    // Генерация ключа для удаления задачи
                    Key = GetKey();

                    try
                    {
                        // Отправка пароля по почте
                        MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", MainAdminMail, "Программа Psico", "В программе была зарегистрированна попытка удаления задачи пользователем под номером: "+Program.user+", ключ для удаления: " + Key);
                        SmtpClient client = new SmtpClient("smtp.yandex.ru");
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                        client.EnableSsl = true;
                        client.Send(mail);

                        textBox1.Visible = true;
                        Delete = 1;
                    }
                    catch
                    {
                        CreateInfo("Ошибка отправки ключа, обратитесь к главному администратору!","red", panel1);
                    }

                    break;

                case 1:
                    if (textBox1.Text != "")
                    {
                        if (kolvoPopitok < 10)
                        {
                            if (textBox1.Text == Key)
                            {
                                DialogResult result = MessageBox.Show("Если вы удалите задачу, её не возможно будет вернуть, также задача удалится и у остальных пользователях!", "Внимание!",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                if (result == DialogResult.Yes)
                                {
                                    int SelectedNumb = Convert.ToInt32(comboBox1.SelectedValue);

                                    // Удаление задачи
                                    SqlCommand StrPrc1 = new SqlCommand("delete from resh where zadacha_id = " + SelectedNumb + "", con);
                                    StrPrc1.ExecuteNonQuery();
                                    SqlCommand StrPrc2 = new SqlCommand("delete from Fenom1 where zadacha_id = " + SelectedNumb + "", con);
                                    StrPrc2.ExecuteNonQuery();
                                    SqlCommand StrPrc3 = new SqlCommand("delete from CBFormFill where zadacha_id = " + SelectedNumb + "", con);
                                    StrPrc3.ExecuteNonQuery();
                                    SqlCommand StrPrc4 = new SqlCommand("delete from dpo where zadacha_id = " + SelectedNumb + "", con);
                                    StrPrc4.ExecuteNonQuery();
                                    SqlCommand StrPrc5 = new SqlCommand("delete from vernotv where zadacha_id = " + SelectedNumb + "", con);
                                    StrPrc5.ExecuteNonQuery();
                                    SqlCommand StrPrc6 = new SqlCommand("delete from Zadacha where id_zadacha = " + SelectedNumb + "", con);
                                    StrPrc6.ExecuteNonQuery();

                                    // Создание списка задач 
                                    SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
                                    SqlDataReader dr2 = get_otd_name.ExecuteReader();
                                    DataTable dt = new DataTable();
                                    dt.Load(dr2);
                                    comboBox1.DataSource = dt;
                                    comboBox1.ValueMember = "ido";

                                    CreateInfo("Задача успешно удалена!", "lime", panel1);

                                    textBox1.Visible = false;
                                    Delete = 0;
                                }
                                else
                                {
                                    new administrator().Show();
                                    Close();
                                }
                            }
                            else
                            {
                                CreateInfo("Указанный ключ для удаления задачи неверен!", "red", panel1);
                            }
                        }
                        else
                        {
                            textBox1.Visible = false;
                            Delete = 0;
                            kolvoPopitok = 0;
                            CreateInfo("Вы превысили лимит попыток ввода ключа для удаления задачи, вам необходимо отправить новый ключ главному администратору!", "red", panel1);
                        }
                    }
                    else
                    {
                        CreateInfo("Необходимо ввести ключ для удаления задачи!", "red", panel1);
                    }
                    break;
            }

            con.Close();
            kolvoPopitok++;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Создание списка задач 
            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";

            con.Close();

            FormAlignment();
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

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

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
            label.Font = new Font(label.Font.FontFamily, 14);
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

