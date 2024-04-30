using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.KestrelWAF
{
    public class IPAddressBlockedFailSuccessCount
    {
        public string IpAddress { get; }
        public bool Blocked { get; }
        public int FailCount { get; }

        public IPAddressBlockedFailSuccessCount(string ipAddress, bool blocked, int failCount)
        {
            this.IpAddress = ipAddress;
            this.Blocked = blocked;
            this.FailCount = failCount;
        }
    }
}
