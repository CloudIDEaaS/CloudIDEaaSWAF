using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Utils
{
    public class TestingRunspace
    {
        private System.Management.Automation.Runspaces.Runspace runspace;

        public virtual void Close()
        {
            runspace.Close();
        }

        public virtual void CloseAsync()
        {
            runspace.CloseAsync();
        }

        public virtual void Connect()
        {
            runspace.Connect();
        }

        public virtual void ConnectAsync()
        {
            runspace.ConnectAsync();
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreateDisconnectedPipeline()
        {
            return runspace.CreateDisconnectedPipeline();
        }

        public virtual System.Management.Automation.PowerShell CreateDisconnectedPowerShell()
        {
            return runspace.CreateDisconnectedPowerShell();
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreateNestedPipeline()
        {
            return runspace.CreateNestedPipeline();
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreateNestedPipeline(string command, bool addToHistory)
        {
            return runspace.CreateNestedPipeline(command, addToHistory);
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreatePipeline()
        {
            return runspace.CreatePipeline();
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreatePipeline(string command)
        {
            return runspace.CreatePipeline(command);
        }

        public virtual System.Management.Automation.Runspaces.Pipeline CreatePipeline(string command, bool addToHistory)
        {
            return runspace.CreatePipeline(command, addToHistory);
        }

        public virtual void Disconnect()
        {
            runspace.Disconnect();
        }

        public virtual void DisconnectAsync()
        {
            runspace.DisconnectAsync();
        }

        public virtual void Dispose()
        {
            runspace.Dispose();
        }

        public virtual System.Management.Automation.PSPrimitiveDictionary GetApplicationPrivateData()
        {
            return runspace.GetApplicationPrivateData();
        }

        public virtual System.Management.Automation.Runspaces.RunspaceCapability GetCapabilities()
        {
            return runspace.GetCapabilities();
        }

        public virtual void Open()
        {
            runspace.Open();
        }

        public virtual void OpenAsync()
        {
            runspace.OpenAsync();
        }

        public virtual void ResetRunspaceState()
        {
            runspace.ResetRunspaceState();
        }

        public virtual System.Threading.ApartmentState ApartmentState
        {
            get
            {
                return runspace.ApartmentState;
            }

            set
            {
                runspace.ApartmentState = value;
            }
        }

        public virtual System.Management.Automation.Runspaces.RunspaceConnectionInfo ConnectionInfo
        {
            get
            {
                return runspace.ConnectionInfo;
            }
        }

        public virtual System.Management.Automation.Debugger Debugger
        {
            get
            {
                return runspace.Debugger;
            }
        }

        public virtual System.DateTime? DisconnectedOn
        {
            get
            {
                return runspace.DisconnectedOn;
            }
        }

        public virtual System.Management.Automation.PSEventManager Events
        {
            get
            {
                return runspace.Events;
            }
        }

        public virtual System.DateTime? ExpiresOn
        {
            get
            {
                return runspace.ExpiresOn;
            }
        }

        public virtual int Id
        {
            get
            {
                return runspace.Id;
            }
        }

        public virtual System.Management.Automation.Runspaces.InitialSessionState InitialSessionState
        {
            get
            {
                return runspace.InitialSessionState;
            }
        }

        public virtual System.Guid InstanceId
        {
            get
            {
                return runspace.InstanceId;
            }
        }

        public virtual System.Management.Automation.JobManager JobManager
        {
            get
            {
                return runspace.JobManager;
            }
        }

        public virtual string Name
        {
            get
            {
                return runspace.Name;
            }

            set
            {
                runspace.Name = value;
            }
        }

        public virtual System.Management.Automation.Runspaces.RunspaceConnectionInfo OriginalConnectionInfo
        {
            get
            {
                return runspace.OriginalConnectionInfo;
            }
        }

        public virtual System.Management.Automation.Runspaces.RunspaceAvailability RunspaceAvailability
        {
            get
            {
                return runspace.RunspaceAvailability;
            }
        }

        public virtual bool RunspaceIsRemote
        {
            get
            {
                return runspace.RunspaceIsRemote;
            }
        }

        public virtual System.Management.Automation.Runspaces.RunspaceStateInfo RunspaceStateInfo
        {
            get
            {
                return runspace.RunspaceStateInfo;
            }
        }

        public virtual System.Management.Automation.Runspaces.SessionStateProxy SessionStateProxy
        {
            get
            {
                return runspace.SessionStateProxy;
            }
        }

        public virtual System.Management.Automation.Runspaces.PSThreadOptions ThreadOptions
        {
            get
            {
                return runspace.ThreadOptions;
            }

            set
            {
                runspace.ThreadOptions = value;
            }
        }

        public virtual System.Version Version
        {
            get
            {
                return runspace.Version;
            }
        }
    }
}