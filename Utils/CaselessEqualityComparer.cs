using System;
using System.Collections.Generic;

namespace Utils
{
    public class CaselessEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.AsCaseless() == y;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}