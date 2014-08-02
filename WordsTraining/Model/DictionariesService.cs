using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsTraining.Model
{
    abstract public class DictionariesService
    {
        public ObservableCollection<DictionaryInfo> Dictionaries { get; protected set; } // TODO: Implement ObservableDictionary better

        public abstract DictionaryInfo GetDictionary(string name);
        public abstract bool AddDictionary(string name, string lang1, string lang2);

        public abstract bool RemoveDictionary(string name);
        public abstract bool RemoveDictionary(DictionaryInfo dictionary);
    }
}
