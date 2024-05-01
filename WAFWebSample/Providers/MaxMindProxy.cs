using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.Interfaces;

namespace WAFWebSample.WebApi.Providers
{
    public class MaxMindProxy : IMaxMindProxy
    {
        private IHostEnvironment hostEnvironment;

        public MaxMindProxy(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public CityResponse GetCity(IPAddress? remoteIpAddress)
        {
            var path = Path.Combine(hostEnvironment.ContentRootPath, @"wwwroot\GeoLite\GeoLite2-City.mmdb");

            using (var reader = new DatabaseReader(path))
            {
                try
                {
                    var response = reader.City(remoteIpAddress);

                    return response;
                }
                catch (Exception ex)
                {
                    return null!;
                }
            }
        }
    }
}
