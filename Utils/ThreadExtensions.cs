using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public static class ThreadExtensions
    {
        public static void Run(Action action, ApartmentState apartmentState = ApartmentState.STA, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            var thread = new Thread((o) => action());

            thread.ApartmentState = apartmentState;
            thread.Priority = threadPriority;

            thread.Start();
        }
    }
}
