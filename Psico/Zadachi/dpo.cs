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
        SqlConnection con = DBUtils.GetDBConnection(); // Подключение к БД
        int kolvolb; // Количество данных в listbox
        DataGridView datagr = new DataGridView(); // Создание таблицы
        int kolvotext; // Длина названия в listbox
        string smalltext; // Уменьшение длина названия в listbox
        WordInsert wordinsert = new WordInsert(); // Запись в ворд документ

        public dpo()
        {
            InitializeComponent();
        }

        private void dpo_Load(object sender, EventArgs e)
        {
            Program.dpoT = 0; // Переменная времени на форме
            timer1.Enabled = true; // Счётчик времени на форме
            richTextBox1.Text = Program.gipotezi;

            con.Open(); // подключение к БД

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
            rtb.ReadOnly = true;

            // Если в задаче есть рисунки
            if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
            {
                // Уменьшение richtextbox
                rtb.Height = 200;

                // Динамическое создание picturebox
                PictureBox pb = new PictureBox();
                pb.Name = "picturebox";
                pb.Location = new Point(636, 400);
                pb.Width = 680;
                pb.Height = 250;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Click += pb_click;
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
            else rtb.Height = 500; // Увеличение richtextbox

            //Запись данных из таблицы БД в listbox
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
            // Изменение размера рисунка
            if (((panel1.Controls["picturebox"] as PictureBox).Width == 680)&& ((panel1.Controls["picturebox"] as PictureBox).Height == 250))// Если маленький
            {
                // Увеличить
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(0,0);
                (panel1.Controls["picturebox"] as PictureBox).Width = 1345;
                (panel1.Controls["picturebox"] as PictureBox).Height = 740;
                (panel1.Controls["picturebox"] as PictureBox).BringToFront();
            }

            // Если большой
            else
            {
                // Уменьшить
                (panel1.Controls["picturebox"] as PictureBox).Location = new Point(636,400);
                (panel1.Controls["picturebox"] as PictureBox).Width = 680;
                (panel1.Controls["picturebox"] as PictureBox).Height = 250;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Если задача решена
            if (Program.diagnoz == 3)
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о решении задачи в БД
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.dpoT;
                        Program.obsledovaniya = richTextBox2.Text;

                        Program.Insert = "Время на обследовании методик: " + Program.dpoT + " сек";

                        wordinsert.Ins();

                        if (Program.obsledovaniya !="")
                        {
                            Program.Insert = "Данные по обследованиям методик: " + Program.obsledovaniya + "";

                            wordinsert.Ins();
                        }

                        // Выход из программы
                        Application.Exit();
                    }

                    // При возникновении ошибки при записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }

            // Если задача не решена
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.dpoT;
                        Program.obsledovaniya = richTextBox2.Text;

                        Program.Insert = "Время на обследовании методик: " + Program.dpoT + " сек";

                        wordinsert.Ins();

                        if (Program.obsledovaniya != "")
                        {
                            Program.Insert = "Данные по обследованиям методик: " + Program.obsledovaniya + "";

                            wordinsert.Ins();
                        }

                        // Выход из программы
                        Application.Exit();
                    }

                    // При возникновении ошибки при записи данных в ворд документ
                    catch
                    {
                        MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.dpoT;
                Program.obsledovaniya = richTextBox2.Text;

                Program.Insert = "Время на обследовании методик: " + Program.dpoT + " сек";

                wordinsert.Ins();

                if (Program.obsledovaniya != "")
                {
                    Program.Insert = "Данные по обследованиям методик: " + Program.obsledovaniya + "";

                    wordinsert.Ins();
                }

                // Переход на главную форму задачи
                Zadacha zadacha = new Zadacha();
                zadacha.Show();
                Close();
            }

            // При возникновении ошибки при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.dpoT;
                Program.obsledovaniya = richTextBox2.Text;

                Program.Insert = "Время на обследовании методик: " + Program.dpoT + " сек";

                wordinsert.Ins();

                if (Program.obsledovaniya != "")
                {
                    Program.Insert = "Данные по обследованиям методик: " + Program.obsledovaniya + "";

                    wordinsert.Ins();
                }

                // Переход на главную форму задачи
                Zadacha zadacha = new Zadacha();
                zadacha.Show();
                Close();
            }

            // При возникновении ошибки при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            // Запись данных в ворд документ
            try
            {

                if (listBox1.SelectedItem.ToString() != "")
                {
                    Program.Insert = "Просмотрено: " + listBox1.SelectedItem.ToString() + "";

                    wordinsert.CBIns();
                }
            }

            // При возникновении ошибки при записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }

            // Перебор всех данных в listbox'е
            for (int i = 1; i < kolvolb; i++)
            {

                if (listBox1.SelectedIndex == i-1)
                {
                    // Если в таблице есть рисунок
                    if (Convert.ToString(datagr.Rows[1].Cells[3].Value) != "")
                    {
                        //загрузка рисунок
                        (panel1.Controls["picturebox"] as PictureBox).Load("" + Convert.ToString(datagr.Rows[i - 1].Cells[3].Value + ""));
                    }

                    // Запись данных в richtextbox
                    panel1.Controls["richtextbox"].Text = Convert.ToString(datagr.Rows[i-1].Cells[2].Value);

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

                    // Если есть ещё рисунок, то показ дополнительной кнопки
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.dpoT = Program.dpoT + 1;
        }
    }
}
