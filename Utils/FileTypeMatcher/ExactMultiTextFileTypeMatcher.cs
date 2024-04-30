using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class ExactMultiTextFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte[] startBytes;
        private readonly byte[] endBytes;
        private readonly bool trimText;
        private bool matchedStart;
        public override bool IsMultiMatch => true; 

        public ExactMultiTextFileTypeMatcher(string startText, string endText, bool trimText = false)
        {
            this.startBytes = startText.ToBytes();
            this.endBytes = endText.ToBytes();
            this.trimText = trimText;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            byte[] bytes;

            if (!matchedStart)
            {
                bytes = startBytes;
            }
            else
            {
                bytes = endBytes;
            }

            foreach (var b in bytes)
            {
                var readByte = stream.ReadByte();

                if (trimText && ((char) readByte).IsWhitespace())
                {
                    continue;
                }

                if (readByte != b)
                {
                    return false;
                }
            }

            if (!matchedStart)
            {
                matchedStart = true;
            }

            return true;
        }
    }
}
