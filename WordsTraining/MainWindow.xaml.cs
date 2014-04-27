using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dictionariesFolder = "dictionaries";
        private WordsDictionary wordsDictionary;


        public List<String> MyDictionaries
        {
            get;
            set;
        }
        

        public MainWindow()
        {
            InitializeComponent();

            // Create Dictionaries folder if not exist
            if (!Directory.Exists(dictionariesFolder))
            {
                Directory.CreateDirectory(dictionariesFolder);
            }

            // read dictionaries list
            MyDictionaries = new List<string>();
            string [] dictionaries = Directory.GetFiles(dictionariesFolder);
            foreach (var file in dictionaries)
            {
                MyDictionaries.Add(file.Replace(".xml", ""));
            }

            listDictionaries.ItemsSource = MyDictionaries;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (listDictionaries.SelectedIndex == -1) return;
            string pathToXMl = listDictionaries.SelectedItem.ToString() + ".xml";
            wordsDictionary = new XmlDataLayer(pathToXMl).Read();
            DataContext = wordsDictionary;
        }

        private void lang1Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lang2Words.SelectedIndex = lang1Words.SelectedIndex;
            UpdateWordView(lang1Words.SelectedIndex);
        }

        private void lang2Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lang1Words.SelectedIndex = lang2Words.SelectedIndex;
            UpdateWordView(lang1Words.SelectedIndex);
        }

        private void UpdateWordView(int position)
        {
            wordsDictionary.SelectedIndex = position;
            WordCard card = wordsDictionary.SelectedCard;
            card.SelectedLanguage = WordsTraining.Language.Lang1;
            txtWord1.Text = card.Word;
            txtComment1.Text = card.Comment;
            card.SelectedLanguage = WordsTraining.Language.Lang2;
            txtWord2.Text = card.Word;
            txtComment2.Text = card.Comment;
            txtType.Text = card.Type.ToString();
            txtComment.Text = card.Comment;
        }
    }
}
