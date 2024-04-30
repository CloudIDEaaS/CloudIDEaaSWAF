using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils.WindowsSearch.Interfaces
{
    internal static class NativeMethods
    {
        private const string Ole32LibraryName = "ole32.dll";

        [DllImport(Ole32LibraryName, PreserveSig = false)]
        public static extern int CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, Int32 dwClsContext, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out ISearchManager diaDataSourceInterface);
    }
}
