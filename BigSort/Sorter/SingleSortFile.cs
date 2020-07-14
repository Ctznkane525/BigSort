using BigSort.Compare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BigSort.Sorter
{
    public class SingleSortFile
    {

        public string InFile { get; private set; }
        public string OutFile { get; private set; }

        public CompareString StringCompare { get; private set; }

        private Encoding Encoding { get; }
        
        public SingleSortFile(string inFile, string outFile, CompareString cs,
            Encoding encoding)
        {
            this.InFile =inFile;
            this.OutFile = outFile;
            this.StringCompare = cs;
            this.Encoding = encoding;
        }

        public void SortFile()
        {

            // Read File
            List<string> lines = ReadFile();
            lines.Sort(StringCompare);
            WriteFile(lines);          
        }

        private List<string> ReadFile()
        {
            List<string> lines = new List<string>();
            using (var reader = new System.IO.StreamReader(InFile, this.Encoding))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }

            return lines;
        }

        private void WriteFile(List<string> lines)
        {
            using (var writer = new System.IO.StreamWriter(OutFile, false, this.Encoding))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }
    }
}
