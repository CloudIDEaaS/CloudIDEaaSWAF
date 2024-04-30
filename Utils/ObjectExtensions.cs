using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.TeamFoundation.VersionControl.Client;

#if INCLUDE_SHARPSERIALIZER
using Polenter.Serialization;
#endif

namespace Utils
{
    public static class ObjectExtensions
    {
        public static object Assure(this object obj)
        {
            Debug.Assert(obj != null);

            return obj;
        }

        public unsafe static IntPtr GetPointer(this object obj)
        {
            TypedReference tr = __makeref(obj);
            IntPtr ptr = **(IntPtr**)(&tr);

            return ptr;
        }

        public static object NullToZero(this object obj)
        {
            return obj == null ? 0 : obj;
        }

        public static string NullToEmpty(this string str)
        {
            return str == null ? string.Empty : str;
        }

        public static T CreateCopy<T>(this object objFrom) where T : new()
        {
            var objTo = Activator.CreateInstance<T>();

            objFrom.CopyTo(objTo);

            return objTo;
        }

        public static void CopyTo(this IDictionary<string, dynamic> dictionary, object objTo)
        {
            foreach (var pair in dictionary)
            {
                if (objTo.HasProperty(pair.Key))
                {
                    var propertyInfoTo = objTo.GetProperty(pair.Key);
                    var pairValueType = pair.Value.GetType();
                    var value = (object) pair.Value;

                    if (pairValueType == typeof(JArray) && propertyInfoTo.PropertyType.IsArray)
                    {
                        value = ((JArray)pair.Value).ToObject(propertyInfoTo.PropertyType);
                    }

                    if (propertyInfoTo.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        if (propertyInfoTo.CanWrite)
                        {
                            if (value is JObject jObject)
                            {
                                value = jObject.ToObject<object>();
                            }

                            objTo.SetPropertyValue(propertyInfoTo.Name, value);
                        }
                    }
                }
            }
        }

        public static void CopyTo(this object objFrom, object objTo)
        {
            var properties = objFrom.GetPublicProperties();

            foreach (var propertyInfoFrom in properties)
            {
                if (objTo.HasProperty(propertyInfoFrom.Name))
                {
                    var propertyInfoTo = objTo.GetProperty(propertyInfoFrom.Name);

                    if (propertyInfoTo.PropertyType.IsAssignableFrom(propertyInfoFrom.PropertyType))
                    {
                        if (propertyInfoTo.CanWrite)
                        {
                            objTo.SetPropertyValue(propertyInfoTo.Name, objFrom.GetPropertyValue<object>(propertyInfoFrom.Name));
                        }
                    }
                }
            }
        }

        public static void CopyFieldsTo(this object objFrom, object objTo)
        {
            var fields = objFrom.GetPublicFields();

            foreach (var fieldInfoFrom in fields)
            {
                if (objTo.HasField(fieldInfoFrom.Name))
                {
                    var fieldInfoTo = objTo.GetField(fieldInfoFrom.Name);

                    if (fieldInfoTo.FieldType.IsAssignableFrom(fieldInfoFrom.FieldType))
                    {
                        objTo.SetFieldValue(fieldInfoTo.Name, objFrom.GetFieldValue<object>(fieldInfoFrom.Name));
                    }
                }
            }
        }

        public static IDisposable StartStopwatch(this object notUsed, Action<TimeSpan> func)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            return notUsed.CreateDisposable(() =>
            {
                stopWatch.Stop();
                func(stopWatch.Elapsed);
            });
        }

        public static bool IsEmptyValue<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        public static ParallelLoopResult MultiActParallel<T>(Action<T> action, params T[] args)
        {
            if (typeof(T) == typeof(Control))
            {
                using (var disposable = typeof(Control).CreateDisposable((s, e) => Control.CheckForIllegalCrossThreadCalls = true))
                {
                    Control.CheckForIllegalCrossThreadCalls = false;

                    return Parallel.ForEach(args, (a) => action(a));
                }
            }
            else
            {
                return Parallel.ForEach(args, (a) => action(a));
            }
        }

        public static void MultiAct<T>(Action<T> action, params T[] args)
        {
            foreach (var arg in args)
            {
                action(arg);
            }
        }

        public static void MultiAct<T>(Func<T, bool> condition, Action<T> action, params T[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Nothing to MultiAct on");
            }

            foreach (var arg in args)
            {
                if (condition(arg))
                {
                    action(arg);
                }
            }
        }

        public static void GetDeepPropertyValue<T, TPropertyValue>(this T obj, Expression<Func<TPropertyValue>> expression, Action<TPropertyValue> action)
        {
            throw new NotImplementedException();
        }

        public static IDictionary<string, object> GetFieldNameValueDictionary(this object obj)
        {
            var dictionary = new Dictionary<string, object>();
            var type = obj.GetType();
            var flags = BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
            var fields = type.GetFields(flags);

            foreach (var field in fields)
            {
                var name = field.Name;
                var value = field.GetValue(obj);

                dictionary.Add(name, value);
            }

            return dictionary;
        }

        public static long GetUniqueHashCode(this object obj, bool includeToString = true)
        {
            var hash = ((ulong) obj.GetHashCode()) << 32;

            if (includeToString)
            {
                hash |= (uint)(obj.GetType().FullName + ">>" + obj.ToString()).GetHashCode();
            }
            else
            {
                hash |= (uint)(obj.GetType().FullName).GetHashCode();
            }

            return (long) hash;
        }

        public static T CastTo<T>(this object obj)
        {
            return (T)obj;
        }

        public static int ToPercentageOf(this int x, int total)
        {
            if (total == 0)
            {
                return 0;
            }

            return (int)Math.Floor(((x.As<float>() / total.As<float>()) * 100));
        }

        public static float ToDecimalPercentageOf(this int x, int total)
        {
            if (total == 0)
            {
                return 0;
            }

            return x.As<float>() / total.As<float>();
        }

        public static int ToPercentageOf(this float x, float total)
        {
            return (int)((x / total) * 100);
        }

        public static T As<T>(this object obj) where T : IEquatable<T>
        {
            var type = obj.GetType();

            if (typeof(T).Is<int>())
            {
                if (type.Is<uint>())
                {
                    var uintValue = (uint)obj;
                    var intValue = (int)uintValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var intValue = (int)shortValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var intValue = (int)ushortValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<long>())
                {
                    var longValue = (long)obj;
                    var intValue = (int)longValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<ulong>())
                {
                    var ulongValue = (ulong)obj;
                    var intValue = (int)ulongValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<IntPtr>())
                {
                    var ptrValue = (IntPtr)obj;
                    var intValue = (int)ptrValue;

                    return (T)(object)intValue;
                }
            }
            else if (typeof(T).Is<ulong>())
            {
                if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var ulongValue = (ulong)shortValue;

                    return (T)(object)ulongValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var ulongValue = (ulong)ushortValue;

                    return (T)(object)ulongValue;
                }
                else if (type.Is<long>())
                {
                    var longValue = (long)obj;
                    var ulongValue = (ulong)longValue;

                    return (T)(object)ulongValue;
                }
                else if (type.Is<int>())
                {
                    var intValue = (int)obj;
                    var ulongValue = (ulong)intValue;

                    return (T)(object)intValue;
                }
                else if (type.Is<IntPtr>())
                {
                    var ptrValue = (IntPtr)obj;
                    var ulongValue = (ulong)ptrValue;

                    return (T)(object)ulongValue;
                }
            }
            else if (typeof(T).Is<long>())
            {
                if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var longValue = (long)shortValue;

                    return (T)(object)longValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var longValue = (long)ushortValue;

                    return (T)(object)longValue;
                }
                else if (type.Is<ulong>())
                {
                    var ulongValue = (ulong)obj;
                    var longValue = (long)ulongValue;

                    return (T)(object)longValue;
                }
                else if (type.Is<int>())
                {
                    var intValue = (int)obj;
                    var longValue = (long)intValue;

                    return (T)(object)longValue;
                }
                else if (type.Is<IntPtr>())
                {
                    var ptrValue = (IntPtr)obj;
                    var longValue = (long)ptrValue;

                    return (T)(object)longValue;
                }
            }
            else if (typeof(T).Is<float>())
            {
                if (type.Is<int>())
                {
                    var intValue = (int)obj;
                    var floatValue = (float)intValue;

                    return (T)(object) floatValue;
                }
                else if (type.Is<uint>())
                {
                    var uintValue = (uint)obj;
                    var floatValue = (float)uintValue;

                    return (T)(object)floatValue;
                }
                else if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var floatValue = (float)shortValue;

                    return (T)(object)floatValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var floatValue = (float)ushortValue;

                    return (T)(object)floatValue;
                }
                else if (type.Is<long>())
                {
                    var longValue = (long)obj;
                    var floatValue = (float)longValue;

                    return (T)(object)floatValue;
                }
                else if (type.Is<ulong>())
                {
                    var ulongValue = (ulong)obj;
                    var floatValue = (float)ulongValue;

                    return (T)(object)floatValue;
                }
                else if (type.Is<double>())
                {
                    var doubleValue = (double)obj;
                    var floatValue = (float)doubleValue;

                    return (T)(object)floatValue;
                }
            }
            else if (typeof(T).Is<double>())
            {
                if (type.Is<int>())
                {
                    var intValue = (int)obj;
                    var doubleValue = (double)intValue;

                    return (T)(object)doubleValue;
                }
                else if (type.Is<uint>())
                {
                    var uintValue = (uint)obj;
                    var doubleValue = (double)uintValue;

                    return (T)(object)doubleValue;
                }
                else if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var doubleValue = (double)shortValue;

                    return (T)(object)doubleValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var doubleValue = (double)ushortValue;

                    return (T)(object)doubleValue;
                }
                else if (type.Is<long>())
                {
                    var longValue = (long)obj;
                    var doubleValue = (double)longValue;

                    return (T)(object)doubleValue;
                }
                else if (type.Is<ulong>())
                {
                    var ulongValue = (ulong)obj;
                    var doubleValue = (double)ulongValue;

                    return (T)(object)doubleValue;
                }
            }
            else if (typeof(T).Is<decimal>())
            {
                if (type.Is<int>())
                {
                    var intValue = (int)obj;
                    var decimalValue = (decimal)intValue;

                    return (T)(object)decimalValue;
                }
                else if (type.Is<uint>())
                {
                    var uintValue = (uint)obj;
                    var decimalValue = (decimal)uintValue;

                    return (T)(object)decimalValue;
                }
                else if (type.Is<short>())
                {
                    var shortValue = (short)obj;
                    var decimalValue = (decimal)shortValue;

                    return (T)(object)decimalValue;
                }
                else if (type.Is<ushort>())
                {
                    var ushortValue = (ushort)obj;
                    var decimalValue = (decimal)ushortValue;

                    return (T)(object)decimalValue;
                }
                else if (type.Is<long>())
                {
                    var longValue = (long)obj;
                    var decimalValue = (decimal)longValue;

                    return (T)(object)decimalValue;
                }
                else if (type.Is<ulong>())
                {
                    var ulongValue = (ulong)obj;
                    var decimalValue = (decimal)ulongValue;

                    return (T)(object)decimalValue;
                }
            }

            return (T)obj;
        }

        public static object Convert(this string obj, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, obj);
            }
            else
            {
                switch (type.Name)
                {
                    case "Byte":
                        return System.Convert.ToByte(obj);
                    case "Int16":
                        return System.Convert.ToInt16(obj);
                    case "Int32":
                        return System.Convert.ToInt32(obj);
                    case "UInt16":
                        return System.Convert.ToUInt16(obj);
                    case "UInt32":
                        return System.Convert.ToUInt32(obj);
                    case "Boolean":
                        return System.Convert.ToBoolean(obj);
                    case "DateTime":
                        return DateTime.Parse(obj);
                    case "String":
                        return obj;
                }
            }

            throw new NotSupportedException(string.Format("Utils.ObjectExtensions.Convert does not support conversion from type {0}", type.FullName));
        }

        //public static Type InspectIDispatch(this object obj)
        //{
        //    if (IDispatchUtility.ImplementsIDispatch(obj))
        //    {
        //        var type = IDispatchUtility.GetType(obj, true);

        //        return type;
        //    }

        //    return null;
        //}

        public static string InspectComObject(this object obj)
        {
            return ComObjectUtility.InspectComObject(obj);
        }

        public static void Process(this object objectToAdd, Action<object> action)
        {
            action(objectToAdd);
        }

#if INCLUDE_SHARPSERIALIZER
        public static string GetXML(this object obj, bool skipErrors = false, bool ignoreReadOnlyProperties = true, bool useObjectNaming = false)
        {
            return obj.GetXML(int.MaxValue, skipErrors, ignoreReadOnlyProperties, useObjectNaming);
        }

        public static string GetXML(this object obj, int depth = int.MaxValue, bool skipErrors = false, bool ignoreReadOnlyProperties = false, bool useObjectNaming = false)
        {
            var serializer = new SharpSerializer(new SharpSerializerXmlSettings
            {
                Depth = depth,
                SkipErrors = skipErrors,
                IgnoreReadOnlyProperties = ignoreReadOnlyProperties,
                UseObjectNaming = useObjectNaming
            });
            var stream = new MemoryStream();
            var text = string.Empty;

            serializer.Serialize(obj, stream);
            stream.Seek(0, SeekOrigin.Begin);

            text = stream.ToText();

            return text;
        }
#endif
    }
}


namespace Utils.Hierarchies
{
    public static class ObjectExtensions
    {
        public static TReturn GetDescendantsAndSelfReturn<TReturn, TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, TReturn> callback)
        {
            Func<TObject, TReturn> recurseChildren = null;
            TReturn result;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    result = callback(subItem);

                    if (!result.IsEmptyValue())
                    {
                        return result;
                    }

                    result = recurseChildren(subItem);

                    if (!result.IsEmptyValue())
                    {
                        return result;
                    }
                }

                return default(TReturn);
            };

            result = callback(obj);

            if (!result.IsEmptyValue())
            {
                return result;
            }

            return recurseChildren(obj);
        }

        public static void GetDescendantsAndSelf<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Action<TObject> callback)
        {
            Action<TObject> recurseChildren = null;

            recurseChildren = (parent) =>
            {
                var children = childrenSelector(parent);

                if (children == null)
                {
                    return;
                }

                foreach (var subItem in childrenSelector(parent))
                {
                    callback(subItem);

                    recurseChildren(subItem);
                }
            };

            callback(obj);
            recurseChildren(obj);
        }

        public static List<TObject> ReturnDescendantsAndSelf<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, bool>? callback = null)
        {
            Action<TObject> recurseChildren = null;
            var list = new List<TObject>();

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    if (callback == null || callback(subItem))
                    {
                        list.Add(subItem);
                        recurseChildren(subItem);
                    }
                }
            };

            list.Add(obj);

            if (callback != null)
            {
                callback(obj);
            }

            recurseChildren(obj);

            return list;
        }

        public static void GetDescendantsAndSelf<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, bool> callback)
        {
            Action<TObject> recurseChildren = null;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    if (callback(subItem))
                    {
                        recurseChildren(subItem);
                    }
                }
            };

            callback(obj);
            recurseChildren(obj);
        }

        public static void GetDescendantsAndSelf<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Action<TObject, int> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    callback(subItem, x);

                    x++;
                    recurseChildren(subItem);
                    x--;
                }
            };

            callback(obj, x);

            x++;
            recurseChildren(obj);
        }

        public static void GetDescendantsAndSelf<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, int, bool> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    if (callback(subItem, x))
                    {
                        x++;
                        recurseChildren(subItem);
                        x--;
                    }
                }
            };

            if (callback(obj, x))
            {
                x++;
                recurseChildren(obj);
            }
        }

        public static TReturn GetDescendantsReturn<TReturn, TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, TReturn> callback)
        {
            Func<TObject, TReturn> recurseChildren = null;
            TReturn result;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    result = callback(subItem);

                    if (!result.IsEmptyValue())
                    {
                        return result;
                    }

                    result = recurseChildren(subItem);

                    if (!result.IsEmptyValue())
                    {
                        return result;
                    }
                }

                return default(TReturn);
            };

            return recurseChildren(obj);
        }

        public static void GetDescendants<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Action<TObject> callback)
        {
            Action<TObject> recurseChildren = null;

            recurseChildren = (parent) =>
            {
                var children = childrenSelector(parent);

                if (children != null)
                {
                    foreach (var subItem in children)
                    {
                        callback(subItem);

                        recurseChildren(subItem);
                    }
                }
            };

            recurseChildren(obj);
        }

        public static void GetDescendantsWithParent<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Action<TObject, TObject> callback)
        {
            Action<TObject> recurseChildren = null;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    callback(parent, subItem);

                    recurseChildren(subItem);
                }
            };

            recurseChildren(obj);
        }

        public static void GetDescendants<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, bool> callback)
        {
            Action<TObject> recurseChildren = null;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    if (callback(subItem))
                    {
                        recurseChildren(subItem);
                    }
                }
            };

            recurseChildren(obj);
        }

        public static void GetDescendants<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Action<TObject, int> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    callback(subItem, x);

                    x++;
                    recurseChildren(subItem);
                    x--;
                }
            };

            x++;
            recurseChildren(obj);
        }

        public static void GetDescendants<TObject, TStackObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, TStackObject, TStackObject> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;
            var stack = new Stack<TStackObject>();
            var lastLevel = 0;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    TStackObject parentStackObject = default(TStackObject);

                    for (var y = x; y <= lastLevel; y++)
                    {
                        stack.Pop();
                    }

                    if (stack.Count > 0)
                    {
                        parentStackObject = stack.Peek();
                    }

                    parentStackObject = callback(subItem, parentStackObject);
                    stack.Push(parentStackObject);
                    lastLevel = x;

                    x++;
                    recurseChildren(subItem);
                    x--;
                }
            };

            x++;
            recurseChildren(obj);
        }


        public static void GetDescendantsAndSelf<TObject, TStackObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, TStackObject, TStackObject> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;
            var stack = new Stack<TStackObject>();
            var lastLevel = 0;
            TStackObject topLevelParentStackObject;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    TStackObject parentStackObject = default(TStackObject);

                    for (var y = x; y <= lastLevel; y++)
                    {
                        stack.Pop();
                    }

                    if (stack.Count > 0)
                    {
                        parentStackObject = stack.Peek();
                    }

                    parentStackObject = callback(subItem, parentStackObject);

                    if (parentStackObject != null)
                    {
                        stack.Push(parentStackObject);
                    }

                    lastLevel = x;

                    x++;
                    recurseChildren(subItem);
                    x--;
                }
            };

            topLevelParentStackObject = callback(obj, default(TStackObject));

            if (topLevelParentStackObject != null)
            {
                stack.Push(topLevelParentStackObject);
            }

            x++;
            recurseChildren(obj);
        }

        public static void GetDescendants<TObject>(this TObject obj, Func<TObject, IEnumerable<TObject>> childrenSelector, Func<TObject, int, bool> callback)
        {
            Action<TObject> recurseChildren = null;
            var x = 0;

            recurseChildren = (parent) =>
            {
                foreach (var subItem in childrenSelector(parent))
                {
                    if (callback(subItem, x))
                    {
                        x++;
                        recurseChildren(subItem);
                        x--;
                    }
                }
            };

            x++;
            recurseChildren(obj);
        }

        public static IEnumerable<TObject> GetAncestors<TObject>(this TObject obj, Func<TObject, TObject> parentSelector)
        {
            var parent = parentSelector(obj);

            while (parent != null)
            {
                yield return parent;

                parent = parentSelector(parent);
            }
        }

        public static IEnumerable<TObject> GetAncestorsAndSelf<TObject>(this TObject obj, Func<TObject, TObject> parentSelector)
        {
            var parent = obj;

            while (parent != null)
            {
                yield return parent;

                parent = parentSelector(parent);
            }
        }
    }
}

