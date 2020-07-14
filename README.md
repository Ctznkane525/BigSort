# BigSort
This is intended to support the sorting of large fixed width files based on a given key, which is a set of positions and lengths in the file.

The key is a formatted string via a CompareString class.  It's constructor would be something like "1:5,8:4".  This would mean the sort key would be 2 parts.  The first part would be starting at position 0 with a length of 5.  The second part would be starting at position 7 with a length of 4.  Note that the text constructor key positions start with array position 1.


