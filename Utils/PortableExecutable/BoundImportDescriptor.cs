using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class BoundImportDescriptor : DescriptorEntry
    {
        public uint TimeDateStamp { get; set; }
        public ushort OffsetModuleName { get; set; }
        public string ModuleName { get; set; }
        public ushort NumberOfModuleForwarderRefs { get; set; }
        public ulong StartOfModuleForwarderRefs { get; set; }
        private List<BoundForwarderReference> references;

        public BoundImportDescriptor(ulong offset, ulong size) : base(offset, size)
        {
            references = new List<BoundForwarderReference>();
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"TimeDateStamp: 0x{0:x8}, "
        			+ "OffsetModuleName: 0x{1:x4}, "
                    + "ModuleName: {2},"
        			+ "NumberOfModuleForwarderRefs: 0x{3:x4}",
        			this.TimeDateStamp,
        			this.OffsetModuleName,
                    this.ModuleName,
        			this.NumberOfModuleForwarderRefs);
            }
        }

        public override bool HasChildren
        {
            get
            {
                return references.Count > 0;
            }
        }

        public override IEnumerable<DataDirectoryEntry> Children
        {
            get 
            {
                return references;
            }
        }

        public static List<BoundImportDescriptor> ReadImports(BinaryReader reader, DataDirectory dataDirectory)
        {
            var offset = dataDirectory.Address;
            var descriptors = new List<BoundImportDescriptor>();
            BoundImportDescriptor descriptor;

            reader.Seek(offset);

            do
            {
                var descriptorOffset = reader.BaseStream.Position;

                descriptor = new BoundImportDescriptor((ulong) descriptorOffset, (sizeof(uint) * 1) + (sizeof(ushort) * 2))
                {
        		    TimeDateStamp = reader.ReadUInt32(),
        		    OffsetModuleName = reader.ReadUInt16(),
        		    NumberOfModuleForwarderRefs = reader.ReadUInt16(),
                    StartOfModuleForwarderRefs = (ulong)reader.BaseStream.Position
                };

                if (descriptor.TimeDateStamp != 0)
                {
                    ImageLayoutEvents.AddRelationship(descriptor, offset, ((DataDirectoryEntry)descriptor).Size, dataDirectory);

                    using (var reset = reader.MarkForReset())
                    {
                        var nameOffset = (ulong)offset + descriptor.OffsetModuleName;

                        reader.Seek(nameOffset);
                        descriptor.ModuleName = reader.ReadNullTermString(IOExtensions.MAX_PATH);

                        ImageLayoutEvents.AddReference<string, BoundImportDescriptor, ushort>(descriptor.ModuleName, "ModuleName", nameOffset, (ulong)descriptor.ModuleName.Length, descriptor, (d) => d.OffsetModuleName);
                    }

                    for (var x = 0; x < descriptor.NumberOfModuleForwarderRefs; x++)
                    {
                        var referenceOffset = reader.BaseStream.Position;

                        var reference = new BoundForwarderReference((ulong)referenceOffset, sizeof(ushort))
                        {
                            TimeDateStamp = reader.ReadUInt32(),
                            OffsetModuleName = reader.ReadUInt16(),
                            Reserved = reader.ReadUInt16()
                        };

                        descriptor.references.Add(reference);

                        ImageLayoutEvents.AddReference<BoundImportDescriptor, ulong>(reference, (ulong)referenceOffset, reference.Size, descriptor, (d) => d.StartOfModuleForwarderRefs);

                        using (var reset = reader.MarkForReset())
                        {
                            var nameOffset = (ulong)offset + descriptor.OffsetModuleName;

                            reader.Seek(nameOffset);
                            reference.ModuleName = reader.ReadNullTermString(IOExtensions.MAX_PATH);

                            ImageLayoutEvents.AddReference<string, BoundForwarderReference, ushort>(reference.ModuleName, "ModuleName", nameOffset, (ulong)reference.ModuleName.Length, reference, (r) => r.OffsetModuleName);
                        }

                    }

                    descriptors.Add(descriptor);
                }
            }
            while (descriptor.TimeDateStamp != 0);

            return descriptors;
        }
    }
}
