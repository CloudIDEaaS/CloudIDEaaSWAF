using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils.Wrappers.Implementations
{
    public class Socket : System.Net.Sockets.Socket, ISocket
    {
        public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.Accept()
        {
            return new SocketWrapped(base.Accept());
        }

        bool Utils.Wrappers.Interfaces.ISocket.AcceptAsync(SocketAsyncEventArgs e)
        {

            return base.AcceptAsync(e);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(System.AsyncCallback callback, object state)
        {
            return base.BeginAccept(callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(int receiveSize, System.AsyncCallback callback, object state)
        {
            return base.BeginAccept(receiveSize, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginAccept(Utils.Wrappers.Interfaces.ISocket acceptSocket, int receiveSize, System.AsyncCallback callback, object state)
        {
            return base.BeginAccept((Utils.Wrappers.Implementations.Socket) acceptSocket, receiveSize, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginConnect(remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.IPAddress address, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(address, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(System.Net.IPAddress[] addresses, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(addresses, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginConnect(string host, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(host, port, requestCallback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginDisconnect(bool reuseSocket, System.AsyncCallback callback, object state)
        {
            return base.BeginDisconnect(reuseSocket, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendFile(string fileName, System.AsyncCallback callback, object state)
        {
            return base.BeginSendFile(fileName, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags, System.AsyncCallback callback, object state)
        {
            return base.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);
        }

        System.IAsyncResult Utils.Wrappers.Interfaces.ISocket.BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);
        }

        void Utils.Wrappers.Interfaces.ISocket.Bind(System.Net.EndPoint localEP)
        {
            base.Bind(localEP);
        }

        void Utils.Wrappers.Interfaces.ISocket.Close()
        {
            base.Close();
        }

        void Utils.Wrappers.Interfaces.ISocket.Close(int timeout)
        {
            base.Close(timeout);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.EndPoint remoteEP)
        {
            base.Connect(remoteEP);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.IPAddress address, int port)
        {
            base.Connect(address, port);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(System.Net.IPAddress[] addresses, int port)
        {
            base.Connect(addresses, port);
        }

        void Utils.Wrappers.Interfaces.ISocket.Connect(string host, int port)
        {
            base.Connect(host, port);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ConnectAsync(SocketAsyncEventArgs e)
        {
            return base.ConnectAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.Disconnect(bool reuseSocket)
        {
            base.Disconnect(reuseSocket);
        }

        bool Utils.Wrappers.Interfaces.ISocket.DisconnectAsync(SocketAsyncEventArgs e)
        {
            return base.DisconnectAsync(e);
        }

        SocketInformation Utils.Wrappers.Interfaces.ISocket.DuplicateAndClose(int targetProcessId)
        {
            return base.DuplicateAndClose(targetProcessId);
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(out byte[] buffer, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(out buffer, asyncResult));
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(out byte[] buffer, out int bytesTransferred, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(out buffer, out bytesTransferred, asyncResult));
        }

        Utils.Wrappers.Interfaces.ISocket Utils.Wrappers.Interfaces.ISocket.EndAccept(System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(asyncResult));
        }

        void Utils.Wrappers.Interfaces.ISocket.EndConnect(System.IAsyncResult asyncResult)
        {
            base.EndConnect(asyncResult);
        }

        void Utils.Wrappers.Interfaces.ISocket.EndDisconnect(System.IAsyncResult asyncResult)
        {
            base.EndDisconnect(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceive(System.IAsyncResult asyncResult)
        {
            return base.EndReceive(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceive(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return base.EndReceive(asyncResult, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceiveFrom(System.IAsyncResult asyncResult, ref System.Net.EndPoint endPoint)
        {
            return base.EndReceiveFrom(asyncResult, ref endPoint);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndReceiveMessageFrom(System.IAsyncResult asyncResult, ref SocketFlags socketFlags, ref System.Net.EndPoint endPoint, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return base.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSend(System.IAsyncResult asyncResult)
        {
            return base.EndSend(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSend(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return base.EndSend(asyncResult, out errorCode);
        }

        void Utils.Wrappers.Interfaces.ISocket.EndSendFile(System.IAsyncResult asyncResult)
        {
            base.EndSendFile(asyncResult);
        }

        int Utils.Wrappers.Interfaces.ISocket.EndSendTo(System.IAsyncResult asyncResult)
        {
            return base.EndSendTo(asyncResult);
        }

        object Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return base.GetSocketOption(optionLevel, optionName);
        }

        void Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            base.GetSocketOption(optionLevel, optionName, optionValue);
        }

        byte[] Utils.Wrappers.Interfaces.ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return base.GetSocketOption(optionLevel, optionName, optionLength);
        }

        int Utils.Wrappers.Interfaces.ISocket.IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return base.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        int Utils.Wrappers.Interfaces.ISocket.IOControl(System.Net.Sockets.IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return base.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.Listen(int backlog)
        {
            base.Listen(backlog);
        }

        bool Utils.Wrappers.Interfaces.ISocket.Poll(int microSeconds, System.Net.Sockets.SelectMode mode)
        {
            return base.Poll(microSeconds, mode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer)
        {
            return base.Receive(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return base.Receive(buffer, offset, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffer, offset, size, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return base.Receive(buffer, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Receive(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return base.Receive(buffers);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return base.Receive(buffers, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffers, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer)
        {
            return base.Receive(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags)
        {
            return base.Receive(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffer, socketFlags, out errorCode);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, ref remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, socketFlags, ref remoteEP);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveFromAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveFromAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return base.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEP, out ipPacketInformation);
        }

        bool Utils.Wrappers.Interfaces.ISocket.ReceiveMessageFromAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveMessageFromAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer)
        {
            return base.Send(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return base.Send(buffer, offset, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffer, offset, size, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return base.Send(buffer, size, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return base.Send(buffers);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return base.Send(buffers, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffers, socketFlags, out errorCode);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer)
        {
            return base.Send(buffer);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }

        int Utils.Wrappers.Interfaces.ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffer, socketFlags, out errorCode);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendAsync(SocketAsyncEventArgs e)
        {
            return base.SendAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.SendFile(string fileName)
        {
            base.SendFile(fileName);
        }

        void Utils.Wrappers.Interfaces.ISocket.SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags)
        {
            base.SendFile(fileName, preBuffer, postBuffer, flags);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendPacketsAsync(SocketAsyncEventArgs e)
        {
            return base.SendPacketsAsync(e);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, offset, size, socketFlags, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, size, socketFlags, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, remoteEP);
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTo(byte[] buffer, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, socketFlags, remoteEP);
        }

        bool Utils.Wrappers.Interfaces.ISocket.SendToAsync(SocketAsyncEventArgs e)
        {
            return base.SendToAsync(e);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetIPProtectionLevel(System.Net.Sockets.IPProtectionLevel level)
        {
            base.SetIPProtectionLevel(level);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void Utils.Wrappers.Interfaces.ISocket.Shutdown(SocketShutdown how)
        {
            base.Shutdown(how);
        }

        System.Net.Sockets.AddressFamily Utils.Wrappers.Interfaces.ISocket.AddressFamily
        {
            get
            {
                return base.AddressFamily;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.Available
        {
            get
            {
                return base.Available;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.Blocking
        {
            get
            {
                return base.Blocking;
            }

            set
            {
                base.Blocking = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.Connected
        {
            get
            {
                return base.Connected;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.DontFragment
        {
            get
            {
                return base.DontFragment;
            }

            set
            {
                base.DontFragment = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.DualMode
        {
            get
            {
                return base.DualMode;
            }

            set
            {
                base.DualMode = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.EnableBroadcast
        {
            get
            {
                return base.EnableBroadcast;
            }

            set
            {
                base.EnableBroadcast = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.ExclusiveAddressUse
        {
            get
            {
                return base.ExclusiveAddressUse;
            }

            set
            {
                base.ExclusiveAddressUse = value;
            }
        }

        System.IntPtr Utils.Wrappers.Interfaces.ISocket.Handle
        {
            get
            {
                return base.Handle;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.IsBound
        {
            get
            {
                return base.IsBound;
            }
        }

        System.Net.Sockets.LingerOption Utils.Wrappers.Interfaces.ISocket.LingerState
        {
            get
            {
                return base.LingerState;
            }

            set
            {
                base.LingerState = value;
            }
        }

        System.Net.EndPoint Utils.Wrappers.Interfaces.ISocket.LocalEndPoint
        {
            get
            {
                return base.LocalEndPoint;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.MulticastLoopback
        {
            get
            {
                return base.MulticastLoopback;
            }

            set
            {
                base.MulticastLoopback = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.NoDelay
        {
            get
            {
                return base.NoDelay;
            }

            set
            {
                base.NoDelay = value;
            }
        }

        System.Net.Sockets.ProtocolType Utils.Wrappers.Interfaces.ISocket.ProtocolType
        {
            get
            {
                return base.ProtocolType;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveBufferSize
        {
            get
            {
                return base.ReceiveBufferSize;
            }

            set
            {
                base.ReceiveBufferSize = value;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.ReceiveTimeout
        {
            get
            {
                return base.ReceiveTimeout;
            }

            set
            {
                base.ReceiveTimeout = value;
            }
        }

        System.Net.EndPoint Utils.Wrappers.Interfaces.ISocket.RemoteEndPoint
        {
            get
            {
                return base.RemoteEndPoint;
            }
        }

        System.Net.Sockets.SafeSocketHandle Utils.Wrappers.Interfaces.ISocket.SafeHandle
        {
            get
            {
                return base.SafeHandle;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.SendBufferSize
        {
            get
            {
                return base.SendBufferSize;
            }

            set
            {
                base.SendBufferSize = value;
            }
        }

        int Utils.Wrappers.Interfaces.ISocket.SendTimeout
        {
            get
            {
                return base.SendTimeout;
            }

            set
            {
                base.SendTimeout = value;
            }
        }

        SocketType Utils.Wrappers.Interfaces.ISocket.SocketType
        {
            get
            {
                return base.SocketType;
            }
        }

        short Utils.Wrappers.Interfaces.ISocket.Ttl
        {
            get
            {
                return base.Ttl;
            }

            set
            {
                base.Ttl = value;
            }
        }

        bool Utils.Wrappers.Interfaces.ISocket.UseOnlyOverlappedIO
        {
            get
            {
                return base.UseOnlyOverlappedIO;
            }

            set
            {
                base.UseOnlyOverlappedIO = value;
            }
        }
    }
}
