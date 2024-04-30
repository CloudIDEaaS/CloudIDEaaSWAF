using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public static class ItemTrackingBindingListExtensions
    {
        public static Guid GetItemGuid<T>(this ListControl listControl, T item) where T : IComparable<T>
        {
            var list = (ItemTrackingBindingList<T>)listControl.DataSource;

            return list.GetGuidForItem(item);
        }
    }

    public class ItemTrackingBindingList<T> : BindingList<T> where T : IComparable<T>
    {
        private Dictionary<T, Guid> itemGuids;

        public ItemTrackingBindingList()
        {
        }

        public ItemTrackingBindingList(IList<T> list)
        {
            foreach (var item in list)
            {
                this.Add(item);
            }
        }

        public Guid GetGuidForItem(T item)
        {
            var guid = itemGuids[item];

            return guid;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (itemGuids == null)
            {
                itemGuids = new Dictionary<T, Guid>();
            }

            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        var item = this[e.NewIndex];

                        if (!itemGuids.ContainsKey(item))
                        {
                            itemGuids.Add(item, Guid.NewGuid());
                        }
                    }
                    break;
                case ListChangedType.Reset:
                    itemGuids.Clear();
                    break;
            }

            base.OnListChanged(e);
        }
    }
}
