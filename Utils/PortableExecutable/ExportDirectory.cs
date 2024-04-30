using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.IO;

namespace Utils.PortableExecutable
{
    public class ExportDirectory : DescriptorEntry
    {
        public uint Characteristics { get; set; }
        public uint TimeDateStamp { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public uint NameRVA { get; set; }
        public string Name { get; set; }
        public uint Base { get; set; }
        public uint NumberOfFunctions { get; set; }
        public uint NumberOfNames { get; set; }
        public uint AddressOfFunctions { get; set; }
        public uint AddressOfNames { get; set; }
        public uint AddressOfNameOrdinals { get; set; }
        private Dictionary<int, Tuple<uint, uint>> functions;
        private Dictionary<int, Tuple<uint, uint, string>> names;
        private Dictionary<int, Tuple<ushort, uint>> ordinals;
        private List<ExportData> exportData;

        public ExportDirectory(ulong offset, ulong size) : base(offset, size)
        {
            functions = new Dictionary<int, Tuple<uint, uint>>();
            names = new Dictionary<int, Tuple<uint, uint, string>>();
            ordinals = new Dictionary<int, Tuple<ushort, uint>>();
            exportData = new List<ExportData>();
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                var exports = new List<ExportData>();

                foreach (var pair in functions)
                {
                    var index = pair.Key;
                    var functionTuple = pair.Value;
                    var nameTuple = names[index];
                    var ordinalTuple = ordinals[index];

                    var exportData = new ExportData(0, 0)
                    {
                        AddressOfFunction = functionTuple.Item1,
                        AddressOfName = (nameTuple == null) ? null : (uint?) nameTuple.Item1,
                        Name = (nameTuple == null) ? null : nameTuple.Item3,
                        NameOrdinal = (ordinalTuple == null) ? null : (ushort?) ordinalTuple.Item1,
                        Ordinal = (ordinalTuple == null) ? null : (uint?)ordinalTuple.Item2,
                    };

                    exports.Add(exportData);
                }

                return exports.OrderBy(e => e.Ordinal);
            }
        }

        public override string DebugInfo
        {
            get
            {
                return string.Format(
                    "Characteristics: 0x{0:x8}, "
                    + "TimeDateStamp: 0x{1:x8}, "
                    + "MajorVersion: 0x{2:x4}, "
                    + "MinorVersion: 0x{3:x4}, "
                    + "NameRVA: 0x{4:x8}, "
                    + "Name: {5}, "
                    + "Base: 0x{6:x8}, "
                    + "NumberOfFunctions: {7}, "
                    + "NumberOfNames: {8}, "
                    + "AddressOfFunctions: 0x{9:x8}, "
                    + "AddressOfNames: 0x{10:x8}, "
                    + "AddressOfNameOrdinals: 0x{11:x8}",
                    this.Characteristics,
                    this.TimeDateStamp,
                    this.MajorVersion,
                    this.MinorVersion,
                    this.NameRVA,
                    this.Name,
                    this.Base,
                    this.NumberOfFunctions,
                    this.NumberOfNames,
                    this.AddressOfFunctions,
                    this.AddressOfNames,
                    this.AddressOfNameOrdinals);

            }
        }

        internal static List<ExportDirectory> ReadExports(BinaryReader reader, DataDirectory dataDirectory, IntPtr baseAddress)
        {
            ExportDirectory directory;
            var directories = new List<ExportDirectory>();
            ulong offset;
            ulong nameOffset;
            ulong addressOfFunctionsOffset;
            ulong addressOfNamesOffset;
            ulong addressOfNameOrdinalsOffset;
            uint addressOfFunction;
            uint addressOfName;
            uint ordinal;
            var y = 0;

            if (reader is ProcessBinaryReader)
            {
                offset = dataDirectory.Address;
            }
            else
            {
                offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);
            }

            reader.Seek(offset);

            // there's only one

            directory = new ExportDirectory(offset, (sizeof(uint) * 9) * (sizeof(ushort) *2))
            {
                Characteristics = reader.ReadUInt32(),
                TimeDateStamp = reader.ReadUInt32(),
                MajorVersion = reader.ReadUInt16(),
                MinorVersion = reader.ReadUInt16(),
                NameRVA = reader.ReadUInt32(),
                Base = reader.ReadUInt32(),
                NumberOfFunctions = reader.ReadUInt32(),
                NumberOfNames = reader.ReadUInt32(),
                AddressOfFunctions = reader.ReadUInt32(),
                AddressOfNames = reader.ReadUInt32(),
                AddressOfNameOrdinals = reader.ReadUInt32(),
            };

            ImageLayoutEvents.AddRelationship(directory, offset, directory.Size, dataDirectory); 

            directories.Add(directory);

            if (reader is ProcessBinaryReader)
            {
                nameOffset = directory.NameRVA;
                addressOfFunctionsOffset = directory.AddressOfFunctions;
                addressOfNamesOffset = directory.AddressOfNames;
                addressOfNameOrdinalsOffset = directory.AddressOfNameOrdinals;
            }
            else
            {
                nameOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(directory.NameRVA, dataDirectory.Section);
                addressOfFunctionsOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(directory.AddressOfFunctions, dataDirectory.Section);
                addressOfNamesOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(directory.AddressOfNames, dataDirectory.Section);
                addressOfNameOrdinalsOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(directory.AddressOfNameOrdinals, dataDirectory.Section);
            }

            reader.Seek(nameOffset);

            directory.Name = reader.ReadNullTermString(IOExtensions.MAX_PATH);

            ImageLayoutEvents.AddReference<string, ExportDirectory, uint>(directory.Name, "Name", nameOffset, (ulong)directory.Name.Length, directory, (d) => d.NameRVA);

            reader.Seek(addressOfFunctionsOffset);

            for (var x = 0; x < directory.NumberOfFunctions; x++)
            {
                Tuple<uint, uint> tuple;
                ulong functionOffset;

                addressOfFunction = reader.ReadUInt32();

                if (reader is ProcessBinaryReader)
                {
                    functionOffset = addressOfFunction;
                }
                else
                {
                    functionOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(addressOfFunction, dataDirectory.Section);
                }

                tuple = new Tuple<uint, uint>(addressOfFunction, (uint)functionOffset);

                ImageLayoutEvents.AddReference<Tuple<uint, uint>, ExportDirectory, uint>(tuple, "Function", functionOffset, (ulong)sizeof(uint), directory, (d) => d.AddressOfFunctions, "AddressOfFunction", "FunctionOffset");

                directory.functions.Add(x, tuple);
            }

            reader.Seek(addressOfNamesOffset);

            for (var x = 0; x < directory.NumberOfFunctions; x++)
            {
                if (x >= (int)(directory.NumberOfFunctions - directory.NumberOfNames))
                {
                    string name;
                    Tuple<uint, uint, string> tuple;

                    addressOfName = reader.ReadUInt32();

                    if (reader is ProcessBinaryReader)
                    {
                        nameOffset = addressOfName;
                    }
                    else
                    {
                        nameOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(addressOfName, dataDirectory.Section);
                    }

                    using (var reset = reader.MarkForReset())
                    {
                        reader.BaseStream.Seek((int)nameOffset, SeekOrigin.Begin);

                        name = reader.ReadNullTermString(IOExtensions.MAX_PATH);
                    }

                    tuple = new Tuple<uint, uint, string>(addressOfName, (uint)nameOffset, name);

                    ImageLayoutEvents.AddReference<Tuple<uint, uint, string>, ExportDirectory, uint>(tuple, "NameOffset", nameOffset, (ulong)sizeof(uint), directory, (d) => d.AddressOfNames, "AddressOfName", "NameOffset", "Name");

                    ImageLayoutEvents.EnumReferences(directory, (index, reference) =>
                    {
                        if (index == y)
                        {
                            var referenced = reference.Referenced.Item;

                            ImageLayoutEvents.AddReference(name, "Name", nameOffset, (ulong)name.Length, reference.Referenced, "NameOffset"); 
                        }

                    }, (reference) => reference.Referenced.Name == "NameOffset");

                    directory.names.Add(x, tuple);

                    y++;
                }
                else
                {
                    directory.names.Add(x, null);
                }
            }

            reader.Seek(addressOfNameOrdinalsOffset);

            for (var x = 0; x < directory.NumberOfFunctions; x++)
            {
                if (x >= (int)(directory.NumberOfFunctions - directory.NumberOfNames))
                {
                    Tuple<ushort, uint> tuple;
                    var nameOrdinal = reader.ReadUInt16();
                    ordinal = directory.Base + nameOrdinal;

                    tuple = new Tuple<ushort, uint>(nameOrdinal, ordinal);

                    ImageLayoutEvents.AddReference<Tuple<ushort, uint>, ExportDirectory, uint>(tuple, "NameOrdinal", addressOfNameOrdinalsOffset, (ulong)sizeof(uint), directory, (d) => d.AddressOfNameOrdinals, "NameOrdinal", "Ordinal");

                    directory.ordinals.Add(x, tuple);
                }
                else
                {
                    directory.ordinals.Add(x, null);
                }
            }

            return directories;
        }
    }
}
