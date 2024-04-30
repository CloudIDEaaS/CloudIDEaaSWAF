using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Utils.Wrappers.Interfaces
{
    public interface IRunspaceFactory
    {
        IRunspace CreateRunspace(InitialSessionState initialSessionState);
        IRunspace CreateRunspace(RunspaceConnectionInfo connectionInfo);
        IRunspace CreateOutOfProcessRunspace(TypeTable typeTable, PowerShellProcessInstance processInstance);
    }
}
