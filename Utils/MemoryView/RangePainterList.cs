using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Collections.Specialized;

namespace Utils.MemoryView
{
    public class RangePainterList : BaseList<RangePainter>
    {
        private Dictionary<string, RangePainter> painters;
        private string lockedPainterName;

        public RangePainterList(Dictionary<string, RangePainter> painters)
        {
            this.painters = painters;
        }

        public override void Add(RangePainter item)
        {
            using (var notifier = this.Notify(NotifyCollectionChangedAction.Add, item))
            {
                painters.Add(item.Name, item);
            }
        }

        public override RangePainter this[int index]
        {
            get
            {
                return painters.Values.ElementAt(index);
            }

            set
            {
                e.Throw<NotImplementedException>("Cannot change RangePainter via index");
            }
        }

        public RangePainter this[string name]
        {
            get
            {
                return painters[name];
            }
        }

        public bool ContainsKey(string name)
        {
            return painters.ContainsKey(name);
        }

        public override void Clear()
        {
            using (var notifier = this.Notify(NotifyCollectionChangedAction.Reset))
            {
                foreach (var painter in this.painters.ToList())
                {
                    if (painter.Key != lockedPainterName)
                    {
                        painters.Remove(painter.Key);
                    }
                }
            }
        }

        public override bool Contains(RangePainter item)
        {
            return painters.ContainsValue(item);
        }

        public override bool Remove(RangePainter item)
        {
            using (var notifier = this.Notify(NotifyCollectionChangedAction.Remove))
            {
                return painters.Remove(item.Name);
            }
        }

        public override IEnumerator<RangePainter> GetEnumerator()
        {
            return painters.Values.GetEnumerator();
        }

        public void LockFromClear(string painterName)
        {
            lockedPainterName = painterName;
        }
    }
}
