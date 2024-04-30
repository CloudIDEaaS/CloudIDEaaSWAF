using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void OnBufferDump(object sender, BufferDumpEventArgs e);

    public class BufferDumpEventArgs
    {
        private byte[] currentBuffer;

        public BufferDumpEventArgs(byte[] currentBuffer)
        {
            this.currentBuffer = currentBuffer;
        }
    }

    public class BufferBuilder : IDisposable
    {
        private MemoryStream stream;
        private BufferedStream buffer;
        public event OnBufferDump OnBufferDump;

        public BufferBuilder(int size)
        {
            stream = new MemoryStream();
            buffer = new BufferedStream(stream, size);
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public void WriteLine(string value)
        {
            this.Write(value + "\r\n");
        }

        public void Write(string value)
        {
            var valueLength = (long) value.Length;
            var remainingLength = buffer.Length - buffer.Position;
            string leftValue;
            string rightValue;
            long leftLength;
            long rightLength;

            if (remainingLength < valueLength)
            {
                byte[] bytes;

                leftLength = valueLength - remainingLength;
                rightLength = valueLength - leftLength;

                leftValue = value.Left((int) leftLength);
                rightValue = value.Right((int)rightLength);

                buffer.Write(ASCIIEncoding.ASCII.GetBytes(leftValue), 0, leftLength);
                buffer.Flush();

                bytes = new byte[buffer.Position];
                buffer.Read(bytes, 0, (int) buffer.Position);

                OnBufferDump(this, new BufferDumpEventArgs(bytes));

                buffer.Rewind();
            }
            else
            {
                buffer.Write(ASCIIEncoding.ASCII.GetBytes(value), 0, valueLength);
                buffer.Flush();
            }
        }
    }
}
