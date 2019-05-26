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
            SqlDataAdapter da1 = new SqlDataAdapter("select * from users where id_user = " + Program.user + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "users");
            datagr.DataSource = ds1.Tables[0];
            panel2.Controls.Add(datagr);
            datagr.Visible = false;

            if (datagr.Rows[0].Cells[4].Value.ToString() != "")
            {
                textBox1.Text = datagr.Rows[0].Cells[4].Value.ToString();
                textBox6.Text = datagr.Rows[0].Cells[5].Value.ToString();
                textBox7.Text = datagr.Rows[0].Cells[6].Value.ToString();
                textBox2.Text = datagr.Rows[0].Cells[7].Value.ToString();
                textBox3.Text = datagr.Rows[0].Cells[8].Value.ToString();
                textBox4.Text = datagr.Rows[0].Cells[9].Value.ToString();
                textBox5.Text = datagr.Rows[0].Cells[10].Value.ToString();
            }
        }

        private void OpenFormAutorization(object sender, EventArgs e)
        {
            new ExitProgram().UpdateUserStatus();
            new Autorization().Show();
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Присвоение переменным даты и времени
            var date = DateTime.Now.ToString("dd.MM.yyyy");
            var timeProtokol = DateTime.Now.ToString("HH.mm.ss");

            // проверка на заполнение данных
            if (textBox1.Text !="" || textBox2.Text !="" || textBox4.Text != "" || textBox5.Text != "" || textBox6.Text != "")
            {
                // Присвоение переменным, заполенными данными
                Program.FIO = textBox1.Text +" "+ textBox6.Text +" "+ textBox7.Text;
                Program.Study = textBox2.Text;
                Program.Work = textBox3.Text;
                Program.Year = textBox4.Text;
                Program.Old = textBox5.Text;

                // Запись данных о решении задачи в БД
                SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@id_user", Program.user);
                StrPrc1.Parameters.AddWithValue("@User_Login", datagr.Rows[0].Cells[1].Value.ToString());
                StrPrc1.Parameters.AddWithValue("@User_Password", datagr.Rows[0].Cells[2].Value.ToString());
                StrPrc1.Parameters.AddWithValue("@User_Mail", datagr.Rows[0].Cells[3].Value.ToString());
                StrPrc1.Parameters.AddWithValue("@Fam", textBox1.Text);
                StrPrc1.Parameters.AddWithValue("@Imya", textBox6.Text);
                StrPrc1.Parameters.AddWithValue("@Otch", textBox7.Text);
                StrPrc1.Parameters.AddWithValue("@Study", Program.Study);
                StrPrc1.Parameters.AddWithValue("@Work", Program.Work);
                StrPrc1.Parameters.AddWithValue("@Year", Program.Year);
                StrPrc1.Parameters.AddWithValue("@Old", Program.Old);
                StrPrc1.Parameters.AddWithValue("@UserStatus", 1);
                StrPrc1.Parameters.AddWithValue("@Teacher_id", Convert.ToInt16(datagr.Rows[0].Cells[12].Value));
                StrPrc1.ExecuteNonQuery();

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
                    CreateInfo("Отсутствует шаблон протокола! Обратитесь к администратору.", "red", panel1);
                    wordApp.Quit();
                }
            }

            else
            {
                CreateInfo("Необходимо заполнить все поля для дальнейшей работы!","red", panel1);
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            new ExitProgram().UpdateUserStatus();

            // Выход из программы
            Application.Exit(); 
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
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
            }
            catch { }
        }

        private void ZapretRusAndEng(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только цифры!", "red", panel1);
            }
        }

        private void ZapretNumber(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) || Char.IsLetter(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только буквы!", "red", panel1);
            }
        }
    }
}
