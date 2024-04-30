using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Utils
{
    public static class ClipboardExtensions
    {
        [DllImport("user32.dll")]
        static extern uint EnumClipboardFormats(uint format);

        [DllImport("user32.dll")]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        static extern int GetClipboardFormatName(uint format, [Out] StringBuilder lpszFormatName, int cchMaxCount);

        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("ole32.dll")]
        public static extern int OleFlushClipboard();

        [DllImport("ole32.dll")]
        static extern int OleGetClipboard([MarshalAs(UnmanagedType.IUnknown)]out object ppDataObj);

        public static void ClearClipboards()
        {
            EmptyClipboard();
            OleFlushClipboard();
        }

        public static IEnumerable<KeyValuePair<uint, string>> GetFormats()
        {
            uint lastRetrievedFormat = 0;

            OpenClipboard(IntPtr.Zero);

            while (0 != (lastRetrievedFormat = EnumClipboardFormats(lastRetrievedFormat)))
            {
                var format = GetClipboardFormatName(lastRetrievedFormat);
                var dataFormat = DataFormats.GetFormat((int) lastRetrievedFormat);

                yield return new KeyValuePair<uint, string>(lastRetrievedFormat, format);
            }

            CloseClipboard();
        }

        public static string GetClipboardFormatName(uint ClipboardFormat)
        {
            var builder = new StringBuilder(1000);

            GetClipboardFormatName(ClipboardFormat, builder, builder.Capacity);

            return builder.ToString();
        }
    }
}
