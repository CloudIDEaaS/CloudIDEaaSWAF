using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.ConditionPoints
{
    public class ComparableListConditionPoint<T> : ConditionPointBase, IList<T>, IComparableListConditionPoint where T : IComparable<T>
    {
        private List<T> internalList;
        public bool IsReadOnly => false;
        public FanListSetting<T> FanListSetting { get; private set; }
        object IComparableListConditionPoint.FanListSetting => this.FanListSetting;

        public ComparableListConditionPoint(ConditionPointKind conditionPointType) : base(conditionPointType)
        {
            this.internalList = new List<T>();
        }

        public void SetFanListSettings(T start, T end, Func<T, T> getNext)
        {
            this.FanListSetting = new FanListSetting<T>(start, end, getNext);
        }

        public void FanList()
        {
            T next;
            T last;

            next = this.FanListSetting.Start;
            last = next;

            do
            {
                this.internalList.Add(next);
                next = this.FanListSetting.GetNext(next);

                if (next.CompareTo(last) <= 0)
                {
                    throw new ArgumentOutOfRangeException("Fan list retrieving same value and will never complete.");
                }
            }
            while (next.CompareTo(this.FanListSetting.End) != 0);
        }

        public override List<IList<IConditionItem>> GetConditionLists()
        {
            var itemName = this.GetItemName();

            return new List<IList<IConditionItem>> { this.Select(i => new ConditionItem<T>(itemName, i)).Cast<IConditionItem>().ToList() };
        }

        public T this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual void Add(T item)
        {
            internalList.Add(item);
        }

        public virtual void AddRange(System.Collections.Generic.IEnumerable<T> collection)
        {
            internalList.AddRange(collection);
        }

        public virtual System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly()
        {
            return internalList.AsReadOnly();
        }

        public virtual int BinarySearch(int index, int count, T item, System.Collections.Generic.IComparer<T>? comparer)
        {
            return internalList.BinarySearch(index, count, item, comparer);
        }

        public virtual int BinarySearch(T item)
        {
            return internalList.BinarySearch(item);
        }

        public virtual int BinarySearch(T item, System.Collections.Generic.IComparer<T>? comparer)
        {
            return internalList.BinarySearch(item, comparer);
        }

        public virtual void Clear()
        {
            internalList.Clear();
        }

        public virtual bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public virtual void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            internalList.CopyTo(index, array, arrayIndex, count);
        }

        public virtual void CopyTo(T[] array)
        {
            internalList.CopyTo(array);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public virtual bool Exists(System.Predicate<T> match)
        {
            return internalList.Exists(match);
        }

        public virtual T Find(System.Predicate<T> match)
        {
            return internalList.Find(match);
        }

        public virtual System.Collections.Generic.List<T> FindAll(System.Predicate<T> match)
        {
            return internalList.FindAll(match);
        }

        public virtual int FindIndex(int startIndex, int count, System.Predicate<T> match)
        {
            return internalList.FindIndex(startIndex, count, match);
        }

        public virtual int FindIndex(int startIndex, System.Predicate<T> match)
        {
            return internalList.FindIndex(startIndex, match);
        }

        public virtual int FindIndex(System.Predicate<T> match)
        {
            return internalList.FindIndex(match);
        }

        public virtual T FindLast(System.Predicate<T> match)
        {
            return internalList.FindLast(match);
        }

        public virtual int FindLastIndex(int startIndex, int count, System.Predicate<T> match)
        {
            return internalList.FindLastIndex(startIndex, count, match);
        }

        public virtual int FindLastIndex(int startIndex, System.Predicate<T> match)
        {
            return internalList.FindLastIndex(startIndex, match);
        }

        public virtual int FindLastIndex(System.Predicate<T> match)
        {
            return internalList.FindLastIndex(match);
        }

        public virtual void ForEach(System.Action<T> action)
        {
            internalList.ForEach(action);
        }

        public virtual System.Collections.Generic.List<T>.Enumerator GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        public virtual System.Collections.Generic.List<T> GetRange(int index, int count)
        {
            return internalList.GetRange(index, count);
        }

        public virtual int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        public virtual int IndexOf(T item, int index)
        {
            return internalList.IndexOf(item, index);
        }

        public virtual int IndexOf(T item, int index, int count)
        {
            return internalList.IndexOf(item, index, count);
        }

        public virtual void Insert(int index, T item)
        {
            internalList.Insert(index, item);
        }

        public virtual void InsertRange(int index, System.Collections.Generic.IEnumerable<T> collection)
        {
            internalList.InsertRange(index, collection);
        }

        public virtual int LastIndexOf(T item)
        {
            return internalList.LastIndexOf(item);
        }

        public virtual int LastIndexOf(T item, int index)
        {
            return internalList.LastIndexOf(item, index);
        }

        public virtual int LastIndexOf(T item, int index, int count)
        {
            return internalList.LastIndexOf(item, index, count);
        }

        public virtual bool Remove(T item)
        {
            return internalList.Remove(item);
        }

        public virtual int RemoveAll(System.Predicate<T> match)
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

        public virtual void Sort(System.Collections.Generic.IComparer<T>? comparer)
        {
            internalList.Sort(comparer);
        }

        public virtual void Sort(System.Comparison<T> comparison)
        {
            internalList.Sort(comparison);
        }

        public virtual void Sort(int index, int count, System.Collections.Generic.IComparer<T>? comparer)
        {
            internalList.Sort(index, count, comparer);
        }

        public virtual T[] ToArray()
        {
            return internalList.ToArray();
        }

        public virtual void TrimExcess()
        {
            internalList.TrimExcess();
        }

        public virtual bool TrueForAll(System.Predicate<T> match)
        {
            return internalList.TrueForAll(match);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        public List<IConditionItem> GenerateConditionItems()
        {
            throw new NotImplementedException();
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
