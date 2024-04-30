using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ChildrenEnumerator : ItemEnumerator
    {
        public IImageLayoutItem ExistingItemParent { get; private set; }

        public ChildrenEnumerator(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> childrenIterator) : base(childrenIterator)
        {
            this.ExistingItemParent = existingItemParent;
        }
    }
}
