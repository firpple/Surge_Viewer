//code author: Ocph23 (stackExchange User)
//code modifier: Evan Su

using MySql.Data;
using MySql.Data.MySqlClient;

namespace MVC5App.Models
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (connection == null)
            {
                if (string.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=dbcs426.cuqu42tp1zuc.us-west-1.rds.amazonaws.com; database= SurgeDB; UID=MasterRoot; password=AlphaApple#");
                connection = new MySqlConnection(connstring);
                connection.Open();
                
                if(connection.State == System.Data.ConnectionState.Open)
                {
                    System.Console.WriteLine("connected!");
                }
                else
                {
                    System.Console.WriteLine("NO CONNECTION");
                }
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}