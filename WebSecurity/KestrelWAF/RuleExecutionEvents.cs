using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF;

public delegate void RuleExecutedEventHandler(object sender, RuleExecutedEventArgs e);
public delegate void RuleExecutingEventHandler(object sender, RuleExecutingEventArgs e);

public class RuleExecutedEventArgs : EventArgs
{
    public Rule Rule { get; }
    public WebContext WebContext { get; }
    public bool Success { get; }
    public bool ReturnSuccess { get; set; }
    public int Index { get; }
    public int Count { get; }

    public RuleExecutedEventArgs(Rule rule, int index, int count, WebContext webContext, bool success)
    {
        this.Rule = rule;
        this.Index = index;
        this.Count = count;
        this.WebContext = webContext;
        this.Success = success;
        this.ReturnSuccess = success;
    }
}

public class RuleExecutingEventArgs : EventArgs
{
    public Rule Rule { get; }
    public WebContext WebContext { get; }
    public int Index { get; }
    public int Count { get; }
    public bool SkipRule { get; set; }

    public RuleExecutingEventArgs(Rule rule, int index, int count, WebContext webContext)
    {
        this.Rule = rule;
        this.Index = index;
        this.Count = count;
        this.WebContext = webContext;
    }
}
