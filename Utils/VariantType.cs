using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Utils
{
    public static class VariantExtensions 
    {
        public unsafe static object GetObjectForVariant(this IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                var pArg = (VARIANTARG*)ptr;
                var obj = Marshal.GetObjectForNativeVariant(ptr);

                if (obj == null)
                {
                }

                return obj;
            }
            else
            {
               return null;
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct VARIANTARG
    {
        [FieldOffset(0)]
        public VariantType vt;

        [FieldOffset(2)]
        public ushort wReserved1;

        [FieldOffset(4)]
        public ushort wReserved2;

        [FieldOffset(6)]
        public ushort wReserved3;

        [FieldOffset(8)]
        public long llVal;

        [FieldOffset(8)]
        public int lVal;

        [FieldOffset(8)]
        public byte bVal;

        [FieldOffset(8)]
        public short iVal;

        [FieldOffset(8)]
        public float fltVal;

        [FieldOffset(8)]
        public double dblVal;

        [FieldOffset(8)]
        public short boolVal;

        [FieldOffset(8)]
        public int scode;

        //[FieldOffset(8)]
        //public CY cyVal;

        [FieldOffset(8)]
        public double date;

        [FieldOffset(8)]
        public unsafe ushort* bstrVal;

        [FieldOffset(8)]
        public unsafe IntPtr punkVal;

        [FieldOffset(8)]
        public unsafe IntPtr pdispVal;

        [FieldOffset(8)]
        public unsafe IntPtr parray;

        [FieldOffset(8)]
        public unsafe byte* pbVal;

        [FieldOffset(8)]
        public unsafe short* piVal;

        [FieldOffset(8)]
        public unsafe int* plVal;

        [FieldOffset(8)]
        public unsafe long* pllVal;

        [FieldOffset(8)]
        public unsafe float* pfltVal;

        [FieldOffset(8)]
        public unsafe double* pdblVal;

        [FieldOffset(8)]
        public unsafe short* pboolVal;

        [FieldOffset(8)]
        public unsafe int* pscode;

        [FieldOffset(8)]
        public unsafe IntPtr pcyVal;

        [FieldOffset(8)]
        public unsafe double* pdate;

        [FieldOffset(8)]
        public unsafe ushort** pbstrVal;

        [FieldOffset(8)]
        public unsafe IntPtr ppunkVal;

        [FieldOffset(8)]
        public unsafe IntPtr ppdispVal;

        [FieldOffset(8)]
        public unsafe IntPtr pparray;

        [FieldOffset(8)]
        public unsafe VARIANTARG* pvarVal;

        [FieldOffset(8)]
        public unsafe void* byref;

        [FieldOffset(8)]
        public sbyte cVal;

        [FieldOffset(8)]
        public ushort uiVal;

        [FieldOffset(8)]
        public uint ulVal;

        [FieldOffset(8)]
        public ulong ullVal;

        [FieldOffset(8)]
        public int intVal;

        [FieldOffset(8)]
        public uint uintVal;

        [FieldOffset(8)]
        public unsafe sbyte* pcVal;

        [FieldOffset(8)]
        public unsafe ushort* puiVal;

        [FieldOffset(8)]
        public unsafe uint* pulVal;

        [FieldOffset(8)]
        public unsafe ulong* pullVal;

        [FieldOffset(8)]
        public unsafe int* pintVal;

        [FieldOffset(8)]
        public unsafe uint* puintVal;

        [FieldOffset(8)]
        public unsafe void* pvRecord;
    }

    public enum VariantType : ushort
    {
        EMPTY = 0,
        NULL = 1,
        [DotNetType(DotNetType.ShortType)] I2 = 2,
        [DotNetType(DotNetType.IntType)] I4 = 3,
        [DotNetType(DotNetType.FloatType)] R4 = 4,
        R8 = 5,
        CY = 6,
        DATE = 7,
        [DotNetType(DotNetType.StringType)] BSTR = 8,
        [DotNetType(DotNetType.ObjectType)] DISPATCH = 9,
        ERROR = 10,
        [DotNetType(DotNetType.BoolType)] BOOL = 11,
        [DotNetType(DotNetType.ObjectType)] VARIANT = 12,
        UNKNOWN = 13,
        DECIMAL = 14,
        [DotNetType(DotNetType.ByteType)] I1 = 16,
        UI1 = 17,
        UI2 = 18,
        UI4 = 19,
        I8 = 20,
        UI8 = 21,
        [DotNetType(DotNetType.IntType)] INT = 22,
        UINT = 23,
        [DotNetType(DotNetType.ObjectType)] VOID = 24,
        HRESULT = 25,
        [DotNetType(DotNetType.ObjectType)] PTR = 26,
        SAFEARRAY = 27,
        CARRAY = 28,
        USERDEFINED = 29,
        LPSTR = 30,
        LPWSTR = 31,
        RECORD = 36,
        INT_PTR = 37,
        UINT_PTR = 38,
        FILETIME = 64,
        BLOB = 65,
        STREAM = 66,
        STORAGE = 67,
        STREAMED_OBJECT = 68,
        STORED_OBJECT = 69,
        BLOB_OBJECT = 70,
        CF = 71,
        CLSID = 72,
        VERSIONED_STREAM = 73,
        BSTR_BLOB = 0xfff,
        VECTOR = 0x1000,
        ARRAY = 0x2000,
        BYREF = 0x4000,
        RESERVED = 0x8000,
        ILLEGAL = 0xffff,
        ILLEGALMASKED = 0xfff,
        TYPEMASK = 0xfff
    }
}
