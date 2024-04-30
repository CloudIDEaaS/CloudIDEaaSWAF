using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.MemoryView
{
    public class StatusEventArgs : EventArgs
    {
        public string Status { get; private set; }
        public float PercentComplete { get; private set; }

        public StatusEventArgs(string status)
        {
            Status = status;
            PercentComplete = -1;
        }

        public StatusEventArgs(string status, float percentComplete)
        {
            Status = status;
            PercentComplete = percentComplete;
        }

        public StatusEventArgs(float percentComplete)
        {
            PercentComplete = percentComplete;
        }
    }
}
