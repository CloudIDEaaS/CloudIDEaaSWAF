using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Reflection;
using System.Xml.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO.Packaging;
#if !SILVERLIGHT
using System.Drawing;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Utils.ProcessHelpers;

[StructLayout(LayoutKind.Sequential)]
public struct SHFILEINFO
{
    public IntPtr hIcon; 
    public int iIcon;
    public uint dwAttributes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szDisplayName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string szTypeName;
};

static class Win32
{
    public const uint SHGFI_ICON = 0x100;
    public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
    public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

    [DllImport("shell32.dll")]
    public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
    [DllImport("kernel32.dll")]
    public static extern bool CreateDirectory(string lpNewDirectory, IntPtr lpSecurityAttributes);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool RemoveDirectory(string lpPathName);
    [DllImport("shell32.dll")]
    public static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwFlags, [Out] StringBuilder pszPath);
}

#endif

namespace Utils
{
    using Microsoft.Win32;
    using System.Drawing.Imaging;
    using System.IO.Pipelines;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Windows.Forms;
    using Utils.IO;
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
    using Utils.IO;
#endif
    using static Utils.FileOperation;

    public enum SpecialFolderType
    {
        // Version 5.0. The file system directory that is used
        // to store administrative tools for an individual user.
        // The Microsoft Management Console (MMC) will save customized
        // consoles to this directory, and it will roam with the user.
        AdministrativeTools = 0x0030,

        // Version 5.0. The file system directory containing
        // administrative tools for all users of the computer.
        CommonAdministrativeTools = 0x002f,

        // Version 4.71. The file system directory that serves as
        // a common repository for application-specific data.
        // A typical path is C:\Documents and Settings\username\Application Data.
        // This CSIDL is supported by the redistributable Shfolder.dll
        // for systems that do not have the Microsoft Internet Explorer 4.0
        // integrated Shell installed
        ApplicationData = 0x001a,

        // Version 5.0. The file system directory containing
        // application data for all users. A typical path is
        // C:\Documents and Settings\All Users\Application Data.
        CommonAppData = 0x0023,

        // The file system directory that contains documents
        // that are common to all users. A typical paths is
        // C:\Documents and Settings\All Users\Documents.
        // Valid for Windows NT systems and Microsoft Windows 95 and
        // Windows 98 systems with Shfolder.dll installed.
        CommonDocuments = 0x002e,

        // The file system directory that serves as a common repository
        // for Internet cookies. A typical path is
        // C:\Documents and Settings\username\Cookies.
        Cookies = 0x0021,

        // Version 5.0. Combine this CSIDL with any of the following CSIDLs
        // to force the creation of the associated folder.
        CreateFlag = 0x8000,

        // The file system directory that serves as a common repository
        // for Internet history items.
        History = 0x0022,

        // Version 4.72. The file system directory that serves as
        // a common repository for temporary Internet files. A typical
        // path is C:\Documents and Settings\username\Local Settings\Temporary Internet Files.
        InternetCache = 0x0020,

        // Version 5.0. The file system directory that serves as a data
        // repository for local (nonroaming) applications. A typical path
        // is C:\Documents and Settings\username\Local Settings\Application Data.
        LocalApplicationData = 0x001c,

        // Version 5.0. The file system directory that serves as
        // a common repository for image files. A typical path is
        // C:\Documents and Settings\username\My Documents\My Pictures.
        MyPictures = 0x0027,

        // Version 6.0. The virtual folder representing the My Documents
        // desktop item. This is equivalent to CSIDL_MYDOCUMENTS.
        // Previous to Version 6.0. The file system directory used to
        // physically store a user's common repository of documents.
        // A typical path is C:\Documents and Settings\username\My Documents.
        // This should be distinguished from the virtual My Documents folder
        // in the namespace. To access that virtual folder,
        // use SHGetFolderLocation, which returns the ITEMIDLIST for the
        // virtual location, or refer to the technique described in
        // Managing the File System.
        Personal = 0x0005,

        // Version 5.0. The Program Files folder. A typical
        // path is C:\Program Files.
        ProgramFiles = 0x0026,

        // Version 5.0. A folder for components that are shared across
        // applications. A typical path is C:\Program Files\Common.
        // Valid only for Windows NT, Windows 2000, and Windows XP systems.
        // Not valid for Windows Millennium Edition (Windows Me).
        CommonProgramFiles = 0x002b,

        // Version 5.0. The Windows System folder. A typical
        // path is C:\Windows\System32.
        System = 0x0025,

        // Version 5.0. The Windows directory or SYSROOT.
        // This corresponds to the %windir% or %SYSTEMROOT% environment
        // variables. A typical path is C:\Windows.
        Windows = 0x0024,

        // Fonts directory. A typical path is c:\Windows\Fonts.
        Fonts = 0x0014
    }

    public enum SpecialFolderCSIDL : int
    {
        CSIDL_DESKTOP = 0x0000,    // <desktop>
        CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
        CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs
        CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel
        CSIDL_PRINTERS = 0x0004,    // My Computer\Printers
        CSIDL_PERSONAL = 0x0005,    // My Documents
        CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites
        CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup
        CSIDL_RECENT = 0x0008,    // <user name>\Recent
        CSIDL_SENDTO = 0x0009,    // <user name>\SendTo
        CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin
        CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu
        CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop
        CSIDL_DRIVES = 0x0011,    // My Computer
        CSIDL_NETWORK = 0x0012,    // Network Neighborhood
        CSIDL_NETHOOD = 0x0013,    // <user name>\nethood
        CSIDL_FONTS = 0x0014,    // windows\fonts
        CSIDL_TEMPLATES = 0x0015,
        CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu
        CSIDL_COMMON_PROGRAMS = 0x0017,    // All Users\Programs
        CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup
        CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
        CSIDL_APPDATA = 0x001a,    // <user name>\Application Data
        CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood
        CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)
        CSIDL_ALTSTARTUP = 0x001d,    // non localized startup
        CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
        CSIDL_COMMON_FAVORITES = 0x001f,
        CSIDL_INTERNET_CACHE = 0x0020,
        CSIDL_COOKIES = 0x0021,
        CSIDL_HISTORY = 0x0022,
        CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data
        CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()
        CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()
        CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files
        CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures
        CSIDL_PROFILE = 0x0028,    // USERPROFILE
        CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC
        CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC
        CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common
        CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
        CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates
        CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents
        CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools
        CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools
        CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections
        CSIDL_CDBURN_AREA = 0x003B,    // Data for burning with interface ICDBurn
    };

    public class FileOperation
    {
        public enum FO_Func : uint
        {
            FO_MOVE = 0x0001,
            FO_COPY = 0x0002,
            FO_DELETE = 0x0003,
            FO_RENAME = 0x0004,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
        struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FO_Func wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public ushort fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;

        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHFileOperation([In, Out] ref SHFILEOPSTRUCT lpFileOp);

        private SHFILEOPSTRUCT _ShFile;

        public FILEOP_FLAGS fFlags;

        public IntPtr hwnd
        {
            set
            {
                this._ShFile.hwnd = value;
            }
        }
        public FO_Func wFunc
        {
            set
            {
                this._ShFile.wFunc = value;
            }
        }

        public string pFrom
        {
            set
            {
                this._ShFile.pFrom = value + '\0' + '\0';
            }
        }
        public string pTo
        {
            set
            {
                this._ShFile.pTo = value + '\0' + '\0';
            }
        }

        public bool fAnyOperationsAborted
        {
            set
            {
                this._ShFile.fAnyOperationsAborted = value;
            }
        }
        public IntPtr hNameMappings
        {
            set
            {
                this._ShFile.hNameMappings = value;
            }
        }
        public string lpszProgressTitle
        {
            set
            {
                this._ShFile.lpszProgressTitle = value + '\0';
            }
        }

        public FileOperation()
        {
            this.fFlags = new FILEOP_FLAGS();
            this._ShFile = new SHFILEOPSTRUCT();
            this._ShFile.hwnd = IntPtr.Zero;
            this._ShFile.wFunc = FO_Func.FO_COPY;
            this._ShFile.pFrom = "";
            this._ShFile.pTo = "";
            this._ShFile.fAnyOperationsAborted = false;
            this._ShFile.hNameMappings = IntPtr.Zero;
            this._ShFile.lpszProgressTitle = "";

        }

        public bool Execute()
        {
            this._ShFile.fFlags = this.fFlags.Flag;
            return SHFileOperation(ref this._ShFile) == 0;//true if no errors
        }

        public class FILEOP_FLAGS
        {
            [Flags]
            private enum FILEOP_FLAGS_ENUM : ushort
            {
                FOF_MULTIDESTFILES = 0x0001,
                FOF_CONFIRMMOUSE = 0x0002,
                FOF_SILENT = 0x0004,  // don't create progress/report
                FOF_RENAMEONCOLLISION = 0x0008,
                FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
                FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
                                                 // Must be freed using SHFreeNameMappings
                FOF_ALLOWUNDO = 0x0040,
                FOF_FILESONLY = 0x0080,  // on *.*, do only files
                FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
                FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
                FOF_NOERRORUI = 0x0400,  // don't put up error UI
                FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
                FOF_NORECURSION = 0x1000,  // don't recurse into directories.
                FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected elements.
                FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
                FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers
            }

            public bool FOF_MULTIDESTFILES = false;
            public bool FOF_CONFIRMMOUSE = false;
            public bool FOF_SILENT = false;
            public bool FOF_RENAMEONCOLLISION = false;
            public bool FOF_NOCONFIRMATION = false;
            public bool FOF_WANTMAPPINGHANDLE = false;
            public bool FOF_ALLOWUNDO = false;
            public bool FOF_FILESONLY = false;
            public bool FOF_SIMPLEPROGRESS = false;
            public bool FOF_NOCONFIRMMKDIR = false;
            public bool FOF_NOERRORUI = false;
            public bool FOF_NOCOPYSECURITYATTRIBS = false;
            public bool FOF_NORECURSION = false;
            public bool FOF_NO_CONNECTED_ELEMENTS = false;
            public bool FOF_WANTNUKEWARNING = false;
            public bool FOF_NORECURSEREPARSE = false;

            public ushort Flag
            {
                get
                {
                    ushort ReturnValue = 0;

                    if (this.FOF_MULTIDESTFILES)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_MULTIDESTFILES;
                    if (this.FOF_CONFIRMMOUSE)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_CONFIRMMOUSE;
                    if (this.FOF_SILENT)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SILENT;
                    if (this.FOF_RENAMEONCOLLISION)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_RENAMEONCOLLISION;
                    if (this.FOF_NOCONFIRMATION)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMATION;
                    if (this.FOF_WANTMAPPINGHANDLE)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTMAPPINGHANDLE;
                    if (this.FOF_ALLOWUNDO)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_ALLOWUNDO;
                    if (this.FOF_FILESONLY)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_FILESONLY;
                    if (this.FOF_SIMPLEPROGRESS)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SIMPLEPROGRESS;
                    if (this.FOF_NOCONFIRMMKDIR)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMMKDIR;
                    if (this.FOF_NOERRORUI)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOERRORUI;
                    if (this.FOF_NOCOPYSECURITYATTRIBS)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCOPYSECURITYATTRIBS;
                    if (this.FOF_NORECURSION)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSION;
                    if (this.FOF_NO_CONNECTED_ELEMENTS)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NO_CONNECTED_ELEMENTS;
                    if (this.FOF_WANTNUKEWARNING)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTNUKEWARNING;
                    if (this.FOF_NORECURSEREPARSE)
                        ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSEREPARSE;

                    return ReturnValue;
                }
            }
        }

    }

    public static class IOExtensions
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] b1, byte[] b2, long count);
        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        public static extern void ZeroMemory(IntPtr dest, uint size);
        [DllImport("kernel32.dll")]
        public static extern uint OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);

        [Flags]
        public enum ProcessAccessFlags : uint
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

        public static string ToFileUri(this FileSystemInfo fileSystemInfo)
        {
            string uriPath;
            var uri = new System.Uri(fileSystemInfo.FullName);
            
            uriPath = uri.AbsoluteUri;

            return uriPath;
        }

        public static void Prepend(this FileStream file, string value)
        {
            PrependString(value, file);
        }

        public static void PrependString(string value, FileStream file)
        {
            byte[] numArray = new byte[(int)file.Length];
            file.Position = (long)0;

            while (file.Read(numArray, 0, (int)numArray.Length) != 0)
            {
            }

            if (!file.CanWrite)
            {
                throw new ArgumentException("The specified file cannot be written.", "file");
            }

            file.Position = (long)0;
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            
            file.SetLength((long)((int)numArray.Length + (int)bytes.Length));
            file.Write(bytes, 0, (int)bytes.Length);
            file.Write(numArray, 0, (int)numArray.Length);
        }

        public static string GetAppDataDirectory(string app = null)
        {
            if (app == null)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), app);
            }
        }

        public static uint Checksum(this string value)
        {
            byte[] inputData = Encoding.ASCII.GetBytes(value);
            uint sum = 0;

            for (var i = 0; i < inputData.Length; i++)
            {
                int product = inputData[i] & 0x7F; // Take the low 7 bits from the record.
                product *= i + 1; // Multiply by the 1 based position.
                sum += (uint) product; // Add the product to the running sum.
            }

            return sum;
        }

        public static string GetDocumentsDirectory()
        {
            const int MaxPath = 260;
            StringBuilder builder = new StringBuilder(MaxPath);

            Win32.SHGetFolderPath(IntPtr.Zero, (int) SpecialFolderCSIDL.CSIDL_PERSONAL, IntPtr.Zero, 0x0000, builder);

            return builder.ToString();
        }

        static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }
        public const int MAX_PATH = 260;
#if !SILVERLIGHT
        internal static Dictionary<TextWriter, Stack<TagHandler>> tagHandlerStack;

        static IOExtensions()
        {
            tagHandlerStack = new Dictionary<TextWriter, Stack<TagHandler>>();
        }
#endif
#region Watchers
#if !SILVERLIGHT
        private static List<FileSystemWatcher> watchers;

        public static byte[] ReadToEnd(this Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".exif":
                    return ImageFormat.Exif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;
                default:
                    throw new NotSupportedException(
                        "Unknown file extension " + extension);
            }

        }

        public static string GetExtension(this ImageFormat imageFormat)
        {
            if (imageFormat.Equals(ImageFormat.Jpeg)) return ".jpg";
            else if (imageFormat.Equals(ImageFormat.Png)) return ".png";
            else if (imageFormat.Equals(ImageFormat.Gif)) return ".gif";
            else if (imageFormat.Equals(ImageFormat.Bmp)) return ".bmp";
            else if (imageFormat.Equals(ImageFormat.Tiff)) return ".tif";
            else if (imageFormat.Equals(ImageFormat.Icon)) return ".ico";
            else if (imageFormat.Equals(ImageFormat.Emf)) return ".emf";
            else if (imageFormat.Equals(ImageFormat.Wmf)) return ".wmf";
            else if (imageFormat.Equals(ImageFormat.Exif)) return ".exif";
            else if (imageFormat.Equals(ImageFormat.MemoryBmp)) return ".bmp";
            return ".unknown";
        }

        public static string GetMimeType(this ImageFormat imageFormat)
        {
            if (imageFormat.Equals(ImageFormat.Jpeg)) return GetMimeType(".jpg");
            else if (imageFormat.Equals(ImageFormat.Png)) return GetMimeType(".png");
            else if (imageFormat.Equals(ImageFormat.Gif)) return GetMimeType(".gif");
            else if (imageFormat.Equals(ImageFormat.Bmp)) return GetMimeType(".bmp");
            else if (imageFormat.Equals(ImageFormat.Tiff)) return GetMimeType(".tif");
            else if (imageFormat.Equals(ImageFormat.Icon)) return GetMimeType(".ico");
            else if (imageFormat.Equals(ImageFormat.Emf)) return GetMimeType(".emf");
            else if (imageFormat.Equals(ImageFormat.Wmf)) return GetMimeType(".wmf");
            else if (imageFormat.Equals(ImageFormat.Exif)) return GetMimeType(".exif");
            else if (imageFormat.Equals(ImageFormat.MemoryBmp)) return GetMimeType(".bmp");
            return ".unknown";
        }

        public static string GetRelativePath(this FileInfo file, string basePath)
        {
            return FileUtilities.MakeRelative(basePath, file.FullName);
        }

        public static bool Compare(this byte[] b1, byte[] b2)
        {
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        public static bool HasFile(this DirectoryInfo directoryInfo, string fileName)
        {
            var fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).SingleOrDefault(f => f.Name.AsCaseless() == fileName || f.Name.IsWildcardMatch(fileName));

            return fileInfo != null;
        }

        public static FileInfo FindFile(this DirectoryInfo directoryInfo, string fileName)
        {
            var fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).SingleOrDefault(f => f.Name.AsCaseless() == fileName || f.Name.IsWildcardMatch(fileName));

            return fileInfo;
        }

        public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directoryInfo, string fileName)
        {
            var fileInfos = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Where(f => f.Name.AsCaseless() == fileName || f.Name.IsWildcardMatch(fileName));

            return fileInfos;
        }

        public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directoryInfo, params string[] fileNames)
        {
            var fileInfos = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Where(f => fileNames.Any(f2 => f2.AsCaseless() == f.Name));

            return fileInfos;
        }

        public static DirectoryInfo FindDirectory(this DirectoryInfo directoryInfo, string directoryName)
        {
            var directoryInfoFind = directoryInfo.GetDirectories("*.*", SearchOption.AllDirectories).FirstOrDefault(f => f.Name.AsCaseless() == directoryName);

            return directoryInfoFind;
        }

        public static FileInfo FindLastFile<TKey>(this DirectoryInfo directoryInfo, string fileName, Func<FileInfo, TKey> orderBy)
        {
            var fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).OrderBy(orderBy).LastOrDefault(f => f.Name.AsCaseless() == fileName);

            return fileInfo;
        }

        public static string GetShortenedPath(this FileInfo file, int maxLength = 60)
        {
            if (file.FullName.Length > maxLength)
            {
                var path = file.FullName;
                var left = path.LeftUpToIndexOf('\\');
                var right = path.Right(maxLength - left.Length - @"\...\".Length);

                right = right.RightAtIndexOf('\\');

                return left + @"\...\" + right;
            }
            else
            {
                return file.FullName;
            }
        }

        public static string GetRelativePath(this DirectoryInfo directory, string basePath)
        {
            return FileUtilities.MakeRelative(basePath, directory.FullName);
        }

        public static void WriteAll(this Stream target, Stream source)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;

            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
        }

        public static string GetDefaultExtension(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        public static string GetMimeType(this FileInfo fileInfo)
        {
            return GetMimeType(fileInfo.Extension);
        }

        public static string GetMimeType(string extension)
        {
            foreach (var subKey in Registry.ClassesRoot.ToIndexable().SubKeys)
            {
                if (subKey.SubName.AsCaseless() == extension)
                {
                    var contentType = (string) subKey.Key.GetValue("Content Type");

                    return contentType;
                }
            }

            foreach (var subKey in Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type").ToIndexable().SubKeys)
            {
                if (((string)subKey.Key.GetValue("Extension")).AsCaseless() == extension)
                {
                    return subKey.SubName;
                }
            }

            return null;
        }

        public static string GetExactPath(this FileSystemInfo fileSystemInfo)
        {
            var pathName = fileSystemInfo.FullName;
            var directory = new DirectoryInfo(pathName);

            if (!(File.Exists(pathName) || Directory.Exists(pathName)))
            {
                return null;
            }

            if (directory.Parent != null)
            {
                return Path.Combine(directory.Parent.GetExactPath(), directory.Parent.GetFileSystemInfos(directory.Name)[0].Name);
                
            }
            else
            {
                return directory.Name.ToUpper();
            }
        }

        public static bool CreateShortcut(this FileInfo fileInfo, string shortcutFolder)
        {
            var shortcutFile = Path.Combine(shortcutFolder, fileInfo.Name + ".lnk");
            var shortcutTarget = "\"" + fileInfo.FullName + "\"";
            var obj = Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046"), true);
            var shellLink = Activator.CreateInstance(obj) as IShellLinkW;
            IPersistFile persistFile;

            if (shellLink != null)
            {
                shellLink.SetPath(shortcutTarget);
                shellLink.SetDescription(shortcutTarget);

                persistFile = (IPersistFile)shellLink;

                persistFile.Save(shortcutFile, true);

                Marshal.ReleaseComObject(persistFile);
                Marshal.ReleaseComObject(shellLink);

                return true;
            }

            return false;
        }

        public static string RemoveInvalidFileNameCharacters(string path)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();

            return string.Join(string.Empty, path.Where(c => !invalids.Any(i => i == c)));
        }

        public static bool CompareTo(this DirectoryInfo dir1, DirectoryInfo dir2)
        {
            // Take a snapshot of the file system.  
            var list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            var list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            var fileCompare = new FileCompare();

            // This query determines whether the two folders contain  
            // identical file lists, based on the custom file comparer  
            // that is defined in the FileCompare class.  
            // The query executes immediately because it returns a bool.  

            return list1.SequenceEqual(list2, fileCompare);
        }

        public static bool CompareTo(this DirectoryInfo dir1, DirectoryInfo dir2, out TimeSpan timeSpan)
        {
            // Take a snapshot of the file system.  
            var elapsed = TimeSpan.MinValue;
            bool result;

            using (dir1.StartStopwatch((e) => elapsed = e))
            {
                var list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                var list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                var fileCompare = new FileCompare();

                // This query determines whether the two folders contain  
                // identical file lists, based on the custom file comparer  
                // that is defined in the FileCompare class.  
                // The query executes immediately because it returns a bool.  

                result = list1.SequenceEqual(list2, fileCompare);
            }

            timeSpan = elapsed;

            return result;
        }

        public static void OnChangedOrDeleted(this FileInfo file, Action<FileSystemEventArgs> action)
        {
            var watcher = new FileSystemWatcher(file.DirectoryName, file.Name);

            if (watchers == null)
            {
                watchers = new List<FileSystemWatcher>();
            }

            watchers.Add(watcher);

            watcher.Changed += (sender, e) =>
            {
                action(e);

                watchers.Remove(watcher);
            };

            watcher.Deleted += (sender, e) =>
            {
                action(e);

                watchers.Remove(watcher);
            };

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        public static void OnCreatedOrChanged(this FileInfo file, Action<FileSystemEventArgs> action, bool raiseIfExists = true)
        {
            var creationTime = DateTime.MinValue;
            var modifiedTime = DateTime.MinValue;

            if (File.Exists(file.FullName))
            {
                var fileCheck = new FileInfo(file.FullName);

                creationTime = fileCheck.CreationTime;
                modifiedTime = fileCheck.LastWriteTime;
            }

            OnCreatedOrChanged(file, creationTime, modifiedTime, action);
        }

        public static bool FastCopyTo(this DirectoryInfo sourceDirectory, string targetDirectory)
        {
            var fileOperation = new FileOperation
            {
                pFrom = sourceDirectory.FullName,
                pTo = targetDirectory,
                wFunc = FileOperation.FO_Func.FO_COPY,
                fFlags = new FILEOP_FLAGS
                {
                    FOF_NOCONFIRMATION = true,
                    FOF_NOERRORUI = true,
                    FOF_SILENT = true,
                    FOF_NOCOPYSECURITYATTRIBS = true
                }
            };

            return fileOperation.Execute();
        }

        public static void AssurePathCreated(this FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is DirectoryInfo directory)
            {
                if (!directory.Exists)
                {
                    directory.Create();
                }
            }
            else if (fileSystemInfo is FileInfo fileInfo)
            {
                fileInfo.Directory.AssurePathCreated();
            }
            else
            {
                DebugUtils.Break();
            }
        }

        public static void OpenFileLocation(this FileSystemInfo fileSystemInfo)
        {
            var filePath = fileSystemInfo.FullName;

            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", "/select, " + filePath.SurroundWithQuotes());
            }
        }

        public static void CopyTo(this DirectoryInfo sourceDirectory, string targetDirectory, bool overwrite, Func<FileSystemInfo, bool> filter)
        {
            sourceDirectory.CopyTo(targetDirectory, overwrite, false, filter);
        }

        public static string RemoveInvalidPathChars(this string str)
        {
            var chars = Path.GetInvalidFileNameChars();

            return string.Join(string.Empty, str.Where(c => !chars.Contains(c)));
        }

        public static FileInfo CopyTo(this FileInfo file, DirectoryInfo directory)
        {
            var newPath = Path.Combine(directory.FullName, file.Name);

            file.CopyTo(newPath, true);

            return new FileInfo(newPath);
        }

        public static FileInfo MoveTo(this FileInfo file, DirectoryInfo directory)
        {
            var newPath = Path.Combine(directory.FullName, file.Name);

            file.MoveTo(Path.Combine(directory.FullName, file.Name));

            return new FileInfo(newPath);
        }

        public static void CopyTo(this DirectoryInfo sourceDirectory, string targetDirectory, bool overwrite, bool skipErrors = false, Func<FileSystemInfo, bool> filter = null)
        {
            Action<string, string> copy = null;

            if (filter == null)
            {
                filter = (f) => true;
            }

            copy = new Action<string, string>((source, target) =>
            {
                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                foreach (var file in Directory.GetFiles(source))
                {
                    var continueCopy = true;
                    var count = 0;

                    while (continueCopy)
                    {
                        try
                        {
                            if (filter(new FileInfo(file)))
                            {
                                if (!Directory.Exists(target))
                                {
                                    Directory.CreateDirectory(target);
                                }

                                File.Copy(file, Path.Combine(target, Path.GetFileName(file)), overwrite);
                            }

                            continueCopy = false;
                        }
                        catch (Exception ex)
                        {
                            if (skipErrors)
                            {
                                continueCopy = false;
                                Thread.Sleep(100);
                            }
                            else
                            {
                                count++;

                                if (count > 20)
                                {
                                    MessageBox.Show("Exceeded copy retry limit");
                                    Debugger.Launch();
                                }
                            }
                        }
                    }
                }

                foreach (var directory in Directory.GetDirectories(source))
                {
                    if (filter(new DirectoryInfo(directory)))
                    {
                        copy(directory, Path.Combine(target, Path.GetFileName(directory)));
                    }
                }
            });

            copy(sourceDirectory.FullName, targetDirectory);
        }

        public static string GetLastCharsOfFile(string filename, int numChars)
        {
            var fileInfo = new FileInfo(filename);

            using (var stream = File.OpenRead(filename))
            {
                stream.Seek(fileInfo.Length - numChars, SeekOrigin.Begin);

                using (var textReader = new StreamReader(stream))
                {
                    return textReader.ReadToEnd();
                }
            }
        }

        public static string GetSubPart(this DirectoryInfo directory, int offsetPlusOrMinus, int count = 1)
        {
            var parts = directory.FullName.Split('\\');

            if (offsetPlusOrMinus > 0)
            {
                return string.Join(@"\", parts.Skip(offsetPlusOrMinus).Take(count));
            }
            else
            {
                return string.Join(@"\", parts.Reverse().Skip(Math.Abs(offsetPlusOrMinus) - 1).Take(count).Reverse());
            }
        }

        public static void OnCreatedOrChanged(this FileInfo file, DateTime creationTime, DateTime modifiedTime, Action<FileSystemEventArgs> action, bool raiseIfExists = true)
        {
            var directory = file.Directory;

            while (!Directory.Exists(directory.FullName))
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                var watcher = new FileSystemWatcher(directory.FullName, file.Name);

                if (watchers == null)
                {
                    watchers = new List<FileSystemWatcher>();
                }

                if (raiseIfExists && file.Exists)
                {
                    action(new FileSystemEventArgs(WatcherChangeTypes.Created, file.DirectoryName, file.Name));
                }
                else
                {
                    // in case file was created after we created Watcher but before we hooked events

                    var oneTimeAction = new OneTimeTimer(100);

                    oneTimeAction.Start(() =>
                    {
                        if (File.Exists(file.FullName) && watchers.Contains(watcher))
                        {
                            if (creationTime != DateTime.MinValue)
                            {
                                if (creationTime != file.CreationTime)
                                {
                                    action(new FileSystemEventArgs(WatcherChangeTypes.Created, file.DirectoryName, file.Name));
                                }
                            }
                            else if (modifiedTime != DateTime.MinValue)
                            {
                                if (modifiedTime != file.LastWriteTime)
                                {
                                    action(new FileSystemEventArgs(WatcherChangeTypes.Changed, file.DirectoryName, file.Name));
                                }
                            }

                            watchers.Remove(watcher);
                        }
                    });
                }

                watchers.Add(watcher);

                watcher.Created += (sender, e) =>
                {
                    if (e.FullPath.AsCaseless() == file.FullName)
                    {
                        if (watchers.Contains(watcher))
                        {
                            if (creationTime != DateTime.MinValue)
                            {
                                if (creationTime != file.CreationTime)
                                {
                                    action(e);
                                }
                            }
                            else if (modifiedTime != DateTime.MinValue)
                            {
                                if (modifiedTime != file.LastWriteTime)
                                {
                                    action(e);
                                }
                            }

                            watchers.Remove(watcher);
                        }
                    }
                };

                watcher.Changed += (sender, e) =>
                {
                    if (e.FullPath.AsCaseless() == file.FullName)
                    {
                        if (watchers.Contains(watcher))
                        {
                            if (creationTime != DateTime.MinValue)
                            {
                                if (creationTime != file.CreationTime)
                                {
                                    action(e);
                                }
                            }
                            else if (modifiedTime != DateTime.MinValue)
                            {
                                if (modifiedTime != file.LastWriteTime)
                                {
                                    action(e);
                                }
                            }

                            watchers.Remove(watcher);
                        }
                    }
                };

                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
            else
            {
                Debugger.Break();
            }
        }

        public static void OnCreated(this FileInfo file, Action action)
        {
            var directory = file.Directory;

            while (!Directory.Exists(directory.FullName))
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                var watcher = new FileSystemWatcher(directory.FullName);

                if (watchers == null)
                {
                    watchers = new List<FileSystemWatcher>();
                }

                if (File.Exists(file.FullName))
                {
                    action();
                    return;
                }
                else
                {
                    // in case file was created after we created Watcher but before we hooked events

                    var oneTimeAction = new OneTimeTimer(100);

                    oneTimeAction.Start(() =>
                    {
                        if (File.Exists(file.FullName) && watchers.Contains(watcher))
                        {
                            watchers.Remove(watcher);
                            action();
                        }
                        else
                        {
                            Debug.WriteLine("'{0}' still does not exist", file.FullName);
                        }
                    });
                }

                Debug.WriteLine("Watching '{0}'", directory.FullName);

                watchers.Add(watcher);

                watcher.Created += (sender, e) =>
                {
                    Debug.WriteLine("'{0}' created", e.FullPath);

                    if (e.FullPath.AsCaseless() == file.FullName)
                    {
                        if (watchers.Contains(watcher))
                        {
                            watchers.Remove(watcher);
                            action();
                        }
                    }
                };

                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
            else
            {
                Debugger.Break();
            }
        }

        public static void OnReadOnlyChanged(this FileInfo file, Action action)
        {
            var isReadOnly = file.IsReadOnly;

            var thread = new Thread(() =>
            {
                file = new FileInfo(file.FullName);

                while (isReadOnly == file.IsReadOnly)
                {
                    file = new FileInfo(file.FullName);

                    Thread.Sleep(1000);
                }

                action();
            });

            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;

            thread.Start();
        }

        public static void OnChanged(this FileInfo file, Action action)
        {
            var watcher = new FileSystemWatcher(file.Directory.FullName);

            watcher.Changed += (sender, e) =>
            {
                if (e.FullPath.AsCaseless() == file.FullName)
                {
                    action();
                }
            };

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        public static void OnDeleted(this FileInfo file, Action action)
        {
            var watcher = new FileSystemWatcher(file.FullName);

            watcher.Deleted += (sender, e) =>
            {
                if (e.FullPath.AsCaseless() == file.FullName)
                {
                    action();
                }
            };

            watcher.EnableRaisingEvents = true;
        }

        public static string GetHash(this string str)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = str.ToStream())
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }

        public static byte[] GetHashData(this string str)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = str.ToStream())
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public static string GetHashDataAsHexString(this string str)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = str.ToStream())
                {
                    return md5.ComputeHash(stream).GetHexString(noSpaces: true, noEOFMarker: true);
                }
            }
        }

        public static List<string> GetComparisonExceptions(this DirectoryInfo directory1, DirectoryInfo directory2)
        {
            var files1 = directory1.GetFiles().Select(f => f.FullName.RemoveStart(directory1.FullName)).ToList();
            var files2 = directory2.GetFiles().Select(f => f.FullName.RemoveStart(directory2.FullName)).ToList();

            return files1.Except(files2).ToList();
        }

        public static string GetHash(this DirectoryInfo directory, bool hashFileContents = true)
        {
            try
            {
                var path = directory.FullName;
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).OrderBy(p => p).ToList();
                MD5 md5 = MD5.Create();

                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string file = files[i];

                        // hash path
                        string relativePath = file.Substring(path.Length + 1);
                        byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());

                        if (hashFileContents)
                        {
                            md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);
                            // hash contents
                            byte[] contentBytes = File.ReadAllBytes(file);

                            if (i == files.Count - 1)
                            {
                                md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                            }
                            else
                            {
                                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                            }
                        }
                        else
                        {
                            if (i == files.Count - 1)
                            {
                                md5.TransformFinalBlock(pathBytes, 0, pathBytes.Length);
                            }
                            else
                            {
                                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);
                            }
                        }
                    }

                    return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetHash(this FileInfo file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file.FullName))
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }
#endif
#endregion

#if !SILVERLIGHT
        public static T GetSmallIcon<T>(this FileInfo file) 
        {
            var shInfo = new SHFILEINFO();
            var hImgSmall = Win32.SHGetFileInfo(file.FullName, 0, ref shInfo, (uint)Marshal.SizeOf(shInfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            var icon = Icon.FromHandle(shInfo.hIcon);
            object returnVal = null;

            switch (typeof(T).FullName)
            {
                case "System.Drawing.Icon":

                    returnVal = icon;
                    break;

                case "System.Drawing.Bitmap":
                    
                    var bitmap = icon.ToBitmap();
                    returnVal = bitmap;
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return (T)returnVal;
        }

        public static T GetLargeIcon<T>(this FileInfo file)
        {
            var shInfo = new SHFILEINFO();
            var hImgSmall = Win32.SHGetFileInfo(file.FullName, 0, ref shInfo, (uint)Marshal.SizeOf(shInfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            var icon = Icon.FromHandle(shInfo.hIcon);
            object returnVal = null;

            switch (typeof(T).FullName)
            {
                case "System.Drawing.Icon":

                    returnVal = icon;
                    break;

                case "System.Drawing.Bitmap":

                    var bitmap = icon.ToBitmap();
                    returnVal = bitmap;
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return (T)returnVal;
        }


        public static void Replace(string originalFile, string outputFile, string searchTerm, string replaceTerm)
        {
            byte b;
            var searchBytes = Encoding.UTF8.GetBytes(searchTerm.ToUpper());
            var searchBytesLower = Encoding.UTF8.GetBytes(searchTerm.ToLower());
            var bytesToAdd = new byte[searchBytes.Length];
            var searchBytesLength = searchBytes.Length;
            var searchByte0 = searchBytes[0];
            var searchByte0Lower = searchBytesLower[0];
            var replaceBytes = Encoding.UTF8.GetBytes(replaceTerm);
            var counter = 0;

            using (FileStream inputStream = File.OpenRead(originalFile))
            {
                //input length
                long srcLength = inputStream.Length;
                using (BinaryReader inputReader = new BinaryReader(inputStream))
                {
                    using (FileStream outputStream = File.OpenWrite(outputFile))
                    {
                        using (BinaryWriter outputWriter = new BinaryWriter(outputStream))
                        {
                            for (int nSrc = 0; nSrc < srcLength; ++nSrc)
                                //first byte
                                if ((b = inputReader.ReadByte()) == searchByte0
                                    || b == searchByte0Lower)
                                {
                                    bytesToAdd[0] = b;
                                    int nSearch = 1;
                                    //next bytes
                                    for (; nSearch < searchBytesLength; ++nSearch)
                                        //get byte, save it and test
                                        if ((b = bytesToAdd[nSearch] = inputReader.ReadByte()) != searchBytes[nSearch]
                                            && b != searchBytesLower[nSearch])
                                        {
                                            break;//fail
                                        }
                                    //Avoid overflow. No need, in my case, because no chance to see searchTerm at the end.
                                    //else if (nSrc + nSearch >= srcLength)
                                    //    break;

                                    if (nSearch == searchBytesLength)
                                    {
                                        //success
                                        ++counter;
                                        outputWriter.Write(replaceBytes);
                                        nSrc += nSearch - 1;
                                    }
                                    else
                                    {
                                        //failed, add saved bytes
                                        outputWriter.Write(bytesToAdd, 0, nSearch + 1);
                                        nSrc += nSearch;
                                    }
                                }
                                else
                                    outputWriter.Write(b);
                        }
                    }
                }
            }
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern Int32 StrFormatByteSize(
             long fileSize,
             [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
             int bufferSize);

        // Return a file size created by the StrFormatByteSize API function.
        public static string ToFileSizeApi(this long file_size)
        {
            StringBuilder sb = new StringBuilder(20);
            StrFormatByteSize(file_size, sb, 20);
            return sb.ToString();
        }

        public static string ToFileSize(this float value)
        {
            return ((double)value).ToFileSize();
        }

        // Return a string describing the value as a file size.
        // For example, 1.23 MB.
        public static string ToFileSize(this double value)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            for (int i = 0; i < suffixes.Length; i++)
            {
                if (value <= (Math.Pow(1024, i + 1)))
                {
                    return ThreeNonZeroDigits(value / Math.Pow(1024, i)) + " " + suffixes[i];
                }
            }

            return ThreeNonZeroDigits(value / Math.Pow(1024, suffixes.Length - 1)) + " " + suffixes[suffixes.Length - 1];
        }

        public static string ToByteSize(this long size)
        {
            if (size > NumberExtensions.GB)
            {
                return ((int)(size / NumberExtensions.GB)).ToString() + " GB";
            }
            else if (size > NumberExtensions.MB)
            {
                return ((int)(size / NumberExtensions.MB)).ToString() + " MB";
            }
            else if (size > NumberExtensions.KB)
            {
                return ((int)(size / NumberExtensions.KB)).ToString() + " KB";
            }
            else
            {
                return size.ToString() + " B";
            }
        }

        // Return the value formatted to include at most three
        // non-zero digits and at most two digits after the
        // decimal point. Examples:
        //         1
        //       123
        //        12.3
        //         1.23
        //         0.12
        private static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100)
            {
                // No digits after the decimal.
                return value.ToString("0,0");
            }
            else if (value >= 10)
            {
                // One digit after the decimal.
                return value.ToString("0.0");
            }
            else
            {
                // Two digits after the decimal.
                return value.ToString("0.00");
            }
        }

        public static void WriteLineFormat(this TextWriter writer, string format, params object[] args)
        {
            writer.Write(format + "\r\n", args);
        }

        public static void WriteLineFormatSpaceIndent(this TextWriter writer, int count, string format, params object[] args)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(format + "\r\n", args);
        }

        public static void WriteLineFormatTabIndent(this TextWriter writer, int count, string format, params object[] args)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(format + "\r\n", args);
        }

        public static TagHandler WriteTag(this TextWriter writer, string tag, object attributesOrValue = null)
        {
            Stack<TagHandler> stack = null;
            TagHandler handler;

            if (!tagHandlerStack.ContainsKey(writer))
            {
                stack = new Stack<TagHandler>();
                tagHandlerStack.Add(writer, stack);
            }
            else
            {
                stack = tagHandlerStack[writer];
            }

            handler = WriteTag(writer, stack.Count, tag, attributesOrValue);

            stack.Push(handler);

            handler.Disposed += (sender, e) =>
            {
                var handlerPeek = stack.Peek();

                Debug.Assert(handlerPeek.Tag == handler.Tag);

                stack.Pop();

                if (stack.Count == 0)
                {
                    tagHandlerStack.Remove(writer);
                }
            };

            return handler;
        }

        public static TagHandler WriteTag(this TextWriter writer, string tag, string value, object attributes = null)
        {
            Stack<TagHandler> stack = null;
            TagHandler handler;

            if (!tagHandlerStack.ContainsKey(writer))
            {
                stack = new Stack<TagHandler>();
                tagHandlerStack.Add(writer, stack);
            }
            else
            {
                stack = tagHandlerStack[writer];
            }

            handler = WriteTag(writer, stack.Count, tag, value, attributes);

            stack.Push(handler);

            handler.Disposed += (sender, e) =>
            {
                var handlerPeek = stack.Peek();

                Debug.Assert(handlerPeek.Tag == handler.Tag);

                stack.Pop();

                if (stack.Count == 0)
                {
                    tagHandlerStack.Remove(writer);
                }
            };

            return handler;
        }

        private static TagHandler WriteTag(this TextWriter writer, int indent, string tag, string value, object attributes = null)
        {
            if (attributes != null)
            {
                var type = attributes.GetType();

                writer.WriteFormatTabIndent(indent, "<{0}", tag);

                foreach (var property in type.GetProperties())
                {
                    var propertyName = property.Name.Replace("_", "-");
                    var propertyValue = property.GetValue(attributes, null);

                    writer.Write(" {0}=\"{1}\"", propertyName, propertyValue.ToString());
                }

                writer.WriteLine(">{0}", value);
            }
            else
            {
                writer.WriteLineFormatTabIndent(indent, "<{0}>{1}", tag, value);
            }

            return new TagHandler(writer, indent, tag);
        }

        private static TagHandler WriteTag(this TextWriter writer, int indent, string tag, object attributesOrValue = null)
        {
            if (attributesOrValue != null)
            {
                var type = attributesOrValue.GetType();

                if (type.Is<string>() || type.Is<DateTime>() || type.IsEnum || type.IsPrimitive)
                {
                    writer.WriteFormatTabIndent(indent, "<{0}>{1}", tag, attributesOrValue.ToString());
                    return new TagHandler(writer, 0, tag);
                }
                else
                {
                    writer.WriteFormatTabIndent(indent, "<{0}", tag);

                    foreach (var property in type.GetProperties())
                    {
                        var propertyName = property.Name.Replace("_", "-");
                        var value = property.GetValue(attributesOrValue, null);

                        writer.Write(" {0}=\"{1}\"", propertyName, value.ToString());
                    }

                    writer.WriteLine(">");
                }
            }
            else
            {
                writer.WriteLineFormatTabIndent(indent, "<{0}>", tag);
            }

            return new TagHandler(writer, indent, tag);
        }

        public static void WriteSpaceIndent(this TextWriter writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatSpaceIndent(this TextWriter writer, int count, string format, params object[] args)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteTabIndent(this TextWriter writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatTabIndent(this TextWriter writer, int count, string format, params object[] args)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteLineSpaceIndent(this TextWriter writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.WriteLine(text);
        }

        public static void WriteLineTabIndent(this TextWriter writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.WriteLine(text);
        }

        public static void WriteLinesTabIndent(this TextWriter writer, int count, string[] text)
        {
            foreach (var line in text)
            {
                writer.WriteLineTabIndent(count, line);
            }
        }

        public static StreamReset MarkForReset(this BinaryReader reader)
        {
            return new StreamReset(reader.BaseStream);
        }

        public static StreamReset MarkForReset(this BinaryWriter reader)
        {
            return new StreamReset(reader.BaseStream);
        }

        public static StreamReset MarkForReset(this StreamReader reader)
        {
            return new StreamReset(reader.BaseStream);
        }

        public static StreamReset MarkForReset(this Stream stream)
        {
            return new StreamReset(stream);
        }

        public static string GetHexString(this Array array, int maxLength = 255, bool rewind = false, bool noEOFMarker = false, bool noSpaces = false)
        {
            var targetLength = (int) Math.Min((int) array.Length, maxLength);
            byte[] bytes = new byte[targetLength];
            Stream stream;

            Array.Copy(array, bytes, targetLength);

            stream = bytes.ToMemory();

            return stream.GetHexString(targetLength, rewind, noEOFMarker, noSpaces);
        }

        public static string GetHexString(this Stream stream, int maxLength = 255, bool rewind = false, bool noEOFMarker = false, bool noSpaces = false)
        {
            var reader = new BinaryReader(stream);

            return reader.GetHexString(maxLength, rewind, noEOFMarker, noSpaces);
        }

        public static string GetHexString(this BinaryReader reader, int maxLength = 255, bool rewind = false, bool noEOFMarker = false, bool noSpaces = false)
        {
            var builder = new StringBuilder();
            var stream = reader.BaseStream;
            long pos = stream.Position;
            long end = Math.Min(stream.Length, maxLength == -1 ? int.MaxValue : maxLength);

            using (var reset = reader.MarkForReset())
            {
                if (rewind)
                {
                    reader.Rewind();
                }

                pos = stream.Position;
                end = Math.Min(stream.Length, maxLength == -1 ? int.MaxValue : maxLength);

                while (end > pos)
                {
                    var b = reader.ReadByte();

                    if (noSpaces)
                    {
                        builder.AppendFormat("{0}", b.ToHexString());
                    }
                    else
                    {
                        builder.AppendFormat("{0} ", b.ToHexString());
                    }

                    pos++;
                }

                if (stream.Position == stream.Length && !noEOFMarker)
                {
                    builder.Append("EOF");
                }
            }

            return builder.ToString();
        }

        public static string GetDataString(this Array array, int length = 255, bool rewind = false)
        {
            var targetLength = (int)Math.Min((int)array.Length, length);
            byte[] bytes = new byte[targetLength];
            Stream stream;

            Array.Copy(array, bytes, targetLength);

            stream = bytes.ToMemory();

            return stream.GetDataString(targetLength, rewind);
        }

        public static string GetDataString(this Stream stream, int length = 255, bool rewind = false)
        {
            var reader = new BinaryReader(stream);

            return reader.GetDataString(length, rewind);
        }

        public static string GetDataString(this BinaryReader reader, int length = 255, bool rewind = false)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
            var builder = new StringBuilder();
            var stream = reader.BaseStream;
            long pos = stream.Position;
            long end = Math.Min(stream.Length, length == -1 ? int.MaxValue : length);

            using (var reset = reader.MarkForReset())
            {
                if (rewind)
                {
                    reader.Rewind();
                }

                pos = stream.Position;
                end = Math.Min(stream.Length, length == -1 ? int.MaxValue : length);

                while (end > pos)
                {
                    var c = reader.ReadChar();

                    builder.Append(c);

                    pos++;
                }
            }

            return builder.ToString();
        }

        public static void SeekToLine(this StreamReader reader, int lineNumber)
        {
            string line;

            for (var x = 1; x < lineNumber; x++)
            {
                line = reader.ReadLine();
            }
        }

        public static void Seek(this ProcessBinaryReader reader, long offset, ProcessSeekOrigin origin = ProcessSeekOrigin.Begin)
        {
            ((ProcessStream)reader.BaseStream).Seek(offset, origin);
        }

        public static void Seek(this ProcessBinaryWriter writer, long offset, ProcessSeekOrigin origin = ProcessSeekOrigin.Begin)
        {
            ((ProcessStream)writer.BaseStream).Seek(offset, origin);
        }

        public static long Seek(this StreamReader reader, long offset)
        {
            return reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public static long Seek(this BinaryWriter writer, long offset)
        {
            return writer.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public static long Seek(this StreamReader reader, long offset, SeekOrigin origin)
        {
            return reader.BaseStream.Seek(offset, origin);
        }

        public static string Read(this StreamReader reader, int count)
        {
            var buffer = new char[count];

            reader.Read(buffer, (int) 0, count);

            return new string(buffer);
        }

        public static void Seek(this BinaryReader reader, long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            reader.BaseStream.Seek(offset, origin);
        }

        public static void Advance(this BinaryReader reader, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Current);
        }

        public static void Rewind(this BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public static void Rewind(this StreamReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            reader.DiscardBufferedData();
        }

        public static void Rewind(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        public static void Seek(this BinaryReader reader, ulong offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            reader.BaseStream.Seek((long) offset, origin);
        }

        public static void Advance(this BinaryReader reader, ulong offset)
        {
            reader.BaseStream.Seek((long) offset, SeekOrigin.Current);
        }

        public static byte[] ToArray(this GZipStream inputStream)
        {
            var output = new byte[0];
            var bytes = new byte[100];
            var offset = 0;
            var totalCount = 0;

            while (true)
            {
                var bytesRead = inputStream.Read(bytes, offset, 100);

                if (bytesRead == 0)
                {
                    break;
                }

                offset += bytesRead;
                totalCount += bytesRead;

                output = output.Append(bytes);
            }

            return output;
        }

        public static void ToZipFile(this DirectoryInfo directory, string newZipFileName, Func<FileInfo, bool> filter = null, bool skipEmpty = true)
        {
            FileStream outputStream;

            if (skipEmpty)
            {
                var count = directory.GetFiles("*.*", SearchOption.AllDirectories).Where(f =>
                {
                    if (filter != null && !filter(f))
                    {
                        return false;
                    }

                    if (f.FullName.AsCaseless() == newZipFileName)
                    {
                        return false;
                    }

                    return true;

                }).Count();

                if (count == 0)
                {
                    return;
                }
            }

            outputStream = File.OpenWrite(newZipFileName);

            using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create))
            {
                foreach (var file in directory.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (filter != null && !filter(file))
                    {
                        continue;
                    }

                    if (file.FullName.AsCaseless() != newZipFileName)
                    {
                        var path = file.FullName.RemoveStart(directory.FullName).RemoveStartIfMatches(@"\");
                        var entry = zipArchive.CreateEntry(path);

                        using (var zipStream = entry.Open())
                        {
                            var bytes = File.ReadAllBytes(file.FullName);

                            zipStream.Write(bytes, 0, bytes.Length);
                            zipStream.Flush();
                        }
                    }
                }
            }
        }

        public static void Unzip(this FileInfo zipFile, DirectoryInfo directory)
        {
            var inputStream = File.OpenRead(zipFile.FullName);

            using (var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    var name = entry.FullName;
                    var deflateStream = entry.Open();
                    var memoryStream = new MemoryStream();
                    var path = Path.Combine(directory.FullName, name);
                    var newFile = new FileInfo(path);
                    var newDirectory = newFile.Directory;

                    if (!newDirectory.Exists)
                    {
                        newDirectory.Create();
                    }

                    deflateStream.CopyTo(memoryStream);

                    File.WriteAllBytes(path, memoryStream.ToArray());
                }
            }
        }

        public static void ToZipFile(this XDocument document, string fileName)
        {
            GZipStream zipStream = null;
            var outputStream = File.OpenWrite(fileName);

            using (zipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                document.Save(zipStream);
            }
        }

        public static XDocument ZipFileToXDocument(string fileName)
        {
            var inputStream = File.OpenRead(fileName);

            using (var zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                return XDocument.Load(zipStream);
            }
        }

        public static StreamReader ZipFileToReader(string fileName)
        {
            var inputStream = File.OpenRead(fileName);

            var zipStream = new GZipStream(inputStream, CompressionMode.Decompress);

            return new StreamReader(zipStream);
        }

        public static byte[] ToZip(this string text)
        {
            GZipStream zipStream = null;
            var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(text);
            byte[] output;

            using (var outputStream = new MemoryStream())
            {
                using (zipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                    zipStream.Flush();
                }

                output = outputStream.ToArray();
            }

            return output;
        }

        public static byte[] ToZip(this byte[] bytes)
        {
            GZipStream zipStream = null;
            byte[] output;

            using (var outputStream = new MemoryStream())
            {
                using (zipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                    zipStream.Flush();
                }

                output = outputStream.ToArray();
            }

            return output;
        }

        public static string FromZip(this byte[] bytes)
        {
            GZipStream zipStream;
            string output;
            var stream = bytes.ToMemory();

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            zipStream = new GZipStream(stream, CompressionMode.Decompress);

            using (var outputStream = new MemoryStream())
            {
                zipStream.CopyTo(outputStream);

                output = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            return output;
        }

        public static Bitmap ToBitmap(this Stream inputream)
        {
            var bitmap = new Bitmap(inputream);

            return bitmap;
        }

        public static Icon ToIcon(this Stream inputream)
        {
            var icon = new Icon(inputream);

            return icon;
        }
#endif
        public static string GetCommonRoot(params string[] paths)
        {
            var minDepth = 999;
            var parsedPaths = new List<string[]>();

            foreach (var path in paths)
            {
                var parts = path.Split('\\');

                parsedPaths.Add(parts);
                minDepth = parts.Length < minDepth ? parts.Length : minDepth;
            }

            var builder = new StringBuilder();

            for (var x = 0; x < minDepth; x++)
            {
                var list = new List<string>();

                foreach (var dir in parsedPaths)
                {
                    list.Add(dir[x].ToLower());
                }

                if (!AllEqual(list.ToArray()))
                {
                    break;
                }

                builder.AppendFormat("{0}\\", list[0]);
            }

            return builder.ToString();
        }

        private static bool AllEqual(params string[] strings)
        {
            var returnValue = true;

            for (int i = 1; i < strings.Length; i++)
            {
                returnValue &= strings[0].AsCaseless() == strings[i];
            }

            return returnValue;
        }

        public static void MakeWritable(this FileInfo file)
        {
            file.Attributes &= (~FileAttributes.ReadOnly);
        }

        public static void Hide(this FileSystemInfo info)
        {
            if (!info.Attributes.HasFlag(FileAttributes.Hidden))
            {
                info.Attributes |= FileAttributes.Hidden;
            } 
        }

        public static DirectoryInfo Append(this DirectoryInfo directory, string path)
        {
            return new DirectoryInfo(Path.Combine(directory.FullName, path));
        }

        public static void WriteText(this MemoryStream stream, string text)
        {
            var bytes = text.ToBytes();

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void ExtendTo(this MemoryStream stream, int width)
        {
            if (stream.Length < width)
            {
                using (var reset = stream.MarkForReset())
                {
                    var delta = (int)(width - stream.Length);

                    stream.Seek(0, SeekOrigin.End);

                    stream.WriteText("\0".Repeat(delta));
                }
            }
        }

        public static MemoryStream ToMemory(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes.Length);

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static MemoryStream ToMemory(this byte[] bytes, int length)
        {
            var stream = new MemoryStream(length);

            stream.Write(bytes, 0, length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static MemoryStream Trim(this Stream inputStream)
        {
            var outputStream = new MemoryStream();
            var bytes = inputStream.ReadUntil(new byte[] { (byte)'\0' }, true, (int)inputStream.Length);

            return bytes.ToMemory();
        }

        public static MemoryStream ToMemory(this Stream inputStream)
        {
            var outputStream = new MemoryStream();

            CopyTo(inputStream, outputStream);

            return outputStream;
        }

        public static long GetActualPosition(this StreamReader reader)
        {
            System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField;

            // The current buffer of decoded characters
            char[] charBuffer = (char[])reader.GetType().InvokeMember("charBuffer", flags, null, reader, null);

            // The index of the next char to be read from charBuffer
            int charPos = (int)reader.GetType().InvokeMember("charPos", flags, null, reader, null);

            // The number of decoded chars presently used in charBuffer
            int charLen = (int)reader.GetType().InvokeMember("charLen", flags, null, reader, null);

            // The current buffer of read bytes (byteBuffer.Length = 1024; this is critical).
            byte[] byteBuffer = (byte[])reader.GetType().InvokeMember("byteBuffer", flags, null, reader, null);

            // The number of bytes read while advancing reader.BaseStream.Position to (re)fill charBuffer
            int byteLen = (int)reader.GetType().InvokeMember("byteLen", flags, null, reader, null);

            // The number of bytes the remaining chars use in the original encoding.
            int numBytesLeft = reader.CurrentEncoding.GetByteCount(charBuffer, charPos, charLen - charPos);

            // For variable-byte encodings, deal with partial chars at the end of the buffer
            int numFragments = 0;

            if (byteLen > 0 && !reader.CurrentEncoding.IsSingleByte)
            {
                if (reader.CurrentEncoding.CodePage == 65001) // UTF-8
                {
                    byte byteCountMask = 0;

                    while ((byteBuffer[byteLen - numFragments - 1] >> 6) == 2) // if the byte is "10xx xxxx", it's a continuation-byte
                    {
                        byteCountMask |= (byte)(1 << ++numFragments); // count bytes & build the "complete char" mask
                    }

                    if ((byteBuffer[byteLen - numFragments - 1] >> 6) == 3) // if the byte is "11xx xxxx", it starts a multi-byte char.
                    {
                        byteCountMask |= (byte)(1 << ++numFragments); // count bytes & build the "complete char" mask
                    }

                    // see if we found as many bytes as the leading-byte says to expect
                    if (numFragments > 1 && ((byteBuffer[byteLen - numFragments] >> 7 - numFragments) == byteCountMask))
                    {
                        numFragments = 0; // no partial-char in the byte-buffer to account for
                    }
                }
                else if (reader.CurrentEncoding.CodePage == 1200) // UTF-16LE
                {
                    if (byteBuffer[byteLen - 1] >= 0xd8) // high-surrogate
                    {
                        numFragments = 2; // account for the partial character
                    }
                }
                else if (reader.CurrentEncoding.CodePage == 1201) // UTF-16BE
                {
                    if (byteBuffer[byteLen - 2] >= 0xd8) // high-surrogate
                    {
                        numFragments = 2; // account for the partial character
                    }
                }
            }
        
            return reader.BaseStream.Position - numBytesLeft - numFragments;
        }

        public static MemoryStream ToWriteableMemory(this Stream inputStream)
        {
            var outputStream = new MemoryStream();
            var buffer = new byte[inputStream.Length];
            int read;

            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, read);
            }

            return outputStream;
        }

        public static byte[] ToArray(this Stream inputStream)
        {
            if (inputStream.CanSeek)
            {
                var bytes = new byte[inputStream.Length];

                inputStream.Read(bytes, 0, bytes.Length);

                return bytes;
            }
            else
            {
                var thisRead = 0;
                var read = 0;
                var innerStream = new MemoryStream();
                var position = innerStream.Position;
                var bufferSize = 64 * 1024;
                var buffer = new byte[bufferSize];

                do
                {
                    thisRead = inputStream.Read(buffer, 0, bufferSize - read);
                    innerStream.Write(buffer, 0, thisRead);
                    read += thisRead;
                }
                while (inputStream.CanRead && thisRead > 0);

                innerStream.Rewind();

                return innerStream.ToArray();
            }
        }

#if !SILVERLIGHT
        public static char[] ToCharArray(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes).ToCharArray();
        }
#endif

        public static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static byte[] ToArray(this string text)
        {
            var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(text);

            return bytes;
        }

        public static byte[] ToUnicodeArray(this string text)
        {
            var bytes = System.Text.UnicodeEncoding.Unicode.GetBytes(text);

            return bytes;
        }

        public static MemoryStream ToStream(this string text)
        {
            var stream = new MemoryStream();
            var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(text);

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static BinaryReader ToReader(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);

            return reader;
        }

        public static void CopyTo(this Stream inputream, Stream outputStream)
        {
            var bytes = new byte[inputream.Length];

            inputream.Read(bytes, 0, bytes.Length);
            outputStream.Write(bytes, 0, (int) inputream.Length);

            outputStream.Rewind();
        }

        public static string ToText(this Stream inputream)
        {
            var bytes = new byte[inputream.Length];

            inputream.Read(bytes, 0, bytes.Length);
            inputream.Rewind();

            return bytes.ToText();
        }

        public static XDocument ToXml(this Stream inputream)
        {
            return XDocument.Load(inputream);
        }

        private static IEnumerable<string> GetBestMatchPaths(this Assembly assembly, string path)
        {
            var names = assembly.GetManifestResourceNames();

            return null;
        }

        public static T ReadResource<T>(this Type type, string path, bool throwError = true)
        {
            object result = null;
            Stream stream;
            var newPath = path;
            
            if (newPath.Contains('\\'))
            {
                string extension;
                string directory;
                var slashedNamespace = type.Namespace.Replace('.', '\\');

                if (!newPath.StartsWith(slashedNamespace))
                {
                    newPath = Path.GetFullPath(slashedNamespace + @"\" + newPath);
                    newPath = newPath.RemoveStart(Directory.GetCurrentDirectory() + 1);
                }

                extension = Path.GetExtension(newPath);
                directory = Path.GetDirectoryName(newPath);
                
                newPath = Path.Combine(directory, Path.GetFileNameWithoutExtension(newPath)).Replace('\\', '.').Append(extension);
            }
            else if (newPath.Contains('/'))
            {
                Debugger.Break();

                var extension = Path.GetExtension(newPath);
                var directory = Path.GetDirectoryName(newPath);

                newPath = Path.Combine(directory, Path.GetFileNameWithoutExtension(newPath)).Replace('/', '.').Append(extension);
            }

            stream = type.Assembly.GetManifestResourceStream(newPath);

            if (stream == null)
            {
                stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + newPath);
            }

            if (stream == null)
            {
                if (throwError)
                {
                    e.Throw<ArgumentException>("Stream null by ReadResource");
                }
                else
                {
                    return default(T);
                }
            }

            using (stream)
            {
                switch (typeof(T).Name)
                {
                    case "Byte[]":
                        result = stream.ToArray();
                        break;
                    case "String":
                        result = stream.ToText();
                        break;
                    case "XDocument":
                        result = stream.ToXml();
                        break;
#if !SILVERLIGHT
                    case "Icon":
                        result = stream.ToIcon();
                        break;
                    case "Bitmap":
                        result = stream.ToBitmap();
                        break;
                    case "Stream":

                        var unmanagedStream = (UnmanagedMemoryStream)stream;
                        var bytes = unmanagedStream.ToArray();

                        result = bytes.ToMemory();
                        break;
#endif
                }
            }

            return (T)result;
        }

        public static T ReadResource<T>(this Assembly assembly, string path)
        {
            object result = null;
            var stream = assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                e.Throw<ArgumentException>("Stream null by ReadResource");
            }

            using (stream)
            {
                switch (typeof(T).Name)
                {
                    case "Byte[]":
                        result = stream.ToArray();
                        break;
                    case "String":
                        result = stream.ToText();
                        break;
                    case "XDocument":
                        result = stream.ToXml();
                        break;
                    case "Stream":

                        var unmanagedStream = (UnmanagedMemoryStream)stream;
                        var bytes = unmanagedStream.ToArray();

                        result = bytes.ToMemory();
                        break;
                }
            }

            return (T)result;
        }

#if !SILVERLIGHT

        public static bool HasChangedFilesSince(this DirectoryInfo directory, DateTime date, string pattern)
        {
            foreach (var file in directory.GetFiles(pattern))
            {
                if (file.LastWriteTime > date)
                {
                    return true;
                }
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                if (subDirectory.HasChangedFilesSince(date, pattern))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToBase64(this Stream inputStream)
        {
            var bytes = inputStream.ToArray();

            return Convert.ToBase64String(bytes);
        }

        public static byte[] Read(this Stream stream, int length)
        {
            byte[] data = new byte[length];

            stream.Read(data, 0, length);

            return data;
        }

        public static TExpandTo ExpandTo<TExpandTo>(this Stream stream, object obj) where TExpandTo : new()
        {
            var toLength = TypeExtensions.SizeOf<TExpandTo>();
            var fromLength = obj.SizeOf();
            var difference = toLength - fromLength;
            var buffer = obj.ToByteArray().ExpandRight(difference);

            stream.Read(buffer, fromLength, difference);

            return ReadType<TExpandTo>(buffer);
        }

        public static byte[] ToByteArray(this object obj)
        {
            var size = obj.SizeOf();
            var ptr = Marshal.AllocCoTaskMem(size);
            var buffer = new byte[size];

            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, buffer, 0, size);

            return buffer;
        }

        public static byte[] ToByteArray(params object[] objects)
        {
            byte[] combinedBuffer = new byte[0];

            foreach (var obj in objects)
            {
                if (obj is string)
                {
                    var buffer = ((string)obj).ToArray();

                    combinedBuffer = combinedBuffer.Append(buffer);
                }
                else if (obj is byte[])
                {
                    combinedBuffer = combinedBuffer.Append((byte[])obj);
                }
                else
                {
                    var buffer = obj.ToByteArray();

                    combinedBuffer = combinedBuffer.Append(buffer);
                }
            }

            return combinedBuffer;
        }

        public static T ReadType<T>(this byte[] buffer) where T : new()
        {
            var result = new T();
            var size = buffer.Length;
            var ptr = Marshal.AllocCoTaskMem(size);

            Marshal.Copy(buffer, 0, ptr, size);
            Marshal.PtrToStructure(ptr, result);

            return result;
        }

        public static T ReadType<T>(this Stream stream) where T : new()
        {
            var size = TypeExtensions.SizeOf<T>();
            var buffer = new byte[size];

            stream.Read(buffer, 0, size);

            return buffer.ReadType<T>();
        }

        public static void Write(this Stream stream, object obj)
        {
            var data = obj.ToByteArray();

            stream.Write(data);
        }

        public static void Write(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public static void Write(this Stream stream, string text)
        {
            var data = ASCIIEncoding.ASCII.GetBytes(text);

            stream.Write(data);
        }

        public static void Write(this Stream stream, params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj is string)
                {
                    stream.Write((string)obj);
                }
                else if (obj is byte[])
                {
                    stream.Write((byte[])obj);
                }
                else
                {
                    stream.Write(obj);
                }
            }
        }

        public static byte[] ReadUntil(this Stream stream, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return stream.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static byte[] ReadUntil(this Stream stream, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new byte[0];
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new byte[size];

                stream.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data;
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data = data.TrimRight(terminator.Length);
                        }

                        return data;
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }

        public static byte[] ReadUntil(this BinaryReader reader, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return reader.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static byte[] ReadUntil(this BinaryReader reader, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new byte[0];
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new byte[size];

                reader.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data;
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data = data.TrimRight(terminator.Length);
                        }

                        return data;
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }

        public static string ReadUntil(this TextReader reader, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return reader.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static string ReadUntil(this TextReader reader, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new StringBuilder();
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new char[size];

                reader.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data.ToString();
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data.RemoveEnd(terminator.Length);
                        }

                        return data.ToString();
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }

        public static string Expand(this string pathLocation)
        {
            return Environment.ExpandEnvironmentVariables(pathLocation);
        }

        public static void DeleteAndCreate(this DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                directory.ForceDelete();
                directory.Refresh();
            }

            Win32.CreateDirectory(directory.FullName, IntPtr.Zero);
        }

        public static void CreateIfNotExists(this DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public static void CreateIfNotExists(params DirectoryInfo[] directories)
        {
            foreach (var directory in directories)
            {
                directory.CreateIfNotExists();
            }
        }

        public static void ForceDelete(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (file.IsReadOnly)
                {
                    file.MakeWritable();
                }

                file.Delete();
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                subDirectory.ForceDelete();
            }

            try
            {
                directory.Delete();
            }
            catch
            {
                directory = new DirectoryInfo(directory.FullName);

                foreach (var file in directory.GetFiles())
                {
                    if (file.IsReadOnly)
                    {
                        file.MakeWritable();
                    }

                    file.Delete();
                }

                foreach (var subDirectory in directory.GetDirectories())
                {
                    subDirectory.ForceDelete();
                }

                directory.Delete();
            }
        }

        public static void ForceDeleteFiles(this DirectoryInfo directory, bool throwError = false)
        {
            foreach (var file in directory.GetFiles())
            {
                try
                {
                    if (file.IsReadOnly)
                    {
                        file.MakeWritable();
                    }

                    file.Delete();
                }
                catch (Exception ex)
                {
                    if (throwError)
                    {
                        throw;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForceDeleteAllFilesAndSubFolders(this DirectoryInfo topDirectory, bool throwError = false, Func<FileSystemInfoStatus, bool> filter = null, Func<FileSystemInfoStatus, bool> onOpenFileError = null)
        {
            var count = 0;
            var originalCount = 0;
            var lastCountTime = DateTime.MinValue;
            var deleted = new List<FileSystemInfo>();
            var successful = false;
            var retry = 0;
            List<DirectoryInfo> directories;
            SearchOption searchOption;

            Func<FileSystemInfoStatus, SearchOption, bool> innerFilter = (fi, s) =>
            {
                var now = DateTime.Now;

                if (now - lastCountTime > TimeSpan.FromMilliseconds(500))
                {
                    lastCountTime = now;
                    count = topDirectory.GetDirectories("*", s).OrderByDescending(d => d.FullName.Length).Concat(new List<DirectoryInfo> { topDirectory }).Count();
                }

                fi.OriginalCount = originalCount;
                fi.Count = count;

                return filter(fi);
            };

            if (filter == null)
            {
                filter = new Func<FileSystemInfoStatus, bool>((f) => true);
            }

            if (!topDirectory.Exists)
            {
                return;
            }

            searchOption = SearchOption.TopDirectoryOnly;
            directories = topDirectory.GetDirectories("*", searchOption).OrderByDescending(d => d.FullName.Length).Concat(new List<DirectoryInfo> { topDirectory }).ToList();
            originalCount = directories.Count;

            foreach (var directory in directories)
            {
                if (topDirectory.FullName != directory.FullName)
                {
                    var directoryDelete = new DirectoryInfo(directory.FullName);
                    var fileSystemInfoStatus = new FileSystemInfoStatus(directoryDelete);

                    if (innerFilter(fileSystemInfoStatus, searchOption))
                    {
                        try
                        {
                            if (!fileSystemInfoStatus.Deleted)
                            {
                                directoryDelete.Delete(true);
                            }

                            deleted.Add(directoryDelete);
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            continue;
                        }
                        catch (IOException ex)
                        {
                            var regex = new Regex("The process cannot access the file '(?<path>.*?)' because it is being used by another process");

                            if (regex.IsMatch(ex.Message))
                            {
                                var match = regex.Match(ex.Message);
                                var path = match.GetGroupValue("path");

                                if (Directory.Exists(path))
                                {
                                    var directory2 = new DirectoryInfo(path);

                                    try
                                    {
                                        var processes = directory2.FindLockingProcesses();

                                        if (onOpenFileError != null)
                                        {
                                            fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, processes.ToList());

                                            if (!onOpenFileError(fileSystemInfoStatus))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        if (onOpenFileError != null)
                                        {
                                            fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, true);

                                            if (!onOpenFileError(fileSystemInfoStatus))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                }
                                else if (File.Exists(path))
                                {
                                    var file = new FileInfo(path);

                                    try
                                    {
                                        var processes = file.FindLockingProcesses();

                                        if (onOpenFileError != null)
                                        {
                                            fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                            if (!onOpenFileError(fileSystemInfoStatus))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        if (onOpenFileError != null)
                                        {
                                            fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, true);

                                            if (!onOpenFileError(fileSystemInfoStatus))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            var regex = new Regex("Access to the path '(?<path>.*?)' is denied");

                            if (regex.IsMatch(ex.Message))
                            {
                                var match = regex.Match(ex.Message);
                                var path = match.GetGroupValue("path");
                                var file = directoryDelete.GetFiles(path, SearchOption.AllDirectories).Single();
                                var process = Process.GetCurrentProcess();
                                var processes = file.FindLockingProcesses();

                                if (onOpenFileError != null)
                                {
                                    fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                    if (!onOpenFileError(fileSystemInfoStatus))
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Thread.Sleep(1);
                                directoryDelete = new DirectoryInfo(directoryDelete.FullName);

                                if (directoryDelete.Exists)
                                {
                                    directoryDelete.Delete(true);
                                    deleted.Add(directoryDelete);
                                }
                            }
                            catch (Exception ex2)
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            while (!successful)
            {
                try
                {
                    Thread.Sleep(100);

                    searchOption = SearchOption.AllDirectories;
                    directories = topDirectory.GetDirectories("*", searchOption).OrderByDescending(d => d.FullName.Length).Concat(new List<DirectoryInfo> { topDirectory }).ToList();
                    originalCount = directories.Count;

                    foreach (var directory in directories)
                    {
                        DirectoryInfo directoryDelete;
                        FileInfo fileDelete = null;

                        try
                        {
                            foreach (var file in directory.GetFiles())
                            {
                                fileDelete = file;

                                if (innerFilter(new FileSystemInfoStatus(file), searchOption))
                                {
                                    if (file.IsReadOnly)
                                    {
                                        file.MakeWritable();
                                    }

                                    try
                                    {
                                        file.Delete();
                                        deleted.Add(file);
                                    }
                                    catch (UnauthorizedAccessException ex)
                                    {
                                        var regex = new Regex("Access to the path '(?<path>.*?)' is denied");

                                        if (regex.IsMatch(ex.Message))
                                        {
                                            var match = regex.Match(ex.Message);
                                            var path = match.GetGroupValue("path");
                                            var process = Process.GetCurrentProcess();
                                            var processes = file.FindLockingProcesses();

                                            if (onOpenFileError != null)
                                            {
                                                var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                                if (!onOpenFileError(fileSystemInfoStatus))
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch (FileNotFoundException ex)
                                    {
                                        continue;
                                    }
                                    catch (DirectoryNotFoundException ex)
                                    {
                                        continue;
                                    }
                                    catch (IOException ex)
                                    {
                                        var regex = new Regex("The process cannot access the file '(?<path>.*?)' because it is being used by another process");

                                        if (regex.IsMatch(ex.Message))
                                        {
                                            var match = regex.Match(ex.Message);
                                            var path = match.GetGroupValue("path");
                                            var file2 = fileDelete;

                                            try
                                            {
                                                var processes = file.FindLockingProcesses();

                                                if (onOpenFileError != null)
                                                {
                                                    var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                                    if (!onOpenFileError(fileSystemInfoStatus))
                                                    {
                                                        return;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                if (onOpenFileError != null)
                                                {
                                                    var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, true);

                                                    if (!onOpenFileError(fileSystemInfoStatus))
                                                    {
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }

                            fileDelete = null;

                            if (topDirectory.FullName != directory.FullName)
                            {
                                directoryDelete = new DirectoryInfo(directory.FullName);

                                if (innerFilter(new FileSystemInfoStatus(directoryDelete), searchOption))
                                {
                                    var innerRetry = 0;

                                    while (directoryDelete.Exists)
                                    {
                                        try
                                        {
                                            directoryDelete.Delete();
                                            deleted.Add(directoryDelete);
                                        }
                                        catch (Exception ex)
                                        {
                                            innerRetry++;

                                            if (innerRetry >= 10)
                                            {
                                                throw;
                                            }

                                            try
                                            {
                                                Thread.Sleep(1);
                                                directoryDelete = new DirectoryInfo(directoryDelete.FullName);

                                                if (directoryDelete.Exists)
                                                {
                                                    directoryDelete.Delete(true);
                                                    deleted.Add(directoryDelete);
                                                }
                                            }
                                            catch (UnauthorizedAccessException ex2)
                                            {
                                                var regex = new Regex("Access to the path '(?<path>.*?)' is denied");

                                                if (regex.IsMatch(ex.Message))
                                                {
                                                    var match = regex.Match(ex.Message);
                                                    var path = match.GetGroupValue("path");
                                                    var file = directoryDelete.GetFiles(path, SearchOption.AllDirectories).Single();
                                                    var process = Process.GetCurrentProcess();
                                                    var processes = file.FindLockingProcesses();

                                                    if (onOpenFileError != null)
                                                    {
                                                        var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                                        if (!onOpenFileError(fileSystemInfoStatus))
                                                        {
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (DirectoryNotFoundException ex2)
                                            {
                                                break;
                                            }
                                            catch (IOException ex2)
                                            {
                                                var regex = new Regex("The process cannot access the file '(?<path>.*?)' because it is being used by another process");

                                                if (regex.IsMatch(ex.Message))
                                                {
                                                    var match = regex.Match(ex.Message);
                                                    var path = match.GetGroupValue("path");
                                                    var file = directoryDelete.GetFiles(Path.GetFileName(path), SearchOption.AllDirectories).SingleOrDefault();

                                                    if (Directory.Exists(path))
                                                    {
                                                        var directory2 = new DirectoryInfo(path);

                                                        try
                                                        {
                                                            var processes = directory2.FindLockingProcesses();

                                                            if (onOpenFileError != null)
                                                            {
                                                                var fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, processes.ToList());

                                                                if (!onOpenFileError(fileSystemInfoStatus))
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            if (onOpenFileError != null)
                                                            {
                                                                var fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, true);

                                                                if (!onOpenFileError(fileSystemInfoStatus))
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (File.Exists(path))
                                                    {
                                                        var file2 = new FileInfo(path);

                                                        try
                                                        {
                                                            var processes = file2.FindLockingProcesses();

                                                            if (onOpenFileError != null)
                                                            {
                                                                var fileSystemInfoStatus = new FileSystemInfoStatus(file2, ex, processes.ToList());

                                                                if (!onOpenFileError(fileSystemInfoStatus))
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            if (onOpenFileError != null)
                                                            {
                                                                var fileSystemInfoStatus = new FileSystemInfoStatus(file2, ex, true);

                                                                if (!onOpenFileError(fileSystemInfoStatus))
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    throw;
                                                }
                                            }
                                            catch (Exception ex2)
                                            {
                                                if (throwError)
                                                {
                                                    throw;
                                                }
                                            }
                                        }

                                        directoryDelete = new DirectoryInfo(directory.FullName);
                                    }
                                }
                            }
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            continue;
                        }
                    }

                    successful = true;
                }
                catch (Exception ex)
                {
                    retry++;

                    if (retry >= 10)
                    {
                        throw;
                    }

                    successful = false;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForceDelete(this DirectoryInfo directoryDelete, bool throwError = false, Func<FileSystemInfoStatus, bool> onOpenFileError = null)
        {
            if (!directoryDelete.Exists)
            {
                return;
            }

            try
            {
                directoryDelete.Delete(true);
            }
            catch (DirectoryNotFoundException ex)
            {
                return;
            }
            catch (IOException ex)
            {
                var regex = new Regex("The process cannot access the file '(?<path>.*?)' because it is being used by another process");

                if (regex.IsMatch(ex.Message))
                {
                    var match = regex.Match(ex.Message);
                    var path = match.GetGroupValue("path");

                    if (Directory.Exists(path))
                    {
                        var directory2 = new DirectoryInfo(path);

                        try
                        {
                            var processes = directory2.FindLockingProcesses();

                            if (onOpenFileError != null)
                            {
                                var fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, processes.ToList());

                                if (!onOpenFileError(fileSystemInfoStatus))
                                {
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            if (onOpenFileError != null)
                            {
                                var fileSystemInfoStatus = new FileSystemInfoStatus(directory2, ex, true);

                                if (!onOpenFileError(fileSystemInfoStatus))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    else if (File.Exists(path))
                    {
                        var file = new FileInfo(path);

                        try
                        {
                            var processes = file.FindLockingProcesses();

                            if (onOpenFileError != null)
                            {
                                var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                                if (!onOpenFileError(fileSystemInfoStatus))
                                {
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            if (onOpenFileError != null)
                            {
                                var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, true);

                                if (!onOpenFileError(fileSystemInfoStatus))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    throw;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                var regex = new Regex("Access to the path '(?<path>.*?)' is denied");

                if (regex.IsMatch(ex.Message))
                {
                    var match = regex.Match(ex.Message);
                    var path = match.GetGroupValue("path");
                    var file = directoryDelete.GetFiles(path, SearchOption.AllDirectories).Single();
                    var process = Process.GetCurrentProcess();
                    var processes = file.FindLockingProcesses();

                    if (onOpenFileError != null)
                    {
                        var fileSystemInfoStatus = new FileSystemInfoStatus(file, ex, processes.ToList());

                        if (!onOpenFileError(fileSystemInfoStatus))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Thread.Sleep(1);
                    directoryDelete = new DirectoryInfo(directoryDelete.FullName);

                    if (directoryDelete.Exists)
                    {
                        directoryDelete.Delete(true);
                    }
                }
                catch (Exception ex2)
                {
                    throw;
                }
            }
        }

        public static bool IsClrImage(string fileName)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] dat = new byte[300];
                fs.Read(dat, 0, 128);
                if ((dat[0] != 0x4d) || (dat[1] != 0x5a)) // "MZ" DOS header 
                    return false;

                int lfanew = BitConverter.ToInt32(dat, 0x3c);
                fs.Seek(lfanew, SeekOrigin.Begin);
                fs.Read(dat, 0, 24); // read signature & PE file header 
                if ((dat[0] != 0x50) || (dat[1] != 0x45)) // "PE" signature 
                    return false;

                fs.Read(dat, 0, 96 + 128); // read PE optional header 
                if ((dat[0] != 0x0b) || (dat[1] != 0x01)) // magic 
                    return false;

                int clihd = BitConverter.ToInt32(dat, 208); // get IMAGE_COR20_HEADER rva-address 
                return clihd != 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
#endif
    }
}
