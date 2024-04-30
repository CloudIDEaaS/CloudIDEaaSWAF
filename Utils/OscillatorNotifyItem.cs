using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public enum OscillatorNotifyType
    {
        Begin,
        Next,
        End
    }

    public class OscillatorNotityItem<T>
    {
        public OscillatorNotifyType NotifyType { get; private set; }
        public T Item { get; private set; }

        public OscillatorNotityItem(OscillatorNotifyType notifyType, T item)
        {
            this.NotifyType = notifyType;
            this.Item = item;
        }
    }
}
