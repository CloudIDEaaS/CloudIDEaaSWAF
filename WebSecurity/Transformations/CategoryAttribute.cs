using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Transformations;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class CategoryAttribute: Attribute
{
    public string Category { get; }

    public CategoryAttribute(string category) 
    {
        this.Category = category;
    }
}
