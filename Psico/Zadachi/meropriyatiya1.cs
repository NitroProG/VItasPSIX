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
        SqlConnection con = DBUtils.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public meropriyatiya1()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.meropr1T = 0;
            timer1.Enabled = true; 

            richTextBox1.Text = Program.rekMa;
            richTextBox2.Text = Program.rekPodr;
            richTextBox3.Text = Program.rekRukovod;

            // подключение к БД
            con.Open();
            
            // Запись данных из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Запись данных в протокол
            Program.Insert = "Окно - Мероприятия (Свободная форма): ";
            wordinsert.Ins();

            // Адаптация
            new FormAlign().Alignment(panel1, panel2, label3, this, button1, button2, button3);
        }

        private void ExitProgram(object sender, EventArgs e)
        {
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

                ExitFromThisForm();

                Program.Insert = "Время общее на этапе мероприятий: " + Program.AllMeropr + " сек";
                wordinsert.Ins();

                StageInfo();

                Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
                Program.AllMeropr = 0;

                exitProgram.ExProgr();

                exitProgram.ExProtokolSent();

                Application.Exit();
            }
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе мероприятий: " + Program.AllMeropr + " сек";
            wordinsert.Ins();

            StageInfo();

            Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
            Program.AllMeropr = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Если поля на форме заполнены
            if ((richTextBox1.Text != "")&&(richTextBox2.Text != "")&&(richTextBox3.Text != ""))
            {
                ExitFromThisForm();

                meropriyatiya2 meropriyatiya2 = new meropriyatiya2();
                meropriyatiya2.Show();
                Close();
            }

            else MessageBox.Show("Чтобы продолжить вам необходимо написать рекомендации!","Внимание!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.meropr1T++;
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;

            Program.AllT = Program.AllT + Program.meropr1T;
            Program.AllMeropr = Program.meropr1T + Program.AllMeropr;

            Program.rekMa = richTextBox1.Text;
            Program.rekPodr = richTextBox2.Text;
            Program.rekRukovod = richTextBox3.Text;

            // Запись данных в протокол
            Program.Insert = "Рекомендации матери: " + Program.rekMa;
            wordinsert.Ins();
            Program.Insert = "Рекомендации подростку: " + Program.rekPodr;
            wordinsert.Ins();
            Program.Insert = "Рекомендации классному руководителю: " + Program.rekRukovod;
            wordinsert.Ins();

            Program.Insert = "Время на мероприятиях (Свободная форма): " + Program.meropr1T + " сек";
            wordinsert.Ins();
        }

        private void StageInfo()
        {
            Program.StageName.Add("М");
            Program.StageSec.Add(Program.AllMeropr);
            Program.NumberStage.Add(5);
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }
    }
}
