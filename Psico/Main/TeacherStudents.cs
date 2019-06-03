using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class TeacherStudents : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        Timer timer = new Timer();

        public TeacherStudents()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(780, 500);
            datagr1.Location = new Point(panel1.Width / 2 - datagr1.Width / 2, 70);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datagr1.CellClick += SelectUser;
            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionBackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionForeColor = Color.Black;
            datagr1.BackgroundColor = Color.White;
            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.EnableHeadersVisualStyles = false;
            panel1.Controls.Add(datagr1);

            // Заполнение таблицы
            TableFill();

            // Адаптация под разрешение экрана
            FormAlignment();

            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].HeaderText = "Логин";
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[4].Visible = false;
            datagr1.Columns[5].Visible = false;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[7].Visible = false;
            datagr1.Columns[8].Visible = false;
            datagr1.Columns[9].Visible = false;
            datagr1.Columns[10].Visible = false;
            datagr1.Columns[11].HeaderText = "В сети";
            datagr1.Columns[12].Visible = false;
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Изменение статуса на противоположный у пользователя
                if (Convert.ToInt32(datagr1.CurrentRow.Cells[11].Value) == 1)
                {
                    // Изменение статуса пользователя на "Не в сети"
                    new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                }
                else
                {
                    // Изменение статуса пользователя на "В сети"
                    new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=1 WHERE id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                }

                // Заполнение таблицы
                TableFill();

                // Вывод сообщения
                CreateInfo("Статус успешно изменён!", "lime", panel1);
            }
            catch
            {
                // Вывод сообщения
                CreateInfo("Необходимо выбрать пользователя в таблице, для того чтобы изменить его статус!", "red", panel1);
            }
        }

        private void TableFill()
        {
            // Проверка роли пользователя
            if (Program.UserRole == 1)
            {
                // Вывод данных в datagr1
                new SQL_Query().UpdateDatagr("select * from users","users",datagr1);
            }
            else
            {
                // Выбор номера преподавателя
                int Teacherid = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select Teacher_id from users where id_user = " + Program.user + ""));

                // Вывод данных в datagr1
                new SQL_Query().UpdateDatagr("select * from users where Teacher_id = " + Teacherid + "", "users", datagr1);
            }

            // Отмена выбора ячейки в datagr1
            datagr1.CurrentCell = null;
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            // Проверка роли пользователя
            if (Program.UserRole == 1)
            {
                // Удаление динамической созданной Panel
                new Autorization().CloseInfo();

                // Открытие формы администратора
                new administrator().Show();
            }
            else
            {
                // Изменение статуса пользователя на "Не в сети"
                new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

                // Удаление динамической созданной Panel
                new Autorization().CloseInfo();

                // Открытие формы авторизации
                new Autorization().Show();
            }
            Close();
        }

        private void FormAlignment()
        {
            // Адаптация разрешения экрана
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel1.Location = new Point(screen.Width / 2 - panel1.Width / 2, screen.Height / 2 - panel1.Height / 2);
        }

        private void Find(object sender, EventArgs e)
        {
            // Заполнение datagr1
            TableFill();

            // Поиск в datagr1
            int Find;          
            for (int x = 0; x < datagr1.Rows.Count; x++)
            {
                Find = 0;
                for (int y = 0; y < datagr1.ColumnCount; y++)
                {
                    if (datagr1.Rows[x].Cells[y].Value.ToString().Contains(textBox6.Text))
                    {
                        Find = 1;
                    }
                }
                if (Find != 1)
                {
                    datagr1.Rows[x].Visible = false;
                }
            }

            // Очищение строки поиска
            textBox6.Text = "";
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
