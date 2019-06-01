using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using System.Net;
using System.Net.Mail;

namespace Psico
{
    public partial class DeleteZadacha : Form
    {
        Timer timer = new Timer();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        int Delete = 0;
        int kolvoPopitok = 0;
        string Key = "";

        public DeleteZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Открытие главной формы администратора
            new administrator().Show();
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
            switch (Delete)
            {
                case 0:
                    // Вывод сообщения
                    CreateInfo("На почту главного администратора выслан ключ для удаления задачи, вам необходимо его указать и ещё раз нажать на кнопку удаления", "lime", panel1);

                    //Выбор почты главного администратора
                    string MainAdminMail = new Shifr().DeShifrovka(new SQL_Query().GetInfoFromBD("select User_Mail from users where id_user = 1"), "Mail");

                    // Генерация ключа для удаления задачи
                    Key = GetKey();

                    try
                    {
                        // Отправка пароля по почте
                        MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", MainAdminMail, "Удаление диагностической задачи в программе Psico", "В программе была зарегистрированна попытка удаления задачи пользователем под номером: "+Program.user+", ключ для удаления: " + Key);
                        SmtpClient client = new SmtpClient("smtp.yandex.ru");
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                        client.EnableSsl = true;
                        client.Send(mail);

                        // Обновление формы
                        comboBox1.Enabled = false;
                        textBox1.Visible = true;
                        Delete = 1;
                    }
                    catch
                    {
                        // Вывод сообщения
                        CreateInfo("Ошибка отправки ключа, обратитесь к главному администратору!","red", panel1);
                    }

                    break;

                case 1:
                    // Проверка на ввод данных
                    if (textBox1.Text != "")
                    {
                        // Проверка количества попыток ввода кода подтрвеждения
                        if (kolvoPopitok < 5)
                        {
                            // Проверка на корректность указанного кода подтверждения
                            if (textBox1.Text == Key)
                            {
                                // Вывод сообщения
                                DialogResult result = MessageBox.Show("Если вы удалите задачу, её не возможно будет вернуть, также задача удалится и у остальных пользователях!", "Внимание!",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                // Если была нажата кнопка "Ок"
                                if (result == DialogResult.Yes)
                                {
                                    // Запись в переменную выбранной диагностической задачи
                                    int SelectedNumb = Convert.ToInt32(comboBox1.SelectedValue);

                                    // Удаление задачи
                                    new SQL_Query().DeleteInfoFromBD("delete from resh where zadacha_id = " + SelectedNumb + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Fenom1 where zadacha_id = " + SelectedNumb + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from CBFormFill where zadacha_id = " + SelectedNumb + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from dpo where zadacha_id = " + SelectedNumb + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from vernotv where zadacha_id = " + SelectedNumb + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Zadacha where id_zadacha = " + SelectedNumb + "");

                                    // Обновление списка задач
                                    new SQL_Query().GetInfoForCombobox("select id_zadacha as \"ido\" from zadacha",comboBox1);

                                    // Вывод сообщения
                                    CreateInfo("Задача успешно удалена!", "lime", panel1);

                                    // Обновление формы
                                    textBox1.Visible = false;
                                    textBox1.Text = "";
                                    comboBox1.Enabled = true;
                                    Delete = 0;
                                    kolvoPopitok = 0;
                                }
                                else
                                {
                                    // Открытие главной формы администратора
                                    new administrator().Show();
                                    Close();
                                }
                            }
                            else
                            {
                                // Вывод сообщения
                                CreateInfo("Указанный ключ для удаления задачи неверен!", "red", panel1);
                            }
                        }
                        else
                        {
                            // Обновление формы
                            textBox1.Visible = false;
                            textBox1.Text = "";
                            comboBox1.Enabled = true;
                            Delete = 0;
                            kolvoPopitok = 0;

                            // Вывод сообщения
                            CreateInfo("Вы превысили лимит попыток ввода ключа для удаления задачи, вам необходимо отправить новый ключ главному администратору!", "red", panel1);
                        }
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо ввести ключ для удаления задачи!", "red", panel1);
                    }
                    break;
            }

            // Прибавление количества попыток ввода кода подтверждения
            kolvoPopitok++;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Обновление списка задач
            new SQL_Query().GetInfoForCombobox("select id_zadacha as \"ido\" from zadacha", comboBox1);
            comboBox1.Width = 100;

            // Адаптация под разрешение экрана
            FormAlignment();
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
    }
}

