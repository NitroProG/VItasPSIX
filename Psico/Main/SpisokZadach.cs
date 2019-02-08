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
    public partial class SpisokZadach : Form
    {
        DataGridView datagr = new DataGridView(); // Создание таблицы 
        int error; // Переменная ошибки
        int kolvoreshzadach; // Количество решённых задач
        WordInsert wordinsert = new WordInsert(); // Подключение класса

        public SpisokZadach()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Окрытие формы авторизации
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void SpisokZadach_Load(object sender, EventArgs e)
        {

            // Запись данных в ворд документ
            try
            {
                if (Program.AllT !=0)
                {
                    Program.Insert = "Время работы с задачей: " + Program.AllT + " сек";

                    wordinsert.Ins();
                }
            }

            // Если возникла ошибка во время записи данных в ворд документ
            catch
            {
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
            }

            // создание подключения к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open(); // подключение к БД

            // Выбор количества решённых задач пользователем
            SqlCommand kolvo = new SqlCommand("select count(*) as 'kolvo' from resh where users_id = " + Program.user + "", con);
            SqlDataReader dr0 = kolvo.ExecuteReader();
            dr0.Read();
            kolvoreshzadach = Convert.ToInt32(dr0["kolvo"].ToString());
            dr0.Close();
            kolvoreshzadach = kolvoreshzadach + 1;

            // Динамическое создание таблицы 
            datagr.Name = "datagrview";
            datagr.Location = new Point(100, 100);
            SqlDataAdapter da1 = new SqlDataAdapter("select zadacha_id from resh where users_id = " + Program.user + "", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "dpo");
            datagr.DataSource = ds1.Tables[0];
            datagr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel2.Controls.Add(datagr);
            datagr.Visible = false;

            // Создание списка задач 
            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";

            // Обнуление переменных
            Program.AllT = 0;
            Program.fenomenologiya = "";
            Program.glavsved = "";
            Program.gipotezi = "";
            Program.obsledovaniya = "";
            Program.zakluch = "";
            Program.zaklOTV = 0;
            Program.NeVernOtv = 0;
            Program.diagnoz = 0;

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

        private void button1_Click(object sender, EventArgs e)
        {
            error = 0; // Присвоение переменной
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1; // Присвоение переменной номера выбранной задачи
            for (int i = 1; i < kolvoreshzadach; i++) // Цикл проверяющий решена ли выбранная задача
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i-1].Cells[0].Value)) // Если в таблице решённых задач есть выбранная задача
                {
                    DialogResult result = MessageBox.Show("Данная диагностическая задача была уже решена!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения 
                    error = 1; // Пприсвоение переменной
                }
            }

            // Если выбранная задача не решена
            if (error == 0)
            {

                // Запись данных в ворд документ
                try
                {
                    Program.Insert = "Диагностическая задача №" + Program.NomerZadachi + ""; // Присвоение переменной данных, которые необходимо записать в ворд документ

                    wordinsert.Ins();

                    // Переход на главную форму задачи
                    Zadacha zadacha = new Zadacha();
                    zadacha.Show();
                    Close();
                }

                // Если возникла ошибка во время записи данных в ворд документ
                catch
                {
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Выход из программы
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            error = 0; // Присвоение переменной
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1; // Присовоение переменной выбранной задачи
            for (int i = 1; i < kolvoreshzadach; i++) // Цикл проверяющий решена ли выбранная задача
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i - 1].Cells[0].Value)) // Если выбранная задача есть в таблице решённых задач
                {
                    label3.Visible = true; // Вывод label 
                    error = 1; // Присвоение переменной
                }
            }

            // Если выбранной задачи нет в таблице решённых задач
            if (error == 0)
            {
                label3.Visible = false; // Скрытие label
            }
        }
    }
}
