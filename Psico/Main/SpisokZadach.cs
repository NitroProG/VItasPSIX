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
            exprg.ProtokolSent();

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

        private void OpenNextForm(object sender, EventArgs e)
        {
            // Подключение к БД
            SqlConnection con = DBUtils.GetDBConnection();
            con.Open();

            error = 0;
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;

            // Проверка данных о решении задачи
            for (int i = 1; i < kolvoreshzadach; i++)
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i-1].Cells[0].Value))
                {
                    DialogResult result = MessageBox.Show("Данная диагностическая задача была уже решена!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    error = 1;
                }
            }

            // Если выбранная задача не решена
            if (error == 0)
            {
                // Запись данных в протокол
                try
                {
                    Program.Insert = "Диагностическая задача №" + Program.NomerZadachi + "";
                    wordinsert.Ins();

                    try
                    {
                        // Обнуление выбранных ответов пользователем
                        SqlCommand delete = new SqlCommand("delete from otvGip where users_id = " + Program.user + "", con);
                        delete.ExecuteNonQuery();
                        SqlCommand delete1 = new SqlCommand("delete from otvDiag where users_id = " + Program.user + "", con);
                        delete1.ExecuteNonQuery();
                        SqlCommand delete2 = new SqlCommand("delete from otvFenom where users_id = " + Program.user + "", con);
                        delete2.ExecuteNonQuery();
                        SqlCommand delete3 = new SqlCommand("delete from DpoSelected where users_id = " + Program.user + "", con);
                        delete3.ExecuteNonQuery();
                        SqlCommand delete4 = new SqlCommand("delete from FenomSelected where users_id = " + Program.user + "", con);
                        delete4.ExecuteNonQuery();
                        SqlCommand delete5 = new SqlCommand("delete from TeorSelected where users_id = " + Program.user + "", con);
                        delete5.ExecuteNonQuery();
                    }

                    catch
                    {
                        MessageBox.Show("Ошибка в БД, обратитесь к администратору","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }


                    Zadacha zadacha = new Zadacha();
                    zadacha.Show();
                    Close();
                }

                catch
                {
                    MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            exprg.ProtokolSent();

            Application.Exit();
        }

        private void CBCheckedChanged(object sender, EventArgs e)
        {
            error = 0;
            Program.NomerZadachi = Convert.ToInt32(comboBox1.SelectedIndex) + 1;

            // Проверка данных о решении задачи
            for (int i = 1; i < kolvoreshzadach; i++)
            {
                if (Convert.ToString(Program.NomerZadachi) == Convert.ToString(datagr.Rows[i - 1].Cells[0].Value))
                {
                    label3.Visible = true;
                    error = 1;
                }
            }

            if (error == 0)
            {
                label3.Visible = false;
            }
        }
    }
}
