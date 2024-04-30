using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class BaseRelocation : DescriptorEntry
    {
        public uint BaseRVA { get; set; }
        public uint SizeOfBlock { get; set; }
        public long StartOfBlock { get; set; }
        private List<RelocationEntry> entries;

        public BaseRelocation() : base(0, 0)
        {
        }

        public BaseRelocation(ulong offset, ulong size) : base(offset, size)
        {
            this.entries = new List<RelocationEntry>();
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                return entries;
            }
        }

        public override string DebugInfo
        {
            get
            {
                return string.Format(
        			"BaseRVA: 0x{0:x8}, "
        			+ "SizeOfBlock: 0x{1:x8}",
        			this.BaseRVA,
        			this.SizeOfBlock);
            }
        }

        public static List<BaseRelocation> ReadRelocations(BinaryReader reader, DataDirectory dataDirectory)
        {
            var offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);
            var baseRelocations = new List<BaseRelocation>();
            BaseRelocation baseRelocation;

            reader.Seek(offset);

            do
            {
                baseRelocation = new BaseRelocation(offset, sizeof(uint) * 2)
                {
                    BaseRVA = reader.ReadUInt32(),
                    SizeOfBlock = reader.ReadUInt32()
                };

                ImageLayoutEvents.AddRelationship(baseRelocation, offset, ((DataDirectoryEntry)baseRelocation).Size, dataDirectory); 

                if (baseRelocation.BaseRVA != 0)
                {
                    var count = (baseRelocation.SizeOfBlock - BaseRelocation.Size) / sizeof(ushort);

                    for (var x = 0; x < count; x++)
                    {
                        var entryOffset = reader.BaseStream.Position;
                        var value = reader.ReadUInt16();
                        var offsetFromBase = (ushort)(value & (ushort)0xfff);
                        var rva = baseRelocation.BaseRVA + offsetFromBase;
                        var type = (RelocationType)((value & (ushort)0xf000) >> 12);

                        var entry = new RelocationEntry((ulong) reader.BaseStream.Position, sizeof(ushort))
                        {
        		            OffsetFromBase = offsetFromBase,
        		            RVA = rva,
        		            Type = type,
                        };

                        baseRelocation.entries.Add(entry);

                        ImageLayoutEvents.AddReference<BaseRelocation, uint>(entry, (ulong) entryOffset, entry.Size, baseRelocation, (b) => b.BaseRVA);
                    }

                    baseRelocations.Add(baseRelocation);
                }
            }
            while (baseRelocation.BaseRVA != 0);

            return baseRelocations;
        }

        public static int Size
        {
            get
            {
                return TypeExtensions.PrimitiveFieldSizeOf<BaseRelocation>() - sizeof(long); // exclude StartOfBlock
            }
        }
    }
}
