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
    public partial class SpisokZadach : Form
    {
        public SpisokZadach()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void SpisokZadach_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
            con.Open(); // подключение к БД
            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
