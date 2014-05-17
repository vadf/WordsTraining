using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordsTraining;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestWordCard
    {
        WordCard card;
        string word1 = "Hi";
        string word2 = "Tere";
        WordType type = WordType.Noun;

        [TestInitialize]
        public void SetUp()
        {
            card = new WordCard(word1, word2, type);
        }

        [TestMethod]
        public void TestInit()
        {
            Assert.AreEqual(word1, card.Word1, "Validating that word1 is initialized correctly");
            Assert.AreEqual(word2, card.Word2, "Validating that word2 is initialized correctly");
            Assert.AreEqual(type, card.Type, "Validating that word2 is initialized correctly");
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
            card.Counter1++;
            Assert.AreEqual(1, card.Counter1, "Check that number of successsfull attempts for word1 is incremented");
        }

        [TestMethod]
        public void TestSetCounterTo0()
        {
            card.Counter2++;
            card.Counter2 = 0;
            Assert.AreEqual(0, card.Counter2, "Check that number of successsfull attempts for word2 is reset to 0");
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
            WordCard card2 = new WordCard(word1, word2, type);
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal");
        }

        [TestMethod]
        public void TestCardsnotEqual1()
        {
            WordCard card2 = new WordCard("Hello", word2, type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }

        [TestMethod]
        public void TestCardsnotEqual2()
        {
            WordCard card2 = new WordCard(word1, "Tsau", type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }

        [TestMethod]
        public void TestCardsnotEqual3()
        {
            WordCard card2 = new WordCard(word1, word2, WordType.Pronoun);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }
    }
}
