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
        Random random = new Random();

        CardsGenerator generator = new CardsGenerator();

        [SetUp]
        public void SetUp()
        {
            maxCards = random.Next(3, 100);
            dictionary = new WordsDictionary(language1, language2);
        }

        [Test]
        public void TestInit()
        {
            Assert.AreEqual(language1, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language2, dictionary.Language2, "Validating that language2 is initialized correctly");
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
            WordCard card = generator.GetCard();
            WordCard cardExpected = new WordCard(card.Word1, card.Word2, card.Type);
            dictionary.Add(card);
            WordCard cardActual = dictionary[0];
            Assert.AreEqual(cardExpected, cardActual, "Validating that card is added correctly");

            int sizeExpected = 1;
            int sizeActual = dictionary.Count;
            Assert.AreEqual(sizeExpected, sizeActual, "Validating dictionary size");
        }

        [Test]
        [Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddWordCardTwice()
        {
            WordCard card1 = generator.GetCard();
            WordCard card2 = new WordCard(card1.Word1, card1.Word2, card1.Type);
            dictionary.Add(card1);
            dictionary.Add(card2);
        }

        [Test]
        public void TestAddWordCardMax()
        {
            for (int i = 0; i < maxCards; i++)
            {
                dictionary.Add(generator.GetCard());
            }

            int sizeExpected = maxCards;
            int sizeActual = dictionary.Count;
            Assert.AreEqual(sizeExpected, sizeActual, "Validating dictionary size");
        }

        [Test]
        [Ignore]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAddWordCardMax1()
        {
            for (int i = 0; i < maxCards + 1; i++)
            {
                dictionary.Add(generator.GetCard());
            }
        }
    }
}
