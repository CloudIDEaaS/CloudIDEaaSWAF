using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF;

public delegate void HandleMethodAmbiguityEventHandler(object sender, HandleMethodAmbiguityEventArgs e);

public class HandleMethodAmbiguityEventArgs : EventArgs
{
    public string MethodName { get; }
    public object[] InputArguments { get; set; }
    public bool RearrangedArgs { get; set; }

    public HandleMethodAmbiguityEventArgs(string methodName, object[] inputArguments)
    {
        MethodName = methodName;
        InputArguments = inputArguments;
    }
}
