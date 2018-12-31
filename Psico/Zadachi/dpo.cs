﻿using System;
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
    public partial class dpo : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-38O7FKR\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f");
        int kolvolb;
        DataGridView datagr = new DataGridView();
        int kolvotext;

        public dpo()
        {
            InitializeComponent();
        }

        private void dpo_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.NomerZadachi);

            richTextBox1.Text = Program.gipotezi;

            con.Open(); // подключение к БД

            SqlCommand Zaprosi = new SqlCommand("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            dr.Close();

            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from dpo where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvolb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvolb = kolvolb + 1;

            datagr.Name = "datagrview";
            datagr.Location = new Point(300, 300);
            SqlDataAdapter da1 = new SqlDataAdapter("select lb, lbtext, lb_image, lb_image2 from dpo where zadacha_id = " + Program.NomerZadachi + "", con);
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
                panel1.Controls.Add(pb);
            }
            else rtb.Height = 500;

            for (int i = 1; i < kolvolb; i++)
            {
                listBox1.Items.Add(Convert.ToString(datagr.Rows[i-1].Cells[0].Value));
            }
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
                    if (Convert.ToString(datagr.Rows[1].Cells[2].Value) != "")
                    {
                        (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[2].Value + ""));
                    }
                    panel1.Controls["richtextbox"].Text = Convert.ToString(datagr.Rows[i-1].Cells[1].Value);
                    panel1.Controls["label"].Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                    kolvotext = panel1.Controls["label"].Text.Length;

                    if (kolvotext > 70)
                    {
                        (panel1.Controls["label"] as Label).AutoSize = false;
                        (panel1.Controls["label"] as Label).Width = 680;
                        (panel1.Controls["label"] as Label).Height = 40;
                    }

                    if (Convert.ToString(datagr.Rows[i - 1].Cells[3].Value) != "")
                    {
                        button5.Visible = true;
                    }
                    else button5.Visible = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < kolvolb; i++)
            {
                if (listBox1.SelectedIndex == i-1)
                {
                    (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[3].Value + ""));
                    button5.Visible = false;
                }
            }
        }
    }
}
