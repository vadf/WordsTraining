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
        private bool isNew = false;
        private Visibility _commonVisibility = Visibility.Visible;
        private Visibility _saveVisibility = Visibility.Collapsed;

        public WordsDictionary MyDictionary { get; set; }

        public Visibility CommonVisibility
        {
            get { return _commonVisibility; }
            set
            {
                _commonVisibility = value;
                NotifyPropertyChanged("CommonVisibility");
            }
        }

        public Visibility SaveVisibility
        {
            get { return _saveVisibility; }
            set
            {
                _saveVisibility = value;
                NotifyPropertyChanged("SaveVisibility");
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
                WordCardElement.Lang1 = DictionariesControl.selectedDictionary.Language1;
                WordCardElement.Lang2 = DictionariesControl.selectedDictionary.Language2;
            }
            filtersPanel.Visibility = Visibility.Collapsed;
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
                WordCardElement.SelectedCard = MyDictionary[index];
                WordCardElement.SelectedWordType = WordCardElement.SelectedCard.Type;
                if (lang1Words.SelectedIndex != index)
                {
                    lang1Words.SelectedIndex = index;
                }
                if (lang2Words.SelectedIndex != index)
                {
                    lang2Words.SelectedIndex = index;
                }
            }
            CommonView();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            WordCardElement.SelectedCard = new WordCard("new", "new", WordType.Noun);
            isNew = true;
            UpdateView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WordCardElement.SelectedCard.Word1 = WordCardElement.txtWord1.Text;
            WordCardElement.SelectedCard.Comment1 = WordCardElement.txtComment1.Text;
            WordCardElement.SelectedCard.Word2 = WordCardElement.txtWord2.Text;
            WordCardElement.SelectedCard.Comment2 = WordCardElement.txtComment2.Text;
            WordCardElement.SelectedCard.Type = (WordType)WordCardElement.txtType.SelectedItem;
            WordCardElement.SelectedCard.CommentCommon = WordCardElement.txtComment.Text;
            if (isNew)
            {
                DictionariesControl.selectedDictionary.Insert(0, WordCardElement.SelectedCard);
            }
            CommonView();

            lang1Words.Items.Refresh();
            lang2Words.Items.Refresh();

            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DictionariesControl.selectedDictionary.Remove(WordCardElement.SelectedCard);
            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CommonView();
            WordCardElement.SelectedCard = null;
        }

        private void CommonView()
        {
            CommonVisibility = Visibility.Visible;
            SaveVisibility = Visibility.Collapsed;
            isNew = false;
        }

        private void UpdateView()
        {
            CommonVisibility = Visibility.Collapsed;
            SaveVisibility = Visibility.Visible;
        }

        private void Filters_Click(object sender, RoutedEventArgs e)
        {
            filtersPanel.Visibility = Visibility.Visible;
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            filtersPanel.Visibility = Visibility.Collapsed;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var filter =
                from c in DictionariesControl.selectedDictionary
                where c.Word1.Contains("a") || c.Word2.Contains("a")
                select c;
            MyDictionary = filter as WordsDictionary;
            lang1Words.Items.Refresh();
            lang2Words.Items.Refresh();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MyDictionary = DictionariesControl.selectedDictionary;
            lang1Words.Items.Refresh();
            lang2Words.Items.Refresh();
        }
    }
}
