using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils.IO
{
    using System;
    using System.Diagnostics;
    using System.ComponentModel;
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
    using ProcessDiagnosticsLibrary;
#endif
    using System.IO;

    public class ProcessBinaryReader : BinaryReader
    {
        private Process process;
        private ulong baseAddress;
        private uint hProcess;
        private ProcessStream baseStream;
        private static ProcessExtensions.SYSTEM_INFO systemInfo;
        private Dictionary<ulong, ProcessBinaryReader> regions;
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
        private IProcessDiagnostics processDiagnostics;
#endif

        static ProcessBinaryReader()
        {
            ProcessExtensions.GetSystemInfo(out systemInfo);
        }

        private ProcessBinaryReader(IntPtr baseAddress, Stream inputStream) : base(inputStream)
        {
            this.baseStream = (ProcessStream)this.BaseStream;
            this.baseAddress = (ulong)unchecked(baseAddress.ToInt64());
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
            this.processDiagnostics = new ProcessDiagnosticsWrapper();

            this.baseStream.ProcessDiagnostics = processDiagnostics;
#endif

            this.regions = new Dictionary<ulong, ProcessBinaryReader>();
        }

        public unsafe ProcessBinaryReader(uint hProcess, IntPtr baseAddress, ulong size) : this(baseAddress, InitializeStream(hProcess, baseAddress, size))
        {
            baseStream.OnSeekOverun += new SeekOverunHandler(OnSeekOverun);
            this.hProcess = hProcess;
        }

#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
        public unsafe ProcessBinaryReader(uint hProcess, IntPtr baseAddress, ulong size, IProcessDiagnostics processDiagnostics) : this(hProcess, baseAddress, size)
        {
            this.processDiagnostics = new ProcessDiagnosticsWrapper(processDiagnostics);
            this.baseStream.ProcessDiagnostics = processDiagnostics;
        }
#endif

        public unsafe ProcessBinaryReader(Process process, IntPtr baseAddress, ulong size) : this(baseAddress, InitializeStream((uint) process.Handle, baseAddress, size))
        {
            baseStream.OnSeekOverun += new SeekOverunHandler(OnSeekOverun);
            this.process = process;
            this.hProcess = (uint) process.Handle;
        }

        public override void Close()
        {
            ProcessExtensions.CloseHandle(hProcess);
            base.Close();
        }

        private void OnSeekOverun(object sender, SeekOverunEventArgs e)
        {
            ProcessExtensions.MEMORY_BASIC_INFORMATION memBasicInfo;
            ulong address = 0;

            switch (e.Location)
            {
                case ProcessSeekOrigin.BeginningOfProcessMemory:
                    address = (ulong)e.Offset;
                    break;
                case ProcessSeekOrigin.Begin:
                    address = baseAddress + (ulong) e.Offset; 
                    break;
                case ProcessSeekOrigin.Current:
                    address = baseAddress + (ulong) (baseStream.Position + e.Offset); 
                    break;
                case ProcessSeekOrigin.End:
                    address = baseAddress + (ulong) (baseStream.Length + e.Offset);
                    break;
            }

            e.Position = (long) address;
            e.ContinueBaseSeek = false;

            memBasicInfo = ProcessExtensions.GetMemoryInfo(hProcess, address);

            if (memBasicInfo.Protect == 0 | memBasicInfo.Protect == ProcessExtensions.AllocationProtectEnum.Readonly | memBasicInfo.Protect == ProcessExtensions.AllocationProtectEnum.ReadWrite | memBasicInfo.Protect == ProcessExtensions.AllocationProtectEnum.ExecuteRead | memBasicInfo.Protect == ProcessExtensions.AllocationProtectEnum.ExecuteReadWrite)
            {
                var offsetFromBase = (long)(address - baseAddress);
                
                e.MemoryAllocated = true;
                e.ContinueBaseSeek = true;
                e.BaseSeekOrigin = SeekOrigin.Begin;
                e.BaseOffset = offsetFromBase;

                if (memBasicInfo.AllocationBase.As<ulong>() != baseAddress && (offsetFromBase > systemInfo.AllocationGranularity || offsetFromBase < 0))
                {
                    if (!regions.ContainsKey((ulong) memBasicInfo.AllocationBase))
                    {
                        regions.Add((ulong) memBasicInfo.AllocationBase, new ProcessBinaryReader(hProcess, (IntPtr)memBasicInfo.AllocationBase, (ulong) memBasicInfo.RegionSize));
#if INCLUDE_PROCESSDIAGNOSTICSLIBRARY
                        processDiagnostics.RegisterRegion((ulong)memBasicInfo.BaseAddress, (ulong)memBasicInfo.AllocationBase, (int)memBasicInfo.AllocationProtect, (ulong)memBasicInfo.RegionSize, (int)memBasicInfo.State, (int)memBasicInfo.Protect, (int)memBasicInfo.Type);
#endif
                    }

                    e.OutOfRegionPosition = true;
                }
                else
                {
                    e.OutOfRegionPosition = false;
                }
            }
            else
            {
                throw new Win32Exception(unchecked((int) 0xc0000005), string.Format("Attempt to read memory marked as {0} by {1}", memBasicInfo.Protect, this.GetType().FullName));
            }
        }

        private unsafe static Stream InitializeStream(uint hProcess, IntPtr baseAddress, ulong size)
        {
            return new ProcessStream(size);
        }

        public override byte[] ReadBytes(int count)
        {
            var returnBytes = new byte[count];
            byte[] byteBlock;

            if (baseStream.BufferLength > baseStream.Position + count)
            {
                returnBytes = baseStream.Read(count);
            }
            else
            {
                var address = ((ulong) baseStream.Position) + baseAddress;
                var region = regions.Where(p => address.IsBetween(p.Key, regions.GetNextAddress(p.Key))).Select(p => p.Value).SingleOrDefault();

                if (region != null)
                {
                    var offset = (long) (address - regions.GetBaseAddress(region));

                    region.Seek(offset, ProcessSeekOrigin.Begin);

                    returnBytes = region.ReadBytes(count);

                    this.Seek(count, ProcessSeekOrigin.Current);
                }
                else
                {
                    if (baseStream.BufferLength < baseStream.Position)
                    {
                        using (var reset = this.MarkForReset())
                        {
                            var backupOffset = (long) (baseStream.BufferLength - baseStream.Position);

                            if (backupOffset < 0)
                            {
                                baseStream.Seek((long) backupOffset, ProcessSeekOrigin.Current);

                                if (process != null)
                                {
                                    byteBlock = process.ReadProcessMemory(baseAddress + (ulong)this.BaseStream.Position, count + (int)-backupOffset);
                                }
                                else
                                {
                                    byteBlock = ProcessExtensions.ReadProcessMemory(hProcess, baseAddress + (ulong)this.BaseStream.Position, count + (int)-backupOffset);
                                }
                            }
                            else
                            {
                                byteBlock = null;
                            }

                            baseStream.Write(byteBlock);
                        }
                    }
                    else
                    {
                        var remainingBytes = baseStream.BufferLength - (int)baseStream.Position;
                        var neededBytes = count - remainingBytes;

                        if (process != null)
                        {
                            byteBlock = process.ReadProcessMemory(baseAddress + (ulong)this.BaseStream.Position, count);
                        }
                        else
                        {
                            byteBlock = ProcessExtensions.ReadProcessMemory(hProcess, baseAddress + (ulong)this.BaseStream.Position, count);
                        }

                        using (var reset = this.MarkForReset())
                        {
                            baseStream.Write(byteBlock);
                        }
                    }

                    return ReadBytes(count);
                }
            }

            return returnBytes;
        }

        public override ushort ReadUInt16()
        {
            byte[] bytes;

            bytes = this.ReadBytes(sizeof(UInt16));

            return BitConverter.ToUInt16(bytes, 0);
        }

        public override uint ReadUInt32()
        {
            byte[] bytes;

            bytes = this.ReadBytes(sizeof(UInt32));

            return BitConverter.ToUInt32(bytes, 0);
        }

        public override byte ReadByte()
        {
            byte[] bytes;

            bytes = this.ReadBytes(sizeof(byte));

            return bytes[0];
        }

        public override short ReadInt16()
        {
            byte[] bytes;

            bytes = this.ReadBytes(sizeof(Int16));

            return BitConverter.ToInt16(bytes, 0);
        }

        public override ulong ReadUInt64()
        {
            byte[] bytes;

            bytes = this.ReadBytes(sizeof(UInt64));

            return BitConverter.ToUInt64(bytes, 0);
        }

        protected override void FillBuffer(int numBytes)
        {
            base.FillBuffer(numBytes);
        }

        public override int PeekChar()
        {
            throw new NotImplementedException();
        }

        public override int Read()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override bool ReadBoolean()
        {
            throw new NotImplementedException();
        }

        public override char ReadChar()
        {
            throw new NotImplementedException();
        }

        public override char[] ReadChars(int count)
        {
            throw new NotImplementedException();
        }

        public override decimal ReadDecimal()
        {
            throw new NotImplementedException();
        }

        public override double ReadDouble()
        {
            throw new NotImplementedException();
        }

        public override int ReadInt32()
        {
            throw new NotImplementedException();
        }

        public override long ReadInt64()
        {
            throw new NotImplementedException();
        }

        public override sbyte ReadSByte()
        {
            throw new NotImplementedException();
        }

        public override float ReadSingle()
        {
            throw new NotImplementedException();
        }

        public override string ReadString()
        {
            throw new NotImplementedException();
        }
    }
}

