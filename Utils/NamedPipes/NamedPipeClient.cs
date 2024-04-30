using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Utils.NamedPipes
{
    public class NamedPipeClient : IDisposable
    {
        private string pipeName;
        private NamedPipeClientStream pipeClient;
        private bool connected;
        private StreamReader reader;
        private StreamWriter writer;
        public event PipeExceptionEventHandler PipeError;
        public event PipeMessageEventHandler ServerMessage;

        public NamedPipeClient(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public void Connect()
        {
            pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut);
            string ack;

            try
            {
                pipeClient.Connect(1000);

                reader = new StreamReader(pipeClient);
                writer = new StreamWriter(pipeClient);

                Console.WriteLine("Generator waiting for acknowledgement...");

                ack = reader.ReadLine();

                if (ack == "Connected!")
                {
                    this.connected = true;

                    Console.WriteLine(ack);
                }
                else
                {
                    Console.WriteLine(ack);
                }
            }
            catch (Exception ex)
            {
                PipeError(this, new IOException(string.Format("Could not connect to pipe '{0}'. Error '{1}'", pipeName, ex.Message), ex));
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
                PipeError(this, new IOException(string.Format("Could not write to pipe '{0}'. Error '{1}'", pipeName, ex.Message), ex));
            }
        }

        private void Read()
        {
            try
            {
                var commandPacket = reader.ReadJsonCommand();

                ServerMessage(this, commandPacket);
            }
            catch (Exception ex)
            {
                PipeError(this, new IOException(string.Format("Could not read from pipe '{0}'. Error '{1}'", pipeName, ex.Message), ex));
            }
        }

        public void Dispose()
        {
            this.Disconnect();
        }

        private void Disconnect()
        {
            if (pipeClient != null)
            {
                writer.Close();
                reader.Close();

                pipeClient.Close();
                pipeClient.Dispose();
            }
        }
    }
}
