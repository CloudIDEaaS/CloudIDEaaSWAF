using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebSecurity.KestrelWAF;
using WebSecurity.KestrelWAF.RulesEngine;
using MaxMind.GeoIP2.Model;
using System.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebSecurity;

public static class DependencyInjection
{
    public static void UseKestrelWAF(this IApplicationBuilder app)
    {
        app.UseMiddleware<WAFMiddleware>();
    }

    public static IServiceCollection AddKestrelWAF(this IServiceCollection services, IConfiguration configuration)
    {
        var ruleset = configuration.GetSection("WAFConfiguration:Ruleset");

        services.Configure<Rule>(opt => ruleset.Bind(opt));

        services.AddMemoryCache();
        services.AddSingleton<IWAFDataFileCache, WAFDataFileCache>();

        return services;
    }
}