using BigSort.Compare;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigSort.Merger
{
    public class MergeFileOptions
    {
        public MergeFileOptions(List<string> inFiles, string outFile, CompareString Cs)
        {
            this.InFiles = inFiles;
            this.Cs = Cs;
            this.OutFile = outFile;
        }

        public long BufferSize { get; set; } = 256L * 1024L * 1024L;

        public int EstimatedRecordLength { get; set; } = 100;

        public List<string> InFiles { get; }
        public CompareString Cs { get; }
        public string OutFile { get; }
    }
}
