using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWASPCoreRulesetParser.Emitter
{
    public class ExpectedResult
    {
        public string NoLogContains { get; internal set; }
        public string LogContains { get; internal set; }
        public int? Status { get; internal set; }
        public string Error { get; internal set; }
    }
}
