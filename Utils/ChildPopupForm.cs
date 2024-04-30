using WindowsMessage = Utils.ControlExtensions.WindowsMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace Utils
{
    public class ChildPopupForm : Form
    {
        protected Control parentControl;
        private static WndProcDelegate wndProc;
        private int prevMainWndFunc;
        private int prevMsgBoxWndFunc;
        private IntPtr hwndPopupForm;
        private IntPtr hwndMain;
        private bool closing;
        private bool moved;
        private int offPointCount;
        private bool asControl;
        private Point setLocation;
        private bool allowMove;
        private bool autoSetPosition;
        private System.Windows.Forms.NativeWindow parentNativeWindow;

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, Utils.ControlExtensions.SetWindowPosFlags uFlags);
        [DllImport("user32.dll")]
        static extern int CallWindowProc(int lpPrevWndFunc, IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        internal delegate int WndProcDelegate(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
        public bool SuppressClickHandling { get; set; }

        public ChildPopupForm()
        {
        }

        public ChildPopupForm(System.Windows.Forms.NativeWindow nativeWindow, bool asControl = false, bool allowMove = false, bool autoSetPosition = true)
        {
            this.parentNativeWindow = nativeWindow;

            this.Load += ChildPopupForm_Load;
            this.Move += ChildPopupForm_Move;

            this.asControl = asControl;
            this.allowMove = allowMove;
            this.autoSetPosition = autoSetPosition;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public ChildPopupForm(Control parentControl, bool asControl = false, bool allowMove = false, bool autoSetPosition = true)
        {
            this.parentControl = parentControl;

            this.Load += ChildPopupForm_Load;
            this.Move += ChildPopupForm_Move;

            this.asControl = asControl;
            this.allowMove = allowMove;
            this.autoSetPosition = autoSetPosition;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        public void UpdateAll()
        {
            if (parentControl != null)
            {
                this.Update();
                this.parentControl.Update();
                var rc = new RECT();

                ControlExtensions.GetWindowRect(hwndMain, ref rc);
                ControlExtensions.RedrawWindow(hwndMain, ref rc, IntPtr.Zero, RedrawWindowFlags.Frame | RedrawWindowFlags.Invalidate);
            }
        }
    
        private int WndProc(IntPtr hWnd, uint msg, int wParam, IntPtr lParam)
        {
            var message = (WindowsMessage)msg;

            if (closing)
            {
                return 0;
            }

            if (msg == (uint)WindowsMessage.DESTROY)
            {
                closing = true;
            }

            if (msg == (uint)WindowsMessage.PARENTNOTIFY)
            {
                var lowHigh = new LowHiWord { Number = (uint)wParam };
                
                message = (WindowsMessage)lowHigh.Low;
                wParam = lowHigh.High;

                if (message == WindowsMessage.LBUTTONDOWN)
                {
                    int x;
                    int y;
                    Point point;
                    IntPtr hwndPoint;
                    Control control;

                    lowHigh = new LowHiWord { Number = (uint)lParam };

                    x = lowHigh.Low;
                    y = lowHigh.High;

                    point = new Point(x, y);
                    point = this.PointToScreen(point);

                    hwndPoint = ControlExtensions.WindowFromPoint(point);
                    control = Control.FromHandle(hwndPoint);

                    if (control != null && !this.SuppressClickHandling)
                    {
                        point = control.PointToClient(point);

                        lParam = ControlExtensions.MakeLParam(point.X, point.Y);

                        if (control is ToolStrip toolStrip)
                        {
                            var item = toolStrip.Items.Cast<ToolStripItem>().SingleOrDefault(i => i.Bounds.Contains(point));

                            if (item != null && item is ToolStripButton button)
                            {
                                button.PerformClick();
                            }
                            else if (item != null && item is ToolStripSplitButton splitButton)
                            {
                                var splitButtonButton = splitButton.GetPrivateFieldValue<ToolStripButton>("splitButtonButton");

                                splitButton.PerformClick();
                            }
                        }
                    }
                }
            }

            if (hWnd == hwndPopupForm)
            {
                var hwndForeground = ControlExtensions.GetForegroundWindow();
                var rect = this.Bounds;
                var point = rect.Location;
                IntPtr hwndPoint;

                point.Offset(5, 5);

                hwndPoint = ControlExtensions.WindowFromPoint(point);

                if (hwndPoint != hwndPopupForm && autoSetPosition)
                {
                    offPointCount++;

                    if (offPointCount > 100)
                    {
                        if (!allowMove)
                        {
                            SetPosition();
                            offPointCount = 0;
                        }
                    }
                }

                if (message == WindowsMessage.MOUSEACTIVATE)
                {
                    OnActivated(EventArgs.Empty);
                }

                return CallWindowProc(prevMsgBoxWndFunc, hWnd, msg, wParam, lParam);
            }
            else if (hWnd == hwndMain)
            {
                int result;

                if (!moved && message.IsOneOf(WindowsMessage.SIZE, WindowsMessage.SIZING))
                {
                    if (autoSetPosition)
                    {
                        SetPosition();
                    }
                }

                try
                {
                    result = CallWindowProc(prevMainWndFunc, hWnd, msg, wParam, lParam);
                }
                catch
                {
                    result = 0;
                }

                if (message == WindowsMessage.ACTIVATEAPP)
                {
                    this.BeginInvoke(() =>
                    {
                        var focus = this.GetFocus();

                        if (Application.OpenForms.Cast<Form>().SelectMany(f => f.GetAllControls()).Any(c => c == focus))
                        {
                            if (!this.GetAllControls().Any(c => c == focus))
                            {
                                OnDeactivate(EventArgs.Empty);
                            }
                        }
                    });
                }

                if (message == WindowsMessage.MOUSEACTIVATE)
                {
                    this.BeginInvoke(() =>
                    {
                        var focus = this.GetFocus();

                        if (!this.GetAllControls().Any(c => c == focus))
                        {
                            OnDeactivate(EventArgs.Empty);
                        }
                    });
                }
                //else if (message == WindowsMessage.PARENTNOTIFY)
                //{
                //    OnDeactivate(EventArgs.Empty);
                //}

                return result;
            }

            return 1;
        }

        private void ChildPopupForm_Load(object sender, EventArgs e)
        {
            if (autoSetPosition)
            {
                SetPosition();
            }
        }

        protected override void CreateHandle()
        {
            bool result;

            if (this.IsDisposed || (parentControl == null && parentNativeWindow == null))
            {
                return;
            }

            base.CreateHandle();

            offPointCount = 0;
            closing = false;
            moved = false;

            wndProc = new WndProcDelegate(WndProc);

            if (parentControl != null)
            {
                hwndMain = parentControl.Handle;
            }
            else
            {
                hwndMain = parentNativeWindow.Handle;
            }

            hwndPopupForm = this.Handle;

            var delegatePtr = Marshal.GetFunctionPointerForDelegate(wndProc).ToInt32();

            prevMainWndFunc = SetWindowLong(hwndMain, (int)Utils.ControlExtensions.WindowLongIndex.GWL_WNDPROC, delegatePtr);
            prevMsgBoxWndFunc = SetWindowLong(hwndPopupForm, (int)Utils.ControlExtensions.WindowLongIndex.GWL_WNDPROC, delegatePtr);

            if (parentControl != null)
            {
                result = this.SetAsChildOf(parentControl, asControl);
            }
            else
            {
                result = this.SetAsChildOf(parentNativeWindow.Handle, asControl);
            }
        }

        public new Point Location
        {
            get
            {
                return setLocation;
            }

            set
            {
                setLocation = value;
            }
        }

        private void SetPosition()
        {
            Point location;

            if (setLocation.IsEmpty)
            {
                location = parentControl.Bounds.GetCenteredRectPosition(this.Size);
            }
            else
            {
                location = setLocation;
                setLocation = location;
            }

            if (this.IsDisposed)
            {
                return;
            }

            SetWindowPos(this.Handle, Utils.ControlExtensions.HWND.Top, location.X, location.Y, 0, 0, Utils.ControlExtensions.SetWindowPosFlags.IgnoreResize | Utils.ControlExtensions.SetWindowPosFlags.ShowWindow);

            this.Activate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Load -= ChildPopupForm_Load;
            this.Move -= ChildPopupForm_Move;

            SetWindowLong(hwndMain, (int)Utils.ControlExtensions.WindowLongIndex.GWL_WNDPROC, prevMainWndFunc);
            SetWindowLong(hwndPopupForm, (int)Utils.ControlExtensions.WindowLongIndex.GWL_WNDPROC, prevMsgBoxWndFunc);

            this.DoEvents();

            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void ChildPopupForm_Move(object sender, EventArgs e)
        {
            moved = true;
        }
    }
}
