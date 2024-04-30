using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ImageLayoutRelationship<T> : IImageLayoutRelationship
    {
        public ImageLayoutItem Parent { get; set; }
        public ImageLayoutItem<T> Child { get; set; }
        public LayoutFlags Flags { get; set; }

        public ImageLayoutRelationship(ImageLayoutItem<T> child, ImageLayoutItem parent)
        {
            this.Parent = parent;
            this.Child = child;
        }

        public ImageLayoutRelationship(ImageLayoutItem<T> child, ImageLayoutItem parent, LayoutFlags flags)
        {
            this.Parent = parent;
            this.Child = child;
            this.Flags = flags;
        }

        IImageLayoutItemContainer IImageLayoutRelationship.Parent
        {
            get 
            {
                return this.Parent;
            }
        }

        IImageLayoutItemContainer IImageLayoutRelationship.Child
        {
            get 
            {
                return this.Child;
            }
        }
    }
}
