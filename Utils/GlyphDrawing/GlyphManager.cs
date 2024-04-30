using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;

namespace Utils.GlyphDrawing
{
    public class GlyphManager : IMessageFilter
    {
        public GlyphDictionary Glyphs { get; private set; }
        public List<TabControl> RenamableTabControls { get; private set; }
        private Dictionary<Control, NativeWindow> trackedControls;
        private List<Control> mouseDownControls;
        
        public GlyphManager()
        {
            this.Glyphs = new GlyphDictionary();
            this.RenamableTabControls = new List<TabControl>();
            this.Glyphs.PropertyChanged += new PropertyChangedEventHandler(Glyphs_PropertyChanged);

            trackedControls = new Dictionary<Control, NativeWindow>();
            mouseDownControls = new List<Control>();

            Application.AddMessageFilter(this);
        }

        private void Glyphs_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var newControls = this.Glyphs.Where(g => !trackedControls.Keys.Any(c => c == g.Key)).Select(g => g.Key).ToList();
            var removedControls = trackedControls.Keys.Where(c => !this.Glyphs.Keys.Any(k => k == c)).ToList();

            foreach (var newControl in newControls)
            {
                var nativeWindow = newControl.GetMessages(WndProc, WndPostProc);
                var tme = new TRACKMOUSEEVENT(TMEFlags.TME_HOVER | TMEFlags.TME_LEAVE | TMEFlags.TME_NONCLIENT, nativeWindow.Handle, 400);

                ControlExtensions.TrackMouseEvent(ref tme);

                trackedControls.Add(newControl, nativeWindow);
            }

            foreach (var removedControl in removedControls)
            {
                var nativeWindow = trackedControls[removedControl];
                var tme = new TRACKMOUSEEVENT(TMEFlags.TME_CANCEL, nativeWindow.Handle, 0);

                ControlExtensions.TrackMouseEvent(ref tme);

                trackedControls.Add(removedControl, nativeWindow);

                nativeWindow.Dispose();
            }
        }

        private void WndPostProc(Message m)
        {
            var message = (Utils.ControlExtensions.WindowsMessage)m.Msg;
            var control = Control.FromHandle(m.HWnd);

            if (control != null && Glyphs.Keys.Any(c => c == control))
            {
                var glyphs = this.Glyphs[control];

                switch (message)
                {
                    case ControlExtensions.WindowsMessage.PAINT:

                        using (var graphics = control.CreateGraphics())
                        {
                            foreach (var glyph in glyphs)
                            {
                                var location = glyph.Location;
                                var showOnFocusHoverOnly = false;
                                Rectangle rect;
                                Rectangle borderRect;

                                location.Offset(glyph.Offset);
                                rect = new Rectangle(location, glyph.Size);
                                borderRect = rect;

                                borderRect.Width--;
                                borderRect.Height--;

                                if (glyph.Clicked)
                                {
                                    graphics.FillRectangle(SystemBrushes.ControlDark, rect);
                                    graphics.DrawRectangle(SystemPens.ButtonShadow, borderRect);

                                    showOnFocusHoverOnly = true;
                                }
                                else if (glyph.Hovered)
                                {
                                    graphics.FillRectangle(SystemBrushes.ControlLightLight, rect);
                                    graphics.DrawRectangle(SystemPens.ButtonShadow, borderRect);

                                    showOnFocusHoverOnly = true;
                                }

                                if (glyph.FocusedOrHovered)
                                {
                                    showOnFocusHoverOnly = true;
                                }

                                location.Offset(glyph.TextOffset);

                                if (glyph.ShowOnFocusHoverOnly)
                                {
                                    if (showOnFocusHoverOnly)
                                    {
                                        graphics.DrawString(glyph.ImageChar.ToString(), glyph.Font, glyph.Brush, location);
                                    }
                                }
                                else
                                {
                                    graphics.DrawString(glyph.ImageChar.ToString(), glyph.Font, glyph.Brush, location);
                                }
                            }
                        }

                        break;
                }
            }
        }

        private bool WndProc(Message m)
        {
            var message = (Utils.ControlExtensions.WindowsMessage)m.Msg;
            var control = Control.FromHandle(m.HWnd);

            if (control != null && Glyphs.Keys.Any(c => c == control))
            {
                Point point;
                LowHiWordSigned lowHighWord;
                Rectangle rect;
                var glyphs = this.Glyphs[control].ToList();

                switch (message)
                {
                    case ControlExtensions.WindowsMessage.LBUTTONDOWN:

                        lowHighWord = m.LParam.ToLowHiWordSigned();
                        point = new Point(lowHighWord.Low, lowHighWord.High);

                        foreach (var glyph in glyphs)
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);

                            rect = new Rectangle(location, glyph.Size);

                            if (rect.Contains(point))
                            {
                                glyph.Clicked = true;

                                control.Invalidate(rect);
                                control.Update();
                            }
                            else
                            {
                                glyph.Clicked = false;
                            }
                        }

                        break;

                    case ControlExtensions.WindowsMessage.LBUTTONUP:

                        lowHighWord = m.LParam.ToLowHiWordSigned();
                        point = new Point(lowHighWord.Low, lowHighWord.High);

                        foreach (var glyph in glyphs.Where(g => g.Clicked))
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);

                            rect = new Rectangle(location, glyph.Size);

                            if (rect.Contains(point))
                            {
                                glyph.PerformClick();
                                glyph.Clicked = false;

                                control.Invalidate(rect);
                                control.Update();
                            }
                        }

                        break;

                    case ControlExtensions.WindowsMessage.NCHITTEST:

                        lowHighWord = m.LParam.ToLowHiWordSigned();
                        point = new Point(lowHighWord.Low, lowHighWord.High);

                        foreach (var glyph in glyphs)
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);
                            point = control.PointToClient(point);

                            rect = new Rectangle(location, glyph.Size);

                            if (rect.Contains(point))
                            {
                                if (!glyph.Hovered)
                                {
                                    glyph.Hovered = true;

                                    control.Invalidate(rect);
                                    control.Update();
                                }
                            }
                            else if (glyph.Hovered)
                            {
                                glyph.Hovered = false;
                                glyph.Clicked = false;

                                control.Invalidate(rect);
                                control.Update();
                            }
                        }

                        break;

                    case ControlExtensions.WindowsMessage.NCMOUSELEAVE:

                        foreach (var glyph in glyphs.Where(g => g.Hovered))
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);
                            rect = new Rectangle(location, glyph.Size);

                            glyph.Hovered = false;
                            glyph.Clicked = false;

                            control.Invalidate(rect);
                            control.Update();
                        }

                        break;

                    case ControlExtensions.WindowsMessage.MOUSEMOVE:

                        lowHighWord = m.LParam.ToLowHiWordSigned();
                        point = new Point(lowHighWord.Low, lowHighWord.High);

                        foreach (var glyph in glyphs)
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);

                            rect = new Rectangle(location, glyph.Size);

                            if (rect.Contains(point))
                            {
                                if (!glyph.Hovered)
                                {
                                    glyph.Hovered = true;

                                    control.Invalidate(rect);
                                    control.Update();
                                }
                            }
                            else if (glyph.Hovered)
                            {
                                glyph.Hovered = false;

                                control.Invalidate(rect);
                                control.Update();
                            }
                        }

                        if (Keys.LButton.IsPressed())
                        {
                            if (!mouseDownControls.Contains(control))
                            {
                                control.SetCapture();

                                control.SendMessage(ControlExtensions.WindowsMessage.NCLBUTTONDOWN, m.WParam, m.LParam);
                                mouseDownControls.Add(control);
                            }
                        }
                        else
                        {
                            if (mouseDownControls.Contains(control))
                            {
                                control.ReleaseCapture();

                                control.SendMessage(ControlExtensions.WindowsMessage.NCLBUTTONUP, m.WParam, m.LParam);
                                mouseDownControls.Remove(control);
                            }
                        }

                        break;

                    case ControlExtensions.WindowsMessage.MOUSELEAVE:

                        foreach (var glyph in glyphs.Where(g => g.Hovered))
                        {
                            var location = glyph.Location;

                            location.Offset(glyph.Offset);
                            rect = new Rectangle(location, glyph.Size);

                            glyph.Hovered = false;
                            glyph.Clicked = false;

                            control.Invalidate(rect);
                            control.Update();
                        }

                        break;
                }
            }
            else if (control != null && Glyphs.Keys.Any(c => c.GetParentForm() == control))
            {
                switch (message)
                {
                    case ControlExtensions.WindowsMessage.LBUTTONDOWN:
                    case ControlExtensions.WindowsMessage.LBUTTONUP:

                        foreach (var childControl in Glyphs.Keys.Where(c => c.GetParentForm() == control))
                        {
                            var lowHighWord = m.LParam.ToLowHiWordSigned();
                            var point = new Point(lowHighWord.Low, lowHighWord.High);
                            IntPtr lParam;

                            point = childControl.TranslatePoint(point, control);
                            lParam = ControlExtensions.MakeLParam(point.X, point.Y);

                            m.HWnd = childControl.Handle;
                            m.LParam = lParam;

                            WndProc(m);
                        }

                        break;
                }
            }

            return true;
        }

        public bool PreFilterMessage(ref Message m)
        {
            return !this.WndProc(m);
        }
    }
}
