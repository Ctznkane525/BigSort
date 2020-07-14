using BigSort.Compare;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigSort.Sorter
{
    public class BigSortFileOptions
    {
        public BigSortFileOptions(string inFile, string outFile, CompareString cs, Encoding encoding)
        {
            this.InFile = inFile;
            this.OutFile = outFile;
            this.Cs = cs;
            this.Encoding = encoding;
            this.TempFolderPath = System.IO.Path.GetTempPath();
            this.BufferSize = 256L * 1024L * 1024L;
        }

        public string TempFolderPath { get; set; }

        public long BufferSize { get; set; }

        public int EstimatedRecordLength { get; set; } = 100;

        public bool CleanupFiles { get; set; } = true;

        public string InFile { get; }
        public string OutFile { get; }
        public CompareString Cs { get; }
        public Encoding Encoding { get; }
    }
}
