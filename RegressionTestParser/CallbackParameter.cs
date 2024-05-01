using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWASPCoreRulesetParser.Emitter
{
    public class CallbackParameter
    {
        public string Name { get; set; }
        public string ParameterType { get; set; }

        public CallbackParameter(string parameterType, string name)
        {
            this.ParameterType = parameterType;
            this.Name = name;
        }
    }
}
