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
            card.SelectedLanguage = Language.Lang1;
            Assert.AreEqual(word1, card.Word, "Validating that word1 is initialized correctly");
            card.SelectedLanguage = Language.Lang2;
            Assert.AreEqual(word2, card.Word, "Validating that word2 is initialized correctly");
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
            card.SelectedLanguage = Language.Lang1;
            card.Word = "";
        }

        [Test]
        public void TestSetComment()
        {
            card.SelectedLanguage = Language.Lang1;
            card.Comment = null;
            Assert.IsNull(card.Comment, "Check that comment can be null");

            card.SelectedLanguage = Language.Lang2;
            card.Comment = "";
            Assert.AreEqual("", card.Comment, "Check that comment can be empty");
            
            card.CommentCommon = "test";
            Assert.AreEqual("test", card.CommentCommon, "Check comment");
        }

        [Test]
        public void TestIncrCounter()
        {
            card.SelectedLanguage = Language.Lang1;
            card.IncrementCounter();
            Assert.AreEqual(1, card.SuccessfulCounter, "Check that number of successsfull attempts for word1 is incremented");
        }

        [Test]
        public void TestSetCounterTo0()
        {
            card.SelectedLanguage = Language.Lang2;
            card.IncrementCounter();
            card.ResetCounter();
            Assert.AreEqual(0, card.SuccessfulCounter, "Check that number of successsfull attempts for word2 is reset to 0");
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
