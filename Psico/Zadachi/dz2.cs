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
        SqlConnection con = DBUtils.GetDBConnection(); // Создания подключения к БД
        int kolvoCb; // Количество checkbox на форму
        int kolvootv; // Количество правильных ответов у задачи
        DataGridView datagr = new DataGridView(); // Создание таблицы
        DataGridView datagr1 = new DataGridView(); // Создание таблицы
        int kolvotext; // Количество символов в названии checkbox
        int stolb = 0; // Переменная для уравнивания столбцов на форме
        WordInsert wordinsert = new WordInsert(); // Запись данных в ворд документ

        public dz2()
        {
            InitializeComponent();
        }

        private void dz2_Load(object sender, EventArgs e)
        {
            Program.zakl2T = 0; // Переменная времени на фореме
            timer1.Enabled = true; // Счётчик времени на форме

            Program.zaklOTV = 0; // Переменная отвечающая за правильные ответы
            Program.NeVernOtv = 0; // Переменная отвечающая за неправильные ответы

            con.Open(); // подключение к БД

            richTextBox1.Text = Program.zakluch; // Запись данных на форму данных пользователя

            // Запись данных на форму из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Выбор количества checkbox необходимых для заполнения формы
            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from dz where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;
            stolb = kolvoCb / 2;

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from vernotv where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvootv = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            // Динамическое создание таблицы
            datagr.Name = "datagrview";
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from dz where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "dz");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            SqlDataAdapter da2 = new SqlDataAdapter("select otv from vernotv where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "vernotv");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            // Динамическое создание checkbox 
            for (int x = 242, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                checkBox.Location = new Point(x, y);
                checkBox.CheckedChanged += checkBox_CheckedChanged;
                panel1.Controls.Add(checkBox);
                kolvotext = checkBox.Text.Length;

                // Если название у checkbox слишком длинное перенос на следующую строку
                if (kolvotext > 70)
                {
                    checkBox.AutoSize = false;
                    checkBox.Width = 500;
                    checkBox.Height = 40;
                    y = y + 40;
                }

                // Если название не длинное
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
        }

        public void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            // Выбор всех checkbox на форме
            CheckBox checkBox = (CheckBox)sender;

            // Переборка checkbox по их количеству
            for (int x = 1; x < kolvoCb; x++)
            {
                // При выборе определённого checkbox
                if (checkBox.Name == "checkbox" + x + "")
                {

                    if (checkBox.Checked == true)
                    {
                        // Запись данных в ворд документ
                        try
                        {
                            // Запись данных о выборе checkbox
                            Program.Insert = "Выбран: " + checkBox.Text + "";

                            wordinsert.Ins();
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал кнопку ОК
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

                        Program.AllT = Program.AllT + Program.zakl2T;

                        Program.Insert = "Время на заключении 2:" + Program.zakl2T + " сек";

                        wordinsert.Ins();

                        // Выход из программы
                        Application.Exit();
                    }

                    // Если возникла ошибка при записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }

            // Если задача ещё не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал кнопку ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;

                        Program.AllT = Program.AllT + Program.zakl2T;

                        Program.Insert = "Время на заключении 2:" + Program.zakl2T + " сек";

                        wordinsert.Ins();

                        // Выход из программы
                        Application.Exit();
                    }

                    // Если возникла ошибка при записи данных в ворд документ
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

                Program.AllT = Program.AllT + Program.zakl2T;

                Program.Insert = "Время на заключении 2:" + Program.zakl2T + " сек";

                wordinsert.Ins();

                // Переход на предыдущую форму
                dz1 dz1 = new dz1();
                dz1.Show();
                Close();
            }

            // Если возникла ошибка при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int otv = 0; // Объявление переменной
            otv = kolvootv - 1; // Переменная отвечающая за количество правильных ответов у задачи

            // Перебор всех checkbox
            for (int i = 1; i < kolvoCb; i++)
            {
                // Если checkbox выбран
                if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Checked == true)
                {
                    // Перебор всех правильных ответов у задачи
                    for (int a = 0; a < kolvootv; a++)
                    {
                        // Если выбранный checkbox правильный 
                        if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == Convert.ToString(datagr1.Rows[a].Cells[0].Value))
                        {
                            // Запись данных о правильном выборе checkbox
                            Program.zaklOTV = Program.zaklOTV + 1;
                        }

                        // Запись даннных о неправильном выборе checkbox
                        else Program.NeVernOtv = Program.NeVernOtv + 1;
                    }

                    Program.NeVernOtv = Program.NeVernOtv - otv;
                }
            }

            // Если количество выбранных правильных ответов равно количество правильных ответов у задачи и пользователь не выбрал ни одного неправильного ответа
            if (Program.zaklOTV == kolvootv && Program.NeVernOtv == 0)
            {
                Program.diagnoz = 3; // Задача решена
            }

            // Если количество выбранных правильных ответов меньше количества правильных ответов у задачи и не равняется 0, а также пользователь не выбрал ни одного неправильного ответа
            else if (Program.zaklOTV <= kolvootv && Program.zaklOTV != 0 && Program.NeVernOtv == 0)
            {
                Program.diagnoz = 2; // Задача частично решена
            }

            // Иначе
            else
            {
                Program.diagnoz = 1; // Задача не решена
            }

            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;

                Program.AllT = Program.AllT + Program.zakl2T;

                Program.Insert = "Время на заключении 2:" + Program.zakl2T + " сек";

                wordinsert.Ins();

                // Переход на главную форму задачи
                Zadacha zadacha = new Zadacha();
                zadacha.Show();
                Close();
            }

            // Если возникла ошибка при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.zakl2T = Program.zakl2T + 1; // Счётчик времени на форме
        }
    }
}
