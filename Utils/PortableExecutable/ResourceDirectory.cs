using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using System.Diagnostics;

namespace Utils.PortableExecutable
{
    public class ResourceDirectory : ResourceEntry
    {
        public uint Characteristics { get; set; }
        public uint TimeDateStamp { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort NumberOfIdEntries { get; set; }
        public ushort NumberOfNamedEntries { get; set; }
        public ulong StartOfEntries { get; set; }
        private List<ResourceDirectoryEntry> entries;

        public ResourceDirectory(ulong offset, ulong size) : base(offset, size)
        {
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
        			+ "NumberOfIdEntries: 0x{4:x4}, "
        			+ "NumberOfNamedEntries: 0x{5:x4}",
        			this.Characteristics,
        			this.TimeDateStamp,
        			this.MajorVersion,
        			this.MinorVersion,
        			this.NumberOfIdEntries,
        			this.NumberOfNamedEntries
                );
            }
        }

        public static ulong BaseSize
        {
            get
            {
                return (sizeof(uint) * 2) + (sizeof(ushort) * 4);
            }
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
                return entries;
            }
        }

        public static ResourceDirectory ReadDirectory(BinaryReader reader, DataDirectory dataDirectory, ResourceDirectoryEntry referencing = null, bool noSeek = false)
        {
            ResourceDirectory resourceDirectory;
            var isTopLevel = false;

            if (!noSeek)
            {
                var offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);

                reader.Seek(offset);
            }

            resourceDirectory = new ResourceDirectory((ulong) reader.BaseStream.Position, ResourceDirectory.BaseSize)
            {
        		Characteristics = reader.ReadUInt32(),
        		TimeDateStamp = reader.ReadUInt32(),
        		MajorVersion = reader.ReadUInt16(),
        		MinorVersion = reader.ReadUInt16(),
        		NumberOfNamedEntries = reader.ReadUInt16(),
        		NumberOfIdEntries = reader.ReadUInt16(),
                StartOfEntries = (ulong) reader.BaseStream.Position
            };

            if (referencing != null)
            {
                ImageLayoutEvents.AddReference<ResourceDirectoryEntry, uint>(resourceDirectory, resourceDirectory.Offset, resourceDirectory.Size, referencing, (e) => e.DataPointer);
            }
            else
            {
                isTopLevel = true;
                ImageLayoutEvents.AddRelationship(resourceDirectory, resourceDirectory.Offset, resourceDirectory.Size, dataDirectory);
            }

            resourceDirectory.entries = ResourceDirectoryEntry.ReadEntries(reader, resourceDirectory, dataDirectory, resourceDirectory.NumberOfNamedEntries, resourceDirectory.NumberOfIdEntries, isTopLevel);

            return resourceDirectory;
        }
    }
}
