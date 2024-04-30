using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Utils.Hierarchies;
using System.Diagnostics.CodeAnalysis;

namespace Utils.Logging
{
    public static class TraceExtensions
    {
        private static IManagedLockObject lockObject;
        private static List<TraceLogEventSink> eventSinks;

        static TraceExtensions()
        {
            lockObject = LockManager.CreateObject();
            eventSinks = new List<TraceLogEventSink>();
        }

        public static LoggerConfiguration Trace(this LoggerSinkConfiguration sinkConfiguration, string ipAddress, int port, IConfigurationSection serilogConfig, CancellationToken cancellationToken)
        {
            TraceLogEventSink eventSink;
            var comparer = new ObjectHashCodeComparer<IConfiguration>();
            var configurationTree = new ObjectTree<IConfiguration>(comparer);
            IConfigurationSection traceConfigurationSection;
            ObjectTreeItem<IConfiguration> treeItem;
            LinkedListNode<ObjectTreeItem<IConfiguration>> sibling;
            TraceLogEventSinkConfigArgs traceLogEventSinkConfigArgs = null;
            string key;

            configurationTree.AddChild(serilogConfig);

            serilogConfig.GetDescendantsWithParent(s => s.GetChildren(), (parent, child) =>
            {
                var parentItem = configurationTree.FindTreeItem(parent);

                parentItem.AddChild(child);
            });

            traceConfigurationSection = configurationTree.GetDescendants().Select(d => d.InternalObject).OfType<IConfigurationSection>().SingleOrDefault(s => s.Path.RegexIsMatch(@"Serilog:WriteTo:(\d+:)?Name") && s.Value == "Trace");
            treeItem = configurationTree.FindTreeItem(traceConfigurationSection);
            sibling = CompareExtensions.GetNonNull(treeItem.LinkedListNode.Next, treeItem.LinkedListNode.Previous);

            key = ((IConfigurationSection)sibling.Value.InternalObject).Key;

            if (sibling != null && ((IConfigurationSection) sibling.Value.InternalObject).Key == "Args")
            {
                traceLogEventSinkConfigArgs = sibling.Value.InternalObject.Get<TraceLogEventSinkConfigArgs>(key);
            }

            using (lockObject.Lock())
            {
                if (eventSinks.Any(s => s.Address == ipAddress && s.Port == port))
                {
                    eventSink = eventSinks.Single(s => s.Address == ipAddress && s.Port == port);
                }
                else
                {
                    eventSink = new TraceLogEventSink(ipAddress, port, cancellationToken, traceLogEventSinkConfigArgs);

                    eventSinks.Add(eventSink);
                }
            }

            return sinkConfiguration.Sink(eventSink);
        }
     }
}
