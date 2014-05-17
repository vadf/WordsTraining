using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordsTraining;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestTraining
    {
        WordsDictionary dictionary;
        string language1 = "ENG";
        string language2 = "EST";
        int maxCards;
        int wordsToLearn;
        int maxCounter = 5;
        Language langFrom = Language.Lang2;
        Language langTo = Language.Lang1;

        CardsGenerator generator = new CardsGenerator();
        Random random = new Random();

        [TestInitialize]
        public void SetUp()
        {
            maxCards = random.Next(5, 10);
            wordsToLearn = random.Next(1, maxCards);
            dictionary = new WordsDictionary(language1, language2);
            for (int i = 0; i < maxCards; i++)
            {
                dictionary.Add(generator.GetCardExtra(maxCounter));
            }
        }

        [TestMethod]
        public void TestDictionaryEmpty()
        {
            Training training = new Training(new WordsDictionary(language1, language2), langFrom, langTo, wordsToLearn, maxCounter);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test0ToLearn()
        {
            int expected = 0;
            Training training = new Training(dictionary, langFrom, langTo, expected, maxCounter);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test1ToLearn()
        {
            int expected = 1;
            Training training = new Training(dictionary, langFrom, langTo, expected, maxCounter);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestLearnGreaterThanCards()
        {
            int expected = maxCards;
            Training training = new Training(dictionary, langFrom, langTo, maxCards + 1, maxCounter);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestNothingToLearn()
        {
            int counter = 1;
            foreach (var card in dictionary)
            {
                card.SelectedLanguage = langFrom;
                card.Counter = counter;
            }
            Training training = new Training(dictionary, langFrom, langTo, maxCards, counter);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestSucceeded1In1()
        {
            int amount = 1;

            // set counter to 0 for all cards
            foreach (var card in dictionary)
            {
                card.SelectedLanguage = langFrom;
                card.Counter = 0;
            }
            Training training = new Training(dictionary, langFrom, langTo, amount, 1);

            // increase counter from 0 to 1 for one card
            var cardToLearn = training.NextCard();
            training.Succeeded();

            // check that card counter increased and number of learned cards
            cardToLearn.SelectedLanguage = langFrom;
            Assert.AreEqual(amount, cardToLearn.Counter, "Validating card counter");
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(amount, training.TotalCards, "Validating number of cards in training");
        }

        [TestMethod]
        public void TestSucceededSeveralInMany()
        {
            int amount = random.Next(maxCards / 2);
            // set counter to 0 for all cards
            foreach (var card in dictionary)
            {
                card.SelectedLanguage = langFrom;
                card.Counter = 0;
            }
            Training training = new Training(dictionary, langFrom, langTo, maxCards, 1);
            // increase counter from 0 to 1 for several cards
            for (int i = 0; i < amount; i++)
            {
                training.NextCard();
                training.Succeeded();
            }

            // check number of correct answers and total cards in training
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");

            // check that training set decreased by 1 card
            training = new Training(dictionary, langFrom, langTo, maxCards, 1);
            int expected = maxCards - amount;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }
    }
}
