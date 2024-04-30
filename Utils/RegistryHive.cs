using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class RegistryHive : IDisposable
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegLoadKey(IntPtr hKey, string lpSubKey, string lpFile);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegSaveKey(IntPtr hKey, string lpFile, uint securityAttrPtr = 0);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegUnLoadKey(IntPtr hKey, string lpSubKey);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern IntPtr RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("advapi32.dll")]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref UInt64 lpLuid);

        [DllImport("advapi32.dll")]
        static extern bool LookupPrivilegeValue(IntPtr lpSystemName, string lpName, ref UInt64 lpLuid);

        private RegistryKey parentKey;
        private string name;
        private string originalPath;
        public RegistryKey RootKey;

        private RegistryHive() { }

        public static RegistryHive LoadFromFile(string path)
        {
            RegistryHive result = new RegistryHive();

            AcquirePrivileges();

            result.parentKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.Users, RegistryView.Default);
            result.name = Guid.NewGuid().ToString();
            result.originalPath = path;
            IntPtr parentHandle = result.parentKey.Handle.DangerousGetHandle();
            RegLoadKey(parentHandle, result.name, path);
            //Console.WriteLine(Marshal.GetLastWin32Error());
            result.RootKey = result.parentKey.OpenSubKey(result.name, true);
            return result;
        }

        public static void AcquirePrivileges()
        {
            ulong luid = 0;
            bool throwaway;
            LookupPrivilegeValue(IntPtr.Zero, "SeRestorePrivilege", ref luid);
            RtlAdjustPrivilege((int)luid, true, false, out throwaway);
            LookupPrivilegeValue(IntPtr.Zero, "SeBackupPrivilege", ref luid);
            RtlAdjustPrivilege((int)luid, true, false, out throwaway);
        }

        public static void ReturnPrivileges()
        {
            ulong luid = 0;
            bool throwaway;
            LookupPrivilegeValue(IntPtr.Zero, "SeRestorePrivilege", ref luid);
            RtlAdjustPrivilege((int)luid, false, false, out throwaway);
            LookupPrivilegeValue(IntPtr.Zero, "SeBackupPrivilege", ref luid);
            RtlAdjustPrivilege((int)luid, false, false, out throwaway);
        }

        public void SaveAndUnload()
        {
            RootKey.Close();
            RegUnLoadKey(parentKey.Handle.DangerousGetHandle(), name);
            parentKey.Close();
        }

        public void Dispose()
        {
            SaveAndUnload();
            ReturnPrivileges();
        }
    }
}
