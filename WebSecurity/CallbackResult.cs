using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity
{
    public class CallbackResult
    {
        public bool PeformBlock { get; set; }
        public string? SkipAfter { get; set; }

        public CallbackResult(bool peformBlock, string? skipAfter)
        {
            this.PeformBlock = peformBlock;
            this.SkipAfter = skipAfter;
        }
    }
}
