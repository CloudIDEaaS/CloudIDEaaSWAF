using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class MultiRangeTextFileTypeMatcher : FileTypeMatcher
    {
        private readonly FileTypeMatcher matcher;
        private readonly bool trimText;
        private readonly int maximumStartLocation;

        public MultiRangeTextFileTypeMatcher(FileTypeMatcher matcher, int maximumStartLocation, bool trimText)
        {
            this.matcher = matcher;
            this.trimText = trimText;
            this.maximumStartLocation = maximumStartLocation;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            for (var i = 0; i < this.maximumStartLocation; i++)
            {
                stream.Position = i;

                //if (
            }

            for (var i = 0; i < this.maximumStartLocation; i++)
            {
                stream.Position = i;

                if (matcher.Matches(stream, resetPosition: false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
