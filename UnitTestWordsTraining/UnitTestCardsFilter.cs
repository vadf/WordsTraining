using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using WordsTraining.Model;

namespace UnitTestWordsTraining
{
    [TestClass]
    public class UnitTestCardsFilter
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
            for (int i = 0; i < maxCards; i++)
            {
                dictionary.Add(generator.GetCardExtra(5));
            }
        }

        [TestMethod]
        public void TestFilterByWord_OK()
        {
            string word = new String('a', 5);
            dictionary[2].Word1 = word;
            int actual = 1; // CountWordsByWord(dictionary, word);
            var result = CardsFilter.FilterByWord(dictionary, word);
            int expected = CheckFilterWords(result, word);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByWord_CaseInsensitive()
        {
            string word = new String('b', 5);
            dictionary[2].Word2 = word.ToUpper();
            int actual = 1; // CountWordsByWord(dictionary, word);
            var result = CardsFilter.FilterByWord(dictionary, word);
            int expected = CheckFilterWords(result, word);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByWord_OneSimbol()
        {
            string word = "c";
            int actual = CountWordsByWord(dictionary, word);
            var result = CardsFilter.FilterByWord(dictionary, word);
            int expected = CheckFilterWords(result, word);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByWord_EmptyResult()
        {
            string word = new String('d', 15);
            int actual = 0;
            var result = CardsFilter.FilterByWord(dictionary, word);
            int expected = CheckFilterWords(result, word);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByWord_EmptyWord()
        {
            string word = "";
            int actual = dictionary.Count;
            var result = CardsFilter.FilterByWord(dictionary, word);
            int expected = CheckFilterWords(result, word);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByType_OK()
        {
            WordType type = WordType.Noun;
            foreach (var card in dictionary)
                card.Type = WordType.Verb;
            dictionary[0].Type = type;
            int actual = 1;
            var result = CardsFilter.FilterByType(dictionary, type);
            int expected = CheckFilterType(result, type);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByType_ResultEmpty()
        {
            WordType type = WordType.Noun;
            foreach (var card in dictionary)
                card.Type = WordType.Verb;
            int actual = 0;
            var result = CardsFilter.FilterByType(dictionary, type);
            int expected = CheckFilterType(result, type);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByType_Random()
        {
            var tmp = generator.GetCard();
            WordType type = tmp.Type;
            int actual = CountWordsByType(dictionary, type);
            var result = CardsFilter.FilterByType(dictionary, type);
            int expected = CheckFilterType(result, type);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFilterByCounterEqual()
        {
            int counter = 3;
            foreach (var card in dictionary)
            {
                card.Counter1 = counter + 1;
                card.Counter2 = counter - 1;
            }
            dictionary[dictionary.Count - 1].Counter1 = counter;
            int actual = 1;
            var result = CardsFilter.FilterByCounter(dictionary, FilterType.Equals, counter);
            int expected = CheckFilterCounter(result, FilterType.Equals, counter);
            Assert.AreEqual(actual, expected);
        }

        public void TestFilterByCounterEqual_ResultEmpty()
        {
            int counter = 5;
            foreach (var card in dictionary)
            {
                card.Counter1 = counter + 1;
                card.Counter2 = counter - 1;
            }
            int actual = 0;
            var result = CardsFilter.FilterByCounter(dictionary, FilterType.Equals, counter);
            int expected = CheckFilterCounter(result, FilterType.Equals, counter);
            Assert.AreEqual(actual, expected);
        }

        public void TestFilterByCounterEqual_Random()
        {
            int counter = random.Next(5);
            int actual = CountWordsByCounter(dictionary, FilterType.Equals, counter);
            var result = CardsFilter.FilterByCounter(dictionary, FilterType.Equals, counter);
            int expected = CheckFilterCounter(result, FilterType.Equals, counter);
            Assert.AreEqual(actual, expected);
        }

        // TODO: tests for less and more

        #region CheckResults

        // Check that all words in result contians specified word
        // return words amount in result
        private int CheckFilterWords(IEnumerable<WordCard> result, string word)
        {
            int count = 0;
            foreach (var item in result)
            {
                Assert.IsTrue(item.Word1.ToLower().Contains(word.ToLower()) || item.Word2.ToLower().Contains(word.ToLower()));
                count++;
            }
            return count;
        }

        // Check that all words in result have specified counter value
        // return words amount in result
        private int CheckFilterCounter(IEnumerable<WordCard> result, FilterType type, int counter)
        {
            int count = 0;
            foreach (var item in result)
            {
                if (type == FilterType.Equals)
                    Assert.IsTrue(item.Counter1 == counter || item.Counter2 == counter);
                else if (type == FilterType.More)
                    Assert.IsTrue(item.Counter1 > counter || item.Counter2 > counter);
                else if (type == FilterType.Less)
                    Assert.IsTrue(item.Counter1 < counter || item.Counter2 < counter);
                count++;
            }
            return count;
        }

        // Check that all words in result have specified type
        // return words amount in result
        private int CheckFilterType(IEnumerable<WordCard> result, WordType type)
        {
            int count = 0;
            foreach (var item in result)
            {
                Assert.IsTrue(item.Type == type);
                count++;
            }
            return count;
        }

        // count amount of words with specified word in dictionary
        private int CountWordsByWord(WordsDictionary dictionary, string word)
        {
            int count = 0;
            foreach (var card in dictionary)
            {
                if (card.Word1.ToLower().Contains(word.ToLower()) || card.Word2.ToLower().Contains(word.ToLower()))
                    count++;
            }
            return count;
        }

        // count amount of words with specified type in dictionary
        private int CountWordsByType(WordsDictionary dictionary, WordType type)
        {
            int count = 0;
            foreach (var card in dictionary)
            {
                if (card.Type == type)
                    count++;
            }
            return count;
        }

        // count amount of words with specified counter in dictionary
        private int CountWordsByCounter(WordsDictionary dictionary, FilterType type, int counter)
        {
            int count = 0;
            foreach (var item in dictionary)
            {
                if (type == FilterType.Equals)
                {
                    if (item.Counter1 == counter || item.Counter2 == counter)
                        count++;
                }
                else if (type == FilterType.More)
                {
                    if (item.Counter1 > counter || item.Counter2 > counter)
                        count++;
                }
                else if (type == FilterType.Less)
                {
                    if (item.Counter1 < counter || item.Counter2 < counter)
                        count++;
                }
            }
            return count;

        }

        #endregion
    }
}
