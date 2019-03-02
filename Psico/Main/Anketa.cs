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

            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
            }

            // Позиционирование элементов формы пользователя
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;
        }

        private void FIOHint(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text == "ФИО")
            {
                textBox1.Text = "";
            }
        }

        private void StudyHint(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text == "Образование")
            {
                textBox2.Text = "";
            }
        }

        private void WorkHint(object sender, KeyPressEventArgs e)
        {
            if (textBox3.Text == "Место работы и стаж")
            {
                textBox3.Text = "";
            }
        }

        private void YearHint(object sender, KeyPressEventArgs e)
        {
            if (textBox4.Text == "Год обучения")
            {
                textBox4.Text = "";
            }
        }

        private void OldHint(object sender, KeyPressEventArgs e)
        {
            if (textBox5.Text == "Возраст")
            {
                textBox5.Text = "";
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
            if (
                (textBox1.Text != "") && (textBox1.Text != "ФИО") &&
                (textBox2.Text != "") && (textBox2.Text != "Образование") &&
                (textBox3.Text != "") && (textBox3.Text != "Место работы и стаж") &&
                (textBox4.Text != "") && (textBox4.Text != "Год обучения") &&
                (textBox5.Text != "") && (textBox5.Text != "Возраст")
                )
            {
                // Присвоение переменным, заполенными данными
                Program.FIO = textBox1.Text;
                Program.Study = textBox2.Text;
                Program.Work = textBox3.Text;
                Program.Year = textBox4.Text;
                Program.Old = textBox5.Text;

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
                    Program.doc = "C:\\Protokol\\" + Program.FIO + "   " + date + "   " 
                        + timeProtokol + ".docx";
                    wordDocument.SaveAs2(Program.doc); 
                    wordApp.Quit();

                    Vstuplenie vstuplenie = new Vstuplenie();
                    vstuplenie.Show();
                    Close();
                }

                catch

                { 
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    wordApp.Quit();
                }
            }

            else
            {
                MessageBox.Show("Не все поля заполнены!");
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            // Выход из программы
            Application.Exit(); 
        }
    }
}
