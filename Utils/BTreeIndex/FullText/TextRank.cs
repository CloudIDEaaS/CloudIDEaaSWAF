using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.BTreeIndex.FullText
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class TextRank<T> where T : IComparable<T>
    {
        public int Occurences { get; set; }
        public string Text { get; set; }
        public T Data { get; }
        public List<TextRank<T>> Children { get; }

        public TextRank(string text, T data)
        {
            this.Text = text;
            this.Data = data;
            this.Occurences = 1;
            this.Children = new List<TextRank<T>>();
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("{0}, Hash:{1}", this.Data.ToString(), this.GetHashCode());
            }
        }
    }
}
