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
    public partial class dpo : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        int kolvolb;
        DataGridView datagr = new DataGridView();
        int kolvotext;
        string smalltext;

        public dpo()
        {
            InitializeComponent();
        }

        private void dpo_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Program.gipotezi;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from dpo where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvolb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvolb = kolvolb + 1;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            SqlDataAdapter da1 = new SqlDataAdapter("select lb_small,lb, lbtext, lb_image, lb_image2 from dpo where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "dpo");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            Label lb = new Label();
            lb.Name = "label";
            lb.Location = new Point(636,122);
            lb.AutoSize = true;
            panel1.Controls.Add(lb);

            RichTextBox rtb = new RichTextBox();
            rtb.Name = "richtextbox";
            rtb.Location = new Point(636, 180);
            rtb.Width = 680;
            panel1.Controls.Add(rtb);
            rtb.ReadOnly = true;

            if (Convert.ToString(datagr.Rows[1].Cells[2].Value) != "")
            {
                rtb.Height = 200;

                PictureBox pb = new PictureBox();
                pb.Name = "picturebox";
                pb.Location = new Point(636, 400);
                pb.Width = 680;
                pb.Height = 250;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Click += pb_click;
                panel1.Controls.Add(pb);
                pb.Cursor = Cursors.SizeNESW;

                Label label = new Label();
                label.Name = "label";
                label.Text = "Для увеличения или уменьшения рисунка нажмите левую кнопку мыши";
                label.Location = new Point(636,660);
                label.AutoSize = true;
                panel1.Controls.Add(label);
            }
            else rtb.Height = 500;

            for (int i = 1; i < kolvolb; i++)
            {
                smalltext = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);
                if (smalltext != "")
                {
                    listBox1.Items.Add(Convert.ToString(datagr.Rows[i - 1].Cells[0].Value));
                }
                else listBox1.Items.Add(Convert.ToString(datagr.Rows[i - 1].Cells[1].Value));
            }
        }

        private void pb_click(object sender, EventArgs e)
        {
            if (((panel1.Controls["picturebox"] as PictureBox).Width == 680)&& ((panel1.Controls["picturebox"] as PictureBox).Height == 250))
            {
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(0,0);
                (panel1.Controls["picturebox"] as PictureBox).Width = 1345;
                (panel1.Controls["picturebox"] as PictureBox).Height = 740;
                (panel1.Controls["picturebox"] as PictureBox).BringToFront();
            }
            else
            {
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(636,400);
                (panel1.Controls["picturebox"] as PictureBox).Width = 680;
                (panel1.Controls["picturebox"] as PictureBox).Height = 250;
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
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.obsledovaniya = richTextBox2.Text;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < kolvolb; i++)
            {
                if (listBox1.SelectedIndex == i-1)
                {
                    if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
                    {
                        (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[3].Value + ""));
                    }
                    panel1.Controls["richtextbox"].Text = Convert.ToString(datagr.Rows[i-1].Cells[2].Value);

                    panel1.Controls["label"].Text = Convert.ToString(datagr.Rows[i - 1].Cells[1].Value);
                    kolvotext = panel1.Controls["label"].Text.Length;

                    if (kolvotext > 70)
                    {
                        (panel1.Controls["label"] as Label).AutoSize = false;
                        (panel1.Controls["label"] as Label).Width = 680;
                        (panel1.Controls["label"] as Label).Height = 40;
                    }

                    if (Convert.ToString(datagr.Rows[i - 1].Cells[4].Value) != "")
                    {
                        button5.Visible = true;
                        button4.Visible = false;
                    }
                    else button5.Visible = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Следующий рисунок")
            {
                for (int i = 1; i < kolvolb; i++)
                {
                    if (listBox1.SelectedIndex == i - 1)
                    {
                        (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[4].Value + ""));
                        button5.Text = "Предыдущий рисунок";
                    }
                }
            }
            else
            {
                for (int i = 1; i < kolvolb; i++)
                {
                    if (listBox1.SelectedIndex == i - 1)
                    {
                        (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[3].Value + ""));
                        button5.Text = "Следующий рисунок";
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button5.Visible = false;
            button4.Visible = true;
        }
    }
}
