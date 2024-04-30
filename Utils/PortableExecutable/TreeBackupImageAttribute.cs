using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils.PortableExecutable
{
    public class TreeBackupImageAttribute : Attribute
    {
        public string[] ChildImages { get; private set; }
        public string Image { get; private set; }

        public TreeBackupImageAttribute(string image)
        {
            this.Image = image;
        }

        public TreeBackupImageAttribute(string image, params string[] childImages) : this(image)
        {
            this.ChildImages = childImages;
        }
    }
}
