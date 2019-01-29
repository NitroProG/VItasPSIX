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
    public partial class Fenom2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int kolvoCb;
        int stolb = 0;
        DataGridView datagr = new DataGridView();

        public Fenom2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Fenom1 fenom1 = new Fenom1();
            fenom1.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void Fenom2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Program.fenomenologiya;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from fenom2 where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;

            stolb = kolvoCb / 2;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            SqlDataAdapter da1 = new SqlDataAdapter("select CB from fenom2 where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "fenom2");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            for (int x = 200, y = 246, i = 1; i < kolvoCb; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "checkbox" + i + "";
                checkBox.Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                checkBox.Location = new Point(x,y);
                checkBox.AutoSize = true;
                panel1.Controls.Add(checkBox);
                y = y + 30;
                if (i == stolb)
                {
                    x = 770;
                    y = 246;
                }
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

                label3.MaximumSize = new Size(950, 64);
                label3.AutoSize = true;

                button3.Left = button3.Left - 350;
                button1.Left = button1.Left - 340;
                label4.Left = label4.Left - 170;
                richTextBox1.Left = richTextBox1.Left - 170;

                foreach (Control ctrl in panel1.Controls)
                {
                    int newFontSize = 12; //размер
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize);

                    if (ctrl is CheckBox)
                    {
                        ctrl.Left = ctrl.Left - 170;
                        int nFontSize = 8; //размер
                        ctrl.Font = new Font(ctrl.Font.FontFamily, nFontSize);
                    }
                }
            }
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.Left = panel1.Width / 2 - label3.Width / 2;
        }
    }
}
