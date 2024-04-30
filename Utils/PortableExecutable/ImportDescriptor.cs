using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Utils;
using System.Diagnostics;
using Utils.IO;

namespace Utils.PortableExecutable
{
    public class ImportDescriptor : DescriptorEntry
    {
        public uint ArrayOfImports { get; set; }         // RVA to original unbound IAT (PIMAGE_THUNK_DATA)
        public uint TimeDateStamp { get; set; }
        public uint ForwarderChain { get; set; }
        public uint NameRVA { get; set; }
        public string Name { get; set; }
        public uint FirstThunk { get; set; }
        private List<ImportByName> importsByName;
        private List<ThunkData> thunks;

        public ImportDescriptor(ulong offset, ulong size) : base(offset, size)
        {
            importsByName = new List<ImportByName>();
            thunks = new List<ThunkData>();
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get 
            {
                return importsByName.Cast<DataDirectoryEntry>()
                    .Concat(thunks.Cast<DataDirectoryEntry>());
            }
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
                    "ArrayOfImports: 0x{0:x8}, "
                    + "TimeStamp: 0x{1:x4}, "
                    + "ForwarderChain: 0x{2:x8}, "
                    + "NameRVA: 0x{3:x8}, "
                    + "Name: {4}, "
                    + "FirstThunk: 0x{5:x8}, "
                    + "ImportCount: {6}",
                    this.ArrayOfImports,
                    this.TimeDateStamp,
                    this.ForwarderChain,
                    this.NameRVA,
                    this.Name,
                    this.FirstThunk,
                    this.Children.Count());
            }
        }

        public static List<ImportDescriptor> ReadImports(BinaryReader reader, DataDirectory dataDirectory, IntPtr baseAddress)
        {
            ImportDescriptor descriptorRaw;
            var descriptors = new List<ImportDescriptor>();
            ulong offset;
            var machine = dataDirectory.Machine;
            var is64bit = machine.Is64Bit();

            if (reader is ProcessBinaryReader)
            {
                offset = dataDirectory.Address;
            }
            else
            {
                offset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, dataDirectory.Section);
            }

            reader.Seek( offset);

            do 
            {
                descriptorRaw = new ImportDescriptor((ulong) reader.BaseStream.Position, sizeof(uint) * 5)
                {
                    ArrayOfImports = reader.ReadUInt32(),
                    TimeDateStamp = reader.ReadUInt32(),
                    ForwarderChain = reader.ReadUInt32(),
                    NameRVA = reader.ReadUInt32(),
                    FirstThunk = reader.ReadUInt32(),
                };

                if (descriptorRaw.ArrayOfImports != 0)
                {
                    ImageLayoutEvents.AddRelationship(descriptorRaw, descriptorRaw.Offset, descriptorRaw.Size, dataDirectory);

                    descriptors.Add(descriptorRaw);
                }
            }
            while (descriptorRaw.ArrayOfImports != 0);

            foreach (var descriptor in descriptors)
            {
                ulong nameOffset;
                ulong arrayOfImportsOffset;
                var firstThunkRVA = descriptor.FirstThunk;
                ulong firstThunkOffset;
                ulong importByNameRVA = 0;
                ulong importByNameOffset = 0;

                if (reader is ProcessBinaryReader)
                {
                    nameOffset = descriptor.NameRVA;
                    arrayOfImportsOffset = descriptor.ArrayOfImports;
                }
                else
                {
                    nameOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(descriptor.NameRVA, dataDirectory.Section);
                    arrayOfImportsOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(descriptor.ArrayOfImports, dataDirectory.Section);
                }

                reader.Seek(nameOffset);

                descriptor.Name = reader.ReadNullTermString(IOExtensions.MAX_PATH);

                ImageLayoutEvents.AddReference<string, ImportDescriptor, uint>(descriptor.Name, "Name", nameOffset, (ulong)descriptor.Name.Length, descriptor, (d) => d.NameRVA);

                reader.Seek(arrayOfImportsOffset);

                do
                {
                    if (is64bit)
                    {
                        importByNameRVA = reader.ReadUInt64();
                    }
                    else
                    {
                        importByNameRVA = reader.ReadUInt32();
                    }

                    if (importByNameRVA != 0)
                    {
                        ImportByName importByName;
                        uint ordinal = 0x80000000;

                        if (is64bit && (importByNameRVA & 0x8000000000000000) == 0x8000000000000000)
                        {
                            // is an ordinal as opposed to name

                            ordinal = (uint)(importByNameRVA & 0x7FFFFFFFFFFFFFFF);

                            importByName = ImportByName.Read(reader, ordinal, true);

                            ImageLayoutEvents.AddReference<ImportDescriptor, uint>(importByName, importByName.Offset, importByName.Size, descriptor, (i) => i.ArrayOfImports);
                        }
                        else if ((importByNameRVA & 0x80000000) == 0x80000000)
                        {
                            // is an ordinal as opposed to name
                                                               
                            ordinal = (uint)(importByNameRVA & 0x7FFFFFFF);

                            importByName = ImportByName.Read(reader, ordinal, true);

                            ImageLayoutEvents.AddReference<ImportDescriptor, uint>(importByName, importByName.Offset, importByName.Size, descriptor, (i) => i.ArrayOfImports);
                        }
                        else
                        {
                            if (reader is ProcessBinaryReader)
                            {
                                importByNameOffset = importByNameRVA;
                            }
                            else
                            {
                                importByNameOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(importByNameRVA, dataDirectory.Section);
                            }

                            importByName = ImportByName.Read(reader, importByNameOffset);

                            ImageLayoutEvents.AddReference<ImportDescriptor, uint>(importByName, importByName.Offset, importByName.Size, descriptor, (i) => i.ArrayOfImports);
                        }

                        descriptor.importsByName.Add(importByName);
                    }
                }
                while (importByNameRVA != 0);

                if (firstThunkRVA != 0)
                {
                    var x = 0;
                    ThunkData thunk;

                    if (reader is ProcessBinaryReader)
                    {
                        firstThunkOffset = firstThunkRVA;
                    }
                    else
                    {
                        firstThunkOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(firstThunkRVA, dataDirectory.Section);
                    }

                    reader.Seek(firstThunkOffset);

                    do
                    {
                        if (is64bit)
                        {
                            thunk = new ThunkData(machine, (ulong)reader.BaseStream.Position, sizeof(uint))
                            {
                                RVA = reader.ReadUInt64(),
                            };
                        }
                        else
                        {
                            thunk = new ThunkData(machine, (ulong)reader.BaseStream.Position, sizeof(uint))
                            {
                                RVA = reader.ReadUInt32(),
                            };
                        }

                        if (thunk.RVA != 0)
                        {
                            thunk.ImportByName = descriptor.importsByName.ElementAt(x);

                            ImageLayoutEvents.AddReference<ImportDescriptor, uint>(thunk, thunk.Offset, thunk.Size, descriptor, (i) => i.FirstThunk);

                            descriptor.thunks.Add(thunk);
                        }

                        x++;
                    }
                    while (thunk.RVA != 0);
                }
            }

            return descriptors;
        }
    }
}
