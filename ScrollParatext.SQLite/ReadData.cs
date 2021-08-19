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



        public List<string> GetDistinctBookIDs()
        {
            VerseConversions vc = new VerseConversions();
            var books = new List<string>();

            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandText = "SELECT DISTINCT BOOK FROM VERSES ORDER BY BOOK";

            sqliteDatareader = sqliteCmd.ExecuteReader();
            while (sqliteDatareader.Read())
            {
                string book = sqliteDatareader.GetString(0);
                books.Add(vc.ConvertTextId2BookName(book));
            }

            return books;
        }

        public List<string> GetDistinctChapterIDs(int index)
        {
            index++;
            string bookID = index.ToString().PadLeft(2, '0');

            var chapters = new List<string>();

            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandText = string.Format("SELECT DISTINCT CHAPTER FROM VERSES WHERE BOOK='{0}' ORDER BY CHAPTER", bookID);

            sqliteDatareader = sqliteCmd.ExecuteReader();
            while (sqliteDatareader.Read())
            {
                string chapter = sqliteDatareader.GetString(0);
                chapters.Add(chapter);
            }

            return chapters;
        }

        public List<string> GetDistinctVerseIDs(int bookIndex, int index)
        {
            bookIndex++;
            string bookID = bookIndex.ToString().PadLeft(2, '0');
            index++;
            string chapterID = index.ToString().PadLeft(3, '0');

            var verses = new List<string>();

            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandText = string.Format("SELECT DISTINCT VERSE FROM VERSES WHERE BOOK='{0}' AND CHAPTER='{1}' ORDER BY VERSE", bookID, chapterID);

            sqliteDatareader = sqliteCmd.ExecuteReader();
            while (sqliteDatareader.Read())
            {
                string verse = sqliteDatareader.GetString(0);
                verses.Add(verse);
            }

            return verses;
        }

        public string GetVerseText(string verseID)
        {
            SQLiteDataReader sqliteDatareader;
            SQLiteCommand sqliteCmd;
            sqliteCmd = Conn.CreateCommand();
            sqliteCmd.CommandText = string.Format("SELECT VERSETEXT FROM VERSES WHERE VERSEID='{0}'", verseID);

            sqliteDatareader = sqliteCmd.ExecuteReader();
            while (sqliteDatareader.Read())
            {
                string verseText = sqliteDatareader.GetString(0);
                return verseText;
            }

            return "";
        }
    }

}
