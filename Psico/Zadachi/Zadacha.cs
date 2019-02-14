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
using System.IO;
using word = Microsoft.Office.Interop.Word;
using InsertWord;
using System.Threading;

namespace Psico
{
    public partial class Zadacha : Form
    {
        string podzadacha; // переменная подзадачи
        SqlConnection con = DBUtils.GetDBConnection(); // Класс подключения к бд
        WordInsert wordinsert = new WordInsert(); // Класс вставки данных в ворд

        public Zadacha()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.diagnoz == 3) // Если диагноз верный
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                if (result == DialogResult.OK) // Если пользователь нажимает кнопку ОК
                {
                    // Добавление данных о решении задачи пользователем
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    //Добавление данных в ворд
                    try
                    {

                        timer1.Enabled = false; // Отключение таймера времени на форме
                        Program.AllT = Program.AllT + Program.MainT; // Общее время работы на задаче
                        Program.Insert = "Время на основной форме:" + Program.MainT + " сек"; // Данные, которые нужно вывести в ворд документ

                        wordinsert.Ins();

                        Application.Exit(); // Выход из программы
                    }

                    // Если возникла ошибка во время записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }

            // Если задача ещё не решена или диагноз поставлен не верно
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                if (result == DialogResult.OK) // Если пользователь нажимает кнопку ОК
                {

                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false; // Отключение таймера времени на форме
                        Program.AllT = Program.AllT + Program.MainT; // Общее время выполнения задачи
                        Program.Insert = "Время на основной форме:" + Program.MainT + " сек"; // Данные, которые необходимо занести в ворд документ

                        wordinsert.Ins();

                        Application.Exit(); // Выход из программа
                    }

                    // Если возникла ошибка во время записи данных в ворд документ
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
            if (Program.diagnoz == 3) // Если диагноз верный
            {
                DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                if (result == DialogResult.OK) // Если пользователь нажал кнопку ОК
                {
                    // Добавление данных о решении задачи пользователем
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Запись данных в ворд документ
                    try
                    {
                        Program.AllT = Program.AllT + Program.MainT;
                        Program.Insert = "Время на основной форме:" + Program.MainT + " сек";

                        timer1.Enabled = false;

                        wordinsert.Ins();

                        // Закрытие формы и переход на форму со списком задач
                        SpisokZadach spisokZadach = new SpisokZadach();
                        spisokZadach.Show();
                        Close();
                    }

                    // Если возникла ошибка во время записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }

            // Если задача решена или диагноз поставлен не верно
            else
            {
                DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                if (result == DialogResult.OK) // Если пользователь нажимает кнопку ОК
                {

                    // Запись данных в ворд документ
                    try
                    {
                        Program.AllT = Program.AllT + Program.MainT;
                        Program.Insert = "Время на основной форме:" + Program.MainT + " сек";

                        timer1.Enabled = false;

                        wordinsert.Ins();

                        // Переход на форму со списком задач
                        SpisokZadach spisokZadach = new SpisokZadach();
                        spisokZadach.Show();
                        Close();
                    }

                    // Если возникла ошибка во время записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }
        }

        private void Zadacha_Load(object sender, EventArgs e)
        {
            Program.MainT = 0; // Переменная времени на форме
            timer1.Enabled = true; // Счётчик времени на форме

            //Выбор данных из БД
            con.Open(); // подключение к БД
            SqlCommand get_otd_name = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con); // Выбор данных из БД
            SqlDataReader dr = get_otd_name.ExecuteReader(); // Считывание данных из БД

            // Запись данных из БД
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Проверка решения задачи
            switch (Program.diagnoz)
            {
                case 1:
                    label4.Text = "Диагноз неверный";
                    label4.ForeColor = Color.Red;
                    break;
                case 2:
                    label4.Text = "Диагноз частично верный";
                    label4.ForeColor = Color.Green;
                    break;
                case 3:
                    label4.Text = "Диагноз верный";
                    label4.ForeColor = Color.Lime;
                    radioButton3.Enabled = true;
                    radioButton6.Enabled = true;
                    break;
                default:
                    label4.Text = "";
                    break;
            }

            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;

                label3.MaximumSize = new Size(950, 64);
                label3.AutoSize = true;
            }

            // Позиционирование элементов на форме
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.Left = panel1.Width / 2 - label3.Width / 2;
            label4.Left = panel1.Width / 2 - label4.Width / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.MainT;
                Program.Insert = "Время на основной форме:" + Program.MainT + " сек";

                wordinsert.Ins();

                // Переход на выбранную форму
                switch (podzadacha)
                {
                    case "1":
                        Fenom1 fenom1 = new Fenom1();
                        fenom1.Show();
                        Close();
                        break;
                    case "2":
                        teor1 teor1 = new teor1();
                        teor1.Show();
                        Close();
                        break;
                    case "3":
                        dpo dpo = new dpo();
                        dpo.Show();
                        Close();
                        break;
                    case "4":
                        dz1 dz1 = new dz1();
                        dz1.Show();
                        Close();
                        break;
                    case "5":
                        meropriyatiya1 meropriyatiya1 = new meropriyatiya1();
                        meropriyatiya1.Show();
                        Close();
                        break;
                    case "6":
                        katamnez katamnez = new katamnez();
                        katamnez.Show();
                        Close();
                        break;
                }
            }

            // Если возникла ошибка при записи данных в ворд докуменрт
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "1";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "2";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "3";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "4";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "5";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            // Присвоение переменной значение выбранного radiobutton
            podzadacha = "6";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.MainT = Program.MainT + 1; // Счётчик времени на форме
        }
    }
}
