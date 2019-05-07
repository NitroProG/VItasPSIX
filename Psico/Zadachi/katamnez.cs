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
    public partial class katamnez : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public katamnez()
        {
            InitializeComponent();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                ExitFromProgram();

                exitProgram.ExProtokolSent();

                Application.Exit();
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenSpisokZadach(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!","Внимание!", 
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Если пользователь нажал ОК
            if (result == DialogResult.OK)
            {
                ExitFromProgram();

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
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Выравнивание
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.TextAlign = ContentAlignment.TopCenter;

            // Запись данных из БД
            SqlCommand text = new SqlCommand("select katamneztext from katamnez where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["katamneztext"].ToString();
            dr1.Close();

            // Запись данных в протокол
            Program.Insert = "Окно - Катамнез:";
            wordinsert.Ins();

            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.katamT = Program.katamT + 1;
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.katamT;
            Program.FullAllKatam = Program.FullAllKatam + Program.katamT;

            // Запись данных в протокол
            Program.Insert = "Время на катамнезе:" + Program.katamT + " сек";
            wordinsert.Ins();

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

            ExitFromThisForm();

            exitProgram.ExProgr();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }
    }
}
