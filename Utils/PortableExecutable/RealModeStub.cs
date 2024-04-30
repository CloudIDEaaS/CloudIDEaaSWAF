using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\RealModeStub.png")]
    public class RealModeStub : IImageLayoutCodeProviderItem
    {
        public ulong Offset { get; set; }
        public ulong Size { get; set; }
        public byte[] Code { get; set; }
        public Guid UniqueId { get; private set; }
        public event EventHandler Disposed;
        [Browsable(false)]
		public ISite Site { get; set; }

        public static RealModeStub Read(BinaryReader reader, ulong size)
        {
            var offset = (ulong) reader.BaseStream.Position;

            var stub = new RealModeStub
            {
                Offset = offset,
                Size = size,
                UniqueId = Guid.NewGuid()
            };

            if (((int)size) > 0)
            {
                //stub.Code = reader.ReadBytes((int)size);
            }

            return stub;
        }

        public void Dispose()
        {
            this.Raise(Disposed);
        }

        public IEnumerable<CodeSource> GetCodePoints()
        {
            var flags = CodeTypeFlags.Bytes | CodeTypeFlags.X86Assembly;
            var name = "RealModeStub";
            var fullName = string.Format("{0}.{1}", name, UniqueId.ToString());

            return new List<CodeSource>() { new CodeSource(name, fullName, flags, "Assembly", this.Code, this.Offset) };
        }
    }
}
