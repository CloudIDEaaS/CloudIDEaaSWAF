using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class ResourceDataEntry : ResourceEntry
    {
        public uint DataRVA { get; set; }
        public uint CodePage { get; set; }
        public uint Reserved { get; set; }
        public byte[] Data { get; set; }

        public ResourceDataEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"DataRVA: 0x{0:x8}, "
        			+ "Size: 0x{1:x8}, "
        			+ "CodePage: 0x{2:x8}, "
        			+ "Reserved: 0x{3:x8}, "
                    + "Data {4}",
        			this.DataRVA,
        			this.Size,
        			this.CodePage,
        			this.Reserved,
                    this.Data.AsDisplayText(25)
                );
            }
        }

        public static ResourceDataEntry ReadData(BinaryReader reader, DataDirectory dataDirectory)
        {
            var entry = new ResourceDataEntry((ulong) reader.BaseStream.Position, 0)
            {
        		DataRVA = reader.ReadUInt32(),
                Size = reader.ReadUInt32(),
                CodePage = reader.ReadUInt32(),
                Reserved = reader.ReadUInt32()
            };

            using (var reset = reader.MarkForReset())
            {
                var dataOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(entry.DataRVA, dataDirectory.Section);

                reader.Seek(dataOffset);

                entry.Data = reader.ReadBytes((int) entry.Size);
            }

            return entry;
        }
    }
}
