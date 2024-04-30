using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileTypeMatcher
{
    public class RangeFileTypeMatcher : FileTypeMatcher
    {
        private readonly FileTypeMatcher matcher;
        private readonly int maximumStartLocation;
        private readonly int maximumEndLocation;

        public RangeFileTypeMatcher(FileTypeMatcher matcher, int maximumStartLocation, int maximumEndLocation)
        {
            this.matcher = matcher;
            this.maximumStartLocation = maximumStartLocation;
            this.maximumEndLocation = maximumEndLocation;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            if (matcher.IsMultiMatch)
            {
                for (var i = 0; i < Math.Max(this.maximumStartLocation, stream.Length); i++)
                {
                    stream.Position = i;

                    if (matcher.Matches(stream, resetPosition: false))
                    {
                        break;
                    }
                }

                for (var i = 0; i < Math.Max(this.maximumStartLocation, stream.Length) - stream.Position; i++)
                {
                    if (matcher.Matches(stream, resetPosition: false))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
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
}
