using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ImageLayoutReference<T> : IImageLayoutGenericReference
    {
        public ImageLayoutItem Primary { get; set; }
        public ImageLayoutItem<T> Referenced { get; set; }
        public string ReferencingProperty { get; set; }
        public string ReferenceName { get; set; }

        public ImageLayoutReference(ImageLayoutItem<T> referenced, ImageLayoutItem primary, string referencingProperty)
        {
            this.Primary = primary;
            this.Referenced = referenced;
            this.ReferencingProperty = referencingProperty;
        }

        public ImageLayoutReference(ImageLayoutItem<T> referenced, ImageLayoutItem primary, string referencingProperty, string referenceName)
        {
            this.Primary = primary;
            this.Referenced = referenced;
            this.ReferencingProperty = referencingProperty;
            this.ReferenceName = referenceName;
        }

        IImageLayoutItemContainer IImageLayoutReference.Primary
        {
            get 
            {
                return this.Primary;
            }
        }

        IImageLayoutItemContainer IImageLayoutReference.Referenced
        {
            get 
            {
                return this.Referenced;
            }
        }
    }
}
