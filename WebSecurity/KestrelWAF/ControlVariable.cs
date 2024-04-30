using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public class ControlVariable
    {
        public Rule Rule { get; set; }
        public ControlVariableType ControlVariableType { get; set; }
        public string Value { get; set; }
        public string Target { get; set; }

        public ControlVariable(Rule rule, ControlVariableType controlVariableType, string value, string target)
        {
            this.Rule = rule;
            this.ControlVariableType = controlVariableType;
            this.Value = value;
            this.Target = target;
        }
    }
}
