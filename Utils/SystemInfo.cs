using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Management;

namespace Utils
{
    public static class SystemInfo
    {
        private static Process thisProc;
        private static bool hasData = false;
        private static PerformanceCounter processTimeCounter;
        private static PerformanceCounter memoryPercentCounter;
        private static PerformanceCounter systemUpTimeCounter;
        public static float MaximumCpuUsageForCurrentProcess { private set; get; }
        public static float MaximumMemoryUsageForCurrentProcess { private set; get; }
        public static int MaximumIPConnectionsForCurrentProcess { private set; get; }
        public static bool UseProcessorID = true;
        public static bool UseBaseBoardProduct = true;
        public static bool UseBaseBoardManufacturer = true;
        public static bool UseDiskDriveSignature = true;
        public static bool UseVideoControllerCaption = true;
        public static bool UsePhysicalMediaSerialNumber = true;
        public static bool UseBiosVersion = true;
        public static bool UseBiosManufacturer = true;
        public static bool UseWindowsSerialNumber = true;

        private static void Init()
        {
            if (hasData)
            {
                return;
            }

            if (CheckForPerformanceCounterCategoryExist("Process"))
            {
                processTimeCounter = new PerformanceCounter();

                processTimeCounter.CategoryName = "Process";
                processTimeCounter.CounterName = "% Processor Time";
                processTimeCounter.InstanceName = FindInstanceName("Process");

                if (processTimeCounter.InstanceName.IsNullOrEmpty())
                {
                    Thread.Sleep(100);

                    processTimeCounter.InstanceName = FindInstanceName("Process");
                }

                processTimeCounter.NextValue();
            }

            if (CheckForPerformanceCounterCategoryExist("Memory"))
            {
                memoryPercentCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            }

            if (CheckForPerformanceCounterCategoryExist("System"))
            {
                systemUpTimeCounter = new PerformanceCounter("System", "System Up Time");
            }

            MaximumCpuUsageForCurrentProcess = 0;
            MaximumMemoryUsageForCurrentProcess = 0;
            MaximumIPConnectionsForCurrentProcess = 0;

            hasData = true;
        }

        public static string GetUniqueSystemInfo(string softwareName = null)
        {
            if (softwareName == null)
            { 
                softwareName = Assembly.GetExecutingAssembly().GetName().FullName;
            }

            if (UseProcessorID == true)
            {
                softwareName += RunQuery("Processor", "ProcessorId");
            }

            if (UseBaseBoardProduct == true)
            {
                softwareName += RunQuery("BaseBoard", "Product");
            }

            if (UseBaseBoardManufacturer == true)
            {
                softwareName += RunQuery("BaseBoard", "Manufacturer");
            }

            if (UseDiskDriveSignature == true)
            {
                softwareName += RunQuery("DiskDrive", "SerialNumber");
            }

            if (UseVideoControllerCaption == true)
            {
                softwareName += RunQuery("VideoController", "Caption");
            }

            if (UsePhysicalMediaSerialNumber == true)
            {
                softwareName += RunQuery("PhysicalMedia", "SerialNumber");
            }

            if (UseBiosVersion == true)
            {
                softwareName += RunQuery("BIOS", "Version");
            }

            if (UseWindowsSerialNumber == true)
            {
                softwareName += RunQuery("OperatingSystem", "SerialNumber");
            }

            softwareName = RemoveUseLess(softwareName);

            if (softwareName.Length < 25)
            {
                return GetUniqueSystemInfo(softwareName);
            }

            return softwareName.Substring(0, 25).ToUpper();
        }

        private static string RemoveUseLess(string st)
        {
            char ch;
            for (int i = st.Length - 1; i >= 0; i--)
            {
                ch = char.ToUpper(st[i]);

                if ((ch < 'A' || ch > 'Z') &&
                    (ch < '0' || ch > '9'))
                {
                    st = st.Remove(i, 1);
                }
            }
            return st;
        }

        private static string RunQuery(string TableName, string MethodName)
        {
            var searcher = new ManagementObjectSearcher("Select * from Win32_" + TableName);

            foreach (ManagementObject managementObject in searcher.Get())
            {
                try
                {
                    return managementObject[MethodName].ToString();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }
            return "";
        }
        private static bool CheckForPerformanceCounterCategoryExist(string categoryName)
        {
            return PerformanceCounterCategory.Exists(categoryName);
        }

        public static string FindInstanceName(string categoryName)
        {
            var result = String.Empty;

            thisProc = Process.GetCurrentProcess();

            if (!ReferenceEquals(thisProc, null))
            {
                if (!String.IsNullOrEmpty(categoryName))
                {
                    if (CheckForPerformanceCounterCategoryExist(categoryName))
                    {
                        var category = new PerformanceCounterCategory(categoryName);
                        var instances = category.GetInstanceNames();
                        var processName = thisProc.ProcessName;

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

        public static double NetworkUtilization
        {
            get
            {
                var category = new PerformanceCounterCategory("Network Interface");
                var instanceNames = category.GetInstanceNames().ToList();
                var count = instanceNames.Count;
                double total = 0;

                foreach (string name in instanceNames)
                {
                    total += GetNetworkUtilization(name);
                }

                return total / count;
            }
        }

        private static double GetNetworkUtilization(string networkCard)
        {
            const int numberOfIterations = 10;
            float sendSum = 0;
            float receiveSum = 0;
            float bandwidth;
            float dataSent;
            float dataReceived;
            double utilization;
            PerformanceCounter bandwidthCounter;
            PerformanceCounter dataSentCounter;
            PerformanceCounter dataReceivedCounter;

            bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);
            bandwidth = bandwidthCounter.NextValue();

            dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);

            dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            for (var index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();

                Thread.Sleep(1);
            }

            dataSent = sendSum;
            dataReceived = receiveSum;

            if (bandwidth == 0)
            {
                utilization = 0;
            }
            else
            {
                utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            }

            return utilization;
        }

        public static float SystemUpTime
        {
            get
            {
                Init();

                if (!ReferenceEquals(systemUpTimeCounter, null))
                {
                    var result = systemUpTimeCounter.NextValue();

                    return result;
                }

                return 0;
            }
        }

        public static float MemoryUsageForCurrentProcess
        {
            get
            {
                Init();

                if (!ReferenceEquals(memoryPercentCounter, null))
                {
                    var result = memoryPercentCounter.NextValue();

                    if (MaximumMemoryUsageForCurrentProcess < result)
                    {
                        MaximumMemoryUsageForCurrentProcess = result;
                    }

                    return result;
                }

                return 0;
            }
        }

        public static float CpuUsageForCurrentProcess
        {
            get
            {
                Init();

                if (!ReferenceEquals(processTimeCounter, null))
                {
                    var result = processTimeCounter.NextValue();
                    result /= Environment.ProcessorCount;

                    if (MaximumCpuUsageForCurrentProcess < result)
                    {
                        MaximumCpuUsageForCurrentProcess = result;
                    }

                    return result;
                }

                return 0;
            }
        }

        public static int IPConnectionsForCurrentProcess
        {
            get
            {
                Init();

                var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                var result = ipGlobalProperties.GetActiveTcpConnections().Length;

                if (MaximumIPConnectionsForCurrentProcess < result)
                {
                    MaximumIPConnectionsForCurrentProcess = result;
                }

                return 0;
            }
        }
    }
}
