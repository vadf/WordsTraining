using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using WordsTraining.Model;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestDictionaryInfo
    {
        DictionaryInfo info;
        WordsDictionary dictionary;
        DataLayer dataLayer = new TestDataLayer();
        string name = "test";
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
            for (int i = 0; i < maxCards; i++)
            {
                dictionary.Add(generator.GetCardExtra(5));
            }

            dataLayer.Save(dictionary);
        }
        
        [TestMethod]
        public void TestDictionaryInfoInit()
        {
            info = new DictionaryInfo(name, dataLayer);
            Assert.AreEqual(name, info.Name, "Validating name");
            Assert.AreEqual(language1, info.Language1, "Validating lang1");
            Assert.AreEqual(language2, info.Language2, "Validating lang2");
            Assert.AreEqual(dictionary.Count, info.NumOfCards, "Validating number of cards");
            foreach (var type in WordCard.TrainingTypes)
            {
                double average = 0;
                foreach (var card in dictionary)
                {
                    average += (card.Counter1[type] + card.Counter2[type]) / 2.0;
                }
                average /= dictionary.Count;
                Assert.AreEqual(average.ToString("0.00"), info.TrainingAverage[type].ToString("0.00"));
            }
        }

        [TestMethod]
        public void TestDictionaryInfoNotEqual1()
        {
            info = new DictionaryInfo(name, dataLayer);
            DictionaryInfo newInfo = new DictionaryInfo(name + "123", dataLayer);
            Assert.AreNotEqual(newInfo, info);
        }

        [TestMethod]
        public void TestDictionaryInfoNotEqual2()
        {
            info = new DictionaryInfo(name, dataLayer);
            Assert.AreNotEqual(null, info);
        }

        [TestMethod]
        public void TestDictionaryInfoEqual1()
        {
            info = new DictionaryInfo(name, dataLayer);
            Assert.AreEqual(info, info);
        }

        [TestMethod]
        public void TestDictionaryInfoEqual2()
        {
            info = new DictionaryInfo(name, dataLayer);
            DictionaryInfo newInfo = new DictionaryInfo(String.Copy(name), dataLayer);
            Assert.AreEqual(newInfo, info);
        }
    }

    class TestDataLayer : DataLayer
    {
        WordsDictionary dictionary;

        public override WordsDictionary Read()
        {
            return dictionary;
        }

        public override void Save(WordsDictionary dictionary)
        {
            this.dictionary = dictionary;
        }
    }
}
