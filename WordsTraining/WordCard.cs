using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public enum WordType { Noun, Verb, Adjective, Adverb, Pronoun, Preposition, Conjunction, Interjunction }
    public enum Language { Lang1 = 1, Lang2 }

    /// <summary>
    /// Describes the Word card that contains two words
    /// </summary>
    public class WordCard
    {
        private IDictionary<Language, WordClass> words = new Dictionary<Language, WordClass>();

        public Language SelectedLanguage { get; set; }

        // Words, mandatory
        public string Word
        {
            get { return words[SelectedLanguage].Word; }
            set { words[SelectedLanguage].Word = value; }
        }

        public string Word1
        {
            get { return words[Language.Lang1].Word; }
        }

        public string Word2
        {
            get { return words[Language.Lang2].Word; }
        }

        /// <summary>
        /// Words type
        /// </summary>
        public WordType Type { get; set; }

        // Comment for word and common comment for card. All are optional
        public string Comment
        {
            get { return words[SelectedLanguage].Comment; }
            set { words[SelectedLanguage].Comment = value; }
        }

        public string Comment1
        {
            get { return words[Language.Lang1].Comment; }
        }

        public string Comment2
        {
            get { return words[Language.Lang2].Comment; }
        }

        public string CommentCommon { get; set; }

        // counters
        public uint SuccessfulCounter
        {
            get { return words[SelectedLanguage].SuccessfulCounter; }
            set { words[SelectedLanguage].SuccessfulCounter = value; }
        }

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

            SelectedLanguage = Language.Lang1;
            this.Word = word1;
            SelectedLanguage = Language.Lang2;
            this.Word = word2;
            this.Type = type;
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

            wc.SelectedLanguage = Language.Lang1;
            this.SelectedLanguage = Language.Lang1;
            if (wc.Word != this.Word)
            {
                return false;
            }

            wc.SelectedLanguage = Language.Lang2;
            this.SelectedLanguage = Language.Lang2;
            if (wc.Word != this.Word)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            string tmp = String.Join(",", words.Select(kv => kv.Key + "=" + kv.Value).ToArray());
            return String.Format("Card: type - {0}, words {1}", this.Type, tmp);
        }
    }

    /// <summary>
    /// Describes Word
    /// </summary>
    class WordClass
    {
        private string _word;
        private uint _successfulCounter = 0;

        /// <summary>
        /// Word text
        /// Should not be null or empty
        /// </summary>
        public string Word
        {
            get { return _word; }
            set
            {
                if (value == null || value == "") throw new ArgumentNullException("Word is empty");
                _word = value;
            }
        }

        /// <summary>
        /// Word comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Set the number of word was successfully guessed during exercize
        /// Could be increased by one or set to 0
        /// </summary>
        public uint SuccessfulCounter
        {
            get { return _successfulCounter; }
            set { _successfulCounter = value; }
        }

        public WordClass() { }

        public override string ToString()
        {
            return _word;
        }
    }
}
