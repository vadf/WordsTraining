using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsTraining.Model
{
    public class XmlDictionariesService : DictionariesService
    {
        private string dictionariesFolder;
        private string extention = ".xml";

        public XmlDictionariesService(string dictionariesFolder)
        {
            this.dictionariesFolder = dictionariesFolder;

            // Create Dictionaries folder if not exist
            if (!Directory.Exists(dictionariesFolder))
            {
                Directory.CreateDirectory(dictionariesFolder);
            }

            Dictionaries = new ObservableCollection<DictionaryInfo>();

            // read dictionaries files list
            string[] dictionaries = Directory.GetFiles(dictionariesFolder, "*" + extention);

            // create dictionary info for each file
            foreach (var item in dictionaries)
            {
                Dictionaries.Add(new DictionaryInfo(System.IO.Path.GetFileName(item).Replace(extention, ""), new XmlDataLayer(item)));
            }
        }

        public override bool RemoveDictionary(string name)
        {
            return RemoveDictionary(GetDictionary(name));
        }

        public override bool RemoveDictionary(DictionaryInfo dictionary)
        {
            if (dictionary == null)
                return false;
            File.Delete(GetFullPath(dictionary.Name));
            return Dictionaries.Remove(dictionary);
        }

        public override bool AddDictionary(string name, string lang1, string lang2)
        {
            DictionaryInfo check = GetDictionary(name);
            if (check != null)
                return false;

            WordsDictionary dictionary = new WordsDictionary(lang1, lang2);
            dictionary.Add(new WordCard("Hello", "Hello", WordType.Noun)); // HACK:
            DataLayer data = new XmlDataLayer(GetFullPath(name));
            data.Save(dictionary);

            DictionaryInfo dictionaryInfo = new DictionaryInfo(name, data);
            Dictionaries.Add(dictionaryInfo);

            return true;
        }

        public override DictionaryInfo GetDictionary(string name)
        {
            var dictionary =
                from d in Dictionaries
                where d.Name == name
                select d;
            if (dictionary.Count() > 1)
                throw new Exception("There are several dictionaries with the same name");
            else if (dictionary.Count() == 0)
                return null;

            return dictionary.First();
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
