﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public class Training
    {
        private IList<WordCard> cardsToLearn;
        private bool isSwitched;
        private int _cardIndex = 0;
        private int _correctAnswers = 0;
        private Random r = new Random();
        private WordsDictionary dictionary;

        public int CorrectAnswers
        {
            get { return _correctAnswers; }
        }

        public int TotalCards
        {
            get { return cardsToLearn.Count; }
        }

        /// <summary>
        /// Initiates Training object
        /// Select amount of words for specific lang from dictionary to learn
        /// </summary>
        /// <param name="dictionary">Dictionary with word cards</param>
        /// <param name="langFrom">Language 'from' learn</param>
        /// <param name="langTo">Language 'to' learn</param>
        /// <param name="amount">Amount of cards to learn</param>
        /// <param name="maxCounter">Max counter value</param>
        public Training(WordsDictionary dictionary, bool isSwitched, int amount, int maxCounter)
        {
            this.isSwitched = isSwitched;
            this.dictionary = dictionary;
            foreach (var card in dictionary)
                card.Switched = isSwitched;

            // select all cards that have counter less than maxCounter
            var tmpCards =
                from c in dictionary
                where c.Counter1 < maxCounter
                select c;

            IList<WordCard> cards = tmpCards.ToList<WordCard>();
            if (cards.Count <= amount)
            {
                cardsToLearn = cards;
            }
            else
            {
                cardsToLearn = new List<WordCard>();
                while (cardsToLearn.Count < amount)
                {
                    int pos = r.Next(cards.Count);
                    if (!cardsToLearn.Contains(cards[pos]))
                        cardsToLearn.Add(cards[pos]);
                }
            }

            if (cardsToLearn.Count == 0)
                _cardIndex = -1;

            foreach (var card in cardsToLearn)
                card.Switched = isSwitched;
        }

        /// <summary>
        /// Switch all Cards back when finish training
        /// </summary>
        public void Close()
        {
            foreach (var card in dictionary)
                card.Switched = false;
        }

        /// <summary>
        /// Correct Answere was provided
        /// </summary>
        public void Succeeded()
        {
            var card = cardsToLearn[_cardIndex - 1];
            card.Counter1++;
            _correctAnswers++;
        }

        /// <summary>
        /// Get next word to learn
        /// </summary>
        /// <returns>Word 'from' learn</returns>
        public WordCard NextCard()
        {
            try
            {
                return cardsToLearn[_cardIndex++];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
