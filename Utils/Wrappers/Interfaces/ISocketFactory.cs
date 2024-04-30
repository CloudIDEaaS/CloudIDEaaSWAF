using System.Net.Sockets;

namespace Utils.Wrappers.Interfaces
{
    public interface ISocketFactory
    {
        ISocket CreateSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType);
    }
}