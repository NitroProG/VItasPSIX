using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class Anketa : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr = new DataGridView();

        public Anketa()
        {
            InitializeComponent();
        }

        private void ReplaceWord(string stubToReplace, string text, word.Document worddocument)
        {
            // Замена переменных в ворд документе
            var range = worddocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            FormAlignment();

            // Динамическое создание таблицы
            SqlDataAdapter da1 = new SqlDataAdapter("select * from InfoUser where users_id = " + Program.user + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "InfoUser");
            datagr.DataSource = ds1.Tables[0];
            panel2.Controls.Add(datagr);
            datagr.Visible = false;

            if (datagr.Rows.Count > 1)
            {
                textBox1.Text = datagr.Rows[0].Cells[1].Value.ToString();
                textBox6.Text = datagr.Rows[0].Cells[2].Value.ToString();
                textBox7.Text = datagr.Rows[0].Cells[3].Value.ToString();
                textBox2.Text = datagr.Rows[0].Cells[4].Value.ToString();
                textBox3.Text = datagr.Rows[0].Cells[5].Value.ToString();
                textBox4.Text = datagr.Rows[0].Cells[6].Value.ToString();
                textBox5.Text = datagr.Rows[0].Cells[7].Value.ToString();
            }
        }

        private void OpenFormAutorization(object sender, EventArgs e)
        {
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Присвоение переменным даты и времени
            var date = DateTime.Now.ToString("dd.MM.yyyy");
            var timeProtokol = DateTime.Now.ToString("HH.mm.ss");

            // проверка на заполнение данных
            if (textBox1.Text !="" && textBox2.Text !="" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "")
            {
                // Присвоение переменным, заполенными данными
                Program.FIO = textBox1.Text + textBox6.Text + textBox7.Text;
                Program.Study = textBox2.Text;
                Program.Work = textBox3.Text;
                Program.Year = textBox4.Text;
                Program.Old = textBox5.Text;

                if (datagr.Rows.Count <= 1)
                {
                    // Запись данных о решении задачи в БД
                    SqlCommand StrPrc1 = new SqlCommand("InfoUser_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Fam", textBox1.Text);
                    StrPrc1.Parameters.AddWithValue("@Imya", textBox6.Text);
                    StrPrc1.Parameters.AddWithValue("@Otch", textBox7.Text);
                    StrPrc1.Parameters.AddWithValue("@Study", Program.Study);
                    StrPrc1.Parameters.AddWithValue("@Work", Program.Work);
                    StrPrc1.Parameters.AddWithValue("@Year", Program.Year);
                    StrPrc1.Parameters.AddWithValue("@Old", Program.Old);
                    StrPrc1.Parameters.AddWithValue("@User_id", Program.user);
                    StrPrc1.ExecuteNonQuery();
                }
                else
                {
                    // Выбор количества данных в таблице БД
                    SqlCommand GetTeacherId = new SqlCommand("select id_info as 'id' from InfoUser where users_id=" + Program.user + "", con);
                    SqlDataReader dr4 = GetTeacherId.ExecuteReader();
                    dr4.Read();
                    int Infoid = Convert.ToInt32(dr4["id"].ToString());
                    dr4.Close();

                    // Изменение данных о решении задачи в БД
                    SqlCommand StrPrc1 = new SqlCommand("InfoUser_update", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@id_info", Infoid);
                    StrPrc1.Parameters.AddWithValue("@Fam", textBox1.Text);
                    StrPrc1.Parameters.AddWithValue("@Imya", textBox6.Text);
                    StrPrc1.Parameters.AddWithValue("@Otch", textBox7.Text);
                    StrPrc1.Parameters.AddWithValue("@Study", Program.Study);
                    StrPrc1.Parameters.AddWithValue("@Work", Program.Work);
                    StrPrc1.Parameters.AddWithValue("@Year", Program.Year);
                    StrPrc1.Parameters.AddWithValue("@Old", Program.Old);
                    StrPrc1.Parameters.AddWithValue("@User_id", Program.user);
                    StrPrc1.ExecuteNonQuery();
                }

                var wordApp = new word.Application();

                try
                {
                    // Открытие Word документа
                    var wordDocument = wordApp.Documents.Open("\\Protokol.docx");

                    // Замена данных в ворд документе
                    ReplaceWord("{date}", Convert.ToString(date), wordDocument);
                    ReplaceWord("{timeProtokol}", Convert.ToString(timeProtokol), wordDocument);
                    ReplaceWord("{FIO}", Convert.ToString(Program.FIO), wordDocument);
                    ReplaceWord("{Study}", Convert.ToString(Program.Study), wordDocument);
                    ReplaceWord("{Work}", Convert.ToString(Program.Work), wordDocument);
                    ReplaceWord("{Year}", Convert.ToString(Program.Year), wordDocument);
                    ReplaceWord("{Old}", Convert.ToString(Program.Old), wordDocument);

                    // Сохранение документа
                    Program.doc = "C:\\Protokol\\" + date + "   " + Program.FIO + "   " 
                        + timeProtokol + ".docx";
                    wordDocument.SaveAs2(Program.doc); 
                    wordApp.Quit();

                    Vstuplenie vstuplenie = new Vstuplenie();
                    vstuplenie.Show();
                    Close();
                }

                catch

                {
                    CreateInfo("Отсутствует шаблон протокола! Обратитесь к администратору.", "red");
                    wordApp.Quit();
                }
            }

            else
            {
                CreateInfo("Необходимо заполнить все поля для дальнейшей работы!","red");
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            // Выход из программы
            Application.Exit(); 
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void CreateInfo(string labelinfo, string color)
        {
            Timer timer = new Timer();
            timer.Tick += TimerTick;
            timer.Start();

            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(panel1.Width / 2 - panel.Width / 2, panel1.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel);
            panel.BringToFront();

            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;

            switch (color)
            {
                case "red":
                    label.ForeColor = Color.Red;
                    timer.Interval = 5000;
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    timer.Interval = 2000;
                    break;
                default:
                    label.ForeColor = Color.Black;
                    timer.Interval = 5000;
                    break;
            }

            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.BringToFront();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                (sender as Timer).Stop();
            }
            catch
            {

            }
        }

        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
        }
    }
}
