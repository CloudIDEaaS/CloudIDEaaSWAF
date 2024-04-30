using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.KestrelWAF
{
    [DebuggerDisplay(" { Signature } ")]
    public class RuleMemberInfo
    {
        public string Signature { get; }
        public Type ResultType { get; }
        public ParameterInfo[] Parameters { get; }

        public RuleMemberInfo(MethodInfo method)
        {
            this.Signature = method.GetSignature();
            this.ResultType = method.ReturnType;
            this.Parameters = method.GetParameters();
        }

        public RuleMemberInfo(PropertyInfo property)
        {
            this.Signature = property.GetSignature();
            this.ResultType = property.PropertyType;
        }
    }
}
