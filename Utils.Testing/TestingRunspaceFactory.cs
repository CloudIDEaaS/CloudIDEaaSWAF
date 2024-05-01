using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils
{
    public delegate void CreateRunspaceEventHandler(object sender, CreateRunspaceEventArgs e);

    public class CreateRunspaceEventArgs : EventArgs
    {
        public TypeTable TypeTable { get; }
        public PowerShellProcessInstance ProcessInstance { get; }
        public InitialSessionState InitialSessionState { get; }
        public RunspaceConnectionInfo ConnectionInfo { get; }
        public IRunspace Runspace { get; set; }

        public CreateRunspaceEventArgs(InitialSessionState initialSessionState)
        {
            this.InitialSessionState = initialSessionState;
        }

        public CreateRunspaceEventArgs(RunspaceConnectionInfo connectionInfo)
        {
            this.ConnectionInfo = connectionInfo;
        }

        public CreateRunspaceEventArgs(TypeTable typeTable, PowerShellProcessInstance processInstance)
        {
            this.TypeTable = typeTable;
            this.ProcessInstance = processInstance;
        }
    }

    public class TestingRunspaceFactory : Wrappers.Interfaces.IRunspaceFactory
    {
        public static event CreateRunspaceEventHandler OnCreateRunspace;

        public IRunspace CreateOutOfProcessRunspace(TypeTable typeTable, PowerShellProcessInstance processInstance)
        {
            if (TestingRunspaceFactory.OnCreateRunspace != null)
            {
                var eventArgs = new CreateRunspaceEventArgs(typeTable, processInstance);

                TestingRunspaceFactory.OnCreateRunspace(this, eventArgs);

                return eventArgs.Runspace;
            }

            return new Utils.Wrappers.Implementations.Runspace(System.Management.Automation.Runspaces.RunspaceFactory.CreateOutOfProcessRunspace(typeTable, processInstance));
        }

        public IRunspace CreateRunspace(InitialSessionState initialSessionState)
        {
            if (TestingRunspaceFactory.OnCreateRunspace != null)
            {
                var eventArgs = new CreateRunspaceEventArgs(initialSessionState);

                TestingRunspaceFactory.OnCreateRunspace(this, eventArgs);

                return eventArgs.Runspace;
            }

            return new Utils.Wrappers.Implementations.Runspace(System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace(initialSessionState));
        }

        public IRunspace CreateRunspace(RunspaceConnectionInfo connectionInfo)
        {
            if (TestingRunspaceFactory.OnCreateRunspace != null)
            {
                var eventArgs = new CreateRunspaceEventArgs(connectionInfo);

                TestingRunspaceFactory.OnCreateRunspace(this, eventArgs);

                return eventArgs.Runspace;
            }

            return new Utils.Wrappers.Implementations.Runspace(System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace(connectionInfo));
        }
    }
}
