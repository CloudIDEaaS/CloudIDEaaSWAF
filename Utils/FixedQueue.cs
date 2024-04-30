using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;

namespace Utils
{
    public class FixedQueue<T> : Queue<T>
    {
        private int maxCount;

        public FixedQueue(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public new void Enqueue(T item)
        {
            if (this.Count >= maxCount)
            {
                base.Dequeue();
            }

            base.Enqueue(item);
        }
    }
}
