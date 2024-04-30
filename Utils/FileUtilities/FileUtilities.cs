namespace Utils
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    internal static class FileUtilities
    {
        internal static string cacheDirectory = null;
        private static string executablePath;
        internal const string FileTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss'.'fffffff";
        internal const int MaxPath = 260;

    //    internal static string AttemptToShortenPath(string path)
    //    {
    //        if ((path.Length >= Microsoft.Build.Shared.NativeMethodsShared.MAX_PATH) || (!IsRootedNoThrow(path) && (((Environment.CurrentDirectory.Length + path.Length) + 1) >= Microsoft.Build.Shared.NativeMethodsShared.MAX_PATH)))
    //        {
    //            path = GetFullPathNoThrow(path);
    //        }
    //        return path;
    //    }

    //    internal static void ClearCacheDirectory()
    //    {
    //        if (Directory.Exists(GetCacheDirectory()))
    //        {
    //            Directory.Delete(GetCacheDirectory(), true);
    //        }
    //    }

        private static Uri CreateUriFromPath(string path)
        {
            Uri result = null;
            if (!Uri.TryCreate(path, UriKind.Absolute, out result))
            {
                result = new Uri(path, UriKind.Relative);
            }
            return result;
        }

    //    internal static void DeleteNoThrow(string path)
    //    {
    //        try
    //        {
    //            File.Delete(path);
    //        }
    //        catch (Exception exception)
    //        {
    //            if (Microsoft.Build.Shared.ExceptionHandling.NotExpectedException(exception))
    //            {
    //                throw;
    //            }
    //        }
    //    }

    //    internal static bool DirectoryExistsNoThrow(string fullPath)
    //    {
    //        fullPath = AttemptToShortenPath(fullPath);
    //        Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA lpFileInformation = new Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA();
    //        return (Microsoft.Build.Shared.NativeMethodsShared.GetFileAttributesEx(fullPath, 0, ref lpFileInformation) && ((lpFileInformation.fileAttributes & 0x10) != 0));
    //    }

        internal static bool EndsWithSlash(string fileSpec)
        {
            if (fileSpec.Length <= 0)
            {
                return false;
            }

            return IsSlash(fileSpec[fileSpec.Length - 1]);
        }

    //    internal static string EnsureNoLeadingSlash(string path)
    //    {
    //        if ((path.Length > 0) && IsSlash(path[0]))
    //        {
    //            path = path.Substring(1);
    //        }
    //        return path;
    //    }

    //    internal static string EnsureNoTrailingSlash(string path)
    //    {
    //        if (EndsWithSlash(path))
    //        {
    //            path = path.Substring(0, path.Length - 1);
    //        }
    //        return path;
    //    }

        internal static string EnsureTrailingSlash(string fileSpec)
        {
            if ((fileSpec.Length > 0) && !EndsWithSlash(fileSpec))
            {
                fileSpec = fileSpec + Path.DirectorySeparatorChar;
            }
            return fileSpec;
        }

    //    internal static bool FileExistsNoThrow(string fullPath)
    //    {
    //        fullPath = AttemptToShortenPath(fullPath);
    //        Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA lpFileInformation = new Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA();
    //        return (Microsoft.Build.Shared.NativeMethodsShared.GetFileAttributesEx(fullPath, 0, ref lpFileInformation) && ((lpFileInformation.fileAttributes & 0x10) == 0));
    //    }

    //    internal static bool FileOrDirectoryExistsNoThrow(string fullPath)
    //    {
    //        fullPath = AttemptToShortenPath(fullPath);
    //        Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA lpFileInformation = new Microsoft.Build.Shared.NativeMethodsShared.WIN32_FILE_ATTRIBUTE_DATA();
    //        return Microsoft.Build.Shared.NativeMethodsShared.GetFileAttributesEx(fullPath, 0, ref lpFileInformation);
    //    }

    //    internal static string GetCacheDirectory()
    //    {
    //        if (cacheDirectory == null)
    //        {
    //            cacheDirectory = Path.Combine(Path.GetTempPath(), string.Format(Thread.CurrentThread.CurrentUICulture, "MSBuild{0}", new object[] { Process.GetCurrentProcess().Id }));
    //        }
    //        return cacheDirectory;
    //    }

    //    internal static string GetDirectory(string fileSpec)
    //    {
    //        string directoryName = Path.GetDirectoryName(fileSpec);
    //        if (directoryName == null)
    //        {
    //            return fileSpec;
    //        }
    //        if ((directoryName.Length > 0) && !EndsWithSlash(directoryName))
    //        {
    //            directoryName = directoryName + Path.DirectorySeparatorChar;
    //        }
    //        return directoryName;
    //    }

    //    internal static string GetDirectoryNameOfFullPath(string fullPath)
    //    {
    //        if (fullPath == null)
    //        {
    //            return null;
    //        }
    //        int length = fullPath.Length;
    //        while (((length > 0) && (fullPath[--length] != Path.DirectorySeparatorChar)) && (fullPath[length] != Path.AltDirectorySeparatorChar))
    //        {
    //        }
    //        return fullPath.Substring(0, length);
    //    }

    //    internal static FileInfo GetFileInfoNoThrow(string filePath)
    //    {
    //        FileInfo info;
    //        filePath = AttemptToShortenPath(filePath);
    //        try
    //        {
    //            info = new FileInfo(filePath);
    //        }
    //        catch (Exception exception)
    //        {
    //            if (Microsoft.Build.Shared.ExceptionHandling.NotExpectedException(exception))
    //            {
    //                throw;
    //            }
    //            return null;
    //        }
    //        if (info.Exists)
    //        {
    //            return info;
    //        }
    //        return null;
    //    }

    //    internal static string GetFullPath(string fileSpec, string currentDirectory)
    //    {
    //        fileSpec = Microsoft.Build.Shared.EscapingUtilities.UnescapeAll(fileSpec);
    //        string str = Microsoft.Build.Shared.EscapingUtilities.Escape(NormalizePath(Path.Combine(currentDirectory, fileSpec)));
    //        if (EndsWithSlash(str))
    //        {
    //            return str;
    //        }
    //        Match match = Microsoft.Build.Shared.FileUtilitiesRegex.DrivePattern.Match(fileSpec);
    //        Match match2 = Microsoft.Build.Shared.FileUtilitiesRegex.UNCPattern.Match(str);
    //        if ((!match.Success || (match.Length != fileSpec.Length)) && (!match2.Success || (match2.Length != str.Length)))
    //        {
    //            return str;
    //        }
    //        return (str + Path.DirectorySeparatorChar);
    //    }

    //    internal static string GetFullPathNoThrow(string path)
    //    {
    //        try
    //        {
    //            path = NormalizePath(path);
    //        }
    //        catch (Exception exception)
    //        {
    //            if (Microsoft.Build.Shared.ExceptionHandling.NotExpectedException(exception))
    //            {
    //                throw;
    //            }
    //        }
    //        return path;
    //    }

    //    internal static bool HasExtension(string fileName, string[] allowedExtensions)
    //    {
    //        string extension = Path.GetExtension(fileName);
    //        foreach (string str2 in allowedExtensions)
    //        {
    //            if (string.Compare(extension, str2, true, CultureInfo.CurrentCulture) == 0)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    internal static bool IsMetaprojectFilename(string filename)
    //    {
    //        return string.Equals(Path.GetExtension(filename), ".metaproj", StringComparison.OrdinalIgnoreCase);
    //    }

    //    internal static bool IsRootedNoThrow(string path)
    //    {
    //        try
    //        {
    //            return Path.IsPathRooted(path);
    //        }
    //        catch (Exception exception)
    //        {
    //            if (Microsoft.Build.Shared.ExceptionHandling.NotExpectedException(exception))
    //            {
    //                throw;
    //            }
    //            return false;
    //        }
    //    }

        internal static bool IsSlash(char c)
        {
            if (c != Path.DirectorySeparatorChar)
            {
                return (c == Path.AltDirectorySeparatorChar);
            }
            return true;
        }

    //    internal static bool IsSolutionFilename(string filename)
    //    {
    //        return string.Equals(Path.GetExtension(filename), ".sln", StringComparison.OrdinalIgnoreCase);
    //    }

    //    internal static bool IsVCProjFilename(string filename)
    //    {
    //        return string.Equals(Path.GetExtension(filename), ".vcproj", StringComparison.OrdinalIgnoreCase);
    //    }

        internal static string MakeRelative(string basePath, string path)
        {
            string relativePath;

            if (basePath.Length == 0)
            {
                return path;
            }

            var baseUri = new Uri(EnsureTrailingSlash(basePath), UriKind.Absolute);
            var relativeUri = CreateUriFromPath(path);

            if (!relativeUri.IsAbsoluteUri)
            {
                relativeUri = new Uri(baseUri, relativeUri);
            }

            var uri3 = baseUri.MakeRelativeUri(relativeUri);

            relativePath = Uri.UnescapeDataString(uri3.IsAbsoluteUri ? uri3.LocalPath : uri3.ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            return relativePath;
        }

    //    internal static string NormalizePath(string path)
    //    {
    //        Microsoft.Build.Shared.ErrorUtilities.VerifyThrowArgumentLength(path, "path");
    //        int errorCode = 0;
    //        int num2 = Microsoft.Build.Shared.NativeMethodsShared.MAX_PATH;
    //        StringBuilder buffer = new StringBuilder(num2 + 1);
    //        int num3 = Microsoft.Build.Shared.NativeMethodsShared.GetFullPathName(path, buffer.Capacity, buffer, IntPtr.Zero);
    //        errorCode = Marshal.GetLastWin32Error();
    //        if (num3 > num2)
    //        {
    //            num2 = num3;
    //            buffer = new StringBuilder(num2 + 1);
    //            num3 = Microsoft.Build.Shared.NativeMethodsShared.GetFullPathName(path, buffer.Capacity, buffer, IntPtr.Zero);
    //            errorCode = Marshal.GetLastWin32Error();
    //            Microsoft.Build.Shared.ErrorUtilities.VerifyThrow((num3 + 1) < buffer.Capacity, "Final buffer capacity should be sufficient for full path name and null terminator.");
    //        }
    //        if (num3 <= 0)
    //        {
    //            errorCode = -2147024896 | errorCode;
    //            Marshal.ThrowExceptionForHR(errorCode);
    //            return null;
    //        }
    //        string message = buffer.ToString();
    //        if (message.Length >= 260)
    //        {
    //            throw new PathTooLongException(message);
    //        }
    //        message = Path.Combine(message, string.Empty);
    //        if (message.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
    //        {
    //            int num4 = 2;
    //            while (num4 < message.Length)
    //            {
    //                char ch = message[num4];
    //                if (ch.Equals('\\'))
    //                {
    //                    num4++;
    //                    break;
    //                }
    //                num4++;
    //            }
    //            if ((num4 == message.Length) || (message.IndexOf(@"\\?\globalroot", StringComparison.OrdinalIgnoreCase) != -1))
    //            {
    //                message = Path.GetFullPath(message);
    //            }
    //        }
    //        if (string.Equals(message, path, StringComparison.Ordinal))
    //        {
    //            message = path;
    //        }
    //        return message;
    //    }

    //    internal static string TrimAndStripAnyQuotes(string path)
    //    {
    //        path = path.Trim();
    //        path = path.Trim(new char[] { '"' });
    //        return path;
    //    }

    //    internal static string CurrentExecutableConfigurationFilePath
    //    {
    //        get
    //        {
    //            return (CurrentExecutablePath + ".config");
    //        }
    //    }

    //    internal static string CurrentExecutableDirectory
    //    {
    //        get
    //        {
    //            return Path.GetDirectoryName(CurrentExecutablePath);
    //        }
    //    }

    //    internal static string CurrentExecutablePath
    //    {
    //        get
    //        {
    //            if (executablePath == null)
    //            {
    //                StringBuilder buffer = new StringBuilder(Microsoft.Build.Shared.NativeMethodsShared.MAX_PATH);
    //                if (Microsoft.Build.Shared.NativeMethodsShared.GetModuleFileName(Microsoft.Build.Shared.NativeMethodsShared.NullHandleRef, buffer, buffer.Capacity) == 0)
    //                {
    //                    throw new Win32Exception();
    //                }
    //                executablePath = buffer.ToString();
    //            }
    //            return executablePath;
    //        }
    //    }
    }
}

