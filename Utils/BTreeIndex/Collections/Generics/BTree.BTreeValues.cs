/* 
 * Copyright(c) 2002 Gerardo Recinto/4A Technologies (http://www.4atech.net)
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace BTreeIndex.Collections.Generic.BTree
{
	internal class BTreeValues<TKey, TValue> : System.Collections.Generic.ICollection<TValue>, IEnumerable<TValue>
	{
		public BTreeValues(BTreeIndex.Collections.Generic.BTree.BTreeDictionary<TKey, TValue> Dictionary)
		{
			this.Dictionary = Dictionary;
		}
		public void Add(TValue item)
		{
			throw new InvalidOperationException();
		}

		public void Clear()
		{
			throw new InvalidOperationException();
		}

		public bool Contains(TValue item)
		{
			throw new InvalidOperationException();
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			if (arrayIndex < 0 || arrayIndex >= array.Length)
				throw new ArgumentOutOfRangeException("arrayIndex");
			if (Dictionary.Count > array.Length - arrayIndex)
				throw new InvalidOperationException("There are more items to copy than elements on target starting from index");
			if (Dictionary.MoveFirst())
			{
				do
				{
					array[arrayIndex++] = Dictionary.CurrentValue;
				} while (Dictionary.MoveNext());
			}
		}

		public int Count
		{
			get
			{
				return Dictionary.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool Remove(TValue item)
		{
			throw new InvalidOperationException();
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return new BTreeDictionary<TKey, TValue>.BTreeEnumeratorValue(Dictionary);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

        BTreeIndex.Collections.Generic.BTree.BTreeDictionary<TKey, TValue> Dictionary;
	}
}
