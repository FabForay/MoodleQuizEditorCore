using MoodleQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MoodleQuiz.Tests
{

    [TestClass()]
    public class ShortAnswerQuestionTests
    {


        [TestMethod( "Question à réponse courte")]
        public void ShortAnswerQuestionTest()
        {
            ShortAnswerQuestion q = new ShortAnswerQuestion("Titre", "Texte");
            //
            Assert.AreEqual(QuestionType.ShortAnswer, q.Type);
            Assert.AreEqual("Titre", q.Name);
            Assert.AreEqual("Texte", q.QuestionText);
            Assert.IsNotNull(q.Answers);
        }

        [TestMethod( "Question à réponse courte - Par défaut")]
        public void ShortAnswerQuestionDefaultTest()
        {
            ShortAnswerQuestion q = new ShortAnswerQuestion();
            //
            Assert.AreEqual(QuestionType.ShortAnswer, q.Type);
            Assert.AreEqual(true, String.IsNullOrEmpty(q.Name));
            Assert.AreEqual(true, String.IsNullOrEmpty(q.QuestionText));
            Assert.IsNotNull(q.Answers);
            Assert.AreEqual(false, q.Is100Percent);
        }

        [TestMethod( "Ajout d'une réponse courte - Texte")]
        public void ShortAnswerQuestionAddTest()
        {
            ShortAnswerQuestion q = new ShortAnswerQuestion();
            //
            q.AddAnswer("Réponse", "Feedback", 100);
            Assert.AreEqual(1, q.Answers.Count);
            //
            Answer shortAnswer = q.Answers[0];
            Assert.AreEqual("Réponse", shortAnswer.Text);
            Assert.AreEqual("Feedback", shortAnswer.Feedback);
            Assert.AreEqual(100, shortAnswer.Fraction);
        }

        [TestMethod( "Ajout de réponses courtes - Texte")]
        public void ShortAnswerQuestionAddTest2()
        {
            ShortAnswerQuestion q = new ShortAnswerQuestion();
            //
            q.AddAnswer("true", "aucune", 50);
            q.AddAnswer("true", "aucune", 50);
            Action action = () => q.AddAnswer("true", "aucune", 50);
            Assert.ThrowsException<OverflowException>(action);
            //
            Trace.WriteLine(q.MoodleXML.OuterXml);
        }

        [TestMethod( "ShortAnswer: From XML")]
        public void ShortAnswerFromXMLTest()
        {
            String xml = "  <question type='shortanswer'>" +
                            "    <name>" +
                            "      <text>Linux</text>" +
                            "    </name>" +
                            "    <questiontext format='html'>" +
                            "      <text><![CDATA[<p>Linux</p>]]></text>" +
                            "    </questiontext>" +
                            "    <generalfeedback format='html'>" +
                            "      <text>feedback</text>" +
                            "    </generalfeedback>" +
                            "    <defaultgrade>1.0000000</defaultgrade>" +
                            "    <penalty>0.3333333</penalty>" +
                            "    <hidden>0</hidden>" +
                            "    <usecase>0</usecase>" +
                            "    <answer fraction='100' format='moodle_auto_format'>" +
                            "      <text>trim</text>" +
                            "      <feedback format='html'>" +
                            "        <text></text>" +
                            "      </feedback>" +
                            "    </answer>" +
                            "  </question>";
            //
            //
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            //
            XmlElement root = doc.DocumentElement;
            //
            ShortAnswerQuestion question = new ShortAnswerQuestion();
            question.MoodleXML = (XmlNode)root;
            //
            Assert.AreEqual("Linux", question.Name);
            Assert.AreEqual("<p>Linux</p>", question.QuestionText);
            Assert.AreEqual("feedback", question.GeneralFeedback);
            Assert.AreEqual(1, question.DefaultGrade);
            Assert.AreEqual(0.3333333, question.Penalty);
            Assert.AreEqual(false, question.UseCase);
            //
            Assert.AreEqual(1, question.Answers.Count);
            Answer ans = question.Answers[0];
            Assert.AreEqual(100, ans.Fraction);
            Assert.AreEqual("trim", ans.Text);
            Assert.AreEqual(true, String.IsNullOrEmpty(ans.Feedback));
        }
    }
}