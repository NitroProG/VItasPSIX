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
    public partial class katamnez : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();

        public katamnez()
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
            DialogResult result = MessageBox.Show("Если вы перейдёте к списку задач, у вас не будет возможности вернутся к этой задаче!","Внимание!", 
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                SpisokZadach spisokZadach = new SpisokZadach();
                spisokZadach.Show();
                Close();
            }
        }

        private void katamnez_Load(object sender, EventArgs e)
        {
            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand text = new SqlCommand("select katamneztext from katamnez where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["katamneztext"].ToString();
            dr1.Close();
        }
    }
}
