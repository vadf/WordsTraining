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
        int maxCounter = 5;
        bool isSwitched;
        TrainingType type = TrainingType.Writting;
        int cardsToChoose;

        CardsGenerator generator = new CardsGenerator();
        Random random = new Random();

        [TestInitialize]
        public void SetUp()
        {
            maxCards = random.Next(5, 10);
            cardsToChoose = random.Next(1, maxCards - 1);
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
            Training training = new Training(new WordsDictionary(language1, language2), isSwitched, 1, maxCounter, type);

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

        [TestMethod]
        public void TestChooseCardsMoreThanAvailableType()
        {
            int amount = random.Next(1);
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);

            // set word type as currentCard
            var currentCard = training.NextCard();
            foreach (var card in dictionary)
                card.Type = currentCard.Type;
            var chooseList = training.Choose(currentCard, cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCardsEqualToAvailableType()
        {
            int amount = random.Next(1);
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);

            // set word type as currentCard            
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            for (int i = 0; i < dictionary.Count; i++)
                dictionary[i].Type = currentCard.Type;
            var chooseList = training.Choose(currentCard, cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCardsLessThanType()
        {
            int amount = random.Next(1);
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);

            // set word type as currentCard            
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            for (int i = 0; i < dictionary.Count - 1; i++)
                dictionary[i].Type = currentCard.Type;
            var chooseList = training.Choose(currentCard, cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCards0Type()
        {
            int amount = random.Next(1);
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);

            // all other cards have different type as currentCard
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            var chooseList = training.Choose(currentCard, cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChoose()
        {
            int amount = random.Next(maxCards / 2);
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type);
            
            // go trough all cards in training and choose cards
            var currentCard = training.NextCard();
            while (currentCard != null)
            {
                var chooseList = training.Choose(currentCard);
                CheckChooseList(chooseList, currentCard);
                currentCard = training.NextCard();
            }
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

        private void CheckChooseList(IList<WordCard> chooseList, WordCard currentCard)
        {
            Assert.IsTrue(chooseList.Contains(currentCard), "Validating that chooseList contains currentCard");
            foreach (var card in chooseList)
            {
                Assert.AreEqual(card.Type, currentCard.Type, "Validating that card in chooseList has the same type as currentCard");
            }
        }
    }
}
