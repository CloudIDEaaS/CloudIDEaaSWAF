using System;
using System.Collections.Generic;
using System.Text;
using Utils.Hierarchies;
using System.Linq;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo }")]
    public class ObjectTreeItem<T> where T : class
    {
        public ObjectTreeItem<T> Parent { get; set; }
        public LinkedListNode<ObjectTreeItem<T>> LinkedListNode { get; private set; }
        public T InternalObject { get; }
        public BaseList<ObjectTreeItem<T>> Children { get; }
        public LinkedList<ObjectTreeItem<T>> ChildrenLinkedList { get; }

        public ObjectTreeItem(T obj)
        {
            if (obj != null)
            {
                this.InternalObject = obj;
            }

            this.Children = new BaseList<ObjectTreeItem<T>>();
            this.ChildrenLinkedList = new LinkedList<ObjectTreeItem<T>>();

            ((INotifyCollectionChanged) this.Children).CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:

                        foreach (var item in e.NewItems.Cast<ObjectTreeItem<T>>())
                        {
                            item.Parent = this;

                            item.LinkedListNode = this.ChildrenLinkedList.AddLast(item);
                        }

                        this.Root.ItemsAdded(e.NewItems);

                        break;

                    case NotifyCollectionChangedAction.Remove:

                        foreach (var item in e.OldItems.Cast<ObjectTreeItem<T>>())
                        {
                            item.Parent = this;

                            this.ChildrenLinkedList.Remove(item);
                        }

                        this.Root.ItemsRemoved(e.OldItems);

                        break;

                    default:
                        DebugUtils.Break();
                        break;
                }
            };
        }

        public ObjectTreeItem(T parentObject, T obj) : this(obj)
        {
            this.Parent = new ObjectTreeItem<T>(parentObject);

            this.Parent.AddChild(obj);
        }

        public ObjectTree<T> Root
        {
            get
            {
                return (ObjectTree<T>) this.GetAncestorsAndSelf((i) => i.Parent).Last();
            }
        }

        public IEnumerable<ObjectTreeItem<T>> GetDescendants()
        {
            var list = new List<ObjectTreeItem<T>>();

            this.GetDescendants((i) => i.Children.AsEnumerable(), (c) =>
            {
                list.Add(c);
            });

            return list;
        }

        public ObjectTreeItem<T> AddChild(T childObject)
        {
            var child = new ObjectTreeItem<T>(childObject);

            this.Children.Add(child);

            return child;
        }

        public void AddChild(ObjectTreeItem<T> childItem)
        {
            this.Children.Add(childItem);
        }

        public IEnumerable<ObjectTreeItem<T>> GetAncestors()
        {
            return this.GetAncestors((i) => i.Parent);
        }

        public string DebugInfo
        {
            get
            {
                if (this.InternalObject != null)
                {
                    return this.InternalObject.GetDebuggerDisplay();
                }
                else if (this is ObjectTree<T>)
                {
                    return "/";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
