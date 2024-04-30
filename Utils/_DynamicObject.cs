using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using DynamicJsonObject = Utils.DynamicJsonConverter.DynamicJsonObject;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Utils
{
    public static class DynamicObjects
    {
        public static DynamicObject ToDynamic(this IDictionary<string, object> dictionary)
        {
            return new DynamicObjects.Dynamic(dictionary);
        }

        public static Dictionary<string, object> GetDynamicMemberNameValueDictionary(this object obj)
        {
            var dynamicObject = (DynamicJsonObject)obj;
            var dictionary = new Dictionary<string, object>();

            foreach (var name in dynamicObject.GetDynamicMemberNames())
            {
                var binder = Binder.GetMember(CSharpBinderFlags.None, name, obj.GetType(),  new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
                var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
                var value = callsite.Target(callsite, obj);

                dictionary.Add(name, value);
            }

            return dictionary;
        }

        public static T GetDynamicMemberValue<T>(this object obj, string name)
        {
            var dynamicObject = (DynamicJsonObject)obj;
            var binder = Binder.GetMember(CSharpBinderFlags.None, name, obj.GetType(), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            var value = callsite.Target(callsite, obj);

            return (T) value;
        }

        public static bool HasDynamicMember(this object obj, string name)
        {
            var dynamicObject = (DynamicJsonObject)obj;
            var hasMember = dynamicObject.GetDynamicMemberNames().Any(n => n == name);

            return hasMember;
        }

        public static void SetDynamicMember(this object obj, string name, object value)
        {
            var dynamicObject = (DynamicJsonObject)obj;

            var binder = Binder.SetMember(CSharpBinderFlags.None, name, obj.GetType(), new List<CSharpArgumentInfo>
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            });

            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);

            callsite.Target(callsite, obj, value);
        }

        public class Dynamic : DynamicObject
        {
            private IDictionary<string, object> dictionary;
            private object obj;

            public Dynamic(object obj)
            {
                this.obj = obj;
            }

            public Dynamic(IDictionary<string, object> dictionary)
            {
                this.dictionary = dictionary;
            }

            public override bool TryConvert(ConvertBinder binder, out object result)
            {
                return base.TryConvert(binder, out result);
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                string name = binder.Name;

                return dictionary.TryGetValue(name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                dictionary[binder.Name] = value;
                return true;
            }
        }
    }
}
