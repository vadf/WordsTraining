using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;

using WordsTraining.Model;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestXmlDictionariesService
    {
        DictionariesService dictionaryService;
        string folder = "dictionaries";
        WordsDictionary dictionary;
        DataLayer dataLayer;

        [TestInitialize]
        public void SetUp()
        {
            Directory.CreateDirectory(folder);
            dictionary = new WordsDictionary("Lang1", "Lang2");
            dictionary.Add(new WordCard("Hi", "World", WordType.Noun));
        }

        [TestCleanup]
        public void TearDown()
        {
            Directory.Delete(folder, true);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceInitWithEmptyFolder()
        {
            dictionaryService = new XmlDictionariesService(folder);
            Assert.AreEqual(0, dictionaryService.Dictionaries.Count, "Validating that dictionaries were not created after read empty folder ");
        }

        [TestMethod]
        public void TestXmlDictionariesServiceInit1Dictionary()
        {
            string dName = "test";
            dataLayer = new XmlDataLayer(Path.Combine(folder, dName + ".xml"));
            dataLayer.Save(dictionary);
            dictionaryService = new XmlDictionariesService(folder);
            Assert.AreEqual(1, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestXmlDictionariesServiceInitFailed()
        {
            string dName = "test";
            File.WriteAllText(Path.Combine(folder, dName + ".xml"), "Fake");
            dictionaryService = new XmlDictionariesService(folder);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceAddToNewFolder()
        {
            dictionaryService = new XmlDictionariesService(folder);
            bool result = dictionaryService.AddDictionary("test", "Lang1", "Lang2");
            Assert.IsTrue(result);
            Assert.AreEqual(1, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceAddOk()
        {
            string dName1 = "test1";
            dataLayer = new XmlDataLayer(Path.Combine(folder, dName1 + ".xml"));
            dataLayer.Save(dictionary);
            string dName2 = "test2";
            dataLayer = new XmlDataLayer(Path.Combine(folder, dName2 + ".xml"));
            dataLayer.Save(dictionary);
            dictionaryService = new XmlDictionariesService(folder);
            Assert.AreEqual(2, dictionaryService.Dictionaries.Count);
            bool result = dictionaryService.AddDictionary("test", "Lang1", "Lang2");
            Assert.IsTrue(result);
            Assert.AreEqual(3, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceAddDuplicate()
        {
            string dName1 = "test1";
            dictionaryService = new XmlDictionariesService(folder);
            dictionaryService.AddDictionary(dName1, "Lang1", "Lang2");
            bool result = dictionaryService.AddDictionary(dName1, "WrongLang1", "WrongLang2");
            Assert.IsFalse(result);
            Assert.AreEqual(1, dictionaryService.Dictionaries.Count);

            // check that old dictionary was not overwritten
            dictionaryService = new XmlDictionariesService(folder);
            Assert.AreEqual("Lang1", dictionaryService.Dictionaries[0].Language1);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceRemoveByName()
        {
            string dName = "test";
            dataLayer = new XmlDataLayer(Path.Combine(folder, dName + ".xml"));
            dataLayer.Save(dictionary);
            dictionaryService = new XmlDictionariesService(folder);
            bool result = dictionaryService.RemoveDictionary(dName);
            Assert.IsTrue(result);
            Assert.AreEqual(0, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceRemoveByNameEmpty()
        {
            dictionaryService = new XmlDictionariesService(folder);
            bool result = dictionaryService.RemoveDictionary("test");
            Assert.IsFalse(result);
            Assert.AreEqual(0, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceRemoveByDictionary()
        {
            string dName = "test";
            dataLayer = new XmlDataLayer(Path.Combine(folder, dName + ".xml"));
            dataLayer.Save(dictionary);
            dictionaryService = new XmlDictionariesService(folder);
            bool result = dictionaryService.RemoveDictionary(dictionaryService.Dictionaries[0]);
            Assert.IsTrue(result);
            Assert.AreEqual(0, dictionaryService.Dictionaries.Count);
        }

        [TestMethod]
        public void TestXmlDictionariesServiceAddRemove()
        {
            string dName = "test";            
            dictionaryService = new XmlDictionariesService(folder);
            dictionaryService.AddDictionary(dName, "Lang1", "Lang2");
            bool result = dictionaryService.RemoveDictionary(dName);
            Assert.IsTrue(result);
            Assert.AreEqual(0, dictionaryService.Dictionaries.Count);
        }
    }

}
