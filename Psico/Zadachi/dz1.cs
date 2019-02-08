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
    public partial class dz1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        WordInsert wordinsert = new WordInsert();

        public dz1()
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

                        Program.AllT = Program.AllT + Program.zakl1T;
                        Program.zakluch = richTextBox4.Text;

                        if (Program.zakluch != "")
                        {
                            // Данные, которые нужно записать в ворд документ
                            Program.Insert = "Заключение" + Program.zakluch + "";
                            wordinsert.Ins();
                        }

                        Program.Insert = "Время на заключении 1:" + Program.zakl1T + " сек";
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

                        Program.AllT = Program.AllT + Program.zakl1T;
                        Program.zakluch = richTextBox4.Text;

                        if (Program.zakluch != "")
                        {
                            // Данные, которые нужно записать в ворд документ
                            Program.Insert = "Заключение" + Program.zakluch + "";
                            wordinsert.Ins();
                        }

                        Program.Insert = "Время на заключении 1:" + Program.zakl1T + " сек";
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

                Program.AllT = Program.AllT + Program.zakl1T;
                Program.zakluch = richTextBox4.Text;

                if (Program.zakluch != "")
                {
                    // Данные, которые нужно записать в ворд документ
                    Program.Insert = "Заключение" + Program.zakluch + "";
                    wordinsert.Ins();
                }

                Program.Insert = "Время на заключении 1:" + Program.zakl1T + " сек";
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
            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;

                Program.AllT = Program.AllT + Program.zakl1T;
                Program.zakluch = richTextBox4.Text;

                if (Program.zakluch != "")
                {
                    // Данные, которые нужно записать в ворд документ
                    Program.Insert = "Заключение: " + Program.zakluch + "";
                    wordinsert.Ins();
                }

                Program.Insert = "Время на заключении 1: " + Program.zakl1T + " сек";
                wordinsert.Ins();

                // Переход на следующую форму
                dz2 dz2 = new dz2();
                dz2.Show();
                Close();
            }

            // Если возникла ошибка при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void dz1_Load(object sender, EventArgs e)
        {
            Program.zakl1T = 0; // Переменная времени на фореме
            timer1.Enabled = true; // Счётчик времени на форме

            con.Open(); // подключение к БД

            // Присвоение переменным данные записанные на форме
            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;
            richTextBox3.Text = Program.obsledovaniya;
            richTextBox4.Text = Program.zakluch;

            // Запись данных из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.zakl1T = Program.zakl1T + 1; // Счётчик времени на форме
        }
    }
}
