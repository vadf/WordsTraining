using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.Linq;

namespace WordsTraining.Model
{
    public class XmlDataLayer : DataLayer
    {
        private string pathToXml;

        // name of xml tags and attributes
        private string strRoot = "dictionary";
        private string strLang1 = Language.Lang1.ToString().ToLower();
        private string strLang2 = Language.Lang2.ToString().ToLower();
        private string strCard = "card";
        private string strComment = "comment";
        private string strWordText = "text";
        private string strWordType = "type";
        private string strCounters = "counters";
        private string strCounter = "counter";
        private string strCounterType = "type";
        private string strCounterValue = "value";

        /// <summary>
        /// Initiates DataLayerXml with path to dictionary xml file
        /// </summary>
        /// <param name="pathToXml">Path to xml file</param>
        public XmlDataLayer(string pathToXml)
        {
            this.pathToXml = pathToXml;
        }

        /// <summary>
        /// Read Dictionary from xml file
        /// Returns Dictionary
        /// </summary>
        /// <returns>WordsDictionary</returns>
        public override WordsDictionary Read()
        {
            XDocument xd = XDocument.Load(pathToXml);

            WordsDictionary dictionary = new WordsDictionary(xd.Root.Attribute(strLang1).Value, xd.Root.Attribute(strLang2).Value);

            foreach (XElement cardXml in xd.Root.Elements(strCard))
            {
                XElement word1Xml = cardXml.Element(strLang1);
                XElement word2Xml = cardXml.Element(strLang2);

                // create word card class
                string word1 = word1Xml.Element(strWordText).Value;
                string word2 = word2Xml.Element(strWordText).Value;
                string type = cardXml.Attribute(strWordType).Value;
                WordCard card = new WordCard(word1, word2, (WordType)Enum.Parse(typeof(WordType), type));

                // set counter and comment for word1
                foreach (XElement counter in word1Xml.Element(strCounters).Elements(strCounter))
                {
                    TrainingType trainingType = (TrainingType)Enum.Parse(typeof(TrainingType), counter.Attribute(strCounterType).Value, true);
                    card.Counter1[trainingType] = int.Parse(counter.Attribute(strCounterValue).Value);
                }
                XElement commentXml = word1Xml.Element(strComment);
                if (commentXml != null)
                    card.Comment1 = commentXml.Value;

                // set counter and comment for word2
                foreach (XElement counter in word2Xml.Element(strCounters).Elements(strCounter))
                {
                    TrainingType trainingType = (TrainingType)Enum.Parse(typeof(TrainingType), counter.Attribute(strCounterType).Value, true);
                    card.Counter2[trainingType] = int.Parse(counter.Attribute(strCounterValue).Value);
                }
                commentXml = word2Xml.Element(strComment);
                if (commentXml != null)
                    card.Comment2 = commentXml.Value;

                // set common comment
                commentXml = cardXml.Element(strComment);
                if (commentXml != null)
                    card.CommentCommon = commentXml.Value;

                dictionary.Add(card);
            }

            return dictionary;
        }


        /// <summary>
        /// Save Dictionary to xml file
        /// </summary>
        /// <param name="dictionary">Dictionary to save</param>
        public override void Save(WordsDictionary dictionary)
        {
            XDocument xd = new XDocument();
            xd.Declaration = new XDeclaration("1.0", "utf-8", "");

            XElement root = new XElement(strRoot, new XAttribute(strLang1, dictionary.Language1), new XAttribute(strLang2, dictionary.Language2));
            xd.Add(root);

            // iterate over each card and add it to xml
            foreach (var card in dictionary)
            {
                // switch back all cards, that could be switched during training
                card.Switched = false;

                XElement wordCard = new XElement(strCard, new XAttribute(strWordType, card.Type.ToString()));

                XElement word1 = GetWordNode(card.Word1, card.Comment1, card.Counter1, strLang1);
                wordCard.Add(word1);

                XElement word2 = GetWordNode(card.Word2, card.Comment2, card.Counter2, strLang2);
                wordCard.Add(word2);

                if (card.CommentCommon != null)
                {
                    XElement comment = new XElement(strComment, card.CommentCommon);
                    wordCard.Add(comment);
                }

                root.Add(wordCard);
            }

            xd.Save(pathToXml);
        }


        /// <summary>
        /// Create a word node for specific language
        /// </summary>
        /// <param name="cardWord">Word</param>
        /// <param name="cardComment">Comment</param>
        /// <param name="cardCounter">Counter</param>
        /// <param name="lang">Language for word</param>
        /// <returns>Xml Node for word</returns>
        private XElement GetWordNode(string cardWord, string cardComment, Dictionary<TrainingType, int> cardCounter, string lang)
        {
            XElement word = new XElement(lang,
                new XElement(strWordText, cardWord));
            XElement counters = new XElement(strCounters);
            foreach (KeyValuePair<TrainingType, int> pair in cardCounter)
            {
                XElement counter = new XElement(strCounter, new XAttribute(strCounterType, pair.Key.ToString()), new XAttribute(strCounterValue, pair.Value.ToString()));
                counters.Add(counter);
            }
            word.Add(counters);

            if (cardComment != null)
            {
                XElement comment = new XElement(strComment, cardComment);
                word.Add(comment);
            }

            return word;
        }

        private XmlDocument XmlDocInit()
        {
            XmlDocument xd = new XmlDocument();

            // read XSD scheme for XML validation
            byte[] byteArray = Encoding.UTF8.GetBytes(Resource.DictionaryScheme);
            MemoryStream streamXsd = new MemoryStream(byteArray);
            XmlReader scheme = XmlReader.Create(streamXsd);
            xd.Schemas.Add("", scheme);
            return xd;
        }
    }
}