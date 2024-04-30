using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace Utils
{
    public class StatusEntry
    {
        public string StatusText { get; set; }
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }
        public int Progress { get; set; }

        public StatusEntry(string statusText, Color foreColor, Color backColor)
        {
            this.StatusText = statusText;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
        }

        public StatusEntry(string statusText)
        {
            this.StatusText = statusText;
            this.ForeColor = Color.Transparent;
            this.BackColor = Color.Transparent;
        }

        public StatusEntry(string format, int progress, params object[] args)
        {
            this.StatusText = string.Format(format, args);
            this.ForeColor = Color.Transparent;
            this.BackColor = Color.Transparent;
            this.Progress = progress;
        }

        public StatusEntry(string format, params object[] args)
        {
            this.StatusText = string.Format(format, args);
            this.ForeColor = Color.Transparent;
            this.BackColor = Color.Transparent;
        }

        public static bool operator ==(StatusEntry a, StatusEntry b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(StatusEntry a, StatusEntry b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj);
        }

        // kn - best approach to equality comparisons

        private static bool Equals(StatusEntry a, StatusEntry b)
        {
            bool equals;

            if (CompareExtensions.CheckNullEquality(a, b, out equals))
            {
                return equals;
            }
            else
            {
                if (a.StatusText != b.StatusText || a.ForeColor != b.ForeColor || a.BackColor != b.BackColor)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
