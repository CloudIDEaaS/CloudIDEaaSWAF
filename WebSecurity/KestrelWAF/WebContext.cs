using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Utils;
using Utils.FileTypeMatcher;
using WebSecurity.Interfaces;
using WebSecurity.Models;
using WebSecurity.KestrelWAF.RulesEngine;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;
using BTreeIndex.Collections.Generic.BTree;
using WebSecurity.Transformations;
using System.Reflection;

namespace WebSecurity.KestrelWAF;

public partial class WebContext : IDisposable
{
    protected readonly IMemoryCache cache;
    protected readonly IMaxMindProxy maxMindProxy;
    protected readonly ThreadCacheServiceProvider serviceProvider;
    private readonly IList<Rule> rules;
    private readonly ITransformationsActionsProvider? transActionsProvider;
    protected readonly HttpRequest request;
    protected readonly HttpResponse response;
    protected string ipCountry;
    private BTreeDictionary<string, IConfigurationSection> rulesConfigIndex;
    public HttpContext HttpContext { get; }

    public WebContext(HttpContext httpContext, IMemoryCache cache, IMaxMindProxy maxMindProxy, ThreadCacheServiceProvider serviceProvider, BTreeDictionary<string, IConfigurationSection> rulesConfigIndex, IList<Rule> rules)
    {
        this.HttpContext = httpContext;
        this.request = httpContext.Request;
        this.response = httpContext.Response;
        this.cache = cache;
        this.maxMindProxy = maxMindProxy;
        this.serviceProvider = serviceProvider;
        this.rulesConfigIndex = rulesConfigIndex;
        this.rules = rules;

        transActionsProvider = serviceProvider.GetService<ITransformationsActionsProvider>();
    }

    public Rule CurrentRule
    {
        get
        {
            return MicroRulesEngine.ExecutedRules.Peek();
        }
    }

    public List<Func<object, object>> Transformations
    {
        get
        {
            var currentRule = this.CurrentRule;
            var ruleConfig = rulesConfigIndex[currentRule.Id];
            var transformationsActions = ruleConfig.GetSection("TransformationsActions").ToJson();
            var transformations = transformationsActions.ToArray().OfType<JProperty>().Where(p => p.Name == "transformations").SelectMany(p => p.Value.Children()).Select(t => t.ToObject<string>()!).ToList();

            return typeof(TransformationFunctions).GetMethods().Where(m => m.HasCustomAttribute<TransformationAttribute>() && transformations.Any(t => m.GetCustomAttribute<TransformationAttribute>().TransformationType.ToString().AsCaseless() == t)).Select(m => m.CreateDelegate<Func<object, object>>()).ToList();
        }
    }

    public Dictionary<string, string> GroupCaptures
    {
        get
        {
            return MicroRulesEngine.GroupCaptures;
        }
    }
    
    public ContextMatches ContextMatches
    {
        get
        {
            return WebContextExtensions.GetContextMatches();
        }
    }

    public bool PartialMatchFromFile(string fileName, string searchText)
    {
        var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
        var settings = repository.GetSettings();
        var inFile = settings.PartialMatchFromFile(fileName, searchText);

        return inFile;
    }

    public bool PartialMatchFromFile(string fileName, Dictionary<string, string> dictionary)
    {
        var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
        var settings = repository.GetSettings();

        this.HttpContext.AddMatchedVar(nameof(PartialMatchFromFile), fileName);

        using (settings.IndexFile(fileName))
        {
            foreach (var pair in dictionary)
            {
                var inFile = settings.PartialMatchFromFile(fileName, pair.Key);

                if (inFile)
                {
                    return true;
                }

                inFile = settings.PartialMatchFromFile(fileName, pair.Value);

                if (inFile)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool PartialMatchFromFile(string fileName, IEnumerable<string> enumerable)
    {
        var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
        var settings = repository.GetSettings();

        using (settings.IndexFile(fileName))
        {
            foreach (var item in enumerable)
            {
                var inFile = settings.PartialMatchFromFile(fileName, item);

                if (inFile)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Rule: ************************* 100027 *************************

    public long? CombinedFileSizes()
    {
        var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
        var settings = repository.GetSettings();

        return (long?)settings["CombinedFileSizes"];
    }

    // Rule: ************************* 100027 *************************

    public long FilesCombinedSize
    {
        get
        {
            return this.HttpContext.GetFilesCombinedSize();
        }
    }

    public string? XFileName
    {
        get
        {
            return request.Headers.ToDictionary(h => h.Key, h => (string?)h.Value)["X-File-Name"];
        }
    }

    public bool XFileNameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
    {
        var captures = this.GroupCaptures;
        var rule = this.CurrentRule;

        if (!disabled)
        {
            var transActionsProvider = serviceProvider.GetService<ITransformationsActionsProvider>();

            transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
        }

        return !disabled;
    }

    // Rule: ************************* 100023 *************************

    public int? GetArgLength()
    {
        var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
        var settings = repository.GetSettings();

        return (int?)settings["ArgLength"];
    }

    public int GetParamCounterForRegexPattern(string pattern)
    {
        throw new NotImplementedException();
    }

    public string GetRfiParameterForRegexPattern(string pattern)
    {
        throw new NotImplementedException();
    }

    public string? Xml
    {
        get
        {
            var xml = this.HttpContext.GetXml();

            return xml;
        }
    }

    public string? ContentLength
    {
        get
        {
            return request.Headers.ToDictionary(h => h.Key, h => (string?)h.Value).GetDefault("Content-Length", string.Empty);
        }
    }

    public string RemoteIp
    {
        get 
        {
            return this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString(); 
        }
    }

    public bool Authenticated
    {
        get 
        { 
            return this.HttpContext.Request.HttpContext.User.Identity.IsAuthenticated; 
        }
    }

    public string? IpCountry
    {
        get
        {
            if (ipCountry == null)
            {
                var context = this.HttpContext;
                var connection = context.Connection;
                var ipAddress = connection.RemoteIpAddress.GetPublicIPV4();
                var cityResponse = maxMindProxy.GetCity(ipAddress);

                if (cityResponse != null)
                {
                    var country = cityResponse.Country.Name;
                    ipCountry = cityResponse.Country.IsoCode!;
                }
            }

            return ipCountry;
        }
    }

    public bool InSubnet(string ip, int mask)
    {
        var network = new IPNetwork(IPAddress.Parse(ip), mask);
        return network.Contains(this.HttpContext.Connection.RemoteIpAddress);
    }

    public bool IpInFile(string path)
    {
        var keyname = System.IO.Path.GetFileNameWithoutExtension(path);
        var data = cache.Get<IEnumerable<string>>(keyname);

        if (data == null && File.Exists(path))
        {
            data = cache.Set<IEnumerable<string>>(keyname, File.ReadAllLines(path), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
        }

        return data?.Contains(RemoteIp, StringComparer.OrdinalIgnoreCase) ?? false;
    }

    public List<string> GetAllRequestArgs()
    {
        var args = new List<string>();
        var request = this.HttpContext.Request;

        if (request.ContentLength > 0)
        {
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
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
            formResult = form.Select(a => (string?)a.Value).ToList()!;

            if (request.QueryString.HasValue)
            {
                queryResult = request.Query.Select(a => (string?)a.Value).ToList()!;
            }

            rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("{", "}", true), 1024, 1024 * 2);

            if (rangeFileTypeMatcher.Matches(request.Body))
            {
                Exception exception;

                request.Body.Position = 0;

                _ = request.Body.ReadAsync(buffer, 0, buffer.Length);

                content = Encoding.UTF8.GetString(buffer);

                if (JsonExtensions.IsValidJson(content, out exception))
                {
                    bodyResult = JsonExtensions.GetAllJsonValues(content);
                }
            }

            rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("<", ">", true), 1024, 1024 * 2);

            if (rangeFileTypeMatcher.Matches(request.Body))
            {
                XDocument document;

                request.Body.Position = 0;

                _ = request.Body.ReadAsync(buffer, 0, buffer.Length);

                content = Encoding.UTF8.GetString(buffer);

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
                            bodyResult = root.Attributes().Select(a => a.Value).Append(document.Root.Value).ToList()!;
                        }
                        else
                        {
                            elements = document.Descendants().Where(e => !e.HasElements);
                            bodyResult = elements.SelectMany(e => e.Attributes().Select(a => a.Value).Append(e.Value)).Where(e => e.Length > 0).ToList()!;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            request.Body.Position = 0;

            args.AddRange(formResult.Concat(bodyResult).Concat(queryResult));
        }

        return args;
    }

    public void Dispose()
    {
        serviceProvider.Dispose();
    }
}
