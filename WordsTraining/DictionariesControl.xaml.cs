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
using System.IO;

using WordsTraining.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for DictionariesView.xaml
    /// </summary>
    public partial class DictionariesControl : UserControl
    {
        private DictionariesService dictionariesService;

        public static DataLayer dataLayer;
        public static WordsDictionary selectedDictionary;

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
            SelectedDictionary = DictionariesList.Last();
            dataLayer = SelectedDictionary.DataLayer;
            selectedDictionary = SelectedDictionary.Dictionary;
            SwitchTab();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDictionary != null)
            {
                dataLayer = SelectedDictionary.DataLayer;
                selectedDictionary = SelectedDictionary.Dictionary;
                SwitchTab();
            }
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
            if (dataLayer != null && selectedDictionary != null)
                dataLayer.Save(selectedDictionary);
            dataLayer = null;
            selectedDictionary = null;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDictionary != null)
            {
                dictionariesService.RemoveDictionary(SelectedDictionary);
            }
        }
    }
}
