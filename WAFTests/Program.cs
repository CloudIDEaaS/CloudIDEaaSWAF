using WebSecurity.StartupTests.Helpers;
using System.Diagnostics;
using System.Reflection;
using Utils;
using Xunit.Runners;

namespace WebSecurity.StartupTests
{
    public class Program
    {
        // We use consoleLock because messages can arrive in parallel, so we want to make sure we get
        // consistent console output.

        // Use an event to know when we're done
        private static ManualResetEvent finished;

        // Start out assuming success; we'll set this to 1 if we get a failed test
        private static int result = 0;
        public static IIntegrationTestFixtureCoordinator Coordinator { [DebuggerStepThrough] get; } = new IntegrationTestFixtureCoordinator();
        public static DatabaseKind DatabaseKind { get; set; }
        private static object consoleLock = new object();

        public static int Main(string[] args)
        {
            var testAssembly = Assembly.GetExecutingAssembly();
            var testAssemblyPath = testAssembly.Location;
            var inProcess = args.Any(a => a == "/" + Switches.IN_PROCESS);
            var isFirstTestKindFlag = true;
            CommandLineParseResult parseResult;

            ControlExtensions.ShowConsoleInSecondaryMonitor(System.Windows.Forms.FormWindowState.Maximized);

            parseResult = CommandLineParser.ParseArgs<CommandLineParseResult>(args, (result, arg) =>
            {
            },
            (result, _switch, switchArg) =>
            {
                var enumText = _switch.RemoveStartIfMatches("Run");

                if (EnumUtils.GetNames<TestKind>().Any(n => n ==  enumText))
                {
                    var value = EnumUtils.GetValue<TestKind>(enumText);

                    if (isFirstTestKindFlag)
                    {
                        result.TestKinds = TestKind.None;
                        isFirstTestKindFlag = false;
                    }

                    result.TestKinds = EnumUtils.SetFlag(result.TestKinds, value);
                }

                switch (_switch)
                {
                    case Switches.LOAD_LOGGING_TRACE_LISTENER:

                        try
                        {
                            var solutionPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%"));
                            var listenerPath = Path.Combine(solutionPath, @"LoggingTraceListener\bin\Debug\netcoreapp3.1\LoggingTraceListener.exe");

                            Debug.Assert(File.Exists(listenerPath));

                            _ = Task.Run(() =>
                            {
                                var process = new Process();

                                process.StartInfo = new ProcessStartInfo
                                {
                                    FileName = listenerPath,
                                    Arguments = "/LaunchDebugger"
                                };

                                process.Start();
                            });

                            Thread.Sleep(30000);
                        }
                        catch (Exception ex)
                        {
                            result.DisplayMessage("Error loading trace listener: '{0}'", ex.Message);
                        }

                        break;

                    case Switches.TEST_TO_RUN:

                        try
                        {
                            var typeName = switchArg.LeftUpToLastIndexOf('.');
                            var methodName = switchArg.RightAtLastIndexOf('.');
                            var type = testAssembly.GetTypes().Single(t => t.FullName == typeName);
                            var method = type.GetMethod(methodName);

                            result.TestToRunMethod = method!;
                        }
                        catch
                        {
                            result.DisplayMessage("Invalid Test Name: '{0}'.  Must be in the format [FullNamespace.Type.Method]", switchArg.AsDisplayText());
                        }

                        break;

                    case Switches.DATABASE_KIND:

                        try
                        {
                            result.DatabaseKind = Enum.Parse<DatabaseKind>(switchArg);
                        }
                        catch (Exception ex)
                        {
                            result.DisplayMessage("Invalid DatabaseKind, {0}", switchArg.AsDisplayText());
                        }

                        break;

                    case Switches.NO_HEADLESS_BROWSER:

                        try
                        {
                            result.NoHeadlessBrowser = bool.Parse(switchArg);
                        }
                        catch (Exception ex)
                        {
                            result.DisplayMessage("Invalid DatabaseKind, {0}", switchArg.AsDisplayText());
                        }

                        break;

                    case "version":

                        result.DisplayVersion(inProcess);
                        break;

                    case "help":

                        Switches.UnitTests = testAssembly.GetTests(TestKind.UnitTests).Select(m => m.Name).ToMultiLineList().SurroundWith("\r\n");
                        Switches.IntegrationTests = testAssembly.GetTests(TestKind.IntegrationTests).Select(m => m.Name).ToMultiLineList().SurroundWith("\r\n");
                        Switches.StartupTests = testAssembly.GetTests(TestKind.StartupTests).Select(m => m.Name).ToMultiLineList().SurroundWith("\r\n");

                        result.DisplayHelp(true, inProcess);

                        break;
                }
            });

            if (parseResult.HelpDisplayed || parseResult.VersionDisplayed || parseResult.MessageDisplayed)
            {
                return 0;
            }

            AppDomain.CurrentDomain.SetData(Switches.NO_HEADLESS_BROWSER, parseResult.NoHeadlessBrowser);

            Program.DatabaseKind = parseResult.DatabaseKind;

            TestHelpers.RunPreFixtureTestSetup();

            using (var runner = Xunit.Runners.AssemblyRunner.WithoutAppDomain(testAssemblyPath))
            {
                runner.OnDiscoveryComplete = OnDiscoveryComplete;
                runner.OnExecutionComplete = OnExecutionComplete;
                runner.OnTestFailed = OnTestFailed;
                runner.OnTestSkipped = OnTestSkipped;
                runner.OnTestStarting = OnTestStarting;
                runner.OnTestFinished = OnTestFinished;

                Console.WriteLine("Discovering...");

                if (parseResult.TestToRunMethod != null)
                {
                    var method = parseResult.TestToRunMethod;
                    var type = method.DeclaringType;

                    using (ConsoleColorizer.UseColor(ConsoleColor.DarkYellow))
                    {
                        Console.WriteLine($"Only running test: {method.Name}");
                    }

                    finished = new ManualResetEvent(false);

                    runner.TestCaseFilter = (t) =>
                    {
                        return t.TestMethod.Method.Name == method.Name;
                    };

                    runner.Start(type.FullName);

                    finished.WaitOne();
                    finished.Dispose();

                    return result;
                }

                if (parseResult.TestKinds.HasFlag(TestKind.StartupTests))
                {
                    var types = testAssembly.GetTestTypes(TestKind.StartupTests).ToList();

                    Console.WriteLine("Running StartupTests, tests classes found: {0}", types.Count);

                    if (types.Count > 0)
                    {
                        foreach (var type in types)
                        {
                            finished = new ManualResetEvent(false);

                            runner.Start(type.FullName);

                            finished.WaitOne();
                            finished.Dispose();
                        }
                    }
                }

                if (parseResult.TestKinds.HasFlag(TestKind.UnitTests))
                {
                    var types = testAssembly.GetTestTypes(TestKind.UnitTests).ToList();

                    Console.WriteLine("Running UnitTests, tests classes found: {0}", types.Count);

                    if (types.Count > 0)
                    {
                        foreach (var type in types)
                        {
                            finished = new ManualResetEvent(false);

                            runner.Start(type.FullName);

                            finished.WaitOne();
                            finished.Dispose();
                        }
                    }
                }

                if (parseResult.TestKinds.HasFlag(TestKind.IntegrationTests))
                {
                    var types = testAssembly.GetTestTypes(TestKind.IntegrationTests).ToList();

                    Console.WriteLine("Running IntegrationTests, tests classes found: {0}", types.Count);

                    if (types.Count > 0)
                    {
                        foreach (var type in types)
                        {
                            finished = new ManualResetEvent(false);

                            runner.Start(type.FullName);

                            finished.WaitOne();
                            finished.Dispose();
                        }
                    }
                }

                if (!inProcess)
                {
                    Console.WriteLine("\r\nPress any key to exit.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\r\nContinuing startup\r\n");
                }

                return result;
            }
        }

        private static void OnTestStarting(TestStartingInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"\r\nRunning {info.TestDisplayName}" + "*".Repeat(50) + "\r\n");
            }
        }

        private static void OnTestFinished(TestFinishedInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"\r\nCompleted {info.TestDisplayName}");
            }
        }

        static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
            }
        }

        static void OnExecutionComplete(ExecutionCompleteInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");
            }

            finished.Set();
        }

        static void OnTestFailed(TestFailedInfo info)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("[FAIL] {0}: {1}", info.TestDisplayName, info.ExceptionMessage);

                if (info.ExceptionStackTrace != null)
                {
                    Console.WriteLine(info.ExceptionStackTrace);
                }

                Console.ResetColor();
            }

            result = 1;
        }

        static void OnTestSkipped(TestSkippedInfo info)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[SKIP] {0}: {1}", info.TestDisplayName, info.SkipReason);
                Console.ResetColor();
            }
        }
    }
}