using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ImportAddressDirectory : DataDirectoryEntry
    {
        private List<ImportAddressEntry> entries;

        public ImportAddressDirectory(ulong offset, ulong size) : base(offset, size)
        {
            entries = new List<ImportAddressEntry>();
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"Count: 0x{0:x8}",
                    this.entries.Count);
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

        internal static List<ImportAddressDirectory> ReadDirectories(BinaryReader reader, DataDirectory dataDirectory)
        {
            var offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);
            var addressDirectories = new List<ImportAddressDirectory>();
            var machine = dataDirectory.Machine;
            var is64bit = machine.Is64Bit();

            reader.Seek(offset);

            do
            {
                var directoryOffset = (ulong) reader.BaseStream.Position;
                var addressDirectory = new ImportAddressDirectory(directoryOffset, 0);

                addressDirectory.entries = ImportAddressEntry.ReadAddressEntries(reader, machine);
                addressDirectory.Size = (ulong) addressDirectory.entries.Sum(e => (long) e.Size);

                addressDirectories.Add(addressDirectory);

                if (addressDirectory.Size == 0)
                {
                    ImageLayoutEvents.AddRelationship(addressDirectory, offset, addressDirectory.Size, dataDirectory, LayoutFlags.NoSize);
                }
                else
                {
                    ImageLayoutEvents.AddRelationship(addressDirectory, offset, addressDirectory.Size, dataDirectory);
                }
            }
            while (((ulong)reader.BaseStream.Position) < offset + dataDirectory.Size);

            return addressDirectories;
        }
    }
}
