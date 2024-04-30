using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class DescendantEnumerator : ItemEnumerator
    {
        public IImageLayoutItem ExistingItemParent { get; private set; }
        public bool IncludeReferences { get; private set; }

        public DescendantEnumerator(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> childrenIterator, bool includeReferences = false) : base(childrenIterator)
        {
            this.ExistingItemParent = existingItemParent;
            this.IncludeReferences = includeReferences;
        }
    }
}
