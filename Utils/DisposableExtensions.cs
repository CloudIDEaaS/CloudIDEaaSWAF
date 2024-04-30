using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class DisposableHandler<T> : DisposableBase<T>
    {
        public DisposableHandler(T obj, EventHandlerT<T> disposed) : base(obj, disposed)
        {
        }
    }

    public static class DisposableExtensions
    {
        public static IDisposable CreateDisposable<T>(this T obj, EventHandlerT<T> disposed)
        {
            return new DisposableHandler<T>(obj, disposed);
        }

        public static IDisposable CreateDisposable<T>(this T obj, Action disposed)
        {
            return new DisposableHandler<T>(obj, new EventHandlerT<T>((s, e) => disposed()));
        }
    }
}
