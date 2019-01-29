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

namespace Psico
{
    public partial class dz1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();

        public dz1()
        {
            InitializeComponent();
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
            Program.zakluch = richTextBox4.Text;

            dz2 dz2 = new dz2();
            dz2.Show();
            Close();
        }

        private void dz1_Load(object sender, EventArgs e)
        {
            con.Open(); // подключение к БД

            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;
            richTextBox3.Text = Program.obsledovaniya;
            richTextBox4.Text = Program.zakluch;

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();
        }
    }
}
