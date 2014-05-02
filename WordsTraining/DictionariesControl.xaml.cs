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
        public List<String> MyDictionaries { get; set; }

        public static WordsDictionary LoadedDictionary = null;

        public DictionariesControl()
        {
            InitializeComponent();
            MyDictionaries = MainWindow.GetDictionariesList();
            DataContext = this;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (listDictionaries.SelectedIndex == -1) return;
            string pathToXMl = listDictionaries.SelectedItem.ToString();
            LoadedDictionary = new XmlDataLayer(pathToXMl).Read();
        }
    }
}
