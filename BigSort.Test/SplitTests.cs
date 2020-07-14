using System;
using System.IO;
using BigSort.Split;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigSort.Test
{
    [TestClass]
    public class SplitTests
    {
        [TestMethod]
        [DataRow(10, 1000000)]
        [DataRow(4, 2500000)]
        [DataRow(2, 5000000)]
        [DataRow(1, 10000000)]
        public void FileSplitterTest(int expectedCount, int splitThreshold)
        {

            var inFile = "INFILE.TXT";
            using (StreamWriter sw = new StreamWriter(inFile))
            {
                for (int i = 0; i <= 2000000; i++)
                {
                    sw.WriteLine("123");
                }
            }

            using (var splitter = new FileSplitter(inFile, ".", splitThreshold))
            {
                splitter.SplitFiles();
                Assert.AreEqual(expectedCount, splitter.OutFiles.Count);
            }

        }
    }
}
