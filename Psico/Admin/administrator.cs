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

namespace Psico
{
    public partial class administrator : Form
    {
        ExitProgram exitProgram = new ExitProgram();
        SqlConnection con = DBUtils.GetDBConnection();

        public administrator()
        {
            InitializeComponent();
        }

        private void OpenFormUpdateZadacha(object sender, EventArgs e)
        {
            new UpdateZadacha().Show();
            Close();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            exitProgram.UpdateUserStatus();

            new Autorization().Show();
            Close();
        }

        private void ExitFromProgramm(object sender, EventArgs e)
        {
            exitProgram.UpdateUserStatus();

            Application.Exit();
        }

        private void OpenFormAddZadacha(object sender, EventArgs e)
        {
            new AddZadacha().Show();
            Close();
        }

        private void OpenFormDeleteZadacha(object sender, EventArgs e)
        {
            new DeleteZadacha().Show();
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

            CreateInfo("Ключ активации программы был скопирован в буфер обмена, для того чтобы его посмотреть откройте любой текстовый редактор и нажмите сочитание клавишь 'Ctrl+C'","lime", panel1);
        }

        private void FormLoad(object sender, EventArgs e)
        {
            FormAlignment();
        }

        private void OpenAddUserForm(object sender, EventArgs e)
        {
            new AddUser().Show();
            Close();
        }

        private void OpenUpdateUserForm(object sender, EventArgs e)
        {
            new UpdateUser().Show();
            Close();
        }

        private void OpenDeleteUserForm(object sender, EventArgs e)
        {
            new DeleteUser().Show();
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
            label.Font = new Font(label.Font.FontFamily, 14);
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

        private void OpenUserStatusForm(object sender, EventArgs e)
        {
            new TeacherStudents().Show();
            Close();
        }
    }
}
