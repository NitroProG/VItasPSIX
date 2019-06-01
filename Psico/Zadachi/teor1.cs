using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class teor1 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public teor1()
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
                    // Запись данных о решении задачи
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

            // Запись в протокол
            Program.Insert = "Время общее на этапе гипотезы: " + Program.AllGip + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись о времени
            Program.FullAllGip = Program.FullAllGip + Program.AllGip;
            Program.AllGip = 0;

            // Открытие главной формы диагностической задачи
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Открытие следующей формы
            teor2 teor2 = new teor2();
            teor2.Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.gip1T = 0;
            timer1.Enabled = true;

            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;

            // подключение к БД
            con.Open();

            // Выбор данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Запись данных в протокол
            Program.Insert = "Окно - Гипотезы (Свободная форма): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.gip1T++;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                // Запись о времени
                Program.AllTBezK = Program.AllTBezK + Program.gip1T;
            }
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;

            Program.AllT = Program.AllT + Program.gip1T;
            Program.AllGip = Program.gip1T + Program.AllGip;
            Program.gipotezi = richTextBox2.Text;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Гипотезы: " + Program.gipotezi + "";
            wordinsert.Ins();
            Program.Insert = "Время на гипотезах (Свободная форма): " + Program.gip1T + " сек";
            wordinsert.Ins();
            timer1.Enabled = false;
        }

        private void ExitFromProgram()
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись в протокол
            Program.Insert = "Время общее на этапе гипотезы: " + Program.AllGip + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись о времени
            Program.FullAllGip = Program.FullAllGip + Program.AllGip;
            Program.AllGip = 0;

            // Формирование протокола
            exitProgram.ExProgr();

            // Отправка протокола на почту главному администратору
            exitProgram.ExProtokolSent();

            // Выход из программы
            Application.Exit();
        }

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("Г");
            Program.StageSec.Add(Program.AllGip);
            Program.NumberStage.Add(2);
        }
    }
}
