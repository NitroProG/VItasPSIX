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
            // Подключение к БД на сервере
            // "workstation id=psicotestes.mssql.somee.com;packet size=4096;user id=" + username + ";pwd=" + password + ";data source=" + datasource + ";persist security info=False; initial catalog=" + database + "";
            // string conString = "workstation id=psicotestes.mssql.somee.com;packet size=4096;user id=adminadmin;pwd=adminadmin;data source=psicotestes.mssql.somee.com;persist security info=False;initial catalog=psicotestes";

            // Подключение к локальной БД
            // Data Source=COMPUTER\\FILESBD;initial catalog=psico; Persist Security info = True; User ID = sa; Password = D6747960f
            // string conString = "Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            // Подключение к серверу SmarterASP.net
            // "Data Source=SQL6002.site4now.net;Initial Catalog=DB_A48030_Psicotest;User Id=DB_A48030_Psicotest_admin;Password=YOUR_DB_PASSWORD;"
             string conString = "Data Source=" + datasource + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password;

            // Создание подключения
            SqlConnection con = new SqlConnection(conString);
            return con;
        }
    }
}
