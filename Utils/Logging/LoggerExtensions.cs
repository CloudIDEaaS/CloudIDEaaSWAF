using Castle.Core.Internal;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Expressions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Utils.Hierarchies;
using Utils.Logging;

namespace Utils
{
    public static class LoggerExtensions
    {
        private static IManagedLockObject lockObject;
        private static List<LoggerRelayEventSink> eventSinks;
        public static string SOURCE_CONTEXT_OUTPUT_TEMPLATE = "{Timestamp} [{Level}: {SourceContext}] {Message}{NewLine}{Exception}";

        static LoggerExtensions()
        {
            lockObject = LockManager.CreateObject();
            eventSinks = new List<LoggerRelayEventSink>();
        }

        public static LoggerConfiguration ExpressionLogsFromConfig(this LoggerSinkConfiguration loggerSinkConfiguration, IConfigurationRoot configuration, string logRootPath)
        {
            var loggerConfiguration = loggerSinkConfiguration.GetPrivateFieldValue<LoggerConfiguration>("_loggerConfiguration");
            var expressionLogs = configuration.GetSection("ExpressionLogs");

            foreach (var log in expressionLogs.GetChildren())
            {
                var subFolder = log["Subfolder"];
                var expression = log["Expression"];
                var logPath = Path.Combine(logRootPath, "ByExpression", subFolder);
                var directory = new DirectoryInfo(logPath);

                if (!directory.Exists)
                {
                    directory.Create();
                }

                loggerConfiguration = loggerConfiguration
                    .WriteTo.Logger(l => l
                        .Filter.ByIncludingOnly(expression)
                        .WriteTo.RollingFile(Path.Combine(logPath, "{Date}.log"), outputTemplate: SOURCE_CONTEXT_OUTPUT_TEMPLATE));
            }

            return loggerConfiguration;
        }

        public static bool InCaptureStack(this Dictionary<string, LoggerRelayCaptureMethodAssembly> captureStack)
        {
            var stackFrames = typeof(LoggerExtensions).GetStack().Skip(3).ToList();

            return (stackFrames.Any(f =>
            {
                var method = f.GetMethod();
                var type = method.DeclaringType;
                var methodName = method.Name;
                var typeName = type.FullName;

                if (typeName.Contains("+"))
                {
                    typeName = typeName.RegexGet(@"^(?<typeName>.*?)\+", "typeName");
                }

                if (methodName.Contains("<"))
                {
                    methodName = methodName.RegexGet(@"\<(?<methodName>.*?)\>", "methodName");
                }

                if (method.DeclaringType != null)
                {
                    var assembly = type.Assembly;
                    var assemblyName = assembly.GetName().Name;

                    if (typeName == "HydraDevOpsController")
                    {
                    }

                    if (captureStack.ContainsKey(assemblyName))
                    {
                        var methodAssembly = captureStack[assemblyName];

                        if (methodAssembly.TypesLookup.ContainsKey(typeName))
                        {
                            var methodType = methodAssembly.TypesLookup[typeName];

                            return methodType.Methods.Contains(methodName);
                        }
                    }
                }

                return false;
            }));
        }

        public static LoggerConfiguration Relay(this LoggerSinkConfiguration sinkConfiguration, ILoggerRelay loggerRelay, IConfigurationSection serilogConfig, CancellationToken cancellationToken)
        {
            LoggerRelayEventSink eventSink;
            var comparer = new ObjectHashCodeComparer<IConfiguration>();
            var configurationTree = new ObjectTree<IConfiguration>(comparer);
            IConfigurationSection relayConfigurationSection;
            ObjectTreeItem<IConfiguration> treeItem;
            LinkedListNode<ObjectTreeItem<IConfiguration>> sibling;
            LoggerRelayEventSinkConfigArgs logRelayEventSinkConfigArgs = null;

            configurationTree.AddChild(serilogConfig);

            serilogConfig.GetDescendantsWithParent(s => s.GetChildren(), (parent, child) =>
            {
                var parentItem = configurationTree.FindTreeItem(parent);

                parentItem.AddChild(child);
            });

            relayConfigurationSection = configurationTree.GetDescendants().Select(d => d.InternalObject).OfType<IConfigurationSection>().SingleOrDefault(s => s.Path.RegexIsMatch(@"Serilog:WriteTo:(\d+:)?Name") && s.Value == "Relay");

            if (relayConfigurationSection == null)
            {
                throw new ConfigurationException("Logger relay requires a configuration section with a RootPath member");
            }
            else
            {
                treeItem = configurationTree.FindTreeItem(relayConfigurationSection);
                sibling = CompareExtensions.GetNonNull(treeItem.LinkedListNode.Next, treeItem.LinkedListNode.Previous);
                var key = ((IConfigurationSection)sibling.Value.InternalObject).Key;

                if (sibling != null && ((IConfigurationSection)sibling.Value.InternalObject).Key == "Args")
                {
                    logRelayEventSinkConfigArgs = sibling.Value.InternalObject.Get<LoggerRelayEventSinkConfigArgs>(key);
                }
                else
                {
                    throw new ConfigurationException("Logger relay requires a configuration section with a RootPath member");
                }

                loggerRelay.Initialize(logRelayEventSinkConfigArgs);
            }

            using (lockObject.Lock())
            {
                if (eventSinks.Any(s => s.Domain == loggerRelay.Domain))
                {
                    eventSink = eventSinks.Single(s => s.Domain == loggerRelay.Domain);
                }
                else
                {
                    eventSink = new LoggerRelayEventSink(loggerRelay, cancellationToken, logRelayEventSinkConfigArgs);

                    eventSinks.Add(eventSink);
                }
            }

            return sinkConfiguration.Sink(eventSink);
        }
    }
}
