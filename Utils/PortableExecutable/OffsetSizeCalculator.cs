using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;
using System.Diagnostics;
using Utils.IO;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class OffsetSizeCalculator
    {
        private BinaryReader reader;
        private ulong directoryOffset;
        private Queue<KeyValuePair<IImageLayoutItem, int>> offsetQueue;
        private ulong offset;
        private ulong originalOffset;

        public OffsetSizeCalculator(BinaryReader reader)
        {
            this.reader = reader;
            this.offset = (ulong) reader.BaseStream.Position;
            this.originalOffset = offset;

            offsetQueue = new Queue<KeyValuePair<IImageLayoutItem, int>>();
        }

        public OffsetSizeCalculator(BinaryReader reader, DataDirectory dataDirectory, IList<Section> sections)
        {
            if (reader is ProcessBinaryReader)
            {
                directoryOffset = AddressingUtils.RelativeVirtualAddressToMemoryOffset(dataDirectory.Address, sections);
            }
            else
            {
                directoryOffset = AddressingUtils.RelativeVirtualAddressToFileOffset(dataDirectory.Address, sections);
            }

            this.reader = reader;
            this.offset = directoryOffset + (ulong)reader.BaseStream.Position;
            this.originalOffset = offset;

            offsetQueue = new Queue<KeyValuePair<IImageLayoutItem, int>>();
        }

        public void OffsetBy(ulong offset)
        {
            this.offset += offset;
        }

        private void ProcessQueue()
        {
            foreach (var pair in offsetQueue)
            {
                var imageLayoutItem = (ImageLayoutItem)pair.Key;

                OffsetBy(imageLayoutItem.Offset * (ulong) pair.Value);
            }

            offsetQueue.Clear();
        }

        public ulong Offset
        {
            get
            {
                ProcessQueue();

                return offset;
            }
        }

        public ulong Size
        {
            get
            {
                var endOffset = directoryOffset + (ulong) reader.BaseStream.Position;

                ProcessQueue();

                return (ulong)endOffset - originalOffset;
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format(
        			"Offset: 0x{0:x8}, "
        			+ "Size: 0x{1:x8}", 
        			this.offset,
        			this.Size);
            }
        }

        public static OffsetSizeCalculator operator +(OffsetSizeCalculator calculator, IImageLayoutItem imageLayoutItem)
        {
            calculator.offsetQueue.Enqueue(new KeyValuePair<IImageLayoutItem, int>(imageLayoutItem, 1));

            return calculator;
        }

        public static OffsetSizeCalculator operator -(OffsetSizeCalculator calculator, IImageLayoutItem imageLayoutItem)
        {
            calculator.offsetQueue.Enqueue(new KeyValuePair<IImageLayoutItem, int>(imageLayoutItem, -1));

            return calculator;
        }
    }
}
