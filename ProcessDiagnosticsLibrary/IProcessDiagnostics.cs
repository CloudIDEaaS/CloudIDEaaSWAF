using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessDiagnosticsLibrary
{
    public interface IProcessDiagnostics
    {
        void RegisterProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage);
        void RegisterTestProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage, TestModule[] modules);
        string Ping();
        void RegisterRegion(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type);
        void RegisterRegionName(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type, string name);
        void FreezeUI();
        void UnfreezeUI();
    }
}
