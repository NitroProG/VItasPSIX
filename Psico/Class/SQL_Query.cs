using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    class SQL_Query
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();

        public string GetInfoFromBD(string Query)
        {
            // Выбор данных из БД
            con.Open();
            try
            {
                SqlCommand sq = new SqlCommand(Query, con);
                string Info = sq.ExecuteScalar().ToString();
                con.Close();
                return (Info);
            }
            catch
            {

            }
            string Error = "0";
            con.Close();
            return (Error);
        }

        public void UpdateOneCell(string Query)
        {
            // Изменение данных в БД
            con.Open();
            SqlCommand sq = new SqlCommand(Query, con);
            sq.ExecuteNonQuery();
            con.Close();
        }

        public void CreateDatagr(string Query,string TableName,Panel panel, DataGridView datagr)
        {
            // Запись данных в таблицу из БД
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, TableName);
            datagr.DataSource = ds.Tables[0];
            panel.Controls.Add(datagr);
            datagr.Visible = false;
            con.Close();
        }

        public void GetInfoForCombobox(string Query, ComboBox comboBox)
        {
            // Запись данных в combobox из БД
            con.Open();
            SqlCommand sq = new SqlCommand(Query, con);
            SqlDataReader dr = sq.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            comboBox.DataSource = dt;
            comboBox.ValueMember = "ido";
            con.Close();
        }

        public void DeleteInfoFromBD(string Query)
        {
            // Удаление данных из БД
            con.Open();
            SqlCommand sq = new SqlCommand(Query, con);
            sq.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateDatagr(string Query, string TableName, DataGridView datagr)
        {
            // Обновление datagr
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, TableName);
            datagr.DataSource = ds.Tables[0];
            con.Close();
        }
    }
}
