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
        int maxCards = 5;

        string word11 = "Tere";
        string word12 = "Hi";
        WordType type1 = WordType.Noun;

        string word21 = "rääkima";
        string word22 = "speak";
        WordType type2 = WordType.Verb;

        string word31 = "hästi";
        string word32 = "well";
        WordType type3 = WordType.Adverb;

        string pathToXml = "dictionary.xml";

        [SetUp]
        public void SetUp()
        {
            dictionary = new WordsDictionary(language1, language2, maxCards);
            WordCard card1 = new WordCard(word11, word12, type1);
            WordCard card2 = new WordCard(word21, word22, type2);
            WordCard card3 = new WordCard(word31, word32, type3);
            dictionary.Add(card1);
            dictionary.Add(card2);
            dictionary.Add(card3);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(pathToXml);
        }

        [Test]
        public void TestSaveReadWComments()
        {
            dictionary.SelectedIndex = 0;
            WordCard card = dictionary.SelectedCard;
            card.CommentCommon = "Greeting";
            dictionary.SelectedIndex = 1;
            card = dictionary.SelectedCard;
            card.SelectedLanguage = Language.Lang1;
            card.Comment = "r'ääki[ma r'ääki[da räägi[b räägi[tud";

            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        [Test]
        public void TestSaveReadWOComments()
        {
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }

        [Test]
        public void TestSaveReadCountersIncreased()
        {
            dictionary.SelectedIndex = 0;
            WordCard card = dictionary.SelectedCard;
            card.SelectedLanguage = Language.Lang1;
            card.SuccessfulCounter = 2;
            dictionary.SelectedIndex = 1;
            card = dictionary.SelectedCard;
            card.SelectedLanguage = Language.Lang2;
            card.SuccessfulCounter = 1;

            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
            var dictionaryRead = doc.Read();
            ValidatingDictionaries(dictionary, dictionaryRead);
        }


        private void ValidatingDictionaries(WordsDictionary expected, WordsDictionary actual)
        {
            Assert.AreEqual(expected.Size, actual.Size, "Validating read dictionary size");
            Assert.AreEqual(expected.Language1, actual.Language1, "Validating Language1");
            Assert.AreEqual(expected.Language2, actual.Language2, "Validating Language2");
            for (int i = 0; i < expected.Size; i++)
            {
                expected.SelectedIndex = 0;
                WordCard cardExpected = expected.SelectedCard;
                actual.SelectedIndex = 0;
                WordCard cardActual = actual.SelectedCard;

                Assert.AreEqual(cardExpected, cardActual, "Validating card " + i + " words");
                Assert.AreEqual(cardExpected.CommentCommon, cardActual.CommentCommon, "Validating common comment for card");
                cardExpected.SelectedLanguage = Language.Lang1;
                cardActual.SelectedLanguage = Language.Lang1;
                Assert.AreEqual(cardExpected.Comment, cardActual.Comment, "Validating comment1 for card");
                cardExpected.SelectedLanguage = Language.Lang2;
                cardActual.SelectedLanguage = Language.Lang2;
                Assert.AreEqual(cardExpected.Comment, cardActual.Comment, "Validating comment2 for card");
            }
        }
    }

}
