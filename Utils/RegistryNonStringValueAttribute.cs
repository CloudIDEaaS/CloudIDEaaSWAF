using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegistryNonStringValueAttribute : Attribute
    {
        public RegistryValueKind ValueKind { get; private set; }
        public string SaveFormat { get; set; }

        public RegistryNonStringValueAttribute(RegistryValueKind kind)
        {
            this.ValueKind = kind;
        }
    }
}
