// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Utils;

namespace Utils.Kestrel;

internal struct KestrelStaticFileContext
{
    private readonly HttpContext context;
    private readonly KestrelStaticFileOptions options;
    private readonly HttpRequest request;
    private readonly HttpResponse response;
    private readonly ILogger logger;
    private readonly IFileProvider fileProvider;
    private readonly string method;
    private readonly string? contentType;

    private IFileInfo fileInfo;
    private EntityTagHeaderValue? etag;
    private RequestHeaders? requestHeaders;
    private ResponseHeaders? responseHeaders;
    private RangeItemHeaderValue? range;

    private long length;
    private readonly PathString subPath;
    private DateTimeOffset lastModified;

    private PreconditionState ifMatchState;
    private PreconditionState ifNoneMatchState;
    private PreconditionState ifModifiedSinceState;
    private PreconditionState ifUnmodifiedSinceState;

    private RequestType requestType;

    public KestrelStaticFileContext(HttpContext context, KestrelStaticFileOptions options, ILogger logger, IFileProvider fileProvider, string? contentType, PathString subPath)
    {
        if (subPath.Value == null)
        {
            throw new ArgumentException($"{nameof(subPath)} cannot wrap a null value.", nameof(subPath));
        }

        this.context = context;
        this.options = options;
        this.request = context.Request;
        this.response = context.Response;
        this.logger = logger;
        this.fileProvider = fileProvider;
        this.method = request.Method;
        this.contentType = contentType;
        this.fileInfo = default!;
        this.etag = null;
        this.requestHeaders = null;
        this.responseHeaders = null;
        this.range = null;

        this.length = 0;
        this.subPath = subPath;
        this.lastModified = new DateTimeOffset();
        this.ifMatchState = PreconditionState.Unspecified;
        this.ifNoneMatchState = PreconditionState.Unspecified;
        this.ifModifiedSinceState = PreconditionState.Unspecified;
        this.ifUnmodifiedSinceState = PreconditionState.Unspecified;

        if (HttpMethods.IsGet(method))
        {
            requestType = RequestType.IsGet;
        }
        else if (HttpMethods.IsHead(method))
        {
            requestType = RequestType.IsHead;
        }
        else
        {
            requestType = RequestType.Unspecified;
        }
    }

    private RequestHeaders RequestHeaders => requestHeaders ??= request.GetTypedHeaders();

    private ResponseHeaders ResponseHeaders => responseHeaders ??= response.GetTypedHeaders();

    public bool IsHeadMethod => requestType.HasFlag(RequestType.IsHead);

    public bool IsGetMethod => requestType.HasFlag(RequestType.IsGet);

    public bool IsRangeRequest
    {
        get => requestType.HasFlag(RequestType.IsRange);
        private set
        {
            if (value)
            {
                requestType |= RequestType.IsRange;
            }
            else
            {
                requestType &= ~RequestType.IsRange;
            }
        }
    }

    public string SubPath => subPath.Value!;

    public string PhysicalPath => fileInfo.PhysicalPath ?? string.Empty;

    public bool LookupFileInfo()
    {
        fileInfo = fileProvider.GetFileInfo(SubPath);
        if (fileInfo.Exists)
        {
            length = fileInfo.Length;

            DateTimeOffset last = fileInfo.LastModified;
            // Truncate to the second.
            lastModified = new DateTimeOffset(last.Year, last.Month, last.Day, last.Hour, last.Minute, last.Second, last.Offset).ToUniversalTime();

            long etagHash = lastModified.ToFileTime() ^ length;
            etag = new EntityTagHeaderValue('\"' + Convert.ToString(etagHash, 16) + '\"');
        }
        return fileInfo.Exists;
    }

    public void ComprehendRequestHeaders()
    {
        ComputeIfMatch();

        ComputeIfModifiedSince();

        ComputeRange();

        ComputeIfRange();
    }

    private void ComputeIfMatch()
    {
        var requestHeaders = RequestHeaders;

        // 14.24 If-Match
        var ifMatch = requestHeaders.IfMatch;
        if (ifMatch?.Count > 0)
        {
            ifMatchState = PreconditionState.PreconditionFailed;
            foreach (var etag in ifMatch)
            {
                if (etag.Equals(EntityTagHeaderValue.Any) || etag.Compare(etag, useStrongComparison: true))
                {
                    ifMatchState = PreconditionState.ShouldProcess;
                    break;
                }
            }
        }

        // 14.26 If-None-Match
        var ifNoneMatch = requestHeaders.IfNoneMatch;
        if (ifNoneMatch?.Count > 0)
        {
            ifNoneMatchState = PreconditionState.ShouldProcess;
            foreach (var etag in ifNoneMatch)
            {
                if (etag.Equals(EntityTagHeaderValue.Any) || etag.Compare(etag, useStrongComparison: true))
                {
                    ifNoneMatchState = PreconditionState.NotModified;
                    break;
                }
            }
        }
    }

    private void ComputeIfModifiedSince()
    {
        var requestHeaders = RequestHeaders;
        var now = DateTimeOffset.UtcNow;

        // 14.25 If-Modified-Since
        var ifModifiedSince = requestHeaders.IfModifiedSince;
        if (ifModifiedSince.HasValue && ifModifiedSince <= now)
        {
            bool modified = ifModifiedSince < lastModified;
            ifModifiedSinceState = modified ? PreconditionState.ShouldProcess : PreconditionState.NotModified;
        }

        // 14.28 If-Unmodified-Since
        var ifUnmodifiedSince = requestHeaders.IfUnmodifiedSince;
        if (ifUnmodifiedSince.HasValue && ifUnmodifiedSince <= now)
        {
            bool unmodified = ifUnmodifiedSince >= lastModified;
            ifUnmodifiedSinceState = unmodified ? PreconditionState.ShouldProcess : PreconditionState.PreconditionFailed;
        }
    }

    private void ComputeIfRange()
    {
        // 14.27 If-Range
        var ifRangeHeader = RequestHeaders.IfRange;
        if (ifRangeHeader != null)
        {
            // If the validator given in the If-Range header field matches the
            // current validator for the selected representation of the target
            // resource, then the server SHOULD process the Range header field as
            // requested.  If the validator does not match, the server MUST ignore
            // the Range header field.
            if (ifRangeHeader.LastModified.HasValue)
            {
                if (lastModified > ifRangeHeader.LastModified)
                {
                    IsRangeRequest = false;
                }
            }
            else if (etag != null && ifRangeHeader.EntityTag != null && !ifRangeHeader.EntityTag.Compare(etag, useStrongComparison: true))
            {
                IsRangeRequest = false;
            }
        }
    }

    private void ComputeRange()
    {
        // 14.35 Range
        // http://tools.ietf.org/html/draft-ietf-httpbis-p5-range-24

        // A server MUST ignore a Range header field received with a request method other
        // than GET.
        if (!IsGetMethod)
        {
            return;
        }

        //(var isRangeRequest, var range) = RangeHelper.ParseRange(context, RequestHeaders, length, logger);

        //range = range;
        //IsRangeRequest = isRangeRequest;
    }

    public Task ApplyResponseHeadersAsync(int statusCode)
    {
        // Only clobber the default status (e.g. in cases this a status code pages retry)
        if (response.StatusCode == StatusCodes.Status200OK)
        {
            response.StatusCode = statusCode;
        }
        if (statusCode < 400)
        {
            // these headers are returned for 200, 206, and 304
            // they are not returned for 412 and 416
            if (!string.IsNullOrEmpty(contentType))
            {
                response.ContentType = contentType;
            }

            var responseHeaders = ResponseHeaders;
            responseHeaders.LastModified = lastModified;
            responseHeaders.ETag = etag;
            responseHeaders.Headers.AcceptRanges = "bytes";
        }
        if (statusCode == StatusCodes.Status200OK)
        {
            // this header is only returned here for 200
            // it already set to the returned range for 206
            // it is not returned for 304, 412, and 416
            response.ContentLength = length;
        }

        if (options.OnPrepareResponse != KestrelStaticFileOptions.defaultOnPrepareResponse || options.OnPrepareResponseAsync != KestrelStaticFileOptions.defaultOnPrepareResponseAsync)
        {
            var context = new KestrelStaticFileResponseContext(this.context, fileInfo);
            options.OnPrepareResponse(context);
            return options.OnPrepareResponseAsync(context);
        }
        return Task.CompletedTask;
    }

    public PreconditionState GetPreconditionState() => GetMaxPreconditionState(ifMatchState, ifNoneMatchState, ifModifiedSinceState, ifUnmodifiedSinceState);

    private static PreconditionState GetMaxPreconditionState(params PreconditionState[] states)
    {
        PreconditionState max = PreconditionState.Unspecified;
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] > max)
            {
                max = states[i];
            }
        }
        return max;
    }

    public Task SendStatusAsync(int statusCode)
    {
        logger.LogInformation("SendStatus Ok: {0}, {1}", statusCode, SubPath);

        return ApplyResponseHeadersAsync(statusCode);
    }

    public async Task ServeStaticFile(HttpContext context, RequestDelegate next)
    {
        ComprehendRequestHeaders();
        switch (GetPreconditionState())
        {
            case PreconditionState.Unspecified:
            case PreconditionState.ShouldProcess:
                if (IsHeadMethod)
                {
                    await SendStatusAsync(StatusCodes.Status200OK);
                    return;
                }

                try
                {
                    if (IsRangeRequest)
                    {
                        await SendRangeAsync();
                        return;
                    }

                    await SendAsync();
                    logger.LogInformation("FileServed {0}, {1}", SubPath, PhysicalPath);
                    return;
                }
                catch (FileNotFoundException)
                {
                    context.Response.Clear();
                }
                await next(context);
                return;
            case PreconditionState.NotModified:
                logger.LogInformation("FileNotModified {0}", SubPath);
                await SendStatusAsync(StatusCodes.Status304NotModified);
                return;
            case PreconditionState.PreconditionFailed:
                logger.LogInformation("PreconditionFailed {0}", SubPath);
                await SendStatusAsync(StatusCodes.Status412PreconditionFailed);
                return;
            default:
                var exception = new NotImplementedException(GetPreconditionState().ToString());
                Debug.Fail(exception.ToString());
                throw exception;
        }
    }

    public async Task SendAsync()
    {
        SetCompressionMode();
        await ApplyResponseHeadersAsync(StatusCodes.Status200OK);
        try
        {
            await context.Response.SendFileAsync(fileInfo, 0, length, context.RequestAborted);
        }
        catch (OperationCanceledException ex)
        {
            // Don't throw this exception, it's most likely caused by the client disconnecting.
            logger.LogInformation("WriteCancelled {0}", ex);
        }
    }

    // When there is only a single range the bytes are sent directly in the body.
    internal async Task SendRangeAsync()
    {
        if (range == null)
        {
            // 14.16 Content-Range - A server sending a response with status code 416 (Requested range not satisfiable)
            // SHOULD include a Content-Range field with a byte-range-resp-spec of "*". The instance-length specifies
            // the current length of the selected resource.  e.g. */length
            ResponseHeaders.ContentRange = new ContentRangeHeaderValue(this.length);
            await ApplyResponseHeadersAsync(StatusCodes.Status416RangeNotSatisfiable);

            logger.LogInformation("RangeNotSatisfiable{0}", SubPath);
            return;
        }

        ResponseHeaders.ContentRange = ComputeContentRange(range, out var start, out var length);
        response.ContentLength = length;
        SetCompressionMode();
        await ApplyResponseHeadersAsync(StatusCodes.Status206PartialContent);

        try
        {
            var logPath = !string.IsNullOrEmpty(fileInfo.PhysicalPath) ? fileInfo.PhysicalPath : SubPath;
            logger.LogInformation("SendingFileRange {0}, {1}", response.Headers.ContentRange, logPath);
            await context.Response.SendFileAsync(fileInfo, start, length, context.RequestAborted);
        }
        catch (OperationCanceledException ex)
        {
            // Don't throw this exception, it's most likely caused by the client disconnecting.
            logger.LogInformation("WriteCancelled {0}", ex);
        }
    }

    // Note: This assumes ranges have been normalized to absolute byte offsets.
    private ContentRangeHeaderValue ComputeContentRange(RangeItemHeaderValue range, out long start, out long length)
    {
        start = range.From!.Value;
        var end = range.To!.Value;
        length = end - start + 1;
        return new ContentRangeHeaderValue(start, end, length);
    }

    // Only called when we expect to serve the body.
    private void SetCompressionMode()
    {
        var responseCompressionFeature = context.Features.Get<IHttpsCompressionFeature>();
        if (responseCompressionFeature != null)
        {
            responseCompressionFeature.Mode = options.HttpsCompression;
        }
    }

    internal enum PreconditionState : byte
    {
        Unspecified,
        NotModified,
        ShouldProcess,
        PreconditionFailed
    }

    [Flags]
    private enum RequestType : byte
    {
        Unspecified = 0b_000,
        IsHead = 0b_001,
        IsGet = 0b_010,
        IsRange = 0b_100,
    }
}