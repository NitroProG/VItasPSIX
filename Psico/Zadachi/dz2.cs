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
    public partial class dz2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int kolvoCb;
        int kolvootv;
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        int kolvotext;
        int stolb = 0;

        public dz2()
        {
            InitializeComponent();
        }

        private void dz2_Load(object sender, EventArgs e)
        {
            Program.zaklOTV = 0;
            Program.NeVernOtv = 0;

            con.Open(); // подключение к БД

            richTextBox1.Text = Program.zakluch;

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from dz where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;
            stolb = kolvoCb / 2;

            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from vernotv where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvootv = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            datagr.Name = "datagrview";
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from dz where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "dz");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            datagr1.Name = "datagrview1";
            SqlDataAdapter da2 = new SqlDataAdapter("select otv from vernotv where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "vernotv");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            for (int x = 242, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                checkBox.Location = new Point(x, y);
                panel1.Controls.Add(checkBox);
                kolvotext = checkBox.Text.Length;

                if (kolvotext > 70)
                {
                    checkBox.AutoSize = false;
                    checkBox.Width = 500;
                    checkBox.Height = 40;
                    y = y + 40;
                }
                else
                {
                    checkBox.AutoSize = true;
                    y = y + 20;
                }

                if (i == stolb)
                {
                    x = 750;
                    y = 246;
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            dz1 dz1 = new dz1();
            dz1.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int otv = 0;
            otv = kolvootv - 1;

            for (int i = 1; i < kolvoCb; i++)
            {
                if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Checked == true)
                {
                    for (int a = 0; a < kolvootv; a++)
                    {
                        if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == Convert.ToString(datagr1.Rows[a].Cells[0].Value))
                        {
                            Program.zaklOTV = Program.zaklOTV + 1;
                        }
                        else Program.NeVernOtv = Program.NeVernOtv + 1;
                    }
                    Program.NeVernOtv = Program.NeVernOtv - otv;
                }
            }

            if (Program.zaklOTV == kolvootv && Program.NeVernOtv == 0)
            {
                Program.diagnoz = 3;
            }
            else if (Program.zaklOTV <= kolvootv && Program.zaklOTV != 0 && Program.NeVernOtv == 0)
            {
                Program.diagnoz = 2;
            }
            else
            {
                Program.diagnoz = 1;
            }

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }
    }
}
