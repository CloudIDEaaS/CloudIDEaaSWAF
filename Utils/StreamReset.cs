using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils
{
    public class StreamReset : IDisposable
    {
        private Stream stream;
        private long originalPosition;

        public StreamReset(Stream stream)
        {
            this.stream = stream;
            this.originalPosition = stream.Position;
        }

        public void Dispose()
        {
            stream.Position = originalPosition;
        }
    }
}
