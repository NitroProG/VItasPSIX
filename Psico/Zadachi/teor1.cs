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
    public partial class teor1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");

        public teor1()
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
            Program.gipotezi = richTextBox2.Text;

            teor2 teor2 = new teor2();
            teor2.Show();
            Close();
        }

        private void teor1_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi);

            richTextBox1.Text = Program.fenomenologiya;
            richTextBox2.Text = Program.gipotezi;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            dr.Close();
        }
    }
}
