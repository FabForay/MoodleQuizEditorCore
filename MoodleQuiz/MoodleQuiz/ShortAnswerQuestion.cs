using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{

    /// <summary>
    /// Question avec une liste réponse possible
    /// 
    /// </summary>
    public class ShortAnswerQuestion : Question
    {

        /// <summary>
        /// Indique le type de Question : ShortAnswer
        /// </summary>
        public override QuestionType Type
        {
            get
            {
                return QuestionType.ShortAnswer;
            }
        }

        private bool useCase;
        /// <summary>
        /// Is the Answer Case-Sensitive ?
        /// </summary>
        public bool UseCase { get => useCase; set => useCase = value; }

        /// <summary>
        /// Création d'une ShortAnswer par défaut
        /// </summary>
        public ShortAnswerQuestion() : this("", "", "")
        {
        }

        /// <summary>
        /// Création d'une ShortAnswer
        /// </summary>
        /// <param name="name">Titre de la Question</param>
        /// <param name="questionText">Texte de la Question</param>
        public ShortAnswerQuestion(String name, String questionText) : this(name, questionText, "")
        {
        }

        public ShortAnswerQuestion(String name, String questionText, String feedback) : base(name, questionText, feedback)
        {
            UseCase = false;
        }

        public override void AddAnswer(Answer answer)
        {
            double amount = 0;
            // On ne peut pas ajouter une Réponse si le total devient supérieur à 100%
            foreach (Answer ans in Answers)
            {
                amount += ans.Fraction;
            }
            //
            if (amount + answer.Fraction > 100)
                throw new OverflowException("Cannot have more that 100% as Answer's amount.");
            //
            base.AddAnswer(answer);
        }

        public bool Is100Percent
        {
            get
            {
                double amount = 0;
                // Il faut 100%
                foreach (Answer ans in Answers)
                {
                    amount += ans.Fraction;
                }
                //
                return (amount >= 99.99);
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
                node = xmlDoc.CreateElement("usecase");
                node.InnerText = (this.UseCase ? "1" : "0");
                rootNode.AppendChild(node);
                //
                foreach (var ans in Answers)
                {
                    XmlNode nodeCopy = xmlDoc.ImportNode(ans.MoodleXML, true);
                    rootNode.AppendChild(nodeCopy);
                }
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
                        if (string.Compare(attribute.Value, "shortanswer", true) == 0)
                        {
                            isOk = true;
                            break;
                        }
                    }
                }
                if (!isOk)
                {
                    throw new Exception("The Question is not a ShortAnswer Question.");
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
                    node = XMLHelpers.FindChildNodeByName(root, "usecase");
                    int useCase = Convert.ToInt32(node.InnerText);
                    this.UseCase = (useCase == 1);
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
