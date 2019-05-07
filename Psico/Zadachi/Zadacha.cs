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
        SqlConnection con = DBUtils.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        string podzadacha;

        public Zadacha()
        {
            InitializeComponent();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена верно
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    ExitFromReshZadacha();

                    exitProgram.ExProtokolSent();

                    Application.Exit();
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
                    exitProgram.ExProgr();

                    exitProgram.ExProtokolSent();

                    Application.Exit();
                }
            }
        }

        private void OpenSpisokZadach(object sender, EventArgs e)
        {
            // Если задача решена верно
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    ExitFromReshZadacha();

                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close();
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
                    exitProgram.ExProgr();

                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close();
                }
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            //Выбор данных из БД
            con.Open();
            SqlCommand get_otd_name = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();

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

            // Выравнивание
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.TextAlign = ContentAlignment.TopCenter;
            label4.Width = panel1.Width;
            label4.TextAlign = ContentAlignment.MiddleCenter;

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

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

            //// Позиционирование элементов на форме
            //panel1.Left = Width / 2 - panel1.Width / 2;
            //Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            //label1.Left = panel1.Width / 2 - label1.Width / 2;
            //label3.Left = panel1.Width / 2 - label3.Width / 2;
            //label4.Left = panel1.Width / 2 - label4.Width / 2;
        }

        private void OpenCheckForm(object sender, EventArgs e)
        {
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
                default:
                    MessageBox.Show("Вы ничего не выбрали!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "1";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "2";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "3";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "4";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "5";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "6";
        }

        private void ExitFromReshZadacha()
        {
            // Добавление данных о решении задачи пользователем
            SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
            StrPrc1.CommandType = CommandType.StoredProcedure;
            StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
            StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
            StrPrc1.ExecuteNonQuery();

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
