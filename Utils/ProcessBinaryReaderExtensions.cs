using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.IO
{
    internal static class ProcessBinaryReaderExtensions
    {
        public static ulong GetBaseAddress(this Dictionary<ulong, ProcessBinaryReader> regions, ProcessBinaryReader region)
        {
            var pair = regions.Single(p => p.Value == region);

            return (ulong) pair.Key;
        }

        public static ulong GetNextAddress(this Dictionary<ulong, ProcessBinaryReader> regions, ulong address)
        {
            ulong nextAddress = ulong.MaxValue;
            var foundFirst = false;

            foreach (var pair in regions)
            {
                if (foundFirst)
                {
                    nextAddress = pair.Key;
                }

                if (pair.Key == address)
                {
                    foundFirst = true;
                }
            }

            return nextAddress;
        }
    }
}