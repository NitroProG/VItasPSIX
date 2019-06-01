using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using System.Net;
using System.Net.Mail;

namespace Psico
{
    public partial class administrator : Form
    {
        Timer timer = new Timer();
        ExitProgram exitProgram = new ExitProgram();
        SqlConnection con = SQLConnectionString.GetDBConnection();

        public administrator()
        {
            InitializeComponent();
        }

        private void OpenFormUpdateZadacha(object sender, EventArgs e)
        {
            // Открытие формы изменения задачи
            new UpdateZadacha().Show();
            Close();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Открытие формы авторизации
            new Autorization().Show();
            Close();
        }

        private void ExitFromProgramm(object sender, EventArgs e)
        {
            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Выход из программы
            Application.Exit();
        }

        private void OpenFormAddZadacha(object sender, EventArgs e)
        {
            // Открытие формы добавление задачи
            new AddZadacha().Show();
            Close();
        }

        private void OpenFormDeleteZadacha(object sender, EventArgs e)
        {
            // Открытие формы удаление задачи
            new DeleteZadacha().Show();
            Close();
        }

        private void GetLicenseKey(object sender, EventArgs e)
        {
            try
            {
                //Выбор почты главного администратора
                string MainAdminMail = new Shifr().DeShifrovka(new SQL_Query().GetInfoFromBD("select User_Mail from users where id_user = 1"), "Mail");

                // Отправка пароля по почте
                MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", MainAdminMail, "Ключ активации программы Psico", "Ключ активации программного продукта для преподавателя: " + new Shifr().DeShifrovka(new SQL_Query().GetInfoFromBD("select DefenderKey from defender"), "Kod") + "");
                SmtpClient client = new SmtpClient("smtp.yandex.ru")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger"),
                    EnableSsl = true
                };
                client.Send(mail);

                // Вывод сообщения
                CreateInfo("Ключ активации программного продукта для преподавателя был отправлен на почту главному администратору!", "lime", panel1);
            }
            catch
            {
                // Вывод сообщения
                CreateInfo("Ключ активации программного продукта для преподавателя не был отправлен, проверьте подключение к интернету!", "red", panel1);
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация под разрешение экрана
            FormAlignment();
        }

        private void OpenAddUserForm(object sender, EventArgs e)
        {
            // Открытие формы добавления пользователя
            new AddUser().Show();
            Close();
        }

        private void OpenUpdateUserForm(object sender, EventArgs e)
        {
            // Открытие формы изменения пользователя
            new UpdateUser().Show();
            Close();
        }

        private void OpenDeleteUserForm(object sender, EventArgs e)
        {
            // Открытие формы удаления пользователя
            new DeleteUser().Show();
            Close();
        }

        private void FormAlignment()
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
            Panel panel = new Panel
            {
                Name = "panel",
                Size = new Size(600, 100)
            };
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            // Динамической создание Label
            Label label = new Label
            {
                Name = "label",
                Text = labelinfo,
                Size = new Size(panel.Width, panel.Height)
            };
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

        private void OpenUserStatusForm(object sender, EventArgs e)
        {
            // Открытие формы просмотра статуса пользователей
            new TeacherStudents().Show();
            Close();
        }
    }
}
