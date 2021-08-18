using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrollParatext.SQLite
{
    public class ReadData
    {
        public SQLiteConnection Conn;

        public ReadData(SQLiteConnection conn)
        {
            Conn = conn;
        }


        /// <summary>
        /// Get all the new glosses for a verse.
        ///
        /// Pass into this function the list of the verseID's we want glosses for
        /// </summary>
        /// <param name="verseID"></param>
        /// <returns></returns>
        public string GetSourceVerseText(string verseID)
        {

            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandType = CommandType.Text;

            string sql = "SELECT verseText FROM verses WHERE verseID='"
                         + verseID + "' LIMIT 1";

            sqliteCmd.CommandText = sql;

            sqliteDatareader = sqliteCmd.ExecuteReader();


            string verseText = "";
            while (sqliteDatareader.Read())
            {
                verseText = sqliteDatareader.GetString(0);
            }

            return verseText;
        }


        public Dictionary<string, bool> GetDistinctVerseIDs()
        {
            Dictionary<string, bool> verseIDs = new Dictionary<string, bool>();


            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandText = "SELECT DISTINCT VERSEID FROM BIBLE ORDER BY VERSEID";

            sqliteDatareader = sqliteCmd.ExecuteReader();
            while (sqliteDatareader.Read())
            {
                string myreader = sqliteDatareader.GetString(0);
                if (!verseIDs.ContainsKey(myreader))
                {
                    verseIDs.Add(myreader, false);
                }
            }

            return verseIDs;
        }



    }

}
