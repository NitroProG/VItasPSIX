using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class meropriyatiya2 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public meropriyatiya2()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.meropr2T = 0;
            timer1.Enabled = true;

            // Подключение к БД
            con.Open();

            // Запись данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Запись данных из БД
            richTextBox1.Text = new SQL_Query().GetInfoFromBD("select meroprtext from zadacha where id_zadacha = " + Program.NomerZadachi + "");

            // Запись данных в протокол
            Program.Insert = "Окно - Мероприятия (Общие сведения): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Открытие предыдущей формы
            meropriyatiya1 meropriyatiya1 = new meropriyatiya1();
            meropriyatiya1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись в протокол
            Program.Insert = "Время общее на этапе мероприятий: " + Program.AllMeropr + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись о времени
            Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
            Program.AllMeropr = 0;

            // Открытие главной формы диагностической задачи
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Вывод сообщения
            DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                // Запись данных о решении задачи в БД
                SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                StrPrc1.ExecuteNonQuery();

                // Выход из окна
                ExitFromThisForm();

                // Запись в протокол
                Program.Insert = "Время общее на этапе мероприятий: " + Program.AllMeropr + " сек";
                wordinsert.Ins();

                // Запись данных для графиков
                StageInfo();

                // Записо о времени
                Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
                Program.AllMeropr = 0;

                // Формирование протокола
                exitProgram.ExProgr();

                // Отправка протокола по почте главному администратору
                exitProgram.ExProtokolSent();

                // Выход из программы 
                Application.Exit();
            }
        }

        private void Timer(object sender, EventArgs e)
        {
            //  Счётчик времени на форме
            Program.meropr2T++;
        }

        private void ExitFromThisForm()
        {
            // Запись о времени
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.meropr2T;
            Program.AllMeropr = Program.meropr2T + Program.AllMeropr;

            // Запись данных в протокол
            Program.Insert = "Время на мероприятиях (Общие сведения): " + Program.meropr2T + " сек";
            wordinsert.Ins();
        }

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("М");
            Program.StageSec.Add(Program.AllMeropr);
            Program.NumberStage.Add(5);
        }
    }
}
