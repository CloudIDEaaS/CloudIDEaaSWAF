using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class ExactFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte[] bytes;

        public ExactFileTypeMatcher(IEnumerable<byte> bytes)
        {
            this.bytes = bytes.ToArray();
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            foreach (var b in this.bytes)
            {
                if (stream.ReadByte() != b)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
