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

        private int Buffer { get; set; }

        private List<string> SingleSortedFiles { get; set; }

        public BigSortFile(BigSortFileOptions options)
        {
            this.InFile = options.InFile;
            this.OutFile = options.OutFile;
            this.StringCompare = options.Cs;
            this.Encoding = options.Encoding;
            this.Splitter = new FileSplitter(this.InFile, options.TempFolderPath, options.Buffer);
            this.Splitter.CleanupFiles = options.CleanupFiles;
            this.CleanupFiles = options.CleanupFiles;
            this.Buffer = options.Buffer;
        }

        public void Sort()
        {

            if (new FileInfo(InFile).Length < this.Buffer)
            {
                var ssf = new SingleSortFile(InFile, OutFile, StringCompare, Encoding);
                ssf.SortFile();
            }
            else
            {
                // Split the Files
                this.Splitter.SplitFiles();
                SingleSortedFiles = new List<string>();

                // Sort Each File
                foreach (var file in this.Splitter.OutFiles)
                {
                    var t = file + ".OUT";
                    SingleSortedFiles.Add(t);
                    var ssf = new SingleSortFile(file, t, StringCompare, Encoding);
                    ssf.SortFile();
                }

                // Merge Them Back
                var fm = new FileMerger(SingleSortedFiles, OutFile, StringCompare);
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
