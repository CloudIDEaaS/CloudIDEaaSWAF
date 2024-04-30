using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.Chains;

public class RuleChainSet : ICloneable<RuleChainSet>
{
    public Rule PrimaryRule { get; set; }
    public bool PrimaryPassed { get; }
    public List<string> ChainChildIds { get; set; }
    public List<RuleAndResult> ChainChildRules { get; set; }
    public DateTime FullfilledAt { get; set; }

    public RuleChainSet(Rule primaryRule, bool passed)
    {
        this.ChainChildIds = new List<string>();
        this.ChainChildRules = new List<RuleAndResult>();
        this.PrimaryRule = primaryRule;
        this.PrimaryPassed = passed;

        this.ChainChildIds.AddRange(primaryRule.ChainChildren.Select(c => (string) c));
    }

    public bool IsFullfilled
    {
        get
        {
            return this.ChainChildIds.All(i => this.ChainChildRules.Any(r => r.Rule.Id == i));
        }
    }

    public RuleChainSet DefaultClone()
    {
        return this.DeepClone();
    }

    public RuleChainSet ShallowClone()
    {
        return this.DeepClone();
    }

    public RuleChainSet DeepClone()
    {
        var cloneChainSet = new RuleChainSet(this.PrimaryRule, this.PrimaryPassed);

        cloneChainSet.ChainChildRules.AddRange(this.ChainChildRules);

        return cloneChainSet;
    }
}
