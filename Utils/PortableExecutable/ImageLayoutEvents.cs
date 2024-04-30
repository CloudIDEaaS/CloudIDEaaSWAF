using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Collections;
using System.Linq.Expressions;
using System.Diagnostics;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public static class ImageLayoutEvents
    {
        public static event EventHandlerT<IImageLayoutRelationship> OnRelationshipAdded;
        public static event EventHandlerT<IImageLayoutReference> OnReferenceAdded;
        public static event EventHandlerT<ImageLayoutItemFind> FindExistingItem;
        public static event EventHandlerT<ItemEnumerator> OnEnumerate;

        private static Type Type
        {
            [DebuggerStepThrough]
            get
            {
                return typeof(ImageLayoutEvents);
            }
        }

        public static bool HasHandlers
        {
            get
            {
                var list = new List<bool>
                {
                    OnRelationshipAdded.HasHandler(),
                    OnReferenceAdded.HasHandler(),
                    FindExistingItem.HasHandler(),
                    OnEnumerate.HasHandler()
                };

                return list.AnyAreTrue();
            }
        }

        public static void AddRelationship(IImageLayoutItem itemChild, string childName, ulong offsetChild, ulong childSize, IImageLayoutItem imageParent, string parentName, ulong offsetParent, ulong parentSize)
        {
            if (HasHandlers)
            {
                var child = new ImageLayoutItem(itemChild, childName, offsetChild, childSize);
                var parent = new ImageLayoutItem(imageParent, parentName, offsetParent, parentSize);
                var relationship = new ImageLayoutRelationship(child, parent);

                OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
            }
        }

        public static void AddRelationship(IImageLayoutItem itemChild, ulong offsetChild, ulong childSize, IImageLayoutItem imageParent, ulong offsetParent, ulong parentSize)
        {
            if (HasHandlers)
            {
                var child = new ImageLayoutItem(itemChild, offsetChild, childSize);
                var parent = new ImageLayoutItem(imageParent, offsetParent, parentSize);
                var relationship = new ImageLayoutRelationship(child, parent);

                OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
            }
        }

        public static void AddRelationship(IImageLayoutItem itemChild, ulong offsetChild, ulong childSize, IImageLayoutItem existingItemParent, LayoutFlags flags = LayoutFlags.AddressSizeValid)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemParent);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var child = new ImageLayoutItem(itemChild, offsetChild, childSize);
                    var parent = find.FoundItem;
                    var relationship = new ImageLayoutRelationship(child, parent, flags);

                    OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddRelationship", existingItemParent.GetType().Name, existingItemParent);
                }
            }
        }

        public static void AddRelationship<T>(T itemChild, string childName, ulong offsetChild, ulong childSize, IImageLayoutItem existingItemParent) where T : IConvertible
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemParent);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var child = new ImageLayoutItem<T>(itemChild, childName, offsetChild, childSize);
                    var parent = find.FoundItem;
                    var relationship = new ImageLayoutRelationship<T>(child, parent);

                    OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddRelationship", existingItemParent.GetType().Name, existingItemParent);
                }
            }
        }

        public static void AddRelationship<TTuple>(TTuple itemChild, string childName, ulong offsetChild, ulong childSize, IImageLayoutItem existingItemParent, params string[] itemNames) where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemParent);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var child = new ImageLayoutItemTuple<TTuple>(itemChild, childName, offsetChild, childSize, itemNames);
                    var parent = find.FoundItem;
                    var relationship = new ImageLayoutRelationship<TTuple>(child, parent);

                    OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddRelationship", existingItemParent.GetType().Name, existingItemParent);
                }
            }
        }

        public static void AddRelationship(IImageLayoutItem itemChild, string childName, ulong offsetChild, ulong childSize, IImageLayoutItem existingItemParent)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemParent);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var child = new ImageLayoutItem(itemChild, childName, offsetChild, childSize);
                    var parent = find.FoundItem;
                    var relationship = new ImageLayoutRelationship(child, parent);

                    OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddRelationship", existingItemParent.GetType().Name, existingItemParent);
                }
            }
        }

        public static void AddRelationship(IImageLayoutItem itemRootChild, string name, ulong offsetChild, ulong childSize)
        {
            if (HasHandlers)
            {
                var child = new ImageLayoutItem(itemRootChild, name, offsetChild, childSize);
                var relationship = new ImageLayoutRelationship(child);

                OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
            }
        }

        public static void AddRelationship(IImageLayoutItem itemRootChild, ulong offsetChild, ulong childSize, LayoutFlags flags = LayoutFlags.AddressSizeValid)
        {
            if (HasHandlers)
            {
                var child = new ImageLayoutItem(itemRootChild, offsetChild, childSize);
                var relationship = new ImageLayoutRelationship(child, flags);

                OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
            }
        }

        public static void AddRelationship(IImageLayoutItem itemRootChild, string name, ulong offsetChild, ulong childSize, LayoutFlags flags = LayoutFlags.AddressSizeValid)
        {
            if (HasHandlers)
            {
                var child = new ImageLayoutItem(itemRootChild, name, offsetChild, childSize);
                var relationship = new ImageLayoutRelationship(child, flags);

                OnRelationshipAdded.Raise(ImageLayoutEvents.Type, relationship);
            }
        }

        public static void ReportChildren(IImageLayoutItem existingItemParent, Func<IEnumerable<IImageLayoutItem>> childrenFunc, Func<IImageLayoutItem, ulong> offsetSelector, Func<IImageLayoutItem, ulong> sizeSelector)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemParent);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var parent = find.FoundItem;
                    var children = childrenFunc();

                    foreach (var child in children)
                    {
                        var offset = offsetSelector(child);
                        var size = sizeSelector(child);

                        AddRelationship(child, offset, size, parent, parent.Offset, parent.Size);
                    }
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddRelationship", existingItemParent.GetType().Name, existingItemParent);
                } 
            }
        }

        public static void EnumChildren(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> childrenIterator)
        {
            if (HasHandlers)
            {
                var enumerator = new ChildrenEnumerator(existingItemParent, childrenIterator);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator); 
            }
        }

        public static string PrintDescendants(IImageLayoutItem existingItemParent)
        {
            var builder = new StringBuilder();

            if (HasHandlers)
            {
                EnumDescendants(existingItemParent, (depth, d) =>
                {
                    var debugInfo = d.GetDebuggerDisplay();

                    builder.AppendLineFormat("{0}{1}", new string('\t', depth));
                });
            }

            return builder.ToString();
        }

        public static string PrintDescendantsIncludingReferences(IImageLayoutItem existingItemParent)
        {
            var builder = new StringBuilder();

            if (HasHandlers)
            {
                EnumDescendantsIncludingReferences(existingItemParent, (depth, d) =>
                {
                    var debugInfo = d.GetDebuggerDisplay();

                    builder.AppendLineFormat("{0}{1}", new string('\t', depth), debugInfo);
                });
            }

            return builder.ToString();
        }

        public static void EnumDescendantsIncludingReferences(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> descendantIterator)
        {
            if (HasHandlers)
            {
                var enumerator = new DescendantEnumerator(existingItemParent, descendantIterator, true);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator);
            }
        }

        public static void EnumDescendants(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> descendantIterator)
        {
            if (HasHandlers)
            {
                var enumerator = new DescendantEnumerator(existingItemParent, descendantIterator);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator);
            }
        }

        public static void EnumChildren(IImageLayoutItem existingItemParent, Action<int, IImageLayoutItem> childrenIterator, Func<IImageLayoutItem, bool> childrenFilter)
        {
            if (HasHandlers)
            {
                var enumerator = new ChildrenEnumeratorFilter(existingItemParent, childrenIterator, childrenFilter);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator); 
            }
        }

        public static void EnumReferences(IImageLayoutItem existingItemPrimary, Action<int, ReferencedItem> referenceIterator)
        {
            if (HasHandlers)
            {
                var enumerator = new ReferenceEnumerator(existingItemPrimary, referenceIterator);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator);
            }
        }

        public static void EnumReferences(IImageLayoutItem existingItemPrimary, Action<int, ReferencedItem> referenceIterator, Func<ReferencedItem, bool> referenceFilter)
        {
            if (HasHandlers)
            {
                var enumerator = new ReferenceEnumeratorFilter(existingItemPrimary, referenceIterator, referenceFilter);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator); 
            }
        }

        public static void EnumRoots(Action<int, IImageLayoutItem> rootIterator, Func<IImageLayoutItem, bool> rootFilter)
        {
            if (HasHandlers)
            {
                var enumerator = new RootEnumeratorFilter(rootIterator, rootFilter);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator); 
            }
        }

        public static void EnumRoots(Action<int, IImageLayoutItem> rootIterator)
        {
            if (HasHandlers)
            {
                var enumerator = new RootEnumerator(rootIterator);

                OnEnumerate.Raise(ImageLayoutEvents.Type, enumerator); 
            }
        }

        public static void AddReference(IImageLayoutItem itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem imagePrimary, string primaryName, ulong offsetPrimary, ulong primarySize, string referencingProperty)
        {
            if (HasHandlers)
            {
                var referenced = new ImageLayoutItem(itemReferenced, referencedName, offsetReferenced, referencedSize);
                var primary = new ImageLayoutItem(imagePrimary, primaryName, offsetPrimary, primarySize);
                var reference = new ImageLayoutReference(referenced, primary, referencingProperty);

                OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference); 
            }
        }

        public static void AddReference(IImageLayoutItem itemReferenced, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem imagePrimary, ulong offsetPrimary, ulong primarySize, string referencingProperty)
        {
            if (HasHandlers)
            {
                var referenced = new ImageLayoutItem(itemReferenced, offsetReferenced, referencedSize);
                var primary = new ImageLayoutItem(imagePrimary, offsetPrimary, primarySize);
                var reference = new ImageLayoutReference(referenced, primary, referencingProperty);

                OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference); 
            }
        }

        public static void AddReference(IImageLayoutItem itemReferenced, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, string referencingProperty)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem(itemReferenced, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference(referenced, primary, referencingProperty);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference(IImageLayoutItem existingItemReferenced, IImageLayoutItem existingItemPrimary, string referencingProperty)
        {
            if (HasHandlers)
            {
                var findPrimary = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, findPrimary);

                if (findPrimary.FoundItem != null)
                {
                    var findReferenced = new ImageLayoutItemFind(existingItemReferenced);

                    FindExistingItem.Raise(ImageLayoutEvents.Type, findReferenced);

                    if (findReferenced.FoundItem != null)
                    {
                        var referenced = findReferenced.FoundItem;
                        var primary = findPrimary.FoundItem;
                        var reference = new ImageLayoutReference(referenced, primary, referencingProperty);

                        OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                    }
                    else if (FindExistingItem.HasHandler())
                    {
                        e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemReferenced.GetType().Name, existingItemReferenced);
                    }
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<T>(T itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, string referencingProperty)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem<T>(itemReferenced, referencedName, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference<T>(referenced, primary, referencingProperty, referencedName);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TTuple>(TTuple itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, string referencingProperty, params string[] itemNames) where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItemTuple<TTuple>(itemReferenced, referencedName, offsetReferenced, referencedSize, itemNames);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference<TTuple>(referenced, primary, referencingProperty, referencedName);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference(IImageLayoutItem itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, string referencingProperty)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem(itemReferenced, referencedName, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference(referenced, primary, referencingProperty);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void ReportReferences(IImageLayoutItem existingItemPrimary, Func<IEnumerable<IImageLayoutItem>> referencedFunc, Func<IImageLayoutItem, ulong> offsetSelector, Func<IImageLayoutItem, ulong> sizeSelector, Func<IImageLayoutItem, string> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var primary = find.FoundItem;
                    var references = referencedFunc();

                    foreach (var referenced in references)
                    {
                        var offset = offsetSelector(referenced);
                        var size = sizeSelector(referenced);
                        var referencingProperty = referencingPropertySelector(referenced);

                        AddReference(referenced, offset, size, primary, primary.Offset, primary.Size, referencingProperty);
                    }
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TPrimaryItem, TReferenceKey>(IImageLayoutItem itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem imagePrimary, string primaryName, ulong offsetPrimary, ulong primarySize, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var referenced = new ImageLayoutItem(itemReferenced, referencedName, offsetReferenced, referencedSize);
                var primary = new ImageLayoutItem(imagePrimary, primaryName, offsetPrimary, primarySize);
                var reference = new ImageLayoutReference(referenced, primary, memberInfo.Name);

                OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference); 
            }
        }

        public static void AddReference<TPrimaryItem, TReferenceKey>(IImageLayoutItem itemReferenced, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem imagePrimary, ulong offsetPrimary, ulong primarySize, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var referenced = new ImageLayoutItem(itemReferenced, offsetReferenced, referencedSize);
                var primary = new ImageLayoutItem(imagePrimary, offsetPrimary, primarySize);
                var reference = new ImageLayoutReference(referenced, primary, memberInfo.Name);

                OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference); 
            }
        }

        public static void AddReference<TPrimaryItem, TReferenceKey>(IImageLayoutItem itemReferenced, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem(itemReferenced, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference(referenced, primary, memberInfo.Name);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TPrimaryItem, TReferenceKey>(IImageLayoutItem existingItemReferenced, TPrimaryItem existingItemPrimary, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var findPrimary = new ImageLayoutItemFind((IImageLayoutItem)existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, findPrimary);

                if (findPrimary.FoundItem != null)
                {
                    var findReferenced = new ImageLayoutItemFind(existingItemReferenced);

                    FindExistingItem.Raise(ImageLayoutEvents.Type, findReferenced);

                    if (findReferenced.FoundItem != null)
                    {
                        var referenced = findReferenced.FoundItem;
                        var primary = findPrimary.FoundItem;
                        var reference = new ImageLayoutReference(referenced, primary, memberInfo.Name);

                        OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                    }
                    else if (FindExistingItem.HasHandler())
                    {
                        e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemReferenced.GetType().Name, existingItemReferenced);
                    }
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TReferenced, TPrimaryItem, TReferenceKey>(TReferenced itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem<TReferenced>(itemReferenced, referencedName, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference<TReferenced>(referenced, primary, memberInfo.Name);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TTuple, TPrimaryItem, TReferenceKey>(TTuple itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector, params string[] itemNames) where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItemTuple<TTuple>(itemReferenced, referencedName, offsetReferenced, referencedSize, itemNames);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference<TTuple>(referenced, primary, memberInfo.Name);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void AddReference<TPrimaryItem, TReferenceKey>(IImageLayoutItem itemReferenced, string referencedName, ulong offsetReferenced, ulong referencedSize, IImageLayoutItem existingItemPrimary, Expression<Func<TPrimaryItem, TReferenceKey>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var memberInfo = referencingPropertySelector.GetMemberFromSelector();
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var referenced = new ImageLayoutItem(itemReferenced, referencedName, offsetReferenced, referencedSize);
                    var primary = find.FoundItem;
                    var reference = new ImageLayoutReference(referenced, primary, memberInfo.Name);

                    OnReferenceAdded.Raise(ImageLayoutEvents.Type, reference);
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }

        public static void ReportReferences<TPrimaryItem, TReferenceKey>(IImageLayoutItem existingItemPrimary, Func<IEnumerable<IImageLayoutItem>> referencedFunc, Func<IImageLayoutItem, ulong> offsetSelector, Func<IImageLayoutItem, ulong> sizeSelector, Func<IImageLayoutItem, Expression<Func<TPrimaryItem, TReferenceKey>>> referencingPropertySelector)
        {
            if (HasHandlers)
            {
                var find = new ImageLayoutItemFind(existingItemPrimary);

                FindExistingItem.Raise(ImageLayoutEvents.Type, find);

                if (find.FoundItem != null)
                {
                    var primary = find.FoundItem;
                    var references = referencedFunc();

                    foreach (var referenced in references)
                    {
                        var offset = offsetSelector(referenced);
                        var size = sizeSelector(referenced);
                        var selector = referencingPropertySelector(referenced);
                        var memberInfo = selector.GetMemberFromSelector();

                        AddReference(referenced, offset, size, primary, primary.Offset, primary.Size, memberInfo.Name);
                    }
                }
                else if (FindExistingItem.HasHandler())
                {
                    e.Throw<IndexOutOfRangeException>("Item not found from {0}{2}.  Item searched: {1}, {2}", ImageLayoutEvents.Type, "AddReference", existingItemPrimary.GetType().Name, existingItemPrimary);
                } 
            }
        }
    }
}
