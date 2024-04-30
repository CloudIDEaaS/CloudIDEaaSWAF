using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ChildrenEnumeratorFilter : ChildrenEnumerator
    {
        public Func<IImageLayoutItem, bool> ChildrenFilter { get; set; }

        public ChildrenEnumeratorFilter(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> childrenIterator, Func<IImageLayoutItem, bool> childrenFilter) : base(existingItemParent, childrenIterator)
        {
            this.ChildrenFilter = childrenFilter;
        }
    }
}
