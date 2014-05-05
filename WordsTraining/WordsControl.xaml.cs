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
    /// Interaction logic for WordsView.xaml
    /// </summary>
    public partial class WordsControl : UserControl
    {
        private WordsDictionary dictionary;
        
        public WordsControl()
        {
            InitializeComponent();
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

        private void UpdateWordView(int index)
        {
            WordCard card = dictionary[index];
            txtWord1.Text = card.Word1;
            txtComment1.Text = card.Comment1;
            txtWord2.Text = card.Word2;
            txtComment2.Text = card.Comment2;
            txtType.Text = card.Type.ToString();
            txtComment.Text = card.CommentCommon;
        }

        private void WordsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DictionariesControl.dataLayer != null)
            {
                dictionary = DictionariesControl.dataLayer.Read();
                DataContext = dictionary;
            }
        }
    }
}
