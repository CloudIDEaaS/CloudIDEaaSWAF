using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Utils
{
    public abstract class DisposableBase<T> : IDisposable
    {
        public event EventHandlerT<T> Disposed;
        private EventHandlerT<T> disposed;
        public T InternalObject { get; private set; }

        public DisposableBase(T internalObject, EventHandlerT<T> disposed)
        {
            this.InternalObject = internalObject;
            this.disposed = disposed;
        }

        public void Dispose()
        {
            var args = new EventArgs<T>(this.InternalObject);

            disposed(this, args);

            if (Disposed != null)
            {
                Disposed.Raise(this, args.Value);
            }
        }
    }
}
