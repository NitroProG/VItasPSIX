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
    public partial class teor2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int kolvoCb;
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        int kolvovibor = 0; // Количество выбранных ответов
        int kolvotext;
        int stolb = 0;
        WordInsert wordinsert = new WordInsert();

        public teor2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.gip2T;
                        Program.Insert = "Время на гипотезах 2:" + Program.gip2T + " сек";

                        wordinsert.Ins();

                        // Выход из программы
                        Application.Exit();
                    }

                    // При возникновении ошибки при записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }

            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.gip2T;
                        Program.Insert = "Время на гипотезах 2:" + Program.gip2T + " сек";

                        wordinsert.Ins();

                        // Выход из программы
                        Application.Exit();
                    }

                    // При возникновении ошибки при записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.gip2T;
                Program.Insert = "Время на гипотезах 2:" + Program.gip2T + " сек";

                wordinsert.Ins();

                // Переход на предыдущую форму
                teor1 teor1 = new teor1();
                teor1.Show();
                Close();
            }

            // При возникновении ошибки при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Обновление выбранных ответов
            SqlCommand delete = new SqlCommand("delete from otvGip", con);
            delete.ExecuteNonQuery();

            // Запись данных о выбранных checkbox
            foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
            {

                // Переборка checkbox по их количеству
                for (int x = 1; x < kolvoCb; x++)
                {
                    // При выборе определённого checkbox
                    if (checkBox.Name == "checkbox" + x + "")
                    {

                        if (checkBox.Checked == true)
                        {
                            // Добавление данных о решении задачи пользователем
                            SqlCommand StrPrc2 = new SqlCommand("otvGip_add", con);
                            StrPrc2.CommandType = CommandType.StoredProcedure;
                            StrPrc2.Parameters.AddWithValue("@name_otv", checkBox.Name);
                            StrPrc2.ExecuteNonQuery();

                            // Запись данных в ворд документ
                            try
                            {
                                // Запись данных о выборе checkbox
                                Program.Insert = "Выбран: " + checkBox.Text + "";

                                wordinsert.CBIns();
                            }

                            // При возникновении ошибки при записи данных в ворд документ
                            catch
                            {
                                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                            }
                        }
                    }
                }
            }

            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.gip2T;
                Program.Insert = "Время на гипотезах 2:" + Program.gip2T + " сек";

                wordinsert.Ins();

                // Переход на главную форму задачи
                Zadacha zadacha = new Zadacha();
                zadacha.Show();
                Close();
            }

            // При возникновении ошибки при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void teor2_Load(object sender, EventArgs e)
        {
            Program.gip2T = 0; // Переменная времени на форме
            timer1.Enabled = true; // Счётчик времени на форме

            richTextBox1.Text = Program.gipotezi;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from teor where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;
            stolb = kolvoCb / 2;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from teor where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "teor");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Создание таблицы с данными из БД
            datagr1.Name = "datagrview1";
            datagr1.Location = new Point(400, 400);
            SqlDataAdapter da2 = new SqlDataAdapter("select name_otv from otvGip", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "otvGip");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

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

                if (i == stolb)
                {
                    x = 750;
                    y = 246;
                }
            }

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from otvGip", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvovibor = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            if (datagr1.Rows.Count != 0)
            {
                // выбор всех checkbox
                foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
                {

                    // Переборка checkbox по их количеству
                    for (int x = 1; x < kolvoCb; x++)
                    {
                        for (int i = 0; i < kolvovibor; i++)
                        {
                            // При выборе определённого checkbox
                            if (checkBox.Name == Convert.ToString(datagr1.Rows[i].Cells[0].Value))
                            {
                                checkBox.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.gip2T = Program.gip2T + 1; 
        }
    }
}
