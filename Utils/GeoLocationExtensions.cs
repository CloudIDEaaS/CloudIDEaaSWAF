using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public static class GeoLocationExtensions
    {
        public static GeoCoordinate ToGeoCoordinate(string coordinatesString)
        {
            var regex = new Regex(@"^(?<latitude>[\-\d\.]+),(?<longitude>[\-\d\.]+)");

            if (regex.IsMatch(coordinatesString))
            {
                var match = regex.Match(coordinatesString);
                var latitude = double.Parse(match.GetGroupValue("latitude"));
                var longitude = double.Parse(match.GetGroupValue("longitude"));

                return new GeoCoordinate(latitude, longitude);
            }

            throw new ArgumentException("Invalid coordinate string", nameof(coordinatesString));
        }
    }
}
