using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class CodeViewPDBHeader : CodeViewHeader
    {
        Guid GuidSignature { get; set; }
        public uint Age { get; set; }
        string PdbFileName { get; set; }

        public CodeViewPDBHeader(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get
            {
                return string.Format(
                    "SignatureBytes: 0x{0:x8}, "
                    + "Signature: {1}, "
                    + "GuidSignature: {2}, "
                    + "Age: 0x{3:x8}, "
                    + "PdbFileName: {4}",
                    this.SignatureBytes,
                    this.Signature,
                    this.GuidSignature,
                    this.Age,
                    this.PdbFileName);
            }
        }

        public static new CodeViewHeader Read(BinaryReader reader)
        {
            var header = new CodeViewPDBHeader((ulong)reader.BaseStream.Position, (sizeof(uint) * 2) + sizeof(ushort))
            {
                SignatureBytes = reader.ReadUInt32(),
                GuidSignature = new Guid(reader.ReadBytes(16)),
                Age = reader.ReadUInt32(),
                PdbFileName = reader.ReadNullTermString(IOExtensions.MAX_PATH)
            };

            header.Size += (ulong) header.PdbFileName.Length;

            return header;
        }
    }
}
