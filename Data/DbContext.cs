using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartfinance_server.Data
{
    public class DbContext
    {
        public static string ConnectionString { get; set; }

        public DbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
