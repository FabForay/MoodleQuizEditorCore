using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{

    public class Quiz
    {

        public List<String> Categories
        {
            get
            {
                return new List<String>(Questions.Keys);
            }
        }

        private Category defaultCategory;
        public Category DefaultCategory
        {
            get => defaultCategory;
            private set
            {
                string addTo = null;
                foreach (var cat in Questions.Keys)
                {
                    if (String.Compare(cat, value.Text, true) == 0)
                    {
                        addTo = value.Text;
                        break;
                    }
                }
                // Doesn't exist ?
                if (addTo == null)
                {
                    List<Question> qList = new List<Question>();
                    Questions.Add(value.Text, qList);
                }
                defaultCategory = value;
            }
        }

        Dictionary<String, List<Question>> questions = new Dictionary<String, List<Question>>();

        public Dictionary<String, List<Question>> Questions { get => questions; }


        public Quiz()
        {
            this.DefaultCategory = new Category();
        }

        public Quiz(string xmlFileContent) : this()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFileContent);
            //
            XmlElement root = doc.DocumentElement;
            //
            this.MoodleXML = (XmlNode)root;
        }

        public Question AddQuestion(Question question)
        {
            AddQuestion(this.DefaultCategory, question);
            return question;
        }

        public Question AddQuestion(Category category, Question question)
        {
            return AddQuestion(category.Text, question);
        }

        public Question AddQuestion(string category, Question question)
        {
            string addTo = null;
            foreach (var cat in Questions.Keys)
            {
                if (String.Compare(cat, category, true) == 0)
                {
                    addTo = cat;
                    break;
                }
            }
            // Doesn't exist ?
            if (addTo == null)
            {
                List<Question> qList = new List<Question>();
                qList.Add(question);
                Questions.Add(category, qList);
            }
            else
            {
                List<Question> qList = Questions[addTo];
                qList.Add(question);
            }
            return question;
        }

        public XmlNode MoodleXML
        {
            get
            {
                //
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("quiz");
                //
                foreach (var cat in Questions)
                {
                    // First the Category
                    Category catInfo = new Category(cat.Key);
                    XmlNode category = xmlDoc.ImportNode(catInfo.MoodleXML, true);
                    rootNode.AppendChild(category);
                    //
                    foreach (var question in cat.Value)
                    {
                        XmlNode nodeCopy = xmlDoc.ImportNode(question.MoodleXML, true);
                        rootNode.AppendChild(nodeCopy);
                    }
                }
                //
                return rootNode;
            }

            set
            {
                // Root est l'élément quiz
                XmlNode root = value;
                if (string.Compare(root.Name, "quiz", true) != 0)
                {
                    throw new Exception("Cannot find the Quiz root.");
                }
                //
                foreach (XmlNode node in root.ChildNodes)
                {
                    string name = node.Name.ToLower();
                    //
                    switch (name)
                    {
                        case "question":
                            // Category/truefalse/shortanswer/multichoice??
                            foreach (XmlAttribute attribute in node.Attributes)
                            {
                                if (string.Compare(attribute.Name, "type", true) == 0)
                                {
                                    string typeName = attribute.Value.ToLower();
                                    switch (typeName)
                                    {
                                        case "category":
                                            Category cat = new Category();
                                            cat.MoodleXML = node;
                                            this.DefaultCategory = cat;
                                            break;
                                        case "truefalse":
                                            TrueFalseQuestion tfQuestion = new TrueFalseQuestion();
                                            tfQuestion.MoodleXML = node;
                                            this.AddQuestion(tfQuestion);
                                            break;
                                        case "shortanswer":
                                            ShortAnswerQuestion saQuestion = new ShortAnswerQuestion();
                                            saQuestion.MoodleXML = node;
                                            this.AddQuestion(saQuestion);
                                            break;
                                        case "multichoice":
                                            MultiChoiceQuestion mcQuestion = new MultiChoiceQuestion();
                                            mcQuestion.MoodleXML = node;
                                            this.AddQuestion(mcQuestion);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            //
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Load(string xmlFileFullPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFileFullPath);
            //
            XmlElement root = doc.DocumentElement;
            //
            this.MoodleXML = (XmlNode)root;
        }


    }
}
