using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic.FileIO;
using System.IO;

//code Author: Evan Su
namespace MVC5App.Scripts.Test
{
    public class QueryTools
    {
        //This function reads in a CSV file and returns a string of all the entries
        public string readCSV(string inFile)
        {
            CSVDataStruct tempRow = new CSVDataStruct();
            string outstring = "";
            using (TextFieldParser parser = new TextFieldParser(@inFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    tempRow.company_name = fields[0];
                    tempRow.domain = fields[1];
                    tempRow.ccm_companySize = fields[2];
                    tempRow.ccm_industry = fields[3];
                    tempRow.category_name = fields[4];
                    tempRow.topic_id= fields[5];
                    tempRow.topic_name = fields[6];
                    tempRow.composite_score = fields[7];
                    tempRow.country = fields[8];
                    tempRow.metro_area = fields[9];
                    tempRow.domain_origin = fields[10];
                    tempRow.cdt = fields[11];
                    insertObject(tempRow);
                }
            }
            return outstring;
        }

        //Takes in a properly formatted string and inserts it into the database.
        public int insertString(string rowString)
        {

            //Processing row

            CSVDataStruct tempRow = new CSVDataStruct();
            //creates a stream
            var stream = new MemoryStream();
            var bytes = System.Text.Encoding.Default.GetBytes(rowString);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            TextFieldParser parser = new TextFieldParser(stream);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            string[] fields = parser.ReadFields();
            tempRow.company_name = fields[0];
            tempRow.domain = fields[1];
            tempRow.ccm_companySize = fields[2];
            tempRow.ccm_industry = fields[3];
            tempRow.category_name = fields[4];
            tempRow.topic_id = fields[5];
            tempRow.topic_name = fields[6];
            tempRow.composite_score = fields[7];
            tempRow.country = fields[8];
            tempRow.metro_area = fields[9];
            tempRow.domain_origin = fields[10];
            tempRow.cdt = fields[11];
            insertObject(tempRow);
            return 0;
        }

        //inserts a CSV object into a database.
        public string insertObject(CSVDataStruct insertRow)
        {
            string outMessage = "";
            string companyNumber;
            var insertCommandCompany = "INSERT INTO Company (CompanyName, Domain, CompanySize, Industry) Values(@CompanyName, @Domain, @CompanySize, @Industry)";
            var insertCommandTopic = "INSERT INTO Topic (TopicID,TopicName, Category) Values(@TopicID, @TopicName , @TopicCategory)";
            var insertCommandSurge = "INSERT INTO Surge (TopicID, CompanyID, Date, Score, Country, MetroArea, DomainOrigin) Values(@TopicID, @CompanyID , @Date, @Score, @Country, @MetroArea, @DomainOrigin)";
            var checkCommandCompany = "SELECT * FROM Company WHERE CompanyName = @CompanyName";
            var checkCommandTopic = "SELECT * FROM Topic WHERE TopicID = @TopicID";
            var checkCommandSurge = "SELECT * FROM Surge Where TopicID = @TopicID AND CompanyID = @CompanyID AND Date = @Date";

            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(checkCommandCompany, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", insertRow.company_name);
                var reader = cmd.ExecuteReader();

                if ( !reader.Read())
                {
                    reader.Close();
                    cmd = new MySqlCommand(insertCommandCompany, dbCon.Connection);
                    cmd.Parameters.AddWithValue("@CompanyName", insertRow.company_name);
                    cmd.Parameters.AddWithValue("@Domain",insertRow.domain);
                    cmd.Parameters.AddWithValue("@CompanySize", insertRow.ccm_companySize);
                    cmd.Parameters.AddWithValue("@Industry", insertRow.ccm_industry);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    reader.Close();
                }
                //we need the company number for the surge query

                cmd = new MySqlCommand(checkCommandCompany, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", insertRow.company_name);
                reader = cmd.ExecuteReader();
                reader.Read();
                companyNumber = reader.GetString(0);
                reader.Close();

                //topic check
                cmd = new MySqlCommand(checkCommandTopic, dbCon.Connection);
                cmd.Parameters.AddWithValue("@TopicID", insertRow.topic_id);
                reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    cmd = new MySqlCommand(insertCommandTopic, dbCon.Connection);
                    cmd.Parameters.AddWithValue("@TopicID", insertRow.topic_id);
                    cmd.Parameters.AddWithValue("@TopicName", insertRow.topic_name);
                    cmd.Parameters.AddWithValue("@TopicCategory", insertRow.category_name);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    reader.Close();
                }

                //surge check

                //topic check
                cmd = new MySqlCommand(checkCommandSurge, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyID", companyNumber);
                cmd.Parameters.AddWithValue("@TopicID", insertRow.topic_id);
                cmd.Parameters.AddWithValue("@Date", insertRow.cdt);
                reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    cmd = new MySqlCommand(insertCommandSurge, dbCon.Connection);
                    cmd.Parameters.AddWithValue("@TopicID", insertRow.topic_id);
                    cmd.Parameters.AddWithValue("@CompanyID", companyNumber);
                    cmd.Parameters.AddWithValue("@Date", insertRow.cdt);
                    cmd.Parameters.AddWithValue("@Score", insertRow.composite_score);
                    cmd.Parameters.AddWithValue("@Country", insertRow.country);
                    cmd.Parameters.AddWithValue("@MetroArea", insertRow.metro_area);
                    cmd.Parameters.AddWithValue("@DomainOrigin", insertRow.domain_origin);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    reader.Close();
                }
                dbCon.Close();
            }

                return outMessage;
        }

        //query SQL return companies from topic constrant
        public string returnCompanyFromTopic(string topic, string deliminator)
        {
            var outstring = "";
            var companyByTopicCommand = "SELECT DISTINCT com.CompanyName From Company com ";
            companyByTopicCommand += "INNER JOIN Surge s on com.CompanyID = s.CompanyID ";
            companyByTopicCommand += "LEFT JOIN Topic top on top.TopicID = s.TopicID ";
            companyByTopicCommand += "Where top.TopicName = @TopicName ";
            companyByTopicCommand += "ORDER BY s.Score ";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companyByTopicCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@TopicName", topic);
                var reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }

        //query SQL return companies from topic constrant
        //Returns the score as well
        public string returnCompanyFromTopicWithScore(string topic, string deliminator)
        {
            var outstring = "";
            var companyByTopicCommand = "select Distinct com.CompanyName, s.Score from Surge s "
                                    + "inner join Company com on com.CompanyID = s.CompanyID "
                                    + "inner join Topic top on top.TopicID = s.TopicID "
                                    + "left outer join Surge s2 on(s.CompanyID = s2.CompanyID and s.TopicID = s2.TopicID and s.Date<s2.Date) "
                                    + "where top.TopicName = @TopicName and s2.SurgeID is null "
                                    + "Order by s.Score desc ";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companyByTopicCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@TopicName", topic);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;
                    outstring += reader.GetString(1) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }
        //query SQL return topic from company constrant
        public string returnTopicFromCompany(string company, string deliminator)
        {
            var outstring = "";
            var companyByTopicCommand = "SELECT DISTINCT top.TopicName From Topic top ";
            companyByTopicCommand += "INNER JOIN Surge s on top.TopicID = s.TopicID ";
            companyByTopicCommand += "LEFT JOIN Company com on com.CompanyID = s.CompanyID  ";
            companyByTopicCommand += "WHERE com.CompanyName = @CompanyName ";
            companyByTopicCommand += "ORDER BY s.Score ";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companyByTopicCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", company);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }

        //query SQL return topic from company constrant
        //Returns the score as well
        public string returnTopicFromCompanyWithScore(string company, string deliminator)
        {
            var outstring = "";
            var companyByTopicCommand = "select Distinct top.TopicName, s.Score from Surge s "
                                    + "inner join Company com on com.CompanyID = s.CompanyID "
                                    + "inner join Topic top on top.TopicID = s.TopicID "
                                    + "left outer join Surge s2 on(s.CompanyID = s2.CompanyID and s.TopicID = s2.TopicID and s.Date<s2.Date) "
                                    + "where com.CompanyName = @CompanyName and s2.SurgeID is null "
                                    + "Order by s.Score desc ";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companyByTopicCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", company);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;
                    outstring += reader.GetString(1) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }

        //query SQL find a company name
        public string findCompanyWithString(string partName, string deliminator)
        {
            var outstring = "";
            var companySearchCommand = "Select CompanyName From Company Where CompanyName LIKE @CompanyName";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companySearchCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", "%"+partName+"%");
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }

        //query SQL find a topic name
        public string findTopicWithString(string partName, string deliminator)
        {
            var outstring = "";
            var companySearchCommand = "Select TopicName From Topic Where TopicName LIKE @TopicName";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(companySearchCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@TopicName", "%" + partName + "%");
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }


        //query SQL find a topic name
        public string findSurgeWithCompanyTopic(string company, string topic, string deliminator)
        {
            var outstring = "";
            var surgeSearchCommand = "SELECT s.Date, s.Score FROM SurgeDB.Surge s ";
            surgeSearchCommand += "INNER JOIN SurgeDB.Topic top on top.TopicID = s.TopicID ";
            surgeSearchCommand += "LEFT JOIN SurgeDB.Company com on com.CompanyID = s.CompanyID ";
            surgeSearchCommand += "WHERE ";
            surgeSearchCommand += "com.CompanyName = @CompanyName ";
            surgeSearchCommand += "AND top.TopicName = @TopicName ";
            surgeSearchCommand += "Order BY s.Date";
            //creates connection class instance
            var dbCon = MVC5App.Models.DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            //trys to connect once
            if (dbCon.IsConnect())
            {
                //based on how the code is written, the connection can fail.
                //This attempts to retry connection if intially failed.
                int tryConnection = 0;
                while (dbCon.Connection.State != System.Data.ConnectionState.Open && tryConnection < 10)
                {
                    dbCon.IsConnect();
                    tryConnection++;
                }

                //company check

                var cmd = new MySqlCommand(surgeSearchCommand, dbCon.Connection);
                cmd.Parameters.AddWithValue("@CompanyName", company);
                cmd.Parameters.AddWithValue("@TopicName", topic);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    outstring += reader.GetString(0) + deliminator;
                    outstring += reader.GetString(1) + deliminator;

                }
                reader.Close();
                dbCon.Close();
            }
            return outstring;
        }
    }
}