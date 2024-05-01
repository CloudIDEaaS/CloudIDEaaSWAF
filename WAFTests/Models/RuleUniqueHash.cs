using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF.RulesEngine;


namespace WebSecurity.StartupTests.Models
{
    [DebuggerDisplay(" { Rule.Id }")]
    public class RuleUniqueHash
    {
        public Rule Rule { get; set; }
        public string UniqueValue { get; set; }
        public RuleUniquenessKind Kind { get; }

        public RuleUniqueHash(Rule rule, string value, RuleUniquenessKind kind)
        {
            this.Rule = rule;
            this.UniqueValue = value;
            this.Kind = kind;
        }
    }
}
