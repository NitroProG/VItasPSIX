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
    public partial class Fenom2 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        DataGridView datagr2 = new DataGridView();
        DataGridView datagr5 = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        int AllProsmotrMerodiks;
        int kolvoCb;
        int kolvootv;
        int stolb = 0;
        int kolvovibor = 0;

        public Fenom2()
        {
            InitializeComponent();
        }

        private void ExitProgram(object sender, EventArgs e)
        {         
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о выбранных вариантах ответа
                    GetCBChecked();

                    // Запись данных о решении задачи пользователем в БД
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

                // Если пользователь нажал ОК
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
            Fenom1 fenom1 = new Fenom1();
            fenom1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Запись данных о выбранных вариантах ответа
            GetCBChecked();

            // Выход из окна
            ExitFromThisForm();

            // Запись в протокол
            Program.Insert = "Время общее на этапе феноменологии: " + Program.AllFenom + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            //Запись о времени
            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            // Открытие главной формы задачи
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.Fenom2T = 0;
            timer1.Enabled = true;
            richTextBox1.Text = Program.fenomenologiya;

            // Открытие подключения
            con.Open();

            // Выбор данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Определение количества checkbox на форме
            kolvoCb = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB ='Fenom'")) + 1;

            // Определение переменной для равномерных колонок с checkbox
            stolb = kolvoCb / 2;

            // Выбор количества правильных ответов у задачи
            kolvootv = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Fenom'"));

            // Выбор количества данных в таблице БД
            AllProsmotrMerodiks = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'"));

            // Создание таблицы с данными из БД
            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Заполнение datagr
            new SQL_Query().UpdateDatagr("select CB from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Fenom'","CBFormFill",datagr);

            // Динамическое создание таблицы
            datagr2.Name = "datagrview2";
            datagr2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr2);
            datagr2.Visible = false;

            // Заполнение datagr2
            new SQL_Query().UpdateDatagr("select otv from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Fenom'","vernotv",datagr2);

            // Создание таблицы с данными из БД
            datagr1.Name = "datagrview1";
            datagr1.Location = new Point(400, 400);
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            // Заполнение datagr1
            new SQL_Query().UpdateDatagr("select name_otv from Lastotv where users_id = " + Program.user + " and Form_otv = 'Fenom'","Lastotv",datagr1);

            // Динамическое создание таблицы
            datagr5.Name = "datagrview5";
            datagr5.Location = new Point(300, 300);
            datagr5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr5);
            datagr5.Visible = false;

            // Заполнение datagr5
            new SQL_Query().UpdateDatagr("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'","OtvSelected",datagr5);

            //Динамическое создание checkbox
            for (int x = 200, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                checkBox.Location = new Point(x,y);
                checkBox.AutoSize = true;
                panel1.Controls.Add(checkBox);
                int textlenght = checkBox.Text.Length;

                if (textlenght > 60)
                {
                    checkBox.AutoSize = false;
                    checkBox.Width = panel1.Width / 3;
                    checkBox.Height = 40;
                    y = y + 40;
                }

                else
                {
                    checkBox.AutoSize = true;
                    y = y + 20;
                }

                if (i == stolb)
                {
                    x = 700;
                    y = 246;
                }
            }

            // Выбор количества правильных ответов у задачи
            kolvovibor = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from Lastotv where users_id = " + Program.user + " and Form_otv = 'Fenom'"));
            
            // Выбор checkbox которые были выбраны в предыдущий раз на форме
            if(datagr1.Rows.Count !=0)
            {
                foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
                {
                    for (int x = 1; x < kolvoCb; x++)
                    {
                        for (int i = 0; i < kolvovibor; i++)
                        {
                            if (checkBox.Name == Convert.ToString(datagr1.Rows[i].Cells[0].Value))
                            {
                                checkBox.Checked = true;
                            }
                        }
                    }
                }
            }

            // Запись данных в протокол
            Program.Insert = "Окно - Феноменология (Машинный выбор): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.Fenom2T++;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                // Запись о времени
                Program.AllTBezK = Program.AllTBezK + Program.Fenom2T;
            }
        }

        private void ExitFromThisForm()
        {
            // Запись данных в протокол
            Program.Insert = "Правильных ответов: " + Program.zaklOTV + " из " + kolvootv + "";
            wordinsert.CBIns();

            // Запись о времени
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.Fenom2T;
            Program.AllFenom = Program.Fenom2T + Program.AllFenom;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Время на феноменологии (Машинный выбор): " + Program.Fenom2T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись в протокол
            Program.Insert = "Время общее на этапе феноменологии: " + Program.AllFenom + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись о времени
            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            // Формирование протокола
            exitProgram.ExProgr();

            // Отправка протокола на почту главному администратору
            exitProgram.ExProtokolSent();

            // Выход из программы
            Application.Exit();
        }

        private void GetCBChecked()
        {
            // Обнуление выбранных ответов пользователем
            new SQL_Query().DeleteInfoFromBD("delete from Lastotv where users_id = " + Program.user + " and Form_otv = 'Fenom'");

            // Обнуление переменных
            int otvchek = 0;
            Program.zaklOTV = 0;

            // Перебор всех checkbox
            for (int i = 1; i < kolvoCb; i++)
            {
                // Если checkbox выбран
                if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Checked == true)
                {
                    otvchek = 0;
                    int checkProsmotrmetodik = 0;

                    // Перебор всех правильных ответов у задачи
                    for (int a = 0; a < kolvootv; a++)
                    {
                        // Если выбранный checkbox правильный 
                        if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == Convert.ToString(datagr2.Rows[a].Cells[0].Value))
                        {
                            // Запись данных в протокол
                            Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ✔";
                            wordinsert.CBIns();

                            Program.zaklOTV = Program.zaklOTV + 1;
                            otvchek = 1;
                        }
                    }

                    if (otvchek != 1)
                    {
                        // Запись данных в протокол
                        Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ☒";
                        wordinsert.CBIns();
                    }

                    // Добавление данных о решении задачи пользователем
                    SqlCommand StrPrc2 = new SqlCommand("Lastotv_add", con);
                    StrPrc2.CommandType = CommandType.StoredProcedure;
                    StrPrc2.Parameters.AddWithValue("@name_otv", (panel1.Controls["checkbox" + i + ""] as CheckBox).Name);
                    StrPrc2.Parameters.AddWithValue("@Form_otv", "Fenom");
                    StrPrc2.Parameters.AddWithValue("@user_id", Program.user);
                    StrPrc2.ExecuteNonQuery();

                    if (datagr5.Rows.Count > 1)
                    {
                        for (int y = 0; y < AllProsmotrMerodiks; y++)
                        {
                            if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == datagr5.Rows[y].Cells[0].Value.ToString())
                            {
                                checkProsmotrmetodik = 1;
                            }
                        }
                    }

                    if (checkProsmotrmetodik == 0)
                    {
                        // Запись данных в БД
                        SqlCommand StrPrc1 = new SqlCommand("OtvSelected_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@InfoSelected", (panel1.Controls["checkbox" + i + ""] as CheckBox).Text);
                        StrPrc1.Parameters.AddWithValue("@FormOtvSelected", "Fenom");
                        StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                        StrPrc1.ExecuteNonQuery();

                        // Заполнение datagr5
                        new SQL_Query().UpdateDatagr("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'","OtvSelected",datagr5);

                        // Выбор количества данных в таблице БД
                        AllProsmotrMerodiks = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'"));
                    }
                }                
            }
        }

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("Ф");
            Program.StageSec.Add(Program.AllFenom);
            Program.NumberStage.Add(1);
        }
    }
}
