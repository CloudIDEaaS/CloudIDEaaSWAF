using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class WaitCursor : IDisposable
    {
        private Cursor previousCursor;
        private Control waitControl;

        private WaitCursor()
        {
            previousCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitCursor(Control waitControl)
        {
            if (Cursor.Current == Cursors.WaitCursor)
            {
                previousCursor = Cursors.Default;
            }
            else
            {
                previousCursor = Cursor.Current;
            }

            this.waitControl = waitControl;

            waitControl.Invoke(() =>
            {
                waitControl.Cursor = Cursors.WaitCursor;

                Cursor.Current = waitControl.Cursor;
            });
        }

        public static WaitCursor Wait()
        {
            return new WaitCursor();
        }

        public static WaitCursor Wait(Control waitControl)
        {
            return new WaitCursor(waitControl);
        }

        public void Dispose()
        {
            if (waitControl != null)
            {
                waitControl.Invoke(() =>
                {
                    waitControl.Cursor = previousCursor;
                });
            }
            else
            {
                Cursor.Current = previousCursor;
            }
        }
    }
}
