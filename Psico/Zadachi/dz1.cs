using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class dz1 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public dz1()
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
                    // Запись данных в БД о решении задачи
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
                    // Выход из программы
                    ExitFromProgram();
                }
            }
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись данных в протокол
            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            // Добавление данных для графиков
            StageInfo();

            // Обнуление переменных
            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            // Открытие главной формы задачи
            new Zadacha().Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Открытие следующей формы
            dz2 dz2 = new dz2();
            dz2.Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.zakl1T = 0;
            timer1.Enabled = true;

            // подключение к БД
            con.Open();

            // Запись данных в поля формы
            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;
            richTextBox3.Text = Program.obsledovaniya;
            richTextBox4.Text = Program.zakluch;

            // Запись данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Запись данных в протокол
            Program.Insert = "Окно - Заключение (Свободная форма): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.zakl1T++;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                Program.AllTBezK = Program.AllTBezK + Program.zakl1T;
            }
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;
            
            // Запись данных о времени
            Program.AllT = Program.AllT + Program.zakl1T;
            Program.AllZakl = Program.zakl1T + Program.AllZakl;
            Program.zakluch = richTextBox4.Text;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Заключение: " + Program.zakluch + "";
            wordinsert.Ins();
            Program.Insert = "Время на заключении (Свободная форма): " + Program.zakl1T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись данных в протокол
            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
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

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("Д");
            Program.StageSec.Add(Program.AllZakl);
            Program.NumberStage.Add(4);
        }
    }
}
