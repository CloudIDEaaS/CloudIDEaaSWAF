using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using static alglib;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;

namespace Utils.Kestrel;

internal static class KestrelCliMiddleware
{
    private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5.0);
    private static CancellationToken applicationStopping;
    private static DiagnosticSource diagnosticSource;
    private static IKestrelCliResponseHandler kestrelCliResponseHandler;
    private static string? sourcePath;

    public static void Attach(IKestrelSpaBuilder spa, int allowedPort)
    {
        string packageManagerCommand = spa.Options.PackageManagerCommand;
        IApplicationBuilder applicationBuilder;
        var options = spa.Options;

        sourcePath = options.SourcePath;
        applicationBuilder = spa.ApplicationBuilder;
        kestrelCliResponseHandler = applicationBuilder.ApplicationServices.GetRequiredService<IKestrelCliResponseHandler>();
        kestrelCliResponseHandler.AllowedPort = allowedPort;

        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new ArgumentException("Property 'SourcePath' cannot be null or empty", "spaBuilder");
        }

        options.DefaultPageStaticFileOptions = new KestrelStaticFileOptions
        {
            RequestPath = Path.Combine(sourcePath, "dist").ForwardSlashes(),
            HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
            OnPrepareResponse = (c) => PrepareResponse(c, kestrelCliResponseHandler.PrepareResponse)
        };

        applicationStopping = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
        diagnosticSource = applicationBuilder.ApplicationServices.GetRequiredService<DiagnosticSource>();
    }

    private static bool PrepareResponse(KestrelStaticFileResponseContext context, Func<KestrelStaticFileResponseContext, bool> onPrepareResponse)
    {
        var httpContext = context.Context;
        var request = httpContext.Request;
        var connectionId = httpContext.Connection.Id;
        var localIpAddress = httpContext.Connection.LocalIpAddress;
        var remoteIpAddress = httpContext.Connection.RemoteIpAddress;
        var url = request.GetDisplayUrl();
        //var staticFileMiddleware = httpContext.RequestServices.GetRequiredService<StaticFileMiddleware>();

        Console.WriteLine($"Connection {connectionId} made from {remoteIpAddress} to {localIpAddress}, url: {url}");

        return onPrepareResponse(context);
    }
}