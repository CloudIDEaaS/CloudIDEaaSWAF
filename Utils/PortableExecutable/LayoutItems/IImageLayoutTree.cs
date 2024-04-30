using System;
using Utils;
using Utils.PortableExecutable;
using System.Collections.Generic;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutTree
    {
        void AddImage(string imageFileName);
        void Clear();
        void FindExistingItem(object sender, EventArgs<ImageLayoutItemFind> e);
        void OnRelationshipAdded(object sender, EventArgs<IImageLayoutRelationship> e);
        IEnumerable<ImageLayoutItem> Roots { get; }
    }
}
