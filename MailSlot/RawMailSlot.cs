using System;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace MailSlot
{
    class RawMailSlot
    {
        public const int MailslotNoMessage = -1;

        [Flags]
        public enum FileDesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }

        [Flags]
        public enum FileShareMode : uint
        {
            Zero = 0x00000000,
            FileShareDelete = 0x00000004,
            FileShareRead = 0x00000001,
            FileShareWrite = 0x00000002
        }

        public enum FileCreationDisposition : uint
        {
            CreateNew = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5
        }

        public static SafeMailslotHandle CreateMailSlot(string name, uint maxMessageSize = 0, int timeout = 0)
        {
            return CreateMailSlot(name, maxMessageSize, timeout, IntPtr.Zero);
        }

        public static SafeMailslotHandle CreateMailSlot(string name, uint maxMessageSize, int timeout, IntPtr securityAttrs)
        {
            var ret = CreateMailslot(name, maxMessageSize, (uint) timeout, securityAttrs);

            if (ret.IsInvalid)
            {
                throw new Exception("Unable to create new mailslot", new Win32Exception());
            }

            return ret;
        }

        public static bool Flush(SafeMailslotHandle handle)
        {
            return FlushFileBuffers(handle);
        }

        public static SafeMailslotHandle CreateFile(string name)
        {
            return CreateFile(name, IntPtr.Zero, IntPtr.Zero);
        }

        private static SafeMailslotHandle CreateFile(string name, IntPtr securityAttributes, IntPtr hTemplateFile, FileDesiredAccess access = FileDesiredAccess.GenericWrite, FileShareMode shareMode = FileShareMode.FileShareRead, FileCreationDisposition creationDisposition = FileCreationDisposition.OpenExisting, int flagsAndAttributes = 0)
        {
            var ret = CreateFile(name, access, shareMode, securityAttributes, creationDisposition, flagsAndAttributes, hTemplateFile);
            if (ret.IsInvalid)
            {
                throw Throw("Unable to create new mailslot");
            }
            return ret;
        }

        public static int? GetInfo(SafeMailslotHandle h)
        {
            var result = GetMailslotInfo(h, IntPtr.Zero, out var ret, out var count, IntPtr.Zero);

            if (!result)
            {
                throw Throw("Failed to get info about mail slot");
            }

            if (ret == MailslotNoMessage)
            {
                return null;
            }

            return ret;
        }

        public static byte[] ReadBytes(SafeMailslotHandle h, int count)
        {
            var ret = new byte[count];
            var res = ReadFile(h, ret, count, out var read, IntPtr.Zero);

            if (!res)
            {
                throw Throw("Failed to read bytes");
            }

            return ret;
        }

        public static void WriteBytes(SafeMailslotHandle h, byte[] bytes)
        {
            var success = WriteFile(h, bytes, bytes.Length, out var written, IntPtr.Zero);

            if (!success || bytes.Length != written)
            {
                throw Throw("Failed to write message");
            }
        }

        private static Exception Throw(string msg)
        {
            return new Exception(msg, new Win32Exception());
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeMailslotHandle CreateMailslot(string mailslotName, uint nMaxMessageSize, uint lReadTimeout, IntPtr securityAttributes);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMailslotInfo(SafeMailslotHandle hMailslot, IntPtr lpMaxMessageSize, out int lpNextSize, out int lpMessageCount, IntPtr lpReadTimeout);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadFile(SafeMailslotHandle handle, byte[] bytes, int numBytesToRead, out int numBytesRead, IntPtr overlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WriteFile(SafeMailslotHandle handle, byte[] bytes, int numBytesToWrite, out int numBytesWritten, IntPtr overlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlushFileBuffers(SafeMailslotHandle handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeMailslotHandle CreateFile(string fileName, FileDesiredAccess desiredAccess, FileShareMode shareMode, IntPtr securityAttributes, FileCreationDisposition creationDisposition, int flagsAndAttributes, IntPtr hTemplateFile);
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    public sealed class SafeMailslotHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeMailslotHandle() : base(true) { }
        public SafeMailslotHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
        {
            base.SetHandle(preexistingHandle);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(base.handle);
        }
    }
}
