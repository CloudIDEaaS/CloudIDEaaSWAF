using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.KestrelWAF
{
    public class KestrelStreamPipeReader : PipeReader
    {
        private static Type streamPipeReaderType;
        private object? internalReader;

        static KestrelStreamPipeReader()
        {
            streamPipeReaderType = typeof(PipeReader).Assembly.GetType("System.IO.Pipelines.StreamPipeReader");
        }

        public KestrelStreamPipeReader(Stream stream)
        {
            this.internalReader = Activator.CreateInstance(streamPipeReaderType, stream, new StreamPipeReaderOptions());
        }

        public override void AdvanceTo(SequencePosition consumed)
        {
            internalReader.CallMethod(nameof(AdvanceTo), consumed);
        }

        public override void AdvanceTo(SequencePosition consumed, SequencePosition examined)
        {
            internalReader.CallMethod(nameof(AdvanceTo), consumed, examined);
        }

        public override void CancelPendingRead()
        {
            internalReader.CallMethod(nameof(CancelPendingRead));
        }

        public override void Complete(Exception? exception = null)
        {
            internalReader.CallMethod(nameof(Complete), exception);
        }

        public override async ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = default)
        {
            return await (ValueTask<ReadResult>) internalReader.CallMethod(nameof(ReadAsync), cancellationToken);
        }

        public override bool TryRead(out ReadResult result)
        {
            throw new NotImplementedException();
        }
    }
}
