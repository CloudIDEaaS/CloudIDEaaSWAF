using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.NamedPipes
{
    public class NamedPipeServer : BaseThreadedService
    {
        private NamedPipeServerStream pipeServer;
        private string pipeName;
        private PipeServerConnection connection;
        public event EventHandlerT<CommandPacket> OnCommand;
        public event EventHandlerT<PipeServerConnection> OnConnectionCreated;
        public event EventHandlerT<PipeServerConnection> OnConnectionMade;
        public event EventHandlerT<PipeServerConnection> OnDisconnect;
        public event EventHandlerT<Exception> OnError;

        public NamedPipeServer(string name)
        {
            pipeName = name;
        }

        public string PipeName
        {
            get
            {
                return this.LockReturn(() => pipeName);
            }
        }

        public override void DoWork(bool stopping)
        {
            NamedPipeServerStream pipeServer;
            PipeServerConnection connection;
            string name;

            using (this.Lock())
            {
                connection = this.connection;
                pipeServer = this.pipeServer;
                name = pipeName;
            }

            if (pipeServer == null)
            {
                try
                {
                    pipeServer = new NamedPipeServerStream(name, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                    connection = new PipeServerConnection(name, pipeServer, this);

                    OnConnectionCreated.Raise(this, connection);

                    connection.OnConnectionMade += Connection_OnConnectionMade;
                    connection.OnCommand += Connection_OnCommand;
                    connection.OnError += Connection_OnError;

                    connection.Listen();

                    using (this.Lock())
                    {
                        this.pipeServer = pipeServer;
                        this.connection = connection;
                    }
                }
                catch (Exception ex)
                {
                    OnError.Raise(this, new IOException(string.Format("Pipe creation error: {0}", ex.Message)));
                }
            }
            else if (connection != null && connection.ConnectionMade && !connection.IsConnected)
            {
                connection.OnConnectionMade -= Connection_OnConnectionMade;
                connection.OnCommand -= Connection_OnCommand;
                connection.OnError -= Connection_OnError;

                connection.Dispose();

                try
                {
                    pipeServer.Disconnect();
                }
                catch
                {
                }

                OnDisconnect.Raise(this, connection);
                pipeServer.Dispose();

                using (this.Lock())
                {
                    this.pipeServer = null;
                    this.connection = null;
                }
            }
        }

        public void Send(CommandPacket commandPacket)
        {
            PipeServerConnection connection;
            NamedPipeServerStream pipeServer;

            using (this.Lock())
            {
                connection = this.connection;
                pipeServer = this.pipeServer;
            }

            if (pipeServer != null && connection != null)
            {
                connection.Send(commandPacket);
            }
            else
            {
                OnError.Raise(this, new InvalidOperationException("No pipe server connection established"));
            }
        }

        private void Connection_OnConnectionMade(object sender, EventArgs<PipeServerConnection> e)
        {
            OnConnectionMade.Raise(sender, e.Value);
        }

        private void Connection_OnError(object sender, EventArgs<Exception> e)
        {
            OnError.Raise(sender, e.Value);
        }

        private void Connection_OnCommand(object sender, EventArgs<CommandPacket> e)
        {
            OnCommand.Raise(sender, e.Value);
        }

        public override void Stop()
        {
            NamedPipeServerStream pipeServer;
            PipeServerConnection connection;

            using (this.Lock())
            {
                pipeServer = this.pipeServer;
                connection = this.connection;
            }

            if (connection != null)
            {
                connection.Dispose();
            }

            if (pipeServer != null)
            {
                if (pipeServer.IsConnected)
                {
                    pipeServer.Disconnect();
                }

                pipeServer.Close();
                pipeServer.Dispose();
            }

            base.Stop();
        }
    }
}
