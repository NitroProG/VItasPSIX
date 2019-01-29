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
    public partial class SpisokZadach : Form
    {
        DataGridView datagr = new DataGridView();
        int error;
        int kolvoreshzadach;

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
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open(); // подключение к БД

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from resh where users_id = " + Program.user + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoreshzadach = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoreshzadach = kolvoreshzadach + 1;

            datagr.Name = "datagrview";
            datagr.Location = new Point(100, 100);
            SqlDataAdapter da1 = new SqlDataAdapter("select zadacha_id from resh where users_id = " + Program.user + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "dpo");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel2.Controls.Add(datagr);
            datagr.Visible = false;

            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";

            Program.fenomenologiya = "";
            Program.glavsved = "";
            Program.gipotezi = "";
            Program.obsledovaniya = "";
            Program.zakluch = "";
            Program.zaklOTV = 0;
            Program.NeVernOtv = 0;
            Program.diagnoz = 0;

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            error = 0;
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;
            for (int i = 1; i < kolvoreshzadach; i++)
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i-1].Cells[0].Value))
                {
                    DialogResult result = MessageBox.Show("Данная диагностическая задача была уже решена!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    error = 1;
                }
            }

            if (error == 0)
            {
                Zadacha zadacha = new Zadacha();
                zadacha.Show();
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            error = 0;
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;
            for (int i = 1; i < kolvoreshzadach; i++)
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i - 1].Cells[0].Value))
                {
                    label3.Visible = true;
                    error = 1;
                }
            }

            if (error == 0)
            {
                label3.Visible = false;
            }
        }
    }
}
