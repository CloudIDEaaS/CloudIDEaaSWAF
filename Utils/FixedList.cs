using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace Utils
{
    [Serializable, DebuggerDisplay("Count = {Count}")]
    public class FixedList<T> : IList<T>
    {
        private List<T> internalList;
        private int maxCount;

        public FixedList(int maxCount)
        {
            internalList = new List<T>();
            this.maxCount = maxCount;
        }

        public int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);

            if (this.Count >= maxCount)
            {
                internalList.RemoveAt(0);
            }
        }

        public void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return internalList[index];
            }
            set
            {
                internalList[index] = value;
            }
        }

        public void Add(T item)
        {
            if (this.Count >= maxCount)
            {
                internalList.RemoveAt(0);
            }

            internalList.Add(item);
        }

        public void Clear()
        {
            internalList.Clear();
        }

        public bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get 
            {
                return internalList.Count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            return internalList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalList.GetEnumerator();
        }
    }
}
