using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class FuzzyFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte?[] bytes;

        public FuzzyFileTypeMatcher(IEnumerable<byte?> bytes)
        {
            this.bytes = bytes.ToArray();
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            foreach (var b in this.bytes)
            {
                var c = stream.ReadByte();
                if (c == -1 || (b.HasValue && c != b.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
