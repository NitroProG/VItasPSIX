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
    public partial class Fenom1 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
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
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Fenom1_Load(object sender, EventArgs e)
        {
            richTextBox2.Text = Program.fenomenologiya;
            richTextBox3.Text = Program.glavsved;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
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

            for (int y = 150, i = 1; i < kolvoRb; i++)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Name = "radiobutton" + i + "";
                radioButton.Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                radioButton.Location = new Point(17, y);
                radioButton.AutoSize = true;
                radioButton.CheckedChanged += radiobutton_checkedchanged;
                panel1.Controls.Add(radioButton);
                y = y + 30;
            }

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1366)
            {
                Width = 1024;
                Height = 768;

                panel2.Width = 1024;
                panel2.Height = 768;

                panel1.Width = 1003;
                panel1.Height = 747;

                richTextBox1.Width = 450;
                richTextBox2.Width = 450;
                richTextBox3.Width = 450;

                label3.MaximumSize = new Size(950, 64);
                label3.AutoSize = true;
                label5.Width = 450;

                button3.Left = button3.Left - 350;
                button1.Left = button1.Left - 340;
                label4.Left = label4.Left - 170;
                richTextBox1.Left = richTextBox1.Left - 170;
                richTextBox2.Left = richTextBox2.Left - 170;

                foreach (Control ctrl in panel1.Controls)
                {
                    int newFontSize = 12; //размер
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize);
                }
            }
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.Left = panel1.Width / 2 - label3.Width / 2;
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
            Program.fenomenologiya = richTextBox2.Text;
            Program.glavsved = richTextBox3.Text;

            Fenom2 fenom2 = new Fenom2();
            fenom2.Show();
            Close();
        }
    }
}
