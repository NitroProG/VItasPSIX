using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class SpisokZadach : Form
    {
        Timer timer = new Timer();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exprg = new ExitProgram();
        int error;
        int kolvoreshzadach;

        public SpisokZadach()
        {
            InitializeComponent();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {

            // Если диагностическая задача была решена
            if (Program.checkopenzadacha != 0)
            {
                // Отправка сообщения на почту главного администратора с протоколом
                exprg.ExProtokolSent();
            }

            //Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Удаление динамической созданной Panel
            new Autorization().CloseInfo();

            // Открытие формы авторизации
            new Autorization().Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Выбор количества решённых задач студентом
            kolvoreshzadach = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from resh where users_id = " + Program.user + "")) + 1;

            // Заполнение datagr данными из БД
            new SQL_Query().CreateDatagr("select zadacha_id from resh where users_id = " + Program.user + "","resh",panel2,datagr);

            // Заполнение combobox данными из БД
            new SQL_Query().GetInfoForCombobox("select id_zadacha as \"ido\" from zadacha",comboBox1);

            // Адаптация разрешения экрана 
            FormAlignment();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Проверка на существование диагностических задач в программе
            string checkZadacha = new SQL_Query().GetInfoFromBD("select zapros from zadacha");

            if (checkZadacha != "0")
            {
                // Обнуление переменных
                Program.AllT = 0;
                Program.fenomenologiya = "";
                Program.glavsved = "";
                Program.gipotezi = "";
                Program.obsledovaniya = "";
                Program.zakluch = "";
                Program.zaklOTV = 0;
                Program.NeVernOtv = 0;
                Program.diagnoz = 0;
                Program.WordOpen = 0;
                Program.StageName.Clear();
                Program.StageSec.Clear();
                Program.NumberStage.Clear();
                Program.KolvoOpenZakl = 0;
                error = 0;

                // Запись в переменную выбранного номера диагностической задачи
                Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedValue);

                // Запись данных о начале решения диагностической задачи
                Program.checkopenzadacha = 1;

                // Цикл выбирающий все решённые задачи
                for (int i = 1; i < kolvoreshzadach; i++)
                {
                    // Проверка выбранной диагностической задачи на решённость
                    if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i - 1].Cells[0].Value))
                    {
                        // Вывод сообщения
                        CreateInfo("Данная диагностическая задача была уже решена!", "red", panel1);

                        // Запись в переменную значение об ошибке
                        error = 1;
                    }
                }

                // Если выбранная диагностическая задача не решена
                if (error == 0)
                {
                    try
                    {
                        // Вставка разрыва страницы
                        wordinsert.CreateShift();

                        // Запись данных в протокол
                        Program.Insert = "Диагностическая задача №" + Program.NomerZadachi + "";
                        wordinsert.Ins();

                        // Удаление данных о последних выбранных вариантах ответа пользователя
                        new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Program.user + "");

                        // Удаление данных о всех выбранных вариантах ответа пользователя
                        new SQL_Query().DeleteInfoFromBD("delete from OtvSelected where users_id = " + Program.user + "");

                        // Удаление динамической созданной Panel
                        new Autorization().CloseInfo();

                        // Открытие главной формы диагностической задачи
                        Zadacha zadacha = new Zadacha();
                        zadacha.Show();
                        Close();
                    }
                    catch
                    {
                        CreateInfo("Отсутствует шаблон протокола! Обратитесь к администратору.", "red", panel1);
                    }
                }
            }
            else
            {
                CreateInfo("В программе отсутствуют диагностические задачи!","red",panel1);
            }                
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            // Проверка решения диагностической задачи
            if (Program.checkopenzadacha !=0)
            {
                // Отправка сообщения с протоколом главному администратору
                exprg.ExProtokolSent();
            }

            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Выход из программы
            Application.Exit();
        }

        private void FormAlignment()
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

            // Запуск таймера
            timer.Tick += Timer_Tick;
            timer.Interval = 5000;
            timer.Start();

            // Динамическое создание Panel
            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            // Динамическое создание Label
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

            // Выбор значения по условию переменной color
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
