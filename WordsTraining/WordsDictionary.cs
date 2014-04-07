using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public class WordsDictionary
    {
        private IList<WordCard> wordsList = new List<WordCard>();
        private string _language1;
        private string _language2;
        
        
        public int MaxWords { get; private set; } // do i realy need it?

        /// <summary>
        /// Language1 name
        /// Should not be null or empty
        /// </summary>
        public string Language1
        {
            get { return _language1; }
            private set
            {
                if (value == null || value == "") throw new ArgumentNullException("Language1 is empty");
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
                if (value == null || value == "") throw new ArgumentNullException("Language2 is empty");
                _language2 = value;
            }
        }

        /// <summary>
        /// Current number of cards in Dictionary
        /// </summary>
        public int Size { get; set; }

        public WordsDictionary(string language1, string language2)
            : this(language1, language2, 100)
        {
        }

        public WordsDictionary(string language1, string language2, int maxWords)
        {
            this.Language1 = language1;
            this.Language2 = language2;
            this.MaxWords = maxWords;
        }

        public void Add(WordCard card)
        {
            throw new NotImplementedException();
        }

        public WordCard Get(int p)
        {
            throw new NotImplementedException();
        }

    }
}
