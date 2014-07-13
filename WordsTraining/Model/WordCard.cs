using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WordsTraining.Model
{
    public enum WordType { Noun, Verb, Adjective, Adverb, Pronoun, Preposition, Conjunction, Interjunction, Phrase }
    public enum Language { Lang1 = 1, Lang2 }
    public enum TrainingType { Choose, WrittingWHint, Writting }

    /// <summary>
    /// Describes the Word card that contains two words
    /// </summary>
    public class WordCard
    {
        private IDictionary<Language, WordClass> words = new Dictionary<Language, WordClass>();

        #region Properties

        // Words, mandatory
        public string Word1
        {
            get { return words[Language.Lang1].Word; }
            set { words[Language.Lang1].Word = value; }
        }

        public string Word2
        {
            get { return words[Language.Lang2].Word; }
            set { words[Language.Lang2].Word = value; }
        }

        // Comment for word and common comment for card. All are optional
        public string Comment1
        {
            get { return words[Language.Lang1].Comment; }
            set { words[Language.Lang1].Comment = value; }
        }

        public string Comment2
        {
            get { return words[Language.Lang2].Comment; }
            set { words[Language.Lang2].Comment = value; }
        }

        // counters
        public Dictionary<TrainingType, int> Counter1
        {
            get { return words[Language.Lang1].Counter; }
            set { words[Language.Lang1].Counter = value; }
        }

        public Dictionary<TrainingType, int> Counter2
        {
            get { return words[Language.Lang2].Counter; }
            set { words[Language.Lang2].Counter = value; }
        }

        /// <summary>
        /// Words type
        /// </summary>
        public WordType Type { get; set; }

        public string CommentCommon { get; set; }

        private bool isSwitched = false;
        public bool Switched
        {
            get { return isSwitched; }
            set
            {
                if (isSwitched != value)
                    Swicth();
                isSwitched = value;
            }
        }

        #endregion

        /// <summary>
        /// Creates the Word Card with all Mandatory elements (language1, language2, word1, word2)
        /// </summary>
        /// <param name="word1">Word1 text</param>
        /// <param name="word2">Word2 text</param>
        /// <param name="type">Word type</param>
        public WordCard(string word1, string word2, WordType type)
        {
            words.Add(Language.Lang1, new WordClass());
            words.Add(Language.Lang2, new WordClass());

            this.Word1 = word1;
            this.Word2 = word2;
            this.Type = type;
            Switched = false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            WordCard wc = obj as WordCard;
            if ((System.Object)wc == null)
            {
                return false;
            }

            if (wc.Type != this.Type)
            {
                return false;
            }

            if (wc.Word1 != this.Word1)
            {
                return false;
            }

            if (wc.Word2 != this.Word2)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string tmp = String.Join(",", words.Select(kv => kv.Key + "=" + kv.Value));
            return String.Format("Card: type - {0}, words {1}", this.Type, tmp);
        }

        private void Swicth()
        {
            WordClass tmp = words[Language.Lang1];
            words[Language.Lang1] = words[Language.Lang2];
            words[Language.Lang2] = tmp;
        }

        // list of training types and word types
        public static IEnumerable<TrainingType> TrainingTypes = Enum.GetValues(typeof(TrainingType)).Cast<TrainingType>();
        public static IEnumerable<WordType> WordTypes = Enum.GetValues(typeof(WordType)).Cast<WordType>();
    }

    /// <summary>
    /// Describes Word
    /// </summary>
    class WordClass
    {
        private string _word;
        private Dictionary<TrainingType, int> _counter = new Dictionary<TrainingType, int>();

        /// <summary>
        /// Word text
        /// Should not be null or empty
        /// </summary>
        public string Word
        {
            get { return _word; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Word is empty");
                _word = value;
            }
        }

        /// <summary>
        /// Word comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Training counter
        /// </summary>
        public Dictionary<TrainingType, int> Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public WordClass()
        {
            foreach (var type in WordCard.TrainingTypes)
            {
                _counter.Add(type, 0);
            }
        }

        public override string ToString()
        {
            return _word;
        }
    }
}
