using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    class Training
    {
        private int amountToLearn;
        private Language wichLanguage;
        private IList<WordCard> wordsToLearn = new List<WordCard>();

        /// <summary>
        /// Initiates Training object
        /// Select amount of words for specific lang from dictionary to learn
        /// </summary>
        /// <param name="dictionary">Dictionary with word cards</param>
        /// <param name="lang">Language to learn</param>
        /// <param name="amount">Amount of words to learn</param>
        public Training(WordsDictionary dictionary, Language lang, int amount)
        {}
    }
}
