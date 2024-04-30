using System;
using System.ComponentModel;

namespace MailSlot
{
    public class MailSlot : IDisposable
    {
        private SafeMailslotHandle _handle;
        public static MailSlot ForServer(string domain, string path, int readTimeoutMilliseconds)
        {
            return new MailSlot($@"\\{domain}\mailslot\{path}", true, readTimeoutMilliseconds);
        }

        public static MailSlot ForClient(string domain, string path)
        {
            return new MailSlot($@"\\{domain}\mailslot\{path}", false, 0);
        }

        private MailSlot(string name, bool server, int readTimeoutMilliseconds)
        {
            try
            {
                if (server)
                {
                    _handle = RawMailSlot.CreateMailSlot(name, 0, readTimeoutMilliseconds);
                }
                else
                {
                    _handle = RawMailSlot.CreateFile(name);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to create new mailslot with path {name}", ex);
            }
            
            if (_handle.IsInvalid)
            {
                throw new Exception($"Unable to create new mailslot with path {name}", new Win32Exception());
            }
        }

        public void Flush()
        {
            RawMailSlot.Flush(_handle);
        }

        private int? GetNextMessageSize()
        {
            return RawMailSlot.GetInfo(_handle);
        }

        public byte[] GetNextMessage()
        {
            var size = GetNextMessageSize();

            if (size == null)
            {
                return null;
            }

            return RawMailSlot.ReadBytes(_handle, size.Value);
        }
        public void SendMessage(byte[] msg)
        {
            RawMailSlot.WriteBytes(_handle, msg);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                if (_handle != null)
                {
                    _handle.Close();
                    _handle = null;
                }
                disposedValue = true;
            }
        }

         ~MailSlot()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
