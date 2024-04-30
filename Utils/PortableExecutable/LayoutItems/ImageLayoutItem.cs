using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    public class ImageLayoutItem : IImageLayoutItemContainer
    {
        public IImageLayoutItem Item { get; protected set; }
        public ulong Offset { get; protected set; }
        public ulong Size { get; protected set; }
        public string Name { get; protected set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public ImageLayoutItem(IImageLayoutItem item, ulong offset, ulong size)
        {
            this.Item = item;
            this.Offset = offset;
            this.Size = size;
            this.Name = item.GetType().Name;

            if (item == null)
            {
                this.UniqueId = Guid.NewGuid();
            }
            else
            {
                this.UniqueId = item.UniqueId;
            }
        }

        public ImageLayoutItem(IImageLayoutItem item, string name, ulong offset, ulong size)
        {
            this.Item = item;
            this.Offset = offset;
            this.Size = size;
            this.Name = name;

            if (item == null)
            {
                this.UniqueId = Guid.NewGuid();
            }
            else
            {
                this.UniqueId = item.UniqueId;
            }
        }

        public virtual void AddChild(ImageLayoutItem child)
        {
        }

        public virtual IEnumerable<ImageLayoutItem> Children
        {
            get
            {
                return null;
            }
        }

        public virtual void AddReference(ImageLayoutItem referenced, string referencingProperty, string referenceName)
        {
        }

        public virtual void AddReference(ImageLayoutItem referenced, string referencingProperty)
        {
        }

        public virtual IEnumerable<ReferencedItem> References
        {
            get
            {
                return null;
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
