using Xunit;
using Xunit.Abstractions;

namespace Utils.Testing
{
    public class DiagnosticMessageSink : LongLivedMarshalByRefObject, IMessageSink
    {
        public bool OnMessage(IMessageSinkMessage message)
        {
            Console.WriteLine(message);

            return true;
        }
    }
}