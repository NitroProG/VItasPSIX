﻿using System;
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
            //string username = "adminadmin";
            //string password = "adminadmin";
            //string datasource = "psicotestes.mssql.somee.com";
            //string database = "psicotestes";

            // Подключение к локальной БД
            string username = "sa";
            string password = "D6747960f";
            string datasource = "IWANTTOSAYHELLO\\FILESBD";
            string database = "psico";

            // Возвращение данных о подключении
            return DBSQLServerUtils.GetDBConnection(username, password, datasource, database);
        }
    }

}
