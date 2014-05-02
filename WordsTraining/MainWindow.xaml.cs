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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string dictionariesFolder = "dictionaries";

        public MainWindow()
        {
            InitializeComponent();
        }

        public static List<string> GetDictionariesList()
        {           
            // Create Dictionaries folder if not exist
            if (!Directory.Exists(dictionariesFolder))
            {
                Directory.CreateDirectory(dictionariesFolder);
            }

            // read dictionaries list
            string[] dictionaries = Directory.GetFiles(dictionariesFolder);
            return new List<string>(dictionaries);
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
