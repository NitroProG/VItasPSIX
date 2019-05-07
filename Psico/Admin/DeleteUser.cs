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
    public partial class DeleteUser : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr1 = new DataGridView();
        ComboBox ComboBox1 = new ComboBox();
        RadioButton RadioButton1 = new RadioButton();
        RadioButton RadioButton2 = new RadioButton();
        int checkSettingForDelete = 0;

        public DeleteUser()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            GetOpenMainForm();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            con.Open();

            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(300, 300);
            datagr1.Location = new Point(30, 100);
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectUser;

            DatagrInfo();

            panel1.Controls.Add(datagr1);

            datagr1.BackgroundColor = Color.White;
            datagr1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;

            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
            datagr1.EnableHeadersVisualStyles = false;

            datagr1.Columns[0].HeaderText = "Номер пользователя";
            datagr1.Columns[1].HeaderText = "Логин пользователя";
            datagr1.Columns[2].HeaderText = "Роль пользователя";


            label2.Location = new Point(datagr1.Location.X,datagr1.Location.Y-30);

            // Динамическое создание combobox
            ComboBox1.Name = "Combobox1";
            ComboBox1.Location = new Point(label3.Location.X+20,150);
            ComboBox1.Size = new Size(70,50);
            ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox1.Font = new Font(ComboBox1.Font.FontFamily, 12);
            panel1.Controls.Add(ComboBox1);

            // Динамическое создание combobox
            RadioButton1.Name = "Radiobutton1";
            RadioButton1.Text = "Пользователя.";
            RadioButton1.Location = new Point(label4.Location.X+20, 260);
            RadioButton1.Size = new Size(200, 50);
            RadioButton1.Font = new Font(RadioButton1.Font.FontFamily, 12);
            RadioButton1.Click += UserSettingChanged;
            panel1.Controls.Add(RadioButton1);

            // Динамическое создание combobox
            RadioButton2.Name = "Radiobutton2";
            RadioButton2.Text = "Решённую задачу.";
            RadioButton2.Location = new Point(label4.Location.X+20, 300);
            RadioButton2.Size = new Size(200, 50);
            RadioButton2.Click += ZadachaSettingChanged;
            RadioButton2.Font = new Font(RadioButton2.Font.FontFamily, 12);
            panel1.Controls.Add(RadioButton2);

            FormAlignment();
        }

        private void UserSettingChanged(object sender, EventArgs e)
        {
            checkSettingForDelete = 1;
        }

        private void ZadachaSettingChanged(object sender, EventArgs e)
        {
            checkSettingForDelete = 2;
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            // Создание списка задач 
            SqlCommand get_otd_name = new SqlCommand("select Zadacha_id as \"ido\" from resh where users_id = '" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "'", con);
            SqlDataReader dr = get_otd_name.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            (panel1.Controls["Combobox1"] as ComboBox).DataSource = dt;
            (panel1.Controls["Combobox1"] as ComboBox).ValueMember = "ido";
        }

        private void DeleteUsers(object sender, EventArgs e)
        {
            switch (checkSettingForDelete)
            {
                case 0:
                    CreateInfo("Для удаления необходимо выбрать что вы хотите удалить: пользователя или решённую им задачу!", "red");
                    break;

                case 1:
                    if (datagr1.CurrentRow.Cells[1].Value.ToString() != "admin" && datagr1.CurrentRow.Cells[2].Value.ToString() != "Admin")
                    {
                        try
                        {
                            switch (datagr1.CurrentRow.Cells[2].Value.ToString())
                            {
                                case "Admin":
                                    // Удаление данных
                                    SqlCommand delete1 = new SqlCommand("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete1.ExecuteNonQuery();
                                    SqlCommand delete2 = new SqlCommand("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete2.ExecuteNonQuery();
                                    SqlCommand delete3 = new SqlCommand("delete from InfoUser where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete3.ExecuteNonQuery();
                                    SqlCommand delete4 = new SqlCommand("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete4.ExecuteNonQuery();
                                    SqlCommand delete5 = new SqlCommand("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete5.ExecuteNonQuery();
                                    SqlCommand delete6 = new SqlCommand("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete6.ExecuteNonQuery();

                                    DatagrInfo();
                                    break;

                                case "Teacher":
                                    // Удаление данных
                                    SqlCommand delete7 = new SqlCommand("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete7.ExecuteNonQuery();
                                    SqlCommand delete8 = new SqlCommand("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete8.ExecuteNonQuery();
                                    SqlCommand delete9 = new SqlCommand("delete from InfoUser where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete9.ExecuteNonQuery();
                                    SqlCommand delete10 = new SqlCommand("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete10.ExecuteNonQuery();
                                    SqlCommand delete11 = new SqlCommand("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete11.ExecuteNonQuery();
                                    SqlCommand delete12 = new SqlCommand("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete12.ExecuteNonQuery();

                                    DatagrInfo();
                                    break;

                                case "Student":
                                    // Удаление данных
                                    SqlCommand delete13 = new SqlCommand("delete from Role where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete13.ExecuteNonQuery();
                                    SqlCommand delete14 = new SqlCommand("delete from Lastotv where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete14.ExecuteNonQuery();
                                    SqlCommand delete15 = new SqlCommand("delete from InfoUser where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete15.ExecuteNonQuery();
                                    SqlCommand delete16 = new SqlCommand("delete from Resh where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete16.ExecuteNonQuery();
                                    SqlCommand delete17 = new SqlCommand("delete from OtvSelected where users_id = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete17.ExecuteNonQuery();
                                    SqlCommand delete18 = new SqlCommand("delete from Users where id_user = " + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "", con);
                                    delete18.ExecuteNonQuery();

                                    DatagrInfo();
                                    break;
                            }
                        }
                        catch
                        {
                            CreateInfo("Необходимо выбрать пользователя в таблице для его удаления! Если вы выбрали пользователя, а сообщение осталось обратитесь к администратору!", "red");
                        }
                    }
                    else
                    {
                        CreateInfo("Невозможно удалить главного администратора!", "red");
                    }
                    break;

                case 2:
                    if ((panel1.Controls["Combobox1"] as ComboBox).SelectedIndex < 0)
                    {
                        CreateInfo("У пользователя нет решённых задач, удаленние отменено!", "red");
                    }
                    else
                    {
                        // Выбор данных в таблице БД
                        SqlCommand GetReshId = new SqlCommand("select id_resh as 'id' from resh where users_id=" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + " and zadacha_id=" + (panel1.Controls["Combobox1"] as ComboBox).SelectedValue + "", con);
                        SqlDataReader dr4 = GetReshId.ExecuteReader();
                        dr4.Read();
                        int ReshId = Convert.ToInt32(dr4["id"].ToString());
                        dr4.Close();

                        // Удаление данных
                        SqlCommand StrPrc2 = new SqlCommand("[dbo].Resh_delete", con); // Объявление переменной
                        StrPrc2.CommandType = CommandType.StoredProcedure;
                        StrPrc2.Parameters.AddWithValue("@id_resh", ReshId); // Удаление данных
                        StrPrc2.ExecuteNonQuery();

                        // Создание списка задач 
                        SqlCommand get_otd_name = new SqlCommand("select Zadacha_id as \"ido\" from resh where users_id = '" + Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value) + "'", con);
                        SqlDataReader dr = get_otd_name.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        (panel1.Controls["Combobox1"] as ComboBox).DataSource = dt;
                        (panel1.Controls["Combobox1"] as ComboBox).ValueMember = "ido";
                    }
                    break;
            }
        }

        private void GetOpenMainForm()
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void DatagrInfo()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForDeleteUsers", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CreateTableForUpdateUsers");
            datagr1.DataSource = ds1.Tables[0];
        }

        private void CreateInfo(string labelinfo, string color)
        {
            Timer timer = new Timer();
            timer.Interval = 5000;
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
                    break;
                case "lime":
                    label.ForeColor = Color.LimeGreen;
                    break;
                default:
                    label.ForeColor = Color.Black;
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
                (sender as Timer).Stop();
            }
            catch
            {

            }
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

