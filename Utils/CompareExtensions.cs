using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Drawing;
using System.Collections;
using System.Linq.Dynamic;

namespace Utils
{
    public static class CompareExtensions
    {
        public static bool IsZero(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

        public static IOrderedEnumerable<TSource> FormattedOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source.Count() == source.Select(i => keySelector(i).ToString().IsNumeric()).Count(b => b))
            {
                return source.OrderBy(i => int.Parse(keySelector(i).ToString()));
            }

            // kn - todo - handle dates

            return source.OrderBy(i => keySelector(i).ToString());
        }

        public static bool AllAreTrue(params bool[] results)
        {
            foreach (var result in results)
            {
                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllAreFalse(params bool[] results)
        {
            foreach (var result in results)
            {
                if (result)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllAreNull(params object[] results)
        {
            foreach (var result in results)
            {
                if (result != null)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllAreNotZero(params IComparable[] results)
        {
            foreach (var result in results)
            {
                if (result.CompareWith(0))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllAreZero(params IComparable[] results)
        {
            foreach (var result in results)
            {
                if (!result.CompareWith(0))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllAreNotNull(params object[] results)
        {
            foreach (var result in results)
            {
                if (result == null)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AnyAreNotNull(params object[] results)
        {
            return !AllAreNull(results);
        }

        public static bool AnyAreNull(params object[] results)
        {
            return !AllAreNotNull(results);
        }

        public static bool AnyAreTrue(params bool[] results)
        {
            return !AllAreFalse(results);
        }

        public static bool AnyAreFalse(params bool[] results)
        {
            return !AllAreTrue(results);
        }

        public static bool AllAreNull(this IEnumerable<object> results)
        {
            return AllAreNull(results.ToArray());
        }

        public static T GetNonNull<T>(params T[] results)
        {
            return results.SingleOrDefault(i => i != null);
        }

        public static bool AllAreNotNull(this IEnumerable<object> results)
        {
            return AllAreNotNull(results.ToArray());
        }

        public static bool AnyAreNull(this IEnumerable<object> results)
        {
            return AnyAreNull(results.ToArray());
        }

        public static bool AnyAreNotNull(this IEnumerable<object> results)
        {
            return AnyAreNotNull(results.ToArray());
        }

        public static bool AllAreTrue(this IEnumerable<bool> results)
        {
            return AllAreTrue(results.ToArray());
        }

        public static bool AllAreFalse(this IEnumerable<bool> results)
        {
            return AllAreFalse(results.ToArray());
        }

        public static bool AnyAreTrue(this IEnumerable<bool> results)
        {
            return AnyAreTrue(results.ToArray());
        }

        public static bool AnyAreFalse(this IEnumerable<bool> results)
        {
            return AnyAreFalse(results.ToArray());
        }

        public static bool IsOneOf(this object valueTest, IEnumerable values)
        {
            return valueTest.IsOneOf(values.Cast<object>().ToArray());
        }

        public static bool IsOneOf(this object valueTest, string value)
        {
            // prevents parms passed as char array

            return valueTest.IsOneOf(new List<string>() { value });  
        }

        public static bool IsAllOf(this object valueTest, params object[] values)
        {
            foreach (var value in values)
            {
                var result = CompareWith(valueTest, (info, result) =>
                {
                    if (info.InitialResult == CompareWithComparisonResult.ValuesEqualDefault)
                    {
                        return CompareWithContinuation.ReturnTrue;
                    }
                    else
                    {
                        return CompareWithContinuation.Continue;
                    }

                }, value);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOneOf(this object valueTest, params object[] values)
        {
            return CompareWith(valueTest, (info, result) =>
            {
                if (info.InitialResult == CompareWithComparisonResult.ValuesEqualDefault)
                {
                    return CompareWithContinuation.ReturnTrue;
                }
                else
                {
                    return CompareWithContinuation.Continue;
                }
                
            }, values);
        }

        public static bool IsTruthy(this object valueTest)
        {
            return valueTest.NotOneOf(null!, string.Empty, 0);
        }

        public static bool IsTruthyWithCount(this object valueTest)
        {
            var truthy = valueTest.NotOneOf(null!, string.Empty, 0);

            if (valueTest is IEnumerable enumerable)
            {
                truthy = truthy && enumerable.Count() > 0;
            }

            return truthy;
        }

        public static bool NotOneOf(this object valueTest, params object[] values)
        {
            return !CompareWith(valueTest, (info, result) =>
            {
                if (info.InitialResult == CompareWithComparisonResult.ValuesNotEqualDefault)
                {
                    return CompareWithContinuation.ReturnFalse;
                }
                else
                {
                    return CompareWithContinuation.Continue;
                }

            }, values);
        }

        public static bool CompareWith(this object value, params object[] values)
        {
            return CompareWith(value, null, values);
        }

        public static bool CompareWith(this object value, CompareWithAdditionalComparer additionalComparer, params object[] values)
        {
            return CompareWith(value, null, false, values);
        }

        public static bool CompareWith(this object valueTest, CompareWithAdditionalComparer additionalComparer, bool performParse, params object[] values)
        {
            foreach (var value in values)
            {
                if (value is null && valueTest is null)
                {
                    return true;
                }
                else if (valueTest is null && !(value is null))
                {
                    return true;
                }
                else if (value is string && valueTest is string && ((string)valueTest) == ((string)value))
                {
                    return true;
                }
                else if (value is string && valueTest is CaselessString && ((CaselessString)valueTest) == ((string)value))
                {
                    return true;
                }
                else if (value is CaselessString && valueTest is string && ((string)valueTest) == ((CaselessString)value))
                {
                    return true;
                }
                else if (value is CaselessString && valueTest is CaselessString && ((CaselessString)valueTest) == ((CaselessString)value))
                {
                    return true;
                }
                else if (value is DateTime && valueTest is DateTime && ((DateTime)valueTest) == ((DateTime)value))
                {
                    return true;
                }
                else if (value is Guid && valueTest is Guid && ((Guid)valueTest) == ((Guid)value))
                {
                    return true;
                }
                else if (value != null && value.GetType().IsEnum && valueTest.GetType().IsEnum && valueTest.Equals(value)) 
                {
                    return true;
                }
                else if (value is int && valueTest is int && ((int) valueTest) == ((int) value))
                {
                    return true;
                }
                else if (value is uint && valueTest is uint && ((uint)valueTest) == ((uint)value))
                {
                    return true;
                }
                else if (value is BigInteger && valueTest is BigInteger && ((BigInteger)valueTest) == ((BigInteger)value))
                {
                    return true;
                }
                else if (value is Color && valueTest is Color && ((Color)valueTest).ToArgb() == ((Color)value).ToArgb())
                {
                    return true;
                }
                else if (value is char && valueTest is char && ((char)valueTest) == ((char)value))
                {
                    return true;
                }
                else if (value is Type && valueTest is Type)
                {
                    var type = (Type)value;
                    var typeTest = (Type)valueTest;

                    if (type == typeTest)
                    {
                        return true;
                    }
                    else if (type.IsOrInheritsFrom(typeTest))
                    {
                        return true;
                    }
                    else if (typeTest.IsOrInheritsFrom(type))
                    {
                        return true;
                    }
                }
                else if (value == valueTest || (value != null && value.Equals(valueTest)))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EnumerableCompare<T>(IEnumerable<T> a, IEnumerable<T> b) where T : class
        {
            bool equals;
            var x = 0;

            if (CompareExtensions.CheckNullEquality(a, b, out equals))
            {
                return equals;
            }

            if (a.Count() != b.Count())
            {
                return false;
            }

            foreach (var itemA in a)
            {
                var itemB = b.ElementAt(x);

                if (!itemA.Equals(itemB))
                {
                    return false;
                }

                x++;
            }

            return true;
        }

        public static bool ReflectCompare<T>(T a, T b, Func<string, T, T, bool> propertyCompare)
        {
            bool equals;

            if (CompareExtensions.CheckNullEquality(a, b, out equals))
            {
                return equals;
            }

            foreach (var property in typeof(T).GetProperties(System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (!propertyCompare(property.Name, a, b))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckNullEquality(object obj1, object obj2, out bool equals)
        {
            // see C:\CloudIDEaaS\develop\Utils\StatusEntry.cs

            if (object.ReferenceEquals(obj1, null) && object.ReferenceEquals(obj2, null))
            {
                equals = true;
                return true;
            }

            if (object.ReferenceEquals(obj1, null))
            {
                equals = false; 
                return true;
            }

            if (object.ReferenceEquals(obj2, null))
            {
                equals = false;
                return true;
            }

            equals = false;
            return false;
        }

        public static bool IsPrintable(this char ch)
        {
            return !(ch < 0x20 || ch > 127);
        }

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
