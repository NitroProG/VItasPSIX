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

namespace Psico
{
    public partial class Anketa : Form
    {

        public Anketa()
        {
            InitializeComponent();
        }

        private void ReplaceWord(string stubToReplace, string text, word.Document worddocument)
        {
            // замена переменных в ворд документе
            var range = worddocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void Anketa_Load(object sender, EventArgs e)
        {
            // Заполнение подсказок 
            richTextBox1.Text = "ФИО";
            richTextBox2.Text = "Образование";
            richTextBox3.Text = "Место работы и стаж";
            richTextBox4.Text = "Год обучения";
            richTextBox5.Text = "Возраст";

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

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Удаление подсказки
            if (richTextBox1.Text == "ФИО")
            {
                richTextBox1.Text = "";
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Удаление подсказки
            if (richTextBox2.Text == "Образование")
            {
                richTextBox2.Text = "";
            }
        }

        private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Удаление подсказки
            if (richTextBox3.Text == "Место работы и стаж")
            {
                richTextBox3.Text = "";
            }
        }

        private void richTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Удаление подсказки
            if (richTextBox4.Text == "Год обучения")
            {
                richTextBox4.Text = "";
            }
        }

        private void richTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Удаление подсказки
            if (richTextBox5.Text == "Возраст")
            {
                richTextBox5.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Открытие формы авторизации
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now.ToString("dd.MM.yyyy"); // Дата 
            var timeProtokol = DateTime.Now.ToString("hh.mm.ss"); // Время

            // проверка на заполнение данных
            if (
                (richTextBox1.Text != "") && (richTextBox1.Text != "ФИО") &&
                (richTextBox2.Text != "") && (richTextBox2.Text != "Образование") &&
                (richTextBox3.Text != "") && (richTextBox3.Text != "Место работы и стаж") &&
                (richTextBox4.Text != "") && (richTextBox4.Text != "Год обучения") &&
                (richTextBox5.Text != "") && (richTextBox5.Text != "Возраст")
                )
            {
                // Присвоение переменным, заполенными данными
                Program.FIO = richTextBox1.Text;
                Program.Study = richTextBox2.Text;
                Program.Work = richTextBox3.Text;
                Program.Year = richTextBox4.Text;
                Program.Old = richTextBox5.Text;

                // Подключение процесса ворд
                var wordApp = new word.Application();

                try
                {
                    // Открытие ворд документа
                    var wordDocument = wordApp.Documents.Open("\\Protokol.docx");

                    // Замена данных в ворд документе
                    ReplaceWord("{date}", Convert.ToString(date), wordDocument);
                    ReplaceWord("{timeProtokol}", Convert.ToString(timeProtokol), wordDocument);
                    ReplaceWord("{FIO}", Convert.ToString(Program.FIO), wordDocument);
                    ReplaceWord("{Study}", Convert.ToString(Program.Study), wordDocument);
                    ReplaceWord("{Work}", Convert.ToString(Program.Work), wordDocument);
                    ReplaceWord("{Year}", Convert.ToString(Program.Year), wordDocument);
                    ReplaceWord("{Old}", Convert.ToString(Program.Old), wordDocument);

                    Program.doc = "C:\\Users\\Batya\\Desktop\\" + Program.FIO + "   " + date + "   " 
                        + timeProtokol + ".docx"; // Присовение переменной путь к файлу
                    wordDocument.SaveAs2(Program.doc); // Сохранение файла 
                    wordApp.Quit(); // Освобождение процесса

                    // Открытие формы вступления
                    Vstuplenie vstuplenie = new Vstuplenie();
                    vstuplenie.Show();
                    Close();
                }
                // Если возникла ошибка в замене данных в ворд документе
                catch
                {
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                    wordApp.Quit(); // Освобождение процесса ворд
                }
            }

            // Если заполнены не все поля на форме
            else
            {
                MessageBox.Show("Не все поля заполнены!"); // Вывод сообщения
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Выход из программы
        }
    }
}
