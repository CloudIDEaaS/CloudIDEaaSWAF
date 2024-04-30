using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class ExactTextFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte[] bytes;
        private readonly bool trimText;

        public ExactTextFileTypeMatcher(string text, bool trimText = false)
        {
            this.bytes = text.ToBytes();
            this.trimText = trimText;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            foreach (var b in this.bytes)
            {
                var matchByte = stream.ReadByte();

                if (trimText)
                {
                    if (((char)matchByte).IsWhitespace())
                    {
                        continue;
                    }
                }

                if (matchByte != b)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
