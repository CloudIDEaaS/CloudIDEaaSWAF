using BTreeIndex.Collections.Generic.BTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Utils
{
    public class ObjectTree<T> : ObjectTreeItem<T> where T : class
    {
        private BTreeDictionary<T, ObjectTreeItem<T>> objectToObjectItems;

        public ObjectTree() : base(default(T))
        {
            objectToObjectItems = new BTreeDictionary<T, ObjectTreeItem<T>>();
        }

        public ObjectTree(IComparer<T> comparer) : base(default(T))
        {
            objectToObjectItems = new BTreeDictionary<T, ObjectTreeItem<T>>(comparer);
        }

        internal void ItemsAdded(IList newItems)
        {
            foreach (var item in newItems.Cast<ObjectTreeItem<T>>())
            {
                objectToObjectItems.Add(item.InternalObject, item);
            }
        }

        internal void ItemsRemoved(IList newItems)
        {
            foreach (var item in newItems.Cast<ObjectTreeItem<T>>())
            {
                objectToObjectItems.Remove(item.InternalObject);
            }
        }

        public ObjectTreeItem<T> FindTreeItem(T item)
        {
            return objectToObjectItems[item];
        }
    }
}
