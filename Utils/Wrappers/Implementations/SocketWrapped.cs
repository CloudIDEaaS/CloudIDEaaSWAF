using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Utils.Wrappers.Implementations
{
    public class SocketWrapped : Socket, Utils.Wrappers.Interfaces.ISocket
    {
        internal System.Net.Sockets.Socket InternalSocket { get; }

        public SocketWrapped(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
            InternalSocket = new System.Net.Sockets.Socket(addressFamily, socketType, protocolType);
        }

        public SocketWrapped(System.Net.Sockets.Socket socket) : base(socket.AddressFamily, socket.SocketType, socket.ProtocolType)
        {
            this.InternalSocket = socket;
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.Accept()
        {
            return new SocketWrapped(InternalSocket.Accept());
        }

        bool Utils.Wrappers.Interfaces.ISocket.AcceptAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.AcceptAsync(e);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginAccept(callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(int receiveSize, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginAccept(receiveSize, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(Utils.Wrappers.Interfaces.ISocket acceptSocket, int receiveSize, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginAccept((Utils.Wrappers.Implementations.Socket)acceptSocket, receiveSize, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginConnect(remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.IPAddress address, int port, System.AsyncCallback requestCallback, object state)
        {
            return InternalSocket.BeginConnect(address, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.IPAddress[] addresses, int port, System.AsyncCallback requestCallback, object state)
        {
            return InternalSocket.BeginConnect(addresses, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(string host, int port, System.AsyncCallback requestCallback, object state)
        {
            return InternalSocket.BeginConnect(host, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginDisconnect(bool reuseSocket, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginDisconnect(reuseSocket, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceive(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSend(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSend(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendFile(string fileName, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSendFile(fileName, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return InternalSocket.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);
        }

        void Utils.Wrappers.Interfaces.ISocket.Bind(System.Net.EndPoint localEP)
        {
            InternalSocket.Bind(localEP);
        }

        void Utils.Wrappers.Interfaces.ISocket.Close()
        {
            InternalSocket.Close();
        }

        void Utils.Wrappers.Interfaces.ISocket.Close(int timeout)
        {
            InternalSocket.Close(timeout);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.EndPoint remoteEP)
        {
            InternalSocket.Connect(remoteEP);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.IPAddress address, int port)
        {
            InternalSocket.Connect(address, port);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.IPAddress[] addresses, int port)
        {
            InternalSocket.Connect(addresses, port);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(string host, int port)
        {
            InternalSocket.Connect(host, port);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ConnectAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.ConnectAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.Disconnect(bool reuseSocket)
        {
            InternalSocket.Disconnect(reuseSocket);
        }

        bool Utils.Wrappers.Interfaces.ISocket.DisconnectAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.DisconnectAsync(e);
        }

        SocketInformation Utils.Wrappers.Interfaces.ISocket.DuplicateAndClose(int targetProcessId)
        {
            return InternalSocket.DuplicateAndClose(targetProcessId);
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(out byte[] buffer, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(InternalSocket.EndAccept(out buffer, asyncResult));
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(out byte[] buffer, out int bytesTransferred, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(InternalSocket.EndAccept(out buffer, out bytesTransferred, asyncResult));
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(InternalSocket.EndAccept(asyncResult));
        }

        void Utils.Wrappers.Interfaces.ISocket.EndConnect(System.IAsyncResult asyncResult)
        {
            InternalSocket.EndConnect(asyncResult);
        }

        void Utils.Wrappers.Interfaces.ISocket.EndDisconnect(System.IAsyncResult asyncResult)
        {
            InternalSocket.EndDisconnect(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceive(System.IAsyncResult asyncResult)
        {
            return InternalSocket.EndReceive(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceive(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return InternalSocket.EndReceive(asyncResult, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceiveFrom(System.IAsyncResult asyncResult, ref System.Net.EndPoint endPoint)
        {
            return InternalSocket.EndReceiveFrom(asyncResult, ref endPoint);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceiveMessageFrom(System.IAsyncResult asyncResult, ref SocketFlags socketFlags, ref System.Net.EndPoint endPoint, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return InternalSocket.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSend(System.IAsyncResult asyncResult)
        {
            return InternalSocket.EndSend(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSend(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return InternalSocket.EndSend(asyncResult, out errorCode);
        }

        void Utils.Wrappers.Interfaces.ISocket.EndSendFile(System.IAsyncResult asyncResult)
        {
            InternalSocket.EndSendFile(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSendTo(System.IAsyncResult asyncResult)
        {
            return InternalSocket.EndSendTo(asyncResult);
        }

        object Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return InternalSocket.GetSocketOption(optionLevel, optionName);
        }

        void Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            InternalSocket.GetSocketOption(optionLevel, optionName, optionValue);
        }

        byte[] Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return InternalSocket.GetSocketOption(optionLevel, optionName, optionLength);
        }

        int Utils.Wrappers.Interfaces.ISocket.IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return InternalSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        int Utils.Wrappers.Interfaces.ISocket.IOControl(System.Net.Sockets.IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return InternalSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.Listen(int backlog)
        {
            InternalSocket.Listen(backlog);
        }

        bool Utils.Wrappers.Interfaces.ISocket.Poll(int microSeconds, System.Net.Sockets.SelectMode mode)
        {
            return InternalSocket.Poll(microSeconds, mode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer)
        {
            return InternalSocket.Receive(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return InternalSocket.Receive(buffer, offset, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Receive(buffer, offset, size, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return InternalSocket.Receive(buffer, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return InternalSocket.Receive(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return InternalSocket.Receive(buffers);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return InternalSocket.Receive(buffers, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Receive(buffers, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer)
        {
            return InternalSocket.Receive(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags)
        {
            return InternalSocket.Receive(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Receive(buffer, socketFlags, out errorCode);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.ReceiveAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return InternalSocket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return InternalSocket.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, ref System.Net.EndPoint remoteEP)
        {
            return InternalSocket.ReceiveFrom(buffer, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return InternalSocket.ReceiveFrom(buffer, socketFlags, ref remoteEP);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveFromAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.ReceiveFromAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return InternalSocket.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEP, out ipPacketInformation);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveMessageFromAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.ReceiveMessageFromAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer)
        {
            return InternalSocket.Send(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return InternalSocket.Send(buffer, offset, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Send(buffer, offset, size, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return InternalSocket.Send(buffer, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, SocketFlags socketFlags)
        {
            return InternalSocket.Send(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return InternalSocket.Send(buffers);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return InternalSocket.Send(buffers, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Send(buffers, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer)
        {
            return InternalSocket.Send(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags)
        {
            return InternalSocket.Send(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return InternalSocket.Send(buffer, socketFlags, out errorCode);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.SendAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.SendFile(string fileName)
        {
            InternalSocket.SendFile(fileName);
        }

        void Utils.Wrappers.Interfaces.ISocket.SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags)
        {
            InternalSocket.SendFile(fileName, preBuffer, postBuffer, flags);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendPacketsAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.SendPacketsAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return InternalSocket.SendTo(buffer, offset, size, socketFlags, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return InternalSocket.SendTo(buffer, size, socketFlags, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, System.Net.EndPoint remoteEP)
        {
            return InternalSocket.SendTo(buffer, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return InternalSocket.SendTo(buffer, socketFlags, remoteEP);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendToAsync(SocketAsyncEventArgs e)
        {
            return InternalSocket.SendToAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetIPProtectionLevel(System.Net.Sockets.IPProtectionLevel level)
        {
            InternalSocket.SetIPProtectionLevel(level);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
            InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.Shutdown(SocketShutdown how)
        {
            InternalSocket.Shutdown(how);
        }

        System.Net.Sockets.AddressFamily Utils.Wrappers.Interfaces.ISocket.AddressFamily
        {
            get
            {
                return InternalSocket.AddressFamily;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.Available
        {
            get
            {
                return InternalSocket.Available;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.Blocking
        {
            get
            {
                return InternalSocket.Blocking;
            }

            set
            {
                InternalSocket.Blocking = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.Connected
        {
            get
            {
                return InternalSocket.Connected;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.DontFragment
        {
            get
            {
                return InternalSocket.DontFragment;
            }

            set
            {
                InternalSocket.DontFragment = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.DualMode
        {
            get
            {
                return InternalSocket.DualMode;
            }

            set
            {
                InternalSocket.DualMode = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.EnableBroadcast
        {
            get
            {
                return InternalSocket.EnableBroadcast;
            }

            set
            {
                InternalSocket.EnableBroadcast = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.ExclusiveAddressUse
        {
            get
            {
                return InternalSocket.ExclusiveAddressUse;
            }

            set
            {
                InternalSocket.ExclusiveAddressUse = value;
            }
        }

        System.IntPtr Utils.Wrappers.Interfaces.ISocket.Handle
        {
            get
            {
                return InternalSocket.Handle;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.IsBound
        {
            get
            {
                return InternalSocket.IsBound;
            }
        }

        System.Net.Sockets.LingerOption Utils.Wrappers.Interfaces.ISocket.LingerState
        {
            get
            {
                return InternalSocket.LingerState;
            }

            set
            {
                InternalSocket.LingerState = value;
            }
        }

        System.Net.EndPoint Utils.Wrappers.Interfaces.ISocket.LocalEndPoint
        {
            get
            {
                return InternalSocket.LocalEndPoint;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.MulticastLoopback
        {
            get
            {
                return InternalSocket.MulticastLoopback;
            }

            set
            {
                InternalSocket.MulticastLoopback = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.NoDelay
        {
            get
            {
                return InternalSocket.NoDelay;
            }

            set
            {
                InternalSocket.NoDelay = value;
            }
        }

        System.Net.Sockets.ProtocolType Utils.Wrappers.Interfaces.ISocket.ProtocolType
        {
            get
            {
                return InternalSocket.ProtocolType;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveBufferSize
        {
            get
            {
                return InternalSocket.ReceiveBufferSize;
            }

            set
            {
                InternalSocket.ReceiveBufferSize = value;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveTimeout
        {
            get
            {
                return InternalSocket.ReceiveTimeout;
            }

            set
            {
                InternalSocket.ReceiveTimeout = value;
            }
        }

        System.Net.EndPoint Utils.Wrappers.Interfaces.ISocket.RemoteEndPoint
        {
            get
            {
                return InternalSocket.RemoteEndPoint;
            }
        }

        System.Net.Sockets.SafeSocketHandle Utils.Wrappers.Interfaces.ISocket.SafeHandle
        {
            get
            {
                return InternalSocket.SafeHandle;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.SendBufferSize
        {
            get
            {
                return InternalSocket.SendBufferSize;
            }

            set
            {
                InternalSocket.SendBufferSize = value;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTimeout
        {
            get
            {
                return InternalSocket.SendTimeout;
            }

            set
            {
                InternalSocket.SendTimeout = value;
            }
        }

        SocketType Utils.Wrappers.Interfaces.ISocket.SocketType
        {
            get
            {
                return InternalSocket.SocketType;
            }
        }

        short Utils.Wrappers.Interfaces.ISocket.Ttl
        {
            get
            {
                return InternalSocket.Ttl;
            }

            set
            {
                InternalSocket.Ttl = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.UseOnlyOverlappedIO
        {
            get
            {
                return InternalSocket.UseOnlyOverlappedIO;
            }

            set
            {
                InternalSocket.UseOnlyOverlappedIO = value;
            }
        }
    }
}
