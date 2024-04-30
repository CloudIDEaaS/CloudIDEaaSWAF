using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class ThunkData : DataDirectoryEntry
    {
        public ulong RVA { get; set; }
        public uint Function { get; set; }
        public uint Ordinal { get; set; }
        public uint AddressOfData { get; set; }
        public string ForwarderString { get; set; }
        public ImportByName ImportByName { get; set; }
        private bool is64Bit;

        public ThunkData(Machine machine, ulong offset, ulong size) : base(offset, size)
        {
            is64Bit = machine.Is64Bit();
        }

        public string PointsTo
        {
            get
            {
                if (this.ImportByName != null)
                {
                    return this.ImportByName.Name;
                }

                return null;
            }
        }

        public override string DebugInfo
        {
            get 
            {
                if (is64Bit)
                {
                    return string.Format(
                        "RVA: 0x{0:x16}, "
                        + "Function: 0x{1:x4}, "
                        + "Ordinal: 0x{2:x8}, "
                        + "AddressOfData: 0x{3:x8}, "
                        + "ForwarderString: {4}, "
                        + "PointsTo: {5}",
                        this.RVA,
                        this.Function,
                        this.Ordinal,
                        this.AddressOfData,
                        this.ForwarderString.AsDisplayText(),
                        this.PointsTo.AsDisplayText());
                }
                else
                {
                    return string.Format(
                        "RVA: 0x{0:x8}, "
                        + "Function: 0x{1:x4}, "
                        + "Ordinal: 0x{2:x8}, "
                        + "AddressOfData: 0x{3:x8}, "
                        + "ForwarderString: {4}, "
                        + "PointsTo: {5}",
                        this.RVA,
                        this.Function,
                        this.Ordinal,
                        this.AddressOfData,
                        this.ForwarderString.AsDisplayText(),
                        this.PointsTo.AsDisplayText());
                }
            }
        }
    }
}
