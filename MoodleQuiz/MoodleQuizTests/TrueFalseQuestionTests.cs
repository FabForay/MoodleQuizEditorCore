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
    public class TrueFalseQuestionTests
    {

        [TestMethod( "TrueFalse: sans FeedBack")]
        public void TrueFalseQuestionTest()
        {
            TrueFalseQuestion q = new TrueFalseQuestion("Titre", "Texte", "FeedBack", true);
            //
            Assert.AreEqual(QuestionType.TrueFalse, q.Type);
            Assert.AreEqual("Titre", q.Name);
            Assert.AreEqual("Texte", q.QuestionText);
            Assert.AreEqual("FeedBack", q.GeneralFeedback);
            Assert.AreEqual(true, q.Correct);
            //
            Action action = () => q.AddAnswer("true", "aucune", 100);
            Assert.ThrowsException<OverflowException>(action);
            //
        }

        [TestMethod( "TrueFalse: avec FeedBack")]
        public void TrueFalseQuestionTest1()
        {
            TrueFalseQuestion q = new TrueFalseQuestion("Titre", "Texte", "FeedBack", true, "Feedback Vrai", "Feedback Faux");
            //
            Assert.AreEqual(QuestionType.TrueFalse, q.Type);
            Assert.AreEqual("Titre", q.Name);
            Assert.AreEqual("Texte", q.QuestionText);
            Assert.AreEqual("FeedBack", q.GeneralFeedback);
            Assert.AreEqual("Feedback Vrai", q.FeedbackTrue);
            Assert.AreEqual("Feedback Faux", q.FeedbackFalse);
            Assert.AreEqual(true, q.Correct);
            //
            Action action = () => q.AddAnswer("true", "aucune", 100);
            Exception exc = Assert.ThrowsException<OverflowException>(action);
            //
            Trace.WriteLine(q.MoodleXML.OuterXml);
        }

        [TestMethod( "TrueFalse: From XML")]
        public void TrueFalseFromXMLTest()
        {
            String xml = "    <question type='truefalse'>" +
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
                            "    <penalty>1.0000000</penalty>" +
                            "    <hidden>0</hidden>" +
                            "    <answer fraction='0' format='moodle_auto_format'>" +
                            "      <text>true</text>" +
                            "      <feedback format='html'>" +
                            "        <text></text>" +
                            "      </feedback>" +
                            "    </answer>" +
                            "    <answer fraction='100' format='moodle_auto_format'>" +
                            "      <text>false</text>" +
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
            TrueFalseQuestion question = new TrueFalseQuestion();
            question.MoodleXML = (XmlNode)root;
            //
            Assert.AreEqual("Linux", question.Name);
            Assert.AreEqual("<p>Linux</p>", question.QuestionText);
            Assert.AreEqual("feedback", question.GeneralFeedback);
            Assert.AreEqual(1, question.DefaultGrade);
            Assert.AreEqual(1, question.Penalty);
            Assert.AreEqual(false, question.Correct);
            //
        }
    }
}