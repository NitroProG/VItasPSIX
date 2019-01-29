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

namespace Psico
{
    public partial class Zadacha : Form
    {
        string podzadacha;
        SqlConnection con = DBUtils.GetDBConnection();

        public Zadacha()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close();
                }
            }
            else
            {
                SpisokZadach spisokZadach = new SpisokZadach();
                spisokZadach.Show();
                Close();
            }
        }

        private void Zadacha_Load(object sender, EventArgs e)
        {
            con.Open(); // подключение к БД
            SqlCommand get_otd_name = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();

            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

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

                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    radioButton3.Enabled = true;
                    radioButton6.Enabled = true;
                    break;
                default:
                    label4.Text = "";
                    break;
            }

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1366)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
            }
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.Left = panel1.Width / 2 - label3.Width / 2;
            label4.Left = panel1.Width / 2 - label4.Width / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                    MessageBox.Show("МОЧА");
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
    }
}
