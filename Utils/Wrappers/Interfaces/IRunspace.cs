using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Utils.Wrappers.Interfaces
{
    public interface IRunspace
    {
        ApartmentState ApartmentState { get; set; }
        RunspaceConnectionInfo ConnectionInfo { get; }
        Debugger Debugger { get; }
        DateTime? DisconnectedOn { get; }
        PSEventManager Events { get; }
        DateTime? ExpiresOn { get; }
        int Id { get; }
        InitialSessionState InitialSessionState { get; }
        Guid InstanceId { get; }
        JobManager JobManager { get; }
        string Name { get; set; }
        RunspaceConnectionInfo OriginalConnectionInfo { get; }
        RunspaceAvailability RunspaceAvailability { get; }
        bool RunspaceIsRemote { get; }
        RunspaceStateInfo RunspaceStateInfo { get; }
        SessionStateProxy SessionStateProxy { get; }
        PSThreadOptions ThreadOptions { get; set; }
        Version Version { get; }
        void Close();
        void CloseAsync();
        void Connect();
        void ConnectAsync();
        IPipeline CreateDisconnectedPipeline();
        PowerShell CreateDisconnectedPowerShell();
        IPipeline CreateNestedPipeline();
        IPipeline CreateNestedPipeline(string command, bool addToHistory);
        IPipeline CreatePipeline();
        IPipeline CreatePipeline(string command);
        IPipeline CreatePipeline(string command, bool addToHistory);
        void Disconnect();
        void DisconnectAsync();
        void Dispose();
        PSPrimitiveDictionary GetApplicationPrivateData();
        RunspaceCapability GetCapabilities();
        void Open();
        void OpenAsync();
        void ResetRunspaceState();
    }
}