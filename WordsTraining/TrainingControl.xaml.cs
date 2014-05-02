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
    /// Interaction logic for TrainingSettingsControl.xaml
    /// </summary>
    public partial class TrainingControl : UserControl 
    {        
        public TrainingControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void TrainingView_Loaded(object sender, RoutedEventArgs e)
        {
                     
        }

    }
}
