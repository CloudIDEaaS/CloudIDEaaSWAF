using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Utils;
using System.Diagnostics;
using System.ComponentModel;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\PEHeader.png")]
    public class COFFHeader : IImageLayoutItem
    {
        public Machine Machine { get; set; }
        public ushort NumberOfSections { get; set; }
        public uint DateTimeStamp { get; set; }
        public uint PtrToSymbolTable { get; set; }
        public uint NumberOfSymbols { get; set; }
        public ushort SizeOfOptionalHeaders { get; set; }
        public ImageCharacteristics Characteristics { get; set; }
        public List<COFFSection> Sections { get; set; }
        public bool PEHasSectionData { get; private set; }
        public bool PEHasDirectoryData { get; private set; }
        public bool Is64Bit { get; private set; }
        private static BinaryReader reader;
        public int Size { get; private set; }
        public Guid UniqueId { get; private set; }
        [Browsable(false)]
		public ISite Site { get; set; }
        public event EventHandler Disposed;

        public static COFFHeader ReadHeader(BinaryReader reader)
        {
            COFFHeader header;
            COFFHeader.reader = reader;
            bool is64Bit;
            COFFSectionHeaders sectionHeaders = null;

            reader.Seek(0, SeekOrigin.Begin);

            using (var reset = reader.MarkForReset())
            {
                Machine machine;

                machine = (Machine)reader.ReadUInt16();
                is64Bit = machine.Is64Bit();
            }

            if (is64Bit)
            {
                header = new COFFHeader
                {
                    Machine = (Machine) reader.ReadUInt16(),
                    NumberOfSections = reader.ReadUInt16(),
                    DateTimeStamp = reader.ReadUInt32(),
                    PtrToSymbolTable = reader.ReadUInt32(),
                    NumberOfSymbols = reader.ReadUInt32(),
                    SizeOfOptionalHeaders = reader.ReadUInt16(),
                    Characteristics = (ImageCharacteristics)reader.ReadUInt16(),
                    Size = (int)(reader.BaseStream.Position),
                    UniqueId = Guid.NewGuid()
                };
            }
            else
            {
                header = new COFFHeader
                {
                    Machine = (Machine) reader.ReadUInt16(),
                    NumberOfSections = reader.ReadUInt16(),
                    DateTimeStamp = reader.ReadUInt32(),
                    PtrToSymbolTable = reader.ReadUInt32(),
                    NumberOfSymbols = reader.ReadUInt32(),
                    SizeOfOptionalHeaders = reader.ReadUInt16(),
                    Characteristics = (ImageCharacteristics)reader.ReadUInt16(),
                    Size = (int)(reader.BaseStream.Position),
                    UniqueId = Guid.NewGuid()
                };            
            }

            if (header.SizeOfOptionalHeaders > 0)
            {
                Debugger.Break();
            }

            header.Is64Bit = is64Bit;

            ImageLayoutEvents.AddRelationship(header, 0, (ulong)header.Size);

            if (ImageLayoutEvents.HasHandlers)
            {
                header.Sections = ReadSectionsList(header.NumberOfSections, out sectionHeaders);
            }
            else
            {
                header.Sections = ReadSectionsList(header.NumberOfSections);
            }

            header.PEHasSectionData = true;
            header.PEHasDirectoryData = true;

            var x = 0;

            foreach (var section in header.Sections)
            {
                if (section.PointerToRawData == 0 || section.SizeOfRawData == 0)
                {
                    var flags = LayoutFlags.AddressSizeValid;

                    if (section.PointerToRawData == 0)
                    {
                        flags = EnumUtils.SetFlag<LayoutFlags>(flags, LayoutFlags.NoAddress);
                    }

                    if (section.SizeOfRawData == 0)
                    {
                        flags = EnumUtils.SetFlag<LayoutFlags>(flags, LayoutFlags.NoSize);
                    }

                    //ImageLayoutEvents.AddRelationship(section, section.Name, section.PointerToRawData, section.SizeOfRawData, flags);
                }
                else
                {
                    //ImageLayoutEvents.AddRelationship(section, section.Name, section.PointerToRawData, section.SizeOfRawData);

                    if (ImageLayoutEvents.HasHandlers)
                    {
                        var sectionHeader = sectionHeaders[x];
                        ImageLayoutEvents.AddReference(section, sectionHeader, (h) => h.PointerToRawData);
                    }
                }

                x++;
            }

            x = 0;

            return header;
        }

        private static List<DataDirectory> ReadDirectoriesList(uint directoryCount)
        {
            var directories = new List<DataDirectory>((int)directoryCount);
            var dataDirectories = new DataDirectories(directoryCount);

            ImageLayoutEvents.AddRelationship(dataDirectories, (ulong)reader.BaseStream.Position, (directoryCount * (uint)DataDirectory.HeaderSize));

            for (var x = 0; x < directoryCount; x++)
            {
                var directoryHeaderOffset = (ulong)reader.BaseStream.Position;
                DataDirectoryHeader directoryHeader;

                var directory = new DataDirectory
                {
                    Directory = (DirectoryId)x,
                    Address = reader.ReadUInt32(),
                    Size = reader.ReadUInt32()
                };

                directoryHeader = new DataDirectoryHeader(directory);

                ImageLayoutEvents.AddRelationship((IImageLayoutItem)directoryHeader, directoryHeader.Name, directoryHeaderOffset, (ulong)DataDirectory.HeaderSize, (IImageLayoutItem)dataDirectories);

                directories.Add(directory);
            }

            return directories;
        }

        private static List<COFFSection> ReadSectionsList(ushort numberOfSections)
        {
            COFFSectionHeaders sectionHeaders; // not used

            return ReadSectionsList(numberOfSections, out sectionHeaders);
        }

        private static List<COFFSection> ReadSectionsList(ushort numberOfSections, out COFFSectionHeaders sectionHeaders)
        {
            var sections = new List<COFFSection>(numberOfSections);

            if (ImageLayoutEvents.HasHandlers)
            {
                var headersPosition = (ulong)reader.BaseStream.Position;

                sectionHeaders = new COFFSectionHeaders(numberOfSections);

                for (var x = 0; x < numberOfSections; x++)
                {
                    var sectionOffset = (ulong)reader.BaseStream.Position;

                    var section = new COFFSection
                    {
                        Name = reader.ReadNullTermStringLength(8),
                        VirtualSize = reader.ReadUInt32(),
                        VirtualAddress = reader.ReadUInt32(),
                        SizeOfRawData = reader.ReadUInt32(),
                        PointerToRawData = reader.ReadUInt32(),
                        PointerToRelocations = reader.ReadUInt32(),
                        PointerToLinenumbers = reader.ReadUInt32(),
                        NumberOfRelocations = reader.ReadUInt16(),
                        NumberOfLinenumbers = reader.ReadUInt16(),
                        Characteristics = (DataSectionFlags)reader.ReadUInt32(),
                        Size = (ulong) reader.BaseStream.Position - sectionOffset
                    };

                    var sectionHeader = new COFFSectionHeader(section, sectionOffset);

                    sectionHeaders.Add(sectionHeader);
                    sections.Add(section);
                }

                ImageLayoutEvents.AddRelationship(sectionHeaders, headersPosition, (ulong)reader.BaseStream.Position - headersPosition);

                foreach (var sectionHeader in sectionHeaders)
                {
                    ImageLayoutEvents.AddRelationship(sectionHeader, sectionHeader.Offset, sectionHeader.Size, sectionHeaders);
                }
            }
            else
            {
                sectionHeaders = null;

                for (var x = 0; x < numberOfSections; x++)
                {
                    var section = new COFFSection
                    {
                        Name = reader.ReadNullTermStringLength(8),
                        VirtualSize = reader.ReadUInt32(),
                        VirtualAddress = reader.ReadUInt32(),
                        SizeOfRawData = reader.ReadUInt32(),
                        PointerToRawData = reader.ReadUInt32(),
                        PointerToRelocations = reader.ReadUInt32(),
                        PointerToLinenumbers = reader.ReadUInt32(),
                        NumberOfRelocations = reader.ReadUInt16(),
                        NumberOfLinenumbers = reader.ReadUInt16(),
                        Characteristics = (DataSectionFlags)reader.ReadUInt32()
                    };

                    sections.Add(section);
                }
            }

            return sections;
        }

        public void WriteImage(BinaryWriter writer, string fileName)
        {
            writer.Write((UInt16) this.Machine);
            writer.Write((UInt16) this.NumberOfSections);
            writer.Write((UInt32) this.DateTimeStamp);
            writer.Write((UInt32) this.PtrToSymbolTable);
            writer.Write((UInt32) this.NumberOfSymbols);
            writer.Write((UInt16) this.SizeOfOptionalHeaders);
            writer.Write((UInt16) this.Characteristics);
        }

        public string DebugInfo
        {
            get
            {
                if (this.Is64Bit)
                {
                    return string.Format(
                        "Machine: {0},\r\n"
                        + "NumberOfSections: {1},\r\n"
                        + "DateTimeStamp: 0x{2:x8},\r\n"
                        + "PtrToSymbolTable: 0x{3:x8},\r\n"
                        + "NumberOfSymbols: {4},\r\n"
                        + "SizeOfOptionalHeaders: {5:x4},\r\n"
                        + "Characteristics: {6},\r\n"
                        + "Is64Bit: {36}",
                        this.Machine,
                        this.NumberOfSections,
                        this.DateTimeStamp,
                        this.PtrToSymbolTable,
                        this.NumberOfSymbols,
                        this.SizeOfOptionalHeaders,
                        this.Characteristics,
                        this.Is64Bit);
                }
                else
                {
                    return string.Format(
                        "Machine: {0},\r\n"
                        + "NumberOfSections: {1},\r\n"
                        + "DateTimeStamp: 0x{2:x8},\r\n"
                        + "PtrToSymbolTable: 0x{3:x8},\r\n"
                        + "NumberOfSymbols: {4},\r\n"
                        + "SizeOfOptionalHeaders: {5:x4},\r\n"
                        + "Characteristics: {6},\r\n"
                        + "Is64Bit: {37}",
                        this.Machine,
                        this.NumberOfSections,
                        this.DateTimeStamp,
                        this.PtrToSymbolTable,
                        this.NumberOfSymbols,
                        this.SizeOfOptionalHeaders,
                        this.Characteristics,
                        this.Is64Bit);
                }
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
