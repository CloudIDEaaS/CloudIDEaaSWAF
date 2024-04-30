using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public static class LayoutExtensions
    {
        public static Action<int, IImageLayoutItem> ToItemIterator(this Action<int, ReferencedItem> referenceIterator)
        {
            return null;
        }

        public static string PrintDescendants(this IImageLayoutItem existingItemParent)
        {
            return ImageLayoutEvents.PrintDescendants(existingItemParent);
        }

        public static string PrintDescendantsIncludingReferences(this IImageLayoutItem existingItemParent)
        {
            return ImageLayoutEvents.PrintDescendantsIncludingReferences(existingItemParent);
        }

        public static IImageLayoutItem GetRealItem(this IImageLayoutItem item)
        {
            if (item is ImageLayoutItem)
            {
                var realItem = (ImageLayoutItem)item;

                return realItem.Item;
            }
            else
            {
                return item;
            }
        }
    }
}
