using System;

namespace Utils.ConditionPoints
{
    public class FanListSetting<T>
    {
        public T Start { get; }
        public T End { get; }
        public Func<T, T> GetNext { get; }

        public FanListSetting(T start, T end, Func<T, T> getNext)
        {
            this.Start = start;
            this.End = end;
            this.GetNext = getNext;
        }
    }
}