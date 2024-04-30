using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils.BTreeIndex.Collections.Generics
{
    public class PartialComparer<TKey, TPartialKey> : IComparer<TKey> where TKey : struct, IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey> 
    {
        private int sizeOfKey;
        private Comparer<TKey> defaultComparer;
        private TKey mask;
        private int sizeOfPartialKey;

        public PartialComparer()
        {
            sizeOfKey = Marshal.SizeOf(typeof(TKey));
            sizeOfPartialKey = Marshal.SizeOf(typeof(TPartialKey));
            defaultComparer = System.Collections.Generic.Comparer<TKey>.Default;
            mask = (TKey) typeof(TKey).OperatorAssignFrom(((uint)Math.Pow(2, (sizeOfPartialKey * 8))) - 1);

            if (sizeOfPartialKey > sizeOfKey)
            {
                e.Throw<Exception>("Cannot search with a type larger than key type");
            }
        }

        public int Compare(TKey x, TKey y)
        {
            var maskedKey = (TKey) x.BitwiseAnd(mask);

            if (defaultComparer.Compare(maskedKey, y) == 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
