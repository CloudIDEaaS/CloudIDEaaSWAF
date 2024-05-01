using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils
{
    public delegate void GetHostNameEventHandler(object sender, GetHostNameEventArgs e);
    public delegate void GetHostEntryEventHandler(object sender, GetHostEntryEventArgs e);

    public class GetHostNameEventArgs : EventArgs
    {
        public string HostName { get; set; }
        public Type RequestorType { get; }

        public GetHostNameEventArgs(Type requestorType)
        {
            this.RequestorType = requestorType;
        }
    }

    public class GetHostEntryEventArgs : EventArgs
    {
        public string HostName { get; }
        public Type RequestorType { get; }
        public IPHostEntry IPHostEntry { get; set; }

        public GetHostEntryEventArgs(Type requestorType, string hostName)
        {
            this.RequestorType = requestorType;
            this.HostName = hostName;
        }
    }

    public class TestingDnsProvider : IDnsProvider
    {
        public static event GetHostNameEventHandler OnGetHostName;
        public static event GetHostEntryEventHandler OnGetHostEntry;

        public IPHostEntry GetHostEntry<TRequestor>(string hostName)
        {
            var eventArgs = new GetHostEntryEventArgs(typeof(TRequestor), hostName);

            OnGetHostEntry(this, eventArgs);

            return eventArgs.IPHostEntry;
        }

        public string GetHostName<TRequestor>()
        {
            var eventArgs = new GetHostNameEventArgs(typeof(TRequestor));

            OnGetHostName(this, eventArgs);

            return eventArgs.HostName;
        }
    }
}
