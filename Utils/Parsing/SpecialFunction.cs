using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.Parsing;

namespace Utils.Parsing
{
    [ReadOnly(true)]
    public class SpecialFunction
    {
        public bool IdentifierOnly { get; set; }
        public bool DynamicInitializer { get; set; }
        public bool DynamicAtExitDestructor { get; set; }
        public bool AnonymousNamespace { get; set; }
        public string AnonymousNamespaceName { get; set; }
        public string DynamicAtExitDestructorTypeOrNamespace { get; set; }
        public string DynamicInitializerTypeOrNamespace { get; set; }
        public string Type { get; set; }
    }
}
