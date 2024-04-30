using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class PlatformProcess
    {
        public string Path { get; internal set; }
        public string CommandLine { get; internal set; }
        private bool hasData = false;
        private PerformanceCounter processTimeCounter;
        private PerformanceCounter memoryPercentCounter;
        private PerformanceCounter readBytesPerSecCounter;
        private PerformanceCounter writeBytesPerSecCounter;
        private PerformanceCounter bandwidthCounter;
        private static string networkInterface;
        private Process process;
        private string processName;

        public int MaximumCpuUsage { private set; get; }

        static PlatformProcess()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && !networkInterface.Description.Contains("Virtual"))
                {
                    PlatformProcess.networkInterface = networkInterface.Description;
                    break;
                }
            }
        }
        public Process Process 
        {
            get
            {
                return process;
            }

            set
            {
                process = value;
                processName = process.ProcessName;
            }
        }

        public string ProcessName
        {
            get
            {
                return processName;
            }
        }

        public IEnumerable<PlatformProcess> ChildProcesses
        {
            get
            {
                var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + this.Process.Id);
                var collection = searcher.Get();

                foreach (var managementBaseObject in collection)
                {
                    PlatformProcess platformProcess = null;

                    try
                    {
                        var id = Convert.ToInt32(managementBaseObject["ProcessID"]);

                        platformProcess = new PlatformProcess
                        {
                            Process = Process.GetProcessById(id),
                            Path = (string)managementBaseObject["ExecutablePath"],
                            CommandLine = (string)managementBaseObject["CommandLine"],
                        };
                    }
                    catch
                    {
                    }

                    if (platformProcess != null)
                    {
                        yield return platformProcess;
                    }

                }
                
                yield break;
            }
        }

        public IEnumerable<PlatformProcess> DescendantProcesses
        {
            get
            {
                var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + this.Process.Id);
                var collection = searcher.Get();

                foreach (var managementBaseObject in collection)
                {
                    PlatformProcess platformProcess = null;

                    try
                    {
                        var id = Convert.ToInt32(managementBaseObject["ProcessID"]);

                        platformProcess = new PlatformProcess
                        {
                            Process = Process.GetProcessById(id),
                            Path = (string)managementBaseObject["ExecutablePath"],
                            CommandLine = (string)managementBaseObject["CommandLine"],
                        };

                    }
                    catch
                    {
                    }

                    if (platformProcess != null)
                    {
                        List<PlatformProcess> childProcesses = null;

                        try
                        {
                            childProcesses = platformProcess.ChildProcesses.ToList();
                        }
                        catch
                        {
                        }

                        yield return platformProcess;

                        if (childProcesses != null)
                        {
                            foreach (var descendant in childProcesses)
                            {
                                yield return descendant;
                            }
                        }

                    }
                }

                yield break;
            }
        }

        private void Init()
        {
            var instanceName = FindInstanceName("Process");

            if (hasData)
            {
                return;
            }

            if (CheckForPerformanceCounterCategoryExist("Process"))
            {
                processTimeCounter = new PerformanceCounter("Process", "% Processor Time", instanceName);
                readBytesPerSecCounter = new PerformanceCounter("Process", "IO Read Bytes/sec", instanceName);
                writeBytesPerSecCounter = new PerformanceCounter("Process", "IO Write Bytes/sec", instanceName);
                memoryPercentCounter = new PerformanceCounter("Process", "Working Set", instanceName);

                processTimeCounter.NextValue();
            }

            if (CheckForPerformanceCounterCategoryExist("Network Interface"))
            {
                bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkInterface);
            }

            MaximumCpuUsage = 0;

            hasData = true;
        }

        private bool CheckForPerformanceCounterCategoryExist(string categoryName)
        {
            return PerformanceCounterCategory.Exists(categoryName);
        }

        public string FindInstanceName(string categoryName)
        {
            var result = String.Empty;

            if (!ReferenceEquals(this.Process, null))
            {
                if (!String.IsNullOrEmpty(categoryName))
                {
                    if (CheckForPerformanceCounterCategoryExist(categoryName))
                    {
                        var category = new PerformanceCounterCategory(categoryName);
                        var instances = category.GetInstanceNames();
                        var processName = this.Process.ProcessName;

                        if (instances != null)
                        {
                            foreach (var instance in instances)
                            {
                                if (instance.ToLower().Equals(processName.ToLower()))
                                {
                                    result = instance;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public float MemoryUsage
        {
            get
            {
                try
                {
                    Init();

                    if (!ReferenceEquals(memoryPercentCounter, null))
                    {
                        var result = memoryPercentCounter.NextValue();

                        return result;
                    }
                }
                catch (Exception ex)
                {
                }

                return 0;
            }
        }

        public float NetworkUtilization
        {
            get
            {
                try
                { 
                    Init();

                    if (!ReferenceEquals(readBytesPerSecCounter, null) && !ReferenceEquals(writeBytesPerSecCounter, null) && !ReferenceEquals(bandwidthCounter, null))
                    {
                        var numberOfIterations = 10;
                        float bandwidth = bandwidthCounter.NextValue();
                        float readSum = 0;
                        float writeSum = 0;
                        float dataSent;
                        float dataReceived;
                        float totalData;
                        double utilization;

                        for (var index = 0; index < numberOfIterations; index++)
                        {
                            readSum += readBytesPerSecCounter.NextValue();
                            writeSum += writeBytesPerSecCounter.NextValue();
                        }

                        dataSent = readSum;
                        dataReceived = writeSum;
                        totalData = dataSent + dataReceived;

                        if (totalData > 0)
                        {
                            utilization = (8 * totalData) / (bandwidth * numberOfIterations) * 100;
                        }
                        else
                        {
                            utilization = 0;
                        }

                        return (float) utilization;
                    }
                }
                catch (Exception ex)
                {
                }

                return 0;
            }
        }

        public int CpuUsage
        {
            get
            {
                try
                {
                    Init();

                    if (!ReferenceEquals(processTimeCounter, null))
                    {
                        var result = (int)processTimeCounter.NextValue();
                        result /= Environment.ProcessorCount;

                        if (MaximumCpuUsage < result)
                        {
                            MaximumCpuUsage = result;
                        }

                        return result;
                    }
                }
                catch (Exception ex)
                {
                }

                return 0;
            }
        }

    }
}
