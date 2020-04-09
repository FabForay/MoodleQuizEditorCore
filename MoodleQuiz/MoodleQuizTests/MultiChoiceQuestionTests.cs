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
    public class MultiChoiceQuestionTests
    {


        [TestMethod( "MultiChoice avec réponses")]
        public void MultiChoiceNAnswers()
        {
            MultiChoiceQuestion q = new MultiChoiceQuestion( "Multi Choice", "Texte du Multi Choice");
            //
            q.AddAnswer("Answer 1", "aucune", 50);
            q.AddAnswer("Answer 2", "aucune", 50);
            q.AddAnswer("Answer 3", "aucune", 0);
            //
            Action act = () => q.AddAnswer("Answer 4", "aucune", 50);
            Assert.ThrowsException<OverflowException>( act );
            //
            Trace.WriteLine(q.MoodleXML.OuterXml);
        }

        [TestMethod( "MultiChoice: From XML")]
        public void MultiChoiceFromXMLTest()
        {
            String xml =    " <question type = 'multichoice' >"+
                            "    <name>" +
                            "      <text>Variables</text>" +
                            "    </name>" +
                            "    <questiontext format='html'>" +
                            "      <text><![CDATA[<p>Question</p>]]></text>" +
                            "    </questiontext>" +
                            "    <generalfeedback format='html'>" +
                            "      <text><![CDATA[<p>feedback</p>]]></text>" +
                            "    </generalfeedback>" +
                            "    <defaultgrade>1.0000000</defaultgrade>" +
                            "    <penalty>0.3333333</penalty>" +
                            "    <hidden>0</hidden>" +
                            "    <single>true</single>" +
                            "    <shuffleanswers>true</shuffleanswers>" +
                            "    <answernumbering>abc</answernumbering>" +
                            "    <correctfeedback format='html'>" +
                            "      <text><![CDATA[<p>not</p>]]></text>" +
                            "    </correctfeedback>" +
                            "    <partiallycorrectfeedback format='html'>" +
                            "      <text><![CDATA[<p>partially</p>]]></text>" +
                            "    </partiallycorrectfeedback>" +
                            "    <incorrectfeedback format='html'>" +
                            "      <text><![CDATA[<p>incorrect</p>]]></text>" +
                            "    </incorrectfeedback>" +
                            "    <shownumcorrect/>" +
                            "    <answer fraction='0' format='html'>" +
                            "      <text><![CDATA[<p>Oui</p>]]></text>" +
                            "      <feedback format='html'>" +
                            "        <text></text>" +
                            "      </feedback>" +
                            "    </answer>" +
                            "    <answer fraction='100' format='html'>" +
                            "      <text><![CDATA[<p>Non</p>]]></text>" +
                            "      <feedback format='html'>" +
                            "        <text></text>" +
                            "      </feedback>" +
                            "    </answer>" +
                            "    <answer fraction='0' format='html'>" +
                            "      <text><![CDATA[<p>Aucune réponse n'est correcte</p>]]></text>" +
                            "     <feedback format='html'>" +
                            "        <text></text>" +
                            "      </feedback>" +
                            "    </answer>" +
                            "    <answer fraction='0' format='html'>" +
                            "      <text><![CDATA[<p>Uniquement en utilisant 'strict'</p>]]></text>" +
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
            MultiChoiceQuestion question = new MultiChoiceQuestion();
            question.MoodleXML = (XmlNode)root;
            //
            Assert.AreEqual("Variables", question.Name);
            Assert.AreEqual("<p>Question</p>", question.QuestionText);
            Assert.AreEqual("<p>feedback</p>", question.GeneralFeedback);
            Assert.AreEqual(1, question.DefaultGrade);
            Assert.AreEqual(0.3333333, question.Penalty);
            Assert.AreEqual(true, question.Single);
            Assert.AreEqual(true, question.ShuffleAnswers);
            //
            Assert.AreEqual(4, question.Answers.Count);
            Answer ans = question.Answers[0];
            Assert.AreEqual(0, ans.Fraction);
            Assert.AreEqual("<p>Oui</p>", ans.Text);
            Assert.AreEqual(true, String.IsNullOrEmpty(ans.Feedback));
            //
            ans = question.Answers[1];
            Assert.AreEqual(100, ans.Fraction);
            Assert.AreEqual("<p>Non</p>", ans.Text);
            Assert.AreEqual(true, String.IsNullOrEmpty(ans.Feedback));
        }
    }
}