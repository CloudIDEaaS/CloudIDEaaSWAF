using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
#if !UTILS_INTERNAL
    public class ManagedLockDoOnceAction
#else
    internal class ManagedLockDoOnceAction
#endif
    {
        public IDisposable Lock { get; set; }
        public Delegate Action { get; set; }
        public DateTime TimeStarted { get; set; }
        public TimeSpan TimeOut { get; set; }
    }
}
