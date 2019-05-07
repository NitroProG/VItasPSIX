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
using System.Net;
using System.Net.Mail;
using Xceed.Words.NET;

namespace Psico
{
    public partial class administrator : Form
    {

        SqlConnection con = DBUtils.GetDBConnection();

        public administrator()
        {
            InitializeComponent();
        }

        private void OpenFormUpdateZadacha(object sender, EventArgs e)
        {
            UpdateZadacha updateZadacha = new UpdateZadacha();
            updateZadacha.Show();
            Close();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }

        private void ExitFromProgramm(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenFormAddZadacha(object sender, EventArgs e)
        {
            AddZadacha addZadacha = new AddZadacha();
            addZadacha.Show();
            Close();
        }

        private void OpenFormDeleteZadacha(object sender, EventArgs e)
        {
            DeleteZadacha deleteZadacha = new DeleteZadacha();
            deleteZadacha.Show();
            Close();
        }

        private void GetLicenseKey(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();

            //Выбор данных из БД
            SqlCommand GetKeyDefender = new SqlCommand("select DefenderKey from defender", con);
            SqlDataReader dr = GetKeyDefender.ExecuteReader();

            // Запись данных из БД
            dr.Read();
            Clipboard.SetText(dr["DefenderKey"].ToString());
            dr.Close();

            con.Close();

            MessageBox.Show("Ключ активации программы был скопирован в буфер обмена, для того чтобы его посмотреть откройте любой текстовый редактор и нажмите сочитание клавишь 'Ctrl+C'","Ключ активации",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void FormLoad(object sender, EventArgs e)
        {
            FormAlignment();
        }

        private void OpenAddUserForm(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.Show();
            Close();
        }

        private void OpenUpdateUserForm(object sender, EventArgs e)
        {
            UpdateUser updateUser = new UpdateUser();
            updateUser.Show();
            Close();
        }

        private void OpenDeleteUserForm(object sender, EventArgs e)
        {
            DeleteUser deleteUser = new DeleteUser();
            deleteUser.Show();
            Close();
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
    }
}
