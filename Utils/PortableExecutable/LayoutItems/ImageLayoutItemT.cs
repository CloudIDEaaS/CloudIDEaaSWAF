using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    public class ImageLayoutItem<T> : IImageLayoutItemGenericContainer
    {
        public T Item { get; private set; }
        public ulong Offset { get; private set; }
        public ulong Size { get; private set; }
        public string Name { get; private set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public ImageLayoutItem(T item, string name, ulong offset, ulong size)
        {
            this.Item = item;
            this.Offset = offset;
            this.Size = size;
            this.Name = name;
            this.UniqueId = Guid.NewGuid();
        }

        public virtual IEnumerable<ImageLayoutItem> Children
        {
            get
            {
                return null;
            }
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

        IImageLayoutItem IImageLayoutItemContainer.Item
        {
            get 
            {
                return this;
            }
        }

        object IImageLayoutItemGenericContainer.Item
        {
            get 
            {
                return this.Item;
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
