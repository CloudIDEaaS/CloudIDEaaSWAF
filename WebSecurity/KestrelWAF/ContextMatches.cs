using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.KestrelWAF;

public class ContextMatches
{
    public KeyValuePair<string, string?> MatchedVar { get; set; }
    public string MatchedVarName { get; set; }
    public Dictionary<string, string?> MatchedVars { get; set; }
    public Dictionary<string, object?> TransactionVars { get; set; }
    public List<string> MatchedVarNames { get; set; }
}
