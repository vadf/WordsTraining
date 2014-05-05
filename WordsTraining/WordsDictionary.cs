using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public class WordsDictionary
    {
        private List<WordCard> cardsList = new List<WordCard>(); // CHECK: HashSet ???
        private string _language1;
        private string _language2;
        private int _selectedIndex = 0;

        public int MaxCards { get; private set; } // CHECK: do i realy need it?

        public List<WordCard> Cards
        {
            get { return cardsList; }
        }

        private Dictionary<Language, string> lang = new Dictionary<Language, string>();

        public Dictionary<Language, string> DictinaryLanguages { get { return lang; } }

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
        /// Get WordCard with index i from Dictionary
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>WordCard</returns>
        public WordCard this[int i]
        {
            get { return cardsList[i]; }
        }

        /// <summary>
        /// Current number of cards in Dictionary
        /// </summary>
        public int Size
        {
            get { return cardsList.Count; }
        }

        public WordsDictionary(string language1, string language2)
            : this(language1, language2, 100)
        {
        }

        public WordsDictionary(string language1, string language2, int maxCards)
        {
            this.Language1 = language1;
            this.Language2 = language2;
            this.MaxCards = maxCards;
            lang.Add(Language.Lang1, language1);
            lang.Add(Language.Lang2, language2);
        }

        /// <summary>
        /// Add Card to a dictinary
        /// Throws ArgumentException if such card has beed added
        /// Throws ArgumentOutOfRangeException if max cards limit exceeded
        /// </summary>
        /// <param name="card">Word card</param>
        public void Add(WordCard card)
        {
            if (cardsList.Contains(card))
                throw new ArgumentException(card + " is exist in a dictionary");
            else if (cardsList.Count == MaxCards)
                throw new ArgumentOutOfRangeException("Cards limit exceeded. Max number of cards " + MaxCards);
            else
                cardsList.Add(card);
        }

    }
}
