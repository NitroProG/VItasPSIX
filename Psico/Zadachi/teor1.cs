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
    public partial class teor1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
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

                    ExitFromProgram();
                }
            }

            // Если задача не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    ExitFromProgram();
                }
            }
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе гипотезы: " + Program.AllGip + " сек";
            wordinsert.Ins();

            StageInfo();

            Program.FullAllGip = Program.FullAllGip + Program.AllGip;
            Program.AllGip = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

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
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Запись данных в протокол
            Program.Insert = "Окно - Гипотезы (Свободная форма): ";
            wordinsert.Ins();

            // Адаптация
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
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе гипотезы: " + Program.AllGip + " сек";
            wordinsert.Ins();

            StageInfo();

            Program.FullAllGip = Program.FullAllGip + Program.AllGip;
            Program.AllGip = 0;

            exitProgram.ExProgr();

            exitProgram.ExProtokolSent();

            Application.Exit();
        }

        private void StageInfo()
        {
            Program.StageName.Add("Г");
            Program.StageSec.Add(Program.AllGip);
            Program.NumberStage.Add(2);
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }
    }
}
