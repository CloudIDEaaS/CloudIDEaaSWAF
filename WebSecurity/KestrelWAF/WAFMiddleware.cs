using BTreeIndex.Collections.Generic.BTree;
using MaxMind.GeoIP2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Shell.Interop;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Utils;
using Utils.Kestrel;
using WebSecurity.Chains;
using WebSecurity.Interfaces;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF;

public class WAFMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<WAFMiddleware> logger;
    private readonly IMaxMindProxy maxMindProxy;
    private readonly IMemoryCache cache;
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment environment;
    private readonly IApplicationLifetime applicationLifetime;
    private readonly bool verboseOutput;
    private readonly IConfigurationSection rulesConfig;
    private BTreeDictionary<string, IConfigurationSection> rulesConfigIndex;
    private bool enableWaf;
    private TimeSpan wafThrottle;
    private readonly IList<Rule> rules;
    private readonly Func<WebContext, bool> compiledRule;
    private static ThreadLocal<Dictionary<string, RuleChainSet>> ruleChainsThreadLocal = new ThreadLocal<Dictionary<string, RuleChainSet>>(() => new Dictionary<string, RuleChainSet>());
    private static ThreadLocal<ChainSetState> chainSetStateThreadLocal = new ThreadLocal<ChainSetState>(() => new ChainSetState());
    private static ThreadLocal<WAFSessionInfo> sessionInfoThreadLocal;
    private bool useChainAnimation;
    private Thread chainAnimationThread;
    private IManagedLockObject lockObject;
    private bool createContextSpecificReport;
    private string? reportsPath;
    private ActionQueueService actionQueueService;
    private bool exitChainAnimation;
    private TimeSpan reportRolloverGranularityTimeSpan;
    private bool captureRequestResponse;
    private int reportQueueMax;
    private int maxReportCount;
    private TimeSpan maxReportDuration;
    private int cityFloorLimit;
    private int provinceFloorLimit;
    private int cityCeilingLimit;
    private int provinceCeilingLimit;
    private int geoIterationCycle;
    private int geoIterationCycleMax;
    private IManagedMutexObject managedMutexObject;
    public const string REPORT_MUTEX_NAME = "CloudIDEaaSWAFMutex";
    private Queue<IPAddressBlockedFailSuccessCount> ipAddressBlockedFailCountQueue;

    public WAFMiddleware(RequestDelegate next, IMaxMindProxy maxMindProxy, IMemoryCache cache, IConfiguration configuration, IHostEnvironment environment, IApplicationLifetime applicationLifetime, IOptions<Rule> ruleset, IServiceProvider serviceProvider, ILogger<WAFMiddleware> logger)
    {
        string? wafEnableConfigValue;
        string? wafVerboseOutputConfigValue;
        string? wafThrottleConfigValue;
        string? wafUseChainAnimationConfigValue;
        string? wafCreateContextSpecificReportConfigValue;
        string? wafCaptureRequestResponseConfigValue;
        string? wafReportGranularityConfigValue;
        string? wafDebugBreakOnRulesConfigValue;
        string? wafReportQueueMaxConfigValue;
        string? wafMaxReportCountConfigValue;
        string? wafMaxReportDurationConfigValue;
        string? wafCityProvinceFloorLimitsConfigValue;
        string? wafCityProvinceCeilingLimitsConfigValue;
        string? wafGeoIterationCycleMaxConfigValue;
        string dateTimeGranularity;
        var assemblyFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        this.next = next;
        this.logger = logger;
        this.maxMindProxy = maxMindProxy;
        this.cache = cache;
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.environment = environment;
        this.applicationLifetime = applicationLifetime;

        wafEnableConfigValue = configuration["WAFEnable"];
        wafThrottleConfigValue = configuration["WAFThrottle"];
        wafUseChainAnimationConfigValue = configuration["WAFUseChainAnimation"];
        wafCaptureRequestResponseConfigValue = configuration["WAFCaptureRequestResponse"];
#if DEBUG
        wafDebugBreakOnRulesConfigValue = configuration["WAFDebugBreakOnRules"];

        if (wafDebugBreakOnRulesConfigValue != null )
        {
            MicroRulesEngine.WAFDebugBreakOnRules = wafDebugBreakOnRulesConfigValue.Split(',').ToList();
        }
#endif
        if (wafThrottleConfigValue != null)
        {
            this.wafThrottle = TimeSpan.Parse(wafThrottleConfigValue);

            if (wafThrottle != TimeSpan.Zero)
            {
                using (ConsoleColorizer.UseColor(ConsoleColor.DarkRed))
                {
                    Console.WriteLine($"******************************** WARNING!!! WAFThrottle is set to: {this.wafThrottle} ********************************");
                }
            }
        }

        if (wafCaptureRequestResponseConfigValue != null)
        {
            this.captureRequestResponse = bool.Parse(wafCaptureRequestResponseConfigValue);
        }

        if (wafUseChainAnimationConfigValue != null)
        {
            this.useChainAnimation = bool.Parse(wafUseChainAnimationConfigValue);
        }

        if (useChainAnimation)
        {
            chainAnimationThread = new Thread(DoChainAnimation);
            lockObject = LockManager.CreateObject();

            chainAnimationThread.Priority = ThreadPriority.BelowNormal;
            chainAnimationThread.IsBackground = true;

            chainAnimationThread.Start();
        }

        if (wafEnableConfigValue != null)
        {
            this.enableWaf = bool.Parse(wafEnableConfigValue);
        }

        if (this.enableWaf)
        {
            wafVerboseOutputConfigValue = configuration["WAFVerboseOutput"];

            if (wafVerboseOutputConfigValue != null)
            {
                this.verboseOutput = bool.Parse(wafVerboseOutputConfigValue);
            }

            rulesConfig = configuration.GetSection("WAFConfiguration:Ruleset");
            this.rules = ruleset.Value.Rules;

            sessionInfoThreadLocal = new ThreadLocal<WAFSessionInfo>(() => new WAFSessionInfo(rules));

            BuildIndex();

            MicroRulesEngine.RuleExecutingEvent += MicroRulesEngine_RuleExecutingEvent;
            MicroRulesEngine.RuleExecutedEvent += MicroRulesEngine_RuleExecutedEvent;
            MicroRulesEngine.TypeMismatchEvent += MicroRulesEngine_TypeMismatchEvent;
            MicroRulesEngine.HandleMethodAmbiguityEvent += MicroRulesEngine_HandleMethodAmbiguityEvent;

            if (ruleset.Value.Rules.Count > 0)
            {
                compiledRule = new MicroRulesEngine(this.verboseOutput, this.rules.Count).CompileRule<WebContext>(ruleset.Value);
            }

            wafCreateContextSpecificReportConfigValue = configuration["WAFCreateContextSpecificReport"];
            wafReportQueueMaxConfigValue = configuration["WAFReportQueueMax"];
            wafReportGranularityConfigValue = configuration["WAFReportRolloverGranularity"];
            wafReportQueueMaxConfigValue = configuration["WAFReportQueueMax"];
            wafMaxReportCountConfigValue = configuration["WAFMaxReportCount"];
            wafMaxReportDurationConfigValue = configuration["WAFMaxReportDuration"];
            wafCityProvinceFloorLimitsConfigValue = configuration["WAFCityProvinceFloorLimits"];
            wafCityProvinceCeilingLimitsConfigValue = configuration["WAFCityProvinceCeilingLimits"];
            wafGeoIterationCycleMaxConfigValue = configuration["WAFGeoIterationCycleMax"];
            int[] wafCityLimits;

            if (wafGeoIterationCycleMaxConfigValue != null)
            {
                geoIterationCycleMax = int.Parse(wafGeoIterationCycleMaxConfigValue);
            }

            ipAddressBlockedFailCountQueue = new Queue<IPAddressBlockedFailSuccessCount>();

            if (wafCityProvinceFloorLimitsConfigValue != null)
            {
                wafCityLimits = wafCityProvinceFloorLimitsConfigValue.Split(",").Select(int.Parse).ToArray();

                cityFloorLimit = wafCityLimits[0];
                provinceFloorLimit = wafCityLimits[1];
            }

            if (wafCityProvinceCeilingLimitsConfigValue != null)
            {
                wafCityLimits = wafCityProvinceCeilingLimitsConfigValue.Split(",").Select(int.Parse).ToArray();

                cityCeilingLimit = wafCityLimits[0];
                provinceCeilingLimit = wafCityLimits[1];
            }

            if (wafReportQueueMaxConfigValue == null)
            {
                reportQueueMax = int.MaxValue;
            }
            else
            {
                reportQueueMax = int.Parse(wafReportQueueMaxConfigValue);
            }

            if (wafMaxReportCountConfigValue != null)
            {
                maxReportCount = int.Parse(wafMaxReportCountConfigValue);
            }

            if (wafMaxReportDurationConfigValue == null)
            {
                dateTimeGranularity = "00/00/0000 01:00:00"; // default to every hour
            }
            else
            {
                dateTimeGranularity = wafMaxReportDurationConfigValue;
            }

            maxReportDuration = DateTimeExtensions.ToDateTimeSpan(dateTimeGranularity);

            if (wafReportGranularityConfigValue == null)
            {
                dateTimeGranularity = "00/00/0000 01:00:00"; // default to every hour
            }
            else
            {
                dateTimeGranularity = wafReportGranularityConfigValue!; // default to every hour
            }

            reportRolloverGranularityTimeSpan = DateTimeExtensions.ToDateTimeSpan(dateTimeGranularity);
            reportsPath = configuration["WAFReportsPath"];

            if (!Path.IsPathRooted(reportsPath))
            {
                reportsPath = Path.GetFullPath(Path.Combine(assemblyFolder, reportsPath));
            }

            if (wafCreateContextSpecificReportConfigValue != null)
            {
                logger.LogInformation("Generating report headers");

                createContextSpecificReport = bool.Parse(wafCreateContextSpecificReportConfigValue);

                reportsPath = configuration["WAFReportsPath"];
            }

            if (CompareExtensions.AnyAreNotNull(wafCreateContextSpecificReportConfigValue, wafCaptureRequestResponseConfigValue))
            {
                actionQueueService = new ActionQueueService(reportQueueMax);
                managedMutexObject = LockManager.CreateMutex(REPORT_MUTEX_NAME);

                actionQueueService.OnIdle += (s, e) => CleanupReports();

                actionQueueService.Start();
            }
        }

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("Application shutting down");
            logger.LogInformation("Application shutting down");

            if (chainAnimationThread != null)
            {
                using (lockObject.Lock())
                {
                    exitChainAnimation = true;
                }
            }

            if (actionQueueService != null)
            {
                Console.WriteLine("Stopping action queue service");
                logger.LogInformation("Stopping action queue service");

                actionQueueService.Stop();
            }

            Console.WriteLine("Exiting app");
            logger.LogInformation("Exiting app");
        });
    }

    private void CleanupReports()
    {
        try
        {
            var rootDirectory = new DirectoryInfo(reportsPath);
            var now = DateTime.Now;

            Console.WriteLine("Cleaning up reports");

            foreach (var subDirectory in rootDirectory.GetDirectories())
            {
                var files = subDirectory.GetFiles("*.*", SearchOption.AllDirectories).OrderBy(f => f.Name).ToList();
                var count = files.Count;

                if (count > maxReportCount)
                {
                    var trimCount = count - maxReportCount;

                    files.Take(trimCount).ForEach(f =>
                    {
                        try
                        {
                            using (managedMutexObject.Lock())
                            {
                                f.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, ex.Message);
                        }
                    });
                }

                foreach (var file in files.Where(f => f.IsSortableDateTimeText()))
                {
                    var dateTime = file.GetSortableDateTime();

                    if (now - dateTime > maxReportDuration)
                    {
                        try
                        {
                            using (managedMutexObject.Lock())
                            {
                                file.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, ex.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            logger.LogCritical(ex, ex.Message);

            DebugUtils.BreakIfAttached();
        }
    }

    private void MicroRulesEngine_HandleMethodAmbiguityEvent(object sender, HandleMethodAmbiguityEventArgs e)
    {
        var methodName = e.MethodName;
        var args = e.InputArguments;

        switch (methodName)
        {
            case "ValidateByteRange":
                {
                    e.InputArguments = [args[1], args[0]];
                    e.RearrangedArgs = true;
                }
                break;
            default:
                DebugUtils.Break();
                break;
        }
    }

    private void MicroRulesEngine_TypeMismatchEvent(object sender, TypeMismatchEventArgs e)
    {
        var left = e.Left;
        var right = e.Right;
        var rule = e.Rule;
        var id = rule.Id;

        switch (id)
        {
            case "920520":
                e.LeftResolution = MismatchResolution.GetLenth;
                break;
            case "100026":
                e.LeftResolution = MismatchResolution.ParseToInt | MismatchResolution.CastToLong;
                break;
            default:
                DebugUtils.Break();
                break;
        }
    }

    private Stack<RuleChainSet> CurrentChainSetStack
    {
        get
        {
            var chainSetState = chainSetStateThreadLocal.Value;

            return chainSetState.CurrentChainSetStack;
        }
    }

    private RuleChainSet CurrentChainSet
    {
        get
        {
            var chainSetState = chainSetStateThreadLocal.Value;

            return chainSetState.CurrentChainSetStack.Peek();
        }
    }

    private RuleChainSet FullfilledChainSet
    {
        get
        {
            var chainSetState = chainSetStateThreadLocal.Value;

            return chainSetState.FullfilledChainSet;
        }

        set
        {
            var chainSetState = chainSetStateThreadLocal.Value;

            chainSetState.FullfilledChainSet = value;
        }
    }

    public Dictionary<string, RuleChainSet> RuleChains
    {
        get
        {
            return ruleChainsThreadLocal.Value!;
        }
    }

    private void BuildIndex()
    {
        IConfigurationSection rootSection;
        List<IConfigurationSection> rulesConfigChildren;

        logger.LogInformation("Building rules index");

        rootSection = rulesConfig.GetChildren().First(c => c.Key == "Rules");
        rulesConfigChildren = rootSection.GetChildren().ToList();

        rulesConfigIndex = new BTreeDictionary<string, IConfigurationSection>();

        foreach (var ruleConfig in rulesConfigChildren)
        {
            var id = ruleConfig["Id"];

            if (rulesConfigIndex.ContainsKey(id))
            {
                DebugUtils.Break();
            }

            rulesConfigIndex.Add(id, ruleConfig);
        }

        logger.LogInformation("Building rules index complete");
    }

    public async Task Invoke(HttpContext context)
    {
        if (enableWaf)
        {
            try
            {
                var threadCacheServiceProvider = new ThreadCacheServiceProvider(serviceProvider);
                var webContext = new WebContext(context, cache, maxMindProxy, threadCacheServiceProvider, rulesConfigIndex, rules);
                var executedRules = MicroRulesEngine.ExecutedRules;
                var groupCaptures = MicroRulesEngine.GroupCaptures;
                var stopwatch = new Stopwatch();
                var pass = false;
                string responseBody;
                WAFSessionInfo wafSessionInfo;
                Rule lastRule;

                stopwatch.Start();

                wafSessionInfo = sessionInfoThreadLocal.Value!;

                if (CompareExtensions.AnyAreNotNull(wafSessionInfo.ConnectionId, wafSessionInfo.FinishTime))
                {
                    using (ConsoleColorizer.UseColor(ConsoleColor.Red))
                    {
                        var builder = new StringBuilder();
                        string message;

                        builder.AppendLine("Unexpected reuse of connection info object");
                        builder.AppendLineFormat("Current: ConnectionId={0}", context.Connection.Id, wafSessionInfo.FinishTime);
                        builder.AppendLineFormat("Cached: ConnectionId={0}, FinishTime={1}", wafSessionInfo.ConnectionId, wafSessionInfo.FinishTime);

                        message = builder.ToString();

                        Console.WriteLine(message);
                        logger.LogCritical(message);
                    }

                    CleanupThreadLocals();

                    wafSessionInfo = sessionInfoThreadLocal.Value!;
                }

                wafSessionInfo.ConnectionId = context.Connection.Id;
                wafSessionInfo.StartTime = DateTime.UtcNow;

                if (createContextSpecificReport)
                {
                    var passFailedArray = new bool?[this.rules.Count];
                    var reportPassFailList = wafSessionInfo.ReportPassFailList;

                    reportPassFailList.Add(passFailedArray);
                }

                MicroRulesEngine.RulesExecuted = 0;

                executedRules.Clear();
                groupCaptures.Clear();

                try
                {
                    pass = compiledRule(webContext);
                }
                catch (Exception ex)
                {
                    var lastCalls = MicroRulesEngine.LastCalls;
                    var rule = executedRules.Peek();
                    var message = $"WAF error calling {lastCalls} for rule {rule.Id}, Exception: {ex.Message}";
                    ConsoleKeyInfo key;

                    using (ConsoleColorizer.UseColor(ConsoleColor.Red))
                    {
                        Console.WriteLine(message);
                        logger.LogCritical(ex, message);
                    }
#if DEBUG
                    Console.WriteLine($"Add rule {rule.Id} to breakpoints? Hit Y or any key to continue");
                    key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Y)
                    {
                        AddRuleAsBreakpoint(rule.Id);
                    }
#endif
                }

                responseBody = context.PrepareResponseBodyForCaptureAndCallNext(next);

                pass = compiledRule(webContext);

                if (!PostHandle(context, webContext, pass))
                {
                    pass = false;
                }
                else
                {
                    pass = true;
                }

                lastRule = executedRules.Peek();

                wafSessionInfo.FinishTime = DateTime.UtcNow;
                threadCacheServiceProvider.Dispose();

                CleanupThreadLocals();

                stopwatch.Stop();

                wafSessionInfo.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                wafSessionInfo.Blocked = !pass;

                if (verboseOutput)
                {
                    var message = string.Format("Request/response rule processing took {0} millisecond", stopwatch.ElapsedMilliseconds);

                    Console.WriteLine(message);
                    logger.LogDebug(message);
                }

                if (createContextSpecificReport)
                {
                    var reportPassFailList = wafSessionInfo.ReportPassFailList;
                    var reportPassFailedArrayHeaders = wafSessionInfo.ReportPassFailedArrayHeaders;
                    var passFailedArray = reportPassFailList.Last();
                    var connectionId = wafSessionInfo.ConnectionId;

                    actionQueueService.Run(() =>
                    {
                        try
                        {
                            UpdateReportFiles(context, reportPassFailedArrayHeaders, passFailedArray, reportsPath, wafSessionInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex}");
                            logger.LogCritical(ex, ex.Message);

                            DebugUtils.BreakIfAttached();
                        }
                    });
                }

                if (captureRequestResponse)
                {
                    var connectionId = wafSessionInfo.ConnectionId;

                    actionQueueService.Run(() =>
                    {
                        try
                        {
                            UpdateReportFiles(context, webContext.RequestLine, webContext.RequestBody, webContext.ResponseLine, responseBody, pass, lastRule, reportsPath, wafSessionInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex}");
                            logger.LogCritical(ex, ex.Message);

                            DebugUtils.BreakIfAttached();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                logger.LogCritical(ex, ex.Message);

                DebugUtils.BreakIfAttached();
            }
        }

        await Task.CompletedTask;
    }

    private void CleanupThreadLocals()
    {
        // done just in case a thread is reused

        try
        {
            WAFMiddleware.ruleChainsThreadLocal.Value.Clear();
            WAFMiddleware.chainSetStateThreadLocal.Value = new ChainSetState();
            WAFMiddleware.sessionInfoThreadLocal.Value = new WAFSessionInfo(this.rules);

            MicroRulesEngine.executedRulesThreadLocal.Value.Clear();
            MicroRulesEngine.lastCallsThreadLocal.Value = string.Empty;
            MicroRulesEngine.regexGroupCapturesThreadLocal.Value.Clear();
            MicroRulesEngine.ruleStatsThreadLocal.Value = new RuleStats();

            WebContextExtensions.contextMatchesThreadLocal.Value = new ContextMatches();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            logger.LogCritical(ex, ex.Message);

            DebugUtils.BreakIfAttached();
        }
    }

    private void AddRuleAsBreakpoint(string id)
    {
        var hydraSolutionPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%"));
        DirectoryInfo projectDirectory;
        List<FileInfo> appConfigFiles;
        JObject configObject;
        string? breakOn;

        projectDirectory = new DirectoryInfo(Path.Combine(hydraSolutionPath, @"Hydra\src\presentation\Hydra.WebApi"));
        appConfigFiles = projectDirectory.FindFiles("appsettings.Development.json", "appsettings.json").ToList();

        configObject = (JObject) JsonExtensions.LoadJson<object>(appConfigFiles.First().FullName);
        breakOn = configObject["WAFDebugBreakOnRule"].ToObject<string>();

        foreach (var appConfigFile in appConfigFiles)
        {
            if (breakOn != null && breakOn != id)
            {
                JsonExtensions.SaveJson(appConfigFile.FullName, configObject, Newtonsoft.Json.Formatting.Indented);
            }
        }
    }

    private void MicroRulesEngine_RuleExecutingEvent(object sender, RuleExecutingEventArgs e)
    {
        try
        {
            var rule = e.Rule;
            var id = rule.Id;

            if (rule.IsChainChild)
            {
                var ruleChains = this.RuleChains;
                var ruleChainPair = ruleChains.SingleOrDefault(c => c.Value.ChainChildIds.Contains(rule.Id));
                RuleChainSet ruleChainSet;
                Rule primaryRule;

                if (ruleChainPair.Key == null)
                {
                    DebugUtils.Break();
                }

                ruleChainSet = ruleChainPair.Value;
                primaryRule = ruleChainSet.PrimaryRule;

                if (!ruleChainSet.PrimaryPassed)
                {
                    e.SkipRule = true;
                }
            }

            if (this.verboseOutput)
            {
                Console.WriteLine("Executing rule: {0}, {1} of {2}", rule.Id, e.Index + 1, e.Count);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            logger.LogCritical(ex, ex.Message);

            DebugUtils.BreakIfAttached();
        }
    }

    private void MicroRulesEngine_RuleExecutedEvent(object sender, RuleExecutedEventArgs e)
    {
        try
        {
            var rule = e.Rule;
            var id = rule.Id;
            var webContext = e.WebContext;
            var httpContext = webContext.HttpContext;
            var response = httpContext.Response;
            var statusCode = (HttpStatusCode)response.StatusCode;
            var success = e.Success;
            var postSuccess = false;

            if (PostHandle(httpContext, webContext, success))
            {
                postSuccess = true;
            }

            if (createContextSpecificReport)
            {
                var wafSessionInfo = sessionInfoThreadLocal.Value!;
                var reportPassFailList = wafSessionInfo.ReportPassFailList;
                var reportPassFailedArrayHeaders = wafSessionInfo.ReportPassFailedArrayHeaders;
                var ruleIndex = reportPassFailedArrayHeaders.IndexOf(id);
                var passFailedArray = reportPassFailList.Last();
                var connectionId = wafSessionInfo.ConnectionId;

                passFailedArray[ruleIndex] = postSuccess;

                actionQueueService.Run(() =>
                {
                    try
                    {
                        UpdateReportFiles(httpContext, reportPassFailedArrayHeaders, passFailedArray, reportsPath, wafSessionInfo.DefaultClone());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");
                        logger.LogCritical(ex, ex.Message);

                        DebugUtils.BreakIfAttached();
                    }

                }, connectionId);
            }

            if (postSuccess)
            {
                if (!statusCode.IsSuccess())
                {
                    DebugUtils.Break();
                }

                if (this.verboseOutput)
                {
                    Console.WriteLine("Successfully executed rule: {0}, {1} of {2}, status code: {3}", rule.Id, e.Index + 1, e.Count, statusCode.ToString());
                }
            }
            else
            {
                if (!statusCode.IsSuccess())
                {
                    DebugUtils.Break();
                }

                if (this.verboseOutput)
                {
                    if (success)
                    {
                        Console.WriteLine("Failed rule: {0}, {1} of {2}, status code: {3}", rule.Id, e.Index + 1, e.Count, statusCode.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Failed rule in post handling: {0}, {1} of {2}, status code: {3}", rule.Id, e.Index + 1, e.Count, statusCode.ToString());
                    }
                }
            }

            if (wafThrottle != TimeSpan.Zero)
            {
                Thread.Sleep(wafThrottle);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            logger.LogCritical(ex, ex.Message);

            DebugUtils.BreakIfAttached();
        }
    }

    private void UpdateReportFiles(HttpContext httpContext, string requestLine, string requestBody, string responseLine, string responseBody, bool pass, Rule rule, string? reportsPath, WAFSessionInfo wafSessionInfo)
    {
        if (reportRolloverGranularityTimeSpan.Ticks > 0)
        {
            var connectionId = wafSessionInfo.ConnectionId;
            var startTime = wafSessionInfo.StartTime!.Value;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var reportPath = Path.Combine(reportsPath, "RequestResponse", startTime.ToSortableDateTimeText() + ".tsv");
            var connectionInfo = httpContext.GetConnectionInfo(maxMindProxy, environment);
            var connectionInfoJson = JsonExtensions.ToJsonText(connectionInfo);
            var reportFolder = Path.Combine(reportsPath, "RequestResponse");
            var directory = new DirectoryInfo(reportFolder);
            FileInfo? reportFile;

            if (!directory.Exists)
            {
                directory.Create();
            }

            reportFile = directory.GetFiles().Where(f => f.IsSortableDateTimeText(out DateTime dateTime)).OrderByDescending(f => f.Name).FirstOrDefault();

            if (reportFile != null && reportFile.IsSortableDateTimeText(out DateTime reportDateTime))
            {
                if (DateTime.Now - reportDateTime >= reportRolloverGranularityTimeSpan)
                {
                    reportFile = new FileInfo(Path.Combine(reportFolder, DateTime.Now.ToSortableDateTimeText() + ".tsv"));
                }
            }

            if (reportFile == null)
            {
                reportFile = new FileInfo(Path.Combine(reportFolder, DateTime.Now.ToSortableDateTimeText() + ".tsv"));
            }

            using (managedMutexObject.Lock())
            {
                using (var stream = reportFile.Open(FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                {
                    var requestText = requestLine + "\r\n" + request.Headers.Select(h => h.Key + ":" + h.Value).ToMultiLineList() + "\r\n\r\n" + requestBody;
                    var responseText = responseLine + "\r\n" + response.Headers.Select(h => h.Key + ":" + h.Value).ToMultiLineList() + "\r\n\r\n" + responseBody;
                    int lineCount;

                    requestText = requestText.ToReportDisplayText();
                    responseText = responseText.ToReportDisplayText();
                    connectionInfoJson = connectionInfoJson.ToReportDisplayText();
#if DEBUG
                    lineCount = requestText.GetLineCount();
                    Debug.Assert(lineCount <= 1);

                    lineCount = responseText.GetLineCount();
                    Debug.Assert(lineCount <= 1);

                    lineCount = connectionInfoJson.GetLineCount();
                    Debug.Assert(lineCount <= 1);
#endif
                    if (!reportFile.Exists || reportFile.Length == 0)
                    {
                        writer.WriteLine("Request/Response Report:");
                        writer.WriteLine(new List<string> { "ConnectionId", "StartTime", "FinishTime", "Request", "Response", "Failed", "LastRule", "ConnectionInfo" }.Select(s => s).ToDelimitedList("\t"));
                    }

                    writer.WriteLine(new List<string> { connectionId,
                    startTime.ToShortDateTimeString(),
                    DateTime.Now.ToShortDateString(),
                    requestText,
                    responseText,
                    pass ? "False" : "True",
                    rule.Id,
                    connectionInfoJson.ToSingleLine().RemoveText("\"").SurroundWithQuotes() }.Select(s => s).ToDelimitedList("\t"));

                    writer.Flush();
                }
            }
        }
    }

    private void UpdateReportFiles(HttpContext httpContext, string[] reportPassFailedArrayHeaders, bool?[] passFailedArray, string? reportsPath, WAFSessionInfo wafSessionInfo)
    {
        var connectionInfo = httpContext.GetConnectionInfo(maxMindProxy, environment);
        var connectionInfoJson = JsonExtensions.ToJsonText(connectionInfo);
        var connectionId = connectionInfo.ConnectionId;
        var ipAddress = connectionInfo.IpAddress;
        var startTime = wafSessionInfo.StartTime!.Value;
        var blocked = wafSessionInfo.Blocked;

        if (wafSessionInfo.FinishTime.HasValue)
        {
            var reportPath = Path.Combine(reportsPath, "ByIpAddress", startTime.ToSortableDateTimeText() + "_IpAddress-" + ipAddress + ".tsv");
            var file = new FileInfo(reportPath);
            var directory = file.Directory;

            if (!directory.Exists)
            {
                directory.Create();
            }

            using (managedMutexObject.Lock())
            {
                using (var stream = file.Open(FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                {
                    int lineCount;

                    connectionInfoJson = connectionInfoJson.ToReportDisplayText();
#if DEBUG
                    lineCount = connectionInfoJson.GetLineCount();
                    Debug.Assert(lineCount <= 1);
#endif
                    if (!file.Exists || file.Length == 0)
                    {
                        writer.WriteLine("Pass/Fail Report: (pass = true, fail = false; Note: Failures are not always disruptive, punitive, and/or unfavorable)");
                        writer.WriteLine(reportPassFailedArrayHeaders.Concat(["Blocked", "ConnectionInfo"]).Select(s => s).ToDelimitedList("\t"));
                    }

                    writer.WriteLine(passFailedArray.Select(p => p.AsDisplayText()).Concat([blocked ? "True" : "False", connectionInfoJson]).ToDelimitedList("\t"));
                    writer.Flush();
                }
            }

            if (reportRolloverGranularityTimeSpan.Ticks > 0)
            {
                var reportFolder = Path.Combine(reportsPath, "RuleStatistics");
                FileInfo? reportFile;
                int[]? failCountArray = null;
                
                directory = new DirectoryInfo(reportFolder);

                if (!directory.Exists)
                {
                    directory.Create();
                }

                reportFile = directory.GetFiles().Where(f => f.IsSortableDateTimeText(out DateTime dateTime)).OrderByDescending(f => f.Name).FirstOrDefault();

                if (reportFile != null && reportFile.IsSortableDateTimeText(out DateTime reportDateTime))
                {
                    if (DateTime.Now - reportDateTime <= reportRolloverGranularityTimeSpan)
                    {
                        using (managedMutexObject.Lock())
                        {
                            failCountArray = File.ReadAllText(reportFile.FullName).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Last().Split("\t").Select(int.Parse).ToArray();
                        }

                        failCountArray = failCountArray.Zip(passFailedArray, (x, y) => x + (y.IsValueTrue() ? 1 : 0)).ToArray();

                        reportFile.Delete();
                    }
                    else
                    {
                        reportFile = null;
                    }
                }

                if (reportFile == null)
                {
                    reportFile = new FileInfo(Path.Combine(reportFolder, DateTime.Now.ToSortableDateTimeText() + ".tsv"));
                }
                
                if (failCountArray == null)
                {
                    failCountArray = passFailedArray.Select(v => v.IsValueTrue() ? 1 : 0).ToArray();
                }

                using (managedMutexObject.Lock())
                {
                    using (var stream = reportFile.Open(FileMode.Append, FileAccess.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        if (!reportFile.Exists || reportFile.Length == 0)
                        {
                            writer.WriteLine("Fail Count Report, Note: Failures are not always disruptive, punitive, and/or unfavorable)");
                            writer.WriteLine(reportPassFailedArrayHeaders.Concat(["Blocked", "ConnectionInfo"]).Select(s => s).ToDelimitedList("\t"));
                        }

                        writer.WriteLine(failCountArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                        writer.Flush();
                    }
                }

                UpdateGeoReport(reportsPath, maxMindProxy, connectionInfo.IpAddress, blocked, failCountArray.Sum());
             }
        }
        else
        {
            var reportPath = Path.Combine(reportsPath, "ByConnectionId", startTime.ToSortableDateTimeText() + "_ConnectionId-" + connectionId + ".tsv");
            var file = new FileInfo(reportPath);
            var directory = file.Directory;

            if (!directory.Exists)
            {
                directory.Create();

                using (managedMutexObject.Lock())
                {
                    using (var stream = file.Open(FileMode.Append, FileAccess.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        if (!file.Exists || file.Length == 0)
                        {
                            writer.WriteLine("Pass/Fail Report: (pass = true, fail = false; Note: Failures are not always disruptive, punitive, and/or unfavorable)");
                            writer.WriteLine(reportPassFailedArrayHeaders.Select(s => s).ToDelimitedList("\t"));
                        }

                        writer.WriteLine(passFailedArray.Select(p => p.AsDisplayText()).Concat([blocked ? "True" : "False", connectionInfoJson]).ToDelimitedList("\t"));
                        writer.Flush();
                    }
                }
            }

            if (file.Exists && file.Length > 0)
            {
                using (managedMutexObject.Lock())
                {
                    using (var stream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var reader = new BinaryReader(stream))
                    {
                        // read backwards until you find the beginning of the last record

                        var x = file.Length - 3;
                        char ch = '\0';

                        reader.Seek(x, SeekOrigin.Begin);

                        while (x > 1 && ch != '\r')
                        {
                            ch = (char)reader.ReadByte();

                            reader.Seek(x - 1, SeekOrigin.Begin);
                            x--;
                        }

                        stream.SetLength(x + 3);
                    }

                    using (var stream = file.Open(FileMode.Append, FileAccess.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        if (!file.Exists || file.Length == 0)
                        {
                            writer.WriteLine("Pass/Fail Report: (pass = true, fail = false; Note: Failures are not always disruptive, punitive, and/or unfavorable)");
                            writer.WriteLine(reportPassFailedArrayHeaders.Select(s => s).ToDelimitedList("\t"));
                        }

                        writer.WriteLine(passFailedArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                        writer.Flush();
                    }
                }
            }
        }
    }

    public void UpdateGeoReport(string reportsPath, IMaxMindProxy maxMindProxy, string ipAddress, bool blocked, int failCount)
    {
        geoIterationCycle = (++geoIterationCycle).ScopeRange(1, geoIterationCycleMax);

        if (geoIterationCycle == geoIterationCycleMax)
        {
            var reportFolder = Path.Combine(reportsPath, "GeoReport");
            var directory = new DirectoryInfo(reportFolder);
            FileInfo? deleteFile = null;
            FileInfo? reportFile;
            string[]? cityProvinceOrCountryArray = null;
            string[]? typeArray = null;
            string[]? coordinatesArray = null;
            int[]? blockedCountArray = null;
            int[]? failedCountArray = null;
            int[]? successCountArray = null;
            string?[]? records = null;
            string type;
            List<LocationStats> locationStatsList;
            IEnumerable<IGrouping<string, LocationStats>> locationStatsGrouped;

            Console.WriteLine("Updating GEO report");

            if (!directory.Exists)
            {
                directory.Create();
            }

            reportFile = directory.GetFiles().Where(f => f.IsSortableDateTimeText(out DateTime dateTime)).OrderByDescending(f => f.Name).FirstOrDefault();

            if (reportFile != null && reportFile.IsSortableDateTimeText(out DateTime reportDateTime))
            {
                if (DateTime.Now - reportDateTime <= reportRolloverGranularityTimeSpan)
                {
                    using (managedMutexObject.Lock())
                    {
                        records = File.ReadAllText(reportFile.FullName).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                        cityProvinceOrCountryArray = records[0].Split("\t").ToArray();
                        typeArray = records[1].Split("\t").ToArray();
                        coordinatesArray = records[2].Split("\t").ToArray();
                        blockedCountArray = records[3].Split("\t").Select(int.Parse).ToArray();
                        failedCountArray = records[4].Split("\t").Select(int.Parse).ToArray();
                        successCountArray = records[5].Split("\t").Select(int.Parse).ToArray();

                        deleteFile = reportFile;
                    }
                }
                else
                {
                    reportFile = null;
                }
            }

            ipAddressBlockedFailCountQueue.Enqueue(new IPAddressBlockedFailSuccessCount(ipAddress, blocked, failCount));

            while (ipAddressBlockedFailCountQueue.Count > 0)
            {
                var ipAddressBlockedFailCount = ipAddressBlockedFailCountQueue.Dequeue();
                var cityResponse = maxMindProxy.GetCity(IPAddress.Parse(ipAddressBlockedFailCount.IpAddress));
                string? city = null;
                string? country = null;
                string? province = null;
                GeoCoordinate? geoCoordinates = null;

                blocked = ipAddressBlockedFailCount.Blocked;
                failCount = ipAddressBlockedFailCount.FailCount;

                if (cityResponse != null)
                {
                    city = cityResponse.City.Name;
                    country = cityResponse.Country.Name;
                    province = cityResponse.MostSpecificSubdivision.Name;
                }
                else
                {
                    country = "{Unknown}";
                }

                if (cityResponse != null && cityResponse.Location.HasCoordinates)
                {
                    geoCoordinates = new GeoCoordinate(cityResponse.Location.Longitude!.Value, cityResponse.Location.Latitude!.Value);
                }
                else
                {
                    geoCoordinates = new GeoCoordinate(48, 123);  // pole of inaccessibility
                }

                if (records == null)
                {
                    int partCount;
                    string location;

                    records = new string[5];

                    cityProvinceOrCountryArray = new string[1];

                    if (city != null)
                    {
                        var cityFull = new string[] { city.AsDisplayText(), province.AsDisplayText(), country.AsDisplayText() }.ToDelimitedList(" | ");

                        cityProvinceOrCountryArray[0] = cityFull;

                        location = cityFull;
                        type = "City";
                    }
                    else if (province != null)
                    {
                        var provinceFull = new string[] { province.AsDisplayText(), country.AsDisplayText() }.ToDelimitedList(" | ");

                        cityProvinceOrCountryArray[0] = provinceFull;

                        location = provinceFull;
                        type = "Province";
                    }
                    else
                    {
                        var countryFull = new string[] { country.AsDisplayText() }.ToDelimitedList(" | ");

                        cityProvinceOrCountryArray[0] = countryFull;

                        location = countryFull;
                        type = "Country";
                    }

                    partCount = location.Count(c => c == '|') + 1;

                    switch (type)
                    {
                        case "City":
                            Debug.Assert(partCount == 3);
                            break;
                        case "Province":
                            Debug.Assert(partCount == 2);
                            break;
                        case "Country":
                            Debug.Assert(partCount == 1);
                            break;
                    }

                    typeArray = [type];
                    coordinatesArray = [geoCoordinates.ToString()];
                    blockedCountArray = [blocked ? 1 : 0];
                    failedCountArray = [failCount];
                    successCountArray = [blocked ? 0 : 1];
                }
                else
                {
                    int columnIndex;
                    int partCount;
                    var expandRight = false;
                    string location;

                    blocked = ipAddressBlockedFailCount.Blocked;
                    failCount = ipAddressBlockedFailCount.FailCount;

                    if (city != null)
                    {
                        var cityFull = new string[] { city.AsDisplayText(), province.AsDisplayText(), country.AsDisplayText() }.ToDelimitedList(" | ");

                        if (cityProvinceOrCountryArray.Contains(cityFull))
                        {
                            columnIndex = cityProvinceOrCountryArray.IndexOf(cityFull);
                        }
                        else
                        {
                            columnIndex = cityProvinceOrCountryArray.Length;
                            expandRight = true;
                        }

                        location = cityFull;
                        type = "City";
                    }
                    else if (province != null)
                    {
                        var provinceFull = new string[] { province.AsDisplayText(), country.AsDisplayText() }.ToDelimitedList(" | ");

                        if (cityProvinceOrCountryArray.Contains(provinceFull))
                        {
                            columnIndex = cityProvinceOrCountryArray.IndexOf(provinceFull);
                        }
                        else
                        {
                            columnIndex = cityProvinceOrCountryArray.Length;
                            expandRight = true;
                        }

                        location = provinceFull;
                        type = "Province";
                    }
                    else
                    {
                        var countryFull = new string[] { country.AsDisplayText() }.ToDelimitedList(" | ");

                        if (cityProvinceOrCountryArray.Contains(countryFull))
                        {
                            columnIndex = cityProvinceOrCountryArray.IndexOf(countryFull);
                        }
                        else
                        {
                            columnIndex = cityProvinceOrCountryArray.Length;
                            expandRight = true;
                        }

                        location = countryFull;
                        type = "Country";
                    }

                    if (expandRight)
                    {
                        cityProvinceOrCountryArray = cityProvinceOrCountryArray.ExpandRight(1);
                        typeArray = typeArray.ExpandRight(1);
                        coordinatesArray = coordinatesArray.ExpandRight(1);
                        blockedCountArray = blockedCountArray.ExpandRight(1);
                        failedCountArray = failedCountArray.ExpandRight(1);
                        successCountArray = successCountArray.ExpandRight(1);
                    }

                    partCount = location.Count(c => c == '|') + 1;

                    switch (type)
                    {
                        case "City":
                            Debug.Assert(partCount == 3);
                            break;
                        case "Province":
                            Debug.Assert(partCount == 2);
                            break;
                        case "Country":
                            Debug.Assert(partCount == 1);
                            break;
                    }

                    cityProvinceOrCountryArray[columnIndex] = location;
                    typeArray[columnIndex] = type;
                    coordinatesArray[columnIndex] = geoCoordinates.ToString();
                    blockedCountArray[columnIndex] += blocked ? 1 : 0;
                    failedCountArray[columnIndex] += failCount;
                    successCountArray[columnIndex] += blocked ? 0 : 1;
                }
            }

            locationStatsList = cityProvinceOrCountryArray.Zip(typeArray, (l, t) => new LocationStats(l, t)).ToList();

            locationStatsList = locationStatsList.Zip(coordinatesArray, (l, c) => l.AddCoordinates(c)).ToList();
            locationStatsList = locationStatsList.Zip(blockedCountArray, (l, c) => l.AddBlockCount(c)).ToList();
            locationStatsList = locationStatsList.Zip(failedCountArray, (l, c) => l.AddFailCount(c)).ToList();
            locationStatsList = locationStatsList.Zip(successCountArray, (l, c) => l.AddSuccesCount(c)).ToList();

            locationStatsList.ForEach(s =>
            {
                if (!s.Validate(out string? error))
                {
                    Console.WriteLine($"Error: {error}");
                    logger.LogCritical(error);

                    DebugUtils.BreakIfAttached();
                }
            });

            locationStatsGrouped = locationStatsList.GroupBy(s => s.Type);

            foreach (var grouping in locationStatsGrouped.Where(g => g.Key != "Country"))
            {
                var groupType = grouping.Key;
                var list = grouping.ToList();

                switch (groupType)
                {
                    case "City":
                        {
                            type = "City";

                            if (list.Count > cityCeilingLimit)
                            {
                                var citieStatsBelowFloor = list.Where(c => c.BlockCount < cityFloorLimit).TakeLast(list.Count - cityCeilingLimit);

                                foreach (var cityStats in citieStatsBelowFloor.ToList())
                                {
                                    var cityPart = cityStats.City;
                                    var provincePart = cityStats.Province;
                                    var count = locationStatsList.Count(s => s.Type == "Province" && s.Province == provincePart);

                                    if (count <= 1)
                                    {
                                        var provinceLocation = locationStatsList.SingleOrDefault(s => s.Type == "Province" && s.Province == provincePart);
                                        var citiesInProvince = locationStatsList.Where(s => s.Type == "City" && s.Province == provincePart);
                                        var averageCoordinates = citiesInProvince.Select(c => GeoLocationExtensions.ToGeoCoordinate(c.Coordinates)).Aggregate((a, b) => new GeoCoordinate(Math_.Average(a.Latitude, b.Latitude), Math_.Average(a.Longitude, b.Longitude)))!;

                                        if (provinceLocation == null)
                                        {
                                            var country = cityStats.Country;
                                            var province = cityStats.Province;
                                            var provinceFull = new string[] { province.AsDisplayText(), country.AsDisplayText() }.ToDelimitedList(" | ");

                                            provinceLocation = new LocationStats(provinceFull, "Province");

                                            locationStatsList.Add(provinceLocation);
                                        }

                                        provinceLocation.SuccessCount += cityStats.SuccessCount;
                                        provinceLocation.FailCount += cityStats.FailCount;
                                        provinceLocation.BlockCount += cityStats.BlockCount;
                                        provinceLocation.Coordinates = averageCoordinates.ToString();

                                        locationStatsList.Remove(cityStats);
                                    }
                                    else
                                    {
                                        DebugUtils.Break();
                                    }
                                }
                            }
                        }
                        break;
                    case "Province":
                        {
                            type = "Province";

                            if (list.Count > provinceCeilingLimit)
                            {
                                var provinceStatsBelowFloor = list.Where(c => c.BlockCount < provinceFloorLimit).TakeLast(list.Count - provinceCeilingLimit);

                                foreach (var provinceStats in provinceStatsBelowFloor.ToList())
                                {
                                    var provincePart = provinceStats.Province;
                                    var countryPart = provinceStats.Country;
                                    var count = locationStatsList.Count(s => s.Type == "Country" && s.Country == countryPart);

                                    if (count <= 1)
                                    {
                                        var countryLocation = locationStatsList.SingleOrDefault(s => s.Type == "Country" && s.Country == countryPart);
                                        var provincesInCountry = locationStatsList.Where(s => s.Type == "Province" && s.Country == countryPart);
                                        var averageCoordinates = provincesInCountry.Select(c => GeoLocationExtensions.ToGeoCoordinate(c.Coordinates)).Aggregate((a, b) => new GeoCoordinate(Math_.Average(a.Latitude, b.Latitude), Math_.Average(a.Longitude, b.Longitude)))!;

                                        if (countryLocation == null)
                                        {
                                            var country = provinceStats.Country;
                                            var countryFull = new string[] { country.AsDisplayText() }.ToDelimitedList(" | ");
                                            countryLocation = new LocationStats(countryFull, "Country");

                                            locationStatsList.Add(countryLocation);
                                        }

                                        countryLocation.SuccessCount += provinceStats.SuccessCount;
                                        countryLocation.FailCount += provinceStats.FailCount;
                                        countryLocation.BlockCount += provinceStats.BlockCount;
                                        countryLocation.Coordinates = averageCoordinates.ToString();

                                        locationStatsList.Remove(provinceStats);
                                    }
                                    else
                                    {
                                        DebugUtils.Break();
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        DebugUtils.Break();
                        break;
                }
            }

            if (reportFile == null)
            {
                reportFile = new FileInfo(Path.Combine(reportFolder, DateTime.Now.ToSortableDateTimeText() + ".tsv"));
            }

            locationStatsList = locationStatsList.OrderBy(s => s.TypeIndex).ThenBy(s => s.LocationFull).ToList();

            cityProvinceOrCountryArray = locationStatsList.Select(s => s.LocationFull).ToArray();
            typeArray = locationStatsList.Select(s => s.Type).ToArray();
            coordinatesArray = locationStatsList.Select(s => s.Coordinates).ToArray();
            blockedCountArray = locationStatsList.Select(s => s.BlockCount).ToArray();
            failedCountArray = locationStatsList.Select(s => s.FailCount).ToArray();
            successCountArray = locationStatsList.Select(s => s.SuccessCount).ToArray();

            using (managedMutexObject.Lock())
            {
                if (deleteFile != null)
                {
                    deleteFile.Delete();
                }

                using (var stream = reportFile.Open(FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                {
                    if (!reportFile.Exists || reportFile.Length == 0)
                    {
                        writer.WriteLine("GEO Report, Note: Failures are not always disruptive, punitive, and/or unfavorable), Rows: 2. City, Province, or Country, 3. Type, 4. Coordinates, 5. Blocked, 7. SuccessCount");
                    }

                    writer.WriteLine(cityProvinceOrCountryArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                    writer.WriteLine(typeArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                    writer.WriteLine(coordinatesArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                    writer.WriteLine(blockedCountArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                    writer.WriteLine(failedCountArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));
                    writer.WriteLine(successCountArray.Select(p => p.AsDisplayText()).ToDelimitedList("\t"));

                    writer.Flush();
                }
            }
        }
        else
        {
            ipAddressBlockedFailCountQueue.Enqueue(new IPAddressBlockedFailSuccessCount(ipAddress, blocked, failCount));
        }
    }

    private void DoChainAnimation()
    {
        var messageLength = 0;
        var fullfilledMessageLength = 0;
        var exit = false;

        while (!exit)
        {
            RuleChainSet? ruleChainSet = null;
            RuleChainSet? fullfilledChainSet = null;
            var right = Console.BufferWidth;

            using (lockObject.Lock())
            {
                if (this.CurrentChainSetStack.Count > 0)
                {
                    ruleChainSet = this.CurrentChainSetStack.Peek();
                    fullfilledChainSet = this.FullfilledChainSet;
                }

                exit = exitChainAnimation;
            }

            if (ruleChainSet != null)
            {
                var top = 1;
                var primaryRule = ruleChainSet.PrimaryRule;
                var message = string.Format("Primary rule {0} waiting for chain child rules ({1}) to complete", primaryRule.Id, ruleChainSet.ChainChildRules.Count);

                if (fullfilledChainSet != null)
                {
                    var fullfilledPrimaryRule = fullfilledChainSet.PrimaryRule;
                    var fullfilledMessage = string.Format("Rule {0} fullfilled at {1}", fullfilledPrimaryRule.Id, fullfilledChainSet.FullfilledAt.ToShortDateTimeString());

                    fullfilledMessageLength = fullfilledMessage.Length;

                    using (ConsolePositioner.Set(right - messageLength, top))
                    using (ConsoleColorizer.UseColor(ConsoleColor.Green))
                    {
                        Console.Write(fullfilledMessage);
                    }

                    top++;
                }

                using (ConsolePositioner.Set(right - messageLength, top))
                using (ConsoleColorizer.UseColor(ConsoleColor.DarkYellow))
                {
                    Console.Write(message);
                }
            }
            else if (messageLength > 0 || fullfilledChainSet != null)
            {
                using (ConsolePositioner.Set(right - messageLength, 1))
                {
                    var now = DateTime.Now;

                    if (fullfilledChainSet != null && ((now - fullfilledChainSet.FullfilledAt) < TimeSpan.FromSeconds(5)))
                    {
                        var fullfilledPrimaryRule= fullfilledChainSet.PrimaryRule;
                        var fullfilledMessage = string.Format("Rule {0} fullfilled at {1}", fullfilledPrimaryRule.Id, fullfilledChainSet.FullfilledAt.ToShortDateTimeString());
                    }
                    else
                    {
                        Console.Write(" ".Repeat(messageLength));
                    }
                }

                messageLength = 0;
            }
            else if (fullfilledMessageLength > 0)
            {
                Console.Write(" ".Repeat(fullfilledMessageLength));
            }

            Thread.Sleep(100);
        }
    }

    private bool PostHandle(HttpContext context, WebContext webRequest, bool passed)
    {
        var executedRules = MicroRulesEngine.ExecutedRules;
        var rule = executedRules.Peek();
        var id = rule.Id;
        var callbackMethod = rule.CallbackMethod;
        var rethrowNaked = true;

        if (!rule.Enabled)
        {
            if (callbackMethod != null)
            {
                var section = rulesConfigIndex[rule.MemberName];
                var transformationsActionsSection = section.GetSection("TransformationsActions");
                var json = transformationsActionsSection.ToJson();

                try
                {
                    var callbackResult = (CallbackResult) webRequest.CallMethod(callbackMethod, context, serviceProvider, !passed, true, json);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, ex.Message);

                    if (rethrowNaked)
                    {
                        throw;
                    }
                }
            }

            return false;
        }

        if (rule.ChainChildren != null)
        {
            var ruleChains = this.RuleChains;
            RuleChainSet ruleChainSet;

            if (ruleChains.ContainsKey(rule.Id))
            {
                ruleChainSet = ruleChains[rule.Id];
            }
            else
            {
                ruleChainSet = new RuleChainSet(rule, passed);
                ruleChains.Add(rule.Id, ruleChainSet);
            }

            using (lockObject.Lock())
            {
                var currentChainSet = ruleChainSet.DefaultClone();

                this.CurrentChainSetStack.Push(currentChainSet);
            }
        }
        
        if (rule.IsChainChild)
        {
            var ruleChains = this.RuleChains;
            var ruleChainPair = ruleChains.SingleOrDefault(c => c.Value.ChainChildIds.Contains(rule.Id));
            RuleChainSet ruleChainSet;
            Rule primaryRule;

            if (ruleChainPair.Key == null)
            {
                DebugUtils.Break();
            }

            ruleChainSet = ruleChainPair.Value;
            primaryRule = ruleChainSet.PrimaryRule;

            ruleChainSet.ChainChildRules.Add(new RuleAndResult(rule, passed));

            if (ruleChainSet.IsFullfilled && ruleChainSet.PrimaryPassed)
            {
                var childFailed = false;

                foreach (var childRuleResult in ruleChainSet.ChainChildRules)
                {
                    var childRule = childRuleResult.Rule;
                    var childPassed = childRuleResult.Passed;
                    var childCallbackMethod = childRule.CallbackMethod;

                    if (!HandleCallbacks(childRule, context, webRequest, childPassed, childCallbackMethod, rethrowNaked))
                    {
                        childFailed = true;
                    }
                }

                if (!childFailed)
                {
                    return HandleCallbacks(rule, context, webRequest, passed, callbackMethod, rethrowNaked);
                }

                using (lockObject.Lock())
                {
                    var fullfilledAt = DateTime.Now;

                    this.FullfilledChainSet = CurrentChainSetStack.Pop();
                    this.FullfilledChainSet.FullfilledAt = fullfilledAt;

                    ruleChainSet.FullfilledAt = fullfilledAt;
                }
            }
            else
            {
                using (lockObject.Lock())
                {
                    this.CurrentChainSet.ChainChildRules.Add(new RuleAndResult(rule, passed));
                }
            }
        }
        else if (callbackMethod != null)
        {
            return HandleCallbacks(rule, context, webRequest, passed, callbackMethod, rethrowNaked);
        }

        return passed;
    }

    private bool HandleCallbacks(Rule rule, HttpContext context, WebContext webRequest, bool passed, string callbackMethod, bool rethrowNaked)
    {
        if (callbackMethod != null)
        {
            var ruleConfig = rulesConfigIndex[rule.Id];
            var transformationsActionsSection = ruleConfig.GetSection("TransformationsActions");
            var json = transformationsActionsSection.ToJson();

            try
            {
                var callbackResult = (CallbackResult)webRequest.CallMethod(callbackMethod, context, serviceProvider, !passed, false, json);

                return !callbackResult.PeformBlock;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);

                if (rethrowNaked)
                {
                    throw;
                }

                return false;
            }
        }
        else
        {
            logger.LogWarning($"Forbidden request from {webRequest.RemoteIp}");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            return false;
        }
    }
}
