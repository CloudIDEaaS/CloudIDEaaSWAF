using DocumentFormat.OpenXml.Wordprocessing;
using EnvDTE80;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Utils.Kestrel
{
    public class KestrelConnectionHandler : ConnectionHandler
    {
        public KestrelOptionsInfo KestrelOptionsInfo { get; }
        public CsrDetails CsrDetails { get; }
        public ConnectionDelegate Next { get; set; }

        public KestrelConnectionHandler(KestrelOptionsInfo kestrelOptionsInfo, CsrDetails csrDetails)
        {
            KestrelOptionsInfo = kestrelOptionsInfo;
            CsrDetails = csrDetails;
        }

        public async override Task OnConnectedAsync(ConnectionContext context)
        {
            var contextTypeName = context.GetType().FullName;

            switch (contextTypeName)
            {
                case "Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.Internal.SocketConnection":
                    {
                        var connectionId = context.ConnectionId;
                        var localEndpoint = context.GetPropertyValue<IPEndPoint>("LocalEndPoint");
                        var remoteEndpoint = context.GetPropertyValue<IPEndPoint>("RemoteEndPoint");

                        Console.WriteLine($"Connection {connectionId} made from {remoteEndpoint} to {localEndpoint}");

                        lock (KestrelOptionsInfo)
                        {
                            KestrelOptionsInfo.ConnectionIds.AddToDictionaryList(remoteEndpoint.Address, context.ConnectionId);
                        }
                    }
                    break;
                default:
                    DebugUtils.Break();
                    break;
            }

            await Next(context);
        }
    }
}