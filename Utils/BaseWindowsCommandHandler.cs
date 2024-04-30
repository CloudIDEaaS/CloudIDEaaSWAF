using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void OutputWriteLine(string format, params object[] args);
    public delegate void ErrorWriteLine(string format, params object[] args);

    public abstract class BaseWindowsCommandHandler : IDisposable, IBaseWindowsCommandHandler
    {
        protected Process process;
        protected ManualResetEvent resetEvent;
        private bool launchedInteractively;
        public OutputWriteLine OutputWriteLine { get; set; }
        public ErrorWriteLine ErrorWriteLine { get; set; }
        public string CommandExe { get; set; }
        public string Command { get; set; }
        public bool NoWait { get; set; }
        public bool Succeeded { get; protected set; }
        public Dictionary<string, string> EnvironmentVariables { get; }
        public event EventHandler ProcessStarted;
        public event EventHandler ProcessExited;

        public BaseWindowsCommandHandler(string exe)
        {
            resetEvent = new ManualResetEvent(false);

            if (Path.IsPathRooted(exe))
            {
                CommandExe = exe;
            }
            else
            {
                CommandExe = ProcessExtensions.FindExePath(exe);
            }

            this.Command = Path.GetFileNameWithoutExtension(exe);
            this.EnvironmentVariables = new Dictionary<string, string>();
        }

        protected void RaiseProcessExited()
        {
            ProcessExited?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseProcessStarted()
        {
            ProcessStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void LaunchForStdio(string workingDirectory, params string[] arguments)
        {
            ProcessStartInfo startInfo;
            var extension = Path.GetExtension(CommandExe);
            var name = Path.GetFileNameWithoutExtension(CommandExe);
            int exitCode;

            if (extension == ".exe")
            {
                startInfo = new ProcessStartInfo(CommandExe, arguments.ToDelimitedList(" "));
            }
            else
            {
                startInfo = new ProcessStartInfo("cmd.exe", "/c " + CommandExe.SurroundWithQuotes() + " " + arguments.ToDelimitedList(" "));
            }

            process = new Process();

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.WorkingDirectory = workingDirectory;

            if (this.EnvironmentVariables != null)
            {
                foreach (var pair in this.EnvironmentVariables)
                {
                    startInfo.EnvironmentVariables.Add(pair.Key, pair.Value);
                }
            }

            process.StartInfo = startInfo;

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            process.Start();

            if (ProcessStarted != null)
            {
                ProcessStarted(this, EventArgs.Empty);
            }

            if (!this.NoWait)
            {
                process.WaitForExit();

                exitCode = process.ExitCode;

                if (exitCode != 0)
                {
                    using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                    {
                        Console.WriteLine("{0} failed with ExitCode={0}", exitCode);
                    }
                }

                this.Succeeded = process.ExitCode == 0;
            }
        }

        protected virtual void RunCommand(string command, string workingDirectory, params string[] arguments)
        {
            if (launchedInteractively)
            {
                process.StandardInput.WriteLine(command + " " + arguments.ToDelimitedList(" "));
            }
            else
            {
                ProcessStartInfo startInfo;
                int exitCode;
                var extension = Path.GetExtension(CommandExe);

                if (extension == ".exe")
                {
                    if (command == null)
                    {
                        startInfo = new ProcessStartInfo(CommandExe, (string) null);
                    }
                    else
                    {
                        startInfo = new ProcessStartInfo(CommandExe, command + " " + arguments.ToDelimitedList(" "));
                    }
                }
                else
                {
                    startInfo = new ProcessStartInfo("cmd.exe", "/c " + CommandExe.SurroundWithQuotes() + " " + command + " " + arguments.ToDelimitedList(" "));
                }

                process = new Process();

                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.WorkingDirectory = workingDirectory;

                if (this.EnvironmentVariables != null)
                {
                    foreach (var pair in this.EnvironmentVariables)
                    {
                        startInfo.EnvironmentVariables.Add(pair.Key, pair.Value);
                    }
                }

                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;

                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;

                process.OutputDataReceived += (s, e2) =>
                {
                    var data = e2.Data;

                    if (data != null)
                    {
                        if (OutputWriteLine == null)
                        {
                            Console.WriteLine(data.FormatEscape());
                        }
                        else
                        {
                            OutputWriteLine(data.FormatEscape());
                        }
                    }
                };

                process.ErrorDataReceived += (s, e2) =>
                {
                    var data = e2.Data;

                    if (data != null)
                    {
                        if (ErrorWriteLine == null)
                        {
                            using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                            {
                                Console.WriteLine(data.FormatEscape());
                            }
                        }
                        else
                        {
                            ErrorWriteLine(data.FormatEscape());
                        }
                    }
                };

                process.Exited += (s, e2) =>
                {
                    exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        if (ErrorWriteLine == null)
                        {
                            using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                            {
                                Console.WriteLine("{0} failed with ExitCode={1}", Path.GetFileNameWithoutExtension(CommandExe), exitCode);
                            }
                        }
                        else
                        {
                            ErrorWriteLine("{0} failed with ExitCode={1}", Path.GetFileNameWithoutExtension(CommandExe), exitCode);
                        }
                    }

                    ProcessExited?.Invoke(this, EventArgs.Empty);

                    resetEvent.Set();
                };

                process.Start();

                if (ProcessStarted != null)
                {
                    ProcessStarted(this, EventArgs.Empty);
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (!this.NoWait)
                {
                    process.WaitForExit();

                    exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                        {
                            Console.WriteLine("{0} failed with ExitCode={0}", exitCode);
                        }
                    }

                    this.Succeeded = process.ExitCode == 0;
                }
            }
        }

        protected virtual void Run(string workingDirectory, params string[] arguments)
        {
            this.Run(workingDirectory, true, false, arguments);
        }

        protected virtual void Run(string workingDirectory, bool createNoWindow, bool noRedirect, params string[] arguments)
        {
            if (launchedInteractively)
            {
                process.StandardInput.WriteLine(arguments.ToDelimitedList(" "));
            }
            else
            {
                ProcessStartInfo startInfo;
                int exitCode;
                var extension = Path.GetExtension(CommandExe);

                if (extension == ".exe")
                {
                    startInfo = new ProcessStartInfo(CommandExe, arguments.ToDelimitedList(" "));
                }
                else
                {
                    startInfo = new ProcessStartInfo("cmd.exe", "/c " + CommandExe.SurroundWithQuotes() + " " + arguments.ToDelimitedList(" "));
                }

                process = new Process();

                if (!noRedirect)
                { 
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                }

                startInfo.WorkingDirectory = workingDirectory;

                if (this.EnvironmentVariables != null)
                {
                    foreach (var pair in this.EnvironmentVariables)
                    {
                        startInfo.EnvironmentVariables.Add(pair.Key, pair.Value);
                    }
                }

                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;

                startInfo.CreateNoWindow = createNoWindow;
                startInfo.UseShellExecute = false;

                process.OutputDataReceived += (s, e2) =>
                {
                    var data = e2.Data;

                    if (data != null)
                    {
                        if (OutputWriteLine == null)
                        {
                            Console.WriteLine(data.FormatEscape());
                        }
                        else
                        {
                            OutputWriteLine(data.FormatEscape());
                        }
                    }
                };

                process.ErrorDataReceived += (s, e2) =>
                {
                    var data = e2.Data;

                    if (data != null)
                    {
                        if (ErrorWriteLine == null)
                        {
                            using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                            {
                                Console.WriteLine(data.FormatEscape());
                            }
                        }
                        else
                        {
                            ErrorWriteLine(data.FormatEscape());
                        }
                    }
                };

                process.Exited += (s, e2) =>
                {
                    exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        if (ErrorWriteLine == null)
                        {
                            using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                            {
                                Console.WriteLine("{0} failed with ExitCode={1}", Path.GetFileNameWithoutExtension(CommandExe), exitCode);
                            }
                        }
                        else
                        {
                            ErrorWriteLine("{0} failed with ExitCode={1}", Path.GetFileNameWithoutExtension(CommandExe), exitCode);
                        }
                    }

                    ProcessExited?.Invoke(this, EventArgs.Empty);

                    resetEvent.Set();
                };

                process.Start();

                if (ProcessStarted != null)
                {
                    ProcessStarted(this, EventArgs.Empty);
                }

                if (!noRedirect)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }

                if (!this.NoWait)
                {
                    process.WaitForExit();

                    exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                        {
                            Console.WriteLine("{0} failed with ExitCode={0}", exitCode);
                        }
                    }

                    this.Succeeded = process.ExitCode == 0;
                }
            }
        }

        protected void LaunchInteractively(string workingDirectory, params string[] arguments)
        {
            ProcessStartInfo startInfo;
            var extension = Path.GetExtension(CommandExe);
            var name = Path.GetFileNameWithoutExtension(CommandExe);

            if (extension == ".exe")
            {
                startInfo = new ProcessStartInfo(CommandExe, arguments.ToDelimitedList(" "));
            }
            else
            {
                startInfo = new ProcessStartInfo("cmd.exe", "/c " + CommandExe.SurroundWithQuotes() + " " + arguments.ToDelimitedList(" "));
            }

            process = new Process();

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.WorkingDirectory = workingDirectory;

            process.StartInfo = startInfo;

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            process.EnableRaisingEvents = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Exited += Process_Exited;

            launchedInteractively = true;

            process.Start();

            if (ProcessStarted != null)
            {
                ProcessStarted(this, EventArgs.Empty);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        public bool IsProcessRunning
        {
            get
            {
                if (process == null)
                {
                    return false;
                }

                return !process.HasExited;
            }
        }

        public Process Process
        {
            get
            {
                return process;
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            var exitCode = process.ExitCode;
            var name = Path.GetFileNameWithoutExtension(CommandExe);

            if (exitCode != 0)
            {
                if (ErrorWriteLine == null)
                {
                    using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                    {
                        Console.WriteLine("{0} failed with ExitCode={1}", name, exitCode);
                    }
                }
                else
                {
                    ErrorWriteLine("{0} failed with ExitCode={1}", name, exitCode);
                }
            }

            ProcessExited?.Invoke(this, EventArgs.Empty);
            
            resetEvent.Set();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            if (data != null)
            {
                if (ErrorWriteLine == null)
                {
                    using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                    {
                        Console.WriteLine(data);
                    }
                }
                else
                {
                    ErrorWriteLine(data);
                }
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;

            if (data != null)
            {
                if (OutputWriteLine == null)
                {
                    Console.WriteLine(data);
                }
                else
                {
                    OutputWriteLine(data);
                }
            }
        }

        public StreamWriter Writer
        {
            get
            {
                return process.StandardInput;
            }
        }

        public StreamReader Reader
        {
            get
            {
                return process.StandardOutput;
            }
        }

        public void Wait()
        {
            if (launchedInteractively)
            {
                int exitCode;

                process.WaitForExit();
                exitCode = process.ExitCode;

                if (exitCode != 0)
                {
                    using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                    {
                        Console.WriteLine("dotnet failed with ExitCode={0}", exitCode);
                    }
                }

            }

            resetEvent.WaitOne();
        }

        public void Wait(int milliseconds)
        {
            if (launchedInteractively)
            {
                int exitCode;

                process.WaitForExit(milliseconds);
                exitCode = process.ExitCode;

                if (exitCode != 0)
                {
                    using (var colorizer = new ConsoleColorizer(ConsoleColor.Red))
                    {
                        Console.WriteLine("dotnet failed with ExitCode={0}", exitCode);
                    }
                }

            }

            resetEvent.WaitOne();
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited)
            {
                process.CloseMainWindow();
                process.Close();

                Thread.Sleep(1000);

                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
        }

        void IBaseWindowsCommandHandler.RunCommand(string command, string workingDirectory, params string[] arguments)
        {
            this.RunCommand(command, workingDirectory, arguments);
        }
    }
}
