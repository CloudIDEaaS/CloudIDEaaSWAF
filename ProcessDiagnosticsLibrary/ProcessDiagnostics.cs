using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ProcessDiagnosticsService;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ProcessDiagnosticsLibrary
{
    public class ProcessDiagnostics : IProcessDiagnostics
    {
        internal ProcessDiagnostics()
        {
        }

        public static IProcessDiagnostics GetDiagnostics()
        {
            return new ProcessDiagnostics();
        }

        public static Uri Address
        {
            get
            {
                var address = new Uri("net.pipe://localhost/ProcessDiagnosticsService");

                return address;
            }
        }

        public static NetNamedPipeBinding DefaultBinding
        {
            get
            {
                var binding = new NetNamedPipeBinding();

                binding.SendTimeout = new TimeSpan(0, 0, 30, 0);
                binding.MaxBufferSize = Int32.MaxValue;
                binding.MaxBufferPoolSize = 2147483647;
                binding.MaxReceivedMessageSize = Int32.MaxValue;

                return binding;
            }
        }

        public bool LaunchIfNotStarted()
        {
            bool failing = true;
            bool serverLaunched = false;
            int attempts = 0;

            while (failing)
            {
                failing = false;
                attempts++;

                try
                {
                    var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                    var binding = ProcessDiagnostics.DefaultBinding;

                    var copyServiceChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                    var result = copyServiceChannel.Ping();

                    Debug.Assert(result == "Success");

                    return true;
                }
                catch (Exception ex)
                {
                    if (attempts > 20)
                    {
                        MessageBox.Show("Failing to launch ProcessDiagnosticsService.  Aborting attempts", typeof(ProcessDiagnostics).Name);
                        return false;
                    }

                    if (!serverLaunched)
                    {
                        var process = Process.Start(ProcessDiagnostics.ServerExeLocation);

                        process.Exited += (sender, e) =>
                        {
                            if (process.ExitCode != 0)
                            {

                            }
                        };

                        serverLaunched = true;
                    }

                    failing = true;
                    Thread.Sleep(500);
                }
            }

            return true;
        }

        public bool AlreadyRunning
        {
            get
            {
                try
                {
                    var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                    var binding = ProcessDiagnostics.DefaultBinding;

                    var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                    var result = processDiagnosticsChannel.Ping();

                    Debug.Assert(result == "Success");

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static string ServerExeLocation
        {
            get
            {
                return Path.Combine(ProcessDiagnostics.RootLocation, @"Hydra.Runtime\ProcessDiagnostics\bin\Debug\ProcessDiagnostics.exe");
            }
        }

        public static string RootLocation
        {
            get
            {
                return Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%");
            }
        }

        public void RegisterProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage)
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.RegisterProcess(processId, imageFile, baseAddress, sizeOfImage);
            }
            else
            {
                Debugger.Break();
            }
        }

        public void RegisterTestProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage, TestModule[] modules)
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.RegisterTestProcess(processId, imageFile, baseAddress, sizeOfImage, modules);
            }
            else
            {
                Debugger.Break();
            }
        }

        public string Ping()
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                return processDiagnosticsChannel.Ping();
            }
            else
            {
                Debugger.Break();
            }

            return null;
        }

        public void RegisterRegion(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type)
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.RegisterRegion(baseAddress, allocationBase, allocationProtect, regionSize, state, protect, type);
            }
            else
            {
                Debugger.Break();
            }
        }

        public void RegisterRegionName(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type, string name)
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.RegisterRegionName(baseAddress, allocationBase, allocationProtect, regionSize, state, protect, type, name);
            }
            else
            {
                Debugger.Break();
            }
        }

        public void FreezeUI()
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.FreezeUI();
            }
            else
            {
                Debugger.Break();
            }
        }

        public void UnfreezeUI()
        {
            LaunchIfNotStarted();

            if (Process.GetProcessesByName("ProcessDiagnosticsService").Any())
            {
                var endpointAddress = new EndpointAddress(ProcessDiagnostics.Address);
                var binding = ProcessDiagnostics.DefaultBinding;
                var processDiagnosticsChannel = new ChannelFactory<IProcessDiagnosticsService>(binding, endpointAddress).CreateChannel();

                processDiagnosticsChannel.UnfreezeUI();
            }
            else
            {
                Debugger.Break();
            }
        }
    }
}
