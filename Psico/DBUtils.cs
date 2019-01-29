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
            //string username = "adminadmin";
            //string password = "adminadmin";
            //string datasource = "psicotestes.mssql.somee.com";
            //string database = "psicotestes";

            string username = "sa";
            string password = "D6747960f";
            string datasource = "COMPUTER\\FILESBD";
            string database = "psico";

            return DBSQLServerUtils.GetDBConnection(username, password, datasource, database);
        }
    }

}
