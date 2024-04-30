using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class DisposableHandle : IDisposable
    {
        public event EventHandlerT<IntPtr> Disposed;
        public IntPtr Handle { get; private set; }

        public DisposableHandle(IntPtr handle)
        {
            this.Handle = handle;
        }

        public static implicit operator IntPtr(DisposableHandle handle)
        {
            return handle.Handle;
        }

        public void Dispose()
        {
            Disposed.Raise(this, this.Handle);
        }
    }
}
