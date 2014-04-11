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
        int maxCards = 3;

        string word11 = "Hi";
        string word12 = "Tere";
        WordType type1 = WordType.NOUN;

        string word21 = "rääkima";
        string word22 = "speak";
        WordType type2 = WordType.VERB;

        string word31 = "hästi";
        string word32 = "well";
        WordType type3 = WordType.ADVERB;

        [SetUp]
        public void SetUp()
        {
            dictionary = new WordsDictionary(language1, language2, maxCards);
        }
        
        [Test]
        public void TestInit()
        {
            Assert.AreEqual(language1, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language2, dictionary.Language2, "Validating that language2 is initialized correctly");
            Assert.AreEqual(maxCards, dictionary.MaxCards, "Validating that maxWords is initialized correctly");
        }

        [Test]
        public void TestInit2()
        {
            dictionary = new WordsDictionary(language2, language1);
            Assert.AreEqual(language2, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language1, dictionary.Language2, "Validating that language2 is initialized correctly");
            Assert.AreEqual(100, dictionary.MaxCards, "Validating that maxWords is initialized correctly");
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
            WordCard card = new WordCard(word11, word12, type1);
            dictionary.Add(card);
            WordCard cardExpected = new WordCard(word11, word12, type1);
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
            WordCard card1 = new WordCard(word11, word12, type1);
            WordCard card2 = new WordCard(word11, word12, type1);
            dictionary.Add(card1);
            dictionary.Add(card2);
        }

        [Test]
        public void TestAddWordCardMax()
        {
            WordCard card1 = new WordCard(word11, word12, type1);
            WordCard card2 = new WordCard(word21, word22, type2);
            WordCard card3 = new WordCard(word31, word32, type3);
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
            WordCard card1 = new WordCard(word11, word12, type1);
            WordCard card2 = new WordCard(word21, word22, type2);
            WordCard card3 = new WordCard(word31, word32, type3);
            WordCard card4 = new WordCard(word31, word32, type1);
            dictionary.Add(card1);
            dictionary.Add(card2);
            dictionary.Add(card3);
            dictionary.Add(card4);
        }
    }
}
