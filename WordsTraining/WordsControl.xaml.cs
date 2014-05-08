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
        public WordsDictionary MyDictionary { get; set; }
        private string selectedDictionary;

        public WordsControl()
        {
            InitializeComponent();
        }

        private void WordsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DictionariesControl.selectedDictionary != null && DictionariesControl.selectedDictionary != selectedDictionary)
            {
                MyDictionary = DictionariesControl.dataLayer.Read();
                DataContext = this;
                selectedDictionary = DictionariesControl.selectedDictionary;
                lang1Words.SelectedIndex = 0;
                lang2Words.SelectedIndex = 0;
            }
        }

        private void WordsView_Unloaded(object sender, RoutedEventArgs e)
        {
            DictionariesControl.dataLayer.Save(MyDictionary);
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
            WordCard card = MyDictionary[index];
            txtWord1.Text = card.Word1;
            txtComment1.Text = card.Comment1;
            txtWord2.Text = card.Word2;
            txtComment2.Text = card.Comment2;
            txtType.Text = card.Type.ToString();
            txtComment.Text = card.CommentCommon;
        }



        private void New_Click(object sender, RoutedEventArgs e)
        {
            MyDictionary.Add(new WordCard("new", "new", WordType.Noun));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WordCard card = MyDictionary[lang1Words.SelectedIndex];
            card.Word1 = txtWord1.Text;
            card.Comment1 = txtComment1.Text;
            card.Word2 = txtWord2.Text;
            card.Comment2 = txtComment2.Text;
            // card.Type = txtType.Text;
            card.CommentCommon = txtComment.Text;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            int index = lang1Words.SelectedIndex;
            lang1Words.SelectedIndex = 0;
            lang2Words.SelectedIndex = 0;
            MyDictionary.RemoveAt(index);
        }
    }
}
