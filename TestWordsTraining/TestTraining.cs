using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WordsTraining;

namespace TestWordsTraining
{
    [TestFixture]
    class TestTraining
    {
        WordsDictionary dictionary;
        string language1 = "ENG";
        string language2 = "EST";
        int maxCards;
        int wordsToLearn;
        int maxCounter = 5;

        CardsGenerator generator = new CardsGenerator();
        Random random = new Random();

        [SetUp]
        public void SetUp()
        {
            maxCards = random.Next(5, 10);
            wordsToLearn = random.Next(1, maxCards);
            dictionary = new WordsDictionary(language1, language2, maxCards);
            for (int i = 0; i < maxCards; i++)
            {
                dictionary.Add(generator.GetCardExtra(maxCounter));
            }
        }

        [Test]
        public void TestDictionaryEmpty()
        {
            Training training = new Training(new WordsDictionary(language1, language2, maxCards), Language.Lang1, Language.Lang2, wordsToLearn, (uint)maxCounter);

            int actual = NumWordsInTraining(training);
            int expected = 0;
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        [Test]
        public void Test0ToLearn()
        {
            int expected = 0;
            Training training = new Training(dictionary, Language.Lang1, Language.Lang2, expected, (uint)maxCounter);

            int actual = NumWordsInTraining(training);
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        [Test]
        public void Test1ToLearn()
        {
            int expected = 1;
            Training training = new Training(dictionary, Language.Lang1, Language.Lang2, expected, (uint)maxCounter);

            int actual = NumWordsInTraining(training);
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        [Test]
        public void TestLearnGreaterThanCards()
        {
            int expected = maxCards;
            Training training = new Training(dictionary, Language.Lang1, Language.Lang2, maxCards + 1, (uint)maxCounter);

            int actual = NumWordsInTraining(training);
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        [Test]
        public void TestAllLearned()
        {
            uint counter = 1;
            foreach (var card in dictionary.Cards)
            {
                card.SuccessfulCounter1 = counter;
            }
            Training training = new Training(dictionary, Language.Lang1, Language.Lang2, maxCards, counter);

            int expected = 0;
            int actual = NumWordsInTraining(training);
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        [Test]
        public void TestSucceeded1()
        {
            // set counter to 0 for all cards
            foreach (var card in dictionary.Cards)
            {
                card.SuccessfulCounter1 = 0;
            }
            Training training = new Training(dictionary, Language.Lang1, Language.Lang2, maxCards, 1);
            // increase counter from 0 to 1 for one card
            training.NextWord();
            training.Succeeded();

            // check that training set decreased by 1 card
            training = new Training(dictionary, Language.Lang1, Language.Lang2, maxCards, 1);
            int actual = NumWordsInTraining(training);
            int expected = maxCards - 1;
            Assert.AreEqual(expected, actual, "Validating number of words to learn");
        }

        private int NumWordsInTraining(Training tr)
        {
            int count = 0;
            while (tr.NextWord() != null)
            {
                count++;
            }
            return count;
        }
    }
}
