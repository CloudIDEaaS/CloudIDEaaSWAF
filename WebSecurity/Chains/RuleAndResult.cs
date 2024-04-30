using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.Chains;

public class RuleAndResult
{
    public Rule Rule { get; set; }
    public bool Passed { get; set; }

    public RuleAndResult(Rule rule, bool passed)
    {
        Rule = rule;
        Passed = passed;
    }
}
