using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CheckedListBoxItem<T> : IComparable<CheckedListBoxItem<T>>
    {
        public T DisplayItem { get; set; }
        public bool Checked { get; set; }

        public CheckedListBoxItem(T displayItem, bool _checked)
        {
            this.DisplayItem = displayItem;
            this.Checked = _checked;
        }

        public int CompareTo(CheckedListBoxItem<T> other)
        {
            return this.DisplayItem.ToString().CompareTo(other.DisplayItem.ToString());
        }
    }
}
