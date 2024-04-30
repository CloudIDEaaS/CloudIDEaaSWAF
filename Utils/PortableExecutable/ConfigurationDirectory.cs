using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ConfigurationDirectory : DataDirectoryEntry
    {
        public uint TimeDateStamp { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public uint GlobalFlagsClear { get; set; }
        public uint GlobalFlagsSet { get; set; }
        public uint CriticalSectionDefaultTimeout { get; set; }
        public ulong DeCommitFreeBlockThreshold { get; set; }
        public ulong DeCommitTotalFreeThreshold { get; set; }
        public ulong LockPrefixTable { get; set; }
        public ulong MaximumAllocationSize { get; set; }
        public ulong VirtualMemoryThreshold { get; set; }
        public ulong ProcessAffinityMask { get; set; }
        public HeapCreateOptions ProcessHeapFlags { get; set; }
        public ushort CSDVersion { get; set; }
        public ushort Reserved1 { get; set; }
        public ulong EditList { get; set; }
        public ulong SecurityCookie { get; set; }
        public ulong SEHandlerTable { get; set; }
        public ulong SEHandlerCount { get; set; }
        private bool is64Bit;

        public ConfigurationDirectory(Machine machine, ulong offset, ulong size) : base(offset, size)
        {
            is64Bit = machine.Is64Bit();
        }

        public override string DebugInfo
        {
            get 
            {
                if (is64Bit)
                {
                    return string.Format(
                        "Size: 0x{0:x8}, "
                        + "TimeDateStamp: 0x{1:x8}, "
                        + "MajorVersion: 0x{2:x4}, "
                        + "MinorVersion: 0x{3:x4}, "
                        + "GlobalFlagsClear: 0x{4:x8}, "
                        + "GlobalFlagsSet: 0x{5:x8}, "
                        + "CriticalSectionDefaultTimeout: 0x{6:x8}, "
                        + "DeCommitFreeBlockThreshold: 0x{7:x16}, "
                        + "DeCommitTotalFreeThreshold: 0x{8:x16}, "
                        + "LockPrefixTable: 0x{9:x16}, "
                        + "MaximumAllocationSize: 0x{10:x16}, "
                        + "VirtualMemoryThreshold: 0x{11:x16}, "
                        + "ProcessAffinityMask: 0x{12:x16}, "
                        + "ProcessHeapFlags: {13}, "
                        + "CSDVersion: 0x{14:x4}, "
                        + "Reserved1: 0x{15:x4}, "
                        + "EditList: 0x{16:x16}, "
                        + "SecurityCookie: 0x{17:x16}, "
                        + "SEHandlerTable: 0x{18:x16}, "
                        + "SEHandlerCount: 0x{19:x16}",
                        this.Size,
                        this.TimeDateStamp,
                        this.MajorVersion,
                        this.MinorVersion,
                        this.GlobalFlagsClear,
                        this.GlobalFlagsSet,
                        this.CriticalSectionDefaultTimeout,
                        this.DeCommitFreeBlockThreshold,
                        this.DeCommitTotalFreeThreshold,
                        this.LockPrefixTable,
                        this.MaximumAllocationSize,
                        this.VirtualMemoryThreshold,
                        this.ProcessAffinityMask,
                        this.ProcessHeapFlags,
                        this.CSDVersion,
                        this.Reserved1,
                        this.EditList,
                        this.SecurityCookie,
                        this.SEHandlerTable,
                        this.SEHandlerCount);
                }
                else
                {
                    return string.Format(
                        "Size: 0x{0:x8}, "
                        + "TimeDateStamp: 0x{1:x8}, "
                        + "MajorVersion: 0x{2:x4}, "
                        + "MinorVersion: 0x{3:x4}, "
                        + "GlobalFlagsClear: 0x{4:x8}, "
                        + "GlobalFlagsSet: 0x{5:x8}, "
                        + "CriticalSectionDefaultTimeout: 0x{6:x8}, "
                        + "DeCommitFreeBlockThreshold: 0x{7:x8}, "
                        + "DeCommitTotalFreeThreshold: 0x{8:x8}, "
                        + "LockPrefixTable: 0x{9:x8}, "
                        + "MaximumAllocationSize: 0x{10:x8}, "
                        + "VirtualMemoryThreshold: 0x{11:x8}, "
                        + "ProcessAffinityMask: 0x{12:x8}, "
                        + "ProcessHeapFlags: {13}, "
                        + "CSDVersion: 0x{14:x4}, "
                        + "Reserved1: 0x{15:x4}, "
                        + "EditList: 0x{16:x8}, "
                        + "SecurityCookie: 0x{17:x8}, "
                        + "SEHandlerTable: 0x{18:x8}, "
                        + "SEHandlerCount: 0x{19:x8}",
                        this.Size,
                        this.TimeDateStamp,
                        this.MajorVersion,
                        this.MinorVersion,
                        this.GlobalFlagsClear,
                        this.GlobalFlagsSet,
                        this.CriticalSectionDefaultTimeout,
                        this.DeCommitFreeBlockThreshold,
                        this.DeCommitTotalFreeThreshold,
                        this.LockPrefixTable,
                        this.MaximumAllocationSize,
                        this.VirtualMemoryThreshold,
                        this.ProcessAffinityMask,
                        this.ProcessHeapFlags,
                        this.CSDVersion,
                        this.Reserved1,
                        this.EditList,
                        this.SecurityCookie,
                        this.SEHandlerTable,
                        this.SEHandlerCount);
                }
            }
        }

        public static List<ConfigurationDirectory> ReadDirectories(BinaryReader reader, DataDirectory dataDirectory)
        {
            var offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);
            var directories = new List<ConfigurationDirectory>();
            ConfigurationDirectory directory;
            var machine = dataDirectory.Machine;
            var is64bit = machine.Is64Bit();

            reader.Seek(offset);

            do
            {
                var directoryOffset = reader.BaseStream.Position;

                if (is64bit)
                {
                    directory = new ConfigurationDirectory(machine, (ulong)directoryOffset, 0)
                    {
        		        Size = reader.ReadUInt32(),
        		        TimeDateStamp = reader.ReadUInt32(),
        		        MajorVersion = reader.ReadUInt16(),
        		        MinorVersion = reader.ReadUInt16(),
        		        GlobalFlagsClear = reader.ReadUInt32(),
        		        GlobalFlagsSet = reader.ReadUInt32(),
        		        CriticalSectionDefaultTimeout = reader.ReadUInt32(),
        		        DeCommitFreeBlockThreshold = reader.ReadUInt64(),
        		        DeCommitTotalFreeThreshold = reader.ReadUInt64(),
        		        LockPrefixTable = reader.ReadUInt64(),
        		        MaximumAllocationSize = reader.ReadUInt64(),
        		        VirtualMemoryThreshold = reader.ReadUInt64(),
        		        ProcessAffinityMask = reader.ReadUInt64(),
                        ProcessHeapFlags = (HeapCreateOptions) reader.ReadUInt32(),
        		        CSDVersion = reader.ReadUInt16(),
        		        Reserved1 = reader.ReadUInt16(),
        		        EditList = reader.ReadUInt64(),
        		        SecurityCookie = reader.ReadUInt64(),
        		        SEHandlerTable = reader.ReadUInt64(),
        		        SEHandlerCount = reader.ReadUInt64(),
                    };
                }
                else
                {
                    directory = new ConfigurationDirectory(machine, (ulong)directoryOffset, 0)
                    {
                        Size = reader.ReadUInt32(),
                        TimeDateStamp = reader.ReadUInt32(),
                        MajorVersion = reader.ReadUInt16(),
                        MinorVersion = reader.ReadUInt16(),
                        GlobalFlagsClear = reader.ReadUInt32(),
                        GlobalFlagsSet = reader.ReadUInt32(),
                        CriticalSectionDefaultTimeout = reader.ReadUInt32(),
                        DeCommitFreeBlockThreshold = reader.ReadUInt32(),
                        DeCommitTotalFreeThreshold = reader.ReadUInt32(),
                        LockPrefixTable = reader.ReadUInt32(),
                        MaximumAllocationSize = reader.ReadUInt32(),
                        VirtualMemoryThreshold = reader.ReadUInt32(),
                        ProcessAffinityMask = reader.ReadUInt32(),
                        ProcessHeapFlags = (HeapCreateOptions)reader.ReadUInt32(),
                        CSDVersion = reader.ReadUInt16(),
                        Reserved1 = reader.ReadUInt16(),
                        EditList = reader.ReadUInt32(),
                        SecurityCookie = reader.ReadUInt32(),
                        SEHandlerTable = reader.ReadUInt32(),
                        SEHandlerCount = reader.ReadUInt32(),
                    };
                }

                directories.Add(directory);

                ImageLayoutEvents.AddRelationship(directory, (ulong)directoryOffset, directory.Size, dataDirectory);
            }
            while (((ulong)reader.BaseStream.Position) < offset + dataDirectory.Size);

            return directories;
        }
    }
}
