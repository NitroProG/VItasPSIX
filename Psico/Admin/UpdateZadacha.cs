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
    public partial class UpdateZadacha : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        string SelectedStage;
        int SelectedZadacha;

        public UpdateZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        private void UpdateZadachaa(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            switch (SelectedStage)
            {
                case "Fenom1":
                    SqlCommand StrPrc1 = new SqlCommand("Fenom1_update", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@id_fenom1", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc1.Parameters.AddWithValue("@rb", richTextBox1.Text);
                    StrPrc1.Parameters.AddWithValue("@rbtext",richTextBox2.Text);
                    StrPrc1.Parameters.AddWithValue("@zadacha_id",SelectedZadacha);
                    StrPrc1.ExecuteNonQuery();

                    GetSelectFenom1Stage();
                    MessageBox.Show("Данные успешно изменены!","Отлично!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    break;

                case "Fenom2":
                    SqlCommand StrPrc2 = new SqlCommand("CBFormFill_update", con);
                    StrPrc2.CommandType = CommandType.StoredProcedure;
                    StrPrc2.Parameters.AddWithValue("@id_CBFormFill", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc2.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc2.Parameters.AddWithValue("@FormCB", "Fenom");
                    StrPrc2.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc2.ExecuteNonQuery();

                    GetSelectFenom2Stage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Gip":
                    SqlCommand StrPrc3 = new SqlCommand("CBFormFill_update", con);
                    StrPrc3.CommandType = CommandType.StoredProcedure;
                    StrPrc3.Parameters.AddWithValue("@id_CBFormFill", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc3.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc3.Parameters.AddWithValue("@FormCB", "Teor");
                    StrPrc3.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc3.ExecuteNonQuery();

                    GetSelectGipStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Dpo":
                    SqlCommand StrPrc4 = new SqlCommand("dpo_update", con);
                    StrPrc4.CommandType = CommandType.StoredProcedure;
                    StrPrc4.Parameters.AddWithValue("@dpo_id", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc4.Parameters.AddWithValue("@lb_small", richTextBox1.Text);
                    StrPrc4.Parameters.AddWithValue("@lb", richTextBox2.Text);
                    StrPrc4.Parameters.AddWithValue("@lbtext", richTextBox3.Text);
                    StrPrc4.Parameters.AddWithValue("@lb_image", richTextBox4.Text);
                    StrPrc4.Parameters.AddWithValue("@lb_image2", richTextBox5.Text);
                    StrPrc4.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc4.ExecuteNonQuery();

                    GetSelectDpoStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Zakl":
                    SqlCommand StrPrc5 = new SqlCommand("CBFormFill_update", con);
                    StrPrc5.CommandType = CommandType.StoredProcedure;
                    StrPrc5.Parameters.AddWithValue("@id_CBFormFill", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc5.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc5.Parameters.AddWithValue("@FormCB", "Diag");
                    StrPrc5.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc5.ExecuteNonQuery();

                    GetSelectZaklStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Meropr":
                    SqlCommand StrPrc6 = new SqlCommand("meropr_update", con);
                    StrPrc6.CommandType = CommandType.StoredProcedure;
                    StrPrc6.Parameters.AddWithValue("@id_meropr", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc6.Parameters.AddWithValue("@meroprtext", richTextBox1.Text);
                    StrPrc6.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc6.ExecuteNonQuery();

                    GetSelectMeroprStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Katamnez":
                    SqlCommand StrPrc7 = new SqlCommand("katamnez_update", con);
                    StrPrc7.CommandType = CommandType.StoredProcedure;
                    StrPrc7.Parameters.AddWithValue("@id_katamnez", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc7.Parameters.AddWithValue("@katamneztext", richTextBox1.Text);
                    StrPrc7.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc7.ExecuteNonQuery();

                    GetSelectKatamnezStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "VernOtv":
                    SqlCommand StrPrc8 = new SqlCommand("vernotv_update", con);
                    StrPrc8.CommandType = CommandType.StoredProcedure;
                    StrPrc8.Parameters.AddWithValue("@id_vernotv", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc8.Parameters.AddWithValue("@otv", richTextBox1.Text);
                    StrPrc8.Parameters.AddWithValue("@FormVernOtv", datagr1.CurrentRow.Cells[2].Value.ToString());
                    StrPrc8.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc8.ExecuteNonQuery();

                    GetSelectVernOtvStage();
                    MessageBox.Show("Данные успешно изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    MessageBox.Show("Вы не выбрали этап задачи, который хотите изменить","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    break;
            }

            con.Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Динамическое создание label
            Label label = new Label();
            label.Name = "label";
            label.Text = "Выберите задачу, которую хотите изменить";
            label.Location = new Point(label2.Location.X-80,label2.Location.Y-50);
            label.AutoSize = true;
            label.Font = new Font(label.Font.FontFamily,14);
            panel1.Controls.Add(label);

            // Создание списка задач 
            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(panel1.Width/2,panel1.Height/2);
            datagr1.Location = new Point(panel1.Width/50, panel1.Height/4);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectInfoFromDatagr;         
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.Visible = false;

            con.Close();
        }

        private void SelectInfoFromDatagr(object sender, DataGridViewCellEventArgs e)
        {
            switch (SelectedStage)
            {
                case "Fenom1":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                    break;

                case "Fenom2":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                case "Gip":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                case "Dpo":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                    richTextBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
                    richTextBox4.Text = datagr1.CurrentRow.Cells[4].Value.ToString();
                    richTextBox5.Text = datagr1.CurrentRow.Cells[5].Value.ToString();
                    break;

                case "Zakl":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                case "Meropr":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                case "Katamnez":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                case "VernOtv":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    break;

                default:
                    MessageBox.Show("Вы не выбрали этап задачи, который хотите изменить", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void SelectZadacha(object sender, EventArgs e)
        {
            (panel1.Controls["label"] as Label).Text = "Выберите этап задачи, который хотите изменить";
            label2.Visible = false;
            comboBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = true;
            button3.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            button13.Visible = true;

            SelectedZadacha = Convert.ToInt32(comboBox1.SelectedValue);
        }

        private void BackToChooseZadacha(object sender, EventArgs e)
        {
            (panel1.Controls["label"] as Label).Text = "Выберите задачу, которую хотите изменить";
            (panel1.Controls["label"] as Label).Visible = true;
            (panel1.Controls["datagrview1"] as DataGridView).Visible = false;
            label2.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button13.Visible = false;
            richTextBox1.Visible = false;
            richTextBox2.Visible = false;
            richTextBox3.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
        }

        private void UserSelectStage()
        {
            (panel1.Controls["label"] as Label).Visible = false;
            (panel1.Controls["datagrview1"] as DataGridView).Visible = true;
            button5.Visible = true;
            richTextBox1.Visible = false;
            richTextBox2.Visible = false;
            richTextBox3.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
        }

        private void SelectFenom1Stage(object sender, EventArgs e)
        {
            GetSelectFenom1Stage();
        }

        private void GetSelectFenom1Stage()
        {
            SelectedStage = "Fenom1";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from Fenom1 where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "Fenom1");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            richTextBox1.Text = "Название сведений";
            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 50);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 3);
            richTextBox2.Text = "Описание сведений";
        }

        private void SelectFenom2Stage(object sender, EventArgs e)
        {
            GetSelectFenom2Stage();
        }

        private void GetSelectFenom2Stage()
        {
            SelectedStage = "Fenom2";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Fenom'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            richTextBox1.Text = "Название вариантов ответов";
        }

        private void SelectGipStage(object sender, EventArgs e)
        {
            GetSelectGipStage();
        }

        private void GetSelectGipStage()
        {
            SelectedStage = "Gip";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from  CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Teor'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            richTextBox1.Text = "Название вариантов ответов";
        }

        private void SelectDpoStage(object sender, EventArgs e)
        {
            GetSelectDpoStage();
        }

        private void GetSelectDpoStage()
        {
            SelectedStage = "Dpo";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from Dpo where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "Dpo");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            richTextBox1.Text = "Краткое название методик";
            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 40);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, 30);
            richTextBox2.Text = "Полное название методик";
            richTextBox3.Visible = true;
            richTextBox3.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 80);
            richTextBox3.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 4);
            richTextBox3.Text = "Описание методик";
            richTextBox4.Visible = true;
            richTextBox4.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 280);
            richTextBox4.Size = new Size(panel1.Width / 2 - 100, 30);
            richTextBox4.Text = "Путь к файлу с картинкой";
            richTextBox5.Visible = true;
            richTextBox5.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 320);
            richTextBox5.Size = new Size(panel1.Width / 2 - 100, 30);
            richTextBox5.Text = "Путь к файлу со второй картинкой, если она есть";
        }

        private void SelectZaklStage(object sender, EventArgs e)
        {
            GetSelectZaklStage();
        }

        private void GetSelectZaklStage()
        {
            SelectedStage = "Zakl";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Diag'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            richTextBox1.Text = "Название вариантов ответов";
        }

        private void SelectMeroprStage(object sender, EventArgs e)
        {
            GetSelectMeroprStage();
        }

        private void GetSelectMeroprStage()
        {
            SelectedStage = "Meropr";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from meropr where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "Meropr");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 2);
            richTextBox1.Text = "Описание проводимых мероприятий";
        }

        private void SelectKatamnezStage(object sender, EventArgs e)
        {
            GetSelectKatamnezStage();
        }

        private void GetSelectKatamnezStage()
        {
            SelectedStage = "Katamnez";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from katamnez where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "katamnez");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 2);
            richTextBox1.Text = "Описание катамнеза";
        }

        private void SelectVernOtvStage(object sender, EventArgs e)
        {
            GetSelectVernOtvStage();
        }

        private void GetSelectVernOtvStage()
        {
            SelectedStage = "VernOtv";
            UserSelectStage();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from vernotv where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "vernotv");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            richTextBox1.Text = "Название вариантов ответов";
        }
    }
}
