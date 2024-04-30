using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Utils
{
    public static class AlertingExtensions
    {
        public enum BeepType : uint
        {
            SimpleBeep = 0xFFFFFFFF,
            OK = 0x00,
            Question = 0x20,
            Exclamation = 0x30,
            Asterisk = 0x40,
        }

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool MessageBeep(uint type);

        public static void Beep(BeepType type = BeepType.SimpleBeep)
        {
            MessageBeep((uint)type);
        }
    }
}
