using System;

namespace BigSort.Compare
{
    public class CompareSubString
    {

        public CompareSubString(int startPosition, int length)
        {
            this.StartPosition = startPosition;
            this.Length = length;
        }

        public int StartPosition {get; }
        public int Length { get; }

        public string ParseString(string s)
        {
            if (s.Length >= StartPosition + Length)
            {
                return s.Substring(StartPosition - 1, Length);
            }
            else if (s.Length < StartPosition)
            {
                return new string(Convert.ToChar(0), Length);
            }
            else
            {
                return s.PadRight(StartPosition + Length, Convert.ToChar(0)).Substring(StartPosition - 1, Length);
            }
        }


    }
}
