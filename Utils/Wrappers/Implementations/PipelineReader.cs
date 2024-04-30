using System;
using System.Collections.Generic;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils.Wrappers.Implementations
{
    public class PipelineReader<T> : IPipelineReader<T>
    {
        private System.Management.Automation.Runspaces.PipelineReader<T> pipelineReader;

        public event EventHandler DataReady;

        public PipelineReader(System.Management.Automation.Runspaces.PipelineReader<T> pipelineReader)
        {
            this.pipelineReader = pipelineReader;

            pipelineReader.DataReady += (sender, e) =>
            {
                DataReady?.Invoke(sender, e);
            };
        }

        public virtual void Close()
        {
            pipelineReader.Close();
        }

        public virtual System.Collections.ObjectModel.Collection<T> NonBlockingRead()
        {
            return pipelineReader.NonBlockingRead();
        }

        public virtual System.Collections.ObjectModel.Collection<T> NonBlockingRead(int maxRequested)
        {
            return pipelineReader.NonBlockingRead(maxRequested);
        }

        public virtual T Peek()
        {
            return pipelineReader.Peek();
        }

        public virtual T Read()
        {
            return pipelineReader.Read();
        }

        public virtual System.Collections.ObjectModel.Collection<T> Read(int count)
        {
            return pipelineReader.Read(count);
        }

        public virtual System.Collections.ObjectModel.Collection<T> ReadToEnd()
        {
            return pipelineReader.ReadToEnd();
        }

        public virtual int Count
        {
            get
            {
                return pipelineReader.Count;
            }
        }

        public virtual bool EndOfPipeline
        {
            get
            {
                return pipelineReader.EndOfPipeline;
            }
        }

        public virtual bool IsOpen
        {
            get
            {
                return pipelineReader.IsOpen;
            }
        }

        public virtual int MaxCapacity
        {
            get
            {
                return pipelineReader.MaxCapacity;
            }
        }

        public virtual System.Threading.WaitHandle WaitHandle
        {
            get
            {
                return pipelineReader.WaitHandle;
            }
        }
    }
}
