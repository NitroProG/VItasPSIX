using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class Fenom1 : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        int kolvoRb;
        DataGridView datagr = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();

        public Fenom1()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Запись в протокол
            Program.Insert = "Время общее на этапе феноменологии: " + Program.AllFenom + " сек";
            wordinsert.Ins();

            // Запись данных для графиков
            StageInfo();

            // Запись о времени
            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            // Переход на главноме меню задачи
            Zadacha zadacha = new Zadacha();
            zadacha.Show();
            Close();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена 
            if (Program.diagnoz == 3)
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Запись данных о решении задачи
                    SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
                    StrPrc1.CommandType = CommandType.StoredProcedure;
                    StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
                    StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
                    StrPrc1.ExecuteNonQuery();

                    // Выход из программы
                    ExitFromProgram();
                }
            }

            // Если задача не решена
            else
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Выход из программы
                    ExitFromProgram();
                }
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Program.Fenom1T = 0;
            timer1.Enabled = true;

            richTextBox2.Text = Program.fenomenologiya;
            richTextBox3.Text = Program.glavsved;

            // Открытие подключения к БД
            con.Open();

            // Выбор данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Выравнивание
            label1.Left = panel1.Width / 2 - label1.Width / 2;
            label3.TextAlign = ContentAlignment.TopCenter;

            // Подсчёт количества radiobutton, необходимых для заполнения формы
            kolvoRb = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) from fenom1 where zadacha_id = " + Program.NomerZadachi + "")) + 1;

            // Создание таблицы с данными из БД
            datagr.Name = "datagrview";
            datagr.Location = new Point(300,300);
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr);
            datagr.Visible = false;

            // Заполнение datagr
            new SQL_Query().UpdateDatagr("select rb, rbtext from fenom1 where zadacha_id = " + Program.NomerZadachi + "","fenom1",datagr);

            // Динамическое создание radiobutton на форме
            for (int y = 150, i = 1; i < kolvoRb; i++)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Name = "radiobutton" + i + "";
                radioButton.Text = Convert.ToString(datagr.Rows[i-1].Cells[0].Value);
                radioButton.Location = new Point(17, y);
                radioButton.AutoSize = true;
                radioButton.CheckedChanged += RbChanged;
                panel1.Controls.Add(radioButton);
                y = y + 30;
            }

            // Запись данных в протокол
            Program.Insert = "Окно - Феноменология (Свободная форма): ";
            wordinsert.Ins();

            // Адаптация под разрешение экрана
            FormAlign();
        }

        public void RbChanged(object sender, EventArgs e)
        {
            // Выбор всех radiobutton на форме
            RadioButton radiobtn = (RadioButton)sender;

            // Перебор radiobutton
            for (int x = 1; x < kolvoRb; x++)
            {
                // При выборе определённого radiobutton
                if (radiobtn.Name == "radiobutton" + x + "")
                {
                    richTextBox1.Text = Convert.ToString(datagr.Rows[x - 1].Cells[1].Value);

                    if (radiobtn.Checked == true)
                    {
                        // Запись данных о нажатии на radiobutton
                        Program.Insert = "Просмотрено: " + radiobtn.Text + "";
                        wordinsert.CBIns();
                    }
                }
            }
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Выход из окна
            ExitFromThisForm();

            // Переход на следующее окно
            Fenom2 fenom2 = new Fenom2();
            fenom2.Show();
            Close();
        }

        private void Timer(object sender, EventArgs e)
        {
            // Счётчик времени на форме
            Program.Fenom1T++;
        }

        private void TimeWithoutKatamnez()
        {
            // Если задача не решена
            if (Program.diagnoz != 3)
            {
                // Запись данных о времени
                Program.AllTBezK = Program.AllTBezK + Program.Fenom1T;
            }
        }

        private void ExitFromThisForm()
        {
            timer1.Enabled = false;

            // Запись данных в поля на форме
            Program.AllT = Program.AllT + Program.Fenom1T;
            Program.AllFenom = Program.Fenom1T + Program.AllFenom;
            Program.fenomenologiya = richTextBox2.Text;
            Program.glavsved = richTextBox3.Text;

            // Время до решения задачи
            TimeWithoutKatamnez();

            // Запись данных в протокол
            Program.Insert = "Главные сведения по феноменологии: " + Program.glavsved + "";
            wordinsert.Ins();
            Program.Insert = "Резюме по феноменологии: " + Program.fenomenologiya + "";
            wordinsert.Ins();
            Program.Insert = "Время на феноменологии (Свободная форма): " + Program.Fenom1T + " сек";
            wordinsert.Ins();
        }

        private void ExitFromProgram()
        {
            // Выход из формы
            ExitFromThisForm();

            // Запись данных в протокол
            Program.Insert = "Время общее на этапе феноменологии: " + Program.AllFenom + " сек";
            wordinsert.Ins();

            // Добавление данных о форме
            StageInfo();

            // Обновление переменных
            Program.FullAllFenom = Program.FullAllFenom + Program.AllFenom;
            Program.AllFenom = 0;

            // Выход из программы
            exitProgram.ExProgr();

            // Отправка сообщения с протоколом
            exitProgram.ExProtokolSent();

            // Закрытие приложения
            Application.Exit();
        }

        private void StageInfo()
        {
            // Запись данных для графиков
            Program.StageName.Add("Ф");
            Program.StageSec.Add(Program.AllFenom);
            Program.NumberStage.Add(1);
        }


        private void FormAlign()
        {
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

                richTextBox1.Width = 450;
                richTextBox2.Width = 450;
                richTextBox3.Width = 450;

                label5.Width = 450;

                label4.Left = label4.Left - 170;
                richTextBox1.Left = richTextBox1.Left - 170;
                richTextBox2.Left = richTextBox2.Left - 170;

                foreach (Control ctrl in panel1.Controls)
                {
                    int newFontSize = 12;
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize);
                }
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

            label3.Width = panel1.Width;
            label3.TextAlign = ContentAlignment.TopCenter;
            button2.Left = 10;
            button1.Left = panel1.Width - button1.Width - 10;
            button3.Left = panel1.Width - button3.Width - 10;
        }
    }
}
