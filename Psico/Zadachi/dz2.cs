using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class dz2 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        DataGridView datagr2 = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        int kolvoCb;
        int kolvootv;
        int kolvovibor = 0;
        int kolvotext;
        int stolb = 0;

        public dz2()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.zakl2T = 0;
            timer1.Enabled = true;
            richTextBox1.Text = Program.zakluch;
            Program.KolvoOpenZakl += 1;

            Program.zaklOTV = 0;
            Program.NeVernOtv = 0;

            // подключение к БД
            con.Open();

            // Запись данных на форму из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Выбор количества checkbox необходимых для заполнения формы
            kolvoCb = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Diag'")) + 1;
            stolb = kolvoCb / 2;

            // Выбор количества правильных ответов у задачи
            kolvootv = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Diag'"));

            // Динамическое создание таблицы
            datagr.Name = "datagrview";
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Обновление datagr
            new SQL_Query().UpdateDatagr("select CB from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Diag'", "CBFormFill",datagr);

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            // Обновление datagr1
            new SQL_Query().UpdateDatagr("select otv from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Diag'", "vernotv",datagr1);

            // Создание таблицы с данными из БД
            datagr2.Name = "datagrview2";
            datagr2.Location = new Point(400, 400);
            datagr2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr2);
            datagr2.Visible = false;

            // Обновление datagr2
            new SQL_Query().UpdateDatagr("select name_otv from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'", "Lastotv",datagr2);

            // Динамическое создание checkbox 
            for (int x = 200, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                checkBox.Location = new Point(x, y);
                panel1.Controls.Add(checkBox);
                kolvotext = checkBox.Text.Length;

                if (kolvotext > 60)
                {
                    checkBox.AutoSize = false;
                    checkBox.Width = panel1.Width/3;
                    checkBox.Height = 40;
                    y = y + 40;
                }

                else
                {
                    checkBox.AutoSize = true;
                    y = y + 20;
                }

                // Создание 2 столбца с checkbox
                if (i == stolb)
                {
                    x = 700;
                    y = 246;
                }
            }

            // Выбор количества правильных ответов у задачи
            kolvovibor = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'"));

            // Выбор checkbox которые были выбраны в предыдущий раз на форме
            if (datagr2.Rows.Count != 0)
            {
                foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
                {
                    for (int x = 1; x < kolvoCb; x++)
                    {
                        for (int i = 0; i < kolvovibor; i++)
                        {
                            if (checkBox.Name == Convert.ToString(datagr2.Rows[i].Cells[0].Value))
                            {
                                checkBox.Checked = true;
                            }
                        }
                    }
                }
            }

            // Запись данных в протокол
            Program.Insert = "Окно - Заключение (Машинный выбор): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал кнопку ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о выбранных вариантах ответа
                    GetCBChecked();

                    // Запись данных в БД о решении задачи пользователем
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Выход из программы
                    ExitFromProgram();
                }
            }

            // Если задача не решена
            else
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал кнопку ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о выбранных вариантах ответа
                    GetCBChecked();

                    // Выход из программы   
                    ExitFromProgram();
                }
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            // Запись данных о выбранных вариантах ответа
            GetCBChecked();

            // Выход из окна
            ExitFromThisForm();

            // Открытие предыдущей формы
            dz1 dz1 = new dz1();
            dz1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Запись данных о выбранных вариантах ответа
            GetCBChecked();

            // Выход из окна
            ExitFromThisForm();

            // ЗАпись данных в протокол
            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись данных о времени
            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            // Открытие главной формы администратора
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();

        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.zakl2T++;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                // Запись данных о времени
                Program.AllTBezK = Program.AllTBezK + Program.zakl2T;
            }
        }

        private void ExitFromThisForm()
        {
            // Запись данных в протокол
            Program.Insert = "Правильных ответов: " + Program.zaklOTV + " из " + kolvootv + "";
            wordinsert.CBIns();

            // Условия итогов решения задачи
            if (Program.zaklOTV == kolvootv && Program.NeVernOtv == 0)
            {
                // Задача решена
                Program.diagnoz = 3;
            }

            else if (Program.zaklOTV <= kolvootv && Program.zaklOTV != 0 && Program.NeVernOtv == 0)
            {
                // Задача частично решена
                Program.diagnoz = 2;
            }

            else
            {
                // Задача не решена
                Program.diagnoz = 1;
            }

            // Запись данных в протокол
            switch (Program.diagnoz)
            {
                case 1:
                    Program.Insert = "Диагноз неверный";
                    wordinsert.Ins();
                    break;
                case 2:
                    Program.Insert = "Диагноз частично верный";
                    wordinsert.Ins();
                    break;
                case 3:
                    Program.Insert = "Диагноз верный";
                    wordinsert.Ins();
                    break;
            }

            // Запись данных о времени
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.zakl2T;
            Program.AllZakl = Program.zakl2T + Program.AllZakl;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Время на заключении (Машинный выбор): " + Program.zakl2T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись данных в протокол
            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            // Запись днных для графиков
            StageInfo();

            // Запись данных о времени
            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            // Формирование протокола
            exitProgram.ExProgr();

            // Отправка протокола
            exitProgram.ExProtokolSent();

            // Выход из программы
            Application.Exit();
        }

        private void GetCBChecked()
        {
            // Обновление выбранных ответов
            new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'");

            // Обновление переменных
            int otvchek = 0;
            int otv = 0;
            otv = kolvootv - 1;
            Program.zaklOTV = 0;

            // Перебор всех checkbox
            for (int i = 1; i < kolvoCb; i++)
            {
                // Если checkbox выбран
                if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Checked == true)
                {
                    otvchek = 0;

                    // Перебор всех правильных ответов у задачи
                    for (int a = 0; a < kolvootv; a++)
                    {
                        // Если выбранный checkbox правильный 
                        if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == Convert.ToString(datagr1.Rows[a].Cells[0].Value))
                        {
                            Program.zaklOTV = Program.zaklOTV + 1;

                            // Запись данных в протокол
                            Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ✔";
                            wordinsert.CBIns();

                            otvchek = 1;
                        }

                        else
                        {
                            Program.NeVernOtv = Program.NeVernOtv + 1;
                        }
                    }

                    if (otvchek != 1)
                    {
                        // Запись данных в протокол
                        Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ☒";
                        wordinsert.CBIns();
                    }

                    Program.NeVernOtv = Program.NeVernOtv - otv;

                    // Добавление данных о решении задачи пользователем
                    SqlCommand StrPrc2 = new SqlCommand("Lastotv_add", con);
                    StrPrc2.CommandType = CommandType.StoredProcedure;
                    StrPrc2.Parameters.AddWithValue("@name_otv", (panel1.Controls["checkbox" + i + ""] as CheckBox).Name);
                    StrPrc2.Parameters.AddWithValue("@Form_otv", "Diag");
                    StrPrc2.Parameters.AddWithValue("@user_id", Program.user);
                    StrPrc2.ExecuteNonQuery();
                }
            }
        }

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("Д");
            Program.StageSec.Add(Program.AllZakl);
            Program.NumberStage.Add(4);
        }
    }
}
