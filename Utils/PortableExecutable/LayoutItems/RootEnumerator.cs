using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class RootEnumerator : ItemEnumerator
    {
        public RootEnumerator(Action<int, IImageLayoutItem> rootIterator) : base(rootIterator)
        {
        }
    }
}
