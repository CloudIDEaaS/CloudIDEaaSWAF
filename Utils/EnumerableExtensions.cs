using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using BTreeIndex.Collections.Generic.BTree;
using System.Numerics;

namespace Utils
{
    public static class EnumerableExtensions
    {
        public static IterationStateEnumerable<T> WithState<T>(this IEnumerable<T> enumerable)
        {
            return new IterationStateEnumerable<T>(enumerable);
        }

        public static TValue? GetDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default) where TKey : notnull
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultValue;
        }

        public static IEnumerable<T> RandomExpand<T>(this IEnumerable<T> source, int targetCount)
        {
            var random = new Random();
            var list = new List<T>();

            while (list.Count < targetCount)
            {
                list.AddRange(source.OrderBy<T, int>((item) => random.Next()));
            }

            return list.Randomize().Take(targetCount);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, Func<T, T, bool> compare) where T : IComparable<T>
        {
            var random = new Random();
            var last = default(T);
            var lastIndex = 0;
            var list = source.Randomize().ToList();
            var count = source.Count();
            var redo = true;
            var randomizeCount = 0;
            var redoCount = 0;

            while (redo)
            {
                var x = 0;
                redo = false;

                foreach (var item in list)
                {
                    if (last != null && last.CompareTo(default(T)) != 0)
                    {
                        if (!compare(item, last))
                        {
                            list.Remove(item);

                            if (count - x > x)
                            {
                                list.Add(item);
                            }
                            else
                            {
                                list.Insert(0, item);
                            }

                            last = default(T);
                            redo = true;
                            redoCount++;

                            break;
                        }
                    }

                    last = item;
                    lastIndex = x;

                    x++;
                }

                if (randomizeCount > byte.MaxValue)
                {
                    e.Throw<StackOverflowException>("Not enough disparity among values");
                }
                else if (redoCount > byte.MaxValue)
                {
                    list = source.Randomize().ToList();
                    redoCount = 0;
                    randomizeCount++;
                }
            }

            return list;
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences, bool cloneInstances = false)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            if (cloneInstances)
            {
                return sequences.Aggregate(emptyProduct, (accumulator, sequence) => accumulator.Join(sequence, (o) => true, (i) => true, (o, i) =>
                {
                    var first = o.Select(i => i is ICloneable ? (T)((ICloneable)i).Clone() : i).ToList();
                    var second = i is ICloneable ? (T)((ICloneable)i).Clone() : i;
                    var group = first.Concat(new[] { second });

                    return group;
                }));
            }
            else
            {
                return sequences.Aggregate(emptyProduct, (accumulator, sequence) => accumulator.Join(sequence, (o) => true, (i) => true, (o, i) => o.Concat(new[] { i })));
            }
        }

        public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, bool outerJoin = false)
        {
            var firstCount = first.Count();
            var secondCount = second.Count();

            if (outerJoin)
            {
                if (firstCount == 0)
                {
                    return second.Select(s => (default(T1), (T2)s));
                }
                else if (secondCount == 0)
                {
                    return first.Select(s => ((T1)s, default(T2)));
                }
            }

            return from t1 in first from t2 in second select (t1, t2);
        }

        public static object ToTypedList(this IEnumerable enumerable, Type itemType)
        {
            var listType = typeof(List<>).MakeGenericType(new Type[] { itemType });
            var listInstance = (IList)Activator.CreateInstance(listType);

            foreach (var obj in enumerable)
            {
                listInstance.Add(obj);
            }

            return listInstance;
        }

        public static string NameValuesToUrlQueryString(this IEnumerable<KeyValuePair<string, string>> pairs, string urlBase)
        {
            var builder = new StringBuilder();

            builder.Append(urlBase + "?");

            foreach (var keyValuePair in pairs)
            {
                builder.AppendFormat("{0}={1}&", keyValuePair.Key, keyValuePair.Value);
            }

            builder.RemoveEnd(1);

            return builder.ToString();
        }

        public static List<KeyValuePair<string, string>> PairedListToKeyValuePairs(this IEnumerable<string> pairs, Func<string, KeyValuePair<string, string>> matchExceptionCallback)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            var regex = new Regex(@"(?<key>[\w\d_].*?)=(?<value>.*)$");
            string key = null;
            string value = null;

            foreach (var pair in pairs)
            {
                if (!regex.Match(pair, m =>
                {
                    key = m.GetGroupValue("key");
                    value = m.GetGroupValue("value");
                }))
                {
                    var keyValuePair = matchExceptionCallback(pair);

                    key = keyValuePair.Key;
                    value = keyValuePair.Value;
                }

                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return keyValuePairs;
        }

        public static bool InAll<T>(this IEnumerable<IEnumerable<T>> collectionOfCollections, T value)
        {
            foreach (var collection in collectionOfCollections)
            {
                if (!collection.Any(i => i.Equals(value)))
                {
                    return false;
                }
            }

            return true;
        }

        public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
        {
            for (var x = 0; x < count; x++)
            {
                if (queue.Count > 0)
                {
                    yield return queue.Dequeue();
                }
                else
                {
                    break;
                }
            }
        }

        public static T PopPeek<T>(this Stack<T> stack)
        {
            stack.Pop();
            return stack.Peek();
        }

        public static T Pop<T>(this Stack<T> stack, int count)
        {
            for (var x = 0; x < count - 1; x++)
            {
                stack.Pop();
            }

            return stack.Pop();
        }

        public static string Print<T>(this Stack<T> stack)
        {
            var builder = new StringBuilder();

            foreach (var item in stack.Reverse())
            {
                if (item.Equals(stack.First()))
                {
                    builder.AppendFormat("{0}", item.ToString());
                }
                else
                {
                    builder.AppendFormat("{0} -> ", item.ToString());
                }
            }

            return builder.ToString();
        }

        public static Dictionary<TKey, TValue> ToDictionary2<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            return keyValuePairs.ToDictionary(p => p.Key, p => p.Value);
        }

        public static void AddToListIfNotExists<TListItem>(this List<TListItem> list, TListItem listItem)
        {
            if (!list.Contains(listItem))
            {
                list.Add(listItem);
            }
        }

        public static void AddToListIfNotExists<TListItem>(this List<TListItem> list, IEnumerable<TListItem> listItems)
        {
            foreach (var listItem in listItems)
            {
                if (!list.Contains(listItem))
                {
                    list.Add(listItem);
                }
            }
        }

        public static void AddToDictionaryListCreateIfNotExist<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem, IEqualityComparer<TListItem> comparer = null)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();

                dictionary.Add(key, items);
            }

            if (comparer != null)
            {
                if (!items.Contains(listItem, comparer))
                {
                    items.Add(listItem);
                }
            }
            else
            {
                if (!items.Contains(listItem))
                {
                    items.Add(listItem);
                }
            }
        }

        public static void AddToDictionaryListCreateIfNotExist<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key, IEnumerable<TListItem> listItems)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];

                items.AddRange(listItems);
            }
            else
            {
                items = new List<TListItem>();

                dictionary.Add(key, listItems.ToList());
            }
        }

        public static void AddToBTreeDictionaryListCreateIfNotExist<TKey, TListItem>(this BTreeDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem, IEqualityComparer<TListItem> comparer = null)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();

                dictionary.Add(key, items);
            }

            if (comparer != null)
            {
                if (!items.Contains(listItem, comparer))
                {
                    items.Add(listItem);
                }
            }
            else
            {
                if (!items.Contains(listItem))
                {
                    items.Add(listItem);
                }
            }
        }

        public static void AddToDictionaryList<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem item)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();
                dictionary.Add(key, items);
            }

            items.Add(item);
        }

        public static void AddToDictionaryList<TKey, TListItem>(this SortedDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem item)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();
                dictionary.Add(key, items);
            }

            items.Add(item);
        }

        public static void AddToDictionaryList<TKey, TListItem>(this FixedDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem item)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();
                dictionary.Add(key, items);
            }

            items.Add(item);
        }

        public static void AddToDictionaryListCreateIfNotExist<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();

                dictionary.Add(key, items);
            }
        }

        public static void RemoveFromDictionaryListIfExist<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem)
        {
            if (dictionary.ContainsKey(key))
            {
                var items = dictionary[key];
                items.Remove(listItem);
            }
        }

        public static void ReplaceInDictionaryListIfExist<TKey, TListItem>(this Dictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem, TListItem listItemNew)
        {
            if (dictionary.ContainsKey(key))
            {
                var items = dictionary[key];

                if (items.Contains(listItem))
                {
                    var index = items.IndexOf(listItem);

                    items.Remove(listItem);
                    items.Insert(index, listItemNew);
                }
            }
        }

        public static void AddToBaseDictionaryListCreateIfNotExist<TKey, TListItem>(this BaseDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem)
        {
            List<TListItem> items;

            if (dictionary.ContainsKey(key))
            {
                items = dictionary[key];
            }
            else
            {
                items = new List<TListItem>();

                dictionary.Add(key, items);
            }

            if (!items.Contains(listItem))
            {
                items.Add(listItem);
            }
        }

        public static void RemoveFromBaseDictionaryListIfExist<TKey, TListItem>(this BaseDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem)
        {
            if (dictionary.ContainsKey(key))
            {
                var items = dictionary[key];
                items.Remove(listItem);
            }
        }

        public static void ReplaceInBaseDictionaryListIfExist<TKey, TListItem>(this BaseDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem listItem, TListItem listItemNew)
        {
            if (dictionary.ContainsKey(key))
            {
                var items = dictionary[key];

                if (items.Contains(listItem))
                {
                    var index = items.IndexOf(listItem);

                    items.Remove(listItem);
                    items.Insert(index, listItemNew);
                }
            }
        }

        public static void AddIfNotExist<TKey, TItem>(this SortedDictionary<TKey, TItem> dictionary, TKey key, TItem value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddIfNotExist<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key, TItem value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        public static TItem AddOrUpdateDictionary<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key, TItem value, Action<TItem> action)
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];
                action(item);
            }
            else
            {
                item = (TItem)value;

                dictionary.Add(key, item);
            }

            return item;
        }

        public static void AddRangeOverride<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToAdd)
        {
            dictionaryToAdd.ForEach(x => dictionary[x.Key] = x.Value);
        }

        public static void AddRangeNewOnly<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToAdd)
        {
            dictionaryToAdd.ForEach(x => { if (!dictionary.ContainsKey(x.Key)) dictionary.Add(x.Key, x.Value); });
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToAdd)
        {
            dictionaryToAdd.ForEach(x => dictionary.Add(x.Key, x.Value));
        }

        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> valuesToAdd, bool strictMode = false)
        {
            valuesToAdd.ForEach(x =>
            {
                hashSet.Add(x, strictMode);
            });
        }

        public static void Add<T>(this HashSet<T> hashSet, T valueToAdd, bool strictMode = false)
        {
            if (!hashSet.Add(valueToAdd))
            {
                if (strictMode)
                {
                    throw new IndexOutOfRangeException(string.Format("Value '{0}' is not unique", valueToAdd));
                }
            }
        }

        public static bool ContainsAny<T>(this System.Collections.Generic.ICollection<T> collection, IEnumerable<T> searchItems)
        {
            return collection.Any(c => searchItems.Any(i => EqualityComparer<T>.Default.Equals(c, i)));
        }

        public static bool ContainsAny<T>(this System.Collections.Generic.IDictionary<T, T> dictionary, IEnumerable<T> searchItems)
        {
            return dictionary.Any(c => searchItems.Any(i => EqualityComparer<T>.Default.Equals(c.Key, i) || EqualityComparer<T>.Default.Equals(c.Value, i)));
        }

        public static bool Contains<T>(this System.Collections.Generic.IDictionary<T, T> dictionary, T searchItem)
        {
            return dictionary.Any(c => EqualityComparer<T>.Default.Equals(c.Key, searchItem) || EqualityComparer<T>.Default.Equals(c.Value, searchItem));
        }

        public static void AddOrUpdateRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToAddOrUpdate)
        {
            dictionaryToAddOrUpdate.ForEach(x =>
            {
                if (dictionary.ContainsKey(x.Key))
                {
                    dictionary[x.Key] = x.Value;
                }
                else
                {
                    dictionary.Add(x.Key, x.Value);
                }
            });
        }

        public static bool ContainsKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            bool result = false;
            keys.ForEachOrBreak((x) => { result = dictionary.ContainsKey(x); return result; });
            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEachOrBreak<T>(this IEnumerable<T> source, Func<T, bool> func)
        {
            foreach (var item in source)
            {
                bool result = func(item);
                if (result) break;
            }
        }

        public static TItem AddAndOrUpdateDictionary<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key, TItem value, Action<TItem> action = null)
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];

                if (action == null)
                {
                    dictionary.Remove(key);
                    dictionary.Add(key, value);

                    item = (TItem)value;
                }
            }
            else
            {
                item = (TItem)value;

                dictionary.Add(key, item);
            }

            if (action != null)
            {
                action(item);
            }

            return item;
        }

        public static TItem AddToDictionaryIfNotExist<TKey, TItem>(this SortedDictionary<TKey, TItem> dictionary, TKey key, TItem value)
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];
            }
            else
            {
                item = (TItem)value;

                dictionary.Add(key, item);
            }

            return item;
        }

        public static TItem AddToDictionaryIfNotExist<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key, TItem value)
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];
            }
            else
            {
                item = (TItem) value;

                dictionary.Add(key, item);
            }

            return item;
        }

        public static TItem AddToDictionaryIfNotExist<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key, Func<TItem> createValue)
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];
            }
            else
            {
                item = (TItem) createValue();

                dictionary.Add(key, item);
            }

            return item;
        }

        public static TItem AddToDictionaryIfNotExist<TKey, TItem>(this Dictionary<TKey, TItem> dictionary, TKey key) where TItem : new()
        {
            TItem item;

            if (dictionary.ContainsKey(key))
            {
                item = dictionary[key];
            }
            else
            {
                var constructor = typeof(TItem).GetConstructors().Single(c => c.GetParameters().Length == 0);

                item = (TItem)constructor.Invoke(null);

                dictionary.Add(key, item);
            }

            return item;
        }

        public static void AddToDictionaryDictionaryIfNotExist<TKey, TInternalDictionaryKey, TInternalDictionaryItem>(this Dictionary<TKey, Dictionary<TInternalDictionaryKey, TInternalDictionaryItem>> dictionary, TKey key, TInternalDictionaryKey internalKey, TInternalDictionaryItem item)
        {
            Dictionary<TInternalDictionaryKey, TInternalDictionaryItem> internalDictionary;

            if (dictionary.ContainsKey(key))
            {
                internalDictionary = dictionary[key];
            }
            else
            {
                internalDictionary = new Dictionary<TInternalDictionaryKey, TInternalDictionaryItem>();

                dictionary.Add(key, internalDictionary);
            }

            if (!internalDictionary.ContainsKey(internalKey))
            {
                internalDictionary.Add(internalKey, item);
            }
        }

        public static void RemoveFromDictionaryDictionaryIfExist<TKey, TInternalDictionaryKey, TInternalDictionaryItem>(this Dictionary<TKey, Dictionary<TInternalDictionaryKey, TInternalDictionaryItem>> dictionary, TKey key, TInternalDictionaryKey internalKey)
        {
            if (dictionary.ContainsKey(key))
            {
                var internalDictionary = dictionary[key];
                internalDictionary.Remove(internalKey);
            }
        }

        public static void AddToDictionaryDictionaryIfNotExist<TKey, TInternalDictionaryKey, TInternalDictionaryItem>(this Dictionary<TKey, SortedDictionary<TInternalDictionaryKey, TInternalDictionaryItem>> dictionary, TKey key, TInternalDictionaryKey internalKey, TInternalDictionaryItem item)
        {
            SortedDictionary<TInternalDictionaryKey, TInternalDictionaryItem> internalDictionary;

            if (dictionary.ContainsKey(key))
            {
                internalDictionary = dictionary[key];
            }
            else
            {
                internalDictionary = new SortedDictionary<TInternalDictionaryKey, TInternalDictionaryItem>();

                dictionary.Add(key, internalDictionary);
            }

            if (!internalDictionary.ContainsKey(internalKey))
            {
                internalDictionary.Add(internalKey, item);
            }
        }

        public static void RemoveFromDictionaryDictionaryIfExist<TKey, TInternalDictionaryKey, TInternalDictionaryItem>(this Dictionary<TKey, SortedDictionary<TInternalDictionaryKey, TInternalDictionaryItem>> dictionary, TKey key, TInternalDictionaryKey internalKey)
        {
            if (dictionary.ContainsKey(key))
            {
                var internalDictionary = dictionary[key];
                internalDictionary.Remove(internalKey);
            }
        }

        public static void Remove<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items.ToList())
            {
                list.Remove(item);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source.Count() == 0;
        }

        public static AdvanceableEnumeration<T> AsAdvanceable<T>(this IEnumerable<T> source)
        {
            return new AdvanceableEnumeration<T>(source);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            var random = new Random();

            return source.OrderBy<T, int>((item) => random.Next());
        }

        public static void Run(this Queue<Action> actions)
        {
            while (actions.Count > 0)
            {
                var action = actions.Dequeue();

                action();
            }
        }

        public static IEnumerable NotOfType<T>(this IEnumerable source)
        {
            return source.Cast<object>().Where(i => !(i is T));
        }

        public static IEnumerable NotOfType<T1, T2>(this IEnumerable source)
        {
            return source.Cast<object>().Where(i => !(i is T1) && !(i is T2));
        }

        public static bool GetNext<T>(this IEnumerable<T> items, T item, out T next)
        {
            var found = false;

            next = default(T);

            foreach (var existing in items)
            {
                if (found)
                {
                    next = existing;
                    return true;
                }
                else if (existing.Equals(item))
                {
                    found = true;
                }
            }

            return false;
        }

        public static T Peek<T>(this Queue<T> queue, Func<T, bool> where)
        {
            if (queue.Any(where))
            {
                return queue.First(where);
            }
            else
            {
                return default(T);
            }
        }

        public static TSource FirstGreaterThanOrEqualTo<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TKey value)
        {
            foreach (TSource item in source.OrderBy(keySelector))
            {
                var key = keySelector(item);

                if (typeof(TKey) == typeof(string))
                {
                    var keyString = (string)(object)key;
                    var valueString = (string)(object)value;

                    if (string.Compare(keyString, valueString) >= 0)
                    {
                        return item;
                    }
                }
                else if (typeof(TKey) == typeof(int))
                {
                    var keyInt = (int)(object)key;
                    var valueInt = (int)(object)value;

                    if (keyInt >= valueInt)
                    {
                        return item;
                    }
                }
            }

            return default(TSource);
        }

        public static TSource FirstGreaterThan<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TKey value, bool noSort = false)
        {
            IEnumerable<TSource> enumerable;

            if (noSort)
            {
                enumerable = source;
            }
            else
            {
                enumerable = source.OrderBy(keySelector);
            }

            foreach (TSource item in enumerable)
            {
                var key = keySelector(item);

                if (typeof(TKey) == typeof(string))
                {
                    var keyString = (string)(object) key;
                    var valueString = (string)(object) value;

                    if (string.Compare(keyString, valueString) > 0)
                    {
                        return item;
                    }
                }
                else if (typeof(TKey) == typeof(int))
                {
                    var keyInt = (int)(object)key;
                    var valueInt = (int)(object)value;

                    if (keyInt > valueInt)
                    {
                        return item;
                    }
                }
                else if (typeof(TKey) == typeof(char))
                {
                    var keychar = (char)(object)key;
                    var valuechar = (char)(object)value;

                    if (keychar > valuechar)
                    {
                        return item;
                    }
                }
                else
                {
                    e.Throw<NotImplementedException>("FirstGreaterThan does not handle type: {0}".AsFormat(typeof(TKey).Name));
                }
            }

            return default(TSource);
        }

        public static IEnumerable<T> DistinctByUtils<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static string ToCommaDelimitedList(this IEnumerable list)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += ", ";
                }

                text += item;
            }

            return text;
        }

        public static string ToDelimitedList(this IEnumerable list, string delimiter, Func<string, string> postProcess)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += ",";
                }

                text += postProcess(item);
            }

            return text;
        }

        public static string ToDelimitedList(this IEnumerable list, string delimiter)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += delimiter;
                }

                text += item;
            }

            return text;
        }

        public static string ToDelimitedList(this IEnumerable list, string delimiter, Func<string, bool, string> postProcess)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();
            var isLast = false;

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += delimiter;
                }
                else
                {
                    isLast = true;
                }

                text += postProcess(item, isLast);
            }

            return text;
        }

        public static string ToCommaDelimitedList(this IEnumerable list, Func<string, bool, string> postProcess)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();
            var isLast = false;

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += ",";
                }
                else
                {
                    isLast = true;
                }

                text += postProcess(item, isLast);
            }

            return text;
        }

        public static string ToMultiLineList(this IEnumerable list)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                item += "\r\n";

                text += item;
            }

            return text;
        }

        public static string ToMultiLineList(this IEnumerable list, Func<string, string> postProcess)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                item += "\r\n";

                text += postProcess(item);
            }

            return text;
        }

        public static string ToMultiLineList(this IEnumerable list, Func<string, bool, string> postProcess)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();
            var isLast = false;

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                item += "\r\n";

                if (x == count - 1)
                {
                    isLast = true;
                }

                text += postProcess(item, isLast);
            }

            return text;
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var index = 0;

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            return IndexOf<TSource>(source, item, new DefaultEqualityComparer<TSource>());
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> itemComparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var listOfT = source as IList<TSource>;

            if (listOfT != null)
            {
                return listOfT.IndexOf(item);
            }

            var list = source as IList;

            if (list != null)
            {
                return list.IndexOf(item);
            }

            var i = 0;

            foreach (TSource possibleItem in source)
            {
                if (itemComparer.Equals(item, possibleItem))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
