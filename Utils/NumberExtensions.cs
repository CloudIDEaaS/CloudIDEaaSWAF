using System;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Utils
{
    public static class NumberExtensions
    {
        public static int KB = 1024;
        public static int MB = KB * 1024;
        public static int GB = MB * 1024;
        public static int TB = GB * 1024;
        public static int PB = TB * 1024;
        public static int ZB = PB * 1024;
        public static int EB = ZB * 1024;

        public static string ToChangeText(this float f)
        {
            if (f == 0)
            {
                return f.ToString();
            }
            if (f.IsNegative())
            {
                return f.ToString();
            }
            else
            {
                return "+" + f.ToString();
            }
        }

        public static float ToWholePercentage(this float f)
        {
            f = (float) Math.Round(f * 100, 0);

            return f;
        }

        public static bool IsNegative(this float f)
        {
            return Math.Abs(f) != f;
        }

        public static int Half(this int n)
        {
            return (int)(n.As<float>() / 2f);
        }

        public static int GetPrecision<T>(this T input) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var fullInput = input.ToFullString();
            var number = Decimal.Parse(fullInput);
            var precision = (Decimal.GetBits(number)[3] >> 16) & 0x000000FF;

            return precision;
        }

        public static int ToKb(this long n)
        {
            return (int)Math.Round(n.As<float>() / NumberExtensions.KB.As<float>(), 0);
        }

        public static int ToMB(this long n)
        {
            return (int)Math.Round(n.As<float>() / NumberExtensions.MB.As<float>(), 0);
        }

        public static long KbToBytes(this long n)
        {
            return n * NumberExtensions.KB;
        }

        public static long MBToBytes(this long n)
        {
            return n * NumberExtensions.MB;
        }

        public static int MaxAt(this int n, int max)
        {
            return Math.Min(n, max);
        }

        public static int MinAt(this int n, int max)
        {
            return Math.Max(n, max);
        }

        public static string ToFullString<T>(this T value) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            string[] valueExpSplit;
            string result;
            string decimalSeparator;
            int indexOfDecimalSeparator;
            int exp;
            var formatInfo = CultureInfo.InvariantCulture.NumberFormat;

            if (typeof(T).Name.IsOneOf("Single", "Double", "BitInteger"))
            {
                valueExpSplit = value.ToString("r", formatInfo).ToUpper().Split(new char[] { 'E' });
            }
            else
            {
                valueExpSplit = value.ToString(formatInfo).ToUpper().Split(new char[] { 'E' });
            }

            if (valueExpSplit.Length > 1)
            {
                result = valueExpSplit[0];
                exp = int.Parse(valueExpSplit[1]);
                decimalSeparator = formatInfo.NumberDecimalSeparator;

                if ((indexOfDecimalSeparator = valueExpSplit[0].IndexOf(decimalSeparator)) > -1)
                {
                    exp -= (result.Length - indexOfDecimalSeparator - 1);
                    result = result.Replace(decimalSeparator, "");
                }

                if (exp >= 0)
                {
                    result += new string('0', Math.Abs(exp));
                }
                else
                {
                    exp = Math.Abs(exp);

                    if (exp >= result.Length)
                    {
                        result = "0." + new string('0', exp - result.Length) + result;
                    }
                    else
                    {
                        result = result.Insert(result.Length - exp, decimalSeparator);
                    }
                }
            }
            else
            {
                result = valueExpSplit[0];
            }

            return result;
        }

        public static int ToBigEndian(this int n)
        {
            var lengthStream = new MemoryStream(BitConverter.GetBytes(n).Reverse<byte>().ToArray<byte>());

            return new BinaryReader(lengthStream).ReadInt32();
        }

        public static uint ToBigEndian(this uint n)
        {
            var lengthStream = new MemoryStream(BitConverter.GetBytes(n).Reverse<byte>().ToArray<byte>());

            return new BinaryReader(lengthStream).ReadUInt32();
        }

        public static short ToBigEndian(this short n)
        {
            var lengthStream = new MemoryStream(BitConverter.GetBytes(n).Reverse<byte>().ToArray<byte>());

            return new BinaryReader(lengthStream).ReadInt16();
        }

        public static ushort ToBigEndian(this ushort n)
        {
            var lengthStream = new MemoryStream(BitConverter.GetBytes(n).Reverse<byte>().ToArray<byte>());

            return new BinaryReader(lengthStream).ReadUInt16();
        }

        public static int GetRandomInt()
        {
            var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[4];
            int intVal = 0;

            provider.GetBytes(bytes);
            intVal = BitConverter.ToInt32(bytes, 0);

            return intVal;
        }

        public static T MinMax<T>(this T value, T min, T max) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object) Math.Min(Math.Max((int)(object) value, (int)(object) min), (int)(object) max);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)Math.Min(Math.Max((float)(object)value, (float)(object)min), (float)(object)max);
            }

            throw new NotSupportedException(typeof(T).Name + " is not supported by MinMax<T> function");
        }

        public static T ScopeRange<T>(this T value, T max) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(T) == typeof(int))
            {
                return value.ScopeRange((T)(object) 1, max);
            }
            else if (typeof(T) == typeof(float))
            {
                return value.ScopeRange((T)(object)1, max);
            }

            throw new NotSupportedException(typeof(T).Name + " is not supported by ScopeRange<T> function");
        }

        public static T ScopeRange<T>(this T value, T min, T max) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(T) == typeof(float))
            {
                float returnValue;
                var floatMin = (float)(object)min;
                var floatMax = (float)(object)max;
                var floatValue = (float)(object)value;

                Debug.Assert(floatMin < floatMax);

                returnValue = ((floatValue - floatMin) % floatMax) + floatMin;

                return (T)(object) returnValue;
            }
            else if (typeof(T) == typeof(int))
            {
                int returnValue;
                var intMin = (int)(object)min;
                var intMax = (int)(object)max;
                var intValue = (int)(object)value;

                Debug.Assert(intMin < intMax);

                returnValue = ((intValue - intMin) % intMax) + intMin;

                return (T)(object)returnValue;
            }

            throw new NotSupportedException(typeof(T).Name + " is not supported by ScopeRange<T> function");
        }

        public static int GetRandomIntWithinRange(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", "Min is greater than max for GetRandomIntWithinRange");
            }

            var percent = ((float) Math.Abs(GetRandomInt())) / ((float) int.MaxValue);
	        var random = (int) (min + percent * (max - min));

            return random;
        }

        public static byte GetRandomByte()
        {
            return (byte)GetRandomIntWithinRange(1, 255);
        }

        public static bool GetRandomBool()
        {
            return GetRandomIntWithinRange(0, 1) == 1 ? true : false;
        }

        public static int RoundTo(this int numToRound, int multiplier)
        {
            if (multiplier == 0)
            {
                return numToRound;
            }

            var roundDown = ((int)(numToRound) / multiplier) * multiplier;
            var roundUp = roundDown + multiplier;
            var roundCalc = roundUp;

            return roundCalc;
        }

        public static long RoundTo(this long numToRound, long multiplier)
        {
            if (multiplier == 0)
            {
                return numToRound;
            }

            var roundDown = ((long)(numToRound) / multiplier) * multiplier;
            var roundUp = roundDown + multiplier;
            var roundCalc = roundUp;

            return roundCalc;
        }

        public static float RoundTo(this float numToRound, float multiplier)
        {
            if (multiplier == 0)
            {
                return numToRound;
            }

            var roundDown = ((float)(numToRound) / multiplier) * multiplier;
            var roundUp = roundDown + multiplier;
            var roundCalc = roundUp;

            return roundCalc;
        }

        public static void Loop(this int target, Action<int> loop)
        {
            for (var x = 0; x <= target; x++)
            {
                loop(x);
            }
        }

        public static void Loop(this int start, int target, Action<int> loop)
        {
            for (var x = start; x <= target; x++)
            {
                loop(x);
            }
        }

        public static void Countdown(this int start, Action<int> loop)
        {
            for (var x = start; x != 0; x--)
            {
                loop(x);
            }
        }

        public static void Countdown(this int start, int target, Action<int> loop)
        {
            for (var x = target; x != start; x--)
            {
                loop(x);
            }
        }
    }
}
