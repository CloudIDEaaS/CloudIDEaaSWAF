using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    internal static class ControlExtensions
    {
        public static T GetSafe<T>(this Control control, Func<Control, T> func)
        {
            T result;

            result = (T) control.Invoke(func, control);

            return result;
        }

        public static void DelayInvoke(this Control control, int milliseconds, Action action)
        {
            try
            {
                var timer = new OneTimeTimer(milliseconds);

                timer.Start(() =>
                {
                    if (!control.IsDisposed && !control.Disposing)
                    {
                        try
                        {
                            control.Invoke(action);
                        }
                        catch
                        {
                        }
                    }
                });
            }
            catch
            {
            }
        }

        public static void DoEvents(this Control control)
        {
            Application.DoEvents();
        }

        public static void DoEventsSleep(this Control control, int milliseconds)
        {
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < TimeSpan.FromMilliseconds(milliseconds))
            {
                Application.DoEvents();
            }
        }

        public static void DoEventsSleep(int milliseconds)
        {
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < TimeSpan.FromMilliseconds(milliseconds))
            {
                Application.DoEvents();
            }
        }
    }
}
