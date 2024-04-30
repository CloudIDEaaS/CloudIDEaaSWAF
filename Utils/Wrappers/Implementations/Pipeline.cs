using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils.Wrappers.Implementations
{
    public class Pipeline : IPipeline
    {
        private System.Management.Automation.Runspaces.Pipeline pipeline;

        public Pipeline(System.Management.Automation.Runspaces.Pipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public virtual System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> Connect()
        {
            return pipeline.Connect();
        }

        public virtual void ConnectAsync()
        {
            pipeline.ConnectAsync();
        }

        public virtual System.Management.Automation.Runspaces.Pipeline Copy()
        {
            return pipeline.Copy();
        }

        public virtual void Dispose()
        {
            pipeline.Dispose();
        }

        public virtual System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> Invoke()
        {
            return pipeline.Invoke();
        }

        public virtual System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> Invoke(System.Collections.IEnumerable input)
        {
            return pipeline.Invoke(input);
        }

        public virtual void InvokeAsync()
        {
            pipeline.InvokeAsync();
        }

        public virtual void Stop()
        {
            pipeline.Stop();
        }

        public virtual void StopAsync()
        {
            pipeline.StopAsync();
        }

        public virtual ICollection<Command> Commands
        {
            get
            {
                return pipeline.Commands;
            }
        }

        public virtual IPipelineReader<object> Error
        {
            get
            {
                return new PipelineReader<object>(pipeline.Error);
            }
        }

        public virtual bool HadErrors
        {
            get
            {
                return pipeline.HadErrors;
            }
        }

        public virtual System.Management.Automation.Runspaces.PipelineWriter Input
        {
            get
            {
                return pipeline.Input;
            }
        }

        public virtual long InstanceId
        {
            get
            {
                return pipeline.InstanceId;
            }
        }

        public virtual bool IsNested
        {
            get
            {
                return pipeline.IsNested;
            }
        }

        public virtual IPipelineReader<System.Management.Automation.PSObject> Output
        {
            get
            {
                return new PipelineReader<System.Management.Automation.PSObject>(pipeline.Output);
            }
        }

        public virtual System.Management.Automation.Runspaces.PipelineStateInfo PipelineStateInfo
        {
            get
            {
                return pipeline.PipelineStateInfo;
            }
        }

        public virtual System.Management.Automation.Runspaces.Runspace Runspace
        {
            get
            {
                return pipeline.Runspace;
            }
        }

        public virtual bool SetPipelineSessionState
        {
            get
            {
                return pipeline.SetPipelineSessionState;
            }

            set
            {
                pipeline.SetPipelineSessionState = value;
            }
        }
    }
}
