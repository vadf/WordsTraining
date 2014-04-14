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
        string language1 = "ENG";
        string language2 = "EST";
        int maxCards = 5;

        string word11 = "Hi";
        string word12 = "Tere";
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

//            if (File.Exists(pathToXml))
//                File.Delete(pathToXml);
        }

        [Test]
        public void TestSaveWOComments()
        {
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
        }

        [Test]
        public void TestSaveWComments()
        {
            var card = dictionary.Get(0);
            card.CommentCommon = "Greeting";
            card = dictionary.Get(1);
            card.SelectedLanguage = Language.Lang1;
            card.Comment = "r'ääki[ma r'ääki[da räägi[b räägi[tud";
            XmlDataLayer doc = new XmlDataLayer(pathToXml);
            doc.Save(dictionary);
        }

        [Test]
        [ExpectedException(typeof(XmlSchemaValidationException))]
        public void TestInvalidXml()
        {
            
        }
    }

}
