using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.GPO
{
    public class GroupPolicyObject
    {
        public static GroupPolicyObject Current
        {
            get
            {
                var groupPolicyClass = new GroupPolicyClass();

                if (groupPolicyClass is IGroupPolicyObject2)
                {
                    return new GroupPolicyObject((IGroupPolicyObject2)groupPolicyClass);
                }

                return null;
            }
        }

        IGroupPolicyObject2 gpo;
        private GroupPolicyObject(IGroupPolicyObject2 gpo)
        {
            this.gpo = gpo;
        }
        public void OpenDGGPO(string path, GroupPolicyObjectOpen flags)
        {
            try
            {
                gpo.OpenDSGPO(path, flags);
            }
            catch (COMException)
            {
                throw;
            }
        }
        public void OpenLocalMachineGPO(GroupPolicyObjectOpen flags)
        {
            try
            {
                var hResult = gpo.OpenLocalMachineGPO(flags);
            }
            catch (COMException)
            {
                throw;
            }
        }
        public void OpenLocalMachineGPOForPrincipal(string pszLocalUserOrGroupSID, GroupPolicyObjectOpen flags)
        {
            try
            {
                var hResult = gpo.OpenLocalMachineGPOForPrincipal(pszLocalUserOrGroupSID, flags);
            }
            catch (COMException)
            {
                throw;
            }
        }

        public void OpenRemoteMachineGPO(string computerName, GroupPolicyObjectOpen flags)
        {
            try
            {
                gpo.OpenRemoteMachineGPO(computerName, flags);
            }
            catch (COMException)
            {
                throw;
            }
        }
        static readonly Guid REGISTRY_EXTENSION_GUID = new Guid("35378EAC-683F-11D2-A89A-00C04FBBCFA2");
        static readonly Guid CLSID_GPESnapIn = new Guid("8FC0B734-A0E1-11d1-A7D3-0000F87571E3");
        /// <summary>
        /// The Save method saves the specified registry policy settings to disk and updates the revision number of the GPO.
        /// </summary>
        /// <param name="isMachine">Specifies the registry policy settings to be saved. If this parameter is TRUE, the computer policy settings are saved. Otherwise, the user policy settings are saved.</param>
        /// <param name="isAdd">Specifies whether this is an add or delete operation. If this parameter is FALSE, the last policy setting for the specified extension pGuidExtension is removed. In all other cases, this parameter is TRUE.</param>
        public void Save(bool isMachine, bool isAdd) => Save(isMachine, isAdd, REGISTRY_EXTENSION_GUID, CLSID_GPESnapIn);
        /// <summary>
        /// The Save method saves the specified registry policy settings to disk and updates the revision number of the GPO.
        /// </summary>
        /// <param name="isMachine">Specifies the registry policy settings to be saved. If this parameter is TRUE, the computer policy settings are saved. Otherwise, the user policy settings are saved.</param>
        /// <param name="isAdd">Specifies whether this is an add or delete operation. If this parameter is FALSE, the last policy setting for the specified extension pGuidExtension is removed. In all other cases, this parameter is TRUE.</param>
        /// <param name="GuidExtension">Specifies the GUID or unique name of the snap-in extension that will process policy. If the GPO is to be processed by the snap-in that processes .pol files, you must specify the <see cref="REGISTRY_EXTENSION_GUID"/> value.</param>
        /// <param name="Guid">Specifies the GUID that identifies the MMC snap-in used to edit this policy. The snap-in can be a Microsoft snap-in or a third-party snap-in.</param>
        public void Save(bool isMachine, bool isAdd, Guid GuidExtension, Guid Guid)
        {
            try
            {
                gpo.Save(isMachine, isAdd, GuidExtension, Guid);
            }
            catch (COMException)
            {
                throw;
            }
        }
        /// <summary>
        /// The Delete method deletes the GPO.
        /// </summary>
        public void Delete()
        {
            try
            {
                gpo.Delete();
            }
            catch (COMException)
            {
                throw;
            }
        }
        /// <summary>
        /// retrieves the unique GPO name.
        /// </summary>
        public string Name
        {
            get
            {
                var builder = new StringBuilder(byte.MaxValue);
                try
                {
                    gpo.GetName(builder, byte.MaxValue);
                }
                catch (COMException)
                {
                    throw;
                }
                return builder.ToString();
            }
        }
        public string DisplayName
        {
            get
            {
                var builder = new StringBuilder(byte.MaxValue);
                try
                {
                    gpo.GetDisplayName(builder, byte.MaxValue);
                }
                catch (COMException)
                {
                    throw;
                }
                return builder.ToString();
            }
            set
            {
                try
                {
                    gpo.SetDisplayName(value);
                }
                catch (COMException)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// retrieves the path to the GPO.
        /// </summary>
        public string Path
        {
            get
            {
                var builder = new StringBuilder(byte.MaxValue);
                try
                {
                    gpo.GetPath(builder, byte.MaxValue);
                }
                catch (COMException)
                {
                    throw;
                }
                return builder.ToString();
            }
        }
        public string GetDSPath(GroupPolicyObjectSection Section)
        {
            var builder = new StringBuilder(byte.MaxValue);
            try
            {
                gpo.GetDSPath(Section, builder, byte.MaxValue);
            }
            catch (COMException)
            {
                throw;
            }
            return builder.ToString();
        }
        public string GetFileSysPath(GroupPolicyObjectSection Section)
        {
            var builder = new StringBuilder(byte.MaxValue);
            try
            {
                gpo.GetFileSysPath(Section, builder, byte.MaxValue);
            }
            catch (COMException)
            {
                throw;
            }
            return builder.ToString();
        }

        public UIntPtr GetRegistryKey(bool isMachine)
        {
            return GetRegistryKey(isMachine ? GroupPolicyObjectSection.Machine : GroupPolicyObjectSection.User);
        }

        public UIntPtr GetRegistryKey(GroupPolicyObjectSection Section)
        {
            UIntPtr handle;

            // Machine か User でない場合、エラーとする
            // 
            if (Section != GroupPolicyObjectSection.Machine && Section != GroupPolicyObjectSection.User)
            {
                throw new NotSupportedException($"Not Support {nameof(Section)} ({Section})");
            }

            try
            {
                var hResult =  gpo.GetRegistryKey(Section, out handle);
                return handle;
            }
            catch (COMException)
            {
                throw;
            }
        }
        /// <summary>
        /// グループポリシーの valueName を削除する
        /// </summary>
        /// <param name="isMachine"></param>
        /// <param name="subKey"></param>
        /// <param name="valueName"></param>
        public bool DeleteGroupPolicy(bool isMachine, string subKey)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            try
            {
                return DeleteGroupPolicy(gphKey, isMachine, subKey);
            }
            finally
            {
                RegCloseKey(gphKey);
            }
        }
        bool DeleteGroupPolicy(UIntPtr gphKey, bool isMachine, string subKey)
        {
            if (HasGroupPolicy(gphKey, subKey))
            {
                int hr = RegDeleteKeyEx(gphKey, subKey, RegSAM.Write, 0);
                if (hr != 0)
                    throw new Exception($"RegDeleteKeyEx() fail ({hr:X})");
                Save(isMachine, false);
                return true;
            }
            return false;
        }
        public bool DeleteGroupPolicy(bool isMachine, string subKey, string valueName)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            try
            {
                return DeleteGroupPolicy(gphKey, isMachine, subKey, valueName);
            }
            finally
            {
                RegCloseKey(gphKey);
            }
        }
        bool DeleteGroupPolicy(UIntPtr gphKey, bool isMachine, string subKey, string valueName)
        {
            UIntPtr hKey;
            if (RegOpenKeyEx(gphKey, subKey, 0, RegSAM.SetValue, out hKey) == 0)
            {
                try
                {
                    var hr = RegDeleteValue(hKey, valueName);
                    if (hr != 0)
                        throw new Exception($"{nameof(RegDeleteValue)}() fail 0x{hr:X}");
                    Save(isMachine, false);
                    return true;
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
            return false;
        }
        /// <summary>
        /// グループポリシーの valueName に 
        /// </summary>
        /// <param name="isMachine">保存先のポリシーを選択する。コンピュータポリシーだと真、ユーザポリシーだと偽を指定する。</param>
        /// <param name="subKey">グループポリシーのフルパス</param>
        /// <param name="valueName">グループポリシーのキー名</param>
        /// <param name="value">設定する値。nullを指定すると valueName を削除する。</param>
        /// <param name="Kind">型の指定。指定するとその型で設定を試みる</param>
        /// <returns>Whether the config is successfully set</returns>
        /// <exception cref="COMException">COM操作が不正だった場合</exception>
        /// <exception cref="NotSupportedException">型の設定がこの関数でサポートしていない型だった場合</exception>
        public ResultCode SetGroupPolicy(bool isMachine, string subKey, string valueName, object value, RegistryValueKind Kind = RegistryValueKind.None)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            UIntPtr gphSubKey;
            RegResult flag;
            try
            {
                if (null == value)
                {
                    // check the key’s existance
                    DeleteGroupPolicy(gphKey, isMachine, subKey, valueName);
                }
                else
                {
                    // set the GPO
                    int hr = RegCreateKeyEx(
                        gphKey,
                        subKey,
                        0,
                        null,
                        RegOption.NonVolatile,
                        RegSAM.Write,
                        IntPtr.Zero,
                        out gphSubKey,
                        out flag);
                    if (0 != hr)
                        throw new Exception($"RegCreateKeyEx() fail ({hr:X})");
                    try
                    {
                        if (Kind == RegistryValueKind.None)
                        {
                            if (value.GetType() == typeof(int))
                                Kind = RegistryValueKind.DWord;
                            else if (value.GetType() == typeof(string))
                                Kind = RegistryValueKind.String;
                            else
                                throw new NotSupportedException($"Not Support Type {nameof(value)}:{value.GetType()}");
                        }
                        int cbData;
                        IntPtr keyValue = IntPtr.Zero;
                        switch (Kind)
                        {
                            case RegistryValueKind.DWord:
                                cbData = sizeof(int);
                                keyValue = Marshal.AllocHGlobal(cbData);
                                Marshal.WriteInt32(keyValue, (int)value);
                                hr = RegSetValueEx(gphSubKey, valueName, 0, RegistryValueKind.DWord, keyValue, cbData);
                                break;
                            case RegistryValueKind.String:
                                keyValue = Marshal.StringToBSTR(value.ToString());
                                cbData = System.Text.Encoding.Unicode.GetByteCount(value.ToString()) + 1;
                                hr = RegSetValueEx(gphSubKey, valueName, 0, RegistryValueKind.String, keyValue, cbData);
                                break;
                            default:
                                throw new NotSupportedException($"Not Support Type {nameof(Kind)}:{Kind}");
                        }
                        if (0 != hr)
                            throw new Exception($"RegSetValueEx() fail ({hr:X})");
                        Save(isMachine, true);
                    }
                    finally
                    {
                        RegCloseKey(gphSubKey);
                    }
                }
            }
            finally
            {
                RegCloseKey(gphKey);
            }
            return ResultCode.Succeed;
        }
        /// <summary>
        /// subKey の存在チェック
        /// </summary>
        /// <param name="isMachine"></param>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public bool HasGroupPolicy(bool isMachine, string subKey)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            try
            {
                return HasGroupPolicy(gphKey, subKey);
            }
            finally
            {
                RegCloseKey(gphKey);
            }
        }
        bool HasGroupPolicy(UIntPtr gphKey, string subKey)
        {
            UIntPtr hKey;
            if (RegOpenKeyEx(gphKey, subKey, 0, RegSAM.QueryValue, out hKey) == 0)
            {
                RegCloseKey(hKey);
                return true;
            }
            return false;
        }
        public bool HasGroupPolicy(bool isMachine, string subKey, string valueName)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            try
            {
                return HasGroupPolicy(gphKey, subKey, valueName);
            }
            finally
            {
                RegCloseKey(gphKey);
            }
        }
        bool HasGroupPolicy(UIntPtr gphKey, string subKey, string valueName)
        {
            UIntPtr hKey;
            uint size = 20;
            if (RegOpenKeyEx(gphKey, subKey, 0, RegSAM.Read, out hKey) == 0)
            {
                try
                {
                    RegistryValueKind type;
                    byte[] data = new byte[size]; // to store retrieved the value’s data
                    if (RegQueryValueEx(hKey, valueName, 0, out type, data, ref size) == 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
            return false;
        }
        /// <summary>
        /// Get the config of the group policy.
        /// </summary>
        /// <param name="isMachine">Specifies the registry policy settings to be saved. If this parameter is TRUE, get from the computer policy settings. Otherwise, get from the user policy settings.</param>
        /// <param name="subKey">Group policy config full path</param>
        /// <param name="valueName">Group policy config key name</param>
        /// <returns>The setting of the specified config</returns>
        public T GetGroupPolicy<T>(bool isMachine, string subKey, string valueName, RegistryValueKind Kind = RegistryValueKind.None)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            UIntPtr hKey;
            object keyValue = null;
            uint size = 20;
            try
            {

                if (RegOpenKeyEx(gphKey, subKey, 0, RegSAM.Read, out hKey) == 0)
                {
                    RegistryValueKind type;
                    byte[] data = new byte[size]; // to store retrieved the value’s data
                    if (RegQueryValueEx(hKey, valueName, 0, out type, data, ref size) != 0)
                    {
                        return default(T);
                    }
                    if (Kind == RegistryValueKind.None)
                        Kind = type;
                    else if (Kind != type)
                        throw new ArgumentException($"RegQueryValueEx get {nameof(type)} ({type}) is not {nameof(Kind)} ({Kind})");
                    try
                    {
                        switch (type)
                        {
                            case RegistryValueKind.Unknown:
                            case RegistryValueKind.Binary:
                                keyValue = data;
                                break;
                            case RegistryValueKind.DWord:
                                keyValue = (((data[0] | (data[1] << 8)) | (data[2] << 16)) | (data[3] << 24));
                                break;
                            case RegistryValueKind.DWordBigEdian:
                                keyValue = (((data[3] | (data[2] << 8)) | (data[1] << 16)) | (data[0] << 24));
                                break;
                            case RegistryValueKind.QWord:
                                {
                                    uint numLow = (uint)(((data[0] | (data[1] << 8)) | (data[2] << 16)) | (data[3] << 24));
                                    uint numHigh = (uint)(((data[4] | (data[5] << 8)) | (data[6] << 16)) | (data[7] << 24));
                                    keyValue = (long)(((ulong)numHigh << 32) | (ulong)numLow);
                                    break;
                                }
                            case RegistryValueKind.String:
                                keyValue = Encoding.Unicode.GetString(data, 0, (int)size);
                                break;
                            case RegistryValueKind.ExpandString:
                                keyValue = Environment.ExpandEnvironmentVariables(Encoding.Unicode.GetString(data, 0, (int)size));
                                break;
                            case RegistryValueKind.MultiString:
                                {
                                    var strings = new List<string>();
                                    string packed = Encoding.Unicode.GetString(data, 0, (int)size);
                                    int start = 0;
                                    int end = packed.IndexOf('\0', start);
                                    while (end > start)
                                    {
                                        strings.Add(packed.Substring(start, end - start));
                                        start = end + 1;
                                        end = packed.IndexOf('\0', start);
                                    }
                                    keyValue = strings.ToArray();
                                    break;
                                }
                            default:
                                throw new NotSupportedException($"Not Support Type {nameof(Kind)}:{Kind}");
                        }
                    }
                    finally
                    {
                        RegCloseKey(hKey);
                    }
                }
            }
            finally
            {
                RegCloseKey(gphKey);
            }
            return (T)keyValue;
        }/// <summary>
         /// Get the config of the group policy.
         /// </summary>
         /// <param name="isMachine">Specifies the registry policy settings to be saved. If this parameter is TRUE, get from the computer policy settings. Otherwise, get from the user policy settings.</param>
         /// <param name="subKey">Group policy config full path</param>
         /// <returns>The setting of the specified config</returns>
        public object EnumGroupPolicy(bool isMachine, string subKey)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            UIntPtr hKey;
            List<string> subKeys = new List<string>();
            uint size = 256;
            if (RegOpenKeyEx(gphKey, subKey, 0, RegSAM.Read, out hKey) == 0)
            {
                try
                {
                    uint keyIndex = 0;
                    //byte[] data = new byte[size]; // to store retrieved the value’s data
                    StringBuilder keyName = new StringBuilder((int)size);
                    long lastWriteTime = 0;
                    do
                    {
                        if (RegEnumKeyEx(hKey, keyIndex++, keyName, ref size, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, out lastWriteTime) != 0)
                        {
                            break;

                        }
                        subKeys.Add(keyName.ToString());
                    } while (true);
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
            return subKeys;
        }
        internal object EnumGroupPolicyValue(bool isMachine, string configFullPath)
        {
            UIntPtr gphKey = GetRegistryKey(isMachine);
            UIntPtr hKey;
            List<string> subKeys = new List<string>();
            uint size = byte.MaxValue + 1;
            if (RegOpenKeyEx(gphKey, configFullPath, 0, RegSAM.Read, out hKey) == 0)
            {
                try
                {
                    uint valueIndex = 0;
                    RegistryValueKind type;
                    //byte[] data = new byte[size]; // to store retrieved the value’s data
                    StringBuilder valueName = new StringBuilder((int)size);
                    uint zero = 0;
                    do
                    {
                        if (RegEnumValue(hKey, valueIndex++, valueName, ref size, IntPtr.Zero, out type, null, ref zero) != 0)
                        {
                            break;
                        }
                        // get type name:
                        string typeString = "";
                        switch (type)
                        {
                            case RegistryValueKind.Unknown: typeString = "REG_NONE"; break;
                            case RegistryValueKind.Binary: typeString = "REG_BINARY"; break;
                            case RegistryValueKind.DWord: typeString = "REG_DWORD"; break;
                            case RegistryValueKind.DWordBigEdian: typeString = "REG_DWORD_BIG_ENDIAN"; break;
                            case RegistryValueKind.QWord: typeString = "REG_QWORD"; break;
                            case RegistryValueKind.String: typeString = "REG_SZ"; break;
                            case RegistryValueKind.ExpandString: typeString = "REG_EXPAND_SZ"; break;
                            case RegistryValueKind.MultiString: typeString = "REG_MULTI_SZ"; break;
                            default:
                                throw new NotSupportedException();
                        }
                        subKeys.Add(string.Format("[{0}:{1}]", valueName, typeString));
                    } while (true);
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
            return subKeys;
        }

        #region WinAPI You can find definition of API for C# on: http://pinvoke.net/
        /// <summary>
        /// Opens the specified registry key. Note that key names are not case sensitive.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724897(VS.85).aspx for more info about the parameters.<br/>
        [DllImport("advapi32.dll", CharSet = CharSet.Ansi)]
        static extern int RegOpenKeyEx(
            UIntPtr hKey,
            string subKey,
            int ulOptions,
            RegSAM samDesired,
            out UIntPtr hkResult);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        static extern int RegDeleteValue(
            UIntPtr hKey,
            string lpValueName);
        /// <summary>
        /// enumerate the sub keys according to the given path
        /// </summary>
        [DllImport("advapi32.dll", EntryPoint = "RegEnumKeyEx")]
        static extern int RegEnumKeyEx(
            UIntPtr hkey,
            uint index,
            StringBuilder lpName,
            ref uint lpcbName,
            IntPtr reserved,
            IntPtr lpClass,
            IntPtr lpcbClass,
            out long lpftLastWriteTime);
        /// <summary>
        /// enumerate the value according to the given path
        /// </summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern uint RegEnumValue(
            UIntPtr hKey,
            uint dwIndex,
            StringBuilder lpValueName,
            ref uint lpcValueName,
            IntPtr lpReserved,
            out RegistryValueKind lpType,
            [Out] byte[] lpData,
            ref uint lpcbData);
        /// <summary>
        /// Retrieves the type and data for the specified value name associated with an open registry key.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724911(VS.85).aspx for more info about the parameters and return value.<br/>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        static extern int RegQueryValueEx(
            UIntPtr hKey,
            string lpValueName,
            int lpReserved,
            out RegistryValueKind lpType,
            [Out] byte[] lpData,
            ref uint lpcbData);
        /// <summary>
        /// Sets the data and type of a specified value under a registry key.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724923(VS.85).aspx for more info about the parameters and return value.<br/>
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegSetValueEx(
            UIntPtr hKey,
            [MarshalAs(UnmanagedType.LPStr)] string lpValueName,
            int Reserved,
            RegistryValueKind dwType,
            IntPtr lpData,
            int cbData);
        /// <summary>
        /// Creates the specified registry key. If the key already exists, the function opens it. Note that key names are not case sensitive.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724844(v=VS.85).aspx for more info about the parameters and return value.<br/>
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegCreateKeyEx(
            UIntPtr hKey,
            string lpSubKey,
            uint Reserved,
            string lpClass,
            RegOption dwOptions,
            RegSAM samDesired,
            IntPtr lpSecurityAttributes,
            out UIntPtr phkResult,
            out RegResult lpdwDisposition);
        /// <summary>
        /// Closes a handle to the specified registry key.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724837(VS.85).aspx for more info about the parameters and return value.<br/>
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegCloseKey(
        UIntPtr hKey);
        /// <summary>
        /// Deletes a subkey and its values from the specified platform-specific view of the registry. Note that key names are not case sensitive.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/ms724847(VS.85).aspx for more info about the parameters and return value.<br/>
        [DllImport("advapi32.dll", EntryPoint = "RegDeleteKeyEx", SetLastError = true)]
        public static extern int RegDeleteKeyEx(
        UIntPtr hKey,
        string lpSubKey,
        RegSAM samDesired,
        uint Reserved);
        #endregion
        /// <summary>
        /// Registry creating volatile check.
        /// </summary>
        [Flags]
        public enum RegOption
        {
            NonVolatile = 0x0,
            Volatile = 0x1,
            CreateLink = 0x2,
            BackupRestore = 0x4,
            OpenLink = 0x8
        }
        /// <summary>
        /// Access mask the specifies the platform-specific view of the registry.
        /// </summary>
        [Flags]
        public enum RegSAM
        {
            QueryValue = 0x00000001,
            SetValue = 0x00000002,
            CreateSubKey = 0x00000004,
            EnumerateSubKeys = 0x00000008,
            Notify = 0x00000010,
            CreateLink = 0x00000020,
            WOW64_32Key = 0x00000200,
            WOW64_64Key = 0x00000100,
            WOW64_Res = 0x00000300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }
        /// <summary>
        /// Structure for security attributes.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }
        /// <summary>
        /// Flag returned by calling RegCreateKeyEx.
        /// </summary>
        public enum RegResult
        {
            CreatedNewKey = 0x00000001,
            OpenedExistingKey = 0x00000002
        }

        [ComImport, Guid("EA502722-A23D-11d1-A7D3-0000F87571E3")]
        public class GroupPolicyClass
        {
        }

        [ComImport, Guid("7E37D5E7-263D-45CF-842B-96A95C63E46C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IGroupPolicyObject2
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
            [PreserveSig]
            uint New([MarshalAs(UnmanagedType.LPWStr)] string domainName, [MarshalAs(UnmanagedType.LPWStr)] string displayName, uint flags);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
            [PreserveSig]
            uint OpenDSGPO([MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.U4)] GroupPolicyObjectOpen flags);

            [PreserveSig]
            uint OpenLocalMachineGPO( [MarshalAs(UnmanagedType.U4)] GroupPolicyObjectOpen flags);

            [PreserveSig]
            uint OpenRemoteMachineGPO( [MarshalAs(UnmanagedType.LPWStr)] string computerName, [MarshalAs(UnmanagedType.U4)] GroupPolicyObjectOpen flags);

            [PreserveSig]
            uint Save([MarshalAs(UnmanagedType.Bool)] bool machine, [MarshalAs(UnmanagedType.Bool)] bool add, [MarshalAs(UnmanagedType.LPStruct)] Guid extension, [MarshalAs(UnmanagedType.LPStruct)] Guid app);

            [PreserveSig]
            uint Delete();

            [PreserveSig]
            uint GetName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxLength);

            [PreserveSig]
            uint GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxLength);

            [PreserveSig]
            uint SetDisplayName( [MarshalAs(UnmanagedType.LPWStr)] string name);

            [PreserveSig]
            uint GetPath([MarshalAs(UnmanagedType.LPWStr)] StringBuilder path, int maxPath);

            [PreserveSig]
            uint GetDSPath([MarshalAs(UnmanagedType.U4)] GroupPolicyObjectSection section, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder path, int maxPath);

            [PreserveSig]
            uint GetFileSysPath([MarshalAs(UnmanagedType.U4)] GroupPolicyObjectSection section, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder path, int maxPath);

            [PreserveSig]
            uint GetRegistryKey( [MarshalAs(UnmanagedType.U4)] GroupPolicyObjectSection section, out UIntPtr key);

            [PreserveSig]
            uint GetOptions();

            [PreserveSig]
            uint SetOptions(uint options, uint mask);

            [PreserveSig]
            uint GetType(out IntPtr gpoType);

            [PreserveSig]
            uint GetMachineName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxLength);

            [PreserveSig]
            uint GetPropertySheetPages( out IntPtr pages);

            [PreserveSig]
            uint OpenLocalMachineGPOForPrincipal([MarshalAs(UnmanagedType.LPWStr)] string pszLocalUserOrGroupSID, [MarshalAs(UnmanagedType.U4)] GroupPolicyObjectOpen flags);

            [PreserveSig]
            uint GetRegistryKeyPath( uint section, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszRegistryKeyPath, int maxPath);
        }
    }
    public enum RegistryValueKind : int
    {
        None = -1,
        Unknown = 0,
        String = 1,
        ExpandString = 2,
        Binary = 3,
        DWord = 4,
        DWordBigEdian = 5,
        MultiString = 7,
        QWord = 11,
    }
    public enum GroupPolicyObjectOpen : uint
    {
        LoadRegistry = 1,
        ReadOnly = 2,
    }
    public enum GroupPolicyObjectMask : uint
    {
        DisableUser = 1,
        DisableMachine = 2,
    }
    public enum GroupPolicyObjectSection : uint
    {
        /// <summary>
        /// Root section
        /// </summary>
        Root = 0,
        /// <summary>
        /// User section
        /// </summary>
        User = 1,
        /// <summary>
        /// Machine section
        /// </summary>
        Machine = 2,
    }
    public enum ResultCode
    {
        Succeed = 0,
        CreateOrOpenFailed = -1,
        SetFailed = -2,
        SaveFailed = -3
    }
}