using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class DebugDirectory : DataDirectoryEntry
    {
        public uint Characteristics { get; set; }
        public uint TimeDateStamp { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public DebugType Type { get; set; }
        public uint SizeOfData { get; set; }
        public uint AddressOfRawData { get; set; }
        public uint PointerToRawData { get; set; }
        private List<DataDirectoryEntry> children;

        public DebugDirectory(ulong offset, ulong size) : base(offset, size)
        {
            children = new List<DataDirectoryEntry>();
        }

        public override bool HasChildren
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                return children;
            }
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"Characteristics: 0x{0:x8}, "
        			+ "TimeDateStamp: 0x{1:x8}, "
        			+ "MajorVersion: 0x{2:x4}, "
        			+ "MinorVersion: 0x{3:x4}, "
        			+ "Type: {4}, "
        			+ "SizeOfData: 0x{5:x8}, "
        			+ "AddressOfRawData: 0x{6:x8}, "
        			+ "PointerToRawData: 0x{7:x8}",
        			this.Characteristics,
        			this.TimeDateStamp,
        			this.MajorVersion,
        			this.MinorVersion,
        			this.Type,
        			this.SizeOfData,
        			this.AddressOfRawData,
        			this.PointerToRawData);
            }
        }

        public static List<DebugDirectory> ReadDebugDirectories(BinaryReader reader, DataDirectory dataDirectory)
        {
            DebugDirectory directory;
            var debugDirectories = new List<DebugDirectory>();
            var offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);

            reader.Seek(offset);

            do
            {
                var directoryOffset = reader.BaseStream.Position;

                directory = new DebugDirectory(offset, (sizeof(uint) * 6) + (sizeof(ushort) * 2))
                {
                    Characteristics = reader.ReadUInt32(),
                    TimeDateStamp = reader.ReadUInt32(),
                    MajorVersion = reader.ReadUInt16(),
                    MinorVersion = reader.ReadUInt16(),
                    Type = (DebugType)reader.ReadUInt32(),
                    SizeOfData = reader.ReadUInt32(),
                    AddressOfRawData = reader.ReadUInt32(),
                    PointerToRawData = reader.ReadUInt32()
                };

                ImageLayoutEvents.AddRelationship(directory, (ulong) directoryOffset, directory.Size, dataDirectory); 

                if (directory.AddressOfRawData != 0)
                {
                    using (var reset = reader.MarkForReset())
                    {
                        reader.Seek(directory.PointerToRawData);

                        switch (directory.Type)
                        {
                            case DebugType.CodeView:

                                var header = CodeViewHeader.Read(reader);

                                ImageLayoutEvents.AddReference<DebugDirectory, uint>(header, directory.PointerToRawData, header.Size, directory, (d) => d.AddressOfRawData);

                                directory.children.Add(header);

                                break;
                        }
                    }

                    debugDirectories.Add(directory);
                }
            }
            while (((ulong) reader.BaseStream.Position) < offset + dataDirectory.Size);

            return debugDirectories;
        }
    }
}
