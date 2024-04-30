using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ImageLayoutItemFind
    {
        public IImageLayoutItem SearchItem { get; set; }
        public ImageLayoutItem FoundItem { get; set; }

        public ImageLayoutItemFind(IImageLayoutItem searchItem)
        {
            this.SearchItem = searchItem;
        }
    }
}
