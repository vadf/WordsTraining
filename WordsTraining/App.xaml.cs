﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WordsTraining.Model;

namespace WordsTraining
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DictionaryInfo SelectedDictionary { get; set; }
    }
}
