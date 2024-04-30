

namespace WebSecurity.KestrelWAF
{
    public interface IWAFDataFileCache
    {
        IDisposable IndexFile(string filename);
        bool PartialMatchFromFile(string filename, string searchText);
    }
}