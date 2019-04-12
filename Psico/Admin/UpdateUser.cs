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
    public partial class UpdateUser : Form
    {
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr1 = new DataGridView();

        public UpdateUser()
        {
            InitializeComponent();
        }

        private void OpenMainForm(object sender, EventArgs e)
        {
            GetOpenMainForm();
        }

        private void UpdateUsers(object sender, EventArgs e)
        {
            con.Open();

            if (textBox1.Text != "" && textBox2.Text !="" && textBox3.Text !="" && textBox4.Text !="")
            {
                switch (comboBox1.SelectedIndex)
                {
                    // Администратор
                    case 0:
                        SqlCommand StrPrc4 = new SqlCommand("Teachers_update", con);
                        StrPrc4.CommandType = CommandType.StoredProcedure;
                        StrPrc4.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                        StrPrc4.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                        StrPrc4.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                        StrPrc4.ExecuteNonQuery();

                        SqlCommand StrPrc1 = new SqlCommand("users_update", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                        StrPrc1.Parameters.AddWithValue("@User_Login", textBox1.Text);
                        StrPrc1.Parameters.AddWithValue("@User_Password", textBox2.Text);
                        StrPrc1.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                        StrPrc1.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                        StrPrc1.ExecuteNonQuery();

                        MessageBox.Show("Данные изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                        SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                        DataSet ds1 = new DataSet();
                        da1.Fill(ds1, "users");
                        datagr1.DataSource = ds1.Tables[0];

                        break;

                    // Преподаватель
                    case 1:
                        SqlCommand StrPrc5 = new SqlCommand("Teachers_update", con);
                        StrPrc5.CommandType = CommandType.StoredProcedure;
                        StrPrc5.Parameters.AddWithValue("@id_teacher", Convert.ToInt32(datagr1.CurrentRow.Cells[5].Value.ToString()));
                        StrPrc5.Parameters.AddWithValue("@Unique_Naim", datagr1.CurrentRow.Cells[6].Value.ToString());
                        StrPrc5.Parameters.AddWithValue("@KolvoNeRegStudents", textBox4.Text);
                        StrPrc5.ExecuteNonQuery();

                        SqlCommand StrPrc2 = new SqlCommand("users_update", con);
                        StrPrc2.CommandType = CommandType.StoredProcedure;
                        StrPrc2.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                        StrPrc2.Parameters.AddWithValue("@User_Login", textBox1.Text);
                        StrPrc2.Parameters.AddWithValue("@User_Password", textBox2.Text);
                        StrPrc2.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                        StrPrc2.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                        StrPrc2.ExecuteNonQuery();

                        MessageBox.Show("Данные изменены!", "Отлично!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SqlDataAdapter da2 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                        SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
                        DataSet ds2 = new DataSet();
                        da2.Fill(ds2, "users");
                        datagr1.DataSource = ds2.Tables[0];

                        break;

                    // Студент
                    case 2:
                        SqlCommand StrPrc3 = new SqlCommand("users_update", con);
                        StrPrc3.CommandType = CommandType.StoredProcedure;
                        StrPrc3.Parameters.AddWithValue("@id_user", Convert.ToInt32(datagr1.CurrentRow.Cells[0].Value.ToString()));
                        StrPrc3.Parameters.AddWithValue("@User_Login", textBox1.Text);
                        StrPrc3.Parameters.AddWithValue("@User_Password", textBox2.Text);
                        StrPrc3.Parameters.AddWithValue("@User_Mail", textBox3.Text);
                        StrPrc3.Parameters.AddWithValue("@Teacher_id", Convert.ToInt32(datagr1.CurrentRow.Cells[4].Value.ToString()));
                        StrPrc3.ExecuteNonQuery();

                        MessageBox.Show("Данные изменены!","Отлично!",MessageBoxButtons.OK,MessageBoxIcon.Information);

                        SqlDataAdapter da3 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
                        SqlCommandBuilder cb3 = new SqlCommandBuilder(da3);
                        DataSet ds3 = new DataSet();
                        da3.Fill(ds3, "users");
                        datagr1.DataSource = ds3.Tables[0];

                        break;
                    default:
                        MessageBox.Show("Для изменения данных необходимо выбрать пользователя из таблицы!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Для изменения данных необходимо заполнить все поля!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            con.Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Динамическое создание таблицы
            datagr1.Name = "datagrview1";
            datagr1.Size = new Size(628,230);
            datagr1.Location = new Point((panel1.Width - datagr1.Width)/2, 70);
            datagr1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagr1.CellClick += SelectUser;
            SqlDataAdapter da1 = new SqlDataAdapter("select * from CreateTableForUpdateUsers", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "CreateTableForUpdateUsers");
            datagr1.DataSource = ds1.Tables[0];
            datagr1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            panel1.Controls.Add(datagr1);
            datagr1.Columns[0].Visible = false;
            datagr1.Columns[4].Visible = false;
            datagr1.Columns[5].Visible = false;
            datagr1.Columns[6].Visible = false;
            datagr1.Columns[8].Visible = false;
            datagr1.Columns[10].Visible = false;
            datagr1.Columns[11].Visible = false;
            datagr1.ReadOnly = true;
            datagr1.AllowUserToResizeColumns = false;
            datagr1.AllowUserToResizeRows = false;
            datagr1.AllowUserToAddRows = false;
            datagr1.AllowUserToDeleteRows = false;
            datagr1.AllowUserToOrderColumns = false;
            datagr1.RowHeadersVisible = false;
        }

        private void SelectUser(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Text = datagr1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Enabled = true;
            textBox2.Text = datagr1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Enabled = true;
            textBox3.Text = datagr1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Enabled = true;
            textBox4.Text = datagr1.CurrentRow.Cells[7].Value.ToString();

            switch (datagr1.CurrentRow.Cells[9].Value.ToString())
            {
                case "Admin":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "Teacher":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "Student":
                    comboBox1.SelectedIndex = 2;
                    textBox4.Enabled = false;
                    break;
            }
            comboBox1.Text = datagr1.CurrentRow.Cells[9].Value.ToString();
        }

        private void WriteOnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void GetOpenMainForm()
        {
            administrator admin = new administrator();
            admin.Show();
            Close();
        }
    }
}
