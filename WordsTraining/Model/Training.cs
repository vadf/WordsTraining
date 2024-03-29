﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WordsTraining.Model
{
    public class Training
    {
        private IList<WordCard> cardsToLearn;
        private int _cardIndex = -1;
        private int _correctAnswers = 0;
        private Random r = new Random();
        private WordsDictionary dictionary;
        private TrainingType type;

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
        /// Select amount of words from dictionary to learn
        /// </summary>
        /// <param name="dictionary">Dictionary with word cards</param>
        /// <param name="isSwitched">Switch words in card</param>
        /// <param name="amount">Amount of cards to learn</param>
        /// <param name="maxCounter">Max counter value</param>
        /// <param name="type">Training type</param>
        /// <param name="learnedBefore">If true, Select only words that were learned in previous training type</param>
        public Training(WordsDictionary dictionary, bool isSwitched, int amount, int maxCounter, TrainingType type, bool learnedBefore)
        {
            this.dictionary = dictionary;
            this.type = type;

            // switch all cards if needed
            if (isSwitched)
            {
                foreach (var card in dictionary)
                    card.Switched = isSwitched;
            }

            IEnumerable<WordCard> cardsList = dictionary;
            // get learned cards from prevois training type
            if (learnedBefore && type != 0)
            {
                cardsList =
                    from c in cardsList
                    where c.Counter1[type - 1] >= maxCounter
                    select c;
            }

            // select cards that have counter less than maxCounter for specific training type
            cardsList = (
                from c in cardsList
                where c.Counter1[type] < maxCounter
                orderby c.LastTrained
                select c)
                .Take(amount);
            cardsToLearn = cardsList.ToList();
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
        /// Correct Answer was provided
        /// Increase training counter
        /// </summary>
        private void Succeeded()
        {
            var card = cardsToLearn[_cardIndex];
            card.Counter1[type]++;
            _correctAnswers++;
        }

        /// <summary>
        /// Wrong Answer was provided
        /// Decrease training counter if it positive
        /// </summary>
        private void Failed()
        {
            var card = cardsToLearn[_cardIndex];
            if (card.Counter1[type] > 0)
                card.Counter1[type]--;
        }

        /// <summary>
        /// Get next word to learn
        /// </summary>
        /// <returns>Word 'from' learn</returns>
        public WordCard NextCard()
        {
            try
            {
                _cardIndex++;
                cardsToLearn[_cardIndex].LastTrained = DateTime.Now;
                return cardsToLearn[_cardIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Choose several(cardsToChoose) cards of the same WordType as currentCard
        /// </summary>
        /// <param name="cardsToChoose">Number of card to choose (incl. currentCard)</param>
        /// <returns>Returns the list of WordCards</returns>
        public IList<WordCard> Choose(int cardsToChoose = 5)
        {
            var tmpCards =
                (from c in dictionary
                 where c.Type == cardsToLearn[_cardIndex].Type
                 orderby r.Next()
                 select c).Take(cardsToChoose);

            IList<WordCard> result = tmpCards.ToList();
            if (!result.Contains(cardsToLearn[_cardIndex]))
            {
                result[r.Next(result.Count)] = cardsToLearn[_cardIndex];
            }

            return result;
        }

        /// <summary>
        /// Returns Hint for current card
        /// </summary>
        /// <returns></returns>
        public string GetHint()
        {
            string hint = String.Format("First letter is '{0}', Word length is {1}", cardsToLearn[_cardIndex].Word2[0], cardsToLearn[_cardIndex].Word2.Length);
            return hint;
        }

        /// <summary>
        /// Check answer
        /// </summary>
        /// <param name="answer">Answer string</param>
        /// <param name="decreaseOnFail">Decrease training counter on fail</param>
        /// <returns>True, if answer correct</returns>
        public bool CheckAnswer(string answer, bool decreaseOnFail)
        {
            bool result = false;
            if (string.Equals(answer, cardsToLearn[_cardIndex].Word2, StringComparison.OrdinalIgnoreCase))
            {
                Succeeded();
                result = true;
            }
            else if (decreaseOnFail)
            {
                Failed();
            }
            return result;
        }
    }
}
