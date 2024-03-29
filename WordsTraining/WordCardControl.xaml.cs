﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for WordCardControl.xaml
    /// </summary>
    public partial class WordCardControl : UserControl, INotifyPropertyChanged
    {

        #region Properties

        // Get the list of possible word types
        public IEnumerable<WordType> WordTypeValues
        {
            get
            {
                return WordCard.WordTypes;
            }
        }

        // selected type
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

        // selected Card
        private WordCard _selectedCard;
        public WordCard SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                NotifyPropertyChanged("SelectedCard");
                if (value != null)
                    SelectedWordType = _selectedCard.Type;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public WordCardControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Reset1_Click(object sender, RoutedEventArgs e)
        {
            foreach (TrainingType type in WordCard.TrainingTypes)
                SelectedCard.Counter1[type] = 0;
        }

        private void Reset2_Click(object sender, RoutedEventArgs e)
        {
            foreach (TrainingType type in WordCard.TrainingTypes)
                SelectedCard.Counter2[type] = 0;
        }
    }
}
