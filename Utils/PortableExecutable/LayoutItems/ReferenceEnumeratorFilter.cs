using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ReferenceEnumeratorFilter : ReferenceEnumerator
    {
        public Func<ReferencedItem, bool> ReferenceFilter { get; set; }

        public ReferenceEnumeratorFilter(IImageLayoutItem existingItemPrimary, Action<int, ReferencedItem> referenceIterator, Func<ReferencedItem, bool> referenceFilter) : base(existingItemPrimary, referenceIterator)
        {
            this.ReferenceFilter = referenceFilter;
        }
    }
}
