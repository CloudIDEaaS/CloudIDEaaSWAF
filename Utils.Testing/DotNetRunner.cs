using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Utils
{
    public static class DotNetRunner
    {
        public static Process DotNetRun(string arguments)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files\dotnet\dotnet.exe",
                Arguments = "run " + arguments
            });
        }

        public static Process DotNet(string arguments)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files\dotnet\dotnet.exe",
                Arguments = arguments
            });
        }

        public static Process RunExe(string exe)
        {
            var fileInfo = new FileInfo(exe);

            Directory.SetCurrentDirectory(fileInfo.DirectoryName);

            return Process.Start(new ProcessStartInfo
            {
                FileName = fileInfo.Name,
            });
        }
    }
}
