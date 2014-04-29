using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WordsTraining;

namespace TestWordsTraining
{
    [TestFixture]
    class TestWordCard
    {
        WordCard card;
        string word1 = "Hi";
        string word2 = "Tere";
        WordType type = WordType.Noun;

        [SetUp]
        public void SetUp()
        {
            card = new WordCard(word1, word2, type);
        }

        [Test]
        public void TestInit()
        {
            Assert.AreEqual(word1, card.Word1, "Validating that word1 is initialized correctly");
            Assert.AreEqual(word2, card.Word2, "Validating that word2 is initialized correctly");
            Assert.AreEqual(type, card.Type, "Validating that word2 is initialized correctly");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed1()
        {
            new WordCard(null, word2, type);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed2()
        {
            new WordCard(word1, "", type);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetIncorrectPropertyWord1()
        {
            card.Word1 = "";
        }

        [Test]
        public void TestSetComment()
        {
            card.Comment1 = null;
            Assert.IsNull(card.Comment1, "Check that comment can be null");

            card.Comment2 = "";
            Assert.AreEqual("", card.Comment2, "Check that comment can be empty");
            
            card.CommentCommon = "test";
            Assert.AreEqual("test", card.CommentCommon, "Check comment");
        }

        [Test]
        public void TestIncrCounter()
        {
            card.SuccessfulCounter1++;
            Assert.AreEqual(1, card.SuccessfulCounter1, "Check that number of successsfull attempts for word1 is incremented");
        }

        [Test]
        public void TestSetCounterTo0()
        {
            card.SuccessfulCounter2++;
            card.SuccessfulCounter2 = 0;
            Assert.AreEqual(0, card.SuccessfulCounter2, "Check that number of successsfull attempts for word2 is reset to 0");
        }

        [Test]
        public void TestCardsEqual1()
        {
            WordCard card2 = card;
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal");
        }

        [Test]
        public void TestCardsEqual2()
        {
            WordCard card2 = new WordCard(word1, word2, type);
            Assert.IsTrue(card.Equals(card2), "Validating that cards are equal");
        }

        [Test]
        public void TestCardsnotEqual1()
        {
            WordCard card2 = new WordCard("Hello", word2, type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }

        [Test]
        public void TestCardsnotEqual2()
        {
            WordCard card2 = new WordCard(word1, "Tsau", type);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }

        [Test]
        public void TestCardsnotEqual3()
        {
            WordCard card2 = new WordCard(word1, word2, WordType.Pronoun);
            Assert.IsFalse(card.Equals(card2), "Validating that cards are not equal");
        }
    }
}
