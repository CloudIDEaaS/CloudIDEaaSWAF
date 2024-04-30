using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    public static class EventExtensions
    {
        public static bool HasHandler<TEventArgValue>(this EventHandler<EventArgs<TEventArgValue>> handler)
        {
            return handler != null;

        }
        public static bool HasHandler<TEventArgValue>(this EventHandlerT<TEventArgValue> handler)
        {
            return handler != null;
        }

        public static void Raise<TEventArgValue>(this EventHandler<EventArgs<TEventArgValue>> handler, object sender, TEventArgValue value)
        {
            if (handler != null)
            {
                handler.Invoke(sender, new EventArgs<TEventArgValue>(value));
            }
        }

        public static void Raise<TEventArgValue>(this EventHandlerT<TEventArgValue> handler, object sender, TEventArgValue value)
        {
            if (handler != null)
            {
                handler.Invoke(sender, new EventArgs<TEventArgValue>(value));
            }
        }

        public static void Raise(this EventHandlerT<string> handler, object sender, string format, params object[] args)
        {
            if (handler != null)
            {
                var value = string.Format(format, args);

                handler.Invoke(sender, new EventArgs<string>(value));
            }
        }

        public static TEventArgValue RaiseGet<TEventArgValue>(this EventHandlerT<TEventArgValue> handler, object sender)
        {
            if (handler != null)
            {
                var args = new EventArgs<TEventArgValue>(default(TEventArgValue));

                handler.Invoke(sender, args);

                return args.Value;
            }

            return default(TEventArgValue);
        }

        public static TEventArgValue RaiseGet<TEventArgValue>(this EventHandlerT<TEventArgValue> handler, object sender, TEventArgValue value)
        {
            if (handler != null)
            {
                var args = new EventArgs<TEventArgValue>(value);

                handler.Invoke(sender, args);

                return args.Value;
            }

            return default(TEventArgValue);
        }
    }
}
