using System.IO;
using System.Text;
using System.Data.SqlClient;

namespace SqlConn
{
    class SQLConnectionString
    {
        public static SqlConnection GetDBConnection()
        {
            try
            {
                // Открытие файла со строкой подключения к БД
                string[] s = File.ReadAllLines("C:\\System\\ConnectionString.txt", Encoding.Default);

                // Создание подключения
                SqlConnection con = new SqlConnection(s[0]);

                // Возвращение строки подключения
                return con;
            }catch{}

            // Возвращение ошибкия о подключении к БД
            SqlConnection Error = new SqlConnection("Data Source=Место хранения;initial catalog=Название БД;Persist Security info=True;User ID=Логин;Password=Пароль;");
            return Error;
        }
    }
}