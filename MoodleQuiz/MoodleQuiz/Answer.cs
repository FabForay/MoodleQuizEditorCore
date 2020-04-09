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
    /// Réponse possible pour une Question
    /// </summary>
    public class Answer
    {
        String answerText;
        String feedback;
        double fraction;

        /// <summary>
        /// Texte de la réponse
        /// </summary>
        public string Text { get => answerText; set => answerText = value; }

        /// <summary>
        /// Texte du FeedBack
        /// </summary>
        public string Feedback { get => feedback; set => feedback = value; }

        /// <summary>
        /// Pourcentage de valeur de cette réponse (sur 100%)
        /// </summary>
        public double Fraction { get => fraction; set => fraction = value; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Answer()
        {
            this.Text = "Réponse";
            this.Feedback = "Feedback";
            this.Fraction = 0;
        }

        /// <summary>
        /// Construit une Réponse
        /// </summary>
        /// <param name="answer">Réponse</param>
        /// <param name="feedback">Feedback</param>
        /// <param name="fraction">% de valeur</param>
        public Answer(String answer, String feedback, int fraction) : base()
        {
            this.Text = answer;
            this.Feedback = feedback;
            this.Fraction = fraction;
        }

        public XmlNode MoodleXML
        {
            /*
            <answer fraction="0" format="moodle_auto_format">
              <text>true</text>
              <feedback format="html">
                <text></text>
              </feedback>
            </answer>
             * */
            get
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode node = XMLHelpers.CreateElementWithText(xmlDoc, "answer", this.Text);
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "moodle_auto_format"));
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "fraction", this.Fraction.ToString()));
                //
                XmlNode subnode = XMLHelpers.CreateElementWithText(xmlDoc, "feedback", this.Feedback, true );
                subnode.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "format", "html"));
                //
                node.AppendChild(subnode);
                return node;
            }

            set
            {
                // Root est l'élément category
                XmlNode root = value;
                if (string.Compare(root.Name, "answer", true) != 0)
                {
                    throw new Exception("Cannot find the Answer root.");
                }
                //
                bool isOk = false;
                foreach (XmlAttribute attribute in root.Attributes)
                {
                    if (string.Compare(attribute.Name, "fraction", true) == 0)
                    {
                        this.Fraction = Convert.ToDouble(attribute.Value, CultureInfo.InvariantCulture.NumberFormat);
                        isOk = true;
                        break;
                    }
                }
                if (!isOk)
                {
                    throw new Exception("The Answer has no Fraction attribute.");
                }
                //
                XmlNode node = XMLHelpers.FindChildNodeByName(root, "text");
                this.Text = node.InnerText;
                //
                node = XMLHelpers.FindChildNodeByName(root, "feedback");
                node = XMLHelpers.FindChildNodeByName(node, "text");
                this.Feedback = node.InnerText;
                //
            }
        }

    }

}
