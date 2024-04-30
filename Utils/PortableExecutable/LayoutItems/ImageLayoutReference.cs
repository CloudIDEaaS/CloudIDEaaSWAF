using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ImageLayoutReference : IImageLayoutReference
    {
        public IImageLayoutItemContainer Primary { get; set; }
        public IImageLayoutItemContainer Referenced { get; set; }
        public string ReferencingProperty { get; set; }

        public ImageLayoutReference(ImageLayoutItem referenced, ImageLayoutItem primary, string referencingProperty)
        {
            this.Primary = primary;
            this.Referenced = referenced;
            this.ReferencingProperty = referencingProperty;
        }
    }
}
