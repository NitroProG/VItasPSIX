using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace SqlConn
{
    class DBSQLServerUtils
    {
        public static SqlConnection GetDBConnection(string username, string password, string datasource, string database)
        {
            // workstation id=psicotest.mssql.somee.com;packet size=4096;user id=adminadmin;pwd=adminadmin;data source=psicotest.mssql.somee.com;persist security info=False;initial catalog=psicotest
            //string conString = "workstation id=psicotestes.mssql.somee.com;packet size=4096;user id=" + username + ";pwd=" + password + ";data source=" + datasource + ";persist security info=False; initial catalog=" + database + "";

            // Data Source=COMPUTER\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f
            string conString = "Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            SqlConnection con = new SqlConnection(conString);
            return con;
        }

    }
}
