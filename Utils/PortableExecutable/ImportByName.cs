using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class ImportByName : DataDirectoryEntry
    {
        private bool isOrdinal;
        public ushort Hint { get; set; }
        public string Name { get; set; }
        public uint Ordinal { get; set; }

        public ImportByName(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                if (isOrdinal)
                {
                    return string.Format(
                        "Ordinal: 0x{0:x4}",
                        this.Ordinal);
                }
                else
                {
                    return string.Format(
                        "Hint: 0x{0:x4}, "
                        + "Name: {1}",
                        this.Hint,
                        this.Name);
                }
            }
        }

        public static ImportByName Read(BinaryReader reader, ulong importByNameOffset, bool isOrdinal = false)
        {
            ImportByName importByName;

            if (isOrdinal)
            {
                importByName = new ImportByName((ulong)reader.BaseStream.Position, (ulong)sizeof(uint))
                {
                    Ordinal = (uint) importByNameOffset,
                    isOrdinal = true
                };
            }
            else
            {
                using (var reset = reader.MarkForReset())
                {
                    reader.Seek(importByNameOffset);

                    importByName = new ImportByName((ulong)reader.BaseStream.Position, (ulong)sizeof(ushort))
                    {
                        Hint = reader.ReadUInt16(),
                        Name = reader.ReadNullTermString(IOExtensions.MAX_PATH)
                    };

                    importByName.Size += (ulong) importByName.Name.Length;
                }
            }

            return importByName;
        }
    }
}
