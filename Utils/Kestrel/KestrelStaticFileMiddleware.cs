using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.Net.Mime;
using static alglib;

namespace Utils.Kestrel
{
    public class KestrelStaticFileMiddleware
    {
        private readonly KestrelStaticFileOptions kestrelOptions;
        private readonly KestrelSpaOptions kestrelSpaOptions;
        private readonly IWebHostEnvironment environment;
        private readonly PathString matchUrl;
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly IFileProvider fileProvider;
        private readonly IContentTypeProvider contentTypeProvider;
        private readonly StaticFileMiddleware staticFileMiddleware;
        private readonly IKestrelOptionsService kestrelOptionsService;
        private readonly IKestrelCliResponseHandler kestrelCliResponseHandler;

        public KestrelStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment environment, IOptions<KestrelStaticFileOptions> kestrelOptions, IKestrelOptionsService kestrelOptionsService, IKestrelCliResponseHandler kestrelCliResponseHandler, ILoggerFactory loggerFactory, KestrelSpaOptions kestrelSpaOptions)
        {
            ArgumentNullException.ThrowIfNull(next);
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(kestrelOptions);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            var middlewareStaticOptions = new KestrelMiddlewareStaticFileOptions(kestrelOptions.Value);

            this.staticFileMiddleware = new StaticFileMiddleware(next, environment, middlewareStaticOptions, loggerFactory);
            this.kestrelOptionsService = kestrelOptionsService;
            this.kestrelCliResponseHandler = kestrelCliResponseHandler;
            this.next = next;
            this.kestrelOptions = kestrelOptions.Value;
            this.environment = environment;
            contentTypeProvider = this.kestrelOptions.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            matchUrl = this.kestrelOptions.RequestPath;
            this.kestrelSpaOptions = kestrelSpaOptions;

            logger = loggerFactory.CreateLogger<KestrelStaticFileMiddleware>();
        }

        /// <summary>
        /// Processes a request to determine if it matches a known file, and if so, serves it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var url = request.GetDisplayUrl();

            logger.LogWarning($"Entering middleware for {url}");

            if (!ValidateNoEndpointDelegate(context))
            {
                throw new AccessViolationException("No endpoint request delegate");
            }
            else if (!ValidateMethod(context))
            {
                throw new AccessViolationException("Invalid method");
            }
            else if (!ValidatePath(context, matchUrl, out var subPath))
            {
                logger.LogWarning("Cannot serve path requested {0}", subPath);
            }
            else if (!LookupContentType(context, contentTypeProvider, kestrelOptions, subPath, out var contentType))
            {
                logger.LogWarning("Cannot serve content type requested {0} for path {1}", contentType, subPath);
            }
            else
            {
                //logger.LogInformation("Check if static file requested for {0} for path {1} can be served.", contentType, subPath);
                logger.LogWarning("Check if static file requested for {0} for path {1} can be served.", contentType, subPath);

                if (CanServeStaticFile(context, contentType, ref subPath))
                {
                    //logger.LogInformation("Trying static file requested for {0} for path {1}", contentType, subPath);
                    logger.LogWarning("Trying static file requested for {0} for path {1}", contentType, subPath);

                    await TryServeStaticFile(context, contentType, subPath);
                    
                    // logger.LogInformation("Static file handled for {0} for path {1}", contentType, subPath);
                    logger.LogWarning("Static file handled for {0} for path {1}", contentType, subPath);

                    return;
                }
            }

            // logger.LogInformation("Passing request to standard StaticFileMiddleware");
            logger.LogWarning("Passing request to standard StaticFileMiddleware");

            await staticFileMiddleware.Invoke(context);
        }

        // Return true because we only want to run if there is no endpoint delegate.
        private bool ValidateNoEndpointDelegate(HttpContext context) => context.GetEndpoint()?.RequestDelegate is null;

        private bool ValidateMethod(HttpContext context)
        {
            return context.Request.Method.IsOneOf("GET");
        }

        internal bool ValidatePath(HttpContext context, PathString matchUrl, out PathString subPath) => TryMatchPath(context, matchUrl, forDirectory: false, out subPath);

        private bool TryMatchPath(HttpContext context, PathString matchUrl, bool forDirectory, out PathString subPath)
        {
            var request = context.Request;
            var uri = new Uri(request.GetDisplayUrl());
            subPath = uri.PathAndQuery;

            if (!kestrelOptionsService.Listeners.ContainsKey(uri.Port))
            {
                return false;
            }

            return true;
        }

        internal bool LookupContentType(HttpContext context, IContentTypeProvider contentTypeProvider, KestrelStaticFileOptions options, PathString subPath, out string? contentType)
        {
            var request = context.Request;
            var uri = new Uri(request.GetDisplayUrl());

            if (contentTypeProvider.TryGetContentType(subPath.Value!, out contentType))
            {
                return true;
            }

            if (options.ServeUnknownFileTypes)
            {
                contentType = options.DefaultContentType;
                return true;
            }

            if (CanServeStaticFile(context, ".html", ref subPath))
            {
                return true;
            }

            return false;
        }

        private bool CanServeStaticFile(HttpContext context, string? contentType, ref PathString subPath)
        {
            var request = context.Request;
            var uri = new Uri(request.GetDisplayUrl());

            if (kestrelCliResponseHandler.Handles(uri.AbsolutePath, contentType, ref subPath))
            {
                return true;
            }
            else if (kestrelCliResponseHandler.UseDefaultHandler(uri.AbsolutePath, contentType, ref subPath))
            {
                return true;
            }

            return false;
        }

        private async Task TryServeStaticFile(HttpContext context, string? contentType, PathString subPath)
        {
            var response = context.Response;
            var request = context.Request;
            var uri = new Uri(request.GetDisplayUrl());
            IFileInfo file;
            IFileProvider provider;
            KestrelStaticFileResponseContext staticFileResponseContext;
            KestrelStaticFileContext fileContext;
            UriBuilder uriBuilder;

            uriBuilder = new UriBuilder(uri);

            if (uriBuilder.Path.StartsWith("/") && subPath != "/")
            {
                uriBuilder.Path =  kestrelSpaOptions.DefaultPageStaticFileOptions.RequestPath + subPath;

                uri = uriBuilder.Uri;
            }

            (file, provider) = GetFileAndProvider(context, uri);

            staticFileResponseContext = new KestrelStaticFileResponseContext(context, file);
            fileContext = new KestrelStaticFileContext(context, kestrelOptions, logger, provider, contentType, uriBuilder.Path);

            if (!kestrelCliResponseHandler.PrepareResponse(staticFileResponseContext))
            {
                if (!fileContext.LookupFileInfo())
                {
                    logger.LogError($"FileNotFound: {0}", fileContext.SubPath);
                }
                else
                {
                    // If we get here, we can try to serve the file
                    await fileContext.ServeStaticFile(context, next);
                }

                await next(context);
            }
            else
            {
                //logger.LogInformation($"Handled by kestrelCliResponseHandler:{0}", fileContext.SubPath);
                logger.LogWarning($"Handled by: {0} for {1}", kestrelCliResponseHandler.GetType().FullName, fileContext.SubPath);
            }

            await Task.CompletedTask;
        }

        private (IFileInfo, IFileProvider) GetFileAndProvider(HttpContext context, Uri uri)
        {
            var provider = Helpers.ResolveFileProvider(environment);
            IFileInfo fileInfo;

            if (provider == null)
            {
                throw new FileNotFoundException($"Cannot find provider for {uri}");
            }

            fileInfo = provider.GetFileInfo(uri.AbsolutePath);

            if (fileInfo == null)
            {
                throw new FileNotFoundException($"Cannot find file for {uri}");
            }

            return (fileInfo, provider);
        }
    }
}