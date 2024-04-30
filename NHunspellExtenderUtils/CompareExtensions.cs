using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class CompareExtensions
    {

        public static bool IsBetween<T>(this T value, T lower, T upper, bool inclusive = true) where T : IComparable
        {
            if (typeof(T) == typeof(byte))
            {
                var byteValue = (byte)(object)value;
                var byteUpper = (byte)(object)upper;
                var byteLower = (byte)(object)lower;

                if (inclusive)
                {
                    return byteValue >= byteLower && byteValue <= byteUpper;
                }
                else
                {
                    return byteValue > byteLower && byteValue < byteUpper;
                }
            }
            else if (typeof(T) == typeof(IntPtr))
            {
                var IntPtrValue = (IntPtr)(object)value;
                var IntPtrUpper = (IntPtr)(object)upper;
                var IntPtrLower = (IntPtr)(object)lower;

                if (inclusive)
                {
                    return ((int)IntPtrValue) >= ((int)IntPtrLower) && ((int)IntPtrValue) <= ((int)IntPtrUpper);
                }
                else
                {
                    return ((int)IntPtrValue) > ((int)IntPtrLower) && ((int)IntPtrValue) < ((int)IntPtrUpper);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                var intValue = (int)(object)value;
                var intUpper = (int)(object)upper;
                var intLower = (int)(object)lower;

                if (inclusive)
                {
                    return intValue >= intLower && intValue <= intUpper;
                }
                else
                {
                    return intValue > intLower && intValue < intUpper;
                }
            }
            else if (typeof(T) == typeof(long))
            {
                var longValue = (long)(object)value;
                var longUpper = (long)(object)upper;
                var longLower = (long)(object)lower;

                if (inclusive)
                {
                    return longValue >= longLower && longValue <= longUpper;
                }
                else
                {
                    return longValue > longLower && longValue < longUpper;
                }
            }
            else if (typeof(T) == typeof(uint))
            {
                var uintValue = (uint)(object)value;
                var uintUpper = (uint)(object)upper;
                var uintLower = (uint)(object)lower;

                if (inclusive)
                {
                    return uintValue >= uintLower && uintValue <= uintUpper;
                }
                else
                {
                    return uintValue > uintLower && uintValue < uintUpper;
                }
            }
            else if (typeof(T) == typeof(ulong))
            {
                var ulongValue = (ulong)(object)value;
                var ulongUpper = (ulong)(object)upper;
                var ulongLower = (ulong)(object)lower;

                if (inclusive)
                {
                    return ulongValue >= ulongLower && ulongValue <= ulongUpper;
                }
                else
                {
                    return ulongValue > ulongLower && ulongValue < ulongUpper;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                var floatValue = (float)(object)value;
                var floatUpper = (float)(object)upper;
                var floatLower = (float)(object)lower;

                if (inclusive)
                {
                    return floatValue >= floatLower && floatValue <= floatUpper;
                }
                else
                {
                    return floatValue > floatLower && floatValue < floatUpper;
                }
            }
            else if (typeof(T) == typeof(char))
            {
                var charValue = (char)(object)value;
                var charUpper = (char)(object)upper;
                var charLower = (char)(object)lower;

                if (inclusive)
                {
                    return charValue >= charLower && charValue <= charUpper;
                }
                else
                {
                    return charValue > charLower && charValue < charUpper;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                var doubleValue = (double)(object)value;
                var doubleUpper = (double)(object)upper;
                var doubleLower = (double)(object)lower;

                if (inclusive)
                {
                    return doubleValue >= doubleLower && doubleValue <= doubleUpper;
                }
                else
                {
                    return doubleValue > doubleLower && doubleValue < doubleUpper;
                }
            }
            else if (typeof(T).IsEnum)
            {
                var enumValue = (Enum)(object)value;
                var enumUpper = (Enum)(object)upper;
                var enumLower = (Enum)(object)lower;

                if (inclusive)
                {
                    return enumLower.CompareTo(enumValue) <= 0 && enumUpper.CompareTo(enumValue) >= 0;
                }
                else
                {
                    return enumLower.CompareTo(enumValue) < 0 && enumUpper.CompareTo(enumValue) > 0;
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                var dateValue = (DateTime)(object)value;
                var dateUpper = (DateTime)(object)upper;
                var dateLower = (DateTime)(object)lower;

                if (inclusive)
                {
                    return dateValue >= dateLower && dateValue <= dateUpper;
                }
                else
                {
                    return dateValue > dateLower && dateValue < dateUpper;
                }
            }

            throw new NotImplementedException();
        }
    }
}
