using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.Chains;

namespace WebSecurity.KestrelWAF
{
    public class ChainSetState
    {
        public Stack<RuleChainSet> CurrentChainSetStack { get; }
        public RuleChainSet FullfilledChainSet { get; set; }

        public ChainSetState()
        {
            this.CurrentChainSetStack = new Stack<RuleChainSet>();
        }
    }
}
