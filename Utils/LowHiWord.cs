using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils
{
    [StructLayout(LayoutKind.Explicit)]
    public struct LowHiWord
    {
        [FieldOffset(0)]
        public uint Number;
        [FieldOffset(0)]
        public ushort Low;
        [FieldOffset(2)]
        public ushort High;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct LowHiWordSigned
    {
        [FieldOffset(0)]
        public uint Number;
        [FieldOffset(0)]
        public short Low;
        [FieldOffset(2)]
        public short High;
    }
}
