using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class NativeWindow : System.Windows.Forms.NativeWindow, IDisposable, IMessageFilter
    {
        private Func<Message, bool> msgProc;
        private bool callBaseMethodFirst;
        private Action<Message> msgPostProc;
        private ControlExtensions.MsgProc msgProcByRef;
        private ControlExtensions.MsgPostProc msgPostProcByRef;

        public NativeWindow(Func<Message, bool> msgProc, bool callBaseMethodFirst = false)
        {
            this.msgProc = msgProc;
            this.callBaseMethodFirst = callBaseMethodFirst;
        }

        public NativeWindow(Func<Message, bool> msgProc, Action<Message> msgPostProc)
        {
            this.msgProc = msgProc;
            this.msgPostProc = msgPostProc;
        }

        public NativeWindow(IntPtr handle, Func<Message, bool> msgProc)
        {
            this.AssignHandle(handle);
            this.msgProc = msgProc;
        }

        public NativeWindow(IntPtr handle, Func<Message, bool> msgProc, Action<Message> msgPostProc)
        {
            this.AssignHandle(handle);
            this.msgProc = msgProc;
            this.msgPostProc = msgPostProc;
        }

        public NativeWindow(ControlExtensions.MsgProc msgProcByRef, ControlExtensions.MsgPostProc msgPostProcByRef)
        {
            this.msgProcByRef = msgProcByRef;
            this.msgPostProcByRef = msgPostProcByRef;
        }

        public void HandleAppMessages()
        {
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.HWnd == this.Handle)
            {
                WndProc(ref m);
            }

            return false;
        }

        protected unsafe override void WndProc(ref Message m)
        {
            if (msgProc != null)
            {
                if (msgProc(m))
                {
                    if (callBaseMethodFirst)
                    {
                        var result = m.Result;

                        base.WndProc(ref m);

                        m.Result = result;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
            }
            else if (msgProcByRef != null)
            {
                if (msgProcByRef(ref m))
                {
                    if (callBaseMethodFirst)
                    {
                        var result = m.Result;

                        base.WndProc(ref m);

                        m.Result = result;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
            }
            else
            {
                DebugUtils.Break();
            }

            if (msgPostProc != null)
            {
                msgPostProc(m);
            }
            else if (msgPostProcByRef != null)
            {
                msgPostProcByRef(ref m);
            }
        }

        public void Dispose()
        {
            this.ReleaseHandle();
        }
    }
}
