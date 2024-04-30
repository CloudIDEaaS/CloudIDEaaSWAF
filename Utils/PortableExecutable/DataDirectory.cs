using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Utils;
using System.ComponentModel;
using Utils.PortableExecutable.Enums;
using Utils.IO;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\{ Directory }Directory.png")]
    public class DataDirectory : IImageLayoutItem
    {
        public DirectoryId Directory { get; set; }
        public ulong Address { get; set; }
        public ulong Size { get; set; }
        public Section Section { get; set; }
        public Machine Machine { get; set; }
        public Guid UniqueId { get; private set; }
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public DataDirectory()
        {
            this.UniqueId = Guid.NewGuid();
        }

        public static byte[] ReadVirtualDirectory(BinaryReader reader, DataDirectory dataDirectory, IList<Section> sections)
        {
            if (reader is ProcessBinaryReader)
            {
                var memoryOffset = AddressingUtils.RelativeVirtualAddressToMemoryOffset(dataDirectory.Address, sections);

                reader.Seek(memoryOffset);
            }
            else
            {
                var fileOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, sections);

                reader.Seek(fileOffset);
            }

            return reader.ReadBytes((int)dataDirectory.Size);
        }

        public string Name
        {
            get
            {
                return string.Format("{0} Directory", this.Directory);
            }
        }

        public static int HeaderSize
        {
            get
            {
                return sizeof(uint) * 2;
            }
        }

        public IEnumerable<DataDirectoryEntry> ReadEntries(BinaryReader reader, IntPtr baseAddress)
        {
            switch (this.Directory)
            {
                case DirectoryId.Export:
					return ReadExports(reader, baseAddress);
                case DirectoryId.Import:
					return ReadImports(reader, baseAddress);
                case DirectoryId.Resource:
					return ReadResources(reader);
                case DirectoryId.Exception:
					return ReadExceptions(reader);
                case DirectoryId.Security:
					return ReadSecurity(reader);
                case DirectoryId.Relocation:
					return ReadRelocations(reader);
                case DirectoryId.Debug:
					return ReadDebugEntries(reader);
                case DirectoryId.Architecture:
                    return ReadArchitectureEntries(reader);
                case DirectoryId.GlobalPointer:
					return ReadGlobalPointers(reader);
                case DirectoryId.TLS:
                    return ReadTLSEntries(reader);
                case DirectoryId.LoadConfiguration:
					return ReadConfigurations(reader);
                case DirectoryId.BoundImport:
					return ReadBoundImports(reader);
                case DirectoryId.ImportAddress:
					return ReadImportAddresses(reader);
                case DirectoryId.DelayImport:
					return ReadDelayImports(reader);
            }

            e.Throw<InvalidOperationException>(string.Format("DataDirectory does not support reading {0} directory", this.Directory));
            return null;
        }

        private List<ExportDirectory> ReadExports(BinaryReader reader, IntPtr baseAddress)
		{
			if (this.Directory != DirectoryId.Export)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadExports"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return ExportDirectory.ReadExports(reader, this, baseAddress);
		}

        private List<ImportDescriptor> ReadImports(BinaryReader reader, IntPtr baseAddress)
		{
			if (this.Directory != DirectoryId.Import)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadImports"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return ImportDescriptor.ReadImports(reader, this, baseAddress);
        }

        private List<ResourceEntry> ReadResources(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Resource)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadResources"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return new List<ResourceEntry>() { ResourceDirectory.ReadDirectory(reader, this) };
		}

        private List<ExceptionEntry> ReadExceptions(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Exception)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadExceptions"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            e.Throw<NotImplementedException>("DataDirectory.ReadExceptions is not implemented");
            return null;
        }

        private List<WinCertificate> ReadSecurity(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Security)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadSecurity"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return WinCertificate.ReadCertificates(reader, this);
        }

        private List<BaseRelocation> ReadRelocations(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Relocation)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadRelocations"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return BaseRelocation.ReadRelocations(reader, this);
		}

        private List<DebugDirectory> ReadDebugEntries(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Debug)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadDebugEntries"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return DebugDirectory.ReadDebugDirectories(reader, this);
        }

        private List<ArchitectureEntry> ReadArchitectureEntries(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.Architecture)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadArchitectureEntries"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

			e.Throw<NotImplementedException>("DataDirectory.ReadArchitectureEntries is not implemented");
			return null;
		}

        private List<GlobalPointerEntry> ReadGlobalPointers(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.GlobalPointer)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadGlobalPointers"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

			e.Throw<NotImplementedException>("DataDirectory.ReadGlobalPointers is not implemented");
			return null;
		}

        private List<TLSEntry> ReadTLSEntries(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.TLS)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadTLSEntries"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

			e.Throw<NotImplementedException>("DataDirectory.ReadTLSEntries is not implemented");
			return null;
		}

        private List<ConfigurationDirectory> ReadConfigurations(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.LoadConfiguration)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadConfigurations"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return ConfigurationDirectory.ReadDirectories(reader, this);
        }

        private List<BoundImportDescriptor> ReadBoundImports(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.BoundImport)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadBoundImports"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return BoundImportDescriptor.ReadImports(reader, this);
		}

        private List<ImportAddressDirectory> ReadImportAddresses(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.ImportAddress)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadImportAddresses"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

            return ImportAddressDirectory.ReadDirectories(reader, this);
        }

        private List<DelayImportEntry> ReadDelayImports(BinaryReader reader)
		{
			if (this.Directory != DirectoryId.DelayImport)
			{
				e.Throw<InvalidOperationException>(string.Format("{0}DataDirectory does not support method {1}", this.Directory, "DataDirectory.ReadDelayImports"));
			}
            else if (this.Address == 0 || this.Size == 0)
            {
                return null;
            }

			e.Throw<NotImplementedException>("DataDirectory.ReadDelayImports is not implemented");
			return null;
		}

        public string DebugInfo
        {
            get
            {
                return string.Format("{0}, Address: 0x{1:x8}, SizeHex: 0x{2:x8}, Size: {2}", this.Directory, this.Address, this.Size);
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
