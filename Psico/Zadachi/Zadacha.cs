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

namespace Psico
{
    public partial class Zadacha : Form
    {
        string podzadacha;

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
            SpisokZadach spisokZadach = new SpisokZadach();
            spisokZadach.Show();
            Close();
        }

        private void Zadacha_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi);
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
            con.Open(); // подключение к БД
            SqlCommand get_otd_name = new SqlCommand("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
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
