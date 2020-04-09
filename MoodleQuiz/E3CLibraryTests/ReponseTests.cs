using Microsoft.VisualStudio.TestTools.UnitTesting;
using E3CLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary.Tests
{
    [TestClass()]
    public class ReponseTests
    {
        [TestMethod("Reponse : Constructeur")]
        public void ReponseTest()
        {
            Reponse rep = new Reponse();
            Assert.AreEqual("", rep.Choix);
            Assert.AreEqual(0, rep.Lignes.Count);
            //
            rep = new Reponse("C	FF");
            Assert.AreEqual("C", rep.Choix);
            Assert.AreEqual(1, rep.Lignes.Count);
            Assert.AreEqual("FF", rep.Lignes[0].Trim());
            //

        }
    }
}