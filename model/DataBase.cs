using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKO.model
{
    class DataBase
    {
        private static DataBase instance;
        public string connectionString = "server=localhost;uid=root;pwd=;database=eco_monitoring";
        public TablesNames _currentTable { get; set; }
        //

        private DataBase()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
        }
        public static DataBase getInstance()
        {
            if (instance == null)
                instance = new DataBase();
            return instance;
        }
    }
}
