using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoodleQuiz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz.Tests
{
    [TestClass()]
    public class AnswerTests
    {

        [TestMethod( "Answer: Réponse courte")]
        public void ShortAnswerTest()
        {
            Answer shortAnswer = new Answer("Réponse", "Feedback", 100);
            //
            Assert.AreEqual("Réponse", shortAnswer.Text);
            Assert.AreEqual("Feedback", shortAnswer.Feedback);
            Assert.AreEqual(100, shortAnswer.Fraction);
            Trace.WriteLine(shortAnswer.MoodleXML.OuterXml);
        }

        [TestMethod( "Answer: From XML")]
        public void AnswerFromXmlTest()
        {
            String xml = "    <answer fraction='0' format='moodle_auto_format'>" +
                                "      <text>true</text>" +
                                "      <feedback format='html'>" +
                                "        <text><![CDATA[feedback]]></text>" +
                                "      </feedback>" +
                                "    </answer>";
            //
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            //
            XmlElement root = doc.DocumentElement;
            //
            Answer ans = new Answer();
            ans.MoodleXML = (XmlNode)root;
            //
            Assert.AreEqual(0, ans.Fraction);
            Assert.AreEqual("true", ans.Text);
            Assert.AreEqual("feedback", ans.Feedback);
        }

    }
}