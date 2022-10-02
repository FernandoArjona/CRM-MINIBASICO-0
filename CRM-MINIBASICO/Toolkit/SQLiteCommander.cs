using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace CRM_MINIBASICO
{
    class SQLiteCommander
    {
        //private string databasePath = @"URI=file:D:\GITHUB\CRM-MINIBASICO\CRM-MINIBASICO\crmDataBase.sqli";
          private string databasePath = @"URI=file:D:\GITHUB\CRM-MINIBASICO\CRM-MINIBASICO\crmDataBase.sqli";
        public void WriteCommand(string queryText)
        {
            //https://zetcode.com/csharp/sqlite/
            //https://extendsclass.com/sqlite-browser.html
            using var connection = new SQLiteConnection(databasePath);
            using var command = new SQLiteCommand(connection);
            connection.Open();
            command.CommandText = queryText;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<string> ReadCommand(string queryText, string queryTarget)
        {
            List<string> results = new List<string>();
            using var connection = new SQLiteConnection(databasePath);
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = queryText;
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string cliente = (string)reader[queryTarget];
                        results.Add(cliente);
                    }
                }
            connection.Close();
            return results;
        }

        public List<int> ReadCommandInt(string queryText, string queryTarget)
        {
            List<int> results = new List<int>();
            using var connection = new SQLiteConnection(databasePath);
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = queryText;
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //object cliente = reader[queryTarget];
                    //int intVal = (int)cliente;
                    int val = (int)reader[queryTarget];
                    results.Add(val);
                }
            }
            connection.Close();
            return results;
        }

        public List<double> ReadCommandDouble(string queryText, string queryTarget)
        {
            List<double> results = new List<double>();
            using var connection = new SQLiteConnection(databasePath);
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = queryText;
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    double val = (double)reader[queryTarget];
                    results.Add(val);
                }
            }
            connection.Close();
            return results;
        }

        public List<DateTime> ReadCommandDate(string queryText, string queryTarget)
        {
            List<DateTime> results = new List<DateTime>();
            using var connection = new SQLiteConnection(databasePath);
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = queryText;
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string Test2 = (string)reader[queryTarget];
                    var val = DateTime.ParseExact(Test2, "dd/MM/yyyy hh:mm:ss", null);
                    results.Add(val);
                }
            }
            connection.Close();
            return results;
        }
    }
}
