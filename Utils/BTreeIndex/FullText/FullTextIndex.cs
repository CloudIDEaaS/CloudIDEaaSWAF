using BTreeIndex.Collections.Generic.BTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.BTreeIndex.FullText
{
    public class FullTextIndex<T> : BTreeDictionary<string, List<TextRank<T>>> where T : IComparable<T>
    {
        private IManagedLockObject lockObject;

        public FullTextIndex()
        {
            this.lockObject =  LockManager.CreateObject();
        }

        public void AddUri(Uri uri, T data)
        {
            if (uri == null)
            {
                return;
            }

            if (data == null)
            {
                DebugUtils.Break();
            }

            try
            {
                var wordGroups = uri.GetAllWordGroups();

                using (lockObject.Lock())
                {
                    foreach (var wordGroup in wordGroups)
                    {
                        List<TextRank<T>> list;

                        if (this.ContainsKey(wordGroup))
                        {
                            list = this[wordGroup];

                            if (list != null)
                            {
                                if (list.Any(i => i.Data.Equals(data)))
                                {
                                    var textRank = list.Single(i => i.Data.Equals(data));

                                    textRank.Occurences++;
                                }
                                else
                                {
                                    list.Add(new TextRank<T>(wordGroup, data));
                                }
                            }
                        }
                        else
                        {
                            list = new List<TextRank<T>>();

                            list.Add(new TextRank<T>(wordGroup, data));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void AddText(string text, T data)
        {
            if (text.IsNullOrEmpty())
            {
                return;
            }

            if (data == null)
            {
                DebugUtils.Break();
            }

            try
            {
                var wordGroups = text.GetAllWordGroups();

                using (lockObject.Lock())
                {
                    foreach (var wordGroup in wordGroups)
                    {
                        List<TextRank<T>> list;

                        if (this.ContainsKey(wordGroup))
                        {
                            list = this[wordGroup];

                            if (list != null)
                            {
                                if (list.Any(i => i.Data.Equals(data)))
                                {
                                    var textRank = list.Single(i => i.Data.Equals(data));

                                    textRank.Occurences++;
                                }
                                else
                                {
                                    list.Add(new TextRank<T>(wordGroup, data));
                                }
                            }
                        }
                        else
                        {
                            list = new List<TextRank<T>>();

                            list.Add(new TextRank<T>(wordGroup, data));

                            this.Add(wordGroup, list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public new IEnumerable<TextRank<T>> Search(string key)
        {
            List<TextRank<T>> list;

            using (lockObject.Lock())
            {
                if (this.ContainsKey(key))
                {
                    list = this[key];

                    return list.OrderByDescending(r => r.Occurences);
                }
            }

            return new List<TextRank<T>>();
        }
    }
}
