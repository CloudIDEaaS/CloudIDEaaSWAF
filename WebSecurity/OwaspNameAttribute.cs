using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity
{
    public class OwaspNameAttribute : Attribute
    {
        public string Name { get; }

        public OwaspNameAttribute(string name) 
        {
            this.Name = name;
        }
    }
}
