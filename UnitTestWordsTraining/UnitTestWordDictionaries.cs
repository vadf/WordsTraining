using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordsTraining.Model;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestWordDictionaries
    {
        WordsDictionary dictionary;
        string language1 = "ENG";
        string language2 = "EST";
        int maxCards = 3;
        Random random = new Random();

        CardsGenerator generator = new CardsGenerator();

        [TestInitialize]
        public void SetUp()
        {
            maxCards = random.Next(3, 100);
            dictionary = new WordsDictionary(language1, language2);
        }

        [TestMethod]
        public void TestWordsDictionaryInit()
        {
            Assert.AreEqual(language1, dictionary.Language1, "Validating that language1 is initialized correctly");
            Assert.AreEqual(language2, dictionary.Language2, "Validating that language2 is initialized correctly");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWordsDictionaryInitFailed1()
        {
            dictionary = new WordsDictionary(null, language2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWordsDictionaryInitFailed2()
        {
            dictionary = new WordsDictionary(language1, "");
        }

        [TestMethod]
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

        [TestMethod]
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
    }
}
