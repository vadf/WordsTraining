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

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for DictionariesView.xaml
    /// </summary>
    public partial class DictionariesControl : UserControl
    {
        private string dictionariesFolder = "dictionaries";
        private string extention = ".xml";

        public static DataLayer dataLayer;
        public static string selectedDictionary;

        public DictionariesControl()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            WordsDictionary newDictionary = new WordsDictionary(txtLang1.Text, txtLang2.Text);
            selectedDictionary = txtName.Text;
            dataLayer = new XmlDataLayer(GetFullPath(selectedDictionary));
            newDictionary.Add(new WordCard("Hello", "Hello", WordType.Noun)); // HACK: at least one card is needed to save
            dataLayer.Save(newDictionary);
            SwitchTab();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (listDictionaries.SelectedIndex == -1) return;
            selectedDictionary = listDictionaries.SelectedItem.ToString();
            dataLayer = new XmlDataLayer(GetFullPath(selectedDictionary));
            SwitchTab();
        }

        private void SwitchTab()
        {
            ((TabItem)MainWindow.tabControl.Items[1]).IsEnabled = true;
            ((TabItem)MainWindow.tabControl.Items[2]).IsEnabled = true;
            MainWindow.tabControl.SelectedIndex = 1;
        }
        /// <summary>
        /// Read all files from Dictionary folder
        /// </summary>
        /// <returns>List of file names</returns>
        private List<string> GetDictionariesList()
        {
            // Create Dictionaries folder if not exist
            if (!Directory.Exists(dictionariesFolder))
            {
                Directory.CreateDirectory(dictionariesFolder);
            }

            // read dictionaries list
            string[] dictionaries = Directory.GetFiles(dictionariesFolder, "*" + extention);
            var dict =
                from d in dictionaries
                select System.IO.Path.GetFileName(d).Replace(extention, "");
            return new List<string>(dict);
        }

        private void DictionariesView_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = GetDictionariesList();
            listDictionaries.ItemsSource = list;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listDictionaries.SelectedIndex == -1) return;
            string name = listDictionaries.SelectedItem.ToString();
            if (selectedDictionary == name)
            {
                ((TabItem)MainWindow.tabControl.Items[1]).IsEnabled = false;
                ((TabItem)MainWindow.tabControl.Items[2]).IsEnabled = false;
                dataLayer = null;
                selectedDictionary = null;
            }
            
            File.Delete(GetFullPath(name));
        }

        /// <summary>
        /// Create full path to xml file using dictionary name
        /// </summary>
        /// <param name="name">Dictionary name</param>
        /// <returns>Full path to xml</returns>
        private string GetFullPath(string name)
        {
            return System.IO.Path.Combine(dictionariesFolder, name + extention);
        }
    }
}
