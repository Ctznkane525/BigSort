using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigSort.Compare;
using System.IO;
using BigSort.Sorter;

namespace BigSort.Test
{
    [TestClass]
    public class CompareTests
    {
        [TestMethod]
        public void CompareSubString()
        {
            Assert.AreEqual(new CompareSubString(1, 5).ParseString("HELLO WORLD"), "HELLO");
            Assert.AreEqual(new CompareSubString(7, 5).ParseString("HELLO WORLD"), "WORLD");
            Assert.AreEqual(new CompareSubString(8, 5).ParseString("HELLO WORLD"), "ORLD" + Convert.ToChar(0));
            Assert.AreEqual(new CompareSubString(32, 5).ParseString("HELLO WORLD"), new string(Convert.ToChar(0), 5));
        }

        [TestMethod]
        public void CompareString()
        {
            var css1 = new CompareSubString(1, 5);
            var css2 = new CompareSubString(7, 5);
            Assert.AreEqual(new CompareString(new CompareSubString[]{ css1, css2 }).GetComparerString("HELLO WORLD"), "HELLOWORLD");
        }


        
    }
}
