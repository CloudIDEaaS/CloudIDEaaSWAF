using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class CodeViewHeader : DataDirectoryEntry
    {
        public uint SignatureBytes { get; set; }
        public uint Offset { get; set; }

        public CodeViewHeader(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"SignatureBytes: 0x{0:x8}, "
                    + "Signature: {1}, "
        			+ "Offset: 0x{2:x8}",
        			this.SignatureBytes,
                    this.Signature,
        			this.Offset);
            }
        }

        public string Signature
        {
            get
            {
                var bytes = BitConverter.GetBytes(this.SignatureBytes);
                var signature = ASCIIEncoding.ASCII.GetString(bytes, 0, sizeof(uint));

                return signature;
            }
        }

        public static CodeViewHeader Read(BinaryReader reader)
        {
            CodeViewHeader header;

            using (var reset = reader.MarkForReset())
            {
                header = new CodeViewHeader((ulong)reader.BaseStream.Position, (ulong)sizeof(uint) * 2)
                {
                    SignatureBytes = reader.ReadUInt32(),
                    Offset = reader.ReadUInt32(),
                };
            }

            switch (header.Signature)
            {
                case "RSDS":
                    header = CodeViewPDBHeader.Read(reader);
                    break;
            }

            return header;
        }
    }
}
