using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class RegistryExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(buffer);
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_BUFFER
        {
            public ushort Length;
            public ushort MaximumLength;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst=1000)]
            public byte[] Buffer;

            public UNICODE_BUFFER(string str, bool includeLenPrefixInBuffer = false)
            {
                IntPtr ptr;

                str = str + "\0\0";

                Length = (ushort)(str.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                this.Buffer = new byte[1000];

                ptr = Marshal.StringToHGlobalUni(str);

                if (includeLenPrefixInBuffer)
                {
                    Marshal.Copy(ptr, this.Buffer, 0, Length);
                }
                else
                {
                    Marshal.Copy(ptr + 4, this.Buffer, 0, Length - 4);
                }

                Marshal.FreeHGlobal(ptr);
            }

            public UNICODE_BUFFER(byte[] bytes)
            {
                IntPtr ptr;

                Length = (ushort)bytes.Length;
                MaximumLength = (ushort)(Length + 2);
                this.Buffer = new byte[1000];

                ptr = Marshal.AllocCoTaskMem(this.Length);
                IOExtensions.ZeroMemory(ptr, this.Length);

                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                Marshal.Copy(ptr, this.Buffer, 0, Length);

                Marshal.FreeHGlobal(ptr);
            }
        }
        
        public enum KeyValueInformationClass
        {
            KeyValueBasicInformation,
            KeyValueFullInformation,
            KeyValuePartialInformation,
        };

        public enum RegistryKeyType
        {
            REG_NONE = 0,
            REG_SZ = 1,
            REG_EXPAND_SZ = 2,
            REG_BINARY = 3,
            REG_DWORD = 4,
            REG_DWORD_LITTLE_ENDIAN = 4,
            REG_DWORD_BIG_ENDIAN = 5,
            REG_LINK = 6,
            REG_MULTI_SZ = 7
        }

        public struct KEY_VALUE_FULL_INFORMATION
        {
            public int TitleIndex;
            public RegistryKeyType Type;
            public int DataOffset;
            public int DataLength;
            public int NameLength;
            public byte FirstByte;
        }

        public static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        public static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        public static int KEY_QUERY_VALUE = 0x0001;
        public static int KEY_SET_VALUE = 0x0002;
        public static int KEY_CREATE_SUB_KEY = 0x0004;
        public static int KEY_ENUMERATE_SUB_KEYS = 0x0008;
        public static int KEY_WOW64_64KEY = 0x0100;
        public static int KEY_WOW64_32KEY = 0x0200;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern uint RegOpenKeyEx(
            UIntPtr hKey,
            string subKey,
            int ulOptions,
            int samDesired,
            out UIntPtr KeyHandle
            );

        [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
        public static extern uint RegQueryValueEx(UIntPtr hKey, IntPtr lpValueName, KeyValueInformationClass keyValueInformationClass, out uint lpType, out IntPtr lpData, ref int lpcbData);


        [DllImport("ntdll.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern uint NtQueryValueKey(UIntPtr hKey, IntPtr lpValueName, KeyValueInformationClass keyValueInformationClass, IntPtr lpData, int length, out int resultLength);


        //[DllImport("ntdll.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //static extern uint NtQueryValueKey(
        //    UIntPtr KeyHandle,
        //    IntPtr ValueName,
        //    IntPtr ValueClassInformation,
        //    out IntPtr lpData, 
        //    ref int lpcbData
        //    );

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern uint NtSetValueKey(
            UIntPtr KeyHandle,
            IntPtr ValueName,
            int TitleIndex,
            RegistryKeyType Type,
            IntPtr Data,
            int DataSize
            );

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern uint NtDeleteValueKey(
            UIntPtr KeyHandle,
            IntPtr ValueName
            );

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(
            UIntPtr KeyHandle
            );

        static IntPtr StructureToPtr(object obj)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        static T PtrToStructure<T>(IntPtr ptr)
        {
            var value = Marshal.PtrToStructure<T>(ptr);
            return value;
        }

        public static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        public static void MakeHiddenKey(string registryPath, string valueName, string keyValue)
        {
            UIntPtr regKeyHandle = UIntPtr.Zero;
            string valueNameTrick = "\0\0" + valueName;

            bool IsSystem;
            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                IsSystem = identity.IsSystem;
            }

            uint status = 0xc0000000;
            uint STATUS_SUCCESS = 0x00000000;

            Debug.WriteLine("\n[+] SharpHide running as normal user:\r\n    Using HKCU\\{0}", registryPath);
            status = RegOpenKeyEx(HKEY_CURRENT_USER, registryPath, 0, KEY_SET_VALUE, out regKeyHandle);

            var valueNameString = new UNICODE_STRING(valueNameTrick);
            IntPtr valueNamePtr = StructureToPtr(valueNameString);
            UNICODE_BUFFER valueData;

            valueData = new UNICODE_BUFFER(keyValue);
            IntPtr valuePtr = StructureToPtr(valueData);

            status = NtSetValueKey(regKeyHandle, valueNamePtr, 0, RegistryKeyType.REG_SZ, valuePtr, valueData.Length);

            if (status.Equals(STATUS_SUCCESS))
            {
                Debug.WriteLine("[+] Key successfully created.");
            }
            else
            {
                Debug.WriteLine("[!] Failed to create registry key.");
            }

            RegCloseKey(regKeyHandle);
        }

        public static void MakeHiddenKey(string registryPath, string valueName, byte[] keyValue)
        {
            UIntPtr regKeyHandle = UIntPtr.Zero;
            string valueNameTrick = "\0\0" + valueName;

            bool IsSystem;
            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                IsSystem = identity.IsSystem;
            }

            registryPath = registryPath.RemoveStartIfMatches(@"HKEY_CURRENT_USER\");

            uint status = 0xc0000000;
            uint STATUS_SUCCESS = 0x00000000;

            Debug.WriteLine("\n[+] SharpHide running as normal user:\r\n    Using HKCU\\{0}", registryPath);
            status = RegOpenKeyEx(HKEY_CURRENT_USER, registryPath, 0, KEY_SET_VALUE, out regKeyHandle);

            var valueNameString = new UNICODE_STRING(valueNameTrick)
            {
                Length = (ushort)(2 * valueNameTrick.Length)
            };

            IntPtr valueNamePtr = StructureToPtr(valueNameString);
            UNICODE_BUFFER valueData;

            valueData = new UNICODE_BUFFER(keyValue);
            IntPtr valuePtr = StructureToPtr(valueData);

            status = NtSetValueKey(regKeyHandle, valueNamePtr, 0, RegistryKeyType.REG_SZ, valuePtr, Marshal.SizeOf<UNICODE_BUFFER>());

            if (status.Equals(STATUS_SUCCESS))
            {
                Debug.WriteLine("[+] Key successfully created.");
            }
            else
            {
                Debug.WriteLine("[!] Failed to create registry key.");
            }

            RegCloseKey(regKeyHandle);
        }

        public static T GetHiddenKeyValue<T>(string registryPath, string valueName)
        {
            UIntPtr regKeyHandle = UIntPtr.Zero;
            string valueNameTrick = "\0\0" + valueName;

            bool IsSystem;
            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                IsSystem = identity.IsSystem;
            }

            registryPath = registryPath.RemoveStartIfMatches(@"HKEY_CURRENT_USER\");

            uint status = 0xc0000000;
            uint STATUS_SUCCESS = 0x00000000;
            uint ERROR_MORE_DATA = 0xEA;
            uint BUFFER_TO_SMALL = 0xc0000023;

            Debug.WriteLine("\n[+] SharpHide running as normal user:\r\n    Using HKCU\\{0}", registryPath);
            status = RegOpenKeyEx(HKEY_CURRENT_USER, registryPath, 0, KEY_QUERY_VALUE, out regKeyHandle);

            var valueNameString = new UNICODE_STRING(valueNameTrick);
            IntPtr valueNamePtr = StructureToPtr(valueNameString);
            IntPtr lpData = IntPtr.Zero;
            int lpcbData = 0;

            status = NtQueryValueKey(regKeyHandle, valueNamePtr, KeyValueInformationClass.KeyValueFullInformation, lpData, 0, out lpcbData); 

            if (status.Equals(BUFFER_TO_SMALL))
            {
                lpData = Marshal.AllocCoTaskMem(lpcbData);
                status = NtQueryValueKey(regKeyHandle, valueNamePtr, KeyValueInformationClass.KeyValueFullInformation, lpData, lpcbData, out lpcbData);

                if (status.Equals(STATUS_SUCCESS))
                {
                    var info = Marshal.PtrToStructure<KEY_VALUE_FULL_INFORMATION>(lpData);
                    var valueData = Marshal.PtrToStructure<UNICODE_BUFFER>(lpData + info.DataOffset);
                    var bytes = new byte[valueData.Length];

                    Array.Copy(valueData.Buffer, 0, bytes, 0, valueData.Length);

                    Debug.WriteLine("[+] Key value retrieved created.");

                    Marshal.FreeCoTaskMem(lpData);

                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)ASCIIEncoding.ASCII.GetString(bytes);
                    }
                    else if (typeof(T) == typeof(byte[]))
                    {
                        return (T)(object)bytes;
                    }
                    else
                    {
                        DebugUtils.Break();
                        return default(T);
                    }
                }
            }
            else
            {
                Debug.WriteLine("[!] Failed to get registry key value.");
            }

            RegCloseKey(regKeyHandle);
            return default(T);
        }
    }
}
