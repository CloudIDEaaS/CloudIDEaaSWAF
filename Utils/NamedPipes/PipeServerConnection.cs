using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Utils.NamedPipes
{
    public class PipeServerConnection : IDisposable
    {
        public string Name { get; }
        public event EventHandlerT<CommandPacket> OnCommand;
        public event EventHandlerT<Exception> OnError;
        public event EventHandlerT<PipeServerConnection> OnConnectionMade;
        private StreamWriter writer;
        private StreamReader reader;
        private IAsyncResult asyncResult;
        private NamedPipeServerStream pipeServer;
        private NamedPipeServer server;
        private bool connectionMade;

        public PipeServerConnection(string name, NamedPipeServerStream pipeServer, NamedPipeServer server)
        {
            this.pipeServer = pipeServer;
            this.server = server;
            this.Name = name;
        }

        public void Listen()
        {
            try
            {
                asyncResult = pipeServer.BeginWaitForConnection((r) =>
                {
                    try
                    {
                        pipeServer.EndWaitForConnection(r);

                        connectionMade = true;
                        writer = new StreamWriter(pipeServer);
                        reader = new StreamReader(pipeServer);

                        writer.WriteLine("Connected!");
                        writer.Flush();

                        OnConnectionMade.Raise(this, this);

                        Read();
                    }
                    catch (Exception ex)
                    {
                        OnError.Raise(this, new IOException(string.Format("Pipe Server Connection Error: {0}", ex.Message)));
                    }
                }, null);
            }
            catch (Exception ex)
            {
                OnError.Raise(this, new IOException(string.Format("Pipe Server Connection Error: {0}", ex.Message)));
            }
        }

        public bool ConnectionMade
        {
            get
            {
                return server.LockReturn(() => connectionMade);
            }
        }

        public bool IsConnected
        {
            get
            {
                return server.LockReturn(() => pipeServer.IsConnected);
            }
        }

        private void Read()
        {
            CommandPacket commandPacket;

            try
            {
                commandPacket = reader.ReadJsonCommand();
                OnCommand.Invoke(this, new EventArgs<CommandPacket>(commandPacket));
            }
            catch (Exception ex)
            {
                OnError.Raise(this, ex);
            }
        }

        public void Send(CommandPacket commandPacket)
        {
            try
            {
                writer.WriteJsonCommand(commandPacket);
                writer.Flush();

                Task.Run(() =>
                {
                    Read();
                });
            }
            catch (Exception ex)
            {
                OnError.Raise(this, ex);
            }
        }

        public void Dispose()
        {
            if (writer != null)
            {
                try
                {
                    writer.Dispose();
                }
                catch
                {
                }
            }

            if (reader != null)
            {
                try
                {
                    reader.Close();
                }
                catch
                {
                }
            }

            if (pipeServer.IsConnected)
            {
                pipeServer.Disconnect();
            }

            pipeServer.Dispose();
        }
    }
}
