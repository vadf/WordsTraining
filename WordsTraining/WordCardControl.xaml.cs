using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for WordCardControl.xaml
    /// </summary>
    public partial class WordCardControl : UserControl, INotifyPropertyChanged
    {

        #region Properties

        // Get the list of possible word types
        public IEnumerable<WordType> WordTypeValues
        {
            get
            {
                return WordCard.WordTypes;
            }
        }

        // selected type
        private WordType _selectedWordType;
        public WordType SelectedWordType
        {
            get { return _selectedWordType; }
            set
            {
                _selectedWordType = value;
                NotifyPropertyChanged("SelectedWordType");
            }
        }

        // selected Card
        private WordCard _selectedCard;
        public WordCard SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                NotifyPropertyChanged("SelectedCard");
                if (value != null)
                    SelectedWordType = _selectedCard.Type;
            }
        }

        // Languages
        private string _lang1;
        public string Lang1
        {
            get { return _lang1; }
            set
            {
                _lang1 = value;
                NotifyPropertyChanged("Lang1");
            }
        }

        private string _lang2;
        public string Lang2
        {
            get { return _lang2; }
            set
            {
                _lang2 = value;
                NotifyPropertyChanged("Lang2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public WordCardControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Reset1_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard.Counter1[TrainingType.Writting] = 0;
            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }

        private void Reset2_Click(object sender, RoutedEventArgs e)
        {
            SelectedCard.Counter2[TrainingType.Writting] = 0;
            DictionariesControl.dataLayer.Save(DictionariesControl.selectedDictionary);
        }
    }
}
