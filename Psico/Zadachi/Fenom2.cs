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
        SqlConnection con = DBUtils.GetDBConnection(); // Класс подключения к БД
        int kolvoCb; // Количество checkbox на форме
        int stolb = 0; // Переменная необходимая для разделения checkbox на 2 столбца
        int kolvovibor = 0; // Количество выбранных ответов
        DataGridView datagr = new DataGridView(); // Создание datagridview
        DataGridView datagr1 = new DataGridView(); // Создание datagridview
        WordInsert wordinsert = new WordInsert(); // Класс записи данных в ворд документ

        public Fenom2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.diagnoz == 3) // Если Диагноз верный
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения

                if (result == DialogResult.OK) // Если пользователь нажал ОК
                {

                    // Запись данных о решении задачи пользователем в БД
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.Fenom2T;
                        Program.Insert = "Время на феноменологии 2:" + Program.Fenom2T + " сек";

                        wordinsert.Ins();

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

            // Если задача не решена или диагноз неверный
            else
            {
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); // Вывод сообщения 

                if (result == DialogResult.OK) // Если пользователь нажал ОК
                {
                    // Запись данных в ворд документ
                    try
                    {

                        timer1.Enabled = false;
                        Program.AllT = Program.AllT + Program.Fenom2T;
                        Program.Insert = "Время на феноменологии 2:" + Program.Fenom2T + " сек";

                        wordinsert.Ins();

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
                Program.AllT = Program.AllT + Program.Fenom2T;
                Program.Insert = "Время на феноменологии 2:" + Program.Fenom2T + " сек";

                wordinsert.Ins();

                // Переход обратно
                Fenom1 fenom1 = new Fenom1();
                fenom1.Show();
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
            // Обновление выбранных ответов
            SqlCommand delete = new SqlCommand("delete from otvFenom", con);
            delete.ExecuteNonQuery();

            // Запись данных о выбранных checkbox
            foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
            {
                
                // Переборка checkbox по их количеству
                for (int x = 1; x < kolvoCb; x++)
                {
                    // При выборе определённого checkbox
                    if (checkBox.Name == "checkbox" + x + "")
                    {

                        if (checkBox.Checked == true)
                        {
                            // Добавление данных о решении задачи пользователем
                            SqlCommand StrPrc2 = new SqlCommand("otvFenom_add", con);
                            StrPrc2.CommandType = CommandType.StoredProcedure;
                            StrPrc2.Parameters.AddWithValue("@name_otv", checkBox.Name);
                            StrPrc2.ExecuteNonQuery();

                            // Запись данных в ворд документ
                            try
                            {
                                // Запись данных о выборе checkbox
                                Program.Insert = "Выбран: " + checkBox.Text + "";

                                wordinsert.CBIns();
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
            }

            // Запись данных в ворд документ
            try
            {

                timer1.Enabled = false;
                Program.AllT = Program.AllT + Program.Fenom2T;
                Program.Insert = "Время на феноменологии 2:" + Program.Fenom2T + " сек";

                wordinsert.Ins();

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

        private void Fenom2_Load(object sender, EventArgs e)
        {
            Program.Fenom2T = 0; // Переменная времени на форме
            timer1.Enabled = true; // Счётчик времени на форме
            richTextBox1.Text = Program.fenomenologiya; // Запись данных о резюме по феноменологии

            con.Open(); // подключение к БД

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

            // Создание таблицы с данными из БД
            datagr1.Name = "datagrview1";
            datagr1.Location = new Point(400, 400);
            SqlDataAdapter da2 = new SqlDataAdapter("select name_otv from otvFenom", con);
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
                    int newFontSize = 12; //размер
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize);

                    if (ctrl is CheckBox)
                    {
                        ctrl.Left = ctrl.Left - 130;
                        int nFontSize = 8; //размер
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
            SqlCommand kolotv = new SqlCommand("select count(*) as 'kolvo' from otvFenom", con);
            SqlDataReader dr1 = kolotv.ExecuteReader();
            dr1.Read();
            kolvovibor = Convert.ToInt32(dr1["kolvo"].ToString());
            dr1.Close();
            
            if(datagr1.Rows.Count !=0)
            {
                // выбор всех checkbox
                foreach (var checkBox in panel1.Controls.OfType<CheckBox>())
                {

                    // Переборка checkbox по их количеству
                    for (int x = 1; x < kolvoCb; x++)
                    {
                        for (int i = 0; i < kolvovibor; i++)
                        {
                            // При выборе определённого checkbox
                            if (checkBox.Name == Convert.ToString(datagr1.Rows[i].Cells[0].Value))
                            {
                                checkBox.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.Fenom2T = Program.Fenom2T + 1;
        }
    }
}
