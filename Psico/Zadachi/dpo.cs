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
using InsertWord;
using System.Threading;

namespace Psico
{
    public partial class dpo : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        int AllProsmotrMerodiks;
        int kolvoProsmotrMetodik;
        int kolvolb;
        int kolvotext;
        string smalltext;

        public dpo()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.dpoT = 0;
            Program.infochekT = 0;
            timer1.Enabled = true;
            timer2.Enabled = false;
            richTextBox1.Text = Program.gipotezi;

            // подключение к БД
            con.Open();

            // Запись данных на форму из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Выбор количества данных, необходимых для записи в listbox
            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from dpo where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvolb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvolb = kolvolb + 1;

            // Выбор количества данных в таблице БД
            SqlCommand kolvoProsmotr = new SqlCommand("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'", con);
            SqlDataReader dr1 = kolvoProsmotr.ExecuteReader();
            dr1.Read();
            AllProsmotrMerodiks = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            // Динамическое создание таблицы
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

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Location = new Point(300, 300);
            SqlDataAdapter da2 = new SqlDataAdapter("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "OtvSelected");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            // Динамическое создание label
            Label lb = new Label();
            lb.Name = "label";
            lb.Location = new Point(636,122);
            lb.AutoSize = true;
            panel1.Controls.Add(lb);

            // Динамическое создание richtextbox
            RichTextBox rtb = new RichTextBox();
            rtb.Name = "richtextbox";
            rtb.Location = new Point(636, 180);
            rtb.Width = 680;
            panel1.Controls.Add(rtb);
            rtb.ShortcutsEnabled = false;
            rtb.ReadOnly = true;

            // Если в задаче есть рисунки
            if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
            {
                rtb.Height = 200;

                // Динамическое создание picturebox
                PictureBox pb = new PictureBox();
                pb.Name = "picturebox";
                pb.Location = new Point(636, 400);
                pb.Width = 680;
                pb.Height = 250;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Enabled = false;
                pb.Click += PbClick;
                panel1.Controls.Add(pb);
                pb.Cursor = Cursors.SizeNESW;

                // Динамическое создание label
                Label label = new Label();
                label.Name = "label";
                label.Text = "Для увеличения или уменьшения рисунка нажмите левую кнопку мыши";
                label.Location = new Point(636,660);
                label.AutoSize = true;
                panel1.Controls.Add(label);
            }

            // Если рисунков в задаче нет
            else rtb.Height = 500;

            //Запись данных из БД в listbox
            for (int i = 1; i < kolvolb; i++)
            {
                smalltext = Convert.ToString(datagr.Rows[i - 1].Cells[0].Value);

                if (smalltext != "")
                {
                    listBox1.Items.Add(Convert.ToString(datagr.Rows[i - 1].Cells[0].Value));
                }

                else listBox1.Items.Add(Convert.ToString(datagr.Rows[i - 1].Cells[1].Value));
            }

            // Запись данных в протокол
            Program.Insert = "Окно - Обследование: ";
            wordinsert.Ins();
        }

        private void PbClick(object sender, EventArgs e)
        {
            // Изменение размера рисунка
            if (((panel1.Controls["picturebox"] as PictureBox).Width == 680)&& ((panel1.Controls["picturebox"] as PictureBox).Height == 250))
            {
                // Увеличить
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(0,0);
                (panel1.Controls["picturebox"] as PictureBox).Width = 1345;
                (panel1.Controls["picturebox"] as PictureBox).Height = 740;
                (panel1.Controls["picturebox"] as PictureBox).BringToFront();
            }

            else
            {
                // Уменьшить
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(636,400);
                (panel1.Controls["picturebox"] as PictureBox).Width = 680;
                (panel1.Controls["picturebox"] as PictureBox).Height = 250;
            }
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о решении задачи в БД
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    ExitFromThisForm();

                    exitProgram.ExProgr();

                    exitProgram.ExProtokolSent();

                    Application.Exit();
                }
            }

            // Если задача не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    ExitFromThisForm();

                    exitProgram.ExProgr();

                    exitProgram.ExProtokolSent();

                    Application.Exit();
                }
            }
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            ExitFromThisForm();

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void ShowListBoxInfo(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {  
                int checkProsmotrmetodik = 0;

                if (Program.infochekT != 0)
                {
                    // Запись данных в протокол
                    Program.Insert = "Время просмотра: " + Program.infochekT + " сек";
                    wordinsert.Ins();

                    kolvoProsmotrMetodik = kolvoProsmotrMetodik + 1;
                    Program.infochekT = 0;
                }
                else
                {
                    timer2.Enabled = true;
                }

                // Если в задаче есть рисунки
                if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
                {
                    (panel1.Controls["picturebox"] as PictureBox).Enabled = true;
                }

                // Запись данных в протокол
                Program.Insert = "Просмотрено: " + listBox1.SelectedItem.ToString() + "";
                wordinsert.CBIns();

                if (datagr1.Rows.Count > 1)
                {
                    for (int y = 0; y < AllProsmotrMerodiks; y++)
                    {
                        if (listBox1.SelectedItem.ToString() == datagr1.Rows[y].Cells[0].Value.ToString())
                        {
                            checkProsmotrmetodik = 1;
                        }
                    }
                }

                if (checkProsmotrmetodik == 0)
                {
                    // Запись данных в БД
                    SqlCommand StrPrc1 = new SqlCommand("OtvSelected_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@InfoSelected", listBox1.SelectedItem.ToString());
                    StrPrc1.Parameters.AddWithValue("@FormOtvSelected", "Dpo");
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.ExecuteNonQuery();

                    SqlDataAdapter da2 = new SqlDataAdapter("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'", con);
                    SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2, "OtvSelected");
                    datagr1.DataSource = ds2.Tables[0];

                    // Выбор количества данных в таблице БД
                    SqlCommand kolvoProsmotr = new SqlCommand("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'", con);
                    SqlDataReader dr1 = kolvoProsmotr.ExecuteReader();
                    dr1.Read();
                    AllProsmotrMerodiks = Convert.ToInt32(dr1["kolvo"].ToString());
                    dr1.Close();
                }

                // При выборе данных в listbox
                for (int i = 1; i < kolvolb; i++)
                {
                    if (listBox1.SelectedIndex == i - 1)
                    {
                        // Если в таблице есть рисунок
                        if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
                        {
                            (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[3].Value + ""));
                        }

                        // Запись данных в richtextbox
                        panel1.Controls["richtextbox"].Text = Convert.ToString(datagr.Rows[i - 1].Cells[2].Value);

                        // Запись данных в label
                        panel1.Controls["label"].Text = Convert.ToString(datagr.Rows[i - 1].Cells[1].Value);
                        kolvotext = panel1.Controls["label"].Text.Length;

                        // Если название длинное, то перенос на следующую строчку
                        if (kolvotext > 70)
                        {
                            (panel1.Controls["label"] as Label).AutoSize = false;
                            (panel1.Controls["label"] as Label).Width = 680;
                            (panel1.Controls["label"] as Label).Height = 40;
                        }

                        button4.Visible = false;

                        // Если есть ещё рисунок, то показ дополнительной кнопки
                        if (Convert.ToString(datagr.Rows[i - 1].Cells[4].Value) != "")
                        {
                            button5.Visible = true;
                        }

                        else button5.Visible = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали данные, которые хотите посмотреть, они представленны выше!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void OpenNextPictureBox(object sender, EventArgs e)
        {
            // Смена рисунков
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

        private void ListBoxItemChanged(object sender, EventArgs e)
        {
            button5.Visible = false;
            button4.Visible = true;
        }

        private void TimeOnForm(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.dpoT = Program.dpoT + 1;
        }

        private void TimeOnSeeListboxInfo(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.infochekT = Program.infochekT + 1;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                Program.AllTBezK = Program.AllTBezK + Program.dpoT;
            }
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.dpoT;
            Program.FullAllDpo = Program.FullAllDpo + Program.dpoT;
            Program.obsledovaniya = richTextBox2.Text;

            if (Program.infochekT != 0)
            {
                // Запись данных в протокол
                Program.Insert = "Время просмотра: " + Program.infochekT + " сек";
                wordinsert.Ins();

                kolvoProsmotrMetodik = kolvoProsmotrMetodik + 1;
                Program.infochekT = 0;
            }

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Данные по обследованиям методик: " + Program.obsledovaniya + "";
            wordinsert.Ins();
            Program.Insert = "Количество просмотренных методик: " + kolvoProsmotrMetodik + "";
            wordinsert.Ins();
            Program.Insert = "Время на обследовании методик: " + Program.dpoT + " сек";
            wordinsert.Ins();
        }
    }
}
