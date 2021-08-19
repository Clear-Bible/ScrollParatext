using System;
using System.Collections.Generic;

namespace ScrollParatext.SQLite
{
    public class VerseConversions
    {
        private Dictionary<string, string> _bookLookup = new Dictionary<string, string>();

        public VerseConversions()
        {
            PopulateLookup();
        }

        private void PopulateLookup()
        {
            _bookLookup.Add("01", "Genesis");
            _bookLookup.Add("02", "Exodus");
            _bookLookup.Add("03", "Leviticus");
            _bookLookup.Add("04", "Numbers");
            _bookLookup.Add("05", "Deuteronomy");
            _bookLookup.Add("06", "Joshua");
            _bookLookup.Add("07", "Judges");
            _bookLookup.Add("08", "Ruth");
            _bookLookup.Add("09", "1 Samuel (1 Kings)");
            _bookLookup.Add("10", "2 Samuel (2 Kings)");
            _bookLookup.Add("11", "1 Kings (3 Kings)");
            _bookLookup.Add("12", "2 Kings (4 Kings)");
            _bookLookup.Add("13", "1 Chronicles");
            _bookLookup.Add("14", "2 Chronicles");
            _bookLookup.Add("15", "Ezra");
            _bookLookup.Add("16", "Nehemiah");
            _bookLookup.Add("17", "Esther");
            _bookLookup.Add("18", "Job");
            _bookLookup.Add("19", "Psalms");
            _bookLookup.Add("20", "Proverbs");
            _bookLookup.Add("21", "Ecclesiastes");
            _bookLookup.Add("22", "Song of Solomon");
            _bookLookup.Add("23", "Isaiah");
            _bookLookup.Add("24", "Jeremiah");
            _bookLookup.Add("25", "Lamentations");
            _bookLookup.Add("26", "Ezekiel");
            _bookLookup.Add("27", "Daniel");
            _bookLookup.Add("28", "Hosea");
            _bookLookup.Add("29", "Joel");
            _bookLookup.Add("30", "Amos");
            _bookLookup.Add("31", "Obadiah");
            _bookLookup.Add("32", "Jonah");
            _bookLookup.Add("33", "Micah");
            _bookLookup.Add("34", "Nahum");
            _bookLookup.Add("35", "Habakkuk");
            _bookLookup.Add("36", "Zephaniah");
            _bookLookup.Add("37", "Haggai");
            _bookLookup.Add("38", "Zechariah");
            _bookLookup.Add("39", "Malachi");
            _bookLookup.Add("40", "Matthew");
            _bookLookup.Add("41", "Mark");
            _bookLookup.Add("42", "Luke");
            _bookLookup.Add("43", "John");
            _bookLookup.Add("44", "Acts");
            _bookLookup.Add("45", "Romans");
            _bookLookup.Add("46", "1 Corinthians");
            _bookLookup.Add("47", "2 Corinthians");
            _bookLookup.Add("48", "Galatians");
            _bookLookup.Add("49", "Ephesians");
            _bookLookup.Add("50", "Philippians");
            _bookLookup.Add("51", "Colossians");
            _bookLookup.Add("52", "1 Thessalonians");
            _bookLookup.Add("53", "2 Thessalonians");
            _bookLookup.Add("54", "1 Timothy");
            _bookLookup.Add("55", "2 Timothy");
            _bookLookup.Add("56", "Titus");
            _bookLookup.Add("57", "Philemon");
            _bookLookup.Add("58", "Hebrews");
            _bookLookup.Add("59", "James");
            _bookLookup.Add("60", "1 Peter");
            _bookLookup.Add("61", "2 Peter");
            _bookLookup.Add("62", "1 John");
            _bookLookup.Add("63", "2 John");
            _bookLookup.Add("64", "3 John");
            _bookLookup.Add("65", "Jude");
            _bookLookup.Add("66", "Revelation");
        }

        /// <summary>
        /// With 8 or 12-digit textID (01001001) convert to
        /// Genesis 1:1
        /// </summary>
        /// <param name="textiD"></param>
        /// <returns></returns>
        public string ConvertTextId2FullVersePath(string textiD)
        {
            string book = textiD.Substring(0, 2);
            string chapter = textiD.Substring(2, 3);
            string verse = textiD.Substring(5, 3);

            string verseId;
            try
            {
                verseId = _bookLookup[book] + " " + chapter.ToString() + ":" + verse.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return verseId;
        }


        /// <summary>
        /// Takes in the 2 digit string with the book number and returns the Bible Book Name
        /// </summary>
        /// <param name="bookNum"></param>
        /// <returns></returns>
        public string ConvertTextId2BookName(string bookNum)
        {
            return _bookLookup[bookNum];
        }
    }
}
