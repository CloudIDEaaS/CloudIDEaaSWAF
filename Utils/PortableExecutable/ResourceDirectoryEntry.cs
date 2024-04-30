using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ResourceDirectoryEntry : ResourceEntry
    {
        public uint NameId { get; set; }
        public ResourceId ResourceId { get; set; }
        public string Name { get; set; }
        public uint DataPointer { get; set; }
        private List<ResourceDirectory> directories;
        private List<ResourceDataEntry> data;

        public ResourceDirectoryEntry(ulong offset, ulong size) : base(offset, size)
        {
            directories = new List<ResourceDirectory>();
            data = new List<ResourceDataEntry>();
        }

        public string AKA
        {
            get
            {
                if (Name != null)
                {
                    return Name;
                }
                else
                {
                    return ResourceId.ToString();
                }
            }
        }

        public string IDString
        {
            get
            {
                if (Name != null)
                {
                    return Name;
                }
                else
                {
                    return string.Format("Resource ID: {0}", ((int) ResourceId).ToString());
                }
            }
        }

        public override bool HasChildren
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                return directories.Cast<DataDirectoryEntry>().Concat(data);
            }
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"NameId: 0x{0:x8}, "
                    + "Name: {1}, "
        			+ "ResourceId: {2}, "
        			+ "DataPointer: 0x{3:x8}",
        			this.NameId,
                    this.Name.AsDisplayText(),
        			this.ResourceId,
        			this.DataPointer
                );
            }
        }

        public static List<ResourceDirectoryEntry> ReadEntries(BinaryReader reader, ResourceDirectory parent, DataDirectory dataDirectory, int namedEntryCount, int idEntryCount, bool isTopLevel = false)
        {
            var entries = new List<ResourceDirectoryEntry>();

            for (var x = 0; x < (namedEntryCount + idEntryCount); x++)
            {
                var entry = new ResourceDirectoryEntry((ulong)reader.BaseStream.Position, ResourceDirectoryEntry.StructSize)
                {
                    NameId = reader.ReadUInt32(),
                    DataPointer = reader.ReadUInt32()
                };

                if ((entry.NameId & 0x80000000) == 0x80000000)
                {
                    // is a name

                    var sectionOffset = (uint)(entry.NameId & 0x7FFFFFFF);
                    var nameOffset = AddressingUtils.SectionToFileOffset(sectionOffset, dataDirectory.Section);

                    using (var reset = reader.MarkForReset())
                    {
                        reader.Seek(nameOffset);
                        entry.Name = reader.ReadUnicodeString();
                    }

                    ImageLayoutEvents.AddReference<ResourceDirectory, ulong>(entry, entry.Name, entry.Offset, entry.Size, parent, (d) => d.StartOfEntries);
                    ImageLayoutEvents.AddReference<string, ResourceDirectoryEntry, uint>(entry.Name, "Name", nameOffset, (ulong) entry.Name.Length * 2, entry, (e) => e.NameId);
                }   
                else
                {
                    // is a resource id

                    entry.ResourceId = (ResourceId) entry.NameId;

                    if (isTopLevel)
                    {
                        ImageLayoutEvents.AddReference<ResourceDirectory, ulong>(entry, entry.AKA, entry.Offset, entry.Size, parent, (d) => d.StartOfEntries);
                    }
                    else
                    {
                        ImageLayoutEvents.AddReference<ResourceDirectory, ulong>(entry, entry.IDString, entry.Offset, entry.Size, parent, (d) => d.StartOfEntries);
                    }
                }

                if ((entry.DataPointer & 0x80000000) == 0x80000000)
                {
                    // is a directory

                    ResourceDirectory directory;

                    var sectionOffset = (uint)(entry.DataPointer & 0x7FFFFFFF);
                    var subdirectoryOffset = AddressingUtils.SectionToFileOffset(sectionOffset, dataDirectory.Section);

                    using (var reset = reader.MarkForReset())
                    {
                        reader.Seek(subdirectoryOffset);

                        directory = ResourceDirectory.ReadDirectory(reader, dataDirectory, entry, true);

                        entry.directories.Add(directory);
                    }
                }
                else
                {
                    // is data

                    var sectionOffset = (uint)entry.DataPointer;
                    var dataOffset = AddressingUtils.SectionToFileOffset(sectionOffset, dataDirectory.Section);

                    using (var reset = reader.MarkForReset())
                    {
                        ResourceDataEntry dataEntry;
                        ulong dataEntryDataOffset;

                        reader.Seek(dataOffset);
                        
                        dataEntry = ResourceDataEntry.ReadData(reader, dataDirectory);

                        entry.data.Add(dataEntry);

                        ImageLayoutEvents.AddReference<ResourceDirectoryEntry, uint>(dataEntry, dataOffset, dataEntry.Size, entry, i => i.DataPointer);

                        dataEntryDataOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataEntry.DataRVA, dataDirectory.Section);

                        ImageLayoutEvents.AddReference<byte[], ResourceDataEntry, uint>(dataEntry.Data, "Data", dataEntryDataOffset, (ulong)dataEntry.Data.Length, dataEntry, i => i.DataRVA);
                    }
                }

                entries.Add(entry);
            }

            return entries;
        }

        public static ulong StructSize 
        { 
            get
            {
                return sizeof(uint) * 2;
            }
        }
    }
}
