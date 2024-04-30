using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebSecurity.KestrelWAF
{
    public class LocationStats
    {
        public string LocationFull { get; }
        public string City { get; }
        public string Province { get; }
        public string Country { get; }
        public string Type { get; }
        public int TypeIndex { get; }
        public string Coordinates { get; set; }
        public int BlockCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }

        public LocationStats(string location, string type)
        {
            var partCount = location.Count(c => c == '|') + 1;
            var parts = GetLocationParts(location);

            this.LocationFull = location;
            this.Type = type;
            this.City = parts.city!;
            this.Province = parts.province!;
            this.Country = parts.country;

            switch (type)
            {
                case "City":
                    Debug.Assert(partCount == 3);
                    this.TypeIndex = 1;
                    break;
                case "Province":
                    Debug.Assert(partCount == 2);
                    this.TypeIndex = 2;
                    break;
                case "Country":
                    Debug.Assert(partCount == 1);
                    this.TypeIndex = 3;
                    break;
            }
        }

        public static (string? city, string? province, string country) GetLocationParts(string locationFull)
        {
            var parts = locationFull.Split('|');

            if (parts.Length == 1)
            {
                return (null, null, parts[0]);
            }
            else if (parts.Length == 2)
            {
                return (null, parts[0], parts[1]);
            }
            else if (parts.Length == 3)
            {
                return (parts[0], parts[1], parts[2]);
            }
            else
            {
                DebugUtils.Break();
            }

            return (null, null, null!);
        }

        public LocationStats AddCoordinates(string coordinates)
        {
            this.Coordinates = coordinates;

            return this;
        }

        public LocationStats AddBlockCount(int blockCount)
        {
            this.BlockCount = blockCount;

            return this;
        }

        public LocationStats AddFailCount(int failCount)
        {
            this.FailCount = failCount;

            return this;
        }

        public LocationStats AddSuccesCount(int successCount)
        {
            this.SuccessCount = successCount;

            if (!Validate(out string? error))
            {
                DebugUtils.BreakIfAttached();
            }

            return this;
        }

        public bool Validate(out string? error)
        {
            if (CompareExtensions.AllAreZero(this.SuccessCount, this.BlockCount, this.FailCount))
            {
                error = "Success, Block, and FailCount are all zero";

                return false;
            }
            else if (CompareExtensions.AllAreZero(this.SuccessCount, this.FailCount))
            {
                error = "Success, and FailCount are all zero";

                return false;
            }
            else if (CompareExtensions.AllAreZero(this.SuccessCount, this.BlockCount))
            {
                error = "Success and BlockCount are all zero";

                return false;
            }
            else if (CompareExtensions.AllAreTrue(this.BlockCount == 0, this.FailCount > 0))
            {
                error = $"BlockCount is zero but FailCount = {this.FailCount}";

                return false;
            }
            else if (this.BlockCount > this.FailCount)
            {
                error = $"BlockCount of {this.BlockCount} is greater than FailCount of {this.FailCount}";

                return false;
            }

            error = null;
            return true;
        }
    }
}
