using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
#if !UTILS_INTERNAL
    public interface IJsonTransactionLog : IDisposable
#else
    internal interface IJsonTransactionLog : IDisposable
#endif
    {
        string MasterLogFile { get; }
        IDisposable CreateTransaction(string fileName, string json);
        void CheckSize();
    }
}
