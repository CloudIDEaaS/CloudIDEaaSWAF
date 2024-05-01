using System.Security.Claims;
using WAFWebSample.Data;

namespace WebSecurity.StartupTests.Helpers;

public class Mocks
{
    public ClaimsPrincipal AuthenticatedUser { get; internal set; }
    public WAFWebSampleDbContext WAFWebSampleDbContext { get; internal set; }
}
