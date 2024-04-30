using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Utils.Wrappers.Interfaces
{
    public interface ISocket : IDisposable
    {
        AddressFamily AddressFamily { get; }
        int Available { get; }
        bool Blocking { get; set; }
        bool Connected { get; }
        bool DontFragment { get; set; }
        bool DualMode { get; set; }
        bool EnableBroadcast { get; set; }
        bool ExclusiveAddressUse { get; set; }
        IntPtr Handle { get; }
        bool IsBound { get; }
        EndPoint LocalEndPoint { get; }
        bool MulticastLoopback { get; set; }
        short Ttl { get; set; }
        ProtocolType ProtocolType { get; }
        int ReceiveBufferSize { get; set; }
        int ReceiveTimeout { get; set; }
        EndPoint RemoteEndPoint { get; }
        SafeSocketHandle SafeHandle { get; }
        int SendBufferSize { get; set; }
        int SendTimeout { get; set; }
        SocketType SocketType { get; }
        LingerOption LingerState { get; set; }
        bool NoDelay { get; set; }
        bool UseOnlyOverlappedIO { get; set; }
        Utils.Wrappers.Interfaces.ISocket Accept();
        bool AcceptAsync(SocketAsyncEventArgs e);
        IAsyncResult BeginAccept(AsyncCallback callback, object state);
        IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state);
        IAsyncResult BeginAccept(Utils.Wrappers.Interfaces.ISocket acceptSocket, int receiveSize, AsyncCallback callback, object state);
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);
        IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);
        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);
        IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);
        IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);
        IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);
        IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);
        IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state);
        IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state);
        void Bind(EndPoint localEP);
        void Close(int timeout);
        void Close();
        void Connect(IPAddress address, int port);
        void Connect(IPAddress[] addresses, int port);
        void Connect(string host, int port);
        void Connect(EndPoint remoteEP);
        bool ConnectAsync(SocketAsyncEventArgs e);
        void Disconnect(bool reuseSocket);
        bool DisconnectAsync(SocketAsyncEventArgs e);
        SocketInformation DuplicateAndClose(int targetProcessId);
        Utils.Wrappers.Interfaces.ISocket EndAccept(out byte[] buffer, IAsyncResult asyncResult);
        Utils.Wrappers.Interfaces.ISocket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
        Utils.Wrappers.Interfaces.ISocket EndAccept(IAsyncResult asyncResult);
        void EndConnect(IAsyncResult asyncResult);
        void EndDisconnect(IAsyncResult asyncResult);
        int EndReceive(IAsyncResult asyncResult);
        int EndReceive(IAsyncResult asyncResult, out SocketError errorCode);
        int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint);
        int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation);
        int EndSend(IAsyncResult asyncResult);
        int EndSend(IAsyncResult asyncResult, out SocketError errorCode);
        void EndSendFile(IAsyncResult asyncResult);
        int EndSendTo(IAsyncResult asyncResult);
        object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName);
        void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength);
        int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        void Listen(int backlog);
        bool Poll(int microSeconds, SelectMode mode);
        int Receive(IList<ArraySegment<byte>> buffers);
        int Receive(Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode);
        int Receive(Span<byte> buffer, SocketFlags socketFlags);
        int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        int Receive(byte[] buffer, SocketFlags socketFlags);
        int Receive(Span<byte> buffer);
        int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        int Receive(byte[] buffer);
        int Receive(byte[] buffer, int size, SocketFlags socketFlags);
        bool ReceiveAsync(SocketAsyncEventArgs e);
        int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP);
        bool ReceiveFromAsync(SocketAsyncEventArgs e);
        int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation);
        bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags);
        int Send(byte[] buffer);
        int Send(ReadOnlySpan<byte> buffer);
        int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        int Send(byte[] buffer, SocketFlags socketFlags);
        int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode);
        int Send(byte[] buffer, int size, SocketFlags socketFlags);
        int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        int Send(IList<ArraySegment<byte>> buffers);
        int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        bool SendAsync(SocketAsyncEventArgs e);
        void SendFile(string fileName);
        void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);
        bool SendPacketsAsync(SocketAsyncEventArgs e);
        int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, EndPoint remoteEP);
        bool SendToAsync(SocketAsyncEventArgs e);
        void SetIPProtectionLevel(IPProtectionLevel level);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue);
        void Shutdown(SocketShutdown how);
    }
}