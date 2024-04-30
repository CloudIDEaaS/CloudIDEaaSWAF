using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Transformations
{
    public class TransformationAttribute : Attribute
    {
        public TransformationType TransformationType { get; }

        public TransformationAttribute(TransformationType transformationType) 
        {
            this.TransformationType = transformationType;
        }
    }
}
