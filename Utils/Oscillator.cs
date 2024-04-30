using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Utils
{
    public abstract class BaseOscillator
    {
        public abstract bool Test();
        public abstract void Reset();
        public bool HasPulsed { get; protected set; }
    }

    public abstract class Oscillator<T> : BaseOscillator
    {
        protected T current;
        public event EventHandler<EventArgs<T>> OnPulse;
        public abstract bool Test(out T currentValue);
        public override abstract bool Test();
        public override abstract void Reset();

        protected void RaiseOnPulse(EventArgs<T> args)
        {
            if (OnPulse != null)
            {
                OnPulse(this, args);
            }
        }
    }

    public class NotifyOscillator<TItemType> : Oscillator<OscillatorNotityItem<TItemType>>
    {
        private Action<OscillatorNotityItem<TItemType>> action;

        public NotifyOscillator(Action<OscillatorNotityItem<TItemType>> action)
        {
            this.OnPulse += (sender, e) =>
            {
                action(e.Value);
            };
        }

        internal void RaiseNotification(EventArgs<OscillatorNotityItem<TItemType>> args)
        {
            this.RaiseOnPulse(args);
        }

        public override bool Test(out OscillatorNotityItem<TItemType> currentValue)
        {
            throw new NotImplementedException();
        }

        public override bool Test()
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class TimeOscillator : Oscillator<DateTime>
    {
        private TimeSpan timeSpan;
        private Action<DateTime> action;

        public TimeOscillator(TimeSpan timeSpan, Action<DateTime> action) : this(timeSpan)
        {
            this.action = action;
        }

        public TimeOscillator(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
            Reset();
        }

        public override bool Test(out DateTime currentValue)
        {
            currentValue = DateTime.MinValue;

            if (Test())
            {
                currentValue = current;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Test()
        {
            var now = DateTime.Now;

            if (now - current >= timeSpan)
            {
                current = now;

                if (action != null)
                {
                    action(current);
                }

                RaiseOnPulse(new EventArgs<DateTime>(current));
                this.HasPulsed = true;

                return true;
            }
            else
            {
                this.HasPulsed = false;

                return false;
            }
        }

        public override void Reset()
        {
            this.current = DateTime.Now;
        }
    }

    public class CountOscillator : Oscillator<int>
    {
        private int count;
        private Action<int> action;

        public CountOscillator(int count, Action<int> action) : this(count)
        {
            this.action = action;
        }

        public CountOscillator(int count)
        {
            this.count = count;
        }

        public override bool Test(out int currentValue)
        {
            currentValue = -1;

            if (Test())
            {
                currentValue = current;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Test()
        {
            var modValue = (++current).ScopeRange(count);

            if (modValue == count)
            {
                if (action != null)
                {
                    action(current);
                }

                RaiseOnPulse(new EventArgs<int>(current));
                this.HasPulsed = true;

                return true;
            }
            else
            {
                this.HasPulsed = false;

                return false;
            }
        }

        public override void Reset()
        {
            current = 0;
        }
    }

    public static class OscillatorExtensions
    {
        public static bool Oscillate(this bool loopTest, BaseOscillator oscillator)
        {
            oscillator.Test();

            return loopTest;
        }

        public static IEnumerable<T> Oscillate<T>(this IEnumerable<T> enumerable, BaseOscillator oscillator)
        {
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;

                oscillator.Test();

                yield return current;
            }
        }

        public static IEnumerable<T> Oscillate<T>(this IEnumerable<T> enumerable, NotifyOscillator<T> oscillator)
        {
            var enumerator = enumerable.GetEnumerator();
            var item = new OscillatorNotityItem<T>(OscillatorNotifyType.Begin, default(T));

            oscillator.RaiseNotification(new EventArgs<OscillatorNotityItem<T>>(item));

            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;

                item = new OscillatorNotityItem<T>(OscillatorNotifyType.Next, current);
                oscillator.RaiseNotification(new EventArgs<OscillatorNotityItem<T>>(item));

                yield return current;
            }

            item = new OscillatorNotityItem<T>(OscillatorNotifyType.End, default(T));
            oscillator.RaiseNotification(new EventArgs<OscillatorNotityItem<T>>(item));
        }
    }
}
