using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    /// <summary>
    /// Describes the Word card that contains two words
    /// </summary>
    public class WordCard
    {
        private WordClass word1 = new WordClass();
        private WordClass word2 = new WordClass();

        // Languages, mandatory
        public string Language1
        {
            get { return word1.Language; }
            set { word1.Language = value; }
        }

        public string Language2
        {
            get { return word2.Language; }
            set { word2.Language = value; }
        }

        // Words, mandatory
        public string Word1
        {
            get { return word1.Word; }
            set { word1.Word = value; }
        }

        public string Word2
        {
            get { return word2.Word; }
            set { word2.Word = value; }
        }

        // Comment for each word and common comment for card. All are optional
        public string Comment1
        {
            get { return word1.Comment; }
            set { word1.Comment = value; }
        }
        public string Comment2
        {
            get { return word2.Comment; }
            set { word2.Comment = value; }
        }
        public string CommentCommon { get; set; }

        // counters
        public int SuccessfulCounter1
        {
            get { return word1.SuccessfulCounter; }
            private set { word1.SuccessfulCounter = value; }
        }
        public int SuccessfulCounter2
        {
            get { return word2.SuccessfulCounter; }
            private set { word2.SuccessfulCounter = value; }
        }

        /// <summary>
        /// Creates the Word Card with all Mandatory elements (language1, language2, word1, word2)
        /// </summary>
        /// <param name="language1">Language1 name</param>
        /// <param name="language2">Language2 name</param>
        /// <param name="word1">Word1 text</param>
        /// <param name="word2">Word2 text</param>
        public WordCard(string language1, string language2, string word1, string word2)
        {
            if (language1 == language2)
                throw new ArgumentException("Languages should be different");
            Language1 = language1;
            Language2 = language2;
            Word1 = word1;
            Word2 = word2;
        }

        /// <summary>
        /// Increment successful counter for language
        /// </summary>
        /// <param name="language">Language name</param>
        public void IncrementCounter(string language)
        {
            GetWordClass(language).SuccessfulCounter++;
        }


        /// <summary>
        /// Reset successful counter for language
        /// </summary>
        /// <param name="language">Language name</param>
        public void ResetCounter(string language)
        {
            GetWordClass(language).SuccessfulCounter = 0;
        }

        /// <summary>
        /// Find Word Class by language
        /// </summary>
        /// <param name="language">Language name</param>
        /// <returns>WordClass object</returns>
        private WordClass GetWordClass(string language)
        {
            if (language == Language1) return word1;
            else if (language == Language2) return word2;
            else throw new ArgumentException("There is no language " + language + " in word class");
        }
    }

    /// <summary>
    /// Describes Word
    /// </summary>
    class WordClass
    {
        private string _language;
        /// <summary>
        /// Word language
        /// Should not be null or empty
        /// </summary>
        public string Language
        {
            get { return _language; }
            set
            {
                if (value == null || value == "") throw new ArgumentNullException("Language is empty");
                _language = value;
            }
        }

        private string _word;
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

        private int _successfulCounter = 0;
        /// <summary>
        /// Set the number of word was successfully guessed during exercize
        /// Could be increased by one or set to 0
        /// </summary>
        public int SuccessfulCounter
        {
            get { return _successfulCounter; }
            set
            {
                if (value != 0 && _successfulCounter + 1 != value)
                {
                    throw new InvalidOperationException("Counter can be increased by one or set to 0");
                }
                _successfulCounter = value;
            }
        }

        public WordClass() { }
    }
}
