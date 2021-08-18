using ScrollParatext.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ScrollParatextWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> _bookNames;
        public ObservableCollection<string> BookNames
        {
            get => _bookNames;
            set => _bookNames = value;
        }


        private ObservableCollection<int> _chapNums;
        public ObservableCollection<int> ChapNums
        {
            get => _chapNums;
            set => _chapNums = value;
        }


        private ObservableCollection<int> _verseNums;
        public ObservableCollection<int> VerseNums
        {
            get => _verseNums;
            set => _verseNums = value;
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Book_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Chapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Verse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<BookChapterVerse> bcv = new List<BookChapterVerse>();


            // load up the combo box with the books
            BookNames = new ObservableCollection<string>();
            foreach (var item in bcv)
            {
                BookNames.Add(item.BookName);
            }
        }
    }
}
