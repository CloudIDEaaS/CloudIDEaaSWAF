using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
using ProcessDiagnosticsLibrary;
#endif

namespace Utils
{
    public enum ProcessSeekOrigin
    {
        Begin = 0,
        Current = 1,
        End = 2,
        BeginningOfProcessMemory
    }

    public class ProcessStream : MemoryStream
    {
        internal event SeekOverunHandler OnSeekOverun;
        private ulong internalLength;
        private long outOfRegionPosition;
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
        public IProcessDiagnostics ProcessDiagnostics { get; set; }
#endif

        public ProcessStream(ulong length)
        {
            this.internalLength = length;
            outOfRegionPosition = -1;
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            return Seek(offset, (ProcessSeekOrigin) loc);
        }

        public long Seek(long offset, ProcessSeekOrigin loc)
        {
            SeekOverunEventArgs args = null;
            var continueBaseSeek = true;

            switch (loc)
            {
                case ProcessSeekOrigin.BeginningOfProcessMemory:

                    args = new SeekOverunEventArgs(offset, loc);

                    OnSeekOverun(this, args);
                    continueBaseSeek = args.ContinueBaseSeek;

                    break;

                case ProcessSeekOrigin.Begin:

                    if (offset > this.BufferLength || offset < 0)
                    {
                        args = new SeekOverunEventArgs(offset, loc);

                        OnSeekOverun(this, args);
                        continueBaseSeek = args.ContinueBaseSeek;
                    }

                    break;

                case ProcessSeekOrigin.End:
                    
                    var position = this.BufferLength + offset;

                    if (position > this.BufferLength || position < 0)
                    {
                        args = new SeekOverunEventArgs(offset, loc);

                        OnSeekOverun(this, args);
                        continueBaseSeek = args.ContinueBaseSeek;
                    }
                    
                    break;

                case ProcessSeekOrigin.Current:

                    if (offset > this.BufferLength - this.Position || offset < 0)
                    {
                        args = new SeekOverunEventArgs(offset, loc);

                        OnSeekOverun(this, args);
                        continueBaseSeek = args.ContinueBaseSeek;
                    }

                    break;

                default:

                    e.Throw<ArgumentException>("Invalid seek origin {0}", loc);
                    break;
            }

            if (continueBaseSeek)
            {
                if (args != null)
                {
                    if (args.OutOfRegionPosition)
                    {
                        switch (args.BaseSeekOrigin)
                        {
                            case SeekOrigin.Begin:

                                outOfRegionPosition = args.BaseOffset;
                                break;

                            case SeekOrigin.Current:

                                outOfRegionPosition = base.Position + args.BaseOffset;
                                break;

                            case SeekOrigin.End:

                                outOfRegionPosition = base.Length + args.BaseOffset;
                                break;
                        }

                        return outOfRegionPosition;
                    }
                    else
                    {
                        outOfRegionPosition = -1;

                        try
                        {
                            return base.Seek(args.BaseOffset, args.BaseSeekOrigin);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    try
                    {
                        return base.Seek(offset, (SeekOrigin)loc);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                return args.Position;
            }
        }

        public override long Position
        {
            get
            {
                if (outOfRegionPosition != -1)
                {
                    return outOfRegionPosition;
                }
                else
                {
                    return base.Position;
                }
            }

            set
            {
                base.Position = value;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var remainingBytes = this.BufferLength - (int) this.Position;
            var neededBytes = count - remainingBytes;

            internalLength += (ulong) neededBytes;
            this.Capacity = (int) internalLength;

            base.Write(buffer, offset, count);
        }

        internal int BufferLength
        {
            get
            {
                return this.GetPrivateFieldValue<int>("_length");
            }
        }

        public override long Length
        {
            get
            {
                return (long) internalLength;
            }
        }
    }
}
