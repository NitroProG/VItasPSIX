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
using InsertWord;
using System.Threading;

namespace Psico
{
    public partial class dz2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
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
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Выбор количества checkbox необходимых для заполнения формы
            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Diag'", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;
            stolb = kolvoCb / 2;

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Diag'", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvootv = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            // Динамическое создание таблицы
            datagr.Name = "datagrview";
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Diag'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            SqlDataAdapter da2 = new SqlDataAdapter("select otv from vernotv where zadacha_id = " + Program.NomerZadachi + " and FormVernOtv = 'Diag'", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "vernotv");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            // Создание таблицы с данными из БД
            datagr2.Name = "datagrview2";
            datagr2.Location = new Point(400, 400);
            SqlDataAdapter da3 = new SqlDataAdapter("select name_otv from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'", con);
            SqlCommandBuilder cb3 = new SqlCommandBuilder(da3);
            DataSet ds3 = new DataSet();
            da3.Fill(ds3, "Lastotv");
            datagr2.DataSource = ds3.Tables[0];
            datagr2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr2);
            datagr2.Visible = false;

            // Динамическое создание checkbox 
            for (int x = 242, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                checkBox.Location = new Point(x, y);
                panel1.Controls.Add(checkBox);
                kolvotext = checkBox.Text.Length;

                if (kolvotext > 70)
                {
                    checkBox.AutoSize = false;
                    checkBox.Width = 500;
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
                    x = 750;
                    y = 246;
                }
            }

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotvo = new SqlCommand("select count(*) as 'kolvo' from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'", con);
            SqlDataReader dr2 = kolotvo.ExecuteReader();
            dr2.Read();
            kolvovibor = Convert.ToInt32(dr2["kolvo"].ToString());
            dr2.Close();

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
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал кнопку ОК
                if (result == DialogResult.OK)
                {
                    GetCBChecked();

                    // Запись данных в БД о решении задачи пользователем
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    ExitFromProgram();
                }
            }

            // Если задача не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал кнопку ОК
                if (result == DialogResult.OK)
                {
                    GetCBChecked();

                    ExitFromProgram();
                }
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            GetCBChecked();

            ExitFromThisForm();

            dz1 dz1 = new dz1();
            dz1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            GetCBChecked();

            ExitFromThisForm();

            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();

        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.zakl2T = Program.zakl2T + 1;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
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
            // Проверка решения задачи
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

            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.zakl2T;
            Program.AllZakl = Program.zakl2T + Program.AllZakl;

            // Время до решения задачи
            TimeWithoutKatamnez();

            Program.Insert = "Время на заключении (Машинный выбор): " + Program.zakl2T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            exitProgram.ExProgr();

            exitProgram.ExProtokolSent();

            Application.Exit();
        }

        private void GetCBChecked()
        {
            // Обновление выбранных ответов
            SqlCommand delete = new SqlCommand("delete from Lastotv where users_id = " + Program.user + " and Form_otv = 'Diag'", con);
            delete.ExecuteNonQuery();

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
    }
}
