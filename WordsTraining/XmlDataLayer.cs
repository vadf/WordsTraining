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
        private XmlDocument xd;
        
        private string pathToXml;
        private string strRoot = "dictionary";
        private string strCard = "card";
        private string strLang1 = Language.Lang1.ToString().ToLower();
        private string strLang2 = Language.Lang2.ToString().ToLower();
        private string strComment = "comment";
        private string strWord = "word";
        private string strType = "type";
        private string strCounter = "counter";
        private string strText = "text";


        /// <summary>
        /// Initiates DataLayerXml with path to dictionary xml file
        /// </summary>
        /// <param name="pathToXml">Path to xml file</param>
        public XmlDataLayer(string pathToXml)
        {
            this.pathToXml = pathToXml;
            xd = new XmlDocument();

            // read XSD scheme for XML validation
            byte[] byteArray = Encoding.UTF8.GetBytes(Resource.DictionaryScheme);
            MemoryStream streamXsd = new MemoryStream(byteArray);
            XmlReader scheme = XmlReader.Create(streamXsd);
            xd.Schemas.Add("", scheme);
        }

        /// <summary>
        /// Read Dictionary from xml file
        /// Returns Dictionary
        /// </summary>
        /// <returns>WordsDictionary</returns>
        public override WordsDictionary Read()
        {            
            xd.Load(pathToXml);
            xd.Validate(null);
            XmlElement root = xd.DocumentElement;
            WordsDictionary dictionary = new WordsDictionary(root.Attributes[strLang1].Value, root.Attributes[strLang2].Value);

            XmlNodeList cardsList = xd.GetElementsByTagName(strCard);
            
            for (int i = 0; i < cardsList.Count; i++)
            {                
                // create word card class
                string word1 = cardsList[i].ChildNodes[0].ChildNodes[0].InnerText;
                string word2 = cardsList[i].ChildNodes[1].ChildNodes[0].InnerText;
                string type = cardsList[i].Attributes[strType].Value; 
                WordCard card = new WordCard(word1, word2, (WordType)Enum.Parse(typeof(WordType), type));
                
                // set counter and comment for word1
                card.SelectedLanguage = Language.Lang1;
                card.SuccessfulCounter = uint.Parse(cardsList[i].ChildNodes[0].Attributes[strCounter].Value);
                if (cardsList[i].ChildNodes[0].ChildNodes.Count > 1)
                    card.Comment = cardsList[i].ChildNodes[0].ChildNodes[1].InnerText;

                // set counter and comment for word2
                card.SelectedLanguage = Language.Lang2;
                card.SuccessfulCounter = uint.Parse(cardsList[i].ChildNodes[1].Attributes[strCounter].Value);
                if (cardsList[i].ChildNodes[1].ChildNodes.Count > 1)
                    card.Comment = cardsList[i].ChildNodes[1].ChildNodes[1].InnerText;

                // set common comment
                if (cardsList[i].ChildNodes.Count > 2)
                    card.CommentCommon = cardsList[i].ChildNodes[2].InnerText;

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
            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlNode root = xd.CreateElement(strRoot);
            xd.AppendChild(root);

            XmlAttribute lang1 = xd.CreateAttribute(strLang1);
            lang1.Value = dictionary.Language1;

            XmlAttribute lang2 = xd.CreateAttribute(strLang2);
            lang2.Value = dictionary.Language2;

            root.Attributes.Append(lang1);
            root.Attributes.Append(lang2);

            // iterate over each card and add it to xml
            foreach (var card in dictionary.Cards)
            {
                XmlNode wordCard = xd.CreateElement(strCard);

                XmlAttribute type = xd.CreateAttribute(strType);
                type.Value = card.Type.ToString();
                wordCard.Attributes.Append(type);

                XmlNode word1 = GetWordNode(xd, card, Language.Lang1);
                wordCard.AppendChild(word1);

                XmlNode word2 = GetWordNode(xd, card, Language.Lang2);
                wordCard.AppendChild(word2);

                if (card.CommentCommon != null)
                {
                    XmlNode comment = xd.CreateElement(strComment);
                    comment.InnerText = card.CommentCommon;
                    wordCard.AppendChild(comment);
                }

                root.AppendChild(wordCard);
            }
            
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
            XmlNode word = xd.CreateElement(strWord + (int)lang);
            XmlAttribute counter = xd.CreateAttribute(strCounter);
            counter.Value = card.SuccessfulCounter.ToString();
            word.Attributes.Append(counter);

            XmlNode text = xd.CreateElement(strText);
            text.InnerText = card.Word;
            word.AppendChild(text);

            if (card.Comment != null)
            {
                XmlNode comment = xd.CreateElement(strComment);
                comment.InnerText = card.Comment;
                word.AppendChild(comment);                
            }

            return word;
        }
    }
}
