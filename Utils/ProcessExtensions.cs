using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Reflection;

namespace Utils
{
	public static class ProcessExtensions
	{
		[Flags]
		private enum ProcessAccessFlags : uint
		{
			All = 0x001F0FFF,
			Terminate = 0x00000001,
			CreateThread = 0x00000002,
			VirtualMemoryOperation = 0x00000008,
			VirtualMemoryRead = 0x00000010,
			VirtualMemoryWrite = 0x00000020,
			DuplicateHandle = 0x00000040,
			CreateProcess = 0x000000080,
			SetQuota = 0x00000100,
			SetInformation = 0x00000200,
			QueryInformation = 0x00000400,
			QueryLimitedInformation = 0x00001000,
			Synchronize = 0x00100000
		}

		[Flags]
		public enum AllocationProtectEnum : uint
		{
			Execute = 0x00000010,
			ExecuteRead = 0x00000020,
			ExecuteReadWrite = 0x00000040,
			ExecuteWriteCopy = 0x00000080,
			NoAccess = 0x00000001,
			Readonly = 0x00000002,
			ReadWrite = 0x00000004,
			WriteCopy = 0x00000008,
			Guard = 0x00000100,
			NoCache = 0x00000200,
			WriteCombine = 0x00000400
		}

		[Flags]
		public enum StateEnum : uint
		{
			Commit = 0x1000,
			Free = 0x10000,
			Reserve = 0x2000
		}

		[Flags]
		public enum TypeEnum : uint
		{
			Image = 0x1000000,
			Mapped = 0x40000,
			Private = 0x20000
		}

		public struct MEMORY_BASIC_INFORMATION
		{
			public IntPtr BaseAddress;
			public IntPtr AllocationBase;
			public AllocationProtectEnum AllocationProtect;
			public IntPtr RegionSize;
			public StateEnum State;
			public AllocationProtectEnum Protect;
			public TypeEnum Type;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MODULEINFO
		{
			public IntPtr lpBaseOfDll;
			public uint SizeOfImage;
			public IntPtr EntryPoint;
		}

		[Flags]
		public enum ProcessModulesFilterFlags
		{
			ListModules32Bit = 0x01,  //List the 32-bit modules.
			ListModules64Bit = 0x02,    // List the 64-bit modules.
			ListModulesAll = 0x03,  // List all modules.
			ListModulesDefault = 0x0,   // Use the default behavior.
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct SYSTEM_INFO_UNION
		{
			[FieldOffset(0)]
			public UInt32 OemId;
			[FieldOffset(0)]
			public UInt16 ProcessorArchitecture;
			[FieldOffset(2)]
			public UInt16 Reserved;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SYSTEM_INFO
		{
			public SYSTEM_INFO_UNION CpuInfo;
			public UInt32 PageSize;
			public UInt32 MinimumApplicationAddress;
			public UInt32 MaximumApplicationAddress;
			public UInt32 ActiveProcessorMask;
			public UInt32 NumberOfProcessors;
			public UInt32 ProcessorType;
			public UInt32 AllocationGranularity;
			public UInt16 ProcessorLevel;
			public UInt16 ProcessorRevision;
		}

		[Flags]
		public enum SnapshotFlags : uint
		{
			HeapList = 0x00000001,
			Process = 0x00000002,
			Thread = 0x00000004,
			Module = 0x00000008,
			Module32 = 0x00000010,
			Inherit = 0x80000000,
			All = 0x0000001F
		}

		[StructLayout(LayoutKind.Sequential)]
		[DebuggerDisplay(" { DebugInfo } ")]
		public struct MODULEENTRY32
		{
			public uint dwSize;
			public uint th32ModuleID;
			public uint th32ProcessID;
			public uint GlblcntUsage;
			public uint ProccntUsage;
			public IntPtr modBaseAddr;
			public uint modBaseSize;
			public IntPtr hModule;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string szModule;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szExePath;

			public string DebugInfo
            {
				get
                {
					return string.Format("0x{0:x8} - 0x{1:x8}", modBaseAddr.ToInt64(), modBaseAddr.ToInt64() + modBaseSize);
				}
            }
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct PROCESSENTRY32
		{
			public uint dwSize;
			public uint cntUsage;
			public uint th32ProcessID;
			public IntPtr th32DefaultHeapID;
			public uint th32ModuleID;
			public uint cntThreads;
			public uint th32ParentProcessID;
			public int pcPriClassBase;
			public uint dwFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szExeFile;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct CRITICAL_SECTION
		{
			public IntPtr DebugInfo;
			public int LockCount;
			public int RecursionCount;
			public IntPtr OwningThread;
			public IntPtr LockSemaphore;
			public UIntPtr SpinCount;
		}


		[DllImport("kernel32.dll")]
		public static extern bool IsBadCodePtr(IntPtr lpfn);
		[DllImport("kernel32.dll")]
		public static extern bool IsBadReadPtr(IntPtr lpfn, int cb);
		[DllImport("kernel32.dll")]
		public static extern void InitializeCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
		[DllImport("kernel32.dll")]
		public static extern void EnterCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
		[DllImport("kernel32.dll")]
		public static extern void LeaveCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string lpModuleName);
		[DllImport("kernel32.dll")]
		public static extern bool GetModuleHandleEx(int flags, string lpModuleName, out IntPtr moduleHandle);

		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);
		[DllImport("kernel32.dll")]
		private static extern bool ReadProcessMemory(uint hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int VirtualQueryEx(uint hProcess, UIntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
		[DllImport("kernel32.dll")]
		private static extern uint OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(uint hObject);
		[DllImport("psapi.dll", SetLastError = true)]
		public static extern bool EnumProcessModulesEx(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, [MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded, ProcessModulesFilterFlags flags);
		[DllImport("psapi.dll", SetLastError = true)]
		static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, uint cb);
		[DllImport("psapi.dll", SetLastError = true)]
		static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);
		[DllImport("psapi.dll")]
		static extern uint GetProcessImageFileName(IntPtr hProcess, [Out] StringBuilder lpImageFileName, [In][MarshalAs(UnmanagedType.U4)] int nSize);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint GetProcessId(uint hProcess);
		[DllImport("kernel32.dll")]
		public static extern bool GetProcessWorkingSetSize(uint hProcess, out uint lpMinimumWorkingSetSize, out uint lpMaximumWorkingSetSize);
		[DllImport("kernel32.dll", SetLastError = false)]
		public static extern void GetSystemInfo(out SYSTEM_INFO Info);
		[DllImport("kernel32.dll")]
		static public extern bool Module32First(int hSnapshot, ref MODULEENTRY32 lpme);
		[DllImport("kernel32.dll")]
		static public extern bool Module32Next(int hSnapshot, ref MODULEENTRY32 lpme);
		[DllImport("kernel32.dll")]
		static public extern bool Process32First(int hSnapshot, ref PROCESSENTRY32 lppe);
		[DllImport("kernel32.dll")]
		static public extern bool Process32Next(int hSnapshot, ref PROCESSENTRY32 lppe);
		[DllImport("kernel32.dll", SetLastError = true)]
		static public extern int CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);
		[DllImport("kernel32.dll", SetLastError = true)]
		static public extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
		[DllImport("shell32.dll", SetLastError = true)]
		private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);
		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		public static string GetExeDirectory(string appendPath = null)
        {
			var fileLocation = Assembly.GetExecutingAssembly().Location;
			var file = new FileInfo(fileLocation);

			if (appendPath != null)
            {
				return Path.Combine(file.DirectoryName, appendPath);
            }

			return file.DirectoryName;
        }

		public static string[] CommandLineToArgs(string commandLine)
		{
			int argc;
			var argv = CommandLineToArgvW(commandLine, out argc);

			if (argv == IntPtr.Zero)
			{
				throw new System.ComponentModel.Win32Exception();
			}

			try
			{
				var args = new string[argc];

				for (var i = 0; i < args.Length; i++)
				{
					var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
					args[i] = Marshal.PtrToStringUni(p);
				}

				return args;
			}
			finally
			{
				Marshal.FreeHGlobal(argv);
			}
		}

		public static T GetModuleFunction<T>(IntPtr hModule, string procName)
		{
			var functionPtr = GetProcAddress(hModule, procName);
			var funcDelegate = Marshal.GetDelegateForFunctionPointer(functionPtr, typeof(T));

			return (T)(object) funcDelegate;
		}

		public static bool CanAccess(this Process process)
		{
			try
			{
				if (process.Id != 0 && !process.HasExited)
				{
					return true;
				}
			}
			catch
			{
			}

			return false;
		}

		public static PlatformProcess GetPlatformProcess(this Process process)
        {
			var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process Where ProcessID=" + process.Id;

			using (var searcher = new ManagementObjectSearcher(wmiQueryString))
			using (var results = searcher.Get())
			{
				var query = from p in Process.GetProcesses()
					join mo in results.Cast<ManagementObject>()
					on p.Id equals (int)(uint)mo["ProcessId"]
					select new PlatformProcess()
					{
						Process = p,
						Path = (string)mo["ExecutablePath"],
						CommandLine = (string)mo["CommandLine"],
					};
				return query.FirstOrDefault();
			}
		}

		public static IEnumerable<PlatformProcess> GetAllProcesses()
        {
			var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";

			using (var searcher = new ManagementObjectSearcher(wmiQueryString))
			using (var results = searcher.Get())
			{
				var query = from p in Process.GetProcesses()
							join mo in results.Cast<ManagementObject>()
							on p.Id equals (int)(uint)mo["ProcessId"]
							select new PlatformProcess()
							{
								Process = p,
								Path = (string)mo["ExecutablePath"],
								CommandLine = (string)mo["CommandLine"],
							};
				foreach (var item in query)
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<Process> GetChildProcesses(this Process process)
		{
			var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + process.Id);
			var collection = searcher.Get();

			foreach (var managementBaseObject in collection)
			{
				var id = Convert.ToInt32(managementBaseObject["ProcessID"]);
				var childProcess = Process.GetProcessById(id);

				if (childProcess != null)
				{
					yield return childProcess;
				}
			}

			yield break;
		}

		public static IEnumerable<Process> GetDescendantProcesses(this Process process)
		{
			foreach (var childProcess in process.GetChildProcesses())
			{
				yield return childProcess;

				foreach (var descendant in childProcess.GetDescendantProcesses())
				{
					yield return descendant;
				}
			}

			yield break;
		}

		public static string FindExePath(string exe)
		{
			exe = Environment.ExpandEnvironmentVariables(exe);

			if (!File.Exists(exe))
			{
				if (Path.GetDirectoryName(exe) == String.Empty)
				{
					foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
					{
						string path = test.Trim();

						if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
						{
							return Path.GetFullPath(path);
						}
					}
				}

				if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Programs\Microsoft VS Code\" + exe)))
				{
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\" + exe);
				}
				else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\" + exe)))
                {
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\" + exe);
                }

				throw new FileNotFoundException(new FileNotFoundException().Message, exe);
			}
			
			return Path.GetFullPath(exe);
		}

		public static void TerminateProcess()
		{
			var hProcess = Process.GetCurrentProcess().Handle;

			TerminateProcess(hProcess, 0);
		}

		public static bool AlreadyRunning
		{
			get
			{
				var currentProcess = Process.GetCurrentProcess();
				var processes = Process.GetProcessesByName(currentProcess.ProcessName);

				foreach (var process in processes)
				{
					if (process != null && process.Id != Process.GetCurrentProcess().Id)
					{
						return true;
					}
				}

				return false;
			}
		}

		public static List<MODULEENTRY32> GetModules(this Process process)
		{
			var list = new List<MODULEENTRY32>();
			var moduleEntry = new MODULEENTRY32();
			var hModuleSnap = CreateToolhelp32Snapshot(SnapshotFlags.Module, (uint)process.Id);

			moduleEntry.dwSize = (uint)moduleEntry.SizeOf();

			if (Module32First(hModuleSnap, ref moduleEntry))
			{
				do
				{
					list.Add(moduleEntry);
				} 
				while (Module32Next(hModuleSnap, ref moduleEntry));
			}

			return list;
		}

		public static string GetProcessFileName(IntPtr hProcess)
		{
			var builder = new StringBuilder(260);

			Debug.Assert(GetProcessImageFileName(hProcess, builder, (int)builder.Capacity) > 0);

			return builder.ToString();
		}

		public static IDictionary<string, MODULEINFO> GetModules(IntPtr hProcess)
		{
			var dictionary = new Dictionary<string, MODULEINFO>();
			var builder = new StringBuilder(260);
			var hModule = IntPtr.Zero;
			uint cbNeeded = 0;

			while (EnumProcessModulesEx(hProcess, hModule, (uint)Marshal.SizeOf(typeof(IntPtr)), out cbNeeded, ProcessModulesFilterFlags.ListModulesDefault))
			{
				Debug.Assert(GetModuleBaseName(hProcess, hModule, builder, (uint)builder.Capacity) > 0);
				var name = builder.ToString();
				var moduleInfo = new MODULEINFO();

				Debug.Assert(GetModuleInformation(hProcess, hModule, out moduleInfo, (uint) Marshal.SizeOf(moduleInfo)));
			}

			return dictionary;
		}

		private static string FindIndexedProcessName(int pid)
		{
			var processName = Process.GetProcessById(pid).ProcessName;
			var processesByName = Process.GetProcessesByName(processName);
			string processIndexdName = null;

			for (var index = 0; index < processesByName.Length; index++)
			{
				try
				{
					processIndexdName = index == 0 ? processName : processName + "#" + index;
					var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);

					if ((int)processId.NextValue() == pid)
					{
						return processIndexdName;
					}
				}
				catch
                {
                }
			}

			return processIndexdName;
		}

		private static Process FindPidFromIndexedProcessName(string indexedProcessName)
		{
			var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
			return Process.GetProcessById((int)parentId.NextValue());
		}

		public static Process GetParent(this Process process)
		{
			return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
		}

		public static byte[] ReadProcessMemory(this Process process, ulong address, int dwSize)
		{
			byte[] bytes;
			var hProcess = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, (uint)process.Id);

			bytes = ReadProcessMemory(hProcess, address, dwSize);

			CloseHandle(hProcess);

			return bytes;
		}

		public static byte[] ReadProcessMemory(this Process process, IntPtr address, int dwSize)
		{
			var bytes = new byte[dwSize];
			int bytesRead = 0;
			var hProcess = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, (uint)process.Id);

			ReadProcessMemory(hProcess, address, bytes, dwSize, ref bytesRead);

			CloseHandle(hProcess);

			return bytes;
		}

		public static int WriteProcessMemory(this Process process, ulong address, byte[] bytes)
		{
			int bytesWritten;
			var hProcess = OpenProcess(ProcessAccessFlags.VirtualMemoryWrite, false, (uint)process.Id);

			WriteProcessMemory((IntPtr)hProcess, (IntPtr)address, bytes, bytes.Length, out bytesWritten);

			CloseHandle(hProcess);

			return bytesWritten;
		}

		public static int WriteProcessMemory(this Process process, IntPtr address, byte[] bytes)
		{
			int bytesWritten;
			var hProcess = OpenProcess(ProcessAccessFlags.All, false, (uint)process.Id);

			WriteProcessMemory((IntPtr)hProcess, address, bytes, bytes.Length, out bytesWritten);

			CloseHandle(hProcess);

			return bytesWritten;
		}

		public static MEMORY_BASIC_INFORMATION GetMemoryInfo(uint hProcess, ulong address)
		{
			var memBasicInfo = new MEMORY_BASIC_INFORMATION();
			var processId = GetProcessId(hProcess);

			if (VirtualQueryEx(hProcess, new UIntPtr((ulong)address), out memBasicInfo, (uint)memBasicInfo.SizeOf()) > 0)
			{
				return memBasicInfo;
			}

			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public static byte[] ReadProcessMemory(uint hProcess, ulong address, int dwSize)
		{
			ulong minAddress = address;
			ulong maxAddress = address + (ulong) dwSize;
			MEMORY_BASIC_INFORMATION memBasicInfo;
			var bytes = new byte[0];
			IntPtr regionSize = IntPtr.Zero;

			memBasicInfo = new MEMORY_BASIC_INFORMATION();

			while (minAddress < maxAddress)
			{
				var infoSize = VirtualQueryEx(hProcess, new UIntPtr((ulong)minAddress), out memBasicInfo, (uint) memBasicInfo.SizeOf());

				regionSize = memBasicInfo.RegionSize;

				if (memBasicInfo.Protect == AllocationProtectEnum.Readonly | memBasicInfo.Protect == AllocationProtectEnum.ReadWrite | memBasicInfo.Protect == AllocationProtectEnum.ExecuteRead | memBasicInfo.Protect == AllocationProtectEnum.ExecuteReadWrite)
				{
					int bytesRead = 0;
					var buffer = new byte[(int) regionSize];

					ReadProcessMemory(hProcess, memBasicInfo.BaseAddress, buffer, (int) regionSize, ref bytesRead);

					if (bytesRead.As<int>() > 0)
					{
						if (bytesRead == (int) regionSize)
						{
							bytes = bytes.Append(buffer);
						}
						else
						{
							bytes = bytes.Append(buffer.PadRight((int) regionSize));
						}
					}
				}
				else
				{
					bytes = bytes.Append(new byte[(int) regionSize]);
				}

				minAddress += (ulong) regionSize;
			}

			return bytes;
		}

		public static void Kill(this Process[] processes, bool throwExceptions = false)
		{
			foreach (var process in processes)
			{
				try
				{
					process.Kill();
				}
				catch
				{
					if (throwExceptions)
					{
						throw;
					}
				}
			}
		}

		public static void Kill(this IEnumerable<Process> processes, bool throwExceptions = false)
		{
			foreach (var process in processes)
			{
				try
				{
					process.Kill();
				}
				catch
				{
					if (throwExceptions)
					{
						throw;
					}
				}
			}
		}

		public static void Kill(this Process currentProcess, string name, bool throwExceptions = false)
		{
			var processes = Process.GetProcessesByName(name);

			foreach (var process in processes)
			{
				try
				{
					process.Kill();
				}
				catch
				{
					if (throwExceptions)
					{
						throw;
					}
				}
			}
		}

		public static bool IsWow64(this Process process)
		{
			if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
			{
				try
				{
					bool retVal;

					return IsWow64Process(process.Handle, out retVal) && retVal;
				}
				catch
				{
					return false; // access is denied to the process
				}
			}

			return false; // not on 64-bit Windows
		}

		public static bool Is64Bit(this Process process)
		{
			if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
			{
				try
				{
					bool retVal;

					if (IsWow64Process(process.Handle, out retVal))
					{
						return !retVal;
					}
				}
				catch
				{
					return false; // access is denied to the process
				}
			}

			return false; // not on 64-bit Windows
		}
	}
}
