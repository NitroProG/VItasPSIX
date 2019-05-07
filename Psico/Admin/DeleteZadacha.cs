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

namespace Psico
{
    public partial class DeleteZadacha : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();

        public DeleteZadacha()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        private void DeleteZadachaa(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Если вы удалите задачу, её не возможно будет вернуть, также задача удалится у остальных пользователях!", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                int SelectedNumb = Convert.ToInt32(comboBox1.SelectedValue);

                // Подключение к БД
                con.Open();

                // Удаление задачи
                SqlCommand StrPrc1 = new SqlCommand("delete from resh where zadacha_id = " + SelectedNumb + "", con);
                StrPrc1.ExecuteNonQuery();
                SqlCommand StrPrc2 = new SqlCommand("delete from Fenom1 where zadacha_id = " + SelectedNumb + "", con);
                StrPrc2.ExecuteNonQuery();
                SqlCommand StrPrc3 = new SqlCommand("delete from CBFormFill where zadacha_id = " + SelectedNumb + "", con);
                StrPrc3.ExecuteNonQuery();
                SqlCommand StrPrc4 = new SqlCommand("delete from dpo where zadacha_id = " + SelectedNumb + "", con);
                StrPrc4.ExecuteNonQuery();
                SqlCommand StrPrc5 = new SqlCommand("delete from meropr where zadacha_id = " + SelectedNumb + "", con);
                StrPrc5.ExecuteNonQuery();
                SqlCommand StrPrc6 = new SqlCommand("delete from katamnez where zadacha_id = " + SelectedNumb + "", con);
                StrPrc6.ExecuteNonQuery();
                SqlCommand StrPrc7 = new SqlCommand("delete from vernotv where zadacha_id = " + SelectedNumb + "", con);
                StrPrc7.ExecuteNonQuery();
                SqlCommand StrPrc8 = new SqlCommand("delete from Zadacha where id_zadacha = " + SelectedNumb + "", con);
                StrPrc8.ExecuteNonQuery();

                // Создание списка задач 
                SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
                SqlDataReader dr = get_otd_name.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "ido";

                MessageBox.Show("Задача удалена!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                con.Close();
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            // Создание списка задач 
            SqlCommand get_otd_name = new SqlCommand("select id_zadacha as \"ido\" from zadacha", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "ido";

            con.Close();

            FormAlignment();
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

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);

        }
    }
}
