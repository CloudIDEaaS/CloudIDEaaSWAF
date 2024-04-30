using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class RootEnumeratorFilter : ItemEnumerator
    {
        public Func<IImageLayoutItem, bool> RootFilter { get; set; }

        public RootEnumeratorFilter(Action<int, IImageLayoutItem> rootIterator, Func<IImageLayoutItem, bool> rootFilter) : base(rootIterator)
        {
            this.RootFilter = rootFilter;
        }
    }
}
