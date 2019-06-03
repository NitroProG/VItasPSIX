using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class UpdateZadacha : Form
    {
        Timer timer = new Timer();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        string SelectedStage;
        int SelectedZadacha;

        public UpdateZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Открытие главной формы администратора
            new administrator().Show();
            Close();
        }

        private void UpdateZadachaa(object sender, EventArgs e)
        {
            // Проверка выбранных данных
            if (datagr1.CurrentCell != null)
            {
                // Подключение к БД
                con.Open();

                // Изменение задачи в зависимости от выбранного этапа
                switch (SelectedStage)
                {
                    // Изменение данных на этапе Феноменология и анамнез (Свободная форма)
                    case "Fenom1":
                        new SQL_Query().UpdateOneCell("UPDATE Fenom1 SET RB=N'"+richTextBox1.Text+"', RBText=N'"+richTextBox2.Text+"' WHERE id_fenom1="+ Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id="+ SelectedZadacha + "");
                        GetSelectFenom1Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;

                    // Изменение данных на этапе Феноменология и анамнез (Машинный ввод)
                    case "Fenom2":
                        new SQL_Query().UpdateOneCell("UPDATE CBFormFill SET CB=N'"+richTextBox1.Text+"' WHERE id_CBFormFill="+ Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and FormCB='Fenom' and zadacha_id="+ SelectedZadacha + "");
                        GetSelectFenom2Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;
                    
                    // Изменение данных на этапе Гипотезы (Машинный ввод)
                    case "Gip":
                        new SQL_Query().UpdateOneCell("UPDATE CBFormFill SET CB=N'" + richTextBox1.Text + "' WHERE id_CBFormFill=" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and FormCB='Teor' and zadacha_id=" + SelectedZadacha + "");
                        GetSelectGipStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;

                    // Изменение данных на этапе Обследования
                    case "Dpo":
                        new SQL_Query().UpdateOneCell("Update dpo set " +
                                                      "lb_small=N'"+richTextBox1.Text+"'," +
                                                      "lb=N'"+richTextBox2.Text+"'," +
                                                      "lbtext=N'"+richTextBox3.Text+"'," +
                                                      "lb_image=N'"+richTextBox4.Text+"'," +
                                                      "lb_image2=N'"+richTextBox5.Text+"'" +
                                                      "where " +
                                                      "id_dpo="+ Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " " +
                                                      "and " +
                                                      "zadacha_id="+SelectedZadacha+"");

                        GetSelectDpoStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;

                    // Изменение данных на этапе Заключение (Машинный ввод)
                    case "Zakl":
                        new SQL_Query().UpdateOneCell("UPDATE CBFormFill SET CB=N'" + richTextBox1.Text + "' WHERE id_CBFormFill=" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and FormCB='Diag' and zadacha_id=" + SelectedZadacha + "");
                        GetSelectZaklStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;

                    // Изменение основных данных о диагностической задаче
                    case "ZadachaInfo":
                        new SQL_Query().UpdateOneCell("Update Zadacha SET Zapros=N'"+richTextBox1.Text+"',sved=N'"+richTextBox2.Text+"',meroprtext=N'"+richTextBox3.Text+"',katamneztext=N'"+richTextBox4.Text+"' where id_zadacha="+SelectedZadacha+"");
                        GetSelectZadachaInfo();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;

                    // Изменение данных на этапе Верные ответы
                    case "VernOtv":
                        // Объявление переменной                        
                        string VernOtvStage = "";

                        // Выбор Формы варианта верного ответа
                        switch (comboBox2.SelectedItem)
                        {
                            case "Феноменология":
                                VernOtvStage = "Fenom";
                                break;
                            case "Гипотезы":
                                VernOtvStage = "Teor";
                                break;
                            case "Диагноз":
                                VernOtvStage = "Diag";
                                break;
                        }

                        new SQL_Query().UpdateOneCell("update vernotv set otv=N'"+richTextBox1.Text+"',FormVernOtv=N'"+VernOtvStage+"' where id_vernotv="+ Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id="+SelectedZadacha+"");
                        GetSelectVernOtvStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно изменены!", "lime", panel1);
                        break;
                }

                // Отключение от БД
                con.Close();
            }
            else
            {
                // Вывод сообщения
                CreateInfo("Необходимо выбрать данные в таблице для их изменения!","red",panel1);
            }
        }

        private void SelectInfoFromDatagr(object sender, DataGridViewCellEventArgs e)
        {
            // Проверка выбранных данных
            if (datagr1.CurrentCell !=null)
            {
                // Заполнение редактируемых полей в зависимости от выбранного этапа
                switch (SelectedStage)
                {
                    // Выбор данных этапа Феноменология и анамнез (Свободная форма)
                    case "Fenom1":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                        break;

                    // Выбор данных этапа Феноменология и анамнез (Машинный ввод)
                    case "Fenom2":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        break;

                    // Выбор данных этапа Гипотезы (Машинный ввод)
                    case "Gip":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        break;

                    // Выбор данных этапа Обследования
                    case "Dpo":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                        richTextBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
                        richTextBox4.Text = datagr1.CurrentRow.Cells[4].Value.ToString();
                        richTextBox5.Text = datagr1.CurrentRow.Cells[5].Value.ToString();
                        break;

                    // Выбор данных этапа Заключение (Машинный ввод)
                    case "Zakl":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        break;

                    // Выбор основных данных диагностической задачи
                    case "ZadachaInfo":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                        richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                        richTextBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
                        richTextBox4.Text = datagr1.CurrentRow.Cells[4].Value.ToString();
                        break;

                    // Выбор данных этапа верные ответы
                    case "VernOtv":
                        richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();

                        switch (datagr1.CurrentRow.Cells[2].Value.ToString())
                        {
                            case "Fenom":
                                comboBox2.SelectedIndex = 0;
                                comboBox2.SelectedItem = "Феноменология";
                                break;
                            case "Teor":
                                comboBox2.SelectedIndex = 1;
                                comboBox2.SelectedItem = "Гипотезы";
                                break;
                            case "Diag":
                                comboBox2.SelectedIndex = 2;
                                comboBox2.SelectedItem = "Диагноз";
                                break;
                        }
                        break;

                    // Не выбран этап
                    default:
                        // Вывод сообщения
                        CreateInfo("Вы не выбрали этап задачи, который хотите изменить!", "red", panel1);
                        break;
                }
            }
        }

        private void SelectZadacha(object sender, EventArgs e)
        {
            // Проверка на существование диагностических задач в программе
            string checkZadacha = new SQL_Query().GetInfoFromBD("select zapros from zadacha");

            if (checkZadacha != "0")
            {
                // Обновление формы после выбора диагностической задачи для её изменения
                (panel1.Controls["label"] as Label).Text = "Выберите этап задачи, который хотите изменить";
                label2.Visible = false;
                comboBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = false;
                button6.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
                button10.Visible = true;
                button11.Visible = true;
                button13.Visible = true;

                // Запись данных о выбранной диагностической задаче
                SelectedZadacha = Convert.ToInt32(comboBox1.SelectedValue);
            }
            else
            {
                CreateInfo("В программе отсутствуют диагностические задачи!","red",panel1);
            }
        }

        private void BackToChooseZadacha(object sender, EventArgs e)
        {
            // Обновление формы для выбора диагностической задачи
            (panel1.Controls["label"] as Label).Text = "Выберите задачу, которую хотите изменить";
            (panel1.Controls["label"] as Label).Visible = true;
            (panel1.Controls["datagrview1"] as DataGridView).Visible = false;
            label2.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = false;
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = true;
            button5.Visible = false;
            button12.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            textBox6.Visible = false;
            label9.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button13.Visible = false;
            richTextBox1.Visible = false;
            richTextBox2.Visible = false;
            richTextBox3.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
        }

        private void UserSelectStage()
        {
            // Обновление формы после выбора пользователем этапа для изменения
            (panel1.Controls["label"] as Label).Visible = false;
            (panel1.Controls["datagrview1"] as DataGridView).Visible = true;
            button12.Enabled = true;
            button14.Enabled = true;
            button5.Visible = true;
            button12.Visible = true;
            button14.Visible = true;
            button15.Visible = true;
            button16.Visible = true;
            textBox6.Visible = true;
            label9.Visible = true;
            richTextBox1.Visible = false;
            richTextBox2.Visible = false;
            richTextBox3.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
            comboBox2.Visible = false;
        }

        private void SelectFenom1Stage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа Феноменология и анамнез (Свободная форма)
            GetSelectFenom1Stage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "От кого сведения";
            datagr1.Columns[2].HeaderText = "Сведения";
            richTextBox1.MaxLength = 100;
            richTextBox2.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectFenom1Stage()
        {
            // Обновление формы
            SelectedStage = "Fenom1";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from Fenom1 where zadacha_id = " + SelectedZadacha + "", "Fenom1",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox1, "Название сведений");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 50);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 3);
            new ToolTip().SetToolTip(richTextBox2, "Описание сведений");
        }

        private void SelectFenom2Stage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа Феноменология и анамнез (Машинный выбор)
            GetSelectFenom2Stage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectFenom2Stage()
        {
            // Обновление формы
            SelectedStage = "Fenom2";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Fenom'", "CBFormFill", datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectGipStage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа Гипотезы (Машинный выбор)
            GetSelectGipStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectGipStage()
        {
            // Обновление формы
            SelectedStage = "Gip";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from  CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Teor'", "CBFormFill",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectDpoStage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы
            GetSelectDpoStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = true;
            datagr1.Columns[4].Visible = true;
            datagr1.Columns[5].Visible = true;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[1].HeaderText = "Краткое наименование методики";
            datagr1.Columns[2].HeaderText = "Полное наименование методики";
            datagr1.Columns[3].HeaderText = "Данные";
            datagr1.Columns[4].HeaderText = "Путь к файлу с 1 рисунком";
            datagr1.Columns[5].HeaderText = "Путь к файлу со 2 рисунком";
            richTextBox1.MaxLength = 100;
            richTextBox2.MaxLength = 300;
            richTextBox3.MaxLength = 2147483647;
            richTextBox4.MaxLength = 2147483647;
            richTextBox5.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectDpoStage()
        {
            // Обновление формы
            SelectedStage = "Dpo";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from Dpo where zadacha_id = " + SelectedZadacha + "", "Dpo",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox1, "Краткое название методик");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 40);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox2, "Полное название методик");

            richTextBox3.Visible = true;
            richTextBox3.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 80);
            richTextBox3.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 4);
            new ToolTip().SetToolTip(richTextBox3, "Описание методик");

            richTextBox4.Visible = true;
            richTextBox4.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 280);
            richTextBox4.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox4, "Путь к файлу с картинкой");

            richTextBox5.Visible = true;
            richTextBox5.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 320);
            richTextBox5.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox5, "Путь к файлу со второй картинкой, если она есть");
        }

        private void SelectZaklStage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа Заключение (Машинный выбор)
            GetSelectZaklStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectZaklStage()
        {
            // Обновление формы
            SelectedStage = "Zakl";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Diag'", "CBFormFill",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectZadachaInfo(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа основных сведений диагностической задачи
            GetSelectZadachaInfo();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = true;
            datagr1.Columns[4].Visible = true;
            datagr1.Columns[1].HeaderText = "Запрос";
            datagr1.Columns[2].HeaderText = "Сведения";
            datagr1.Columns[3].HeaderText = "Данные по мероприятиям";
            datagr1.Columns[4].HeaderText = "Данные по катамнезу";
            richTextBox1.MaxLength = 300;
            richTextBox2.MaxLength = 100;
            richTextBox3.MaxLength = 2147483647;
            richTextBox4.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectZadachaInfo()
        {
            // Обновление формы
            SelectedStage = "ZadachaInfo";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from zadacha where id_zadacha = " + SelectedZadacha + "", "zadacha",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 55);
            new ToolTip().SetToolTip(richTextBox1, "Запрос");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, richTextBox1.Location.Y+richTextBox1.Height + 10);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, 35);
            new ToolTip().SetToolTip(richTextBox1, "Сведения по задаче");

            richTextBox3.Visible = true;
            richTextBox3.Location = new Point(panel1.Width / 2 + 50, richTextBox2.Location.Y+richTextBox2.Height + 10);
            richTextBox3.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 6);
            new ToolTip().SetToolTip(richTextBox1, "Описание проводимых мероприятий");

            richTextBox4.Visible = true;
            richTextBox4.Location = new Point(panel1.Width / 2 + 50, richTextBox3.Location.Y+richTextBox3.Height+10);
            richTextBox4.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 6);
            new ToolTip().SetToolTip(richTextBox1, "Описание катамнеза");

            button12.Enabled = false;
            button14.Enabled = false;
        }

        private void SelectVernOtvStage(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Обновление формы после выбора этапа верные ответы
            GetSelectVernOtvStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            datagr1.Columns[2].HeaderText = "Этап";
            richTextBox1.MaxLength = 2147483647;

            // Отключение от БД
            con.Close();
        }

        private void GetSelectVernOtvStage()
        {
            // Обновление формы
            SelectedStage = "VernOtv";
            UserSelectStage();
            UserInfoClean();

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select * from vernotv where zadacha_id = " + SelectedZadacha + "", "vernotv",datagr1);

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название верных ответов");

            comboBox2.Visible = true;
            comboBox2.Location = new Point(richTextBox1.Location.X,richTextBox1.Location.Y + 120);
        }

        private void DeleteFromZadacha(object sender, EventArgs e)
        {
            // Проверка выбранных данных
            if (datagr1.CurrentCell != null)
            {
                // Подключение к БД
                con.Open();

                // Удаление данных в зависимости от выбраного этапа
                switch (SelectedStage)
                {
                    // Выбран этап Феноменология и анамнез (Свободная форма)
                    case "Fenom1":
                        new SQL_Query().DeleteInfoFromBD("delete from Fenom1 where id_Fenom1 = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = " + SelectedZadacha + "");
                        GetSelectFenom1Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;

                    // Выбран этап Феноменология и анамнез (Машинный выбор)
                    case "Fenom2":
                        new SQL_Query().DeleteInfoFromBD("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Fenom' and zadacha_id = " + SelectedZadacha + "");
                        GetSelectFenom2Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;

                    // Выбран этап Гипотезы (Машинный выбор)
                    case "Gip":
                        new SQL_Query().DeleteInfoFromBD("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Teor' and zadacha_id = " + SelectedZadacha + "");
                        GetSelectGipStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;

                    // Выбран этап Обследования
                    case "Dpo":
                        new SQL_Query().DeleteInfoFromBD("delete from dpo where id_dpo = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = " + SelectedZadacha + "");
                        GetSelectDpoStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;

                    // Выбран этап Заключение (Машинный выбор)
                    case "Zakl":
                        new SQL_Query().DeleteInfoFromBD("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Diag' and zadacha_id = " + SelectedZadacha + "");
                        GetSelectZaklStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;

                    // Выбран этап верные ответы
                    case "VernOtv":
                        new SQL_Query().DeleteInfoFromBD("delete from vernotv where id_vernotv = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = " + SelectedZadacha + "");
                        GetSelectVernOtvStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно удалены!", "lime", panel1);
                        break;
                }

                // Отключение от БД
                con.Close();
            }
            else
            {
                // Вывод сообщения
                CreateInfo("Для удаления данных необходимо выбрать их в таблице!","red",panel1);
            }
        }

        private void AddToZadacha(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Добавление данных в зависимости от выбранного этапа
            switch (SelectedStage)
            {
                // Выбран этап Феноменология и анамнез (Свободная форма)
                case "Fenom1":
                    // Проверка ввода данных
                    if (richTextBox1.Text!="" && richTextBox2.Text!="")
                    {        
                        // Запись данных в БД
                        SqlCommand StrPrc1 = new SqlCommand("Fenom1_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@rb", richTextBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@rbtext", richTextBox2.Text);
                        StrPrc1.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                        StrPrc1.ExecuteNonQuery();

                        // Обновление формы
                        GetSelectFenom1Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля для добавления","red",panel1);
                    }
                    break;

                // Выбран этап Феноменология и анамнез (Машинный выбор)
                case "Fenom2":
                    // проверка ввода данных
                    if (richTextBox1.Text!="")
                    {
                        // Запись данных в БД
                        SqlCommand StrPrc2 = new SqlCommand("CBFormFill_add", con);
                        StrPrc2.CommandType = CommandType.StoredProcedure;
                        StrPrc2.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc2.Parameters.AddWithValue("@FormCB", "Fenom");
                        StrPrc2.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                        StrPrc2.ExecuteNonQuery();

                        // Обновление формы
                        GetSelectFenom2Stage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля для добавления", "red", panel1);
                    }
                    break;

                // Выбран этап Гипотезы (Машинный выбор)
                case "Gip":
                    // Проверка ввода данных
                    if (richTextBox1.Text!="")
                    {
                        // Запись данных в БД
                        SqlCommand StrPrc3 = new SqlCommand("CBFormFill_add", con);
                        StrPrc3.CommandType = CommandType.StoredProcedure;
                        StrPrc3.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc3.Parameters.AddWithValue("@FormCB", "Teor");
                        StrPrc3.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                        StrPrc3.ExecuteNonQuery();

                        // Обновление формы
                        GetSelectGipStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля для добавления", "red", panel1);
                    }
                    break;

                // Выбран этап Обследования
                case "Dpo":
                    // Проверка ввода данных
                    if (richTextBox2.Text!="" && richTextBox3.Text!="")
                    {
                        // Запись данных в БД
                        SqlCommand StrPrc4 = new SqlCommand("dpo_add", con);
                        StrPrc4.CommandType = CommandType.StoredProcedure;
                        StrPrc4.Parameters.AddWithValue("@lb_small", richTextBox1.Text);
                        StrPrc4.Parameters.AddWithValue("@lb", richTextBox2.Text);
                        StrPrc4.Parameters.AddWithValue("@lbtext", richTextBox3.Text);
                        StrPrc4.Parameters.AddWithValue("@lb_image", richTextBox4.Text);
                        StrPrc4.Parameters.AddWithValue("@lb_image2", richTextBox5.Text);
                        StrPrc4.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                        StrPrc4.ExecuteNonQuery();

                        // Обновление формы
                        GetSelectDpoStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля со * для добавления", "red", panel1);
                    }
                    break;

                // Выбран этап Заключение (Машинный выбор)
                case "Zakl":
                    //Проверка ввода данных
                    if (richTextBox1.Text!="")
                    {
                        // Запись данных в БД
                        SqlCommand StrPrc5 = new SqlCommand("CBFormFill_add", con);
                        StrPrc5.CommandType = CommandType.StoredProcedure;
                        StrPrc5.Parameters.AddWithValue("@cb", richTextBox1.Text);
                        StrPrc5.Parameters.AddWithValue("@FormCB", "Diag");
                        StrPrc5.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                        StrPrc5.ExecuteNonQuery();

                        // Обновление формы
                        GetSelectZaklStage();

                        // Вывод сообщения
                        CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля для добавления", "red", panel1);
                    }
                    break;
                
                // Выбран этап верные ответы
                case "VernOtv":
                    // Проверка ввода данных
                    if (richTextBox1.Text!=""&& comboBox2.SelectedIndex != -1)
                    {
                        // Объявление переменной
                        string VernOtvStage = "";

                        // Выбор формы верного ответа
                        switch (comboBox2.SelectedItem)
                        {
                            case "Феноменология":
                                VernOtvStage = "Fenom";
                                break;
                            case "Гипотезы":
                                VernOtvStage = "Teor";
                                break;
                            case "Диагноз":
                                VernOtvStage = "Diag";
                                break;
                        }

                        // Проверка указанного верного ответа на существование в БД
                        int CountVernOtv = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count (otv) from vernotv where otv = '" + richTextBox1.Text + "' and FormVernOtv = '" + VernOtvStage + "' and zadacha_id = " + SelectedZadacha + ""));

                        // Если указанный верный ответ не зарегистрирован в БД
                        if (CountVernOtv == 0)
                        {
                            // Запись данных в БД
                            SqlCommand StrPrc8 = new SqlCommand("vernotv_add", con);
                            StrPrc8.CommandType = CommandType.StoredProcedure;
                            StrPrc8.Parameters.AddWithValue("@otv", richTextBox1.Text);
                            StrPrc8.Parameters.AddWithValue("@FormVernOtv", VernOtvStage);
                            StrPrc8.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                            StrPrc8.ExecuteNonQuery();

                            // Проверка указанного варианта ответа на существование в БД
                            int CountCBFormFill = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count (CB) from CBFormFill where CB = '" + richTextBox1.Text + "' and FormCB = '" + VernOtvStage + "' and zadacha_id = " + SelectedZadacha + ""));

                            // Если указанный вариант ответа не зарегистрирован в БД
                            if (CountCBFormFill == 0)
                            {
                                // Запись данных в БД
                                SqlCommand StrPrc9 = new SqlCommand("CBFormFill_add", con);
                                StrPrc9.CommandType = CommandType.StoredProcedure;
                                StrPrc9.Parameters.AddWithValue("@cb", richTextBox1.Text);
                                StrPrc9.Parameters.AddWithValue("@FormCB", VernOtvStage);
                                StrPrc9.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                                StrPrc9.ExecuteNonQuery();
                            }

                            // Обновление формы
                            GetSelectVernOtvStage();

                            // Вывод сообщения
                            CreateInfo("Данные успешно добавлены!", "lime", panel1);
                        }
                        else
                        {
                            // Вывод сообщения
                            CreateInfo("Данный вариант ответа уже существует","red",panel1);
                        }
                    }
                    else
                    {
                        // Вывод сообщения
                        CreateInfo("Необходимо заполнить все поля для добавления", "red", panel1);
                    }
                    break;
            }

            // Отключение от БД
            con.Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Динамическое создание label
            Label label = new Label()
            {
                Name = "label",
                Text = "Выберите задачу, которую хотите изменить",
                AutoSize = true,
                Font = new Font(Font.FontFamily, 14)
            };
            panel1.Controls.Add(label);

            // Заполнение Combobox1
            new SQL_Query().GetInfoForCombobox("select id_zadacha as \"ido\" from zadacha", comboBox1);
            comboBox1.Width = 100;

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(panel1.Width / 2, panel1.Height / 2);
            datagr1.Location = new Point(panel1.Width / 50, panel1.Height / 4);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectInfoFromDatagr;
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            panel1.Controls.Add(datagr1);

            datagr1.BackgroundColor = Color.White;
            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            datagr1.EnableHeadersVisualStyles = false;

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.Visible = false;

            // Отключение от БД
            con.Close();

            // Адаптация под разрешение экрана
            FormAlignment();
        }


        private void FormAlignment()
        {
            // Адаптация под разрешение экрана
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
                panel1.Width = 1000;

                button3.Font = new Font(button3.Font.FontFamily, 12);
                button3.Width = 100;
                button3.Left = 50;

                button6.Font = new Font(button6.Font.FontFamily, 12);
                button6.Width = 100;
                button6.Left = button3.Left + button3.Width+10;

                button7.Font = new Font(button7.Font.FontFamily, 12);
                button7.Width = 100;
                button7.Left = button6.Left + button6.Width + 10;

                button10.Font = new Font(button10.Font.FontFamily, 12);
                button10.Width = 100;
                button10.Left = button7.Left + button7.Width + 10;

                button13.Font = new Font(button13.Font.FontFamily, 12);
                button13.Width = 100;
                button13.Location = new Point(panel1.Width/2-button13.Width/2, button6.Location.Y + 40);

                button11.Font = new Font(button11.Font.FontFamily, 12);
                button11.Width = 100;
                button11.Location = new Point(button13.Location.X-button11.Width-10,button3.Location.Y+40);

                button8.Font = new Font(button8.Font.FontFamily, 12);
                button8.Width = 100;
                button8.Location = new Point(button13.Location.X+button13.Width+10, button10.Location.Y + 40);

                datagr1.Width = panel1.Width / 2;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label2.Left = panel1.Width / 2 - label2.Width / 2 - comboBox1.Width/2;
            (panel1.Controls["label"] as Label).Location = new Point(panel1.Width / 2 - (panel1.Controls["label"] as Label).Width / 2, label2.Location.Y - 50);

            comboBox1.Location = new Point(label2.Location.X + label2.Width + 5, label2.Location.Y+2);

            button1.Left = panel1.Width / 2 - button1.Width / 2;

            button2.Location = new Point(30, panel1.Height - 70);
            button4.Location = new Point(30, panel1.Height - 70);
            button5.Location = new Point(panel1.Width - button5.Width - 30, panel1.Height - 70);
            button12.Location = new Point(button5.Location.X - button12.Width - 10, panel1.Height - 70);
            button14.Location = new Point(button12.Location.X - button14.Width - 10, panel1.Height - 70);
            button15.Location = new Point(button5.Location.X, button5.Location.Y-button15.Height - 10);
            button16.Location = new Point(button15.Location.X-button16.Width-10, button15.Location.Y);
            textBox6.Location = new Point(button14.Location.X, button15.Location.Y + 17);
            textBox6.Width = button14.Width;
            label9.Location = new Point(button14.Location.X, button15.Location.Y);
        }

        private void CleanRichTextBox(object sender, EventArgs e)
        {
            // Обновление формы
            UserInfoClean();
        }

        private void UserInfoClean()
        {
            // Обновление формы в зависимости от выбранного этапа
            switch (SelectedStage)
            {
                // Выбран этап Феноменология и анамнез (Свободная форма)
                case "Fenom1":
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    break;

                // Выбран этап Феноменология и анамнез (Машинный выбор)
                case "Fenom2":
                    richTextBox1.Text = "";
                    break;

                // Выбран этап Гипотезы (Машинный выбор)
                case "Gip":
                    richTextBox1.Text = "";
                    break;

                // Выбран этап Обследования
                case "Dpo":
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    richTextBox3.Text = "";
                    richTextBox4.Text = "";
                    richTextBox5.Text = "";
                    break;

                // Выбран этап Заключение (Машинный выбор)
                case "Zakl":
                    richTextBox1.Text = "";
                    break;

                // Выбран этап основные данные диагностической задачи
                case "ZadachaInfo":
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    richTextBox3.Text = "";
                    richTextBox4.Text = "";
                    break;

                // Выбран этап Верные ответы
                case "VernOtv":
                    richTextBox1.Text = "";
                    break;
            }
        }

        private void InfoFinder(object sender, EventArgs e)
        {
            // Поиск в datagr1 в зависимости от выбранного этапа
            switch (SelectedStage)
            {
                // Выбран этап Феноменология и анамнез (Свободная форма)
                case "Fenom1":
                    GetSelectFenom1Stage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "От кого сведения";
                    datagr1.Columns[2].HeaderText = "Сведения";
                    richTextBox1.MaxLength = 50;
                    break;

                // Выбран этап Феноменология и анамнез (Машинный выбор)
                case "Fenom2":
                    GetSelectFenom2Stage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                // Выбран этап Гипотезы (Машинный выбор)
                case "Gip":
                    GetSelectGipStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                // Выбран этап Обследования
                case "Dpo":
                    GetSelectDpoStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = true;
                    datagr1.Columns[4].Visible = true;
                    datagr1.Columns[5].Visible = true;
                    datagr1.Columns[6].Visible = false;
                    datagr1.Columns[1].HeaderText = "Краткое наименование методики";
                    datagr1.Columns[2].HeaderText = "Полное наименование методики";
                    datagr1.Columns[3].HeaderText = "Данные";
                    datagr1.Columns[4].HeaderText = "Путь к файлу с 1 рисунком";
                    datagr1.Columns[5].HeaderText = "Путь к файлу со 2 рисунком";
                    richTextBox1.MaxLength = 100;
                    break;

                // Выбран этап Заключение (Машинный выбор)
                case "Zakl":
                    GetSelectZaklStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                // Выбран этап основные данные диагностической задачи
                case "ZadachaInfo":
                    GetSelectZadachaInfo();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = true;
                    datagr1.Columns[4].Visible = true;
                    datagr1.Columns[1].HeaderText = "Запрос";
                    datagr1.Columns[2].HeaderText = "Сведения";
                    datagr1.Columns[3].HeaderText = "Данные по мероприятиям";
                    datagr1.Columns[4].HeaderText = "Данные по катамнезу";
                    button12.Enabled = false;
                    button14.Enabled = false;
                    richTextBox1.MaxLength = 2147483647;
                    break;

                // Выбран этап Верные ответы
                case "VernOtv":
                    GetSelectVernOtvStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    datagr1.Columns[2].HeaderText = "Этап";
                    break;
            }

            // Поиск
            int Find;
            datagr1.CurrentCell = null;
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
