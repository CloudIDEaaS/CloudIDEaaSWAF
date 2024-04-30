using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class MemoryStatus
    {
        public float MinWorkingSetStarting { get; set; }
        public float MaxWorkingSetStarting { get; set; }
        public float NonpagedSystemMemorySize64Starting { get; set; }
        public float PagedMemorySize64Starting { get; set; }
        public float PagedSystemMemorySize64Starting { get; set; }
        public float PeakPagedMemorySize64Starting { get; set; }
        public float PeakVirtualMemorySize64Starting { get; set; }
        public float PeakWorkingSet64Starting { get; set; }
        public float VirtualMemorySize64Starting { get; set; }
        public float WorkingSet64Starting { get; set; }
        public float MinWorkingSetCurrent { get; set; }
        public float MaxWorkingSetCurrent { get; set; }
        public float NonpagedSystemMemorySize64Current { get; set; }
        public float PagedMemorySize64Current { get; set; }
        public float PagedSystemMemorySize64Current { get; set; }
        public float PeakPagedMemorySize64Current { get; set; }
        public float PeakVirtualMemorySize64Current { get; set; }
        public float PeakWorkingSet64Current { get; set; }
        public float VirtualMemorySize64Current { get; set; }
        public float WorkingSet64Current { get; set; }
        public float MemoryUsageStarting { get; set; }
        public float MemoryUsageCurrent { get; set; }
        private Process process;

        public void Init()
        {
            process = Process.GetCurrentProcess();

            MinWorkingSetStarting = (float) process.MinWorkingSet;
            MaxWorkingSetStarting = (float) process.MaxWorkingSet;
            NonpagedSystemMemorySize64Starting = (float) process.NonpagedSystemMemorySize64;
            PagedMemorySize64Starting = (float)process.PagedMemorySize64;
            PagedSystemMemorySize64Starting = (float)process.PagedSystemMemorySize64;
            PeakPagedMemorySize64Starting = (float)process.PeakPagedMemorySize64;
            PeakVirtualMemorySize64Starting = (float)process.PeakVirtualMemorySize64;
            PeakWorkingSet64Starting = (float)process.PeakWorkingSet64;
            VirtualMemorySize64Starting = (float)process.VirtualMemorySize64;
            WorkingSet64Starting = (float)process.WorkingSet64;
            MemoryUsageStarting = (float)SystemInfo.MemoryUsageForCurrentProcess;

            Update();
        }

        public void Update()
        {
            process = Process.GetCurrentProcess();

            MinWorkingSetCurrent = (float)process.MinWorkingSet;
            MaxWorkingSetCurrent = (float)process.MaxWorkingSet;
            NonpagedSystemMemorySize64Current = (float)process.NonpagedSystemMemorySize64;
            PagedMemorySize64Current = (float)process.PagedMemorySize64;
            PagedSystemMemorySize64Current = (float)process.PagedSystemMemorySize64;
            PeakPagedMemorySize64Current = (float)process.PeakPagedMemorySize64;
            PeakVirtualMemorySize64Current = (float)process.PeakVirtualMemorySize64;
            PeakWorkingSet64Current = (float)process.PeakWorkingSet64;
            VirtualMemorySize64Current = (float)process.VirtualMemorySize64;
            WorkingSet64Current = (float)process.WorkingSet64;
            MemoryUsageCurrent = (float)SystemInfo.MemoryUsageForCurrentProcess;
        }

        public static MemoryStatus Create()
        {
            var memoryStatus = new MemoryStatus();

            memoryStatus.Init();

            return memoryStatus;
        }
    }
}
