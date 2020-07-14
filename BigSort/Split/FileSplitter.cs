using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigSort.Split
{
    public class FileSplitter : IDisposable
    {
        private string InFile { get; set; }

        private string TempFolder { get; set; }

        public List<string> OutFiles { get; private set; }

        private int BufferSize { get; set; }

        public bool CleanupFiles { get; set; } = true;

        public FileSplitter(string inFile, string tempFolder) : this(inFile, tempFolder, 16 * 1024 * 1024)
        {
        }

        public FileSplitter(string inFile, string tempFolder, int bufferSize)
        {
            this.InFile = inFile;
            this.TempFolder = tempFolder;
            this.BufferSize = bufferSize;
            this.OutFiles = new List<string>();
        }

        public void SplitFiles()
        {
            StreamWriter sw = null;
            try
            {
                int counter = 0;
                sw = new StreamWriter(AddNewFileName());
                using (StreamReader sr = new StreamReader(InFile))
                {
                    while (sr.Peek() >= 0)
                    {
                        // Copy a line
                        sw.WriteLine(sr.ReadLine());
                        counter += 1;

                        if (counter == 50000)
                        {
                            sw.Flush();
                            counter += 1;
                        }
                        
                        // If the file is big, then make a new split,
                        // however if this was the last line then don't bother
                        if (sw.BaseStream.Length > this.BufferSize && sr.Peek() >= 0)
                        {
                            sw.Close();
                            sw = new StreamWriter(AddNewFileName());
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                throw;
            }
            finally
            {
                if ( sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private string AddNewFileName()
        {
            var newfn = System.IO.Path.Combine(TempFolder, System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()));
            OutFiles.Add(newfn);
            return newfn;
        }

        public void Dispose()
        {
            CleanupTempFiles();
        }

        private void CleanupTempFiles()
        {
            if (CleanupFiles)
            {
                foreach (var fn in OutFiles)
                {
                    System.IO.File.Delete(fn);
                }
            }         
        }
    }
}
