#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcessDiagnosticsLibrary;

namespace Utils
{
    public class ProcessDiagnosticsWrapper : IProcessDiagnostics
    {
        private IProcessDiagnostics diagnostics;

        public ProcessDiagnosticsWrapper()
        {
        }

        public ProcessDiagnosticsWrapper(IProcessDiagnostics diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void RegisterProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage)
        {
            throw new NotImplementedException();
        }

        public string Ping()
        {
            throw new NotImplementedException();
        }

        public void RegisterRegion(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type)
        {
            if (diagnostics != null)
            {
                diagnostics.RegisterRegion(baseAddress, allocationBase, allocationProtect, regionSize, state, protect, type);
            }
        }

        public void RegisterRegionName(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type, string name)
        {
            if (diagnostics != null)
            {
                diagnostics.RegisterRegionName(baseAddress, allocationBase, allocationProtect, regionSize, state, protect, type, name);
            }
        }

        public void FreezeUI()
        {
            if (diagnostics != null)
            {
                diagnostics.FreezeUI();
            }
        }

        public void UnfreezeUI()
        {
            if (diagnostics != null)
            {
                diagnostics.UnfreezeUI();
            }
        }

        public void RegisterTestProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage, TestModule[] modules)
        {
            throw new NotImplementedException();
        }
    }
}
#endif