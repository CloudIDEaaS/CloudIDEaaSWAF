using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class LocaleExtensions
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(
               IntPtr consoleOutput,
               bool maximumWindow,
               ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput,  bool maximumWindow, ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);


        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        public unsafe static void SetConsoleFont(string fontName)
        {
            var hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (hStdOut != INVALID_HANDLE_VALUE)
            {
                var info = new CONSOLE_FONT_INFO_EX();

                info.cbSize = (uint)Marshal.SizeOf(info);
                bool tt = false;

                // First determine whether there's already a TrueType font.
                // 
                if (GetCurrentConsoleFontEx(hStdOut, false, ref info))
                {
                    tt = (info.FontFamily & TMPF_TRUETYPE) == TMPF_TRUETYPE;

                    // Set console font to Lucida Console.
                    var newInfo = new CONSOLE_FONT_INFO_EX();

                    newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                    newInfo.FontFamily = TMPF_TRUETYPE;
                    newInfo.FaceName = fontName;
                    

                    // Get some settings from current font.
                    newInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
                    newInfo.FontWeight = info.FontWeight;
                    
                    if (!SetCurrentConsoleFontEx(hStdOut, false, ref newInfo))
                    {
                        Console.WriteLine("Cannot support extended character fonts");
                    }
                }
            }
        }

        public static void LoadApplicationCulture()
        {
            var cultureName = string.Empty;
            var thread = System.Threading.Thread.CurrentThread;
            CultureInfo cultureInfo;

            try
            {
                cultureName = ConfigurationSettings.AppSettings["cultureName"];
            }
            catch
            {
                cultureName = string.Empty;
            }

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = string.Empty;
            }

            if (cultureName.StartsWith("zh"))
            {
                SetConsoleFont("KaiTi");
                Console.OutputEncoding = Encoding.Unicode;
            }

            cultureInfo = new CultureInfo(cultureName);

            thread.CurrentCulture = cultureInfo;
            thread.CurrentUICulture = cultureInfo;
        }
    }
}
