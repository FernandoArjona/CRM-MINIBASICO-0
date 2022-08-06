using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CRM_MINIBASICO
{
    class SQLiteCommander
    {
        public void Command(string command)
        {
            //https://zetcode.com/csharp/sqlite/
            //https://extendsclass.com/sqlite-browser.html
            string cs = @"URI=file:D:\GITHUB\CRM-MINIBASICO\CRM-MINIBASICO\crmDataBase.sqli";
            using var con = new SQLiteConnection(cs);
            using var cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
