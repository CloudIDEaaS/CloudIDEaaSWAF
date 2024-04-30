using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils.PortableExecutable
{
    public static class AddressingUtils
    {
        public static ulong RelativeVirtualAddressToFileOffset(ulong rva, IEnumerable<Section> sections)
        {
            var section = RelativeVirtualAddressToSection(rva, sections);
            var offsetWithinInSection = rva - section.VirtualAddress;
            var fileOffset = section.PointerToRawData + offsetWithinInSection;

            return fileOffset;
        }

        public static ulong RelativeVirtualAddressToVirtualAddress(ulong rva, PEHeader peHeader)
        {
            var section = RelativeVirtualAddressToSection(rva, peHeader.Sections);
            var offsetWithinInSection = rva - section.VirtualAddress;
            var fileOffset = section.PointerToRawData + offsetWithinInSection;

            return FileOffsetToVirtualAddress(fileOffset, peHeader);
        }

        public static ulong VirtualAddressToRelativeVirtualAddress(ulong rva, PEHeader peHeader)
        {
            return rva - peHeader.ImageBase;
        }

        public static ulong FileOffsetToRelativeVirtualAddress(ulong offset, IEnumerable<Section> sections)
        {
            var section = FileOffsetToSection(offset, sections);
            var offsetWithinInSection = offset - section.PointerToRawData;
            var rva = section.VirtualAddress + offsetWithinInSection;

            return rva;
        }

        public static ulong FileOffsetToVirtualAddress(ulong offset, PEHeader peHeader)
        {
            var section = FileOffsetToSection(offset, peHeader.Sections);
            var offsetWithinInSection = offset - section.PointerToRawData;
            var rva = section.VirtualAddress + offsetWithinInSection;

            return rva + peHeader.ImageBase;
        }

        public static ulong RelativeVirtualAddressToMemoryOffset(ulong rva, IEnumerable<Section> sections)
        {
            var section = RelativeVirtualAddressToSection(rva, sections);
            var sectionOffset = section.PointerToRawData + (rva - section.VirtualAddress);

            return section.VirtualAddress + sectionOffset;
        }

        public static ulong RelativeVirtualAddressToFileOffset(ulong rva, Section section)
        {
            var fileOffset = section.PointerToRawData + (rva - section.VirtualAddress);

            return fileOffset;
        }

        public static ulong SectionToFileOffset(ulong offset, Section section)
        {
            var fileOffset = section.PointerToRawData + offset;

            return fileOffset;
        }

        public static ulong RelativeVirtualAddressToMemoryOffset(ulong rva, Section section)
        {
            var sectionOffset = section.PointerToRawData + (rva - section.VirtualAddress);

            return section.VirtualAddress + sectionOffset;
        }

        public static ulong RelativeVirtualAddressToMemoryOffset(ulong rva, IntPtr baseAddress)
        {
            return ((ulong) baseAddress.ToInt64()) + rva;
        }

        public static COFFSection RelativeVirtualAddressToSection(ulong rva, IEnumerable<COFFSection> sections)
        {
            var section = sections.FirstOrDefault(s => rva.IsBetween(s.VirtualAddress, s.VirtualAddress + s.SizeOfRawData));

            return section;
        }

        public static Section RelativeVirtualAddressToSection(ulong rva, IEnumerable<Section> sections)
        {
            var section = sections.FirstOrDefault(s => rva.IsBetween(s.VirtualAddress, s.VirtualAddress + s.SizeOfRawData));

            return section;
        }

        public static Section FileOffsetToSection(ulong address, IEnumerable<Section> sections)
        {
            var section = sections.FirstOrDefault(s => address.IsBetween(s.PointerToRawData, s.PointerToRawData + s.SizeOfRawData));

            return section;
        }

        public static ulong EntryPointToFileOffset(uint addressOfEntryPoint, uint baseOfCode, uint fileAlignment, IEnumerable<Section> sections)
        {
            var section = RelativeVirtualAddressToSection(addressOfEntryPoint, sections);
            var entryPointOffset = section.PointerToRawData + (addressOfEntryPoint - baseOfCode);

            return baseOfCode + entryPointOffset + fileAlignment;
        }
    }
}
