using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.ProcessHelpers;

namespace Utils
{
    public class ProcessAdvancedInfoEx
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public PROCESS_BASIC_INFORMATION BasicInformation { get; set; }
        public RtlUserProcessParameters ProcessParameters { get; set; }
        public string[] CommandLine { get; set; }
        public string CommandLineFull { get; set; }
        public string Conflict { get; set; }
        public string Name { get; set; }
        public string WindowTitle { get; set; }

        public ProcessAdvancedInfoEx(ProcessAdvancedInfo processAdvancedInfo, Process process)
        {
            processAdvancedInfo.CopyTo(this);

            this.Handle = processAdvancedInfo.Handle.ToHexString(true);
            this.Name = process.ProcessName;

            try
            {
                if (process.MainWindowTitle != null)
                {
                    this.WindowTitle = process.MainWindowTitle;
                }
            }
            catch
            { 
            }
        }
    }
}
