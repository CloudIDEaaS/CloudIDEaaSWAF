using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Nest;
using System.Collections;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Utils;
using Utils.FileTypeMatcher;
using WebSecurity.Interfaces;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public static class WebContextExtensions
    {
        public static ThreadLocal<ContextMatches> contextMatchesThreadLocal = new ThreadLocal<ContextMatches>(() => new ContextMatches());

        public static string AppendAdditionalInfo(this HttpContext httpContext, string logMessage, IMaxMindProxy maxMindProxy, IHostEnvironment environment)
        {
            var connectionInfo = httpContext.GetConnectionInfo(maxMindProxy, environment);
            var json = JsonExtensions.ToJsonText(connectionInfo);

            logMessage += json;

            return logMessage;
        }

        public static ConnectionInfo GetConnectionInfo(this HttpContext httpContext, IMaxMindProxy maxMindProxy, IHostEnvironment environment)
        {
            var connection = httpContext.Connection;
            var request = httpContext.Request;
            var ipAddress = connection.RemoteIpAddress?.GetPublicIPV4().ToString();
            ConnectionInfo connectionInfo;

            connectionInfo = new ConnectionInfo
            {
                ConnectionId = connection.Id.ToString(),
                IpAddress = ipAddress.ToString(),
                Port = connection.RemotePort.ToString(),
                Host = request.Headers["Host"].SingleOrDefault(),
                Referer = request.Headers["Referer"].SingleOrDefault(),
                UserAgent = request.Headers["User-Agent"].SingleOrDefault(),
                Origin = request.Headers["Origin"].SingleOrDefault(),
                FetchSite = request.Headers["Sec-Fetch-Site"].SingleOrDefault(),
                FetchMode = request.Headers["Sec-Fetch-Mode"].SingleOrDefault(),
                FetchDestination = request.Headers["Sec-Fetch-Dest"].SingleOrDefault(),
                Location = maxMindProxy.GetCity(connection.RemoteIpAddress)?.ToString(),
                User = httpContext.User?.Identity?.Name,
                ApplicationName = environment.ApplicationName,
                EnvironmentName = environment.EnvironmentName
            };

            return connectionInfo;
        }

        public static string GetLine(this DictionaryEntry entry)
        {
            var line = string.Empty;

            if (entry.Key is (int, string))
            {
                var tuple = ((int, string)) entry.Key!;
                line = tuple.Item2;
            }

            if (!((string?) entry.Value).IsNullOrEmpty())
            {
                if (line.Length > 0)
                {
                    line += " " + (string)entry.Value!;
                }
                else
                {
                    line += (string)entry.Value!;
                }
            }

            return line;
        }

        public static T? GetTransactionVariable<T>(this HttpContext _, string variableName)
        {
            if (contextMatchesThreadLocal.Value.TransactionVars.ContainsKey(variableName))
            {
                return (T?)contextMatchesThreadLocal.Value.TransactionVars[variableName];
            }

            return default(T);
        }

        public static string GetRawUri(this HttpRequest request)
        {
            return request.GetDisplayUrl();
        }

        public static string GetUri(this HttpRequest request)
        {
            var httpContext = request.HttpContext;

            var requestFeature = httpContext.Features.Get<IHttpRequestFeature>();

            return requestFeature.RawTarget;
        }

        public static string GetUriBaseName(this HttpRequest request)
        {
            var uri = new Uri(request.GetRawUri());

            return uri.Segments.Last();
        }

        public static string GetUriFileName(this HttpRequest request)
        {
            var uri = new Uri(request.GetRawUri());

            return uri.LocalPath;
        }

        public static string GetBody(this HttpRequest request)
        {
            string content;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            request.EnableBuffering();

            request.Body.Position = 0;

            _ = request.Body.ReadAsync(buffer, 0, buffer.Length);

            content = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return content;
        }

        public static string PrepareResponseBodyForCaptureAndCallNext(this HttpContext context, RequestDelegate next)
        {
            var response = context.Response;
            var originalBody = context.Response.Body;
            string responseBody;

            using (var memStream = new MemoryStream())
            {
                response.Body = memStream;

                next(context);

                memStream.Rewind();

                responseBody = new StreamReader(memStream).ReadToEnd();

                memStream.Position = 0;
                memStream.CopyToAsync(originalBody);
            }

            return responseBody;
        }

        public static string GetRemoteAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.GetPublicIPV4().ToString();
        }

        public static string GetIpAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.GetPublicIPV4().ToString();
        }


        public static string? GetAuthType(this HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            IEnumerable<KeyValuePair<string, StringValues>> authHeaders;
            string? type = null;

            if (response.HasStarted)
            {
                authHeaders = request.Headers.Where(h => h.Key == "Authorization");
            }
            else
            {
                authHeaders = request.Headers.Where(h => h.Key == "Authorization");
            }

            if (authHeaders.Any())
            {
                var authHeaderValue = (string)authHeaders.ElementAt(0).Value!;
                type = authHeaderValue.RegexGet(@"^(?<type>\w+) .*$", "type");
            }

            return type;
        }

        public static string? GetAuthType(this HttpRequest request)
        {
            var authHeaders = request.Headers.Where(h => h.Key == "Authorization");
            string? type = null;

            if (authHeaders.Any())
            {
                var authHeaderValue = (string)authHeaders.ElementAt(0).Value!;
                type = authHeaderValue.RegexGet(@"^(?<type>\w+) .*$", "type");
            }

            return type;
        }

        public static string? GetAuthType(this HttpResponse response)
        {
            var authHeaders = response.Headers.Where(h => h.Key == "Authorization");
            string? type = null;

            if (authHeaders.Any())
            {
                var authHeaderValue = (string)authHeaders.ElementAt(0).Value!;
                type = authHeaderValue.RegexGet("^(?<type>.*$) .*$", "type");
            }

            return type;
        }

        public static string GetBody(this HttpResponse response)
        {
            string content;

            if (response.Body is MemoryStream memStream)
            {
                memStream.Rewind();

                content = new StreamReader(memStream).ReadToEnd();
                memStream.Rewind();

                return content;
            }

            throw new IOException("Cannot get body of response unless first prepared with PrepareResponseBodyForCapture");
        }

        public static int GetRequestArgsCombinedSize(this HttpContext httpContext)
        {
            var argsCombinedSize = 0;
            var argsCombined = new List<string>();
            var request = httpContext.Request;

            if (request.ContentLength > 0)
            {
                var formValueResult = new List<string>();
                var queryValueResult = new List<string>();
                var formNameResult = new List<string>();
                var queryNameResult = new List<string>();
                Dictionary<string, StringValues> form;
                FormPipeReader formPipeReader;

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(request.BodyReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();
                formValueResult = form.Select(a => (string?)a.Value).ToList()!;
                formNameResult = form.Select(a => (string?)a.Key).ToList()!;

                if (request.QueryString.HasValue)
                {
                    queryValueResult = request.Query.Select(a => (string?)a.Value).ToList()!;
                    queryNameResult = request.Query.Select(a => (string?)a.Key).ToList()!;
                }

                argsCombined.AddRange(formValueResult.Concat(formNameResult).Concat(queryValueResult).Concat(queryNameResult));
            }

            argsCombinedSize = argsCombined.Sum(a => a.Length);

            return argsCombinedSize;
        }

        public static Dictionary<string, string> GetMultiPartHeaders(this HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.ContentType == MediaTypeNames.Multipart.FormData)
            {
                return request.Headers.ToDictionary(h => h.Key, h => (string)h.Value!);
            }

            return new Dictionary<string, string>();
        }

        public static long GetFilesCombinedSize(this HttpContext httpContext)
        {
            var request = httpContext.Request;

            return request.Form.Files.Sum(f => f.Length);
        }

        public static KeyValuePair<string, string?> GetLastMatchedVar(this HttpContext _)
        {
            return contextMatchesThreadLocal.Value.MatchedVar;
        }

        public static string GetLastMatchedVarName(this HttpContext _)
        {
            return contextMatchesThreadLocal.Value.MatchedVarName;
        }

        public static Dictionary<string, string?> GetLastMatchedVars(this HttpContext _)
        {
            return contextMatchesThreadLocal.Value.MatchedVars;
        }

        public static List<string> GetLastMatchedVarsNames(this HttpContext _)
        {
            return contextMatchesThreadLocal.Value.MatchedVarNames;
        }

        public static string? GetLastCapturedGroup(this HttpContext _, int index)
        {
            var pair = MicroRulesEngine.GroupCaptures.ElementAtOrDefault(index);

            return pair.Value;
        }

        public static string GetUniqueId(this HttpContext _)
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetTime(this HttpContext _)
        {
            return DateTime.UtcNow.ToString("HH:mm:ss");
        }

        public static string GetTimeDay(this HttpContext _)
        {
            return DateTime.UtcNow.Day.ToString();
        }

        public static string GetTimeEpoch(this HttpContext _)
        {
            return DateTime.UtcNow.ToEpocTime().ToString();
        }

        public static string GetTimeHour(this HttpContext _)
        {
            return DateTime.UtcNow.Hour.ToString();
        }

        public static string GetTimeMinute(this HttpContext _)
        {
            return DateTime.UtcNow.Minute.ToString();
        }

        public static string GetTimeMonth(this HttpContext _)
        {
            return DateTime.UtcNow.Month.ToString();
        }

        public static string GetTimeWeekday(this HttpContext _)
        {
            return ((int) DateTime.UtcNow.DayOfWeek).ToString();
        }

        public static string GetTimeYear(this HttpContext _)
        {
            return DateTime.UtcNow.Year.ToString();
        }

        public static Dictionary<string, string?> GetRequestArgs(this HttpContext httpContext)
        {
            var args = new Dictionary<string, string?>();
            var request = httpContext.Request;

            if (request.ContentLength > 0)
            {
                var formResult = new Dictionary<string, string?>();
                var queryResult = new Dictionary<string, string?>();
                var bodyResult = new Dictionary<string, string?>();
                Dictionary<string, StringValues> form;
                RangeFileTypeMatcher rangeFileTypeMatcher;
                FormPipeReader formPipeReader;
                string content;

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(request.BodyReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();
                formResult = form.ToDictionary(a => a.Key, a => (string?)a.Value);

                if (request.QueryString.HasValue)
                {
                    queryResult = request.Query.ToDictionary(a => a.Key, a => (string?)a.Value);
                }

                rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("{", "}", true), 1024, 1024 * 2);

                if (rangeFileTypeMatcher.Matches(request.Body))
                {
                    Exception exception;

                    content = request.GetBody();

                    if (JsonExtensions.IsValidJson(content, out exception))
                    {
                        bodyResult = JsonExtensions.GetAllJsonPropertyValues(content);
                    }
                }

                rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("<", ">", true), 1024, 1024 * 2);

                if (rangeFileTypeMatcher.Matches(request.Body))
                {
                    XDocument document;

                    content = request.GetBody();

                    if (XmlExtensions.IsValidXml(content))
                    {
                        try
                        {
                            IEnumerable<XElement> elements;
                            XElement root;

                            document = XDocument.Parse(content);

                            root = document.Root!;

                            if (!root.HasElements)
                            {
                                bodyResult = root.Attributes().ToDictionary(a => a.Name.LocalName, a => (string?) a.Value).Append(new KeyValuePair<string, string?>(document.Root.Name.LocalName, document.Root.Value)).ToDictionary();
                            }
                            else
                            {
                                elements = document.Descendants().Where(e => !e.HasElements);
                                bodyResult = elements.SelectMany(e => e.Attributes().ToDictionary(a => a.Name.LocalName, a => (string?) a.Value).Append(new KeyValuePair<string, string?>(e.Name.LocalName, e.Value))).Where(e => e.Value.Length > 0).ToDictionary();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                request.Body.Position = 0;

                args.AddRange(formResult.Concat(bodyResult).Concat(queryResult).ToDictionary());
            }

            return args;
        }

        public static string? GetRequestBodyProcessor(this HttpContext httpContext)
        {
            var mimeType = httpContext.GetRequestMimeType();
            string? processor = null;

            if (mimeType != null)
            {
                processor = mimeType.RemoveEndAtFirstChar('/').ToUpper();

                if (processor == "Application")
                {
                    processor = mimeType.RemoveStartBefore("/").ToUpper();
                }
            }

            return processor.NullToEmpty();
        }

        public static List<string> GetFiles(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            var files = new List<string>();

            if (request.ContentLength > 0)
            {
                Dictionary<string, StringValues> form;
                FormPipeReader formPipeReader;
                var body = request.Body;
                var pipeReader = new KestrelStreamPipeReader(body);

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(pipeReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();

                if (form.Count > 0)
                {
                    var bodyText = request.GetBody();
                    var lines = bodyText.GetLines().ToList();
                    var lineCount = lines.Count();

                    if (lineCount > 0)
                    {
                        var regexFiles = new Regex("filename=\"(?<filename>.*?)\"", RegexOptions.Multiline | RegexOptions.Singleline);

                        if (regexFiles.IsMatch(bodyText))
                        {
                            var matches = regexFiles.Matches(bodyText);

                            foreach (var match in matches.Cast<Match>())
                            {
                                var fileName = match.GetGroupValue("filename");

                                files.Add(fileName);
                            }
                        }
                    }
                }

                body.Position = 0;
            }

            return files;
        }

        public static List<string> GetFilesNames(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            var names = new List<string>();

            if (request.ContentLength > 0)
            {
                Dictionary<string, StringValues> form;
                FormPipeReader formPipeReader;
                var body = request.Body;
                var pipeReader = new KestrelStreamPipeReader(body);

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(pipeReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();

                if (form.Count > 0)
                {
                    var bodyText = request.GetBody();
                    var lines = bodyText.GetLines().ToList();
                    var lineCount = lines.Count();

                    if (lineCount > 0)
                    {
                        var regexFiles = new Regex("(?<!file)name=\"(?<name>.*?)\"", RegexOptions.Multiline | RegexOptions.Singleline);

                        if (regexFiles.IsMatch(bodyText))
                        {
                            var matches = regexFiles.Matches(bodyText);

                            foreach (var match in matches.Cast<Match>())
                            {
                                var name = match.GetGroupValue("name");

                                names.Add(name);
                            }
                        }
                    }
                }

                body.Position = 0;
            }

            return names;
        }
        public static string? GetRequestMimeType(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            string? processorName = null;

            if (request.ContentLength > 0)
            {
                Dictionary<string, StringValues> form;
                FormPipeReader formPipeReader;
                FileType fileType;
                var body = request.Body;
                var pipeReader = new KestrelStreamPipeReader(body);

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(pipeReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();

                if (form.Count > 0)
                {
                    var bodyText = request.GetBody();
                    var lines = bodyText.GetLines().ToList();
                    var lineCount = lines.Count();

                    if (lineCount > 0) 
                    {
                        processorName = MediaTypeNames.Multipart.FormData;
                    }
                    else
                    {
                        processorName = MediaTypeNames.Application.FormUrlEncoded;
                    }
                }
                else
                {
                    body.Position = 0;

                    fileType = body.GetFileType();

                    switch (fileType.Name)
                    {
                        case "Bitmap":
                            processorName = MediaTypeNames.Image.Bmp;
                            break;
                        case "Portable Network Graphic":
                            processorName = MediaTypeNames.Image.Png;
                            break;
                        case "JPEG":
                            processorName = MediaTypeNames.Image.Jpeg;
                            break;
                        case "Graphics Interchange Format 87a":
                        case "Graphics Interchange Format 89a":
                            processorName = MediaTypeNames.Image.Gif;
                            break;
                        case "Portable Document Format":
                            processorName = MediaTypeNames.Application.Pdf;
                            break;
                        case "Xml":
                            processorName = MediaTypeNames.Application.Xml;
                            break;
                        case "Json":
                            processorName = MediaTypeNames.Application.Json;
                            break;
                        default:
                            DebugUtils.Break();
                            break;
                    }
                }

                body.Position = 0;
            }

            return processorName;
        }

        public static void AddMatchedVar(this HttpContext _, KeyValuePair<string, string?> value)
        {
            contextMatchesThreadLocal.Value.MatchedVar = new KeyValuePair<string, string?>(value.Key, value.Value);
            contextMatchesThreadLocal.Value.MatchedVarName = value.Key;
        }

        public static void AddMatchedVar(this HttpContext _, string variable, string? value)
        {
            contextMatchesThreadLocal.Value.MatchedVar = new KeyValuePair<string, string?>(variable, value);
            contextMatchesThreadLocal.Value.MatchedVarName = variable;
        }

        public static void AddMatchedVars(this HttpContext _, Dictionary<string, string?> vars)
        {
            contextMatchesThreadLocal.Value.MatchedVars = vars;
            contextMatchesThreadLocal.Value.MatchedVarNames = vars.Keys.ToList();

            _.AddMatchedVar(vars.LastOrDefault());
            _.AddMatchedVarName(vars.Keys.LastOrDefault());
        }

        public static ContextMatches GetContextMatches()
        {
            return contextMatchesThreadLocal.Value!;
        }

        public static void AddMatchedVarNames(this HttpContext _, List<string> varNames)
        {
            contextMatchesThreadLocal.Value.MatchedVarNames = varNames;

            _.AddMatchedVarName(varNames.LastOrDefault());
        }

        public static void AddMatchedVarName(this HttpContext _, string varName)
        {
            contextMatchesThreadLocal.Value.MatchedVarName = varName;
        }

        public static Dictionary<string, string?> GetRequestArgsGet(this HttpContext httpContext)
        {
            var args = new Dictionary<string, string?>();
            var request = httpContext.Request;
            var queryResult = new Dictionary<string, string?>();

            if (request.QueryString.HasValue)
            {
                queryResult = request.Query.ToDictionary(a => a.Key, a => (string?) a.Value);
            }

            args.AddRange(queryResult);

            contextMatchesThreadLocal.Value.MatchedVars = args;
            contextMatchesThreadLocal.Value.MatchedVar = args.Last();

            return args;
        }

        public static List<string> GetRequestArgsNames(this HttpContext httpContext)
        {
            var argsNames = new List<string>();
            var request = httpContext.Request;

            if (request.ContentLength > 0)
            {
                var formResult = new List<string>();
                var queryResult = new List<string>();
                var bodyResult = new List<string>();
                Dictionary<string, StringValues> form;
                RangeFileTypeMatcher rangeFileTypeMatcher;
                FormPipeReader formPipeReader;
                string content;

                request.EnableBuffering();

                formPipeReader = new FormPipeReader(request.BodyReader);

                form = formPipeReader.ReadFormAsync().GetAwaiter().GetResult();
                formResult = form.Select(a => (string?)a.Key).ToList()!;

                if (request.QueryString.HasValue)
                {
                    queryResult = request.Query.Select(a => (string?)a.Key).ToList()!;
                }

                rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("{", "}", true), 1024, 1024 * 2);

                if (rangeFileTypeMatcher.Matches(request.Body))
                {
                    Exception exception;

                    content = request.GetBody();

                    if (JsonExtensions.IsValidJson(content, out exception))
                    {
                        bodyResult = JsonExtensions.GetAllJsonPropertyNames(content);
                    }
                }

                rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("<", ">", true), 1024, 1024 * 2);

                if (rangeFileTypeMatcher.Matches(request.Body))
                {
                    XDocument document;

                    content = request.GetBody();

                    if (XmlExtensions.IsValidXml(content))
                    {
                        try
                        {
                            IEnumerable<XElement> elements;
                            XElement root;

                            document = XDocument.Parse(content);

                            root = document.Root!;

                            if (!root.HasElements)
                            {
                                bodyResult = root.Attributes().Select(a => a.Name.LocalName).Append(document.Root.Name.LocalName).ToList()!;
                            }
                            else
                            {
                                elements = document.Descendants().Where(e => !e.HasElements);
                                bodyResult = elements.SelectMany(e => e.Attributes().Select(a => a.Name.LocalName).Append(e.Name.LocalName)).Where(e => e.Length > 0).ToList()!;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                request.Body.Position = 0;

                argsNames.AddRange(formResult.Concat(bodyResult).Concat(queryResult));
            }

            return argsNames;
        }

        public static string? GetXml(this HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.ContentLength > 0)
            {
                RangeFileTypeMatcher rangeFileTypeMatcher;
                string content;

                request.EnableBuffering();

                rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("<", ">", true), 1024, 1024 * 2);

                if (rangeFileTypeMatcher.Matches(request.Body))
                {
                    XDocument document;

                    content = request.GetBody();

                    if (XmlExtensions.IsValidXml(content))
                    {
                        try
                        {
                            document = XDocument.Parse(content);

                            return content;
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                }

                request.Body.Position = 0;
            }

            return null;
        }

        public static List<string> GetRequestArgsGetNames(this HttpContext httpContext)
        {
            var argsNames = new List<string>();
            var request = httpContext.Request;
            var queryResult = new List<string>();

            if (request.QueryString.HasValue)
            {
                queryResult = request.Query.Select(a => (string?)a.Key).ToList()!;
            }

            argsNames.AddRange(queryResult);

            contextMatchesThreadLocal.Value.MatchedVarNames = argsNames;
            contextMatchesThreadLocal.Value.MatchedVarName = argsNames.Last();

            return argsNames;
        }
    }
}
