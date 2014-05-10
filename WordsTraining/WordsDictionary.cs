using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WordsTraining
{
    public class WordsDictionary : ObservableCollection<WordCard>
    {
        private string _language1;
        private string _language2;

        /// <summary>
        /// Language1 name
        /// Should not be null or empty
        /// </summary>
        public string Language1
        {
            get { return _language1; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Language1 is empty");
                _language1 = value;
            }
        }

        /// <summary>
        /// Language2 name
        /// Should not be null or empty
        /// </summary>
        public string Language2
        {
            get { return _language2; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Language2 is empty");
                _language2 = value;
            }
        }

        public WordsDictionary(string language1, string language2)
        {
            this.Language1 = language1;
            this.Language2 = language2;
        }
    }
}
