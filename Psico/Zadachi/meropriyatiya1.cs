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
    public partial class meropriyatiya1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection(); // Подключение к БД
        WordInsert wordinsert = new WordInsert(); // Запись данных в ворд документ

        public meropriyatiya1()
        {
            InitializeComponent();
        }

        private void meropriyatiya1_Load(object sender, EventArgs e)
        {
            Program.meropr1T = 0; // Переменная времени на фореме
            timer1.Enabled = true; // Счётчик времени на форме

            // Запись данных на форму
            richTextBox1.Text = Program.rekMa;
            richTextBox2.Text = Program.rekPodr;
            richTextBox3.Text = Program.rekRukovod;

            con.Open(); // подключение к БД
            
            // Запись данных из БД на форму
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();
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

                        Program.AllT = Program.AllT + Program.meropr1T;

                        // Запись данных указанных на форме
                        Program.rekMa = richTextBox1.Text;
                        Program.rekPodr = richTextBox2.Text;
                        Program.rekRukovod = richTextBox3.Text;

                        Program.Insert = "Время на мероприятиях 1: " + Program.meropr1T + " сек";

                        wordinsert.Ins();

                        if (Program.rekMa != "")
                        {
                            Program.Insert = "Рекомендации матери: " + Program.rekMa;

                            wordinsert.Ins();
                        }

                        if (Program.rekPodr != "")
                        {
                            Program.Insert = "Рекомендации подростку: " + Program.rekPodr;

                            wordinsert.Ins();
                        }

                        if (Program.rekRukovod != "")
                        {
                            Program.Insert = "Рекомендации классному руководителю: " + Program.rekRukovod;

                            wordinsert.Ins();
                        }

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

                        Program.AllT = Program.AllT + Program.meropr1T;

                        // Запись данных указанных на форме
                        Program.rekMa = richTextBox1.Text;
                        Program.rekPodr = richTextBox2.Text;
                        Program.rekRukovod = richTextBox3.Text;

                        Program.Insert = "Время на мероприятиях 1: " + Program.meropr1T + " сек";

                        wordinsert.Ins();

                        if (Program.rekMa != "")
                        {
                            Program.Insert = "Рекомендации матери: " + Program.rekMa;

                            wordinsert.Ins();
                        }

                        if (Program.rekPodr != "")
                        {
                            Program.Insert = "Рекомендации подростку: " + Program.rekPodr;

                            wordinsert.Ins();
                        }

                        if (Program.rekRukovod != "")
                        {
                            Program.Insert = "Рекомендации классному руководителю: " + Program.rekRukovod;

                            wordinsert.Ins();
                        }

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

                Program.AllT = Program.AllT + Program.meropr1T;

                // Запись данных указанных на форме
                Program.rekMa = richTextBox1.Text;
                Program.rekPodr = richTextBox2.Text;
                Program.rekRukovod = richTextBox3.Text;

                Program.Insert = "Время на мероприятиях 1: " + Program.meropr1T + " сек";

                wordinsert.Ins();

                if (Program.rekMa != "")
                {
                    Program.Insert = "Рекомендации матери: " + Program.rekMa;

                    wordinsert.Ins();
                }

                if (Program.rekPodr != "")
                {
                    Program.Insert = "Рекомендации подростку: " + Program.rekPodr;

                    wordinsert.Ins();
                }

                if (Program.rekRukovod != "")
                {
                    Program.Insert = "Рекомендации классному руководителю: " + Program.rekRukovod;

                    wordinsert.Ins();
                }

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

            // Если поля на форме заполнены
            if (
                (richTextBox1.Text != "") &&
                (richTextBox2.Text != "") &&
                (richTextBox3.Text != "")
                )
            {
                // Запись данных в ворд документ
                try
                {

                    timer1.Enabled = false;

                    Program.AllT = Program.AllT + Program.meropr1T;

                    // Запись данных указанных на форме
                    Program.rekMa = richTextBox1.Text;
                    Program.rekPodr = richTextBox2.Text;
                    Program.rekRukovod = richTextBox3.Text;

                    Program.Insert = "Время на мероприятиях 1: " + Program.meropr1T + " сек";

                    wordinsert.Ins();

                    if (Program.rekMa != "")
                    {
                        Program.Insert = "Рекомендации матери: " + Program.rekMa;

                        wordinsert.Ins();
                    }

                    if (Program.rekPodr != "")
                    {
                        Program.Insert = "Рекомендации подростку: " + Program.rekPodr;

                        wordinsert.Ins();
                    }

                    if (Program.rekRukovod != "")
                    {
                        Program.Insert = "Рекомендации классному руководителю: " + Program.rekRukovod;

                        wordinsert.Ins();
                    }

                    // Переход на следующую форму 
                    meropriyatiya2 meropriyatiya2 = new meropriyatiya2();
                    meropriyatiya2.Show();
                    Close();
                }

                // Если возникла ошибка при записи данных в ворд документ
                catch
                {
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                }
            }

            // Если поля на форме не заполнены
            else MessageBox.Show("Чтобы продолжить вам необходимо написать рекомендации!","Внимание!",
                MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.meropr1T = Program.meropr1T + 1; // Счётчик времени на форме
        }
    }
}
