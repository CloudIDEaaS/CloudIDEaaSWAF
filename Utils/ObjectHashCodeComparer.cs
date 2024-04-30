using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Utils
{
    public class ObjectHashCodeComparer<T> : IComparer<T>
    {
        public int Compare([AllowNull] T x, [AllowNull] T y)
        {
            return x.GetHashCode().CompareTo(y.GetHashCode());
        }
    }
}
