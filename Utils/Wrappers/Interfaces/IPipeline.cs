using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Utils.Wrappers.Interfaces
{
    public interface IPipeline
    {
        ICollection<Command> Commands { get; }
        IPipelineReader<object> Error { get; }
        bool HadErrors { get; }
        PipelineWriter Input { get; }
        long InstanceId { get; }
        bool IsNested { get; }
        IPipelineReader<PSObject> Output { get; }
        PipelineStateInfo PipelineStateInfo { get; }
        System.Management.Automation.Runspaces.Runspace Runspace { get; }
        bool SetPipelineSessionState { get; set; }
        Collection<PSObject> Connect();
        void ConnectAsync();
        System.Management.Automation.Runspaces.Pipeline Copy();
        void Dispose();
        Collection<PSObject> Invoke();
        Collection<PSObject> Invoke(IEnumerable input);
        void InvokeAsync();
        void Stop();
        void StopAsync();
    }
}