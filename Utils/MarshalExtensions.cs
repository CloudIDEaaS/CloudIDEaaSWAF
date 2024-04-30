using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace Utils
{
    public class MarshalledStream
    {
        public IStream Stream { get; private set; }
        public Thread MarshalledFromThread { get; private set; }
        public object MarshalledObject { get; private set; }

        public MarshalledStream(IStream stream, object marshalledObject)
        {
            this.Stream = stream;
            this.MarshalledFromThread = Thread.CurrentThread;
            this.MarshalledObject = marshalledObject;
        }
    }

    public static class MarshalExtensions
    {
        public static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        [DllImport("ole32.dll")]
        private static extern int CoMarshalInterThreadInterfaceInStream([In] ref Guid rIID_IUnknown, [MarshalAs(UnmanagedType.IUnknown)] object pUnk, out IStream ppStm);
        [DllImport("ole32.dll")]
        private static extern int CoGetInterfaceAndReleaseStream(IStream pStm, [In] ref Guid rIID_IUnknown, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

        public static MarshalledStream Marshal(this object obj)
        {
            IStream stream;
            var hr = CoMarshalInterThreadInterfaceInStream(ref IID_IUnknown, obj, out stream);

            if (hr != 0)
            {
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            }

            return new MarshalledStream(stream, obj);
        }

        public static T Unmarshal<T>(this MarshalledStream stream)
        {
            return Unmarshal<T>(stream.Stream);
        }

        public static T Unmarshal<T>(this IStream stream)
        {
            object obj;
            var hr = CoGetInterfaceAndReleaseStream(stream, ref IID_IUnknown, out obj);

            if (hr != 0)
            {
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            }

            return (T)obj;
        }

        public static object Unmarshal(this MarshalledStream stream)
        {
            object obj;
            var hr = CoGetInterfaceAndReleaseStream(stream.Stream, ref IID_IUnknown, out obj);

            if (hr != 0)
            {
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            }

            return (object)obj;
        }

        public static void RunAsMarshalled<T>(this MarshalledStream stream, Action<T> action)
        {
            if (Thread.CurrentThread.ManagedThreadId == stream.MarshalledFromThread.ManagedThreadId)
            {
                action((T) stream.MarshalledObject);
            }
            else
            {
                var obj = Unmarshal<T>(stream.Stream);

                action((T)obj);
            }
        }
    }
}
