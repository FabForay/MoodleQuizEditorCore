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
    /// Question de type Vrai / Faux
    /// </summary>
    public class TrueFalseQuestion : Question
    {

        /// <summary>
        /// Indique le type de Question : TrueFalse
        /// </summary>
        public override QuestionType Type
        {
            get
            {
                return QuestionType.TrueFalse;
            }
        }

        public Answer AnswerTrue
        {
            get
            {
                Answer ans = Answers.Find(x => (String.Compare(x.Text, "true", true) == 0));
                if (ans != null)
                    return ans;
                throw new NullReferenceException("No True Answer in Answers.");
            }
        }

        public Answer AnswerFalse
        {
            get
            {
                Answer ans = Answers.Find(x => (String.Compare(x.Text, "false", true) == 0));
                if (ans != null)
                    return ans;
                throw new NullReferenceException("No False Answer in Answers.");
            }
        }

        /// <summary>
        /// Texte du FeedBack pour la réponse True
        /// </summary>
        public string FeedbackTrue
        {
            get
            {
                return AnswerTrue.Feedback;
            }

            set
            {
                AnswerTrue.Feedback = value;
            }
        }

        /// <summary>
        /// Texte du FeedBack pour la réponse False
        /// </summary>
        public string FeedbackFalse
        {
            get
            {
                return AnswerFalse.Feedback;
            }

            set
            {
                AnswerFalse.Feedback = value;
            }
        }


        /// <summary>
        /// Indique quelle est la réponse correcte
        /// </summary>
        public bool Correct
        {
            get
            {
                return (AnswerTrue.Fraction == 100);
            }

            set
            {
                if (value)
                {
                    AnswerFalse.Fraction = 0;
                    AnswerTrue.Fraction = 100;
                }
                else
                {
                    AnswerTrue.Fraction = 0;
                    AnswerFalse.Fraction = 100;
                }
            }
        }

        /// <summary>
        /// Constructeur d'une Question de type Vrai / Faux
        /// </summary>
        /// <param name="name">Titre de la Question</param>
        /// <param name="questionText">Texte de la Question</param>
        /// <param name="feedback">Feedback général de la Question</param>
        /// <param name="answer">Valeur booléenne indiquant la réponse correcte</param>
        /// <param name="feedbackTrue">Texte du FeedBack pour la réponse True</param>
        /// <param name="feedbackFalse">Texte du FeedBack pour la réponse False</param>

        public TrueFalseQuestion(String name, String questionText, String feedback, bool answer,
            String feedbackTrue, String feedbackFalse) : base(name, questionText, feedback)
        {
            this.AddAnswer("true", feedbackTrue, answer ? 100 : 0);
            this.AddAnswer("false", feedbackFalse, answer ? 0 : 100);
        }

        public TrueFalseQuestion(String name, String questionText, String feedback, bool answer) :
            this(name, questionText, feedback, answer, "", "")
        {
        }

        public TrueFalseQuestion() : this("", "", "", true)
        {
        }

        public override void AddAnswer(Answer answer)
        {
            if (this.Answers.Count >= 2)
                throw new OverflowException("Cannot have more that 2 Answers.");
            //
            base.AddAnswer(answer);
        }


        public override XmlNode MoodleXML
        {
            get
            {
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
                        if (string.Compare(attribute.Value, "truefalse", true) == 0)
                        {
                            isOk = true;
                            break;
                        }
                    }
                }
                if (!isOk)
                {
                    throw new Exception("The Question is not a TrueFalse Question.");
                }
                //
                XmlNode node;
                try
                {
                    node = XMLHelpers.FindChildNodeByName(root, "name");
                    node = XMLHelpers.FindChildNodeByName(node, "text");
                    this.Name = node.InnerText;
                }
                catch{ }
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
                List<XmlNode> nodes = XMLHelpers.FindChildNodesByName(root, "answer");
                foreach (var answernode in nodes)
                {
                    Answer ans = new Answer();
                    ans.MoodleXML = answernode;
                    //
                    if ( String.Compare( "true", ans.Text, true ) == 0 )
                    {
                        this.AnswerTrue.Feedback = ans.Feedback;
                        this.AnswerTrue.Fraction = ans.Fraction;
                    }
                    else
                    {
                        this.AnswerFalse.Feedback = ans.Feedback;
                        this.AnswerFalse.Fraction = ans.Fraction;
                    }
                }
            }
        }
    }
}
