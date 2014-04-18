using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace WordsTraining
{
    public class XmlDataLayer : DataLayer
    {
        private string pathToXml;


        /// <summary>
        /// Initiates DataLayerXml with path to dictionary xml file
        /// </summary>
        /// <param name="pathToXml">Path to xml file</param>
        public XmlDataLayer(string pathToXml)
        {
            this.pathToXml = pathToXml;
            //if (!File.Exists(pathToXml))
            //    File.CreateText(pathToXml);
        }

        /// <summary>
        /// Read Dictionary from xml file
        /// Returns Dictionary
        /// </summary>
        /// <returns>WordsDictionary</returns>
        public override WordsDictionary Read()
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Save Dictionary to xml file
        /// </summary>
        /// <param name="dictionary">Dictionary to save</param>
        public override void Save(WordsDictionary dictionary)
        {
            XmlDocument xd = new XmlDocument();
            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlNode root = xd.CreateElement("dictionary");
            xd.AppendChild(root);

            XmlAttribute lan1 = xd.CreateAttribute("lang1");
            lan1.Value = dictionary.Language1;

            XmlAttribute lan2 = xd.CreateAttribute("lang2");
            lan2.Value = dictionary.Language2;

            root.Attributes.Append(lan1);
            root.Attributes.Append(lan2);

            foreach (var card in dictionary.Cards)
            {
                XmlNode wordCard = xd.CreateElement("card");

                XmlNode word1 = GetWordNode(xd, card, Language.Lang1);
                wordCard.AppendChild(word1);

                XmlNode word2 = GetWordNode(xd, card, Language.Lang2);
                wordCard.AppendChild(word2);

                if (card.CommentCommon != null && card.CommentCommon != "")
                {
                    XmlNode comment = xd.CreateElement("comment");
                    comment.InnerText = card.CommentCommon;
                    wordCard.AppendChild(comment);
                }

                root.AppendChild(wordCard);
            }

            byte [] byteArray = Encoding.UTF8.GetBytes(Resource.DictionaryScheme);
            MemoryStream streamXsd = new MemoryStream(byteArray);
            XmlReader scheme = XmlReader.Create(streamXsd);
            xd.Schemas.Add("", scheme);
            
            xd.Validate(null);
            xd.Save(pathToXml);
        }

        /// <summary>
        /// Create a word node for specific language
        /// </summary>
        /// <param name="xd">Xml Document</param>
        /// <param name="card">Card with words</param>
        /// <param name="lang">Language for word</param>
        /// <returns>Xml Node for word</returns>
        private XmlNode GetWordNode(XmlDocument xd, WordCard card, Language lang)
        {
            card.SelectedLanguage = lang;
            XmlNode word = xd.CreateElement("word" + (int)lang);
            XmlAttribute counter = xd.CreateAttribute("counter");
            counter.Value = card.SuccessfulCounter.ToString();
            word.Attributes.Append(counter);

            XmlNode text = xd.CreateElement("text");
            text.InnerText = card.Word;
            word.AppendChild(text);

            if (card.Comment != null && card.Comment != "")
            {
                XmlNode comment = xd.CreateElement("comment");
                comment.InnerText = card.Comment;
                word.AppendChild(comment);                
            }

            return word;
        }
    }
}
