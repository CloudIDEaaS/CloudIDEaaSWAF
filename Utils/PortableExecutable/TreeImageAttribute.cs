using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils.PortableExecutable
{
    public class TreeImageAttribute : Attribute
    {
        public string[] ChildImages { get; private set; }
        public string Image { get; private set; }

        public TreeImageAttribute(string image)
        {
            this.Image = image;
        }

        public TreeImageAttribute(string image, params string[] childImages) : this(image)
        {
            this.ChildImages = childImages;
        }
    }
}
