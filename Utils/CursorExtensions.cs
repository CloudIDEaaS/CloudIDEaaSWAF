using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public static class CursorExtensions
    {
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        /// <summary>
        /// Create a cursor from a bitmap without resizing and with the specified
        /// hot spot
        /// </summary>
        public static Cursor CreateCursor(IntPtr hIcon, int hotSpotX, int hotSpotY)
        {
            var iconInfo = new IconInfo();

            GetIconInfo(hIcon, ref iconInfo);

            iconInfo.xHotspot = hotSpotX;
            iconInfo.yHotspot = hotSpotY;
            iconInfo.fIcon = false;

            hIcon = CreateIconIndirect(ref iconInfo);

            return new Cursor(hIcon);
        }
    }
}
