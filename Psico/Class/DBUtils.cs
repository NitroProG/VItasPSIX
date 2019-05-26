using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SqlConn
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            // Подключение к БД на сервере
            //string datasource = "psicotestes.mssql.somee.com";
            //string database = "psicotestes";
            //string username = "adminadmin";
            //string password = "adminadmin";

            // Подключение к локальной БД
            string datasource = "IWANTTOSAYHELLO\\FILESBD";
            string database = "psico";
            string username = "sa";
            string password = "D6747960f";

            // Подключение к серверу SmarterASP.net
            //string datasource = "SQL6001.site4now.net";
            //string database = "DB_A48030_Psicotest";
            //string username = "DB_A48030_Psicotest_admin";
            //string password = "6747960d";

            // Возвращение данных о подключении
            return DBSQLServerUtils.GetDBConnection(username, password, datasource, database);
        }
    }
}
