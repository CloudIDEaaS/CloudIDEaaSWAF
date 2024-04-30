using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.KestrelWAF
{
    public class VariableMatches
    {
        public string MatchedVar { get; set; }
        public string MatchedVarName { get; set; }
        public List<string> MatchedVars { get;}
        public List<string> MatchedVarNames { get; }

        public VariableMatches()
        {
            this.MatchedVars = new List<string>();
            this.MatchedVarNames = new List<string>();
        }
    }
}
