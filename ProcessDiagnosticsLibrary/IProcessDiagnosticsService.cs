using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProcessDiagnosticsLibrary;

namespace ProcessDiagnosticsService
{
    [ServiceContract]
    public interface IProcessDiagnosticsService
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        void RegisterProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage);

        [OperationContract]
        void RegisterTestProcess(uint processId, string imageFile, ulong baseAddress, ulong sizeOfImage, TestModule[] modules);

        [OperationContract]
        void RegisterRegion(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type);

        [OperationContract]
        void RegisterRegionName(ulong baseAddress, ulong allocationBase, int allocationProtect, ulong regionSize, int state, int protect, int type, string name);

        [OperationContract]
        void FreezeUI();

        [OperationContract]
        void UnfreezeUI();
    }
}
