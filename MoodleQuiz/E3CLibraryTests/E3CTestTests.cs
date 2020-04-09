using Microsoft.VisualStudio.TestTools.UnitTesting;
using E3CLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary.Tests
{
    [TestClass()]
    public class E3CTestTests
    {
        [TestMethod( "E3CTest : Load")]
        public void LoadTest()
        {
            E3CTest test = new E3CTest();
            test.Load("FichierE3C.txt");
            //
            Assert.AreEqual(2, test.Themes.Count);
        }
    }
}