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
        string language1 = "ENG";
        string language2 = "EST";
        string word1 = "Hi";
        string word2 = "Tere";

        [SetUp]
        public void SetUp()
        {
            card = new WordCard(language1, language2, word1, word2);
        }

        [Test]
        public void TestInit()
        {
            Assert.AreEqual(language1, card.Language1, "Test that language1 is initialized correctly");
            Assert.AreEqual(language2, card.Language2, "Test that language2 is initialized correctly");
            Assert.AreEqual(word1, card.Word1, "Test that word1 is initialized correctly");
            Assert.AreEqual(word2, card.Word2, "Test that word2 is initialized correctly");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFail1()
        {
            new WordCard("", language2, word1, word2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFail2()
        {
            new WordCard(language1, null, word1, word2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFail3()
        {
            new WordCard(language1, language2, null, word2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFail4()
        {
            new WordCard(language1, language2, word1, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInitFail5()
        {
            new WordCard(language1, language1, word1, word2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetIncorrectPropertyWord1()
        {
            card.Word1 = "";
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetIncorrectPropertyLanguage2()
        {
            card.Language2 = null;
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
            card.IncrementCounter(language1);
            Assert.AreEqual(1, card.SuccessfulCounter1, "Check that number of successsfull attempts for word1 is incremented");
        }

        [Test]
        public void TestSetCounterTo0()
        {
            card.IncrementCounter(language2);
            card.ResetCounter(language2);
            Assert.AreEqual(0, card.SuccessfulCounter2, "Check that number of successsfull attempts for word2 is reset to 0");
        }
    }
}
