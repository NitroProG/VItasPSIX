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
    public partial class meropriyatiya2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();

        public meropriyatiya2()
        {
            InitializeComponent();
        }

        private void meropriyatiya2_Load(object sender, EventArgs e)
        {
            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand text = new SqlCommand("select meroprtext from meropr where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["meroprtext"].ToString();
            dr1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            meropriyatiya1 meropriyatiya1 = new meropriyatiya1();
            meropriyatiya1.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    Application.Exit();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
    }
}
