using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Runner.Reporters;
using Xunit.Sdk;

namespace Utils.Testing
{
    public static class AssemblyRunner
    {
        private static object consoleLock = new object();
        private static volatile bool cancel;
        private static readonly ConcurrentDictionary<string, ExecutionSummary> completionMessages = new ConcurrentDictionary<string, ExecutionSummary>();

        public static IDisposable WithoutAppDomain(string assemblyPath)
        {
            var testAssembly = Assembly.LoadFrom(assemblyPath);

            var assembly = new XunitProjectAssembly
            {
                AssemblyFilename = testAssembly.Location
            };

            var reporters = GetAvailableRunnerReporters();
            var reporter = reporters.OfType<VerboseReporter>().Single();
            var discoveryOptions = TestFrameworkOptions.ForDiscovery(assembly.Configuration);
            var executionOptions = TestFrameworkOptions.ForExecution(assembly.Configuration);
            var logger = new ConsoleRunnerLogger(true, consoleLock);
            var reporterMessageHandler = MessageSinkWithTypesAdapter.Wrap(reporter.CreateMessageHandler(logger));
            var xUnitProjectAssembly = new XunitProjectAssembly();
            var filters = new XunitFilters();
            var completionMessages = new ConcurrentDictionary<string, ExecutionSummary>();
            var serialize = false;
            var assemblyElement = new XElement("assembly");
            var assemblyDisplayName = Path.GetFileNameWithoutExtension(assembly.AssemblyFilename);
            var diagnosticMessageSink = new DiagnosticMessageSink();
            var failSkips = false;
            var longRunningSeconds = TimeSpan.FromMinutes(10).TotalSeconds;

            using (var controller = new XunitFrontController(AppDomainSupport.IfAvailable, testAssembly.Location))
            using (var discoverySink = new TestDiscoverySink(() => cancel))
            {
                reporterMessageHandler.OnMessage(new TestAssemblyDiscoveryStarting(assembly, true, false, discoveryOptions));

                controller.Find(false, discoverySink, discoveryOptions);
                discoverySink.Finished.WaitOne();

                var testCasesDiscovered = discoverySink.TestCases.Count;
                var filteredTestCases = discoverySink.TestCases.Where(filters.Filter).ToList();
                var testCasesToRun = filteredTestCases.Count;

                reporterMessageHandler.OnMessage(new TestAssemblyDiscoveryFinished(assembly, discoveryOptions, testCasesDiscovered, testCasesToRun));

                // Run the filtered tests
                if (testCasesToRun == 0)
                {
                    completionMessages.TryAdd(Path.GetFileName(assembly.AssemblyFilename), new ExecutionSummary());
                }
                else
                {
                    if (serialize)
                    {
                        filteredTestCases = filteredTestCases.Select(controller.Serialize).Select(controller.Deserialize).ToList();
                    }

                    reporterMessageHandler.OnMessage(new TestAssemblyExecutionStarting(assembly, executionOptions));

                    var resultsOptions = new ExecutionSinkOptions
                    {
                        AssemblyElement = assemblyElement,
                        CancelThunk = () => cancel,
                        FinishedCallback = summary => completionMessages.TryAdd(assemblyDisplayName, summary),
                        DiagnosticMessageSink = diagnosticMessageSink,
                        FailSkips = failSkips,
                        LongRunningTestTime = TimeSpan.FromSeconds(longRunningSeconds),
                    };
                    var resultsSink = new ExecutionSink(reporterMessageHandler, resultsOptions);

                    controller.RunTests(filteredTestCases, resultsSink, executionOptions);
                    resultsSink.Finished.WaitOne();

                    reporterMessageHandler.OnMessage(new TestAssemblyExecutionFinished(assembly, executionOptions, resultsSink.ExecutionSummary));

                    if ((resultsSink.ExecutionSummary.Failed != 0 || resultsSink.ExecutionSummary.Errors != 0) && executionOptions.GetStopOnTestFailOrDefault())
                    {
                        Console.WriteLine("Canceling due to test failure...");
                        cancel = true;
                    }
                }
            }

            return typeof(AssemblyRunner).CreateDisposable(() =>
            {

            });
        }

        private static List<IRunnerReporter> GetAvailableRunnerReporters()
        {
            var result = RunnerReporterUtility.GetAvailableRunnerReporters(Path.GetDirectoryName(typeof(XunitFrontController).Assembly.Location), out var messages);

            return result;
        }
    }
}
