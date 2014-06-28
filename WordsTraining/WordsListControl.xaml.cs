using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for WordsView.xaml
    /// </summary>
    public partial class WordsControl : UserControl, INotifyPropertyChanged
    {
        #region Properties and Variables

        private bool isNew = false;
        private Visibility _commonVisibility = Visibility.Visible;
        private Visibility _saveVisibility = Visibility.Collapsed;
        private ObservableCollection<WordCard> dictionary;

        public ObservableCollection<WordCard> MyDictionary
        {
            get { return dictionary; }
            set
            {
                dictionary = value;
                NotifyPropertyChanged("MyDictionary");
            }
        }

        // Languages
        private string _lang1;
        public string Lang1
        {
            get { return _lang1; }
            set
            {
                _lang1 = value;
                NotifyPropertyChanged("Lang1");
            }
        }

        private string _lang2;
        public string Lang2
        {
            get { return _lang2; }
            set
            {
                _lang2 = value;
                NotifyPropertyChanged("Lang2");
            }
        }

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

        #endregion

        #region Initialization and Card Selection

        public WordsControl()
        {
            InitializeComponent();
        }

        private void WordsView_Loaded(object sender, RoutedEventArgs e)
        {
            // set dictionary and languages
            if (DictionariesControl.selectedDictionary != null && DictionariesControl.selectedDictionary != MyDictionary)
            {
                //MyDictionary = DictionariesControl.selectedDictionary;
                DataContext = this;
                ResetFilter_Click(sender, e);
                UpdateWordView(0);
                Lang1 = DictionariesControl.selectedDictionary.Language1;
                Lang2 = DictionariesControl.selectedDictionary.Language2;
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

        // Set SelectedCard when some word selected from list
        private void UpdateWordView(int index)
        {
            if (index >= 0 && index < MyDictionary.Count)
            {
                WordCardElement.SelectedCard = MyDictionary[index];
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

        #endregion

        #region Cards Action

        // prepation to add new card
        private void New_Click(object sender, RoutedEventArgs e)
        {
            WordCardElement.SelectedCard = new WordCard("new", "new", WordType.Noun);
            isNew = true;
            UpdateView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // save card deatils
            WordCardElement.SelectedCard.Word1 = WordCardElement.txtWord1.Text;
            WordCardElement.SelectedCard.Comment1 = WordCardElement.txtComment1.Text;
            WordCardElement.SelectedCard.Word2 = WordCardElement.txtWord2.Text;
            WordCardElement.SelectedCard.Comment2 = WordCardElement.txtComment2.Text;
            WordCardElement.SelectedCard.Type = (WordType)WordCardElement.txtType.SelectedItem;
            WordCardElement.SelectedCard.CommentCommon = WordCardElement.txtComment.Text;
            if (isNew)
            {
                // if new card, add it to dictionary
                // TODO: think for more elegance solution
                MyDictionary.Insert(0, WordCardElement.SelectedCard);
                if (!DictionariesControl.selectedDictionary.Contains(WordCardElement.SelectedCard))
                    DictionariesControl.selectedDictionary.Insert(0, WordCardElement.SelectedCard);

            }
            CommonView();

            // refresh words lists if some word 'names' are changed
            lang1Words.Items.Refresh();
            lang2Words.Items.Refresh();

            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }

        // remove card from dictionary
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            MyDictionary.Remove(WordCardElement.SelectedCard);
            if (!DictionariesControl.selectedDictionary.Contains(WordCardElement.SelectedCard))
                DictionariesControl.selectedDictionary.Remove(WordCardElement.SelectedCard);
            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
            WordCardElement.SelectedCard = null;
        }

        // set update mode
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }

        // close update mode
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CommonView();
            UpdateWordView(lang1Words.SelectedIndex);
        }

        // set common view mode
        private void CommonView()
        {
            CommonVisibility = Visibility.Visible;
            SaveVisibility = Visibility.Collapsed;
            isNew = false;
            WordCardElement.IsEnabled = false;
        }

        // set update card view
        private void UpdateView()
        {
            CommonVisibility = Visibility.Collapsed;
            SaveVisibility = Visibility.Visible;
            WordCardElement.IsEnabled = true;
        }

        #endregion

        #region Filters

        private string any = "Any";

        // Get the list of possible word types for filter
        public List<String> WordTypeValues
        {
            get
            {
                List<string> list = Enum.GetNames(typeof(WordType)).ToList();
                list.Insert(0, any);
                return list;
            }
        }

        // Get the list of possible filter type values for counters
        public List<String> CounterFilterTypeValues
        {
            get
            {
                List<string> list = Enum.GetNames(typeof(CounterFilterType)).ToList();
                list.Insert(0, any);
                return list;
            }
        }

        // Get the list of possible training types for filter
        public List<String> TrainingTypeValues
        {
            get
            {
                List<string> list = Enum.GetNames(typeof(TrainingType)).ToList();
                list.Insert(0, any);
                return list;
            }
        }

        // selected filer type
        private string _selectedFilterType;
        public string SelectedFilterType
        {
            get { return _selectedFilterType; }
            set
            {
                _selectedFilterType = value;
                NotifyPropertyChanged("SelectedFilterType");
            }
        }

        // selected word type
        private string _selectedWordType;
        public string SelectedWordTypeFilter
        {
            get { return _selectedWordType; }
            set
            {
                _selectedWordType = value;
                NotifyPropertyChanged("SelectedWordTypeFilter");
            }
        }

        // selected training type
        private string _selectedTrainingType;
        public string SelectedTrainingTypeFilter
        {
            get { return _selectedTrainingType; }
            set
            {
                _selectedTrainingType = value;
                NotifyPropertyChanged("SelectedTrainingTypeFilter");
            }
        }

        // show filters
        private void Filters_Click(object sender, RoutedEventArgs e)
        {
            filtersPanel.Visibility = Visibility.Visible;
        }

        // hide filters
        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            filtersPanel.Visibility = Visibility.Collapsed;
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<WordCard> result = DictionariesControl.selectedDictionary;
            if (txtWordFilter.Text != "")
                result = CardsFilter.FilterByWord(result, txtWordFilter.Text);
            if (comboWordTypeFilter.SelectedValue.ToString() != any)
                result = CardsFilter.FilterByType(result, (WordType)Enum.Parse(typeof(WordType), comboWordTypeFilter.SelectedItem.ToString()));

            if (comboCounterFilter.SelectedValue.ToString() != any && comboCounterTrainingType.SelectedItem.ToString() != any)
            {
                int counter = 0;
                int.TryParse(txtCounterFilter.Text, out counter);
                result = CardsFilter.FilterByCounter(result,
                    (CounterFilterType)Enum.Parse(typeof(CounterFilterType), comboCounterFilter.SelectedItem.ToString()),
                    counter,
                    (TrainingType)Enum.Parse(typeof(TrainingType), comboCounterTrainingType.SelectedItem.ToString()));
            }
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        // reset filters
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            txtWordFilter.Text = "";
            comboWordTypeFilter.SelectedIndex = 0;
            comboCounterTrainingType.SelectedIndex = 0;
            comboCounterFilter.SelectedIndex = 0;
            txtCounterFilter.Text = "";
            MyDictionary = DictionariesControl.selectedDictionary;
        }

        #endregion

        #region Sorting

        private void Word1Asc_Click(object sender, RoutedEventArgs e)
        {
            var result = CardsFilter.SortByWord1Asc(MyDictionary);
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        private void Word1Desc_Click(object sender, RoutedEventArgs e)
        {
            var result = CardsFilter.SortByWord1Desc(MyDictionary);
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        private void Word2Asc_Click(object sender, RoutedEventArgs e)
        {
            var result = CardsFilter.SortByWord2Asc(MyDictionary);
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        private void Word2Desc_Click(object sender, RoutedEventArgs e)
        {
            var result = CardsFilter.SortByWord2Desc(MyDictionary);
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        #endregion
    }
}
