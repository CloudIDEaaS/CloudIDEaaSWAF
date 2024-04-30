using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Utils.TextObjectModel;

namespace Utils.TextObjectModel
{
    public class StoryRanges : BaseList<ITextRange>, ITextStoryRanges
    {
        private Dictionary<string, int> namedRanges;

        public StoryRanges()
        {
            namedRanges = new Dictionary<string, int>();
        }

        public ITextRange this[string key]
        {
            get 
            {
                if (namedRanges.ContainsKey(key))
                {
                    var index = namedRanges[key];

                    return internalList[index];
                }
                else
                {
                    e.Throw<IndexOutOfRangeException>("No stories with key: {0}", key);
                    return null;
                }
            }
        }

        public void Add(string key, TextRange textRange)
        {
            int index;

            textRange.NamedRange = key;

            internalList.Add(textRange);

            index = internalList.Cast<TextRange>().IndexOf(r => r.NamedRange == key);

            namedRanges.Add(key, index);
        }
    }
}
