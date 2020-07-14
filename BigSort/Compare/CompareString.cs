using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BigSort.Compare
{
    public class CompareString : Comparer<string>
    {

        public CompareSubString[] CompareItems { get; private set; }

        public CompareString(string compareKey)
        {
            var ci = new List<CompareSubString>();
            foreach (var item in compareKey.Split(','))
            {
                var pos = Convert.ToInt32(item.Split(':')[0]);
                var len = Convert.ToInt32(item.Split(':')[1]);
                ci.Add(new CompareSubString(pos, len));
            }
            this.CompareItems = ci.ToArray();
        }

        public CompareString(CompareSubString[] ci)
        {
            CompareItems = ci;
        }

        public override int Compare(string x, string y)
        {
            return GetComparerString(x).CompareTo(GetComparerString(y));
        }

        public string GetComparerString(string x)
        {
            StringBuilder sb = new StringBuilder("");
            foreach(CompareSubString ci in CompareItems)
            {
                sb.Append(ci.ParseString(x));
            }
            return sb.ToString();
        }
    }
}
