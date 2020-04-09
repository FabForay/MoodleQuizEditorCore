using MoodleQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoodleQuiz.Tests
{
    [TestClass()]
    public class CategoryTests
    {
        [TestMethod( "Category: From XML")]
        public void CategoryTest()
        {
            String xml =    "  <question type='category'>" +
                            "    <category>" +
                            "        <text>$course$/top/Categorie</text>" +
                            "    </category>" +
                            "  </question>";
            //
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            //
            XmlElement root = doc.DocumentElement;
            //
            Category category = new Category();
            category.MoodleXML = (XmlNode)root;
        }

        [TestMethod( "Category: Set Text")]
        public void CategoryTestTextSet()
        {
            Category category = new Category();
            //
            string text = "Défault pour Test";
            category.Text = text;
            Assert.AreEqual(text, category.Text);
            //
            string catText = "$course$/top";
            category.Text = catText;
            Assert.AreEqual( true, String.IsNullOrEmpty(category.Text));
            Assert.AreEqual(catText, category.TextXML);
            //
            category.Text = catText + "/" + text;
            Assert.AreEqual(text, category.Text);
            Assert.AreEqual(catText + "/" + text, category.TextXML);
        }


    }
}