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
using word = Microsoft.Office.Interop.Word;
using InsertWord;
using System.Threading;

namespace Psico
{
    public partial class Fenom2 : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        DataGridView datagr2 = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        int kolvoCb;
        int kolvootv;
        int stolb = 0;
        int kolvovibor = 0;

        public Fenom2()
        {
            InitializeComponent();
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
                    GetCBChecked();

                    // Запись данных о решении задачи пользователем в БД
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    ExitFromProgram();
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
                    GetCBChecked();

                    ExitFromProgram();
                }
            }
        }

        private void OpenPreviousForm(object sender, EventArgs e)
        {
            GetCBChecked();

            ExitFromThisForm();

            Fenom1 fenom1 = new Fenom1();
            fenom1.Show();
            Close();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            GetCBChecked();

            ExitFromThisForm();

            Program.Insert = "Время общее на этапе феноменологии:" + Program.AllFenom + " сек";
            wordinsert.Ins();

            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.Fenom2T = 0;
            timer1.Enabled = true;
            richTextBox1.Text = Program.fenomenologiya;

            // Открытие подключения
            con.Open();

            // Выбор данных из БД
            SqlCommand Zaprosi = new SqlCommand("select Zapros, sved from zadacha where id_zadacha = " + Program.NomerZadachi + "", con);
            SqlDataReader dr = Zaprosi.ExecuteReader();
            dr.Read();
            label3.Text = dr["Zapros"].ToString();
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + dr["sved"].ToString() + "";
            dr.Close();

            // Определение количества checkbox на форме
            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from fenom2 where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoCb = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoCb = kolvoCb + 1;

            // Определение переменной для равномерных колонок с checkbox
            stolb = kolvoCb / 2;

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from vernotv_Fenom where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvootv = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();

            // Создание таблицы с данными из БД
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

            // Динамическое создание таблицы
            datagr2.Name = "datagrview2";
            SqlDataAdapter da3 = new SqlDataAdapter("select otv from vernotv_Fenom where zadacha_id = " + Program.NomerZadachi + "", con);
            SqlCommandBuilder cb3 = new SqlCommandBuilder(da3);
            DataSet ds3 = new DataSet();
            da3.Fill(ds3, "vernotv_Fenom");
            datagr2.DataSource = ds3.Tables[0];
            datagr2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr2);
            datagr2.Visible = false;

            // Создание таблицы с данными из БД
            datagr1.Name = "datagrview1";
            datagr1.Location = new Point(400, 400);
            SqlDataAdapter da2 = new SqlDataAdapter("select name_otv from otvFenom where users_id = " + Program.user + "", con);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "otvFenom");
            datagr1.DataSource = ds2.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Visible = false;

            //Динамическое создание checkbox
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
                    x = 700;
                    y = 246;
                }
            }

            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;

                panel2.Width = 1024;
                panel2.Height = 768;

                panel1.Width = 1003;
                panel1.Height = 747;

                label3.MaximumSize = new Size(950, 64);

                button3.Left = button3.Left - 350;
                button1.Left = button1.Left - 340;
                label4.Left = label4.Left - 170;
                richTextBox1.Left = richTextBox1.Left - 170;

                foreach (Control ctrl in panel1.Controls)
                {
                    int newFontSize = 12;
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize);

                    if (ctrl is CheckBox)
                    {
                        ctrl.Left = ctrl.Left - 130;
                        int nFontSize = 8;
                        ctrl.Font = new Font(ctrl.Font.FontFamily, nFontSize);
                    }
                }
            }

            // Позиционирование элементов формы пользователя
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.Left = panel1.Width / 2 - label3.Width / 2;
            label3.MaximumSize = new Size(1300, 64);
            label3.AutoSize = true;

            // Выбор количества правильных ответов у задачи
            SqlCommand kolotvetov = new SqlCommand("select count(*) as 'kolvo' from otvFenom where users_id = " + Program.user + "", con);
            SqlDataReader dr3 = kolotvetov.ExecuteReader();
            dr3.Read();
            kolvovibor = Convert.ToInt32(dr3["kolvo"].ToString());
            dr3.Close();
            
            // Выбор checkbox которые были выбраны в предыдущий раз на форме
            if(datagr1.Rows.Count !=0)
            {
                foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
                {
                    for (int x = 1; x < kolvoCb; x++)
                    {
                        for (int i = 0; i < kolvovibor; i++)
                        {
                            if (checkBox.Name == Convert.ToString(datagr1.Rows[i].Cells[0].Value))
                            {
                                checkBox.Checked = true;
                            }
                        }
                    }
                }
            }

            // Запись данных в протокол
            Program.Insert = "Окно - Феноменология (Машинный выбор):";
            wordinsert.Ins();
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.Fenom2T = Program.Fenom2T + 1;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                Program.AllTBezK = Program.AllTBezK + Program.Fenom2T;
            }
        }

        private void ExitFromThisForm()
        {
            // Запись данных в протокол
            Program.Insert = "Правильных ответов: " + Program.zaklOTV + " из " + kolvootv + "";
            wordinsert.CBIns();

            timer1.Enabled = false;
            Program.AllT = Program.AllT + Program.Fenom2T;
            Program.AllFenom = Program.Fenom2T + Program.AllFenom;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Время на феноменологии (Машинный выбор):" + Program.Fenom2T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            ExitFromThisForm();

            Program.Insert = "Время общее на этапе феноменологии:" + Program.AllFenom + " сек";
            wordinsert.Ins();

            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            exitProgram.ExProgr();

            exitProgram.ProtokolSent();

            Application.Exit();
        }

        private void GetCBChecked()
        {
            // Обнуление выбранных ответов пользователем
            SqlCommand delete2 = new SqlCommand("delete from otvFenom where users_id = " + Program.user + "", con);
            delete2.ExecuteNonQuery();

            int otvchek = 0;
            Program.zaklOTV = 0;

            // Перебор всех checkbox
            for (int i = 1; i < kolvoCb; i++)
            {
                // Если checkbox выбран
                if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Checked == true)
                {
                    otvchek = 0;

                    // Перебор всех правильных ответов у задачи
                    for (int a = 0; a < kolvootv; a++)
                    {
                        // Если выбранный checkbox правильный 
                        if ((panel1.Controls["checkbox" + i + ""] as CheckBox).Text == Convert.ToString(datagr2.Rows[a].Cells[0].Value))
                        {
                            // Запись данных в протокол
                            Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ✔";
                            wordinsert.CBIns();

                            Program.zaklOTV = Program.zaklOTV + 1;
                            otvchek = 1;
                        }
                    }

                    if (otvchek != 1)
                    {
                        // Запись данных в протокол
                        Program.Insert = "Выбран: " + (panel1.Controls["checkbox" + i + ""] as CheckBox).Text + " ☒";
                        wordinsert.CBIns();
                    }

                    // Добавление данных о решении задачи пользователем
                    SqlCommand StrPrc2 = new SqlCommand("otvFenom_add", con);
                    StrPrc2.CommandType = CommandType.StoredProcedure;
                    StrPrc2.Parameters.AddWithValue("@name_otv", (panel1.Controls["checkbox" + i + ""] as CheckBox).Name);
                    StrPrc2.Parameters.AddWithValue("@user_id", Program.user);
                    StrPrc2.ExecuteNonQuery();
                }
            }
        }
    }
}
