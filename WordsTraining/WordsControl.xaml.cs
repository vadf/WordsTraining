using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for WordsView.xaml
    /// </summary>
    public partial class WordsControl : UserControl, INotifyPropertyChanged
    {
        private WordCard _selectedCard;

        public WordsDictionary MyDictionary { get; set; }
        public WordCard SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                NotifyPropertyChanged("SelectedCard");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Get the list of possible word types
        public IEnumerable<WordType> WordTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(WordType)).Cast<WordType>();
            }
        }

        // selected typed
        private WordType _selectedWordType;
        public WordType SelectedWordType
        {
            get { return _selectedWordType; }
            set
            {
                _selectedWordType = value;
                NotifyPropertyChanged("SelectedWordType");
            }
        }

        public WordsControl()
        {
            InitializeComponent();
        }

        private void WordsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DictionariesControl.selectedDictionary != null && DictionariesControl.selectedDictionary != MyDictionary)
            {
                MyDictionary = DictionariesControl.selectedDictionary;
                DataContext = this;
                UpdateWordView(0);
            }
        }

        private void WordsView_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void lang1Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWordView(lang1Words.SelectedIndex);
        }

        private void lang2Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWordView(lang2Words.SelectedIndex);
        }

        private void UpdateWordView(int index)
        {
            if (index >= 0 && index < MyDictionary.Count)
            {
                SelectedCard = MyDictionary[index];
                SelectedWordType = SelectedCard.Type;
                if (lang1Words.SelectedIndex != index)
                {
                    lang1Words.SelectedIndex = index;
                }
                if (lang2Words.SelectedIndex != index)
                {
                    lang2Words.SelectedIndex = index;
                }
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var card = new WordCard("new", "new", WordType.Noun);
            MyDictionary.Insert(0, card);
            UpdateWordView(0);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard.Word1 = txtWord1.Text;
            SelectedCard.Comment1 = txtComment1.Text;
            SelectedCard.Word2 = txtWord2.Text;
            SelectedCard.Comment2 = txtComment2.Text;
            SelectedCard.Type = (WordType)txtType.SelectedItem;
            SelectedCard.CommentCommon = txtComment.Text;
            lang1Words.Items.Refresh();
            lang2Words.Items.Refresh();

            DictionariesControl.dataLayer.Save(MyDictionary);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            int index = lang1Words.SelectedIndex;
            lang1Words.SelectedIndex = 0;
            lang2Words.SelectedIndex = 0;
            MyDictionary.RemoveAt(index);
        }

        private void Reset1_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard.Counter1 = 0;
            DictionariesControl.dataLayer.Save(MyDictionary);
        }

        private void Reset2_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard.Counter2 = 0;
            DictionariesControl.dataLayer.Save(MyDictionary);
        }
    }
}
