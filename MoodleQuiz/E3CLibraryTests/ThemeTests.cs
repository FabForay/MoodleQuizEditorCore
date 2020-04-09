using Microsoft.VisualStudio.TestTools.UnitTesting;
using E3CLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary.Tests
{
    [TestClass()]
    public class ThemeTests
    {
        [TestMethod( "Theme : Constructeur")]
        public void ThemeTest()
        {
            Theme th = new Theme();
            Assert.AreEqual( "", th.Titre );
            Assert.AreEqual("A", th.Categorie);
            //
            th = new Theme("Thème A : types de base");
            Assert.AreEqual("types de base", th.Titre);
            Assert.AreEqual("A", th.Categorie);
            //
            th = new Theme("Thème B : types construits");
            Assert.AreEqual("types construits", th.Titre);
            Assert.AreEqual("B", th.Categorie);
            //
        }
    }
}