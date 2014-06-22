﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for TrainingSettingsControl.xaml
    /// </summary>
    public partial class TrainingControl : UserControl
    {
        private Language langFrom = Model.Language.Lang1;
        private Language langTo = Model.Language.Lang2;
        private Training training;
        private WordsDictionary dictionary;
        private Dictionary<Language, string> languages = new Dictionary<Language, string>();
        private bool isSwitched = false;

        public int NumOfWords { get; set; }
        public int Counter { get; set; }

        public TrainingControl()
        {
            InitializeComponent();
            NumOfWords = 10;
            Counter = 3;
        }

        private void TrainingView_Loaded(object sender, RoutedEventArgs e)
        {
            trainingTest.Visibility = Visibility.Collapsed;
            trainingSetting.Visibility = Visibility.Collapsed;
            if (DictionariesControl.dataLayer != null)
            {
                dictionary = DictionariesControl.selectedDictionary;
                DataContext = this;
                languages.Clear();
                languages.Add(Model.Language.Lang1, dictionary.Language1);
                languages.Add(Model.Language.Lang2, dictionary.Language2);
                SetDirectionText(dictionary);
                trainingSetting.Visibility = Visibility.Visible;
                trainingSetting.IsEnabled = true;
            }
        }

        private void btnSwitchDirection_Click(object sender, RoutedEventArgs e)
        {
            if (langFrom == Model.Language.Lang1)
            {
                langFrom = Model.Language.Lang2;
                langTo = Model.Language.Lang1;
            }
            else
            {
                langFrom = Model.Language.Lang1;
                langTo = Model.Language.Lang2;
            }
            isSwitched = !isSwitched;
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
            WordCardElement.Lang1 = languages[langFrom];
            WordCardElement.Lang2 = languages[langTo];
            training = new Training(dictionary, isSwitched, NumOfWords, Counter);
            WordCardElement.SelectedCard = training.NextCard();
            if (WordCardElement.SelectedCard != null)
            {
                trainingTest.Visibility = Visibility.Visible;
                trainingSetting.IsEnabled = false;
                btnCheck.IsEnabled = true;

                WordCardElement.IsEnabled = false;
                WordCardElement.Counter1.Visibility = Visibility.Collapsed;
                WordCardElement.Counter2.Visibility = Visibility.Collapsed;
                WordCardElement.txtWord1.FontWeight = FontWeights.Bold;
                WordCardElement.txtWord2.FontWeight = FontWeights.Bold;
                WordCardElement.txtWord1.Background = Brushes.GreenYellow; // or (Brush)bc.ConvertFrom("#FFXXXXXX");
                WordCardElement.txtWord2.Background = Brushes.Gray;
                txtAnswer.Focus();
                UpdateTrainingCard();
            }
        }

        private void UpdateTrainingCard()
        {
            WordCardElement.txtWord2.FontSize = 0.1;
            WordCardElement.txtComment2.FontSize = 0.1;
            btnCheck.IsEnabled = true;
            txtAnswer.Text = "";
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (txtAnswer.Text.ToLower() == WordCardElement.SelectedCard.Word2.ToLower())
            {
                lbResultValue.Text = "Correct";
                lbResultValue.Foreground = Brushes.Green;
                training.Succeeded();
            }
            else
            {
                lbResultValue.Text = "Incorrect";
                lbResultValue.Foreground = Brushes.IndianRed;
            }
            WordCardElement.txtWord2.FontSize = WordCardElement.txtWord1.FontSize;
            WordCardElement.txtComment2.FontSize = WordCardElement.txtComment1.FontSize;
            btnCheck.IsEnabled = false;
        }

        private void btnNetx_Click(object sender, RoutedEventArgs e)
        {
            lbResultValue.Text = "";
            WordCardElement.SelectedCard = training.NextCard();
            if (WordCardElement.SelectedCard != null)
                UpdateTrainingCard();
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
            trainingSetting.IsEnabled = true;
            trainingTest.Visibility = Visibility.Collapsed;
            training.Close();
            training = null;
            DictionariesControl.dataLayer.Save(dictionary);
        }

        private void txtAnswer_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (btnCheck.IsEnabled == true)
                    btnCheck_Click(sender, e);
                else
                    btnNetx_Click(sender, e);
            }
        }

        private void TrainingView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (training != null)
                training.Close();
        }

    }
}
