using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class WeakDictionary<TKey, TValue> : BaseDictionary<TKey, TValue> where TValue : class where TKey : class
    {
        private readonly Dictionary<TKey, WeakReference<TValue>> innerDictionary = new Dictionary<TKey, WeakReference<TValue>>();

        public override int Count { get; }

        public override void Add(TKey key, TValue value)
        {
            innerDictionary.Add(key, new WeakReference<TValue>(value));
        }

        public override void Clear()
        {
            innerDictionary.Clear();
        }

        public override bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key) && innerDictionary[key].TryGetTarget(out TValue? _);
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDictionary.Where(p => p.Value.TryGetTarget(out TValue? target)).Select(p => new KeyValuePair<TKey, TValue>()).GetEnumerator();
        }

        public override bool Remove(TKey key)
        {
            return innerDictionary.Remove(key);
        }
        
        public void Cull()
        {
            var deadKeys = this.innerDictionary.Where(p => p.Value.TryGetTarget(out TValue? _)).Select(p => p.Key).ToList();

            foreach (var key in deadKeys)
            {
                this.innerDictionary.Remove(key);
            }
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var reference = innerDictionary[key];

            value = null!;

            return reference.TryGetTarget(out value);
        }

        protected override void SetValue(TKey key, TValue value)
        {
            innerDictionary.Add(key, new WeakReference<TValue>(value));
        }
    }
}
