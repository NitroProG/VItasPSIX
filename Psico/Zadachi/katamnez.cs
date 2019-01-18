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
    public partial class katamnez : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");

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
                SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                StrPrc1.ExecuteNonQuery();
                SpisokZadach spisokZadach = new SpisokZadach();
                spisokZadach.Show();
                Close();
            }
        }

        private void katamnez_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi) + "  -";

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label5.Text = dr["sved"].ToString();
            dr.Close();

            SqlCommand text = new SqlCommand("select katamneztext from katamnez where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = text.ExecuteReader();
            dr1.Read();
            richTextBox1.Text = dr1["katamneztext"].ToString();
            dr1.Close();
        }
    }
}
