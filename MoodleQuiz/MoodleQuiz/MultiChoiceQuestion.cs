using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{
    public class MultiChoiceQuestion : ShortAnswerQuestion
    {

        public override QuestionType Type
        {
            get
            {
                return QuestionType.MultiChoice;
            }
        }

        private void DefaultInit()
        {
            this.CorrectFeedback = "Votre réponse est correcte.";
            this.PartiallyCorrectFeedback = "Votre réponse est partiellement correcte.";
            this.IncorrectFeedback = "Votre réponse est incorrecte.";
        }

        public MultiChoiceQuestion() : base()
        {
            this.DefaultInit();
        }

        public MultiChoiceQuestion(String name, String questionText) : base(name, questionText)
        {
            this.DefaultInit();
        }

        public MultiChoiceQuestion(String name, String questionText, String feedback) : base(name, questionText, feedback)
        {
            this.DefaultInit();
        }

        public bool ShuffleAnswers { get => shuffleAnswers; set => shuffleAnswers = value; }
        public string CorrectFeedback { get => correctFeedback; set => correctFeedback = value; }
        public string PartiallyCorrectFeedback { get => partiallycorrectFeedback; set => partiallycorrectFeedback = value; }
        public string IncorrectFeedback { get => incorrectFeedback; set => incorrectFeedback = value; }

        private bool shuffleAnswers;

        private String correctFeedback;
        private String partiallycorrectFeedback;
        private String incorrectFeedback;

        public bool Single
        {
            get
            {
                bool single = false;
                // Il faut une question à 100%
                foreach (Answer ans in Answers)
                {
                    if (ans.Fraction == 100)
                    {
                        if (!single)
                        {
                            single = true;
                        }
                        else
                        {
                            single = false;
                            break;
                        }
                    }
                }
                //
                return single;
            }
        }

        public override XmlNode MoodleXML
        {
            get
            {
                //
                if (!this.Is100Percent)
                {
                    throw new Exception("Answer's amount must be 100%.");
                }
                //
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("question");
                rootNode.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "type", this.Type.ToString().ToLower()));
                xmlDoc.AppendChild(rootNode);
                //
                XmlNode node = XMLHelpers.CreateElementWithText(xmlDoc, "name", this.Name);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("questiontext");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));
                XMLHelpers.AddTextToElement(xmlDoc, node, this.QuestionText);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("generalfeedback");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));

                XMLHelpers.AddTextToElement(xmlDoc, node, this.GeneralFeedback);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("defaultgrade");
                node.InnerText = this.DefaultGrade.ToString();
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("penalty");
                node.InnerText = this.Penalty.ToString();
                rootNode.AppendChild(node);
                //
                //
                node = xmlDoc.CreateElement("correctfeedback");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));
                XMLHelpers.AddTextToElement(xmlDoc, node, this.CorrectFeedback);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("partiallycorrectfeedback");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));
                XMLHelpers.AddTextToElement(xmlDoc, node, this.PartiallyCorrectFeedback);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("incorrectfeedback");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));
                XMLHelpers.AddTextToElement(xmlDoc, node, this.IncorrectFeedback);
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("single");
                node.InnerText = this.Single.ToString().ToLower();
                rootNode.AppendChild(node);
                //
                node = xmlDoc.CreateElement("shuffleanswers");
                node.InnerText = this.ShuffleAnswers.ToString().ToLower();
                rootNode.AppendChild(node);
                //
                foreach (var ans in Answers)
                {
                    XmlNode nodeCopy = xmlDoc.ImportNode(ans.MoodleXML, true);
                    rootNode.AppendChild(nodeCopy);
                }
                //
                return rootNode;
            }

            set
            {
                // Root est l'élément category
                XmlNode root = value;
                if (string.Compare(root.Name, "question", true) != 0)
                {
                    throw new Exception("Cannot find the Question root.");
                }
                //
                bool isOk = false;
                foreach (XmlAttribute attribute in root.Attributes)
                {
                    if (string.Compare(attribute.Name, "type", true) == 0)
                    {
                        if (string.Compare(attribute.Value, "multichoice", true) == 0)
                        {
                            isOk = true;
                            break;
                        }
                    }
                }
                if (!isOk)
                {
                    throw new Exception("The Question is not a MultiChoice Question.");
                }
                //
                XmlNode node;
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "name");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.Name = node.InnerText;
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "questiontext");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.QuestionText = node.InnerText;
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "generalfeedback");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.GeneralFeedback = node.InnerText;
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "defaultgrade");
                    this.DefaultGrade = Convert.ToDouble(node.InnerText, CultureInfo.InvariantCulture.NumberFormat);
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "penalty");
                    this.Penalty = Convert.ToDouble(node.InnerText, CultureInfo.InvariantCulture.NumberFormat);
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "shuffleanswers");
                    this.ShuffleAnswers = Convert.ToBoolean(node.InnerText);
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "correctfeedback");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.CorrectFeedback = node.InnerText;
                }
                catch { }
                //
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "partiallyfeedback");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.PartiallyCorrectFeedback = node.InnerText;
                }
                catch { }
                //
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "incorrectfeedback");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.IncorrectFeedback = node.InnerText;
                }
                catch { }
                //
                this.Answers.Clear();
                List<XmlNode> nodes = XMLHelpers.FindChildNodesByName(root, "answer");
                foreach (var answernode in nodes)
                {
                    Answer ans = new Answer();
                    ans.MoodleXML = answernode;
                    //
                    AddAnswer(ans);
                }
            }
        }



    }
}
