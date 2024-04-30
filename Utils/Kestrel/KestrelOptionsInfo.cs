using CERTENROLLLib;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace Utils.Kestrel
{
    public class KestrelOptionsInfo
    {
        public KestrelServerOptions Options { get; }
        public Thread ListenThread { get; set; }
        public int Port { get; }
        public Task BindingTask { get; internal set; }
        public FixedDictionary<IPAddress, List<string>> ConnectionIds { get; private set; }

        public KestrelOptionsInfo(KestrelServerOptions options, int port)
        {
            ConnectionIds = new FixedDictionary<IPAddress, List<string>>(1024);

            Options = options;
            Port = port;
        }
    }
}