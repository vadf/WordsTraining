using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using WordsTraining.Model;
using System.Reflection;

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
        bool learnedBefore = false;

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
            Training training = new Training(new WordsDictionary(language1, language2), isSwitched, 1, maxCounter, type, learnedBefore);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test0ToLearn()
        {
            int expected = 0;
            Training training = new Training(dictionary, isSwitched, expected, maxCounter, type, learnedBefore);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void Test1ToLearn()
        {
            int expected = 1;
            Training training = new Training(dictionary, isSwitched, expected, maxCounter, type, learnedBefore);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestLearnGreaterThanCards()
        {
            int expected = maxCards;
            Training training = new Training(dictionary, isSwitched, maxCards + 1, maxCounter, type, learnedBefore);

            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
            CheckTrainingList(training);
        }

        [TestMethod]
        public void TestNothingToLearn_Switched()
        {
            int counter = 1;
            isSwitched = true;
            foreach (var card in dictionary)
                card.Counter2[type] = counter;

            Training training = new Training(dictionary, isSwitched, maxCards, counter, type, learnedBefore);

            int expected = 0;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
        }

        [TestMethod]
        public void TestSucceeded1In1_NotSwitched()
        {
            int amount = 1;
            int counter = 0;
            isSwitched = false;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter1[type] = counter;

            int countersBefore = GetCountersSum();

            Training training = new Training(dictionary, isSwitched, amount, counter + 1, type, learnedBefore);

            // provide correct answer for one card
            var timeBefore = DateTime.Now;
            var cardToLearn = training.NextCard();
            var timeAfter = DateTime.Now;
            CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained);

            training.CheckAnswer(cardToLearn.Word2, true);
            CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained); // check that doesn't chaned after answer

            // check that card counter increased and number of learned cards
            int countersAfter = GetCountersSum();
            Assert.AreEqual(counter + 1, cardToLearn.Counter1[type], "Validating card counter");
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(amount, training.TotalCards, "Validating number of cards in training");
            Assert.AreEqual(countersBefore + training.CorrectAnswers, countersAfter, "Validating that only counter of one type for one word increased");
        }

        [TestMethod]
        public void TestSucceededSeveralInMany_Switched()
        {
            int amount = random.Next(maxCards / 2);
            isSwitched = true;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter2[type] = 0;
            int countersBefore = GetCountersSum();

            Training training = new Training(dictionary, isSwitched, maxCards, 1, type, learnedBefore);
            CheckTrainingList(training);

            // provide correct answer for one card
            for (int i = 0; i < amount; i++)
            {
                var timeBefore = DateTime.Now;
                var cardToLearn = training.NextCard();
                var timeAfter = DateTime.Now;
                CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained);

                training.CheckAnswer(cardToLearn.Word2, true);
                CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained); // check that doesn't chaned after answer
            }

            // check number of correct answers and total cards in training
            int countersAfter = GetCountersSum();
            Assert.AreEqual(amount, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");
            Assert.AreEqual(countersBefore + training.CorrectAnswers, countersAfter, "Validating that corrent number of counters increased");
            training.Close();

            // check that training set decreased by learned cards
            training = new Training(dictionary, isSwitched, maxCards, 1, type, learnedBefore);
            int expected = maxCards - amount;
            Assert.AreEqual(expected, training.TotalCards, "Validating number of words to learn");
            CheckTrainingList(training);
        }

        [TestMethod]
        public void TestFailedDontDecreaseCounter()
        {
            maxCards = 1;
            maxCounter = 0;

            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter1[type] = maxCounter;
            int countersBefore = GetCountersSum();

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter + 1, type, learnedBefore);

            // provide incorrect answer
            var timeBefore = DateTime.Now;
            var cardToLearn = training.NextCard();
            var timeAfter = DateTime.Now;
            CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained);
            training.CheckAnswer(cardToLearn.Word1, false);
            CheckLastTrained(timeBefore, timeAfter, cardToLearn.LastTrained); // check that doesn't chaned after answer

            // check that card counter increased and number of learned cards
            int countersAfter = GetCountersSum();
            Assert.AreEqual(maxCounter, cardToLearn.Counter1[type], "Validating card counter");
            Assert.AreEqual(0, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");
            Assert.AreEqual(countersBefore, countersAfter, "Validating that none counters increased");
        }

        [TestMethod]
        public void TestFailedDecreaseCounter()
        {
            maxCards = 1;
            maxCounter = 2;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter1[type] = maxCounter;
            int countersBefore = GetCountersSum();

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter + 1, type, learnedBefore);

            // increase counter from 0 to 1 for one card
            var cardToLearn = training.NextCard();
            training.CheckAnswer(cardToLearn.Word1, true);

            // check that card counter increased and number of learned cards
            int countersAfter = GetCountersSum();
            Assert.AreEqual(maxCounter - 1, cardToLearn.Counter1[type], "Validating card counter");
            Assert.AreEqual(0, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");
            Assert.AreEqual(countersBefore - 1, countersAfter, "Validating that one counter increased");
        }

        [TestMethod]
        public void TestFailedDecreaseCounterTo0()
        {
            maxCards = 1;
            maxCounter = 0;
            // set counter to 0 for all cards
            foreach (var card in dictionary)
                card.Counter1[type] = maxCounter;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter + 1, type, learnedBefore);

            // increase counter from 0 to 1 for one card
            var cardToLearn = training.NextCard();
            training.CheckAnswer(cardToLearn.Word1, true);

            // check that card counter increased and number of learned cards
            Assert.AreEqual(maxCounter, cardToLearn.Counter1[type], "Validating card counter");
            Assert.AreEqual(0, training.CorrectAnswers, "Validating number of correct answers");
            Assert.AreEqual(maxCards, training.TotalCards, "Validating number of cards in training");
        }

        [TestMethod]
        public void TestCloseSwitched()
        {
            isSwitched = true;
            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter / 2, type, learnedBefore);
            CheckTrainingList(training);

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
            learnedBefore = true;
            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter / 2, type, learnedBefore);
            CheckTrainingList(training);

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
            maxCounter = 1;
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            CheckTrainingList(training);

            // set word type as currentCard
            var currentCard = training.NextCard();
            foreach (var card in dictionary)
                card.Type = currentCard.Type;
            var chooseList = training.Choose(cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCardsEqualToAvailableType()
        {
            maxCounter = 1;
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            CheckTrainingList(training);

            // set word type as currentCard            
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            for (int i = 0; i < dictionary.Count; i++)
                dictionary[i].Type = currentCard.Type;
            var chooseList = training.Choose(cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCardsLessThanType()
        {
            maxCounter = 1;
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            CheckTrainingList(training);

            // set word type as currentCard            
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            for (int i = 0; i < dictionary.Count - 1; i++)
                dictionary[i].Type = currentCard.Type;
            var chooseList = training.Choose(cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChooseCards0Type()
        {
            maxCounter = 1;
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            CheckTrainingList(training);

            // all other cards have different type as currentCard
            foreach (var card in dictionary)
                card.Type = WordType.Noun;
            var currentCard = training.NextCard();
            currentCard.Type = WordType.Verb;
            var chooseList = training.Choose(cardsToChoose);
            CheckChooseList(chooseList, currentCard);
        }

        [TestMethod]
        public void TestChoose()
        {
            maxCounter = 1;
            type = TrainingType.Choose;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            CheckTrainingList(training);

            // go trough all cards in training and choose cards
            var currentCard = training.NextCard();
            while (currentCard != null)
            {
                var chooseList = training.Choose();
                CheckChooseList(chooseList, currentCard);
                currentCard = training.NextCard();
            }
        }

        [TestMethod]
        public void TestWrittingWHint()
        {
            maxCards = 1;
            type = TrainingType.WrittingWHint;

            Training training = new Training(dictionary, isSwitched, maxCards, maxCounter, type, learnedBefore);
            var currentCard = training.NextCard();
            currentCard.Word2 = "Test";
            string actual = training.GetHint();
            string expected = "First letter is 'T', Word length is 4";

            Assert.AreEqual(expected, actual, "Validating hint");
        }

        [TestMethod]
        public void TestWrittingWHintLearnedBeforeTrueOK()
        {
            type = TrainingType.WrittingWHint;
            learnedBefore = true;
            int cardsToLearn = maxCards / 3;

            // set counter for choose to maxCounter for cardsToLearn
            foreach (var card in dictionary)
                card.Counter1[TrainingType.Choose] = 0;
            for (int i = 0; i < cardsToLearn; i++)
                dictionary[i].Counter1[TrainingType.Choose] = maxCounter;

            Training training = new Training(dictionary, isSwitched, cardsToLearn + 1, maxCounter, type, learnedBefore);
            CheckTrainingList(training);
            Assert.AreEqual(cardsToLearn, training.TotalCards, "Validating number of cards to learn");
        }

        [TestMethod]
        public void TestWrittingWHintLearnedBeforeTrueNOK()
        {
            type = TrainingType.WrittingWHint;
            learnedBefore = true;
            int cardsToLearn = maxCards / 3;

            // set counter for choose to maxCounter for all cards
            foreach (var card in dictionary)
                card.Counter1[TrainingType.Choose] = maxCounter;

            // select cards with counter maxCounter + 1
            Training training = new Training(dictionary, isSwitched, cardsToLearn, maxCounter + 1, type, learnedBefore);
            CheckTrainingList(training);
            Assert.AreEqual(0, training.TotalCards, "Validating number of cards to learn");
        }

        private void CheckTrainingList(Training training)
        {
            List<WordCard> tmpCardsList = new List<WordCard>();
            FieldInfo fi = typeof(Training).GetField("cardsToLearn", BindingFlags.NonPublic | BindingFlags.Instance);
            List<WordCard> cardsToLearn = fi.GetValue(training) as List<WordCard>;

            var prevTime = new DateTime();
            foreach (var card in cardsToLearn)
            {
                Assert.IsFalse(tmpCardsList.Contains(card)); // check for duplicates
                Assert.IsTrue(prevTime <= card.LastTrained); // check that cards sorted by last trained
                prevTime = card.LastTrained;
                tmpCardsList.Add(card);
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

        private int GetCountersSum()
        {
            int count = 0;
            foreach (var type in WordCard.TrainingTypes)
            {
                count += dictionary.Sum(c => c.Counter1[type]);
                count += dictionary.Sum(c => c.Counter2[type]);
            }
            return count;
        }

        private void CheckLastTrained(DateTime timeBefore, DateTime timeAfter, DateTime dateTime)
        {
            Assert.IsTrue(timeBefore <= dateTime);
            Assert.IsTrue(timeAfter >= dateTime);
        }
    }
}
