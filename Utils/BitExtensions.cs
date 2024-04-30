using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using DocumentFormat.OpenXml.InkML;

namespace Utils
{
    public static class BitExtensions
    {
        public static T SetFlag<T>(T bitmap, T flag) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> 
        {
            var bitmapInt = (int)(object) bitmap;
            var flagInt = (int)(object)flag;

            bitmapInt |= flagInt;

            return (T)(object) bitmapInt;
        }

        /// <summary>   A GUID extension method that flip endian. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 5/8/2021. </remarks>
        ///
        /// <param name="guid"> The GUID to act on. </param>
        ///
        /// <returns>   A GUID. </returns>

        public static Guid FlipEndian(this Guid guid)
        {
            var newBytes = new byte[16];
            var oldBytes = guid.ToByteArray();

            for (var i = 8; i < 16; i++)
            {
                newBytes[i] = oldBytes[i];
            }

            newBytes[3] = oldBytes[0];
            newBytes[2] = oldBytes[1];
            newBytes[1] = oldBytes[2];
            newBytes[0] = oldBytes[3];
            newBytes[5] = oldBytes[4];
            newBytes[4] = oldBytes[5];
            newBytes[6] = oldBytes[7];
            newBytes[7] = oldBytes[6];

            return new Guid(newBytes);
        }

        public static T RemoveFlag<T>(T bitmap, T flag) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> 
        {
            var bitmapInt = (int)(object)bitmap;
            var flagInt = (int)(object)flag;

            bitmapInt &= ~flagInt;

            return (T)(object)bitmapInt;
        }

        public static T GetMask<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return unchecked((T)(object)uint.MaxValue);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)uint.MaxValue;
            }

            throw new NotImplementedException();
        }

        public static T BitwiseAnd<T>(this T value1, T value2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> 
        {
            if (typeof(T) == typeof(int))
            {
                var intVal1 = (int)(object)value1;
                var intVal2 = (int)(object)value2;

                return (T)(object)(intVal1 & intVal2);
            }
            else if (typeof(T) == typeof(uint))
            {
                var uIntVal1 = (uint)(object)value1;
                var uIntVal2 = (uint)(object)value2;

                return (T)(object)(uIntVal1 & uIntVal2);
            }
            else if (typeof(T) == typeof(BigInteger))
            {
                var bigIntVal1 = (BigInteger)(object)value1;
                var bigIntVal2 = (BigInteger)(object)value2;

                return (T)(object)(bigIntVal1 & bigIntVal2);
            }

            throw new NotImplementedException();
        }

        public static T ShiftRight<T>(this T value, int bits) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> 
        {
            if (value is int)
            {
                var intVal = (int)(object)value;

                return (T)(object)(intVal >> bits);
            }
            else if (value is uint)
            {
                var uIntVal = (uint)(object)value;

                return (T)(object)(uIntVal >> bits);
            }
            else if (value is BigInteger)
            {
                var bigIntVal = (BigInteger)(object)value;

                return (T)(object)(bigIntVal >> bits);
            }

            throw new NotImplementedException();
        }

        public static Guid ShiftLeft(this Guid guid, int bits)
        {
            var temp = new byte[guid.ToByteArray().Length];

            if (bits >= 8)
            {
                Array.Copy(guid.ToByteArray(), bits / 8, temp, 0, temp.Length - (bits / 8));
            }
            else
            {
                Array.Copy(guid.ToByteArray(), temp, temp.Length);
            }

            if (bits % 8 != 0)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] <<= bits % 8;

                    if (i < temp.Length - 1)
                    {
                        temp[i] |= (byte)(temp[i + 1] >> 8 - bits % 8);
                    }
                }
            }
            //Console.WriteLine("before shifting : {0}", guid);
            
            Guid myguid = new Guid(temp);
            //Console.WriteLine("after shifting : {0}", myguid);
            
            return myguid;
        }

        public static Guid ShiftRight(this Guid guid, int bits)
        {
            var temp = new byte[guid.ToByteArray().Length];

            if (bits >= 8)
            {
                Array.Copy(guid.ToByteArray(), 0, temp, bits / 8, temp.Length - (bits / 8));
            }
            else
            {
                Array.Copy(guid.ToByteArray(), temp, temp.Length);
            }

            if (bits % 8 != 0)
            {
                for (int i = temp.Length - 1; i >= 0; i--)
                {
                    temp[i] >>= bits % 8;
                    if (i > 0)
                    {
                        temp[i] |= (byte)(temp[i - 1] << 8 - bits % 8);
                    }
                }
            }
            //Console.WriteLine("before shifting : {0}", guid);

            Guid myguid = new Guid(temp);
            //Console.WriteLine("after shifting : {0}", myguid);

            return myguid;
        }

        public static Guid BitwiseAnd(this Guid guid1, Guid guid2)
        {
            byte[] g1 = guid1.ToByteArray();
            byte[] g2 = guid2.ToByteArray();
            byte[] result = new byte[16];
            Guid returnGuid;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(g1[i] & g2[i]);
            }

            return returnGuid = new Guid(result);

        }
        
        public static Guid BitwiseOr(this Guid guid1, Guid guid2)
        {
            byte[] g1 = guid1.ToByteArray();
            byte[] g2 = guid2.ToByteArray();
            byte[] result = new byte[16];
            Guid returnGuid;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(g1[i] | g2[i]);
            }

            return returnGuid = new Guid(result);
        }
        
        public static Guid BitwiseNot(this Guid guid1) // needs to TTK regarding 2's complement or not!!!!!!!!!!!!
        {
            byte[] g1 = guid1.ToByteArray();
            byte[] result = new byte[16];
            Guid returnGuid;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(~g1[i]) ;
            }

            return returnGuid = new Guid(result); 
        }
        
        public static Guid BitwiseXor(this Guid guid1, Guid guid2) 
        {
            byte[] g1 = guid1.ToByteArray();
            byte[] g2 = guid2.ToByteArray();
            byte[] result = new byte[16];
            Guid returnGuid;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(g1[i] ^ g2[i]);
            }

            return returnGuid = new Guid(result); 
        }

        public static string ToBinaryText(this string str)
        {
            var xCheck = "";  // kn todo - remove this and below

            str = str.Split("").Aggregate("", (partialResult, c) => 
            {
                var bin = Convert.ToString(c[0], 2).PadLeft(8, '0');

                xCheck += bin + " ";

                return partialResult + bin;
            });

            return str;
        }

        public static string FromBinaryText(this string str)
        {
            string dec;
            var xCheck = "";  // kn todo - remove this and below

            dec = str.RegexGetMatches(".{1,8}").Select(m => m.Value).Aggregate("", (partialResult, p) => 
            {
                var num = int.Parse(p, System.Globalization.NumberStyles.BinaryNumber);
                var result = Convert.ToChar(num).ToString();

                xCheck += result;

                return partialResult + result;

            });

            return dec;
        }

        public static string BitwiseXor(this string str, string mask)
        {
            var length = str.Length;
            var xCheck = "";  // kn todo - remove this and below
            string result;
            int index = 0;

            str = str.ToBinaryText();
            length = str.Length;
            mask = mask.PadRight(length, '0');

            result = mask.Split("").Aggregate("", (partialResult, c) => 
            {
                if (index < length)
                {
                    var n = int.Parse(str[index].ToString());
                    var cn = int.Parse(c);
                    var xor = n ^ cn;

                    xCheck += xor.ToString();
                    index++;

                    return partialResult + xor.ToString();
                }

                return "";
            });

            return result.FromBinaryText();
        }
    }
}
