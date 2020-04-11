using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{

    /// <summary>
    /// Definit le type de Question possible
    /// </summary>
    public enum QuestionType
    {
        TrueFalse,
        ShortAnswer,
        MultiChoice
    }

    /// <summary>
    /// Classe abstraite Question qui définit les éléments communs à tous les types de Question
    /// </summary>
    public abstract class Question
    {
        private String title;
        private String questionText;
        private String generalFeedback;
        private double defaultGrade;
        private double penalty;

        /// <summary>
        /// Indique le type de la Question
        /// </summary>
        public abstract QuestionType Type
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public Question()
        {
            this.Title = "";
            this.QuestionText = "";
            this.GeneralFeedback = "";
            Answers = new List<Answer>();
            this.DefaultGrade = -1;
            this.Penalty = -1;
        }

        public Question(String qName, String qText, String qFeedback) : this()
        {
            this.Title = qName;
            this.QuestionText = qText;
            this.GeneralFeedback = qFeedback;
        }

        /// <summary>
        /// Constructeur de Question
        /// </summary>
        /// <param name="qName">Le texte du Titre de la Question</param>
        /// <param name="qText">Le texte de la Question</param>
        public Question(String qName, String qText): this( qName, qText, "" )
        {
        }


        public string Title { get => title; set => title = value; }
        public string Name { get => Title; set => Title = value; }


        public string QuestionText
        {
            get
            {
                return questionText;
            }

            set
            {
                questionText = value;
            }
        }
        public string GeneralFeedback { get => generalFeedback; set => generalFeedback = value; }

        /// <summary>
        /// Liste des Réponses possibles 
        /// </summary>
        public List<Answer> Answers { get => answers; set => answers = value; }
        public double Penalty { get => penalty; set => penalty = value; }
        public double DefaultGrade { get => defaultGrade; set => defaultGrade = value; }

        List<Answer> answers;

        /// <summary>
        /// Ajout d'une réponse de type ShortAnswer
        /// </summary>
        /// <param name="answer">Un objet de type ShortAnswer</param>
        public virtual void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        /// <summary>
        /// Ajout d'une réponse en précisant ses caractéristiques
        /// </summary>
        /// <param name="answer">Réponse</param>
        /// <param name="feedback">FeedBack</param>
        /// <param name="fraction">% de valeur</param>
        public virtual void AddAnswer(String answer, String feedback, int fraction)
        {
            AddAnswer(new Answer(answer, feedback, fraction));
        }

        public abstract XmlNode MoodleXML
        {
            get;
            set;
        }
    }
}
