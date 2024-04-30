using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Utils
{
    public class AdvanceableEnumeration<T> : IEnumerable<T>
    {
        private LinkedList<T> list;
        private LinkedListNode<T> current;

        public AdvanceableEnumeration()
        {
            list = new LinkedList<T>();
        }

        public AdvanceableEnumeration(IEnumerable<T> enumerable)
        {
            list = new LinkedList<T>(enumerable);
            current = list.First;
        }

        public T Current
        {
            get
            {
                if (current == null)
                {
                    return default(T);
                }
                else
                {
                    return current.Value;
                }
            }
        }

        public void Remove(T value)
        {
            var node = list.Remove(value);
        }

        public void AddFirst(T value, bool makeCurrent = false)
        {
            var node = list.AddFirst(value);
        }

        public void AddLast(T value, bool makeCurrent = false)
        {
            var node = list.AddLast(value);

            if (makeCurrent)
            {
                current = node;
            }
        }

        public T Advance()
        {
            current = current.Next;

            return this.Current;
        }

        public T Rewind()
        {
            current = current.Previous;

            return this.Current;
        }

        public bool CanRewind
        {
            get
            {
                return current.Previous != null;
            }
        }

        public bool CanAdvance
        {
            get
            {
                return current.Next != null;
            }
        }

        public T AdvanceUntil(Func<T, bool> until)
        {
            if (until != null)
            {
                while (current != null)
                {
                    if (until(this.Current))
                    {
                        break;
                    }

                    current = current.Next;
                }
            }

            return this.Current;
        }

        public T RewindUntil(Func<T, bool> until = null)
        {
            if (until != null)
            {
                while (current != null)
                {
                    if (until(this.Current))
                    {
                        break;
                    }

                    current = current.Previous;
                }
            }

            return this.Current;
        }

        public T AdvanceWhile(Func<T, bool> _while)
        {
            if (_while != null)
            {
                while (current != null)
                {
                    if (!_while(this.Current))
                    {
                        break;
                    }

                    current = current.Next;
                }
            }

            return this.Current;
        }

        public T RewindWhile(Func<T, bool> _while = null)
        {
            if (_while != null)
            {
                while (current != null)
                {
                    if (!_while(this.Current))
                    {
                        break;
                    }

                    current = current.Previous;
                }
            }

            return this.Current;
        }

        public void Reset()
        {
            this.current = list.First;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
