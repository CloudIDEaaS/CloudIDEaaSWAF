using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using System.Diagnostics;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\DOSHeader.png")]
    public class DOSHeader : IImageLayoutItem
    {
        public ushort MagicBytes { get; set; }
        public ushort BytesOnLastPage { get; set; }
        public ushort PagesInFile { get; set; }
        public ushort Relocations { get; set; }
        public ushort SizeOfHeader { get; set; }
        public ushort MinExtraParagraphs { get; set; }
        public ushort MaxExtraParagraphs { get; set; }
        public ushort InitialSS { get; set; }
        public ushort InitialSP { get; set; }
        public ushort Checksum { get; set; }
        public ushort InitialIP { get; set; }
        public ushort InitialCS { get; set; }
        public ushort RelocTableAddress { get; set; }
        public ushort OverlayNumber { get; set; }
        public ushort Unknown01 { get; set; }
        public ushort Unknown02 { get; set; }
        public ushort Unknown03 { get; set; }
        public ushort Unknown04 { get; set; }
        public ushort OEMIdentifier { get; set; }
        public ushort OEMInfo { get; set; }
        public ushort Unknown05 { get; set; }
        public ushort Unknown06 { get; set; }
        public ushort Unknown07 { get; set; }
        public ushort Unknown08 { get; set; }
        public ushort Unknown09 { get; set; }
        public ushort Unknown10 { get; set; }
        public ushort Unknown11 { get; set; }
        public ushort Unknown12 { get; set; }
        public ushort Unknown13 { get; set; }
        public ushort Unknown14 { get; set; }
        public uint COFFHeaderAddress { get; set; }
        public int Size { get; private set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public static DOSHeader ReadDOSHeader(BinaryReader reader)
        {
            RealModeStub stub;

            reader.Seek(0, SeekOrigin.Begin);

            var header = new DOSHeader
            {
                MagicBytes = reader.ReadUInt16(),
                BytesOnLastPage = reader.ReadUInt16(),
                PagesInFile = reader.ReadUInt16(),
                Relocations = reader.ReadUInt16(),
                SizeOfHeader = reader.ReadUInt16(),
                MinExtraParagraphs = reader.ReadUInt16(),
                MaxExtraParagraphs = reader.ReadUInt16(),
                InitialSS = reader.ReadUInt16(),
                InitialSP = reader.ReadUInt16(),
                Checksum = reader.ReadUInt16(),
                InitialIP = reader.ReadUInt16(),
                InitialCS = reader.ReadUInt16(),
                RelocTableAddress = reader.ReadUInt16(),
                OverlayNumber = reader.ReadUInt16(),
                Unknown01 = reader.ReadUInt16(),
                Unknown02 = reader.ReadUInt16(),
                Unknown03 = reader.ReadUInt16(),
                Unknown04 = reader.ReadUInt16(),
                OEMIdentifier = reader.ReadUInt16(),
                OEMInfo = reader.ReadUInt16(),
                Unknown05 = reader.ReadUInt16(),
                Unknown06 = reader.ReadUInt16(),
                Unknown07 = reader.ReadUInt16(),
                Unknown08 = reader.ReadUInt16(),
                Unknown09 = reader.ReadUInt16(),
                Unknown10 = reader.ReadUInt16(),
                Unknown11 = reader.ReadUInt16(),
                Unknown12 = reader.ReadUInt16(),
                Unknown13 = reader.ReadUInt16(),
                Unknown14 = reader.ReadUInt16(),
                COFFHeaderAddress = reader.ReadUInt32(),
                Size = (int) reader.BaseStream.Position,
                UniqueId = Guid.NewGuid()
            };

            ImageLayoutEvents.AddRelationship(header, 0, (ulong) header.Size);

            stub = RealModeStub.Read(reader, (ulong)(header.COFFHeaderAddress - reader.BaseStream.Position));

            ImageLayoutEvents.AddRelationship(stub, "Real Mode Stub", stub.Offset, stub.Size);

            return header;
        }

        public string Magic
        {
            get
            {
                var bytes = BitConverter.GetBytes(this.MagicBytes);
                var magic = ASCIIEncoding.ASCII.GetString(bytes, 0, sizeof(ushort));

                return magic;
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format(
        			"Magic: {0},\r\n"
        			+ "BytesOnLastPage: 0x{1:x4},\r\n"
        			+ "PagesInFile: 0x{2:x4},\r\n"
        			+ "Relocations: 0x{3:x4},\r\n"
        			+ "SizeOfHeader: 0x{4:x4},\r\n"
        			+ "MinExtraParagraphs: 0x{5:x4},\r\n"
        			+ "MaxExtraParagraphs: 0x{6:x4},\r\n"
        			+ "InitialSS: 0x{7:x4},\r\n"
        			+ "InitialSP: 0x{8:x4},\r\n"
        			+ "Checksum: 0x{9:x4},\r\n"
        			+ "InitialIP: 0x{10:x4},\r\n"
        			+ "InitialCS: 0x{11:x4},\r\n"
        			+ "RelocTableAddress: 0x{12:x4},\r\n"
        			+ "OverlayNumber: 0x{13:x4},\r\n"
        			+ "Unknown01: 0x{14:x4},\r\n"
        			+ "Unknown02: 0x{15:x4},\r\n"
        			+ "Unknown03: 0x{16:x4},\r\n"
        			+ "Unknown04: 0x{17:x4},\r\n"
        			+ "OEMIdentifier: 0x{18:x4},\r\n"
        			+ "OEMInfo: 0x{19:x4},\r\n"
        			+ "Unknown05: 0x{20:x4},\r\n"
        			+ "Unknown06: 0x{21:x4},\r\n"
        			+ "Unknown07: 0x{22:x4},\r\n"
        			+ "Unknown08: 0x{23:x4},\r\n"
        			+ "Unknown09: 0x{24:x4},\r\n"
        			+ "Unknown10: 0x{25:x4},\r\n"
        			+ "Unknown11: 0x{26:x4},\r\n"
        			+ "Unknown12: 0x{27:x4},\r\n"
        			+ "Unknown13: 0x{28:x4},\r\n"
        			+ "Unknown14: 0x{29:x4},\r\n"
        			+ "COFFHeaderAddress: 0x{30:x8}",
        			this.Magic,
        			this.BytesOnLastPage,
        			this.PagesInFile,
        			this.Relocations,
        			this.SizeOfHeader,
        			this.MinExtraParagraphs,
        			this.MaxExtraParagraphs,
        			this.InitialSS,
        			this.InitialSP,
        			this.Checksum,
        			this.InitialIP,
        			this.InitialCS,
        			this.RelocTableAddress,
        			this.OverlayNumber,
        			this.Unknown01,
        			this.Unknown02,
        			this.Unknown03,
        			this.Unknown04,
        			this.OEMIdentifier,
        			this.OEMInfo,
        			this.Unknown05,
        			this.Unknown06,
        			this.Unknown07,
        			this.Unknown08,
        			this.Unknown09,
        			this.Unknown10,
        			this.Unknown11,
        			this.Unknown12,
        			this.Unknown13,
        			this.Unknown14,
        			this.COFFHeaderAddress);
            }
        }
        
        public static int StructureSize
        {
            get
            {
                return TypeExtensions.PrimitiveFieldSizeOf<DOSHeader>();
            }
        }

        public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
