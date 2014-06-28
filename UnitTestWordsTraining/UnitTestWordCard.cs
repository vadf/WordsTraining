using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using WordsTraining.Model;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestWordCard
    {
        WordCard card;
        string word1 = "Hi";
        string word2 = "Tere";
        WordType type = WordType.Noun;
        Random random = new Random();
        TrainingType trainingType;
        List<TrainingType> trainingTypeList = Enum.GetValues(typeof(TrainingType)).Cast<TrainingType>().ToList();

        [TestInitialize]
        public void SetUp()
        {
            card = new CardsGenerator().GetCardExtra(5);
            trainingType = trainingTypeList[random.Next(trainingTypeList.Count)];
        }

        [TestMethod]
        public void TestInit()
        {
            card = new WordCard(word1, word2, type);
            Assert.AreEqual(word1, card.Word1, "Validating that word1 is initialized correctly");
            Assert.AreEqual(word2, card.Word2, "Validating that word2 is initialized correctly");
            Assert.AreEqual(type, card.Type, "Validating that word2 is initialized correctly");
            Assert.IsFalse(card.Switched, "Validating that card i not switched");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed1()
        {
            new WordCard(null, word2, type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed2()
        {
            new WordCard(word1, "", type);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetIncorrectPropertyWord1()
        {
            card.Word1 = "";
        }

        [TestMethod]
        public void TestSetComment()
        {
            card.Comment1 = null;
            Assert.IsNull(card.Comment1, "Check that comment can be null");

            card.Comment2 = "";
            Assert.AreEqual("", card.Comment2, "Check that comment can be empty");

            card.CommentCommon = "test";
            Assert.AreEqual("test", card.CommentCommon, "Check comment");
        }

        [TestMethod]
        public void TestIncrCounter()
        {
            int expected = card.Counter1[trainingType] + 1;
            card.Counter1[trainingType]++;
            Assert.AreEqual(expected, card.Counter1[trainingType], "Check that number of successsfull attempts for word1 is incremented");
        }

        [TestMethod]
        public void TestSetCounterTo0()
        {
            card.Counter2[trainingType] = 0;
            Assert.AreEqual(0, card.Counter2[trainingType], "Check that number of successsfull attempts for word2 is reset to 0");
        }

        [TestMethod]
        public void TestCardsEqual1()
        {
            WordCard card2 = card;
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal");
        }

        [TestMethod]
        public void TestCardsEqual2()
        {
            WordCard card2 = new WordCard(card.Word1, card.Word2, card.Type);
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal");
        }

        [TestMethod]
        public void TestCardsEqual3()
        {
            WordCard card2 = new CardsGenerator().GetCardExtra(10);
            card2.Word1 = card.Word1;
            card2.Word2 = card.Word2;
            card2.Type = card.Type;
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal when comments and counters are different");
        }

        [TestMethod]
        public void TestCardsnotEqual1()
        {
            WordCard card2 = new WordCard("Hello", card.Word2, card.Type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal (diff in Word1)");
        }

        [TestMethod]
        public void TestCardsnotEqual2()
        {
            WordCard card2 = new WordCard(card.Word1, "Tsau", card.Type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal (diff in Word2)");
        }

        [TestMethod]
        public void TestCardsnotEqual3()
        {
            card.Type = WordType.Noun;
            WordCard card2 = new WordCard(card.Word1, card.Word2, WordType.Pronoun);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal (diff in Type)");
        }

        [TestMethod]
        public void TestSwitch()
        {
            WordCard card2 = new WordCard(card.Word2, card.Word1, card.Type);
            card2.Comment1 = card.Comment2;
            card2.Comment2 = card.Comment1;
            card2.CommentCommon = card.CommentCommon;
            card2.Counter1 = card.Counter2;
            card2.Counter2 = card.Counter1;
            card.Switched = true;
            Assert.IsTrue(card.Switched, "Validating that card is switched (Property)");
            Assert.AreEqual(card, card2, "Validating that card is switched (Cards)");
            Assert.AreEqual(card.CommentCommon, card2.CommentCommon, "Validating that card is switched (Comment)");
            Assert.AreEqual(card.Comment1, card2.Comment1, "Validating that card is switched (Comment1)");
            Assert.AreEqual(card.Comment2, card2.Comment2, "Validating that card is switched (Comment2)");
            Assert.AreEqual(card.Counter1, card2.Counter1, "Validating that card is switched (Counter1)");
            Assert.AreEqual(card.Counter2, card2.Counter2, "Validating that card is switched (Counter2)");
        }

        [TestMethod]
        public void TestSwitchBack()
        {
            WordCard card2 = new WordCard(card.Word1, card.Word2, card.Type);
            card2.Comment1 = card.Comment1;
            card2.Comment2 = card.Comment2;
            card2.CommentCommon = card.CommentCommon;
            card2.Counter1 = card.Counter1;
            card2.Counter2 = card.Counter2;
            card.Switched = true;
            card.Switched = false;
            Assert.IsFalse(card.Switched, "Validating that card is switched (Property)");
            Assert.AreEqual(card, card2, "Validating that card is switched (Cards)");
            Assert.AreEqual(card.CommentCommon, card2.CommentCommon, "Validating that card is switched (Comment)");
            Assert.AreEqual(card.Comment1, card2.Comment1, "Validating that card is switched (Comment1)");
            Assert.AreEqual(card.Comment2, card2.Comment2, "Validating that card is switched (Comment2)");
            Assert.AreEqual(card.Counter1, card2.Counter1, "Validating that card is switched (Counter1)");
            Assert.AreEqual(card.Counter2, card2.Counter2, "Validating that card is switched (Counter2)");
        }
    }
}
