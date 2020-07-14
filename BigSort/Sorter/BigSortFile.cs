using BigSort.Compare;
using BigSort.Merger;
using BigSort.Split;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BigSort.Sorter
{
    public class BigSortFile : IDisposable
    {
        private string InFile { get; set; }

        public bool CleanupFiles { get; set; } = true;

        private string OutFile { get; set; }

        private CompareString StringCompare { get; set; }
        public Encoding Encoding { get; }

        private FileSplitter Splitter { get; set; }

        private FileMerger Merger { get; set; }

        private long BufferSize { get; set; }

        private List<string> SingleSortedFiles { get; set; }

        private int EstimatedRecordLength { get; set; }

        public BigSortFile(BigSortFileOptions options)
        {
            this.InFile = options.InFile;
            this.OutFile = options.OutFile;
            this.StringCompare = options.Cs;
            this.Encoding = options.Encoding;
            this.Splitter = new FileSplitter(this.InFile, options.TempFolderPath, options.BufferSize);
            this.Splitter.CleanupFiles = options.CleanupFiles;
            this.CleanupFiles = options.CleanupFiles;
            this.BufferSize = options.BufferSize;
            this.EstimatedRecordLength = options.EstimatedRecordLength;
        }

        public void Sort()
        {

            SingleSortedFiles = new List<string>();


            if (new FileInfo(InFile).Length < this.BufferSize)
            {
                var ssf = new SingleSortFile(InFile, OutFile, StringCompare, Encoding);
                ssf.SortFile();
            }
            else
            {
                // Split the Files
                this.Splitter.SplitFiles();
                
                // Sort Each File
                foreach (var file in this.Splitter.OutFiles)
                {
                    var t = file + ".OUT";
                    SingleSortedFiles.Add(t);
                    var ssf = new SingleSortFile(file, t, StringCompare, Encoding);
                    ssf.SortFile();
                }

                // Merge Them Back
                var fm = new FileMerger(new MergeFileOptions(SingleSortedFiles, OutFile, StringCompare) { BufferSize = this.BufferSize, EstimatedRecordLength = this.EstimatedRecordLength });
                fm.MergeFiles();
            }
        }

        public void Dispose()
        {
            if ( this.Splitter != null)
            {

                this.Splitter.Dispose();

                if (CleanupFiles)
                {
                    foreach (var fn in this.SingleSortedFiles)
                    {
                        System.IO.File.Delete(fn);
                    }
                }

            }
        }
    }
}
