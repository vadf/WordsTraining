using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WordsTraining;

namespace TestWordsTraining
{
    [TestFixture]
    class TestWordsDictionary
    {
        WordsDictionary dictionary;
        string language1 = "ENG";
        string language2 = "EST";
        int maxWords = 3;

        [SetUp]
        public void SetUp()
        {
            dictionary = new WordsDictionary(language1, language2, maxWords);
        }
        
        [Test]
        public void TestInit()
        {
            Assert.AreEqual(language1, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language2, dictionary.Language2, "Validating that language2 is initialized correctly");
            Assert.AreEqual(maxWords, dictionary.MaxWords, "Validating that maxWords is initialized correctly");
        }

        [Test]
        public void TestInit2()
        {
            dictionary = new WordsDictionary(language2, language1);
            Assert.AreEqual(language2, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language1, dictionary.Language2, "Validating that language2 is initialized correctly");
            Assert.AreEqual(100, dictionary.MaxWords, "Validating that maxWords is initialized correctly");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed1()
        {
            dictionary = new WordsDictionary(null, language2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitFailed2()
        {
            dictionary = new WordsDictionary(language1, "");
        }

        [Test]
        public void TestAddWordCard()
        {
            WordCard card = new WordCard("Hi", "Tere");
            dictionary.Add(card);
            WordCard cardExpected = new WordCard("Hi", "Tere");
            WordCard cardActual = dictionary.Get(0);
            Assert.AreEqual(cardExpected, cardActual, "Validating that card is added correctly");

            int sizeExpected = 1;
            int sizeActual = dictionary.Size;
            Assert.AreEqual(sizeExpected, sizeActual, "Validating dictionary size");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddWordCardTwice()
        {
            WordCard card1 = new WordCard("Hi", "Tere");
            WordCard card2 = new WordCard("Hi", "Tere");
            dictionary.Add(card1);
            dictionary.Add(card2);
        }

        [Test]
        public void TestAddWordCardMax()
        {
            WordCard card1 = new WordCard("Hi", "Tere");
            WordCard card2 = new WordCard("Good morning", "Tere hommikust");
            WordCard card3 = new WordCard("Good day", "Tere päevast");
            dictionary.Add(card1);
            dictionary.Add(card2);
            dictionary.Add(card3);

            int sizeExpected = 3;
            int sizeActual = dictionary.Size;
            Assert.AreEqual(sizeExpected, sizeActual, "Validating dictionary size");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAddWordCardMax1()
        {
            WordCard card1 = new WordCard("Hi", "Tere");
            WordCard card2 = new WordCard("Good morning", "Tere hommikust");
            WordCard card3 = new WordCard("Good day", "Tere päevast");
            WordCard card4 = new WordCard("Goodbye", "Nägamist");
            dictionary.Add(card1);
            dictionary.Add(card2);
            dictionary.Add(card3);
            dictionary.Add(card4);
        }
    }
}
