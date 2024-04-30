using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace Utils.Wrappers.Interfaces
{
    public interface IPipelineReader<T>
    {
        event EventHandler DataReady;
        int Count { get; }
        bool EndOfPipeline { get; }
        bool IsOpen { get; }
        int MaxCapacity { get; }
        WaitHandle WaitHandle { get; }
        void Close();
        Collection<T> NonBlockingRead();
        Collection<T> NonBlockingRead(int maxRequested);
        T Peek();
        T Read();
        Collection<T> Read(int count);
        Collection<T> ReadToEnd();
    }
}