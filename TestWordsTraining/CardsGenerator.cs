using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordsTraining;

namespace TestWordsTraining
{
    class CardsGenerator
    {
        private Random random = new Random();
        private string lang1 = "qwertyuiopasdfghjklzxcvbnm ";
        private string lang2 = "йцукенгшщзхъфывапролджэячсмитьбю ";
        private string lang3 = "qwertyuiopüõasdfghjklöäzxcvbnm ";

        public CardsGenerator()
        { }

        /// <summary>
        /// Returns randomly generated word card
        /// Counters are 0
        /// </summary>
        /// <returns>Generated WordCard</returns>
        public WordCard GetCard()
        {
            WordCard card = new WordCard(RandomString(lang1, random.Next(2, 10)), RandomString(lang2, random.Next(2, 10)), RandomType());
            card.Comment1 = RandomString(lang1, random.Next(0, 10));
            card.Comment2 = RandomString(lang2, random.Next(0, 10));
            card.CommentCommon = RandomString(lang1, random.Next(0, 10));
            return card;
        }

        /// <summary>
        /// Returns randomly generated word card
        /// Counters are set to random values
        /// </summary>
        /// <param name="counter">Max Counter value</param>
        /// <returns>Generated WordCard</returns>
        public WordCard GetCardExtra(int counter)
        {
            WordCard card = GetCard();
            card.SuccessfulCounter1 = (uint)random.Next(counter);
            card.SuccessfulCounter2 = (uint)random.Next(counter);
            return card;
        }

        /// <summary>
        /// Returns the random string with 'size' size
        /// </summary>
        /// <param name="lang">Source string</param>
        /// <param name="size">New string size</param>
        /// <returns>Generated string</returns>
        private string RandomString(string lang, int size)
        {
            string newString = null;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = lang[random.Next(lang.Length)];
                newString += ch + "";
            }
            if (size > 0 && newString[0] == ' ')
                newString = newString.Replace(' ', lang[0]);
            return newString;
        }

        /// <summary>
        /// Returns the random type of WordType enum
        /// </summary>
        /// <returns>Random WordType</returns>
        private WordType RandomType()
        {
            var typesList = Enum.GetValues(typeof(WordType)).Cast<WordType>().ToArray<WordType>();
            return typesList[random.Next(typesList.Length)];
        }
    }
}
