using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WordsTraining;
using System.IO;
using System.Xml.Schema;

namespace TestWordsTraining
{
    [TestFixture]
    class TestXmlDataLayer
    {
        WordsDictionary dictionary;
        string language1 = "EST";
        string language2 = "ENG";
        int maxCards;
        CardsGenerator generator = new CardsGenerator();
        Random random = new Random();

        string pathToXml = "dictionary.xml";

        [SetUp]
        public void SetUp()
        {
            maxCards = random.Next(5, 100);
            dictionary = new WordsDictionary(language1, language2);
            for (int i = 0; i < maxCards/2; i++)
            {
                dictionary.Add(generator.GetCard());
            }
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(pathToXml);
        }

        [Test]
        public void TestSaveReadWComments()
        {
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        [Test]
        public void TestSaveReadWOComments()
        {
            WordCard card = dictionary[0];
            card.CommentCommon = "";
            card = dictionary[1];
            card.Comment1 = null;
            
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        [Test]
        public void TestSaveReadCountersIncreased()
        {
            for (int i = maxCards / 2; i < maxCards - 1; i++)
            {
                dictionary.Add(generator.GetCardExtra(5));
            }

            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        [Test]
        public void TestSaveReadTwice()
        {
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);

            dictionary = dictionaryRead;
            dictionary[0].Word1 = "qwer";
            dictionary[1].CommentCommon = "asdf";
            doc.Save(dictionary);
            dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        private void ValidatingDictionaries(WordsDictionary expected, WordsDictionary actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Validating read dictionary size");
            Assert.AreEqual(expected.Language1, actual.Language1, "Validating Language1");
            Assert.AreEqual(expected.Language2, actual.Language2, "Validating Language2");
            for (int i = 0; i < expected.Count; i++)
            {
                WordCard cardExpected = expected[i];
                WordCard cardActual = actual[i];

                Assert.AreEqual(cardExpected, cardActual, "Validating card " + i + " words");
                Assert.AreEqual(cardExpected.CommentCommon, cardActual.CommentCommon, "Validating common comment for card " + i);
                Assert.AreEqual(cardExpected.Comment1, cardActual.Comment1, "Validating comment1 for card " + i);
                Assert.AreEqual(cardExpected.Comment2, cardActual.Comment2, "Validating comment2 for card " + i);
            }
        }
    }

}
