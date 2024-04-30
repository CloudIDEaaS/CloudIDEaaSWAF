using System.Runtime.Serialization;

namespace WebSecurity.KestrelWAF.RulesEngine;

[DataContract, Serializable]
public class Rule
{
    [DataMember] public string Id { get; set; }
    [DataMember] public string Category { get; set; }
    [DataMember] public string Signature { get; set; }
    [DataMember] public string Marker { get; set; }
    [DataMember] public string Version { get; set; }
    [DataMember] public string MemberName { get; set; }
    [DataMember] public string InputMethod { get; set; }
    [DataMember] public object InputArgument { get; set; }
    [DataMember] public string InputMethod2 { get; set; }
    [DataMember] public object InputArgument2 { get; set; }
    [DataMember] public string Operator { get; set; }
    [DataMember] public object TargetValue { get; set; }
    [DataMember] public bool Enabled { get; set; }
    [DataMember] public IList<Rule> Rules { get; set; }
    [DataMember] public IList<object> Inputs { get; set; }
    [DataMember] public IList<object> ChainChildren { get; set; }
    [DataMember] public bool Negate { get; set; }
    [DataMember] public bool IsChainChild { get; set; }
    [DataMember] public string CallbackMethod { get; set; }
    [DataMember] public object TransformationsActions { get; set; }
    [DataMember] public string OuterOperator { get; set; }

    public static Rule operator |(Rule lhs, Rule rhs)
    {
        var rule = new Rule { Operator = "Or" };
        return MergeRulesInto(rule, lhs, rhs);
    }

    public static Rule operator &(Rule lhs, Rule rhs)
    {
        var rule = new Rule { Operator = "AndAlso" };
        return MergeRulesInto(rule, lhs, rhs);
    }

    private static Rule MergeRulesInto(Rule target, Rule lhs, Rule rhs)
    {
        target.Rules = new List<Rule>();

        if (lhs.Rules != null && lhs.Operator == target.Operator) // left is multiple
        {
            target.Rules.AddRange(lhs.Rules);

            if (rhs.Rules != null && rhs.Operator == target.Operator)
            {
                target.Rules.AddRange(rhs.Rules); // left & right are multiple
            }
            else
            {
                target.Rules.Add(rhs); // left multi, right single
            }
        }
        else if (rhs.Rules != null && rhs.Operator == target.Operator)
        {
            target.Rules.Add(lhs); // left single, right multi
            target.Rules.AddRange(rhs.Rules);
        }
        else
        {
            target.Rules.Add(lhs);
            target.Rules.Add(rhs);
        }

        return target;
    }

    public static Rule Create(string member, mreOperator oper, object target)
    {
        return new Rule { MemberName = member, TargetValue = target, Operator = oper.ToString() };
    }

    public static Rule MethodOnChild(string member, string methodName, params object[] inputs)
    {
        return new Rule { MemberName = member, Inputs = inputs.ToList(), Operator = methodName };
    }

    public static Rule Method(string methodName, params object[] inputs)
    {
        return new Rule { Inputs = inputs.ToList(), Operator = methodName };
    }

    public static Rule Any(string member, Rule rule)
    {
        return new Rule { MemberName = member, Operator = "Any", Rules = new List<Rule> { rule } };
    }

    public static Rule All(string member, Rule rule)
    {
        return new Rule { MemberName = member, Operator = "All", Rules = new List<Rule> { rule } };
    }

    public static Rule IsInteger(string member) => new Rule() { MemberName = member, Operator = "IsInteger" };
    public static Rule IsFloat(string member) => new Rule() { MemberName = member, Operator = "IsSingle" };
    public static Rule IsDouble(string member) => new Rule() { MemberName = member, Operator = "IsDouble" };
    public static Rule IsSingle(string member) => new Rule() { MemberName = member, Operator = "IsSingle" };
    public static Rule IsDecimal(string member) => new Rule() { MemberName = member, Operator = "IsDecimal" };

    public override string ToString()
    {
        if (TargetValue != null)
        {
            return $"{MemberName} {Operator} {TargetValue}";
        }

        if (Rules != null)
        {
            if (Rules.Count == 1)
            {
                return $"{MemberName} {Operator} ({Rules[0]})";
            }
            else
            {
                return $"{MemberName} {Operator} (Rules)";
            }
        }

        if (Inputs != null)
        {
            return $"{MemberName} {Operator} (Inputs)";
        }

        return $"{MemberName} {Operator}";
    }
}
