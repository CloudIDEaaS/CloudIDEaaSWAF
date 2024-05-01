using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public class ItemNameAttribute : Attribute
    {
        public string Name { get; }

        public ItemNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
