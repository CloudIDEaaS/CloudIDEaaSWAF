using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ImageLayoutRelationship : IImageLayoutRelationship
    {
        public IImageLayoutItemContainer Parent { get; set; }
        public IImageLayoutItemContainer Child { get; set; }
        public LayoutFlags Flags { get; set; }

        public ImageLayoutRelationship(ImageLayoutItem child, ImageLayoutItem parent)
        {
            this.Parent = parent;
            this.Child = child;
        }

        public ImageLayoutRelationship(ImageLayoutItem child, ImageLayoutItem parent, LayoutFlags flags)
        {
            this.Parent = parent;
            this.Child = child;
            this.Flags = flags;
        }

        public ImageLayoutRelationship(ImageLayoutItem rootItem)
        {
            this.Child = rootItem;
        }

        public ImageLayoutRelationship(ImageLayoutItem rootItem, LayoutFlags flags)
        {
            this.Child = rootItem;
            this.Flags = flags;
        }
    }
}
