//code author: Ocph23 (stackExchange User)
//code modifier: Evan Su
/*
 * Purpose:
 *  DBConnection is a class that handles connection to the database.
 * 
 * 
 */

using MySql.Data;
using MySql.Data.MySqlClient;

namespace MVC5App.Models
{
    public class DBConnection
    {
        //default private constructor
        private DBConnection()
        {
        }

        //database name
        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        //database password
        public string Password { get; set; }

        //database SQLConnection class
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        //database SQLConnection class
        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            _instance = new DBConnection();
            return _instance;
        }

        //IsConnect attempts to connect to the database based on the parameters passed in.
        //Currently, the database connection string is hardcoded in.
        //This should be fixed in order to prevent unauthorized access to the database. 
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

        //Close, closes the connection to the database. 
        public void Close()
        {
            connection.Close();
        }
    }
}