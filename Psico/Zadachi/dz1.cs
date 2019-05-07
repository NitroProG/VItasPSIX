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
    public partial class dz1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
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

            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            StageInfo();

            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

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

            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;
            richTextBox3.Text = Program.obsledovaniya;
            richTextBox4.Text = Program.zakluch;

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

            // Запись данных в протокол
            Program.Insert = "Окно - Заключение (Свободная форма): ";
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
            Program.zakl1T = Program.zakl1T + 1;
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
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе заключения: " + Program.AllZakl + " сек";
            wordinsert.Ins();

            StageInfo();

            Program.FullAllZakl = Program.FullAllZakl + Program.AllZakl;
            Program.AllZakl = 0;

            exitProgram.ExProgr();

            exitProgram.ExProtokolSent();

            Application.Exit();
        }

        private void StageInfo()
        {
            Program.StageName.Add("Д");
            Program.StageSec.Add(Program.AllZakl);
            Program.NumberStage.Add(4);
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }
    }
}
