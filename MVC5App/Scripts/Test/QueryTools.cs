﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic.FileIO;

//code Author: Evan Su
namespace MVC5App.Scripts.Test
{
    public class QueryTools
    {
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
                    int index = 0;
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        outstring += index.ToString() + field + " \n ";
                        index++;
                    }
                    index = 0;
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
                    outstring += "<><><><><";
                    outstring += insertObject(tempRow);
                    outstring += "><><><><>";
                }
            }
            return outstring;
        }

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
        //query SQL return topic from company constrant
        public string returnTopicFromCompany(string company, string deliminator)
        {
            var outstring = "";
            var companyByTopicCommand = "SELECT DISTINCT top.TopicName From Topic top ";
            companyByTopicCommand += "INNER JOIN Surge s on top.TopicID = s.TopicID ";
            companyByTopicCommand += "LEFT JOIN Company com on com.CompanyID = s.CompanyID  ";
            companyByTopicCommand += "Where com.CompanyName = @CompanyName ";
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





    }
}