using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace CRM_MINIBASICO
{
    class SQLiteCommander
    {
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

        /*
        public string ReadCommand(string queryText)
        {
            using var connection = new SQLiteConnection(databasePath);
            connection.Open();

            using var command = new SQLiteCommand(queryText, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            connection.Close();

            return ($"SQLite version: {version}");
        }*/

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
    }
}
