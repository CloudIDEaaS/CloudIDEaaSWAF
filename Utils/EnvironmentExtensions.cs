using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class EnvironmentExtensions
    {
        [DllImport("user32", EntryPoint = "CallWindowProcW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr CallWindowProcW([In] byte[] bytes, IntPtr hWnd, int msg, [In, Out] byte[] wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);
        const int PAGE_EXECUTE_READWRITE = 0x40;

        public static string GetVolumeSerial()
        {
            string volumeSerial = "";
            var drive1 = DriveInfo.GetDrives().First();
            var drive2 = DriveInfo.GetDrives().First();

            try
            {
                var disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive1.Name.RemoveEndIfMatches("\\") + "\"");
                
                disk.Get();
                volumeSerial = disk["VolumeSerialNumber"].ToString();
            }
            catch
            {
                try
                {
                    var disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive2.Name.RemoveEndIfMatches("\\") + "\"");
                    
                    disk.Get();
                    volumeSerial = disk["VolumeSerialNumber"].ToString();
                }
                catch 
                {
                    return null;
                }
            }

            return volumeSerial;
        }

        public static DateTime GetSystemBootTime()
        {
            var query = new SelectQuery(@"SELECT LastBootUpTime FROM Win32_OperatingSystem WHERE Primary='true'");
            var searcher = new ManagementObjectSearcher(query);

            // get the datetime value and set the local boot
            // time variable to contain that value
            foreach (var managementObject in searcher.Get())
            {
                var bootTime = ManagementDateTimeConverter.ToDateTime(managementObject.Properties["LastBootUpTime"].Value.ToString());

                return bootTime;
            }

            throw new KeyNotFoundException();
        }

        public static string GetProcessorId()
        {
            byte[] sn = new byte[8];

            if (!ExecuteCode(ref sn))
            {
                return null;
            }

            return string.Format("{0}{1}", BitConverter.ToUInt32(sn, 4).ToString("X8"), BitConverter.ToUInt32(sn, 0).ToString("X8"));
        }

        private static bool ExecuteCode(ref byte[] result)
        {
            int num;

            /* The opcodes below implement a C function with the signature:
             * __stdcall CpuIdWindowProc(hWnd, Msg, wParam, lParam);
             * with wParam interpreted as an 8 byte unsigned character buffer.
             * */

            byte[] code_x86 = new byte[] {
                0x55,                      /* push ebp */
                0x89, 0xe5,                /* mov  ebp, esp */
                0x57,                      /* push edi */
                0x8b, 0x7d, 0x10,          /* mov  edi, [ebp+0x10] */
                0x6a, 0x01,                /* push 0x1 */
                0x58,                      /* pop  eax */
                0x53,                      /* push ebx */
                0x0f, 0xa2,                /* cpuid    */
                0x89, 0x07,                /* mov  [edi], eax */
                0x89, 0x57, 0x04,          /* mov  [edi+0x4], edx */
                0x5b,                      /* pop  ebx */
                0x5f,                      /* pop  edi */
                0x89, 0xec,                /* mov  esp, ebp */
                0x5d,                      /* pop  ebp */
                0xc2, 0x10, 0x00,          /* ret  0x10 */
            };
            byte[] code_x64 = new byte[] {
                0x53,                                     /* push rbx */
                0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00, /* mov rax, 0x1 */
                0x0f, 0xa2,                               /* cpuid */
                0x41, 0x89, 0x00,                         /* mov [r8], eax */
                0x41, 0x89, 0x50, 0x04,                   /* mov [r8+0x4], edx */
                0x5b,                                     /* pop rbx */
                0xc3,                                     /* ret */
            };

            byte[] code;

            if (IsX64Process())
            {
                code = code_x64;
            }
            else
            {
                code = code_x86;
            }

            IntPtr ptr = new IntPtr(code.Length);

            if (!VirtualProtect(code, ptr, PAGE_EXECUTE_READWRITE, out num))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            ptr = new IntPtr(result.Length);

            try
            {
                return (CallWindowProcW(code, IntPtr.Zero, 0, result, ptr) != IntPtr.Zero);
            }
            catch 
            {
                return false; 
            }
        }

        private static bool IsX64Process()
        {
            return IntPtr.Size == 8;
        }
    }
}