using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ReferenceEnumerator : ItemEnumerator
    {
        public IImageLayoutItem ExistingItemPrimary { get; private set; }
        public Action<int, ReferencedItem> ReferenceIterator { get; private set; }

        public ReferenceEnumerator(IImageLayoutItem existingItemPrimary, Action<int, ReferencedItem> referenceIterator) : base(referenceIterator.ToItemIterator())
        {
            this.ExistingItemPrimary = existingItemPrimary;
            this.ReferenceIterator = referenceIterator;
        }
    }
}
