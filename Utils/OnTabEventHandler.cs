using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void OnTabEventHandler(object sender, TabEventArgs e);

    public class TabEventArgs
    {
        public bool Forward { get; }
        public bool SkipHide { get; set; }

        public TabEventArgs(bool forward)
        {
            this.Forward = forward;
        }
    }
}
