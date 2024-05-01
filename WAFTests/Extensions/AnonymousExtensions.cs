using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using Utils;

namespace WebSecurity.StartupTests
{
    public static class AnonymousExtensions
    {
        public static CityResponse GetUserLocationResponse(this IPAddress ipAddress)
        {
            var solutionPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%"));
            var webApiRootPath = Path.Combine(solutionPath, @"WAFWebSample");
            var path = Path.Combine(webApiRootPath, @"wwwroot\GeoLite\GeoLite2-City.mmdb");

            using (var reader = new DatabaseReader(path))
            {
                if (ipAddress == null)
                {
                    return null;
                }

                ipAddress = ipAddress.GetPublicIPV4();

                try
                {
                    var city = reader.City(ipAddress);

                    return city;
                }
                catch (Exception ex)
                {
                    return null!;
                }
            }
        }
    }
}
