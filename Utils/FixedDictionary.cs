using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;

namespace Utils
{
    public class FixedDictionary<TKey, TValue> : Queue<TKey>, IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> internalDictionary;
        private int maxCount;

        public FixedDictionary(int maxCount)
        {
            internalDictionary = new Dictionary<TKey, TValue>();
            this.maxCount = maxCount;
        }

        public void Add(TKey key, TValue value)
        {
            if (this.Count >= maxCount)
            {
                internalDictionary.Remove(base.Dequeue());
            }

            base.Enqueue(key);
            internalDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return internalDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get 
            {
                return internalDictionary.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            return internalDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return internalDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get 
            {
                return internalDictionary.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return internalDictionary[key];
            }
            set
            {
                internalDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (this.Count >= maxCount)
            {
                internalDictionary.Remove(base.Dequeue());
            }

            base.Enqueue(item.Key);
            internalDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            base.Clear();
            internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return internalDictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get 
            {
                return internalDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return internalDictionary.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }
    }
}
