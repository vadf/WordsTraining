using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using WordsTraining.Model;

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
        bool isSwitched;
        TrainingType type = TrainingType.Writting;

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
            isSwitched = random.Next(1) == 1;
        }

        [TestMethod]
        public void TestDictionaryEmpty()
        {
            Training training = new Training(new WordsDictionary(language1, language2), isSwitched, wordsToLearn, maxCounter, type);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test0ToLearn()
        {
            int expected = 0;
            Training training = new Training(dictionary, isSwitched, expected, maxCounter, type);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test1ToLearn()
        {
            int expected = 1;
            Training training = new Training(dictionary, isSwitched, expected, maxCounter, type);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestLearnGreaterThanCards()
        {
            int expected = maxCards;
            Training training = new Training(dictionary, isSwitched, maxCards + 1, maxCounter, type);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
            CheckDuplicates(training);
        }

        [TestMethod]
        public void TestNothingToLearn_Switched()
        {
            int counter = 1;
            isSwitched = true;
            foreach (var card in dictionary)
                card.Counter2[type] = counter;

            Training training = new Training(dictionary, isSwitched, maxCards, counter, type);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestSucceeded1In1_NotSwitched()
        {
            int amount = 1;
            isSwitched = false;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter1[type] = 0;

            Training training = new Training(dictionary, isSwitched, amount, 1, type);

            // increase counter from 0 to 1 for one card
            var cardToLearn = training.NextCard();
            training.Succeeded();

            // check that card counter increased and number of learned cards
            Assert.AreEqual(amount, cardToLearn.Counter1[type], "Validating card counter");
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(amount, training.TotalCards, "Validating number of cards in training");
        }

        [TestMethod]
        public void TestSucceededSeveralInMany_Switched()
        {
            int amount = random.Next(maxCards / 2);
            isSwitched = true;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter2[type] = 0;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);
            // increase counter from 0 to 1 for several cards
            for (int i = 0; i < amount; i++)
            {
                training.NextCard();
                training.Succeeded();
            }

            // check number of correct answers and total cards in training
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");
            CheckDuplicates(training);
            training.Close();

            // check that training set decreased by 1 card
            training = new Training(dictionary, isSwitched, maxCards, 1, type);
            int expected = maxCards - amount;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
            CheckDuplicates(training);
        }

        [TestMethod]
        public void TestCloseSwitched()
        {
            isSwitched = true;
            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter / 2, type);

            // check that all cards are switched in dictionary
            foreach (var card in dictionary)
                Assert.IsTrue(card.Switched);

            // check that all cards are switched back after training close
            training.Close();
            foreach (var card in dictionary)
                Assert.IsFalse(card.Switched);
        }

        [TestMethod]
        public void TestCloseNotSwitched()
        {
            isSwitched = false;
            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter / 2, type);

            // check that all cards are not switched in dictionary
            foreach (var card in dictionary)
                Assert.IsFalse(card.Switched);

            // check that all cards are not switched after training close
            training.Close();
            foreach (var card in dictionary)
                Assert.IsFalse(card.Switched);
        }

        private void CheckDuplicates(Training training)
        {
            var card = training.NextCard();
            List<WordCard> cards = new List<WordCard>();
            while (card != null)
            {
                Assert.IsFalse(cards.Contains(card));
                card = training.NextCard();
            }
        }
    }
}
