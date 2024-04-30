using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices; 

namespace Utils
{
#if !UTILS_INTERNAL
    public class ArrayBuilder<TElement>
#else
    internal class ArrayBuilder<TElement>
#endif
    {
        public TElement[] Data { get; private set; }

        public ArrayBuilder(TElement[] data)
        {
            this.Data = data;
        }

        public static ArrayBuilder<TElement> operator +(TElement left, ArrayBuilder<TElement> right)
        {
            var destination = new TElement[1] { left };

            return new ArrayBuilder<TElement>(destination.Append(right.Data));
        }

        public static ArrayBuilder<TElement> operator +(ArrayBuilder<TElement> left, TElement right)
        {
            var append = new TElement[1] { right };

            return new ArrayBuilder<TElement>(left.Data.Append(append));
        }
    }

#if !UTILS_INTERNAL
    public static class ArrayExtensions
#else
    internal static class ArrayExtensions
#endif
    {
#if !SILVERLIGHT
        [DllImport("msvcrt.dll", EntryPoint = " ", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public unsafe static extern IntPtr MemSet(byte* dest, int c, int count);

        public static unsafe float[] ToFloatArray(this byte[] bytes, int readCount)
        {
            var floatCount = readCount / 4;
            var ptr = stackalloc float[floatCount];
            var nPtr = (IntPtr)ptr;
            var floatArray = new float[floatCount];

            Marshal.Copy(bytes, 0, nPtr, readCount);
            Marshal.Copy(nPtr, floatArray, 0, floatCount);

            return floatArray;
        }

        public static void Xor(this byte[] bytes, byte[] with)
        {
            var max = Math.Min(bytes.Length, with.Length);

            for (var x = 0; x < max; x++)
            {
                bytes[x] ^= with[x];
            }
        }

        public unsafe static void MemSet(this byte[] array, byte _byte, int count)
        {
            fixed (byte* ptr = &array[0])
            {
                MemSet(ptr, _byte, count);
            }
        }

        public static string ToBase64(this string str)
        {
            return Convert.ToBase64String(str.ToArray());
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64ToString(this string str)
        {
            return Convert.FromBase64String(str).ToText();
        }

        public static byte[] FromBase64(this string str)
        {
            return Convert.FromBase64String(str);
        }

        public static void MemSet(this byte[] array, byte _byte)
        {
            array.MemSet(_byte, array.Length);
        }
#endif
        public static bool AllAreSetTo(this byte[] array, byte _TElement, int startingIndex = 0)
        {
            return !array.Skip(startingIndex).Any(b => b != _TElement);
        }

        public static bool AllAreZero(this byte[] array, int startingIndex = 0)
        {
            return array.AllAreSetTo(0, startingIndex);
        }

        public static ArrayBuilder<TElement> ToBuilder<TElement>(this TElement[] data)
        {
            return new ArrayBuilder<TElement>(data);
        }

        public static TElement[] TrimRight<TElement>(this TElement[] data, int length)
        {
            return data.Copy(data.Length - length);
        }

        public static TElement[] PadRight<TElement>(this TElement[] data, int length)
        {
            return data.Append(new TElement[length - data.Length]);
        }

        public static TElement[] ExpandRight<TElement>(this TElement[] data, int length = 1)
        {
            var result = new TElement[data.Length + length];

            Array.Copy(data, result, data.Length);

            return result;
        }

        public static TElement[] ExpandLeft<TElement>(this TElement[] data, int length = 1)
        {
            var result = new TElement[data.Length + length];

            Array.Copy(data, 0, result, length, data.Length);

            return result;
        }

        public static TElement[] RemoveLeft<TElement>(this TElement[] data, int length = 1)
        {
            var result = new TElement[data.Length - length];

            Array.Copy(data, length, result, 0, data.Length - length);

            return result;
        }

        public static TElement[] Copy<TElement>(this TElement[] data, int length = -1)
        {
            var result = new TElement[length == -1 ? data.Length : length];

            if (length == -1)
            {
                Array.Copy(data, result, data.Length);
            }
            else
            {
                Array.Copy(data, result, length);
            }

            return result;
        }

        public static TElement[] Prepend<TElement>(this TElement[] destination, TElement _TElement)
        {
            var start = destination.Length;
            var source = new TElement[1];

            destination = destination.ExpandLeft(1);
            source[0] = _TElement;

            Array.Copy(source, 0, destination, 0, source.Length);

            return destination;
        }

        public static TElement[] Append<TElement>(this TElement[] destination, TElement _TElement)
        {
            var start = destination.Length;
            var source = new TElement[1];

            destination = destination.ExpandRight(1);
            source[0] = _TElement;

            Array.Copy(source, 0, destination, start, source.Length);

            return destination;
        }

        public static TElement[] Append<TElement>(this TElement[] destination, TElement[] source)
        {
            int start;

            if (destination == null)
            {
                destination = new TElement[0];
            }

            start = destination.Length;

            destination = destination.ExpandRight(source.Length);

            Array.Copy(source, 0, destination, start, source.Length);

            return destination;
        }

        public static byte[] FromHex(this string text)
        {
            var bytes = new List<byte>();

            for (var x = 0; x < text.Length; x += 2)
            {
                var byteText = text.Substring(x, 2);
                var _byte = byte.Parse(byteText, System.Globalization.NumberStyles.HexNumber);

                bytes.Add(_byte);
            }

            return bytes.ToArray();
        }

        public static string FromHexToString(this string text)
        {
            var bytes = new List<byte>();

            for (var x = 0; x < text.Length; x += 2)
            {
                var byteText = text.Substring(x, 2);
                var _byte = byte.Parse(byteText, System.Globalization.NumberStyles.HexNumber);

                bytes.Add(_byte);
            }

            return ASCIIEncoding.ASCII.GetString(bytes.ToArray());
        }

        public static byte[] ToBytes(this string text)
        {
            return UTF8Encoding.UTF8.GetBytes(text);
        }

        public static string ToText(this byte[] bytes)
        {
            return UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
