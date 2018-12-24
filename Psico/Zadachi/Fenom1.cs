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
    public partial class Fenom1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
        int kolvoRb;
        DataGridView datagr = new DataGridView();

        public Fenom1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Fenom1_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi);

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from fenom1 where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoRb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoRb = kolvoRb + 1;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300,300);
            SqlDataAdapter da1 = new SqlDataAdapter("select rb, rbtext from fenom1 where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "fenom1");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            for (int y = 132, i = 1; i < kolvoRb; i++)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Name = "radiobutton" + i + "";
                radioButton.Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                radioButton.Location = new Point(17, y);
                radioButton.AutoSize = true;
                radioButton.CheckedChanged += this.radiobutton_checkedchanged;
                panel1.Controls.Add(radioButton);
                y = y + 30;
            }
        }

        public void radiobutton_checkedchanged(object sender, EventArgs e)
        {
            RadioButton radiobtn = (RadioButton)sender;

            for (int x = 1; x < kolvoRb; x++)
            {
                if (radiobtn.Name == "radiobutton" + x + "")
                {
                    richTextBox1.Text = Convert.ToString(datagr.Rows[x-1].Cells[1].Value);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fenom2 fenom2 = new Fenom2();
            fenom2.Show();
            this.Close();
        }
    }
}
