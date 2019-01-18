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
    public partial class meropriyatiya1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");

        public meropriyatiya1()
        {
            InitializeComponent();
        }

        private void meropriyatiya1_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi) + "  -";

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label7.Text = dr["sved"].ToString();
            dr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (
                (richTextBox1.Text != "") ||
                (richTextBox2.Text != "") ||
                (richTextBox3.Text != "")
                )
            {
                meropriyatiya2 meropriyatiya2 = new meropriyatiya2();
                meropriyatiya2.Show();
                Close();
            }
            else MessageBox.Show("Вы не написали рекомендации! Чтобы продолжить вам необходимо написать хоть одну рекомендацию!","Внимание!",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
