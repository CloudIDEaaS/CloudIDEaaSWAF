using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utils
{
    public class TestingThreadedService : BaseThreadedService
    {
        public TestingThreadedService(ThreadPriority threadPriority, TimeSpan iterationSleep, TimeSpan iterationTimeOut, TimeSpan startTimeOut) : base(threadPriority, iterationSleep, iterationTimeOut, startTimeOut)
        {
        }

        public override void DoWork(bool stopping)
        {
            Thread.Sleep(100);
        }
    }
}
