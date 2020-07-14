using System;
using System.IO;
using System.Linq;
using BigSort.Sorter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigSort.Test
{
    [TestClass]
    public class SorterTests
    {
        [TestMethod]
        [DataRow(1, 5, "HELLO", "WORLD")]
        [DataRow(5, 1, "WORLD", "HELLO")]
        public void SingleSortFile(int pos, int len, string exp1, string exp2)
        {
            // Create INFILE
            var inFile = "INFILE.TXT";
            var outFile = "OUTFILE.TXT";
            using (StreamWriter writer = new System.IO.StreamWriter(inFile))
            {
                writer.WriteLine("HELLO");
                writer.WriteLine("WORLD");
                writer.Flush();
                writer.Close();
            }

            // Sort File
            SingleSortFile ssf = new SingleSortFile(inFile, outFile,
                new Compare.CompareString(
                       new Compare.CompareSubString[] { new Compare.CompareSubString(pos, len) }),
                System.Text.Encoding.Default
            );
            ssf.SortFile();

            // Read File
            using (StreamReader reader = new System.IO.StreamReader(outFile))
            {
                var line1 = reader.ReadLine();
                var line2 = reader.ReadLine();
                Assert.AreEqual(line1, exp1);
                Assert.AreEqual(line2, exp2);
            }
        }

        [TestMethod]
        [DataRow("1:5", "HELLO", "WORLD")]
        [DataRow("5:1", "WORLD", "HELLO")]
        public void SortFileSmall(string sortKey, string expFirst, string expSecond)
        {
            var inFile = "INFILE.TXT";
            var outFile = "OUTFILE.TXT";
            using (StreamWriter writer = new StreamWriter(inFile))
            {
                writer.WriteLine("WORLD");
                writer.WriteLine("HELLO");
            }
            System.IO.File.Delete(outFile);

            using (BigSortFile bsf = new BigSortFile(new BigSortFileOptions(inFile, outFile,
                new Compare.CompareString(sortKey), System.Text.Encoding.Default)))
            {
                bsf.Sort();
            }

            using (StreamReader reader = new StreamReader(outFile))
            {
                Assert.AreEqual(expFirst, reader.ReadLine());
                Assert.AreEqual(expSecond, reader.ReadLine());
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
        [TestMethod]
        [DataRow("1:100", 100, 10000000, 2048)]
        public void SortFileBig(string sortKey, int textLength, int numRow, int bufferSizeMb)
        {
            var inFile = "INFILE.TXT";
            var outFile = "OUTFILE.TXT";
            using (StreamWriter writer = new StreamWriter(inFile))
            {
                for (int i = 0; i < numRow; i++)
                {
                    writer.WriteLine(RandomString(textLength));
                }
            }
            System.IO.File.Delete(outFile);

            using (BigSortFile bsf = new BigSortFile(new BigSortFileOptions(inFile, outFile,
                new Compare.CompareString(sortKey), System.Text.Encoding.Default)
            {
                TempFolderPath = ".",
                Buffer = bufferSizeMb * 1024 * 1024,
                CleanupFiles = false
                
            }))
            {
                bsf.Sort();
            }

            using (StreamReader reader = new StreamReader(outFile))
            {

                var prevLine = "";
                var line = reader.ReadLine();
                while (line != null)
                {
                    prevLine = line;
                    Assert.IsTrue(prevLine.CompareTo(line) <= 0);
                    line = reader.ReadLine();
                }
            }
        }
    }
}
