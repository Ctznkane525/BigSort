# BigSort
This is intended to support the sorting of large fixed width files based on a given key, which is a set of positions and lengths in the file.

The key is a formatted string via a CompareString class.  It's constructor would be something like "1:5,8:4".  This would mean the sort key would be 2 parts.  The first part would be starting at position 0 with a length of 5.  The second part would be starting at position 7 with a length of 4.  Note that the text constructor key positions start with array position 1.

A couple examples:

In this one, we have a sorting key where we're using the first 5 characters of each line.  The encoding is the default encoding.  Our buffer size is 256 MB, and our temp path is the current directory.

  ```csharp
  using (BigSortFile bsf = new BigSortFile(new BigSortFileOptions(inFile, outFile,
      new Compare.CompareString("1:5"), System.Text.Encoding.Default)
  {
      TempFolderPath = ".",
      Buffer = 256 * 1024 * 1024

  }))
  {
      bsf.Sort();
  }
  ```

In this second example, the sorting key starts at line position 1 with a length of 5, the buffer is 128 MB and the temp path that is used is the computer's temp directory.
  
  ```csharp
  using (BigSortFile bsf = new BigSortFile(new BigSortFileOptions(inFile, outFile,
      new Compare.CompareString("2:5"), System.Text.Encoding.Default)
  {
      TempFolderPath = System.IO.Path.GetTempPath(),
      Buffer = 128 * 1024 * 1024

  }))
  {
      bsf.Sort();
  }
  ```


