using Microsoft.VisualStudio.TestTools.UnitTesting;
using E3CLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary.Tests
{
    [TestClass()]
    public class CorrectionTests
    {
        [TestMethod( "Correction : Constructeur")]
        public void CorrectionTest()
        {
            // Correction : A
            Correction corr = new Correction();
            Assert.AreEqual("", corr.Choix);
            //
            corr = new Correction("Correction");
            Assert.AreEqual("", corr.Choix);
            //
            corr = new Correction("Correction :");
            Assert.AreEqual("", corr.Choix);
            //
            corr = new Correction("Correction : A");
            Assert.AreEqual("A", corr.Choix);
        }
    }
}