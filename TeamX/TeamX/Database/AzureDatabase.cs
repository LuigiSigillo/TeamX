using System;
using System.Data.SqlClient;

namespace TeamX.Database
{
    public static class AzureDatabase
    {
        private static string ConnectionString = "";

        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection();
            conn.ConnectionString = ConnectionString;
            conn.Open();
            return conn;
        }

        public static void CloseConnection(SqlConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
