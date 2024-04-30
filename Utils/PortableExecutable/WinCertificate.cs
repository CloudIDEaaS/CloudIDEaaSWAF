using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class WinCertificate : DataDirectoryEntry
    {
        public uint Length { get; set; }
        public ushort Revision { get; set; }
        public CertificateType CertificateType { get; set; }
        public byte[] Certificate { get; set; }

        public WinCertificate(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"Length: 0x{0:x8}, "
        			+ "Revision: 0x{1:x4}, "
        			+ "CertificateType: {2}, "
                    + "Certificate: {3}",
        			this.Length,
        			this.Revision,
        			this.CertificateType,
                    this.Certificate.AsDisplayText(25)
                );
            }
        }

        internal static List<WinCertificate> ReadCertificates(BinaryReader reader, DataDirectory dataDirectory)
        {
            var certificates = new List<WinCertificate>();
            var offset = dataDirectory.Address;
            WinCertificate certificate;

            reader.Seek(offset);

            do
            {
                var directoryOffset = reader.BaseStream.Position;

                certificate = new WinCertificate(offset, sizeof(uint) + (sizeof(ushort) * 2))
                {
                    Length = reader.ReadUInt32(),
                    Revision = reader.ReadUInt16(),
                    CertificateType = (CertificateType) reader.ReadUInt16()
                };

                certificate.Certificate = reader.ReadBytes((int) certificate.Length);
                certificate.Size += (ulong) certificate.Certificate.Length;

                certificates.Add(certificate);

                ImageLayoutEvents.AddRelationship(certificate, (ulong)directoryOffset, certificate.Size, dataDirectory); 
            }
            while (((ulong)reader.BaseStream.Position) < offset + dataDirectory.Size);

            return certificates;
        }
    }
}
