using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        private Dictionary<WordsTraining.Language, string> languages = new Dictionary<Language, string>();

        public int NumOfWords { get; set; }
        public int Counter { get; set; }

        public TrainingControl()
        {
            InitializeComponent();
        }

        private void TrainingView_Loaded(object sender, RoutedEventArgs e)
        {
            trainingTest.Visibility = Visibility.Collapsed;
            trainingSetting.Visibility = Visibility.Collapsed;
            if (DictionariesControl.dataLayer != null)
            {
                NumOfWords = 10;
                Counter = 3;
                dictionary = DictionariesControl.selectedDictionary;
                DataContext = this;
                languages.Clear();
                languages.Add(WordsTraining.Language.Lang1, dictionary.Language1);
                languages.Add(WordsTraining.Language.Lang2, dictionary.Language2);
                SetDirectionText(dictionary);
                trainingSetting.Visibility = Visibility.Visible;
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
                lbDirectionValue.Text = languages[langFrom] + " -> " + languages[langTo];
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            lbFromValue.Text = languages[langFrom];
            lbToValue.Text = languages[langTo];
            training = new Training(dictionary, langFrom, langTo, NumOfWords, Counter);
            card = training.NextCard();
            if (card != null)
            {
                trainingTest.Visibility = Visibility.Visible;
                trainingSetting.Visibility = Visibility.Collapsed;
                UpdateTrainingCard(card);
            }
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
            else
                ShowResult();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ShowResult();
        }

        private void ShowResult()
        {
            MessageBox.Show(string.Format("Total words in training {0}, correct answers {1}", training.TotalCards, training.CorrectAnswers));
            trainingTest.Visibility = Visibility.Collapsed;
            trainingSetting.Visibility = Visibility.Visible;
            training = null;
            DictionariesControl.dataLayer.Save(dictionary);
        }

    }
}
