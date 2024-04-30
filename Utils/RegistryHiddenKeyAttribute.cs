using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegistryHiddenKeyAttribute : Attribute
    {
        public RegistryHiddenKeyAttribute()
        {
        }
    }
}
