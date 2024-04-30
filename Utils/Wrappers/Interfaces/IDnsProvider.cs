using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Utils.Wrappers.Interfaces
{
    public interface IDnsProvider
    {
        string GetHostName<TThis>();
        IPHostEntry GetHostEntry<T>(string hostName);
    }

    public class DnsProvider : IDnsProvider
    {
        string hostName;

        public IPHostEntry GetHostEntry<T>(string hostName)
        {
            return Dns.GetHostEntry(hostName);
        }

        public string GetHostName<TThis>()
        {
            hostName = Dns.GetHostName();

            return hostName;
        }
    }
}
