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
    public partial class meropriyatiya2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
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

            // подключение к БД
            con.Open();

            // Запись данных из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Запись данных из БД
            SqlCommand text = new SqlCommand("select meroprtext from meropr where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["meroprtext"].ToString();
            dr1.Close();

            // Запись данных в протокол
            Program.Insert = "Окно - Мероприятия (Общие сведения): ";
            wordinsert.Ins();
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            meropriyatiya1 meropriyatiya1 = new meropriyatiya1();
            meropriyatiya1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе мероприятий: " + Program.AllMeropr + " сек";
            wordinsert.Ins();

            Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
            Program.AllMeropr = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
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

                Program.FullAllMeropr = Program.FullAllMeropr + Program.AllMeropr;
                Program.AllMeropr = 0;

                exitProgram.ExProgr();

                exitProgram.ExProtokolSent();

                Application.Exit();
            }
        }

        private void Timer(object sender, EventArgs e)
        {
            //  Счётчик времени на форме
            Program.meropr2T = Program.meropr2T + 1; 
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.meropr2T;
            Program.AllMeropr = Program.meropr2T + Program.AllMeropr;

            // Запись данных в протокол
            Program.Insert = "Время на мероприятиях (Общие сведения): " + Program.meropr2T + " сек";
            wordinsert.Ins();
        }
    }
}
