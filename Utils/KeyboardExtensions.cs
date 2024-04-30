using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Utils
{
    public enum ToAsciiFlags : uint
    {
        MenuIsActive = 1,
        MenuIsInactive = 0
    }

    public static class KeyboardExtensions
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, ToAsciiFlags uFlags);
        [DllImport("user32.dll")]
        static extern int ToAsciiEx(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags, IntPtr hkl);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        public static Keys ToKey(this char ch)
        {
            var vkey = VkKeyScan(ch);
            var key = (Keys)(vkey & 0xff);
            var modifiers = vkey >> 8;

            if ((modifiers & 1) != 0) key |= Keys.Shift;
            if ((modifiers & 2) != 0) key |= Keys.Control;
            if ((modifiers & 4) != 0) key |= Keys.Alt;

            return key;
        }

        public static string ToAscii(this Keys key)
        {
            var keyboardState = new byte[256];
            var builder = new StringBuilder(new string('\0', 25), 25);
            var keyCode = (uint)key;
            int retVal;

            GetKeyboardState(keyboardState);

            retVal = ToAsciiEx((uint)keyCode, 0, keyboardState, builder, 0, IntPtr.Zero);

            if (retVal == 1 && builder.Length > 0)
            {
                return builder[0].ToString();
            }

            return string.Empty;
        }

        public static bool IsAscii(this Keys key)
        {
            var keyboardState = new byte[256];
            var builder = new StringBuilder(new string('\0', 25), 25);
            var keyCode = (uint)key;
            int retVal;

            GetKeyboardState(keyboardState);

            retVal = ToAsciiEx((uint)keyCode, 0, keyboardState, builder, 0, IntPtr.Zero);

            if (retVal == 1 && builder.Length > 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsPressed(this Keys key)
        {
            var keyState = GetAsyncKeyState(key);

            return keyState < 0;
        }
#if USE_WPF
        public static bool IsPressed(this System.Windows.Input.Key key)
        {
            return System.Windows.Input.Keyboard.IsKeyDown(key);
        }
#endif
    }
}
