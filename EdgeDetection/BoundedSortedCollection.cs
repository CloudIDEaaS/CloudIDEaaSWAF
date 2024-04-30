using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BitmapedTextures
{
    public interface ICustomCollection
    {
        void Add(Pixel item);
        Pixel GetItem(int index);
        void Block();

        int Count { get; }
    }

    class BoundedSortedCollection : ICustomCollection
    {
        private class Node
        {
            public Pixel pixel;
            public Node next;

            public Node(Pixel p)
            {
                pixel = p;
                next = null;
            }
        }

        private readonly int MAX;
        private Node head = null;
        private int count = 0;

        public bool IsReadOnly { get { return false; } }
        public int Count { get { return count; } }

        public void Add(Pixel item)
        {
            if (head == null)
            {
                head = new Node(item);
                count++;
            }
            else
            {
                Node current = head;

                while (current.next != null)
                {
                    if (item.Intensity > current.next.pixel.Intensity)
                    {
                        break;
                    }
                    current = current.next;
                }

                if (item != current.pixel)
                {
                    Node tmp = new Node(item);
                    if (current == head && item.Intensity > current.pixel.Intensity)
                    {
                        tmp.next = current;
                        head = tmp;
                    }
                    else
                    {
                        tmp.next = current.next;
                        current.next = tmp;
                    }
                    count++;

                    if (count > MAX)
                    {
                        while (current.next.next != null)
                        {
                            current = current.next;
                        }

                        current.next = null;
                        count--;
                    }
                }
            }
        }

        public void Clear()
        {
            count = 0;
            head = null;
        }

        public bool Contains(Pixel item)
        {
            Node current = head;
            bool result = false;

            while (current.next != null)
            {
                if (item.Intensity == current.pixel.Intensity)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool Remove(Pixel item)
        {
            Node current = head;
            bool result = false;

            while (current.next != null)
            {
                if (item.Intensity == current.pixel.Intensity)
                {
                    current.next = current.next.next;
                    break;
                }
            }

            return result;
        }

        public Pixel GetItem(int index)
        {
            if (index > count - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Node current = head;

            while (index-- > 0)
            {
                current = current.next;
            }

            return current.pixel;
        }

        public BoundedSortedCollection(int max)
        {
            MAX = max;
        }

        public void Display()
        {
            Node current = head;

            while (current != null)
            {
                Console.WriteLine(current.pixel);
                current = current.next;
            }
        }

        public void Block()
        {
            
        }
    }

    class IntensityStack : ICustomCollection
    {
        private Stack stack;
        private IList<Pixel> edges;

        public int Count { get { return edges.Count; } }

        public IntensityStack(int max)
        {
            stack = new Stack(max);
            edges = new List<Pixel>();
        }

        public void Add(Pixel item)
        {
            if (stack.Count == 0)
            {
                stack.Push(item);
                return;
            }

            Pixel popItem = stack.Pop() as Pixel;

            if (Math.Sign(popItem.Intensity) != Math.Sign(item.Intensity))
            {
                edges.Add(popItem);
                edges.Add(item);
            }
            else
            {
                stack.Push(popItem);
                stack.Push(item);
            }
        }

        public void Block()
        {
            stack.Clear();
        }

        public Pixel GetItem(int index)
        {
            return edges[index];
        }
    }
}
