using ScrollParatext.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScrollParatext.Messages;
using SIL.Scripture;
using Application = System.Windows.Forms.Application;
using Path = System.IO.Path;

namespace ScrollParatextWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _lastBBCCCVVV;
        private SQLiteConnection _conn;



        public MainWindow()
        {
            InitializeComponent();

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
            cboBooks.ItemsSource = bookList.ToArray();
        }

        private void Book_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the number of chapters in the selected book
            int index = cboBooks.SelectedIndex;

            ReadData rd = new ReadData(_conn);
            var chapterList = rd.GetDistinctChapterIDs(index);

            cboChapters.ItemsSource = chapterList.ToArray();
            cboChapters.SelectedIndex = 0;
            Chapter_SelectionChanged(null, null);
        }

        private void Chapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the number of verses in the selected book/chapter
            int bookIndex = cboBooks.SelectedIndex;
            int index = cboChapters.SelectedIndex;

            ReadData rd = new ReadData(_conn);
            var versesList = rd.GetDistinctVerseIDs(bookIndex, index);

            cboVerses.ItemsSource = versesList.ToArray();
            cboVerses.SelectedIndex = 0;
            Verse_SelectionChanged(null, null);
        }
        private void Verse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadData rd = new ReadData(_conn);

            string verseID = (cboBooks.SelectedIndex + 1).ToString().PadLeft(2, '0') + cboChapters.SelectedItem +
                             cboVerses.SelectedItem;
            lblVerseText.Text = rd.GetVerseText(verseID);

            TriggerVerseChange(verseID);
        }

        private void TriggerVerseChange(string verseID)
        {
            int ID = Convert.ToInt32(verseID);

            var scrRef = new VerseRef(ID);
            if (!scrRef.Valid || _lastBBCCCVVV == scrRef.BBBCCCVVV)
                return;

            _lastBBCCCVVV = scrRef.BBBCCCVVV;

            int linkIndex = 1;
            if ((bool)radB.IsChecked)
            {
                linkIndex = 2;
            }
            else if ((bool)radC.IsChecked)
            {
                linkIndex = 3;
            }
            else if ((bool)radD.IsChecked)
            {
                linkIndex = 4;
            }
            else if ((bool)radE.IsChecked)
            {
                linkIndex = 5;
            }

            SantaFeFocusMessageHandler.SendFocusMessage(scrRef.Text + "LNK:" + linkIndex);
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
                    if ((bool)radA.IsChecked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 1:
                    if ((bool)radB.IsChecked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 2:
                    if ((bool)radC.IsChecked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 3:
                    if ((bool)radD.IsChecked)
                    {
                        IncommingVerseChange(scrRef.BBBCCCVVV);
                    }
                    break;
                case 4:
                    if ((bool)radE.IsChecked)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<BookChapterVerse> bcv = new List<BookChapterVerse>();

            // init populating the comboboxes
            ReadData rd = new ReadData(_conn);
            var bookList = rd.GetDistinctBookIDs();
            cboBooks.ItemsSource = bookList.ToArray();
        }
    }
}
