using MaxMind.GeoIP2.Responses;
using System.Net;

namespace WebSecurity.Interfaces;

public interface IMaxMindProxy
{
    CityResponse GetCity(IPAddress? remoteIpAddress);
}
