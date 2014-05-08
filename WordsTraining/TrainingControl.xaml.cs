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

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for TrainingSettingsControl.xaml
    /// </summary>
    public partial class TrainingControl : UserControl
    {
        private Language langFrom = WordsTraining.Language.Lang1;
        private Language langTo = WordsTraining.Language.Lang2;
        private Training training;
        private WordCard card;
        private WordsDictionary dictionary;

        public int NumOfWords { get; set; }
        public int Counter { get; set; }

        public TrainingControl()
        {
            InitializeComponent();
        }

        private void TrainingView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DictionariesControl.dataLayer != null)
            {
                NumOfWords = 10;
                Counter = 3;
                dictionary = DictionariesControl.dataLayer.Read();
                SetDirectionText(dictionary);
                DataContext = this;
            }
        }

        private void btnSwitchDirection_Click(object sender, RoutedEventArgs e)
        {
            if (langFrom == WordsTraining.Language.Lang1)
            {
                langFrom = WordsTraining.Language.Lang2;
                langTo = WordsTraining.Language.Lang1;
            }
            else
            {
                langFrom = WordsTraining.Language.Lang1;
                langTo = WordsTraining.Language.Lang2;
            }
            SetDirectionText(dictionary);
        }

        private void SetDirectionText(WordsDictionary dictionary)
        {
            if (dictionary != null)
            {
                lbDirectionValue.Text = dictionary.DictinaryLanguages[langFrom] + " -> " + dictionary.DictinaryLanguages[langTo];
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            lbFromValue.Text = dictionary.DictinaryLanguages[langFrom];
            lbToValue.Text = dictionary.DictinaryLanguages[langTo];
            training = new Training(dictionary, langFrom, langTo, NumOfWords, Counter);
            card = training.NextCard();
            if (card != null)
                UpdateTrainingCard(card);
        }

        private void UpdateTrainingCard(WordCard card)
        {
            card.SelectedLanguage = langFrom;
            txtFrom.Text = card.Word;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            card.SelectedLanguage = langTo;
            if (txtTo.Text.ToLower() == card.Word.ToLower())
            {
                lbResultValue.Text = "Correct";
                training.Succeeded();
            }
            else
            {
                lbResultValue.Text = "Incorrect. " + card.Word;
            }
        }

        private void btnNetx_Click(object sender, RoutedEventArgs e)
        {
            txtFrom.Text = "";
            txtTo.Text = "";
            lbResultValue.Text = "";
            card = training.NextCard();
            if (card != null)
                UpdateTrainingCard(card);
        }

    }
}
