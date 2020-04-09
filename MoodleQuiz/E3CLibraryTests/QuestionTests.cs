using Microsoft.VisualStudio.TestTools.UnitTesting;
using E3CLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary.Tests
{
    [TestClass()]
    public class QuestionTests
    {
        [TestMethod("Question : Constructeur")]
        public void QuestionTest()
        {
            Question q = new Question();
            Assert.AreEqual("", q.Titre);
            Assert.AreEqual("", q.TexteQuestion);
            Assert.AreEqual(0, q.Reponses.Count);
            //
            q = new Question("Question A.2");
            Assert.AreEqual("A.2", q.Titre);
            Assert.AreEqual("", q.TexteQuestion);
            Assert.AreEqual(0, q.Reponses.Count);
            //
        }
    }
}