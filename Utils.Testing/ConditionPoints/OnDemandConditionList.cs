using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.ConditionPoints
{
    public class OnDemandConditionList<T> : IList<IConditionItem>, IOnDemandConditionList
    {
        private List<IConditionItem> internalList;
        public bool IsReadOnly => false;
        public IConditionItem this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<IConditionItem> Group { get; set; }
        public event CreateOnDemandEventHandler CreateOnDemand;

        public OnDemandConditionList()
        {
            this.internalList = new List<IConditionItem>();
        }

        protected OnDemandConditionList(List<IConditionItem> list)
        {
            this.internalList = list;
        }

        public object Clone()
        {
            var onDemandConditionList = new OnDemandConditionList<T>(internalList.ToList());

            onDemandConditionList.Group = this.Group;

            onDemandConditionList.CreateOnDemand += (sender, e) =>
            {
                CreateOnDemand(sender, e);
            };

            return onDemandConditionList;
        }

        public virtual void Add(Utils.ConditionPoints.IConditionItem item)
        {
            internalList.Add(item);
        }

        public virtual void AddRange(System.Collections.Generic.IEnumerable<Utils.ConditionPoints.IConditionItem> collection)
        {
            internalList.AddRange(collection);
        }

        public virtual System.Collections.ObjectModel.ReadOnlyCollection<Utils.ConditionPoints.IConditionItem> AsReadOnly()
        {
            return internalList.AsReadOnly();
        }

        public virtual int BinarySearch(int index, int count, Utils.ConditionPoints.IConditionItem item, System.Collections.Generic.IComparer<Utils.ConditionPoints.IConditionItem>? comparer)
        {
            return internalList.BinarySearch(index, count, item, comparer);
        }

        public virtual int BinarySearch(Utils.ConditionPoints.IConditionItem item)
        {
            return internalList.BinarySearch(item);
        }

        public virtual int BinarySearch(Utils.ConditionPoints.IConditionItem item, System.Collections.Generic.IComparer<Utils.ConditionPoints.IConditionItem>? comparer)
        {
            return internalList.BinarySearch(item, comparer);
        }

        public virtual void Clear()
        {
            internalList.Clear();
        }

        public virtual bool Contains(Utils.ConditionPoints.IConditionItem item)
        {
            return internalList.Contains(item);
        }

        public virtual void CopyTo(int index, Utils.ConditionPoints.IConditionItem[] array, int arrayIndex, int count)
        {
            internalList.CopyTo(index, array, arrayIndex, count);
        }

        public virtual void CopyTo(Utils.ConditionPoints.IConditionItem[] array)
        {
            internalList.CopyTo(array);
        }

        public virtual void CopyTo(Utils.ConditionPoints.IConditionItem[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public virtual bool Exists(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.Exists(match);
        }

        public virtual Utils.ConditionPoints.IConditionItem Find(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.Find(match);
        }

        public virtual System.Collections.Generic.List<Utils.ConditionPoints.IConditionItem> FindAll(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindAll(match);
        }

        public virtual int FindIndex(int startIndex, int count, System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindIndex(startIndex, count, match);
        }

        public virtual int FindIndex(int startIndex, System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindIndex(startIndex, match);
        }

        public virtual int FindIndex(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindIndex(match);
        }

        public virtual Utils.ConditionPoints.IConditionItem FindLast(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindLast(match);
        }

        public virtual int FindLastIndex(int startIndex, int count, System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindLastIndex(startIndex, count, match);
        }

        public virtual int FindLastIndex(int startIndex, System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindLastIndex(startIndex, match);
        }

        public virtual int FindLastIndex(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.FindLastIndex(match);
        }

        public virtual void ForEach(System.Action<Utils.ConditionPoints.IConditionItem> action)
        {
            internalList.ForEach(action);
        }

        public virtual System.Collections.Generic.List<Utils.ConditionPoints.IConditionItem>.Enumerator GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        public virtual System.Collections.Generic.List<Utils.ConditionPoints.IConditionItem> GetRange(int index, int count)
        {
            return internalList.GetRange(index, count);
        }

        public virtual int IndexOf(Utils.ConditionPoints.IConditionItem item)
        {
            return internalList.IndexOf(item);
        }

        public virtual int IndexOf(Utils.ConditionPoints.IConditionItem item, int index)
        {
            return internalList.IndexOf(item, index);
        }

        public virtual int IndexOf(Utils.ConditionPoints.IConditionItem item, int index, int count)
        {
            return internalList.IndexOf(item, index, count);
        }

        public virtual void Insert(int index, Utils.ConditionPoints.IConditionItem item)
        {
            internalList.Insert(index, item);
        }

        public virtual void InsertRange(int index, System.Collections.Generic.IEnumerable<Utils.ConditionPoints.IConditionItem> collection)
        {
            internalList.InsertRange(index, collection);
        }

        public virtual int LastIndexOf(Utils.ConditionPoints.IConditionItem item)
        {
            return internalList.LastIndexOf(item);
        }

        public virtual int LastIndexOf(Utils.ConditionPoints.IConditionItem item, int index)
        {
            return internalList.LastIndexOf(item, index);
        }

        public virtual int LastIndexOf(Utils.ConditionPoints.IConditionItem item, int index, int count)
        {
            return internalList.LastIndexOf(item, index, count);
        }

        public virtual bool Remove(Utils.ConditionPoints.IConditionItem item)
        {
            return internalList.Remove(item);
        }

        public virtual int RemoveAll(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.RemoveAll(match);
        }

        public virtual void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
        }

        public virtual void RemoveRange(int index, int count)
        {
            internalList.RemoveRange(index, count);
        }

        public virtual void Reverse()
        {
            internalList.Reverse();
        }

        public virtual void Reverse(int index, int count)
        {
            internalList.Reverse(index, count);
        }

        public virtual void Sort()
        {
            internalList.Sort();
        }

        public virtual void Sort(System.Collections.Generic.IComparer<Utils.ConditionPoints.IConditionItem>? comparer)
        {
            internalList.Sort(comparer);
        }

        public virtual void Sort(System.Comparison<Utils.ConditionPoints.IConditionItem> comparison)
        {
            internalList.Sort(comparison);
        }

        public virtual void Sort(int index, int count, System.Collections.Generic.IComparer<Utils.ConditionPoints.IConditionItem>? comparer)
        {
            internalList.Sort(index, count, comparer);
        }

        public virtual Utils.ConditionPoints.IConditionItem[] ToArray()
        {
            return internalList.ToArray();
        }

        public virtual void TrimExcess()
        {
            internalList.TrimExcess();
        }

        public virtual bool TrueForAll(System.Predicate<Utils.ConditionPoints.IConditionItem> match)
        {
            return internalList.TrueForAll(match);
        }

        IEnumerator<IConditionItem> IEnumerable<IConditionItem>.GetEnumerator()
        {
            CreateOnDemand(this, new CreateOnDemandEventArgs(this, this.Add));

            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            CreateOnDemand(this, new CreateOnDemandEventArgs(this, this.Add));

            return internalList.GetEnumerator();
        }

        public virtual int Capacity
        {
            get
            {
                return internalList.Capacity;
            }

            set
            {
                internalList.Capacity = value;
            }
        }

        public virtual int Count
        {
            get
            {
                return internalList.Count;
            }
        }
    }
}
