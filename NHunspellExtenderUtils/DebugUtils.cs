using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class DebugUtils
    {
        [DebuggerHidden()]
        public static void Break()
        {
#if DEBUG
            Debugger.Break();
#else
            throw new InvalidOperationException(error);
#endif
        }

        public static string GetStackText(this object obj, int count, int skip)
        {
            string callStack = string.Empty;
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames().Take(count + 1);

            try
            {
                callStack = frames.Reverse().Take(count - skip - 1).Select(f => string.Format("{0}:{1}", f.GetMethod().Name, f.GetFileLineNumber())).ToDelimitedList(" -> ");
            }
            catch
            {
            }

            return callStack;
        }

    }
}
