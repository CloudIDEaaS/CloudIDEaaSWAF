using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MemoryMappedFiles
{
    public class MappedFileInfo : IDisposable
    {
        public FileInfo FileInfo { get; set; }
        public int Length { get; }
        public MemoryMappedFile MappedFile { get; set; }

        public MappedFileInfo(FileInfo file, MemoryMappedFile mappedFile, int length)
        {
            this.MappedFile = mappedFile;
            this.FileInfo = file;
            this.Length = length;
        }

        public void Dispose()
        {
            this.MappedFile.Dispose();
        }
    }
}
