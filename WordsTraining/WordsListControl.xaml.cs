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

        private WordCard _selectedCard;
        public WordCard SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                WordCardElement.SelectedCard = _selectedCard;
                NotifyPropertyChanged("SelectedCard");
                CommonView();
            }
        }

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

        private Visibility _commonVisibility = Visibility.Visible;
        public Visibility CommonVisibility
        {
            get { return _commonVisibility; }
            set
            {
                _commonVisibility = value;
                NotifyPropertyChanged("CommonVisibility");
            }
        }

        private Visibility _saveVisibility = Visibility.Collapsed;
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
                Lang1 = DictionariesControl.selectedDictionary.Language1;
                Lang2 = DictionariesControl.selectedDictionary.Language2;
                WordCardElement.Lang1 = DictionariesControl.selectedDictionary.Language1;
                WordCardElement.Lang2 = DictionariesControl.selectedDictionary.Language2;
            }
            filtersPanel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Cards Action

        // prepation to add new card
        private void New_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard = new WordCard("new", "new", WordType.Noun);
            isNew = true;
            UpdateView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // save card deatils
            SelectedCard.Word1 = WordCardElement.txtWord1.Text;
            SelectedCard.Comment1 = WordCardElement.txtComment1.Text;
            SelectedCard.Word2 = WordCardElement.txtWord2.Text;
            SelectedCard.Comment2 = WordCardElement.txtComment2.Text;
            SelectedCard.Type = (WordType)WordCardElement.txtType.SelectedItem;
            SelectedCard.CommentCommon = WordCardElement.txtComment.Text;
            if (isNew)
            {
                // if new card, add it to dictionary
                // TODO: think for more elegance solution
                MyDictionary.Insert(0, SelectedCard);
                if (!DictionariesControl.selectedDictionary.Contains(SelectedCard))
                    DictionariesControl.selectedDictionary.Insert(0, SelectedCard);

            }
            CommonView();

            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }

        // remove card from dictionary
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            MyDictionary.Remove(SelectedCard);
            if (!DictionariesControl.selectedDictionary.Contains(SelectedCard))
                DictionariesControl.selectedDictionary.Remove(SelectedCard);
            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
            SelectedCard = null;
        }

        // set update mode
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }

        // close update mode
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (isNew) SelectedCard = null;
            CommonView();
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

        // selected filter type
        private string _selectedCounterFilterType;
        public string SelectedCounterFilterType
        {
            get { return _selectedCounterFilterType; }
            set
            {
                _selectedCounterFilterType = value;
                NotifyPropertyChanged("SelectedCounterFilterType");
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
                result = CardsFilter.FilterByType(result, (WordType)Enum.Parse(typeof(WordType), SelectedWordTypeFilter));

            if (SelectedCounterFilterType != any && SelectedTrainingTypeFilter != any)
            {
                int counter = 0;
                int.TryParse(txtCounterFilter.Text, out counter);
                result = CardsFilter.FilterByCounter(result,
                    (CounterFilterType)Enum.Parse(typeof(CounterFilterType), SelectedCounterFilterType),
                    counter,
                    (TrainingType)Enum.Parse(typeof(TrainingType), SelectedTrainingTypeFilter));
            }
            MyDictionary = new ObservableCollection<WordCard>(result);
        }

        // word search
        private void txtWordFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {

                if (txtWordFilter.Text == "")
                {
                    MyDictionary = DictionariesControl.selectedDictionary;
                }
                else
                {
                    IEnumerable<WordCard> result = CardsFilter.FilterByWord(DictionariesControl.selectedDictionary, txtWordFilter.Text);
                    MyDictionary = new ObservableCollection<WordCard>(result);
                }
            }
        }

        // reset filters
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            txtWordFilter.Text = "";
            SelectedWordTypeFilter = any;
            SelectedTrainingTypeFilter = any;
            SelectedCounterFilterType = any;
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
