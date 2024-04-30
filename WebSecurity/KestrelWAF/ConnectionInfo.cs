using MaxMind.GeoIP2.Responses;

namespace WebSecurity.KestrelWAF;

public class ConnectionInfo
{
    public string ConnectionId { get; set; }
    public string IpAddress { get; set; }
    public string Port { get; set; }
    public string? Host { get; set; }
    public string? Referer { get; set; }
    public string? UserAgent { get; set; }
    public string? Origin { get; set; }
    public string? FetchSite { get; set; }
    public string? FetchMode { get; set; }
    public string? FetchDestination { get; set; }
    public string? Location { get; set; }
    public string ApplicationName { get; set; }
    public string EnvironmentName { get; set; }
    public string? User { get; set; }
}
