﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for DictionariesView.xaml
    /// </summary>
    public partial class DictionariesControl : UserControl, INotifyPropertyChanged
    {
        private DictionariesService dictionariesService;
        private List<DictionaryInfo> toRemove = new List<DictionaryInfo>();

        #region Properties

        private string _name;
        public string DictionaryName
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("DictionaryName");
            }
        }

        private string _lang1;
        public string Language1
        {
            get { return _lang1; }
            set
            {
                _lang1 = value;
                NotifyPropertyChanged("Language1");
            }
        }

        private string _lang2;
        public string Language2
        {
            get { return _lang2; }
            set
            {
                _lang2 = value;
                NotifyPropertyChanged("Language2");
            }
        }

        private DictionaryInfo _selectedDictionary;
        public DictionaryInfo SelectedDictionary
        {
            get { return _selectedDictionary; }
            set
            {
                _selectedDictionary = value;
                NotifyPropertyChanged("SelectedDictionary");
            }
        }

        public ObservableCollection<DictionaryInfo> DictionariesList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public DictionariesControl()
        {
            InitializeComponent();

            string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WordsTraining");
            dictionariesService = new XmlDictionariesService(folder);
            DictionariesList = dictionariesService.Dictionaries;
            DataContext = this;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            dictionariesService.AddDictionary(DictionaryName, Language1, Language2);
            App.SelectedDictionary = DictionariesList.Last();
            SwitchTab();
        }

        private void SwitchTab()
        {
            ((TabItem)MainWindow.tabControl.Items[1]).IsEnabled = true;
            ((TabItem)MainWindow.tabControl.Items[2]).IsEnabled = true;
            MainWindow.tabControl.SelectedIndex = 1;
        }

        private void DictionariesView_Loaded(object sender, RoutedEventArgs e)
        {
            ((TabItem)MainWindow.tabControl.Items[1]).IsEnabled = false;
            ((TabItem)MainWindow.tabControl.Items[2]).IsEnabled = false;
            if (App.SelectedDictionary != null)
                App.SelectedDictionary.DataLayer.Save(App.SelectedDictionary.Dictionary);
            toRemove.Clear();
            NewDictionaryPanel.Visibility = System.Windows.Visibility.Collapsed;
            DictionaryDetailsPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var dict in toRemove)
            {
                dictionariesService.RemoveDictionary(dict);
            }
            toRemove.Clear();
        }

        private void OpenDictionary(object sender, RoutedEventArgs e)
        {
            DictionaryInfo dict = GetSelectedDictionary(sender);
            if (dict == null) return;

            App.SelectedDictionary = dict;
            SwitchTab();
        }

        private void AddToRemoveList(object sender, RoutedEventArgs e)
        {
            DictionaryInfo dict = GetSelectedDictionary(sender);
            if (dict == null) return;

            toRemove.Add(dict);
        }

        private void DelFromRemoveList(object sender, RoutedEventArgs e)
        {
            DictionaryInfo dict = GetSelectedDictionary(sender);
            if (dict == null) return;

            toRemove.Remove(dict);
        }

        private DictionaryInfo GetSelectedDictionary(object sender)
        {
            FrameworkElement element = (FrameworkElement)sender;
            return element.DataContext as DictionaryInfo;
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            cb.IsChecked = false;
        }

        private void NewDictionary(object sender, RoutedEventArgs e)
        {
            NewDictionaryPanel.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowDetails(object sender, RoutedEventArgs e)
        {
            DictionaryDetailsPanel.Visibility = System.Windows.Visibility.Visible;
            SelectedDictionary = GetSelectedDictionary(sender);
        }

        private void HideDetails(object sender, RoutedEventArgs e)
        {
            DictionaryDetailsPanel.Visibility = System.Windows.Visibility.Collapsed;
            NewDictionaryPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
