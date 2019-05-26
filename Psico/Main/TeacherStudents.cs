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
    public partial class TeacherStudents : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr1 = new DataGridView();

        public TeacherStudents()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            con.Open();

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(780, 500);
            datagr1.Location = new Point(panel1.Width / 2 - datagr1.Width / 2, 70);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datagr1.CellClick += SelectUser;

            con.Close();

            TableFill();

            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionBackColor = Color.PowderBlue;
            datagr1.DefaultCellStyle.SelectionForeColor = Color.Black;
            datagr1.BackgroundColor = Color.White;

            panel1.Controls.Add(datagr1);

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.EnableHeadersVisualStyles = false;

            datagr1.Columns[0].Visible = false;
            datagr1.Columns[1].HeaderText = "Логин";
            datagr1.Columns[2].Visible = false;
            datagr1.Columns[3].Visible = false;
            datagr1.Columns[4].Visible = false;
            datagr1.Columns[5].Visible = false;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[7].Visible = false;
            datagr1.Columns[8].Visible = false;
            datagr1.Columns[9].Visible = false;
            datagr1.Columns[10].Visible = false;
            datagr1.Columns[11].HeaderText = "В сети";
            datagr1.Columns[12].Visible = false;

            FormAlignment();
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TableFill()
        {
            con.Open();

            if (Program.UserRole == 1)
            {
                SqlDataAdapter da1 = new SqlDataAdapter("select * from users",con);
                SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "users");
                datagr1.DataSource = ds1.Tables[0];
            }
            else
            {
                //Выбор данных из БД
                SqlCommand GetTeacherId = new SqlCommand("select Teacher_id from users where id_user = " + Program.user + "", con);
                SqlDataReader dr1 = GetTeacherId.ExecuteReader();
                dr1.Read();
                int Teacherid = Convert.ToInt32(dr1["Teacher_id"]);
                dr1.Close();

                SqlDataAdapter da1 = new SqlDataAdapter("select * from users where Teacher_id = " + Teacherid + "", con);
                SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "users");
                datagr1.DataSource = ds1.Tables[0];
            }

            datagr1.CurrentCell = null;

            con.Close();
        }

        private void OpenAutorizationForm(object sender, EventArgs e)
        {
            if (Program.UserRole == 1)
            {
                new administrator().Show();
            }
            else
            {
                new ExitProgram().UpdateUserStatus();
                new Autorization().Show();
            }
            Close();
        }

        private void UpdateUser(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(datagr1.CurrentRow.Cells[11].Value) == 1)
                {
                    con.Open();
                    SqlCommand UpdateUserStatus = new SqlCommand("UPDATE users SET UserStatus=0 WHERE id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                    UpdateUserStatus.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    con.Open();
                    SqlCommand UpdateUserStatus = new SqlCommand("UPDATE users SET UserStatus=1 WHERE id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                    UpdateUserStatus.ExecuteNonQuery();
                    con.Close();
                }

                TableFill();
            }
            catch
            {
                CreateInfo("Необходимо выбрать пользователя в таблице, для того чтобы изменить его статус!", "red", panel1);
            }
        }

        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel1.Location = new Point(screen.Width / 2 - panel1.Width / 2, screen.Height / 2 - panel1.Height / 2);
        }

        private void Find(object sender, EventArgs e)
        {
            TableFill();

            int Find;

            for (int x = 0; x < datagr1.Rows.Count; x++)
            {
                Find = 0;

                for (int y = 0; y < datagr1.ColumnCount; y++)
                {
                    if (datagr1.Rows[x].Cells[y].Value.ToString().Contains(textBox6.Text))
                    {
                        Find = 1;
                    }
                }

                if (Find != 1)
                {
                    datagr1.Rows[x].Visible = false;
                }
            }

            textBox6.Text = "";
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
            label.Font = new Font(label.Font.FontFamily, 16);
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
            }catch{}
        }
    }
}
