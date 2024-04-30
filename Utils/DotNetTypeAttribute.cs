using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DotNetTypeAttribute : Attribute
    {
        public DotNetType DotNetType { get; private set; }

        public DotNetTypeAttribute(DotNetType type)
        {
            this.DotNetType = type; 
        }
    }
}
