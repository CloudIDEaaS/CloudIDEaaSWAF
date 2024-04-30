using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegistryKeyEnumerableAttribute : Attribute
    {
        public string KeyName { get; private set; }
        public Type FactoryType { get; private set; }
        public bool SaveValueAsKey { get; }

        public RegistryKeyEnumerableAttribute(string name, Type factoryType)
        {
            this.KeyName = name;
            this.FactoryType = factoryType;
        }

        public RegistryKeyEnumerableAttribute(string name, bool saveValueAsKey = false)
        {
            this.KeyName = name;

            this.SaveValueAsKey = saveValueAsKey;
        }
    }
}
    