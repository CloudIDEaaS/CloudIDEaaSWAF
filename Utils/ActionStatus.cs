using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ActionStatus
    {
        public Action Action { get; set; }
        public bool Cancelled { get; set; }

        public ActionStatus(Action action)
        {
            Action = action;
        }
    }
}
