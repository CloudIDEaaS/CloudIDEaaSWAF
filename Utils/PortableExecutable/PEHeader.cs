using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Utils;
using Utils;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using Utils.PortableExecutable.Enums;
using Machine = Utils.PortableExecutable.Enums.Machine;
using Subsystem = Utils.PortableExecutable.Enums.Subsystem;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\PEHeader.png")]
    public class PEHeader : IImageLayoutItem
    {
        public uint SignatureBytes { get; set; }
        public Enums.Machine Machine { get; set; }
        public ushort NumberOfSections { get; set; }
        public uint DateTimeStamp { get; set; }
        public uint PtrToSymbolTable { get; set; }
        public uint NumberOfSymbols { get; set; }
        public ushort SizeOfOptionalHeaders { get; set; }
        public ImageCharacteristics Characteristics { get; set; }
        public OptionalMagic OptionalMagic { get; set; }
        public byte MajorLinkerVersion { get; set; }
        public byte MinorLinkerVersion { get; set; }
        public uint SizeOfCode { get; set; }
        public uint SizeOfInitData { get; set; }
        public uint SizeOfUninitData { get; set; }
        public uint AddressOfEntryPoint { get; set; }
        public uint BaseOfCode { get; set; }
        public ulong BaseOfData { get; set; }
        public ulong ImageBase { get; set; }
        public uint SectionAlignment { get; set; }
        public uint FileAlignment { get; set; }
        public ushort MajorOSVersion { get; set; }
        public ushort MinorOSVersion { get; set; }
        public ushort MajorImageVersion { get; set; }
        public ushort MinorImageVersion { get; set; }
        public ushort MajorSubsystemVersion { get; set; }
        public ushort MinorSubsystemVersion { get; set; }
        public uint Reserved1 { get; set; }
        public uint SizeOfImage { get; set; }
        public uint SizeOfHeaders { get; set; }
        public uint PEChecksum { get; set; }
        public Subsystem Subsystem { get; set; }
        public Enums.DllCharacteristics DLLCharacteristics { get; set; }
        public ulong SizeOfStackReserve { get; set; }
        public ulong SizeOfStackCommit { get; set; }
        public ulong SizeOfHeapReserve { get; set; }
        public ulong SizeOfHeapCommit { get; set; }
        public HeapCreateOptions LoaderFlags { get; set; }
        public uint DirectoryLength { get; set; }
        public List<DataDirectory> Directories { get; set; }
        public List<Section> Sections { get; set; }
        public bool PEHasSectionData { get; private set; }
        public bool PEHasDirectoryData { get; private set; }
        public bool Is64Bit { get; private set; }
        private static BinaryReader reader;
        public int Size { get; private set; }
        public Guid UniqueId { get; private set; }
        [Browsable(false)]
		public ISite Site { get; set; }
        public event EventHandler Disposed;
       
        public static PEHeader ReadPEHeader(BinaryReader reader, uint headerAddress)
        {
            PEHeader header;
            PEHeader.reader = reader;
            bool is64Bit;
            SectionHeaders sectionHeaders = null;

            reader.Seek(headerAddress, SeekOrigin.Begin);

            using (var reset = reader.MarkForReset())
            {
                Machine machine;

                reader.Advance(sizeof(int));

                machine = (Machine)reader.ReadUInt16();
                is64Bit = machine.Is64Bit();
            }

            if (is64Bit)
            {
                header = new PEHeader
                {
                    SignatureBytes = reader.ReadUInt32(),
                    Machine = (Machine) reader.ReadUInt16(),
                    NumberOfSections = reader.ReadUInt16(),
                    DateTimeStamp = reader.ReadUInt32(),
                    PtrToSymbolTable = reader.ReadUInt32(),
                    NumberOfSymbols = reader.ReadUInt32(),
                    SizeOfOptionalHeaders = reader.ReadUInt16(),
                    Characteristics = (ImageCharacteristics) reader.ReadUInt16(),
                    OptionalMagic = (OptionalMagic) reader.ReadUInt16(),
                    MajorLinkerVersion = reader.ReadByte(),
                    MinorLinkerVersion = reader.ReadByte(),
                    SizeOfCode = reader.ReadUInt32(),
                    SizeOfInitData = reader.ReadUInt32(),
                    SizeOfUninitData = reader.ReadUInt32(),
                    AddressOfEntryPoint = reader.ReadUInt32(),
                    BaseOfCode = reader.ReadUInt32(),
                    ImageBase = reader.ReadUInt64(),
                    SectionAlignment = reader.ReadUInt32(),
                    FileAlignment = reader.ReadUInt32(),
                    MajorOSVersion = reader.ReadUInt16(),
                    MinorOSVersion = reader.ReadUInt16(),
                    MajorImageVersion = reader.ReadUInt16(),
                    MinorImageVersion = reader.ReadUInt16(),
                    MajorSubsystemVersion = reader.ReadUInt16(),
                    MinorSubsystemVersion = reader.ReadUInt16(),
                    Reserved1 = reader.ReadUInt32(),
                    SizeOfImage = reader.ReadUInt32(),
                    SizeOfHeaders = reader.ReadUInt32(),
                    PEChecksum = reader.ReadUInt32(),
                    Subsystem = (Subsystem) reader.ReadUInt16(),
                    DLLCharacteristics = (Enums.DllCharacteristics) reader.ReadUInt16(),
                    SizeOfStackReserve = reader.ReadUInt64(),
                    SizeOfStackCommit = reader.ReadUInt64(),
                    SizeOfHeapReserve = reader.ReadUInt64(),
                    SizeOfHeapCommit = reader.ReadUInt64(),
                    LoaderFlags = (HeapCreateOptions) reader.ReadUInt32(),
                    DirectoryLength = reader.ReadUInt32(),
                    Size = (int)(reader.BaseStream.Position - headerAddress),
                    UniqueId = Guid.NewGuid()
                };
            }
            else
            {
                header = new PEHeader
                {
                    SignatureBytes = reader.ReadUInt32(),
                    Machine = (Enums.Machine) reader.ReadUInt16(),
                    NumberOfSections = reader.ReadUInt16(),
                    DateTimeStamp = reader.ReadUInt32(),
                    PtrToSymbolTable = reader.ReadUInt32(),
                    NumberOfSymbols = reader.ReadUInt32(),
                    SizeOfOptionalHeaders = reader.ReadUInt16(),
                    Characteristics = (ImageCharacteristics) reader.ReadUInt16(),
                    OptionalMagic = (OptionalMagic) reader.ReadUInt16(),
                    MajorLinkerVersion = reader.ReadByte(),
                    MinorLinkerVersion = reader.ReadByte(),
                    SizeOfCode = reader.ReadUInt32(),
                    SizeOfInitData = reader.ReadUInt32(),
                    SizeOfUninitData = reader.ReadUInt32(),
                    AddressOfEntryPoint = reader.ReadUInt32(),
                    BaseOfCode = reader.ReadUInt32(),
                    BaseOfData = reader.ReadUInt32(),
                    ImageBase = reader.ReadUInt32(),
                    SectionAlignment = reader.ReadUInt32(),
                    FileAlignment = reader.ReadUInt32(),
                    MajorOSVersion = reader.ReadUInt16(),
                    MinorOSVersion = reader.ReadUInt16(),
                    MajorImageVersion = reader.ReadUInt16(),
                    MinorImageVersion = reader.ReadUInt16(),
                    MajorSubsystemVersion = reader.ReadUInt16(),
                    MinorSubsystemVersion = reader.ReadUInt16(),
                    Reserved1 = reader.ReadUInt32(),
                    SizeOfImage = reader.ReadUInt32(),
                    SizeOfHeaders = reader.ReadUInt32(),
                    PEChecksum = reader.ReadUInt32(),
                    Subsystem = (Subsystem) reader.ReadUInt16(),
                    DLLCharacteristics = (Enums.DllCharacteristics) reader.ReadUInt16(),
                    SizeOfStackReserve = reader.ReadUInt32(),
                    SizeOfStackCommit = reader.ReadUInt32(),
                    SizeOfHeapReserve = reader.ReadUInt32(),
                    SizeOfHeapCommit = reader.ReadUInt32(),
                    LoaderFlags = (HeapCreateOptions) reader.ReadUInt32(),
                    DirectoryLength = reader.ReadUInt32(),
                    Size = (int)(reader.BaseStream.Position - headerAddress),
                    UniqueId = Guid.NewGuid()
                };            
            }

            header.Is64Bit = is64Bit;

            ImageLayoutEvents.AddRelationship(header, headerAddress, (ulong)header.Size);

            header.Directories = ReadDirectoriesList(header.DirectoryLength);

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

                    ImageLayoutEvents.AddRelationship(section, section.Name, section.PointerToRawData, section.SizeOfRawData, flags);
                }
                else
                {
                    ImageLayoutEvents.AddRelationship(section, section.Name, section.PointerToRawData, section.SizeOfRawData);

                    if (ImageLayoutEvents.HasHandlers)
                    {
                        var sectionHeader = sectionHeaders[x];
                        ImageLayoutEvents.AddReference(section, sectionHeader, (h) => h.PointerToRawData);
                    }
                }

                x++;
            }

            x = 0;

            foreach (var directory in header.Directories)
            {
                directory.Machine = header.Machine;

                if (directory.Address != 0 && directory.Size != 0)
                {
                    Section section;
                    ulong directoryFileOffset;

                    section = AddressingUtils.RelativeVirtualAddressToSection(directory.Address, header.Sections);

                    if (section == null && ImageLayoutEvents.HasHandlers)
                    {
                        ImageLayoutEvents.AddRelationship(directory, directory.Name, directory.Address, directory.Size);

                        ImageLayoutEvents.EnumRoots((index, item) =>
                        {
                            // Note that none of this code gets called if events are not hooked up by outside source.  Saves processing time.

                            var directoryList = (DataDirectories)item;

                            ImageLayoutEvents.EnumChildren(directoryList, (index2, item2) =>
                            {
                                var directoryListItem = (DataDirectoryHeader)item2;

                                if (index2 == x)
                                {
                                    ImageLayoutEvents.AddReference(directory, directoryListItem, (i) => i.Address);
                                }
                            });

                        }, (item) => item is DataDirectories);
                    }
                    else
                    {
                        directory.Section = section;

                        if (ImageLayoutEvents.HasHandlers)
                        {
                            directoryFileOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(directory.Address, directory.Section);

                            ImageLayoutEvents.AddRelationship((IImageLayoutItem)directory, directory.Name, directoryFileOffset, directory.Size, (IImageLayoutItem)section);

                            ImageLayoutEvents.EnumRoots((index, item) =>
                            {
                                // Note that none of this code gets called if events are not hooked up by outside source.  Saves processing time.

                                var directoryList = (DataDirectories)item;

                                ImageLayoutEvents.EnumChildren(directoryList, (index2, item2) =>
                                {
                                    var directoryListItem = (DataDirectoryHeader)item2;

                                    if (index2 == x)
                                    {
                                        ImageLayoutEvents.AddReference(directory, directoryListItem, (i) => i.Address);
                                    }
                                });

                            }, (item) => item is DataDirectories);
                        }
                    }
                }

                x++;
            }

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

        private static List<Section> ReadSectionsList(ushort numberOfSections)
        {
            SectionHeaders sectionHeaders; // not used

            return ReadSectionsList(numberOfSections, out sectionHeaders);
        }

        private static List<Section> ReadSectionsList(ushort numberOfSections, out SectionHeaders sectionHeaders)
        {
            var sections = new List<Section>(numberOfSections);

            if (ImageLayoutEvents.HasHandlers)
            {
                var headersPosition = (ulong)reader.BaseStream.Position;

                sectionHeaders = new SectionHeaders(numberOfSections);

                for (var x = 0; x < numberOfSections; x++)
                {
                    var sectionOffset = (ulong)reader.BaseStream.Position;

                    var section = new Section
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

                    var sectionHeader = new SectionHeader(section, sectionOffset);

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
                    var section = new Section
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
            writer.Write((UInt32) this.SignatureBytes);
            writer.Write((UInt16) this.Machine);
            writer.Write((UInt16) this.NumberOfSections);
            writer.Write((UInt32) this.DateTimeStamp);
            writer.Write((UInt32) this.PtrToSymbolTable);
            writer.Write((UInt32) this.NumberOfSymbols);
            writer.Write((UInt16) this.SizeOfOptionalHeaders);
            writer.Write((UInt16) this.Characteristics);
            writer.Write((UInt16) this.OptionalMagic);
            writer.Write((Byte) this.MajorLinkerVersion);
            writer.Write((Byte) this.MinorLinkerVersion);
            writer.Write((UInt32) this.SizeOfCode);
            writer.Write((UInt32) this.SizeOfInitData);
            writer.Write((UInt32) this.SizeOfUninitData);
            writer.Write((UInt32) this.AddressOfEntryPoint);
            writer.Write((UInt32) this.BaseOfCode);
            writer.Write((UInt32) this.BaseOfData);
            writer.Write((UInt32) this.ImageBase);
            writer.Write((UInt32) this.SectionAlignment);
            writer.Write((UInt32) this.FileAlignment);
            writer.Write((UInt16) this.MajorOSVersion);
            writer.Write((UInt16) this.MinorOSVersion);
            writer.Write((UInt16) this.MajorImageVersion);
            writer.Write((UInt16) this.MinorImageVersion);
            writer.Write((UInt16) this.MajorSubsystemVersion);
            writer.Write((UInt16) this.MinorSubsystemVersion);
            writer.Write((UInt32) this.Reserved1);
            writer.Write((UInt32) this.SizeOfImage);
            writer.Write((UInt32) this.SizeOfHeaders);
            writer.Write((UInt32) this.PEChecksum);
            writer.Write((UInt16) this.Subsystem);
            writer.Write((UInt16) this.DLLCharacteristics);
            writer.Write((UInt32) this.SizeOfStackReserve);
            writer.Write((UInt32) this.SizeOfStackCommit);
            writer.Write((UInt32) this.SizeOfHeapReserve);
            writer.Write((UInt32) this.SizeOfHeapCommit);
            writer.Write((UInt32) this.LoaderFlags);
            writer.Write((UInt32) this.DirectoryLength);
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

        public string DebugInfo
        {
            get
            {
                if (this.Is64Bit)
                {
                    return string.Format(
                        "Signature: {0},\r\n"
                        + "Machine: {1},\r\n"
                        + "NumberOfSections: {2},\r\n"
                        + "DateTimeStamp: 0x{3:x8},\r\n"
                        + "PtrToSymbolTable: 0x{4:x8},\r\n"
                        + "NumberOfSymbols: {5},\r\n"
                        + "SizeOfOptionalHeaders: {6:x4},\r\n"
                        + "Characteristics: {7},\r\n"
                        + "OptionalMagic: {8},\r\n"
                        + "MajorLinkerVersion: 0x{9:x1},\r\n"
                        + "MinorLinkerVersion: 0x{10:x1},\r\n"
                        + "SizeOfCode: 0x{11:x8},\r\n"
                        + "SizeOfInitData: 0x{12:x8},\r\n"
                        + "SizeOfUninitData: 0x{13:x8},\r\n"
                        + "AddressOfEntryPoint: 0x{14:x8},\r\n"
                        + "BaseOfCode: 0x{15:x8},\r\n"
                        + "ImageBase: 0x{16:x16},\r\n"
                        + "SectionAlignment: 0x{17:x8},\r\n"
                        + "FileAlignment: 0x{18:x8},\r\n"
                        + "MajorOSVersion: 0x{19:x4},\r\n"
                        + "MinorOSVersion: 0x{20:x4},\r\n"
                        + "MajorImageVersion: 0x{21:x4},\r\n"
                        + "MinorImageVersion: 0x{22:x4},\r\n"
                        + "MajorSubsystemVersion: 0x{23:x4},\r\n"
                        + "MinorSubsystemVersion: 0x{24:x4},\r\n"
                        + "Reserved1: 0x{25:x8},\r\n"
                        + "SizeOfImage: 0x{26:x8},\r\n"
                        + "SizeOfHeaders: 0x{27:x8},\r\n"
                        + "PEChecksum: 0x{28:x8},\r\n"
                        + "Subsystem: {29},\r\n"
                        + "DLLCharacteristics: {30},\r\n"
                        + "SizeOfStackReserve: 0x{31:x16},\r\n"
                        + "SizeOfStackCommit: 0x{32:x16},\r\n"
                        + "SizeOfHeapReserve: 0x{33:x16},\r\n"
                        + "SizeOfHeapCommit: 0x{34:x16},\r\n"
                        + "LoaderFlags: {35},\r\n"
                        + "DirectoryLength: 0x{36:x8},\r\n"
                        + "Is64Bit: {37}",
                        this.Signature.Replace('\0', '.'),
                        this.Machine,
                        this.NumberOfSections,
                        this.DateTimeStamp,
                        this.PtrToSymbolTable,
                        this.NumberOfSymbols,
                        this.SizeOfOptionalHeaders,
                        this.Characteristics,
                        this.OptionalMagic,
                        this.MajorLinkerVersion,
                        this.MinorLinkerVersion,
                        this.SizeOfCode,
                        this.SizeOfInitData,
                        this.SizeOfUninitData,
                        this.AddressOfEntryPoint,
                        this.BaseOfCode,
                        this.ImageBase,
                        this.SectionAlignment,
                        this.FileAlignment,
                        this.MajorOSVersion,
                        this.MinorOSVersion,
                        this.MajorImageVersion,
                        this.MinorImageVersion,
                        this.MajorSubsystemVersion,
                        this.MinorSubsystemVersion,
                        this.Reserved1,
                        this.SizeOfImage,
                        this.SizeOfHeaders,
                        this.PEChecksum,
                        this.Subsystem,
                        this.DLLCharacteristics,
                        this.SizeOfStackReserve,
                        this.SizeOfStackCommit,
                        this.SizeOfHeapReserve,
                        this.SizeOfHeapCommit,
                        this.LoaderFlags,
                        this.DirectoryLength,
                        this.Is64Bit);
                }
                else
                {
                    return string.Format(
                        "Signature: {0},\r\n"
                        + "Machine: {1},\r\n"
                        + "NumberOfSections: {2},\r\n"
                        + "DateTimeStamp: 0x{3:x8},\r\n"
                        + "PtrToSymbolTable: 0x{4:x8},\r\n"
                        + "NumberOfSymbols: {5},\r\n"
                        + "SizeOfOptionalHeaders: {6:x4},\r\n"
                        + "Characteristics: {7},\r\n"
                        + "OptionalMagic: {8},\r\n"
                        + "MajorLinkerVersion: 0x{9:x1},\r\n"
                        + "MinorLinkerVersion: 0x{10:x1},\r\n"
                        + "SizeOfCode: 0x{11:x8},\r\n"
                        + "SizeOfInitData: 0x{12:x8},\r\n"
                        + "SizeOfUninitData: 0x{13:x8},\r\n"
                        + "AddressOfEntryPoint: 0x{14:x8},\r\n"
                        + "BaseOfCode: 0x{15:x8},\r\n"
                        + "BaseOfData: 0x{16:x8},\r\n"
                        + "ImageBase: 0x{17:x8},\r\n"
                        + "SectionAlignment: 0x{18:x8},\r\n"
                        + "FileAlignment: 0x{19:x8},\r\n"
                        + "MajorOSVersion: 0x{20:x4},\r\n"
                        + "MinorOSVersion: 0x{21:x4},\r\n"
                        + "MajorImageVersion: 0x{22:x4},\r\n"
                        + "MinorImageVersion: 0x{23:x4},\r\n"
                        + "MajorSubsystemVersion: 0x{24:x4},\r\n"
                        + "MinorSubsystemVersion: 0x{25:x4},\r\n"
                        + "Reserved1: 0x{26:x8},\r\n"
                        + "SizeOfImage: 0x{27:x8},\r\n"
                        + "SizeOfHeaders: 0x{28:x8},\r\n"
                        + "PEChecksum: 0x{29:x8},\r\n"
                        + "Subsystem: {30},\r\n"
                        + "DLLCharacteristics: {31},\r\n"
                        + "SizeOfStackReserve: 0x{32:x8},\r\n"
                        + "SizeOfStackCommit: 0x{33:x8},\r\n"
                        + "SizeOfHeapReserve: 0x{34:x8},\r\n"
                        + "SizeOfHeapCommit: 0x{35:x8},\r\n"
                        + "LoaderFlags: {36},\r\n"
                        + "DirectoryLength: 0x{37:x8},\r\n"
                        + "Is64Bit: {38}",
                        this.Signature.Replace('\0', '.'),
                        this.Machine,
                        this.NumberOfSections,
                        this.DateTimeStamp,
                        this.PtrToSymbolTable,
                        this.NumberOfSymbols,
                        this.SizeOfOptionalHeaders,
                        this.Characteristics,
                        this.OptionalMagic,
                        this.MajorLinkerVersion,
                        this.MinorLinkerVersion,
                        this.SizeOfCode,
                        this.SizeOfInitData,
                        this.SizeOfUninitData,
                        this.AddressOfEntryPoint,
                        this.BaseOfCode,
                        this.BaseOfData,
                        this.ImageBase,
                        this.SectionAlignment,
                        this.FileAlignment,
                        this.MajorOSVersion,
                        this.MinorOSVersion,
                        this.MajorImageVersion,
                        this.MinorImageVersion,
                        this.MajorSubsystemVersion,
                        this.MinorSubsystemVersion,
                        this.Reserved1,
                        this.SizeOfImage,
                        this.SizeOfHeaders,
                        this.PEChecksum,
                        this.Subsystem,
                        this.DLLCharacteristics,
                        this.SizeOfStackReserve,
                        this.SizeOfStackCommit,
                        this.SizeOfHeapReserve,
                        this.SizeOfHeapCommit,
                        this.LoaderFlags,
                        this.DirectoryLength,
                        this.Is64Bit);
                }
            }
        }

        public static int StructureSize
        {
            get
            {
                return TypeExtensions.PrimitiveFieldSizeOf<PEHeader>();
            }
        }

        public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
