using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class TextFileTypeMatcher : RangeFileTypeMatcher
    {
        private int maximumStartLocation;
        private FileTypeMatcher matcher;

        public TextFileTypeMatcher(FileTypeMatcher matcher, int maximumStartLocation) : base(matcher, maximumStartLocation)
        {
            this.maximumStartLocation = maximumStartLocation;
            this.matcher = matcher;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            for (var i = 0; i < this.maximumStartLocation; i++)
            {
                stream.Position = i;

                if (matcher.Matches(stream, resetPosition: false))
                {
                    return true;
                }
            }

            return true;
        }
    }
}
