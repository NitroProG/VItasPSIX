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
        DataGridView datagr = new DataGridView();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exprg = new ExitProgram();
        int error;
        int kolvoreshzadach;

        public SpisokZadach()
        {
            InitializeComponent();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            if (Program.diagnoz != 0)
            {
                exprg.ExProtokolSent();
            }

            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open();

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

            FormAlignment();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open();

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
            Program.StageName.Clear();
            Program.StageSec.Clear();

            error = 0;
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedValue);

            // Проверка данных о решении задачи
            for (int i = 1; i < kolvoreshzadach; i++)
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i-1].Cells[0].Value))
                {
                    CreateInfo("Данная диагностическая задача была уже решена!", "red");
                    error = 1;
                }
            }

            // Если выбранная задача не решена
            if (error == 0)
            {
                // Запись данных в протокол
                try
                {
                    wordinsert.CreateShift();

                    Program.Insert = "Диагностическая задача №" + Program.NomerZadachi + "";
                    wordinsert.Ins();

                    try
                    {
                        // Обнуление выбранных ответов пользователем
                        SqlCommand delete = new SqlCommand("delete from Lastotv where users_id = " + Program.user + "", con);
                        delete.ExecuteNonQuery();
                        SqlCommand delete3 = new SqlCommand("delete from OtvSelected where users_id = " + Program.user + "", con);
                        delete3.ExecuteNonQuery();

                        Zadacha zadacha = new Zadacha();
                        zadacha.Show();
                        Close();
                    }

                    catch
                    {
                        CreateInfo("Ошибка в Базе данных, обратитесь к администратору", "red");
                    }
                }

                catch
                {
                    CreateInfo("Отсутствует шаблон протокола! Обратитесь к администратору.", "red");
                }
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            if (Program.diagnoz !=0)
            {
                exprg.ExProtokolSent();
            }

            Application.Exit();
        }

        private void CBCheckedChanged(object sender, EventArgs e)
        {
            //error = 0;
            //Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;

            //// Проверка данных о решении задачи
            //for (int i = 1; i < kolvoreshzadach; i++)
            //{
            //    if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i - 1].Cells[0].Value))
            //    {
            //        CreateInfo("Задача уже решена!","lime");
            //        error = 1;
            //    }
            //}
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void CreateInfo(string labelinfo, string color)
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
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
                (sender as System.Windows.Forms.Timer).Stop();
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
