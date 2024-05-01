using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utils.Wrappers.Implementations;
using Utils.Wrappers.Interfaces;

namespace Utils
{
    public class TestingSocket : System.Net.Sockets.Socket, ISocket
    {
        public TestingSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
        }

        ISocket ISocket.Accept()
        {
            return new SocketWrapped(base.Accept());
        }

        bool ISocket.AcceptAsync(SocketAsyncEventArgs e)
        {
            return base.AcceptAsync(e);
        }

        System.IAsyncResult ISocket.BeginAccept(System.AsyncCallback callback, object state)
        {
            return base.BeginAccept(callback, state);
        }

        System.IAsyncResult ISocket.BeginAccept(int receiveSize, System.AsyncCallback callback, object state)
        {
            return base.BeginAccept(receiveSize, callback, state);
        }

        System.IAsyncResult ISocket.BeginAccept(ISocket acceptSocket, int receiveSize, System.AsyncCallback callback, object state)
        {
            return base.BeginAccept((System.Net.Sockets.Socket) acceptSocket, receiveSize, callback, state);
        }

        System.IAsyncResult ISocket.BeginConnect(System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginConnect(remoteEP, callback, state);
        }

        System.IAsyncResult ISocket.BeginConnect(System.Net.IPAddress address, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(address, port, requestCallback, state);
        }

        System.IAsyncResult ISocket.BeginConnect(System.Net.IPAddress[] addresses, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(addresses, port, requestCallback, state);
        }

        System.IAsyncResult ISocket.BeginConnect(string host, int port, System.AsyncCallback requestCallback, object state)
        {
            return base.BeginConnect(host, port, requestCallback, state);
        }

        System.IAsyncResult ISocket.BeginDisconnect(bool reuseSocket, System.AsyncCallback callback, object state)
        {
            return base.BeginDisconnect(reuseSocket, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult ISocket.BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        System.IAsyncResult ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        System.IAsyncResult ISocket.BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffers, socketFlags, callback, state);
        }

        System.IAsyncResult ISocket.BeginSend(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, System.AsyncCallback callback, object state)
        {
            return base.BeginSend(buffers, socketFlags, out errorCode, callback, state);
        }

        System.IAsyncResult ISocket.BeginSendFile(string fileName, System.AsyncCallback callback, object state)
        {
            return base.BeginSendFile(fileName, callback, state);
        }

        System.IAsyncResult ISocket.BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags, System.AsyncCallback callback, object state)
        {
            return base.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);
        }

        System.IAsyncResult ISocket.BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP, System.AsyncCallback callback, object state)
        {
            return base.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);
        }

        void ISocket.Bind(System.Net.EndPoint localEP)
        {
            base.Bind(localEP);
        }

        void ISocket.Close()
        {
            base.Close();
        }

        void ISocket.Close(int timeout)
        {
            base.Close(timeout);
        }

        void ISocket.Connect(System.Net.EndPoint remoteEP)
        {
            base.Connect(remoteEP);
        }

        void ISocket.Connect(System.Net.IPAddress address, int port)
        {
            base.Connect(address, port);
        }

        void ISocket.Connect(System.Net.IPAddress[] addresses, int port)
        {
            base.Connect(addresses, port);
        }

        void ISocket.Connect(string host, int port)
        {
            base.Connect(host, port);
        }

        bool ISocket.ConnectAsync(SocketAsyncEventArgs e)
        {
            return base.ConnectAsync(e);
        }

        void ISocket.Disconnect(bool reuseSocket)
        {
            base.Disconnect(reuseSocket);
        }

        bool ISocket.DisconnectAsync(SocketAsyncEventArgs e)
        {
            return base.DisconnectAsync(e);
        }

        SocketInformation ISocket.DuplicateAndClose(int targetProcessId)
        {
            return base.DuplicateAndClose(targetProcessId);
        }

        ISocket ISocket.EndAccept(out byte[] buffer, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(out buffer, asyncResult));
        }

        ISocket ISocket.EndAccept(out byte[] buffer, out int bytesTransferred, System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(out buffer, out bytesTransferred, asyncResult));
        }

        ISocket ISocket.EndAccept(System.IAsyncResult asyncResult)
        {
            return new SocketWrapped(base.EndAccept(asyncResult));
        }

        void ISocket.EndConnect(System.IAsyncResult asyncResult)
        {
            base.EndConnect(asyncResult);
        }

        void ISocket.EndDisconnect(System.IAsyncResult asyncResult)
        {
            base.EndDisconnect(asyncResult);
        }

        int ISocket.EndReceive(System.IAsyncResult asyncResult)
        {
            return base.EndReceive(asyncResult);
        }

        int ISocket.EndReceive(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return base.EndReceive(asyncResult, out errorCode);
        }

        int ISocket.EndReceiveFrom(System.IAsyncResult asyncResult, ref System.Net.EndPoint endPoint)
        {
            return base.EndReceiveFrom(asyncResult, ref endPoint);
        }

        int ISocket.EndReceiveMessageFrom(System.IAsyncResult asyncResult, ref SocketFlags socketFlags, ref System.Net.EndPoint endPoint, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return base.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);
        }

        int ISocket.EndSend(System.IAsyncResult asyncResult)
        {
            return base.EndSend(asyncResult);
        }

        int ISocket.EndSend(System.IAsyncResult asyncResult, out SocketError errorCode)
        {
            return base.EndSend(asyncResult, out errorCode);
        }

        void ISocket.EndSendFile(System.IAsyncResult asyncResult)
        {
            base.EndSendFile(asyncResult);
        }

        int ISocket.EndSendTo(System.IAsyncResult asyncResult)
        {
            return base.EndSendTo(asyncResult);
        }

        object ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return base.GetSocketOption(optionLevel, optionName);
        }

        void ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            base.GetSocketOption(optionLevel, optionName, optionValue);
        }

        byte[] ISocket.GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return base.GetSocketOption(optionLevel, optionName, optionLength);
        }

        int ISocket.IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return base.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        int ISocket.IOControl(System.Net.Sockets.IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return base.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        void ISocket.Listen(int backlog)
        {
            base.Listen(backlog);
        }

        bool ISocket.Poll(int microSeconds, System.Net.Sockets.SelectMode mode)
        {
            return base.Poll(microSeconds, mode);
        }

        int ISocket.Receive(byte[] buffer)
        {
            return base.Receive(buffer);
        }

        int ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return base.Receive(buffer, offset, size, socketFlags);
        }

        int ISocket.Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffer, offset, size, socketFlags, out errorCode);
        }

        int ISocket.Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return base.Receive(buffer, size, socketFlags);
        }

        int ISocket.Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Receive(buffer, socketFlags);
        }

        int ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return base.Receive(buffers);
        }

        int ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return base.Receive(buffers, socketFlags);
        }

        int ISocket.Receive(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffers, socketFlags, out errorCode);
        }

        int ISocket.Receive(System.Span<byte> buffer)
        {
            return base.Receive(buffer);
        }

        int ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags)
        {
            return base.Receive(buffer, socketFlags);
        }

        int ISocket.Receive(System.Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Receive(buffer, socketFlags, out errorCode);
        }

        bool ISocket.ReceiveAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveAsync(e);
        }

        int ISocket.ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);
        }

        int ISocket.ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);
        }

        int ISocket.ReceiveFrom(byte[] buffer, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, ref remoteEP);
        }

        int ISocket.ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref System.Net.EndPoint remoteEP)
        {
            return base.ReceiveFrom(buffer, socketFlags, ref remoteEP);
        }

        bool ISocket.ReceiveFromAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveFromAsync(e);
        }

        int ISocket.ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref System.Net.EndPoint remoteEP, out System.Net.Sockets.IPPacketInformation ipPacketInformation)
        {
            return base.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEP, out ipPacketInformation);
        }

        bool ISocket.ReceiveMessageFromAsync(SocketAsyncEventArgs e)
        {
            return base.ReceiveMessageFromAsync(e);
        }

        int ISocket.Send(byte[] buffer)
        {
            return base.Send(buffer);
        }

        int ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return base.Send(buffer, offset, size, socketFlags);
        }

        int ISocket.Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffer, offset, size, socketFlags, out errorCode);
        }

        int ISocket.Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return base.Send(buffer, size, socketFlags);
        }

        int ISocket.Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }

        int ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers)
        {
            return base.Send(buffers);
        }

        int ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return base.Send(buffers, socketFlags);
        }

        int ISocket.Send(System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffers, socketFlags, out errorCode);
        }

        int ISocket.Send(System.ReadOnlySpan<byte> buffer)
        {
            return base.Send(buffer);
        }

        int ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }

        int ISocket.Send(System.ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
        {
            return base.Send(buffer, socketFlags, out errorCode);
        }

        bool ISocket.SendAsync(SocketAsyncEventArgs e)
        {
            return base.SendAsync(e);
        }

        void ISocket.SendFile(string fileName)
        {
            base.SendFile(fileName);
        }

        void ISocket.SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, System.Net.Sockets.TransmitFileOptions flags)
        {
            base.SendFile(fileName, preBuffer, postBuffer, flags);
        }

        bool ISocket.SendPacketsAsync(SocketAsyncEventArgs e)
        {
            return base.SendPacketsAsync(e);
        }

        int ISocket.SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, offset, size, socketFlags, remoteEP);
        }

        int ISocket.SendTo(byte[] buffer, int size, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, size, socketFlags, remoteEP);
        }

        int ISocket.SendTo(byte[] buffer, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, remoteEP);
        }

        int ISocket.SendTo(byte[] buffer, SocketFlags socketFlags, System.Net.EndPoint remoteEP)
        {
            return base.SendTo(buffer, socketFlags, remoteEP);
        }

        bool ISocket.SendToAsync(SocketAsyncEventArgs e)
        {
            return base.SendToAsync(e);
        }

        void ISocket.SetIPProtectionLevel(System.Net.Sockets.IPProtectionLevel level)
        {
            base.SetIPProtectionLevel(level);
        }

        void ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void ISocket.SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            base.SetSocketOption(optionLevel, optionName, optionValue);
        }

        void ISocket.Shutdown(SocketShutdown how)
        {
            base.Shutdown(how);
        }

        System.Net.Sockets.AddressFamily ISocket.AddressFamily
        {
            get
            {
                return base.AddressFamily;
            }
        }

        int ISocket.Available
        {
            get
            {
                return base.Available;
            }
        }

        bool ISocket.Blocking
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

        bool ISocket.Connected
        {
            get
            {
                return base.Connected;
            }
        }

        bool ISocket.DontFragment
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

        bool ISocket.DualMode
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

        bool ISocket.EnableBroadcast
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

        bool ISocket.ExclusiveAddressUse
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

        System.IntPtr ISocket.Handle
        {
            get
            {
                return base.Handle;
            }
        }

        bool ISocket.IsBound
        {
            get
            {
                return base.IsBound;
            }
        }

        System.Net.Sockets.LingerOption ISocket.LingerState
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

        System.Net.EndPoint ISocket.LocalEndPoint
        {
            get
            {
                return base.LocalEndPoint;
            }
        }

        bool ISocket.MulticastLoopback
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

        bool ISocket.NoDelay
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

        System.Net.Sockets.ProtocolType ISocket.ProtocolType
        {
            get
            {
                return base.ProtocolType;
            }
        }

        int ISocket.ReceiveBufferSize
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

        int ISocket.ReceiveTimeout
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

        System.Net.EndPoint ISocket.RemoteEndPoint
        {
            get
            {
                return base.RemoteEndPoint;
            }
        }

        System.Net.Sockets.SafeSocketHandle ISocket.SafeHandle
        {
            get
            {
                return base.SafeHandle;
            }
        }

        int ISocket.SendBufferSize
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

        int ISocket.SendTimeout
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

        SocketType ISocket.SocketType
        {
            get
            {
                return base.SocketType;
            }
        }

        short ISocket.Ttl
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

        bool ISocket.UseOnlyOverlappedIO
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
