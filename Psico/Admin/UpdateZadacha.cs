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
                    CreateInfo("Данные успешно изменены!", "lime",panel1);
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
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
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
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
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
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
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
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
                    break;

                case "ZadachaInfo":
                    SqlCommand StrPrc6 = new SqlCommand("Zadacha_update", con);
                    StrPrc6.CommandType = CommandType.StoredProcedure;
                    StrPrc6.Parameters.AddWithValue("@id_zadacha", SelectedZadacha);
                    StrPrc6.Parameters.AddWithValue("@Zapros", richTextBox1.Text);
                    StrPrc6.Parameters.AddWithValue("@sved", richTextBox2.Text);
                    StrPrc6.Parameters.AddWithValue("@meroprtext", richTextBox3.Text);
                    StrPrc6.Parameters.AddWithValue("@katamneztext", richTextBox4.Text);
                    StrPrc6.ExecuteNonQuery();

                    GetSelectZadachaInfo();
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
                    break;

                case "VernOtv":

                    string VernOtvStage = "";

                    switch (comboBox2.SelectedItem)
                    {
                        case "Феноменология":
                            VernOtvStage = "Fenom";
                            break;
                        case "Гипотезы":
                            VernOtvStage = "Teor";
                            break;
                        case "Диагноз":
                            VernOtvStage = "Diag";
                            break;
                    }

                    SqlCommand StrPrc8 = new SqlCommand("vernotv_update", con);
                    StrPrc8.CommandType = CommandType.StoredProcedure;
                    StrPrc8.Parameters.AddWithValue("@id_vernotv", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value));
                    StrPrc8.Parameters.AddWithValue("@otv", richTextBox1.Text);
                    StrPrc8.Parameters.AddWithValue("@FormVernOtv", VernOtvStage);
                    StrPrc8.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc8.ExecuteNonQuery();

                    GetSelectVernOtvStage();
                    CreateInfo("Данные успешно изменены!", "lime", panel1);
                    break;
            }

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

                case "ZadachaInfo":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
                    richTextBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
                    richTextBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
                    richTextBox4.Text = datagr1.CurrentRow.Cells[4].Value.ToString();
                    break;

                case "VernOtv":
                    richTextBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();

                    switch (datagr1.CurrentRow.Cells[2].Value.ToString())
                    {
                        case "Fenom":
                            comboBox2.SelectedIndex = 0;
                            comboBox2.SelectedItem = "Феноменология";
                            break;
                        case "Teor":
                            comboBox2.SelectedIndex = 1;
                            comboBox2.SelectedItem = "Гипотезы";
                            break;
                        case "Diag":
                            comboBox2.SelectedIndex = 2;
                            comboBox2.SelectedItem = "Диагноз";
                            break;
                    }
                    break;

                default:
                    CreateInfo("Вы не выбрали этап задачи, который хотите изменить!", "red",panel1);
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
            button4.Visible = false;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
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
            comboBox2.Visible = false;
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = true;
            button5.Visible = false;
            button12.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            textBox6.Visible = false;
            label9.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
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
            button12.Enabled = true;
            button14.Enabled = true;
            button5.Visible = true;
            button12.Visible = true;
            button14.Visible = true;
            button15.Visible = true;
            button16.Visible = true;
            textBox6.Visible = true;
            label9.Visible = true;
            richTextBox1.Visible = false;
            richTextBox2.Visible = false;
            richTextBox3.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
            comboBox2.Visible = false;
            datagr1.CurrentCell = null;
        }

        private void SelectFenom1Stage(object sender, EventArgs e)
        {
            GetSelectFenom1Stage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "От кого сведения";
            datagr1.Columns[2].HeaderText = "Сведения";
            richTextBox1.MaxLength = 100;
            richTextBox2.MaxLength = 2147483647;
        }

        private void GetSelectFenom1Stage()
        {
            SelectedStage = "Fenom1";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from Fenom1 where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "Fenom1");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox1, "Название сведений");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 50);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 3);
            new ToolTip().SetToolTip(richTextBox2, "Описание сведений");
        }

        private void SelectFenom2Stage(object sender, EventArgs e)
        {
            GetSelectFenom2Stage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;
        }

        private void GetSelectFenom2Stage()
        {
            SelectedStage = "Fenom2";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Fenom'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectGipStage(object sender, EventArgs e)
        {
            GetSelectGipStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;
        }

        private void GetSelectGipStage()
        {
            SelectedStage = "Gip";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from  CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Teor'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectDpoStage(object sender, EventArgs e)
        {
            GetSelectDpoStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = true;
            datagr1.Columns[4].Visible = true;
            datagr1.Columns[5].Visible = true;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[1].HeaderText = "Краткое наименование методики";
            datagr1.Columns[2].HeaderText = "Полное наименование методики";
            datagr1.Columns[3].HeaderText = "Данные";
            datagr1.Columns[4].HeaderText = "Путь к файлу с 1 рисунком";
            datagr1.Columns[5].HeaderText = "Путь к файлу со 2 рисунком";
            richTextBox1.MaxLength = 100;
            richTextBox2.MaxLength = 300;
            richTextBox3.MaxLength = 2147483647;
            richTextBox4.MaxLength = 2147483647;
            richTextBox5.MaxLength = 2147483647;
        }

        private void GetSelectDpoStage()
        {
            SelectedStage = "Dpo";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from Dpo where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "Dpo");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox1, "Краткое название методик");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 40);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox2, "Полное название методик");

            richTextBox3.Visible = true;
            richTextBox3.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 80);
            richTextBox3.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 4);
            new ToolTip().SetToolTip(richTextBox3, "Описание методик");

            richTextBox4.Visible = true;
            richTextBox4.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 280);
            richTextBox4.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox4, "Путь к файлу с картинкой");

            richTextBox5.Visible = true;
            richTextBox5.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y + 320);
            richTextBox5.Size = new Size(panel1.Width / 2 - 100, 30);
            new ToolTip().SetToolTip(richTextBox5, "Путь к файлу со второй картинкой, если она есть");
        }

        private void SelectZaklStage(object sender, EventArgs e)
        {
            GetSelectZaklStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            richTextBox1.MaxLength = 2147483647;
        }

        private void GetSelectZaklStage()
        {
            SelectedStage = "Zakl";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from CBFormFill where zadacha_id = " + SelectedZadacha + " and FormCB = 'Diag'", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CBFormFill");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название вариантов ответов");
        }

        private void SelectZadachaInfo(object sender, EventArgs e)
        {
            GetSelectZadachaInfo();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = true;
            datagr1.Columns[4].Visible = true;
            datagr1.Columns[1].HeaderText = "Запрос";
            datagr1.Columns[2].HeaderText = "Сведения";
            datagr1.Columns[3].HeaderText = "Данные по мероприятиям";
            datagr1.Columns[4].HeaderText = "Данные по катамнезу";
            richTextBox1.MaxLength = 300;
            richTextBox2.MaxLength = 100;
            richTextBox3.MaxLength = 2147483647;
            richTextBox4.MaxLength = 2147483647;
        }

        private void GetSelectZadachaInfo()
        {
            SelectedStage = "ZadachaInfo";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from zadacha where id_zadacha = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "zadacha");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 55);
            new ToolTip().SetToolTip(richTextBox1, "Запрос");

            richTextBox2.Visible = true;
            richTextBox2.Location = new Point(panel1.Width / 2 + 50, richTextBox1.Location.Y+richTextBox1.Height + 10);
            richTextBox2.Size = new Size(panel1.Width / 2 - 100, 35);
            new ToolTip().SetToolTip(richTextBox1, "Сведения по задаче");

            richTextBox3.Visible = true;
            richTextBox3.Location = new Point(panel1.Width / 2 + 50, richTextBox2.Location.Y+richTextBox2.Height + 10);
            richTextBox3.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 6);
            new ToolTip().SetToolTip(richTextBox1, "Описание проводимых мероприятий");

            richTextBox4.Visible = true;
            richTextBox4.Location = new Point(panel1.Width / 2 + 50, richTextBox3.Location.Y+richTextBox3.Height+10);
            richTextBox4.Size = new Size(panel1.Width / 2 - 100, panel1.Height / 6);
            new ToolTip().SetToolTip(richTextBox1, "Описание катамнеза");

            button12.Enabled = false;
            button14.Enabled = false;
        }

        private void SelectVernOtvStage(object sender, EventArgs e)
        {
            GetSelectVernOtvStage();
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].Visible = true;
            datagr1.Columns[2].Visible = true;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[1].HeaderText = "Вариант ответа";
            datagr1.Columns[2].HeaderText = "Этап";
            richTextBox1.MaxLength = 2147483647;
        }

        private void GetSelectVernOtvStage()
        {
            SelectedStage = "VernOtv";
            UserSelectStage();
            UserInfoClean();

            SqlDataAdapter da1 = new SqlDataAdapter("select * from vernotv where zadacha_id = " + SelectedZadacha + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "vernotv");
            datagr1.DataSource = ds1.Tables[0];

            richTextBox1.Visible = true;
            richTextBox1.Location = new Point(panel1.Width / 2 + 50, datagr1.Location.Y);
            richTextBox1.Size = new Size(panel1.Width / 2 - 100, 100);
            new ToolTip().SetToolTip(richTextBox1, "Название верных ответов");

            comboBox2.Visible = true;
            comboBox2.Location = new Point(richTextBox1.Location.X,richTextBox1.Location.Y + 120);
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void DeleteFromZadacha(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            switch (SelectedStage)
            {
                case "Fenom1":
                    SqlCommand delete1 = new SqlCommand("delete from Fenom1 where id_Fenom1 = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = "+SelectedZadacha+"", con);
                    delete1.ExecuteNonQuery();

                    GetSelectFenom1Stage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;

                case "Fenom2":
                    SqlCommand delete2 = new SqlCommand("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Fenom' and zadacha_id = " + SelectedZadacha + "", con);
                    delete2.ExecuteNonQuery();

                    GetSelectFenom2Stage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;

                case "Gip":
                    SqlCommand delete3 = new SqlCommand("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Teor' and zadacha_id = " + SelectedZadacha + "", con);
                    delete3.ExecuteNonQuery();

                    GetSelectGipStage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;

                case "Dpo":
                    SqlCommand delete4 = new SqlCommand("delete from dpo where id_dpo = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = " + SelectedZadacha + "", con);
                    delete4.ExecuteNonQuery();

                    GetSelectDpoStage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;

                case "Zakl":
                    SqlCommand delete5 = new SqlCommand("delete from CBFormFill where id_CBFormFill = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + "and FormCB = 'Diag' and zadacha_id = " + SelectedZadacha + "", con);
                    delete5.ExecuteNonQuery();

                    GetSelectZaklStage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;

                case "VernOtv":
                    SqlCommand delete8 = new SqlCommand("delete from vernotv where id_vernotv = " + Convert.ToInt16(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id = " + SelectedZadacha + "", con);
                    delete8.ExecuteNonQuery();

                    GetSelectVernOtvStage();
                    CreateInfo("Данные успешно удалены!", "lime", panel1);
                    break;
            }

            con.Close();
        }

        private void AddToZadacha(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            switch (SelectedStage)
            {
                case "Fenom1":
                    SqlCommand StrPrc1 = new SqlCommand("Fenom1_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@rb", richTextBox1.Text);
                    StrPrc1.Parameters.AddWithValue("@rbtext", richTextBox2.Text);
                    StrPrc1.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc1.ExecuteNonQuery();

                    GetSelectFenom1Stage();
                    CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    break;

                case "Fenom2":
                    SqlCommand StrPrc2 = new SqlCommand("CBFormFill_add", con);
                    StrPrc2.CommandType = CommandType.StoredProcedure;
                    StrPrc2.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc2.Parameters.AddWithValue("@FormCB", "Fenom");
                    StrPrc2.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc2.ExecuteNonQuery();

                    GetSelectFenom2Stage();
                    CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    break;

                case "Gip":
                    SqlCommand StrPrc3 = new SqlCommand("CBFormFill_add", con);
                    StrPrc3.CommandType = CommandType.StoredProcedure;
                    StrPrc3.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc3.Parameters.AddWithValue("@FormCB", "Teor");
                    StrPrc3.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc3.ExecuteNonQuery();

                    GetSelectGipStage();
                    CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    break;

                case "Dpo":
                    SqlCommand StrPrc4 = new SqlCommand("dpo_add", con);
                    StrPrc4.CommandType = CommandType.StoredProcedure;
                    StrPrc4.Parameters.AddWithValue("@lb_small", richTextBox1.Text);
                    StrPrc4.Parameters.AddWithValue("@lb", richTextBox2.Text);
                    StrPrc4.Parameters.AddWithValue("@lbtext", richTextBox3.Text);
                    StrPrc4.Parameters.AddWithValue("@lb_image", richTextBox4.Text);
                    StrPrc4.Parameters.AddWithValue("@lb_image2", richTextBox5.Text);
                    StrPrc4.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc4.ExecuteNonQuery();

                    GetSelectDpoStage();
                    CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    break;

                case "Zakl":
                    SqlCommand StrPrc5 = new SqlCommand("CBFormFill_add", con);
                    StrPrc5.CommandType = CommandType.StoredProcedure;
                    StrPrc5.Parameters.AddWithValue("@cb", richTextBox1.Text);
                    StrPrc5.Parameters.AddWithValue("@FormCB", "Diag");
                    StrPrc5.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc5.ExecuteNonQuery();

                    GetSelectZaklStage();
                    CreateInfo("Данные успешно добавлены!", "lime", panel1);
                    break;

                case "VernOtv":

                    string VernOtvStage = "";

                    switch (comboBox2.SelectedItem)
                    {
                        case "Феноменология":
                            VernOtvStage = "Fenom";
                            break;
                        case "Гипотезы":
                            VernOtvStage = "Teor";
                            break;
                        case "Диагноз":
                            VernOtvStage = "Diag";
                            break;
                    }

                    SqlCommand StrPrc8 = new SqlCommand("vernotv_add", con);
                    StrPrc8.CommandType = CommandType.StoredProcedure;
                    StrPrc8.Parameters.AddWithValue("@otv", richTextBox1.Text);
                    StrPrc8.Parameters.AddWithValue("@FormVernOtv", VernOtvStage);
                    StrPrc8.Parameters.AddWithValue("@zadacha_id", SelectedZadacha);
                    StrPrc8.ExecuteNonQuery();

                    GetSelectVernOtvStage();
                    CreateInfo("Данные успешно добавлены!", "lime",panel1);
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
            label.AutoSize = true;
            label.Font = new Font(label.Font.FontFamily, 14);
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
            datagr1.Size = new Size(panel1.Width / 2, panel1.Height / 2);
            datagr1.Location = new Point(panel1.Width / 50, panel1.Height / 4);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectInfoFromDatagr;
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            panel1.Controls.Add(datagr1);

            datagr1.BackgroundColor = Color.White;
            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            datagr1.EnableHeadersVisualStyles = false;

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.Visible = false;

            con.Close();

            FormAlignment();
        }


        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
                panel1.Width = 1000;

                button3.Font = new Font(button3.Font.FontFamily, 12);
                button3.Width = 100;
                button3.Left = 50;

                button6.Font = new Font(button6.Font.FontFamily, 12);
                button6.Width = 100;
                button6.Left = button3.Left + button3.Width+10;

                button7.Font = new Font(button7.Font.FontFamily, 12);
                button7.Width = 100;
                button7.Left = button6.Left + button6.Width + 10;

                button10.Font = new Font(button10.Font.FontFamily, 12);
                button10.Width = 100;
                button10.Left = button7.Left + button7.Width + 10;

                button13.Font = new Font(button13.Font.FontFamily, 12);
                button13.Width = 100;
                button13.Location = new Point(panel1.Width/2-button13.Width/2, button6.Location.Y + 40);

                button11.Font = new Font(button11.Font.FontFamily, 12);
                button11.Width = 100;
                button11.Location = new Point(button13.Location.X-button11.Width-10,button3.Location.Y+40);

                button8.Font = new Font(button8.Font.FontFamily, 12);
                button8.Width = 100;
                button8.Location = new Point(button13.Location.X+button13.Width+10, button10.Location.Y + 40);

                datagr1.Width = panel1.Width / 2;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label2.Left = panel1.Width / 2 - label2.Width / 2 - comboBox1.Width/2;
            (panel1.Controls["label"] as Label).Location = new Point(panel1.Width / 2 - (panel1.Controls["label"] as Label).Width / 2, label2.Location.Y - 50);

            comboBox1.Location = new Point(label2.Location.X + label2.Width + 5, label2.Location.Y+2);

            button1.Left = panel1.Width / 2 - button1.Width / 2;

            button2.Location = new Point(30, panel1.Height - 70);
            button4.Location = new Point(30, panel1.Height - 70);
            button5.Location = new Point(panel1.Width - button5.Width - 30, panel1.Height - 70);
            button12.Location = new Point(button5.Location.X - button12.Width - 10, panel1.Height - 70);
            button14.Location = new Point(button12.Location.X - button14.Width - 10, panel1.Height - 70);
            button15.Location = new Point(button5.Location.X, button5.Location.Y-button15.Height - 10);
            button16.Location = new Point(button15.Location.X-button16.Width-10, button15.Location.Y);
            textBox6.Location = new Point(button14.Location.X, button15.Location.Y + 17);
            textBox6.Width = button14.Width;
            label9.Location = new Point(button14.Location.X, button15.Location.Y);
        }

        private void CleanRichTextBox(object sender, EventArgs e)
        {
            UserInfoClean();
        }

        private void UserInfoClean()
        {
            switch (SelectedStage)
            {
                case "Fenom1":
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    break;

                case "Fenom2":
                    richTextBox1.Text = "";
                    break;

                case "Gip":
                    richTextBox1.Text = "";
                    break;

                case "Dpo":
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    richTextBox3.Text = "";
                    richTextBox4.Text = "";
                    richTextBox5.Text = "";
                    break;

                case "Zakl":
                    richTextBox1.Text = "";
                    break;

                case "Meropr":
                    richTextBox1.Text = "";
                    break;

                case "Katamnez":
                    richTextBox1.Text = "";
                    break;

                case "VernOtv":
                    richTextBox1.Text = "";
                    break;
            }
        }

        private void InfoFinder(object sender, EventArgs e)
        {
            switch (SelectedStage)
            {
                case "Fenom1":
                    GetSelectFenom1Stage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "От кого сведения";
                    datagr1.Columns[2].HeaderText = "Сведения";
                    richTextBox1.MaxLength = 50;
                    break;

                case "Fenom2":
                    GetSelectFenom2Stage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                case "Gip":
                    GetSelectGipStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                case "Dpo":
                    GetSelectDpoStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = true;
                    datagr1.Columns[4].Visible = true;
                    datagr1.Columns[5].Visible = true;
                    datagr1.Columns[6].Visible = false;
                    datagr1.Columns[1].HeaderText = "Краткое наименование методики";
                    datagr1.Columns[2].HeaderText = "Полное наименование методики";
                    datagr1.Columns[3].HeaderText = "Данные";
                    datagr1.Columns[4].HeaderText = "Путь к файлу с 1 рисунком";
                    datagr1.Columns[5].HeaderText = "Путь к файлу со 2 рисунком";
                    richTextBox1.MaxLength = 100;
                    break;

                case "Zakl":
                    GetSelectZaklStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = false;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    richTextBox1.MaxLength = 2147483647;
                    break;

                case "ZadachaInfo":
                    GetSelectZadachaInfo();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = true;
                    datagr1.Columns[4].Visible = true;
                    datagr1.Columns[1].HeaderText = "Запрос";
                    datagr1.Columns[2].HeaderText = "Сведения";
                    datagr1.Columns[3].HeaderText = "Данные по мероприятиям";
                    datagr1.Columns[4].HeaderText = "Данные по катамнезу";
                    button12.Enabled = false;
                    button14.Enabled = false;
                    richTextBox1.MaxLength = 2147483647;
                    break;

                case "VernOtv":
                    GetSelectVernOtvStage();
                    datagr1.Columns[0].Visible = false;
                    datagr1.Columns[1].Visible = true;
                    datagr1.Columns[2].Visible = true;
                    datagr1.Columns[3].Visible = false;
                    datagr1.Columns[1].HeaderText = "Вариант ответа";
                    datagr1.Columns[2].HeaderText = "Этап";
                    break;
            }

            int Find;

            datagr1.CurrentCell = null;

            for (int x = 0; x < datagr1.Rows.Count; x++)
            {
                Find = 0;

                for (int y = 0; y < datagr1.ColumnCount; y++)
                {
                    if (datagr1.Rows[x].Cells[y].Value.ToString().Contains(textBox6.Text))
                    {
                        Find = 1;
                    }
                }

                if (Find != 1)
                {
                    datagr1.Rows[x].Visible = false;
                }
            }
        }

        public void CreateInfo(string labelinfo, string color, Panel MainPanel)
        {
            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 5000;
            timer.Start();

            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.BringToFront();

            switch (color)
            {
                case "red":
                    label.ForeColor = Color.Red;
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    break;
                default:
                    label.ForeColor = Color.Black;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                (sender as Timer).Stop();
            }catch { }
        }
    }
}
