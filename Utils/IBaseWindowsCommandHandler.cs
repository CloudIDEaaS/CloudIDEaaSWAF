using System.Diagnostics;

namespace Utils
{
    public interface IBaseWindowsCommandHandler
    {
        string Command { get; set; }
        string CommandExe { get; set; }
        ErrorWriteLine ErrorWriteLine { get; set; }
        bool IsProcessRunning { get; }
        bool NoWait { get; set; }
        OutputWriteLine OutputWriteLine { get; set; }
        void RunCommand(string command, string workingDirectory, params string[] arguments);
        Process Process { get; }
        bool Succeeded { get; }
        void Dispose();
        void Wait();
    }
}