using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public enum WordType { NOUN, VERB, ADJECTIVE, ADVERB, PRONOUN, PREPOSITION, CONJUCTION, INTERJUNCTION }
    
    /// <summary>
    /// Describes the Word card that contains two words
    /// </summary>
    public class WordCard
    {
        private WordClass word1 = new WordClass();
        private WordClass word2 = new WordClass();

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

        public WordType Type { get; private set; }
        
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
        /// <param name="word1">Word1 text</param>
        /// <param name="word2">Word2 text</param>
        /// <param name="type">Word type</param>
        public WordCard(string word1, string word2, WordType type)
        {
            this.Word1 = word1;
            this.Word2 = word2;
            this.Type = type;
        }

        /// <summary>
        /// Increment successful counter for language
        /// </summary>
        /// <param name="num">Word number (1 or 2) in card</param>
        public void IncrementCounter(int num)
        {
            GetWordClass(num).SuccessfulCounter++;
        }


        /// <summary>
        /// Reset successful counter for language
        /// </summary>
        /// <param name="num">Word number (1 or 2) in card</param>
        public void ResetCounter(int num)
        {
            GetWordClass(num).SuccessfulCounter = 0;
        }

        /// <summary>
        /// Returns word class
        /// </summary>
        /// <param name="num">Word number (1 or 2) in card</param>
        /// <returns>WordClass object</returns>
        private WordClass GetWordClass(int num)
        {
            if (num == 1) return word1;
            else if (num == 2) return word2;
            else throw new ArgumentException("There is no word number " + num + " in word class");
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

            if (wc.Type != this.Type || wc.Word1 != this.Word1 || wc.Word2 != this.Word2)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (this.Type + this.Word1 + this.Word2).GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("Card: type - {0}, word1 - {1}, word2 - {2}", this.Type, this.Word1, this.Word2);
        }
    }

    /// <summary>
    /// Describes Word
    /// </summary>
    class WordClass
    {
        private string _word;
        private int _successfulCounter = 0;

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
