using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    public partial class Anketa : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr = new DataGridView();
        Timer timer = new Timer();

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

            // Адаптация под разрешение экрана
            FormAlignment();

            // Заполнение datagr
            new SQL_Query().CreateDatagr("select * from users where id_user = " + Program.user + "", "users",panel2,datagr);

            // Если студент уже указывал личные данные
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
            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Открытие формы авторизации
            new Autorization().Show();

            // Закрытие этой формы
            Close();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Присвоение к переменным даты и времени
            var date = DateTime.Now.ToString("dd.MM.yyyy");
            var timeProtokol = DateTime.Now.ToString("HH.mm.ss");

            // проверка на ввод данных
            if (textBox1.Text !="" || textBox2.Text !="" || textBox4.Text != "" || textBox5.Text != "" || textBox6.Text != "")
            {
                // Присвоение к переменным, указанными данными
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

                // Создание процесса Word
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

                    // Открытие формы вступления
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
            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");

            // Выход из программы
            Application.Exit(); 
        }

        private void FormAlignment()
        {
            // Адаптация под разрешение экрана
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
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }

            // Создание таймера
            timer.Tick += Timer_Tick;
            timer.Interval = 5000;
            timer.Start();

            // Динамической создание Panel
            Panel panel = new Panel();
            panel.Name = "panel";
            panel.Size = new Size(600, 100);
            panel.Location = new Point(MainPanel.Width / 2 - panel.Width / 2, MainPanel.Height / 2 - panel.Height / 2);
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(panel);
            panel.BringToFront();

            // Динамической создание Label
            Label label = new Label();
            label.Name = "label";
            label.Text = labelinfo;
            label.Size = new Size(panel.Width, panel.Height);
            label.Font = new Font(label.Font.FontFamily, 16);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 0);
            panel.Controls.Add(label);
            label.Click += Label_Click;
            label.BringToFront();

            // Выбор цвета для шрифта сообщения
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

        private void Label_Click(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Удаление динамической созданной Panel
            try
            {
                (panel1.Controls["panel"] as Panel).Dispose();
                timer.Stop();
            }
            catch
            {
            }
        }

        private void ZapretRusAndEng(object sender, KeyPressEventArgs e)
        {
            // Разрешение на ввод только цифр
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только цифры!", "red", panel1);
            }
        }

        private void ZapretNumber(object sender, KeyPressEventArgs e)
        {
            // Запрет на ввод цифр
            if (Char.IsControl(e.KeyChar) || Char.IsLetter(e.KeyChar) || Char.IsSeparator(e.KeyChar)) return;
            else
            {
                e.Handled = true;
                CreateInfo("Возможно ввести только буквы!", "red", panel1);
            }
        }
    }
}
