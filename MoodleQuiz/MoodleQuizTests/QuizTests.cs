using MoodleQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MoodleQuiz.Tests
{
    [TestClass()]
    public class QuizTests
    {

        [TestMethod( "Quiz: Ajout d'une question TrueFalse")]
        public void AddQuestionTrueFalseTest()
        {
            Quiz quiz = new Quiz();
            TrueFalseQuestion question = new TrueFalseQuestion("Titre", "Texte", "FeedBack", true);
            //
            quiz.AddQuestion(question);
            //
            Assert.AreEqual(1, quiz.Questions.Count);
            //
            Trace.WriteLine(quiz.MoodleXML.OuterXml);
        }

        [TestMethod( "Quiz: Init à partir de XML")]
        public void QuizTest()
        {
            String quizText = "<quiz>" +
                                "<!-- question: 0  -->" +
                                "  <question type='category'>" +
                                "    <category>" +
                                "        <text>$course$/top/Categorie</text>" +
                                "    </category>" +
                                "  </question>" +
                                "    <question type='truefalse'>" +
                                "    <name>" +
                                "      <text>Linux</text>" +
                                "    </name>" +
                                "    <questiontext format='html'>" +
                                "      <text><![CDATA[<p>Linux s'appelle ainsi parce que la mascotte est un pingoin</p>]]></text>" +
                                "    </questiontext>" +
                                "    <generalfeedback format='html'>" +
                                "      <text></text>" +
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
                                "  </question>" +
                                "</quiz>";
            Quiz quiz = new Quiz(quizText);
            //
            Assert.AreEqual("Categorie", quiz.DefaultCategory.Text);
            //
        }

        [TestMethod( "Quiz: Load from XMLFile")]
        public void LoadTest()
        {
            Quiz quiz = new Quiz();
            quiz.Load("quizTest.xml");
            //
            Trace.WriteLine(quiz.MoodleXML.OuterXml);
        }
    }
}