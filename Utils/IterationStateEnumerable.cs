using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class IterationStateEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<T> enumerable;
        private EnumeratorState<T> enumerator;
        public int CurrentIndex { get; set; }
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }
        public IEnumerable<T> Original { get; }
        public bool IsEnd { get; internal set; }

        public IterationStateEnumerable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
            this.Original = enumerable;
            this.CurrentIndex = -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerator == null)
            {
                enumerator = new EnumeratorState<T>(this, enumerable.Count());
            }

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (enumerator == null)
            {
                enumerator = new EnumeratorState<T>(this, enumerable.Count());
            }

            return enumerator;
        }
    }

    public class EnumeratorState<T> : IEnumerator<T>
    {
        private IterationStateEnumerable<T> enumerable;
        private int count;
        private int position;
        public T Current { get; private set; }
        object IEnumerator.Current => this.Current;

        public EnumeratorState(IterationStateEnumerable<T> enumerable, int count)
        {
            this.enumerable = enumerable;
            this.count = count;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            bool last;
            bool first;
            bool end;

            first = (position == 0);
            last = (position == count - 1);
            end = (position == count);

            enumerable.IsLast = last;
            enumerable.IsFirst = first;
            enumerable.IsEnd = end;

            if (end)
            {
                enumerable.CurrentIndex = -1;

                return false;
            }

            enumerable.CurrentIndex = position;
            this.Current = enumerable.Original.ElementAt(position);

            position++;

            return !end;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
