using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public abstract class ItemEnumerator
    {
        public Action<int, IImageLayoutItem> ItemIterator { get; private set; }

        public ItemEnumerator(Action<int, IImageLayoutItem> itemIterator)
        {
            this.ItemIterator = itemIterator;
        }
    }
}
