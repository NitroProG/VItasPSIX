using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class katamnez : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public katamnez()
        {
            InitializeComponent();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Вывод сообщения
            DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                // Формирование протокола
                ExitFromProgram();

                // Отправка протокола на почту главному администратору
                exitProgram.ExProtokolSent();

                // Выход из программы
                Application.Exit();
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Открытие главной формы диагностической задачи
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenSpisokZadach(object sender, EventArgs e)
        {
            // Вывод сообщения
            DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!","Внимание!", 
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                // Формирование протокола
                ExitFromProgram();

                // Открытие формы со списком диагностических задач
                SpisokZadach spisokZadach = new SpisokZadach();
                spisokZadach.Show();
                Close();
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.katamT = 0;
            timer1.Enabled = true;

            // подключение к БД
            con.Open();

            // Запись данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Запись данных из БД
            richTextBox1.Text = new SQL_Query().GetInfoFromBD("select katamneztext from zadacha where id_zadacha = " + Program.NomerZadachi + "");

            // Запись данных в протокол
            Program.Insert = "Окно - Катамнез:";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.katamT++;
        }

        private void ExitFromThisForm()
        {
            // Запись о времени
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.katamT;
            Program.FullAllKatam = Program.FullAllKatam + Program.katamT;

            // Запись данных в протокол
            Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            Program.StageName.Add("К");
            Program.StageSec.Add(Program.katamT);
            Program.NumberStage.Add(6);
        }

        private void ExitFromProgram()
        {
            // Запись данных о решении задачи в БД
            SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
            StrPrc1.CommandType = CommandType.StoredProcedure;
            StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
            StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
            StrPrc1.ExecuteNonQuery();

            // Выход из окна
            ExitFromThisForm();

            // Формирование протокола
            exitProgram.ExProgr();
        }
    }
}
