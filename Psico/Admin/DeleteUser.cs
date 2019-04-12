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
            datagr1.Size = new Size(255, 300);
            datagr1.Location = new Point(50, 100);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectUser;
            SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForDeleteUsers", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CreateTableForUpdateUsers");
            datagr1.DataSource = ds1.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;

            // Динамическое создание combobox
            ComboBox1.Name = "Combobox1";
            ComboBox1.Location = new Point(350,150);
            ComboBox1.Size = new Size(70,50);
            ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox1.Font = new Font(ComboBox1.Font.FontFamily, 12);
            panel1.Controls.Add(ComboBox1);

            // Динамическое создание combobox
            RadioButton1.Name = "Radiobutton1";
            RadioButton1.Text = "Пользователя.";
            RadioButton1.Location = new Point(350, 260);
            RadioButton1.Size = new Size(200, 50);
            RadioButton1.Font = new Font(RadioButton1.Font.FontFamily, 12);
            RadioButton1.Click += UserSettingChanged;
            panel1.Controls.Add(RadioButton1);

            // Динамическое создание combobox
            RadioButton2.Name = "Radiobutton2";
            RadioButton2.Text = "Решённую задачу.";
            RadioButton2.Location = new Point(350, 300);
            RadioButton2.Size = new Size(200, 50);
            RadioButton2.Click += ZadachaSettingChanged;
            RadioButton2.Font = new Font(RadioButton2.Font.FontFamily, 12);
            panel1.Controls.Add(RadioButton2);
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
                    MessageBox.Show("Для удаления необходимо выбрать что вы хотите удалить: пользователя или решённую им задачу!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    break;

                case 1:
                    switch ((panel1.Controls["datagrview1"] as DataGridView).CurrentRow.Cells[2].Value.ToString())
                    {
                        case "Admin":

                            break;

                        case "Teacher":

                            break;

                        case "Student":

                            // Удаление данных
                            SqlCommand StrPrc1 = new SqlCommand("[dbo].Role_delete", con); // Объявление переменной
                            StrPrc1.CommandType = CommandType.StoredProcedure;
                            StrPrc1.Parameters.AddWithValue("@id_role", Convert.ToInt32((panel1.Controls["datagrview1"] as DataGridView).CurrentRow.Cells[0].Value)); // Удаление данных
                            StrPrc1.ExecuteNonQuery();

                            // Удаление данных
                            SqlCommand StrPrc2 = new SqlCommand("[dbo].Users_delete", con); // Объявление переменной
                            StrPrc2.CommandType = CommandType.StoredProcedure;
                            StrPrc2.Parameters.AddWithValue("@id_user", Convert.ToInt32((panel1.Controls["datagrview1"] as DataGridView).CurrentRow.Cells[0].Value)); // Удаление данных
                            StrPrc2.ExecuteNonQuery();

                            break;
                    }
                    break;

                case 2:
                    if ((panel1.Controls["Combobox1"] as ComboBox).SelectedIndex < 0)
                    {
                        MessageBox.Show("У пользователя нет решённых задач, удаленние отменено!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Выбор данных в таблице БД
                        SqlCommand GetReshId = new SqlCommand("select id_resh as 'id' from resh where users_id=" + Convert.ToInt32((panel1.Controls["datagrview1"] as DataGridView).CurrentRow.Cells[0].Value) + " and zadacha_id=" + (panel1.Controls["Combobox1"] as ComboBox).SelectedValue + "", con);
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
    }
}
