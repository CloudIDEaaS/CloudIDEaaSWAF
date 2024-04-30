using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable;
using System.Collections;
using Utils;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    public class ImageLayoutItemTuple<TTuple> : ImageLayoutItem<TTuple>, IImageLayoutItemTupleContainer where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public TTuple TupleItem { get; private set; }
        public string[] ItemNames { get; private set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public ImageLayoutItemTuple(TTuple item, string name, ulong offset, ulong size, params string[] itemNames) : base(item, name, offset, size)
        {
            this.TupleItem = item;
            this.ItemNames = itemNames;
            this.UniqueId = Guid.NewGuid();
        }

        public virtual void AddChild(ImageLayoutItem child)
        {
        }

        public override IEnumerable<ImageLayoutItem> Children
        {
            get
            {
                return null;
            }
        }

        public IEnumerable<KeyValuePair<string, string>> TupleValues
        {
            get 
            {
                var tupleValues = new List<KeyValuePair<string, string>>();
                var values = this.TupleItem.GetTupleValues();

                for (var x = 0; x < ItemNames.Length; x++)
                {
                    string elementText;
                    var element = values.ElementAt(x);

                    if (element is uint)
                    {
                        elementText = ((uint) element).ToString("x8").Prepend("0x");
                    }
                    else if (element is ulong)
                    {
                        elementText = ((ulong)element).ToString("x16").Prepend("0x");
                    }
                    else if (element is ushort)
                    {
                        elementText = ((ushort)element).ToString("x4").Prepend("0x");
                    }
                    else if (element is byte)
                    {
                        elementText = ((byte)element).ToString("x2").Prepend("0x");
                    }
                    else
                    {
                        elementText = element.AsDisplayText();
                    }

                    var pair = new KeyValuePair<string, string>(ItemNames[x], elementText);

                    tupleValues.Add(pair);
                }

                return tupleValues;
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
