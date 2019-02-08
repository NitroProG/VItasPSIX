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
    public partial class katamnez : Form
    {
        SqlConnection con = DBUtils.GetDBConnection(); // Создание подклчюения к БД
        WordInsert wordinsert = new WordInsert(); // Запись данных в ворд документ

        public katamnez()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о решении задачи в БД
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;

                        Program.AllT = Program.AllT + Program.katamT;

                        Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";

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

            // Если задача не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;

                        Program.AllT = Program.AllT + Program.katamT;

                        Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";

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

                Program.AllT = Program.AllT + Program.katamT;

                Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";

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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!","Внимание!", 
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                // Запись данных о решении задачи в БД
                SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                StrPrc1.ExecuteNonQuery();

                // Запись данных в ворд документ
                try
                {

                    timer1.Enabled = false;

                    Program.AllT = Program.AllT + Program.katamT;

                    Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";

                    wordinsert.Ins();

                    // Переход на форму со списком задач
                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close(); ;
                }

                // Если возникла ошибка при записи данных в ворд документ
                catch
                {
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                }
            }
        }

        private void katamnez_Load(object sender, EventArgs e)
        {
            Program.katamT = 0; // Переменная времени на фореме
            timer1.Enabled = true; // Счётчик времени на форме

            con.Open(); // подключение к БД

            // Запись данных из БД на форму
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Запись данных из БД на форму
            SqlCommand text = new SqlCommand("select katamneztext from katamnez where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["katamneztext"].ToString();
            dr1.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.katamT = Program.katamT + 1;
        }
    }
}
