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
    public partial class teor2 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
        int kolvoCb;
        DataGridView datagr = new DataGridView();
        int kolvotext;

        public teor2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            teor1 teor1 = new teor1();
            teor1.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void teor2_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi);

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from teor where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from teor where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "teor");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            for (int x = 242, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                checkBox.Location = new Point(x, y);
                panel1.Controls.Add(checkBox);
                kolvotext = checkBox.Text.Length;

                if (kolvotext > 50)
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

                switch (i)
                {
                    case 13:
                        x = 750;
                        y = 246;
                        break;
                }
            }
        }
    }
}
