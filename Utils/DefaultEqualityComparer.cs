using System;
using System.Collections.Generic;

namespace Utils
{
    public class DefaultEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return ((object)x) == ((object)y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}