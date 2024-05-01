using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Utils.Wrappers.Interfaces;
using Utils.Wrappers.Implementations;

namespace Utils
{
    public delegate void CreateSocketEventHandler(object sender, CreateSocketEventArgs e);

    public class CreateSocketEventArgs : EventArgs
    {
        public ISocket Socket { get; set; }
        public AddressFamily AddressFamily { get; }
        public SocketType SocketType { get; }
        public ProtocolType ProtocolType { get; }

        public CreateSocketEventArgs(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            this.AddressFamily = addressFamily;
            this.SocketType = socketType;
            this.ProtocolType = protocolType;
        }
    }

    public class TestingSocketFactory : ISocketFactory
    {
        public static event CreateSocketEventHandler OnCreateSocket;

        public ISocket CreateSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            if (TestingSocketFactory.OnCreateSocket != null)
            {
                var eventArgs = new CreateSocketEventArgs(addressFamily, socketType, protocolType);

                TestingSocketFactory.OnCreateSocket(this, eventArgs);

                return eventArgs.Socket;
            }

            return new Wrappers.Implementations.Socket(addressFamily, socketType, protocolType);
        }
    }
}
