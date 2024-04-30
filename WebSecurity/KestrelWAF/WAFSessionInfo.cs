using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public class WAFSessionInfo : ICloneable<WAFSessionInfo>
    {
        public string ConnectionId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public bool Blocked { get; set; }
        public List<bool?[]> ReportPassFailList { get; }
        public string[] ReportPassFailedArrayHeaders { get; }
        private IList<Rule> rules;

        public WAFSessionInfo(IList<Rule> rules)
        {
            this.rules = rules;
            this.ReportPassFailList = new List<bool?[]>();
            this.ReportPassFailedArrayHeaders = rules.Select(r => r.Id).ToArray();
        }

        public WAFSessionInfo DefaultClone()
        {
            return this.DeepClone();
        }

        public WAFSessionInfo ShallowClone()
        {
            return this.DeepClone();
        }

        public WAFSessionInfo DeepClone()
        {
            var wafSessionInfo = new WAFSessionInfo(rules)
            {
                ConnectionId = ConnectionId,
                StartTime = StartTime,
                FinishTime = FinishTime,
                ElapsedMilliseconds = ElapsedMilliseconds,
            };

            wafSessionInfo.ReportPassFailedArrayHeaders.CopyTo(this.ReportPassFailedArrayHeaders, 0);
            wafSessionInfo.ReportPassFailList.AddRange(this.ReportPassFailList);

            return wafSessionInfo;
        }
    }
}
