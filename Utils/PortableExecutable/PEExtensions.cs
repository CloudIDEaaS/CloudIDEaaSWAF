using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils.PortableExecutable
{
    public static class PEExtensions
    {
        public static OffsetSizeCalculator MarkForCalculation(this BinaryReader reader)
        {
            return new OffsetSizeCalculator(reader);
        }

        public static void Raise(this IImageLayoutItem item, EventHandler disposed)
        {
            if (disposed != null)
            {
                disposed(item, EventArgs.Empty);
            }
        }

        public static OffsetSizeCalculator MarkForCalculation(this BinaryReader directoryDataReader, DataDirectory dataDirectory, IList<Section> sections)
        {
            return new OffsetSizeCalculator(directoryDataReader, dataDirectory, sections);
        }
    }
}
