using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public class Training
    {
        private IList<WordCard> cardsToLearn;
        private int _cardIndex = 0;

        /// <summary>
        /// Initiates Training object
        /// Select amount of words for specific lang from dictionary to learn
        /// </summary>
        /// <param name="dictionary">Dictionary with word cards</param>
        /// <param name="langFrom">Language 'from' learn</param>
        /// <param name="langTo">Language 'to' learn</param>
        /// <param name="amount">Amount of cards to learn</param>
        /// <param name="maxCounter">Max counter value</param>
        public Training(WordsDictionary dictionary, Language langFrom, Language langTo, int amount, uint maxCounter)
        {
            // select all cards that have counter less than maxCounter
            var tmpCards =
                from c in dictionary.Cards
                where c.SuccessfulCounter1 < maxCounter
                select c;

            IList<WordCard> cards = tmpCards.ToList<WordCard>();
            if (cards.Count <= amount)
            {
                cardsToLearn = cards;
            }
            else
            {
                cardsToLearn = new List<WordCard>();
                Random r = new Random();
                while (cardsToLearn.Count < amount)
                {
                    int pos = r.Next(amount);
                    if (!cardsToLearn.Contains(cards[pos]))
                        cardsToLearn.Add(cards[pos]);
                }
            }

            if (cardsToLearn.Count == 0)
                _cardIndex = -1;
        }

        /// <summary>
        /// Correct Answere was provided
        /// </summary>
        public void Succeeded()
        {
            cardsToLearn[_cardIndex].SuccessfulCounter1++;
        }

        /// <summary>
        /// Get next word to learn
        /// </summary>
        /// <returns>Word 'from' learn</returns>
        public string NextWord()
        {
            try
            {
                return cardsToLearn[_cardIndex++].Word1;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
