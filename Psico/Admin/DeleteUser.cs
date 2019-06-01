using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class DeleteUser : Form
    {
        Timer timer = new Timer();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        ComboBox ComboBox1 = new ComboBox();
        RadioButton RadioButton1 = new RadioButton();
        RadioButton RadioButton2 = new RadioButton();
        int checkSettingForDelete = 0;

        public DeleteUser()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Открытие главной формы администратора
            GetOpenMainForm();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(300, 300);
            datagr1.Location = new Point(30, 100);
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectUser;

            // Заполнение datagr1
            DatagrInfo();

            panel1.Controls.Add(datagr1);

            datagr1.BackgroundColor = Color.White;
            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.EnableHeadersVisualStyles = false;

            datagr1.Columns[0].HeaderText = "Номер пользователя";
            datagr1.Columns[1].HeaderText = "Логин пользователя";
            datagr1.Columns[2].HeaderText = "Роль пользователя";

            label2.Location = new Point(datagr1.Location.X,datagr1.Location.Y-30);

            // Динамическое создание combobox
            ComboBox1.Name = "Combobox1";
            ComboBox1.Location = new Point(label3.Location.X+20,label3.Location.Y+35);
            ComboBox1.Size = new Size(70,50);
            ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox1.Font = new Font(ComboBox1.Font.FontFamily, 12);
            panel1.Controls.Add(ComboBox1);

            // Динамическое создание radiobutton
            RadioButton1.Name = "Radiobutton1";
            RadioButton1.Text = "Пользователя.";
            RadioButton1.Location = new Point(label4.Location.X+20, label4.Location.Y+20);
            RadioButton1.Size = new Size(200, 50);
            RadioButton1.Font = new Font(RadioButton1.Font.FontFamily, 12);
            RadioButton1.Click += UserSettingChanged;
            panel1.Controls.Add(RadioButton1);

            // Динамическое создание radiobutton
            RadioButton2.Name = "Radiobutton2";
            RadioButton2.Text = "Решённую задачу.";
            RadioButton2.Location = new Point(label4.Location.X+20, label4.Location.Y+60);
            RadioButton2.Size = new Size(200, 50);
            RadioButton2.Click += ZadachaSettingChanged;
            RadioButton2.Font = new Font(RadioButton2.Font.FontFamily, 12);
            panel1.Controls.Add(RadioButton2);

            // Адаптация под разрешение экаран
            FormAlignment();
        }

        private void UserSettingChanged(object sender, EventArgs e)
        {
            // Изменение типа удаления на "Удаление пользователя"
            checkSettingForDelete = 1;
        }

        private void ZadachaSettingChanged(object sender, EventArgs e)
        {
            // Изменение типа удаления на "Удаление задачи"
            checkSettingForDelete = 2;
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            // Заполнение combobox
            new SQL_Query().GetInfoForCombobox("select Zadacha_id as \"ido\" from resh where users_id = '" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "'", (panel1.Controls["Combobox1"] as ComboBox));
        }

        private void DeleteUsers(object sender, EventArgs e)
        {
            // Удаление данных в зависимости от указанного типа удаления
            switch (checkSettingForDelete)
            {
                // Если тип удаления не выбран
                case 0:
                    CreateInfo("Для удаления необходимо выбрать что вы хотите удалить: пользователя или решённую им задачу!", "red", panel1);
                    break;
                
                // если выбран тип "Удаление пользователя"
                case 1:
                    // Проверка на удаление главного администратора
                    if (datagr1.CurrentRow.Cells[1].Value.ToString() != "admin" || datagr1.CurrentRow.Cells[2].Value.ToString() != "Admin")
                    {
                        try
                        {
                            // Удаление данных в зависимости от роли пользователя, которого необходимо удалить
                            switch (datagr1.CurrentRow.Cells[2].Value.ToString())
                            {
                                // Если роль пользователя "Админ"
                                case "Admin":
                                    // Удаление данных
                                    new SQL_Query().DeleteInfoFromBD("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");

                                    // Обновление datagr
                                    DatagrInfo();
                                    break;

                                case "Teacher":
                                    // Удаление данных
                                    new SQL_Query().DeleteInfoFromBD("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");

                                    // Обновление datagr
                                    DatagrInfo();
                                    break;

                                case "Student":
                                    // Удаление данных
                                    new SQL_Query().DeleteInfoFromBD("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");
                                    new SQL_Query().DeleteInfoFromBD("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "");

                                    // Обновление datagr
                                    DatagrInfo();
                                    break;
                            }

                            // Вывод сообщения
                            CreateInfo("Пользователь успешно удалён!","lime",panel1);
                        }
                        catch
                        {
                            // Вывод сообщения
                            CreateInfo("Необходимо выбрать пользователя в таблице для его удаления! Если вы выбрали пользователя, а сообщение осталось обратитесь к администратору!", "red", panel1);
                        }
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Невозможно удалить главного администратора!", "red", panel1);
                    }
                    break;

                // Если выбран тип "Удаление задачи"
                case 2:
                    // Проверка на существование решённых задач у выбранного пользователя
                    if ((panel1.Controls["Combobox1"] as ComboBox).SelectedIndex < 0)
                    {
                        // Вывод сообщения
                        CreateInfo("У пользователя нет решённых задач, удаленние отменено!", "red", panel1);
                    }
                    else
                    {
                        // Выбор номера указанной диагностической задачи
                        int ReshId = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select id_resh as 'id' from resh where users_id=" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id=" + (panel1.Controls["Combobox1"] as ComboBox).SelectedValue + ""));

                        // Удаление данных о решении выбранной диагностической задачи
                        new SQL_Query().DeleteInfoFromBD("Delete from resh where id_resh = "+ReshId+"");

                        // Заполнение combobox
                        new SQL_Query().GetInfoForCombobox("select Zadacha_id as \"ido\" from resh where users_id = '" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "'", (panel1.Controls["Combobox1"] as ComboBox));

                        // Вывод сообщения
                        CreateInfo("Вы успешно удалили решённую студентом диагностическую задачу!","lime",panel1);
                    }
                    break;
            }
        }

        private void GetOpenMainForm()
        {
            // Открытие главной формы администратора
            new administrator().Show();
            Close();
        }

        private void DatagrInfo()
        {
            // Заполнение datagr1
            new SQL_Query().UpdateDatagr("select * from CreateTableForDeleteUsers", "CreateTableForUpdateUsers", datagr1);
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

        private void tableFilter(object sender, EventArgs e)
        {
            // Объявление переменных
            int comboboxALL = 0;
            string role = "";

            // Выбор данных для фильтрации
            switch (comboBox2.SelectedItem)
            {
                case "Администратор":
                    role = "Admin";
                    break;

                case "Преподаватель":
                    role = "Teacher";
                    break;

                case "Студент":
                    role = "Student";
                    break;
                case "Всё":
                    comboboxALL = 1;
                    break;
            }

            // Заполнение datagr1
            DatagrInfo();

            // Отключение выбора ячейки в datagr1
            datagr1.CurrentCell = null;

            // Фильтрация datagr1
            if (comboboxALL !=1)
            {
                for (int i = 0; i < datagr1.Rows.Count; i++)
                {
                    if (datagr1.Rows[i].Cells[2].Value.ToString() != role)
                    {
                        datagr1.Rows[i].Visible = false;
                    }
                }
            }
        }

        private void TextFinder(object sender, EventArgs e)
        {
            // Заполнение datagr1
            DatagrInfo();

            // Отключение выбора ячейки в datagr1
            datagr1.CurrentCell = null;

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

