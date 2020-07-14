using BigSort.Compare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigSort.Merger
{
    public class FileMerger
    {
        private List<string> InFiles { get; set; }
        private CompareString Cs { get; set; }

        private string OutFile { get; set; }

        public FileMerger(List<string> inFiles, string outFile, CompareString Cs)
        {
            this.InFiles = inFiles;
            this.Cs = Cs;
            this.OutFile = outFile;
        }

        public void MergeFiles()
        {
            int chunks = this.InFiles.Count; // Number of chunks
            int recordsize = 100; // estimated record size
            int records = 10000000; // estimated total # records
            int maxusage = 500000000; // max memory usage
            int buffersize = maxusage / chunks; // bytes of each queue
            double recordoverhead = 7.5; // The overhead of using Queue<>
            int bufferlen = (int)(buffersize / recordsize /
              recordoverhead); // number of records in each queue

            // Open the files
            StreamReader[] readers = new StreamReader[this.InFiles.Count];
            try
            {
                for (int i = 0; i < chunks; i++)
                    readers[i] = new StreamReader(this.InFiles[i]);

                // Make the queues
                Queue<string>[] queues = new Queue<string>[chunks];
                for (int i = 0; i < chunks; i++)
                    queues[i] = new Queue<string>(bufferlen);

                // Load the queues
                for (int i = 0; i < chunks; i++)
                    LoadQueue(queues[i], readers[i], bufferlen);

                // Merge!
                using (StreamWriter sw = new StreamWriter(this.OutFile))
                {
                    bool done = false;
                    int lowest_index, j, progress = 0;
                    string lowest_value;
                    while (!done)
                    {

                        // Find the chunk with the lowest value
                        lowest_index = -1;
                        lowest_value = "";
                        for (j = 0; j < chunks; j++)
                        {
                            if (queues[j] != null)
                            {
                                var peakedItem = queues[j].Peek() + "";
                                if (lowest_index < 0 ||
                                    Cs.Compare(peakedItem, lowest_value) < 0)
                                {
                                    lowest_index = j;
                                    lowest_value = peakedItem;
                                }
                            }
                        }

                        // Was nothing found in any queue? We must be done then.
                        if (lowest_index == -1) { done = true; break; }

                        // Output it
                        sw.WriteLine(lowest_value);

                        // Remove from queue
                        queues[lowest_index].Dequeue();
                        // Have we emptied the queue? Top it up
                        if (queues[lowest_index].Count == 0)
                        {
                            LoadQueue(queues[lowest_index],
                              readers[lowest_index], bufferlen);
                            // Was there nothing left to read?
                            if (queues[lowest_index].Count == 0)
                            {
                                queues[lowest_index] = null;
                            }
                        }
                    }
                    sw.Close();   
                }
            }
            catch (System.Exception e)
            {
                throw;
            }
            finally
            {
                // Close and delete the files
                for (int i = 0; i < chunks; i++)
                {
                    readers[i].Close();
                }
            }
       
        }

        private void LoadQueue(Queue<string> queue,
          StreamReader file, int records)
        {
            for (int i = 0; i < records; i++)
            {
                if (file.Peek() < 0) break;
                queue.Enqueue(file.ReadLine());
            }
        }


    }
}
