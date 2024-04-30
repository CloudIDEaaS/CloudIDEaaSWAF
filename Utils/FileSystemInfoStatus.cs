using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.ProcessHelpers;

namespace Utils
{
    public class FileSystemInfoStatus
    {
        public FileSystemInfo FileSystemInfo { get; private set; }
        public int OriginalCount { get; set; }
        public bool NeedElevationForLockingProcessList { get; }
        public int Count { get; set; }
        public List<HandleInfo> OpenFileHandles { get; set; }
        public List<Process> LockingProcesses { get; set; }
        public Exception Exception { get; set; }
        public bool Continue { get; set; }
        public bool Deleted { get; set; }

        public FileSystemInfoStatus(FileSystemInfo fileSystemInfo)
        {
            this.FileSystemInfo = fileSystemInfo;
            this.Count = -1;
            this.OriginalCount = -1;
        }

        public FileSystemInfoStatus(FileSystemInfo fileSystemInfo, Exception exception, List<Process> lockingProcesses)
        {
            this.FileSystemInfo = fileSystemInfo;
            this.Count = -1;
            this.OriginalCount = -1;
            this.LockingProcesses = lockingProcesses;
            this.Exception = exception;
        }

        public FileSystemInfoStatus(FileSystemInfo fileSystemInfo, Exception exception, bool needElevation)
        {
            this.FileSystemInfo = fileSystemInfo;
            this.Count = -1;
            this.OriginalCount = -1;
            this.NeedElevationForLockingProcessList = needElevation;
            this.Exception = exception;
        }
    }
}
