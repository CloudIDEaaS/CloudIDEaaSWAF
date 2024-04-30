using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MemoryMappedFiles;

public static class MemoryMappedExtensions
{
    public static MappedFileInfo MapFile(this FileInfo file)
    {
        var length = (int)file.Length;
        var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName);

        return new MappedFileInfo(file, mappedFile, length);
    }

    public static T MapFileGetContent<T>(this MappedFileInfo mappedFileInfo)
    {
        var length = mappedFileInfo.Length;
        var mappedFile = mappedFileInfo.MappedFile;

        using (var accessor = mappedFile.CreateViewAccessor(0, 0))
        {
            var type = typeof(T);
            var bytes = new byte[length];

            accessor.ReadArray(0L, bytes, 0, length);

            if (type == typeof(Stream))
            {
                var memoryStream = new MemoryStream();

                memoryStream = bytes.ToMemory();

                return (T)(object)memoryStream;
            }
            else if (type == typeof(string))
            {
                return (T)(object)bytes.ToText();
            }
            else
            {
                DebugUtils.Break();
            }
        }

        return default!;
    }

    public static T MapFileGetContent<T>(this FileInfo file)
    {
        var length = (int) file.Length;

        using (var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, file.FullName, length))
        {
            using (var accessor = mappedFile.CreateViewAccessor(0, 0))
            {
                var type = typeof(T);
                var bytes = new byte[length];

                accessor.ReadArray(0L, bytes, 0, length);

                if (type == typeof(Stream))
                {
                    var memoryStream = new MemoryStream();

                    memoryStream = bytes.ToMemory();

                    return (T)(object) memoryStream;
                }
                else if (type == typeof(string))
                {
                    return (T)(object)bytes.ToText();
                }
                else
                {
                    DebugUtils.Break();
                }
            }
        }

        return default!;
    }
}
