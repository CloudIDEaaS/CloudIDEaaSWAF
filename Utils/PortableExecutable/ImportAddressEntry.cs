using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ImportAddressEntry : DataDirectoryEntry
    {
        public ulong Address { get; set; }
        private bool is64Bit;

        public ImportAddressEntry(Machine machine, ulong offset, ulong size) : base(offset, size)
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
                        "Address: 0x{0:x16}",
                        this.Address);
                }
                else
                {
                    return string.Format(
                        "Address: 0x{0:x8}",
                        this.Address);
                }
            }
        }

        internal static List<ImportAddressEntry> ReadAddressEntries(BinaryReader reader, Machine machine)
        {
            var addressEntries = new List<ImportAddressEntry>();
            ImportAddressEntry addressEntry;
            var is64bit = machine.Is64Bit();

            do
            {
                var offset = (ulong)reader.BaseStream.Position;

                if (is64bit)
                {
                    addressEntry = new ImportAddressEntry(machine, offset, sizeof(uint))
                    {
                        Address = reader.ReadUInt64(),
                    };
                }
                else
                {
                    addressEntry = new ImportAddressEntry(machine, offset, sizeof(uint))
                    {
                        Address = reader.ReadUInt32(),
                    };
                }

                if (addressEntry.Address != 0)
                {
                    addressEntries.Add(addressEntry);
                }
            }
            while (addressEntry.Address != 0);

            return addressEntries;
        }
    }
}
