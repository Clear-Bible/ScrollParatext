using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ScrollParatext.Messages;
using ScrollParatext.SQLite;
using SIL.Scripture;

namespace ScrollParatext
{
    public partial class frmMain : Form
    {
        private int _lastBBCCCVVV;
        private SQLiteConnection _conn;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // wire up the events to watch
            MessageEvents.WatchMessage(SantaFeFocusMessageHandler.FocusMsg);
            MessageEvents.MessageReceived += OnSantaFeFocusMessage;

            // make a connection to the sqlite database
            var path = Application.StartupPath;
            string dbPath = Path.Combine(path, "ManuscriptVerses.sqlite");
            Connection c = new Connection(dbPath);
            _conn = c.Conn;

            // init populating the comboboxes
            ReadData rd = new ReadData(_conn);
            var bookList = rd.GetDistinctBookIDs();
            cboBooks.Items.AddRange(bookList.ToArray());
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clean up
            MessageEvents.MessageReceived -= OnSantaFeFocusMessage;
            _conn.Close();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Called when a SantaFeFocusMessage is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MessageReceivedEventArgs"/> instance 
        /// containing the event data.</param>
        /// ------------------------------------------------------------------------------------
        private void OnSantaFeFocusMessage(object sender, MessageReceivedEventArgs e)
        {
            Debug.Assert(e.Message.Msg == SantaFeFocusMessageHandler.FocusMsg);
            VerseRef scrRef;
            if (!VerseRef.TryParse(SantaFeFocusMessageHandler.ReceiveFocusMessage(e.Message), out scrRef) || _lastBBCCCVVV == scrRef.BBBCCCVVV)
                return;

            int linkChannel = e.Message.LParam.ToInt32();

            Debug.WriteLine("New position from SantaFe: {0}; Book {1}, Chapter {2}, Verse {3}; {4} Link={5}",
                scrRef.BBBCCCVVV, scrRef.Book, scrRef.Chapter, scrRef.Verse,
                scrRef.Text, linkChannel);

            switch (linkChannel)
            {
                case 0:
                    if (radA.Checked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 1:
                    if (radB.Checked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 2:
                    if (radC.Checked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 3:
                    if (radD.Checked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 4:
                    if (radE.Checked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
            }

            _lastBBCCCVVV = scrRef.BBBCCCVVV;
        }

        private void IncommingVerseChange(int iVerse)
        {
            string verseID = iVerse.ToString().PadLeft(8, '0');
            int bookID = Convert.ToInt32(verseID.Substring(0, 2)) - 1;

            cboBooks.Text = cboBooks.Items[bookID].ToString();

            cboChapters.Text = verseID.Substring(2, 3);
            cboVerses.Text = verseID.Substring(5, 3);

            ReadData rd = new ReadData(_conn);
            lblVerseText.Text = rd.GetVerseText(verseID);
        }

        private void TriggerVerseChange(string verseID)
        {
            int ID = Convert.ToInt32(verseID);

            var scrRef = new VerseRef(ID);
            if (!scrRef.Valid || _lastBBCCCVVV == scrRef.BBBCCCVVV)
                return;

            _lastBBCCCVVV = scrRef.BBBCCCVVV;

            int linkIndex = 1;
            if (radB.Checked)
            {
                linkIndex = 2;
            }
            else if(radC.Checked)
            {
                linkIndex = 3;
            }
            else if (radD.Checked)
            {
                linkIndex = 4;
            }
            else if (radE.Checked)
            {
                linkIndex = 5;
            }

            SantaFeFocusMessageHandler.SendFocusMessage(scrRef.Text + "LNK:" + linkIndex);
        }

        private void cboBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the number of chapters in the selected book
            int index = cboBooks.SelectedIndex;

            ReadData rd = new ReadData(_conn);
            var chapterList = rd.GetDistinctChapterIDs(index);

            cboChapters.Items.Clear();
            cboChapters.Items.AddRange(chapterList.ToArray());
            cboChapters.SelectedIndex = 0;
        }

        private void cboChapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the number of verses in the selected book/chapter
            int bookIndex = cboBooks.SelectedIndex;
            int index = cboChapters.SelectedIndex;

            ReadData rd = new ReadData(_conn);
            var versesList = rd.GetDistinctVerseIDs(bookIndex, index);

            cboVerses.Items.Clear();
            cboVerses.Items.AddRange(versesList.ToArray());
            cboVerses.SelectedIndex = 0;
        }

        private void cboVerses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadData rd = new ReadData(_conn);

            string verseID = (cboBooks.SelectedIndex + 1).ToString().PadLeft(2, '0') + cboChapters.Text +
                             cboVerses.Text;
            lblVerseText.Text = rd.GetVerseText(verseID);

            TriggerVerseChange(verseID);
        }
    }
}
