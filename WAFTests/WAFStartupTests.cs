using HtmlAgilityPack;
using WebSecurity.StartupTests.Extensions;
using WebSecurity.StartupTests.Helpers;
using WebSecurity.StartupTests.Models;
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OWASPCoreRulesetParser;
using SeleniumExtras.WaitHelpers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Expressions;
using Serilog.Parsing;
using Simple.Service.Monitoring.Extensions;
using Simple.Service.Monitoring.Library.Monitoring.Abstractions;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Utils;
using Utils.FileTypeMatcher;
using Utils.HttpServer;
using Utils.IntermediateLanguage;
using Utils.MemoryMappedFiles;
using WAFWebSample.Data;
using WAFWebSample.WebApi.Providers;
using WAFWebSample.WebApi.Providers.WAF;
using WebSecurity.Interfaces;
using WebSecurity.KestrelWAF;
using WebSecurity.KestrelWAF.RulesEngine;
using WebSecurity.Models;
using ConnectionInfo = WebSecurity.KestrelWAF.ConnectionInfo;
using LoggerExtensions = Utils.LoggerExtensions;

namespace WebSecurity.StartupTests;

[TestKind(TestKind.StartupTests)]
public class WAFStartupTests : TestsBase, IDisposable
{
    private const int timeoutMinutes = 15;
    private ServiceProvider serviceProvider;
    private ILoggerFactory? loggerFactory;
    private Microsoft.Extensions.Logging.ILogger logger;
    private ActionQueueService actionQueueService;
    private IConfigurationRoot configuration;
    private CsrDetails csrDetails;
    private Assembly webApiAssembly;
    private string token;
    private int userUniqueId;
    private string authorization;
    private Logger serilogLogger;
    private ServiceCollection services;
    private bool noHeadlessBrowser;
    private string? ipAddress;
    private CityResponse? cityResponse;
    private string webApiAssemblyFolder;
    private string rulesetFilePath;
    private string contentRootPath;
    private string testProjectFolder;
    private string? testsAssemblyFolder;
    private IStackMonitoring monitoring;
    private IHostedService hostedService;

    public WAFStartupTests()
    {
        var solutionPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%"));
        var webApiAssemblyFolder = Path.Combine(solutionPath, @"WAFWebSample\bin\Debug\net8.0-windows");
        var webApiAssemblyFile = Path.Combine(webApiAssemblyFolder, @"WAFWebSample.dll");
        var webApiRootFolder = Path.Combine(solutionPath, @"WAFWebSample");
        var builder = new Mock<ILoggingBuilder>();
        var assembly = Assembly.GetEntryAssembly();
        var name = assembly.GetName();
        var location = Path.GetDirectoryName(assembly.Location);
        var logPath = Path.GetFullPath(Path.Combine(location, @"..\..\..\Logs"));
        var serilogFile = Path.Combine(logPath, $"{assembly.GetNameParts().AssemblyName}.log");
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Helpers.Mocks mocks;

        this.webApiAssemblyFolder = webApiAssemblyFolder;
        rulesetFilePath = Path.Combine(webApiAssemblyFolder, "wafruleset.json");
        contentRootPath = webApiRootFolder;
        testProjectFolder = Path.Combine(solutionPath, @"WAFTests");
        testsAssemblyFolder = Path.GetDirectoryName(assembly.Location);

        ControlExtensions.ShowConsoleInSecondaryMonitor(System.Windows.Forms.FormWindowState.Maximized);

        if (AppDomain.CurrentDomain.GetData(Switches.NO_HEADLESS_BROWSER) is bool result)
        {
            noHeadlessBrowser = result;
        }

        if (environment == null)
        {
            environment = "Development";
        }

        Assert.True(Directory.Exists(webApiAssemblyFolder));
        Assert.True(File.Exists(webApiAssemblyFile));
        Assert.True(File.Exists(rulesetFilePath));
        Assert.True(Directory.Exists(logPath));

        configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .SetBasePath(webApiRootFolder)
            .AddJsonFile($"appsettings.{environment}.json", true, true)
            .AddJsonFile($"appsettings.json", true, true)
            .AddJsonFile(rulesetFilePath, false, true)
            .AddJsonFile("appsettings.shared.json", true, true)
            .Build();

        csrDetails = configuration.Get<CsrDetails>("CsrDetails");

        webApiAssembly = Assembly.LoadFrom(webApiAssemblyFile);

        serilogLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithClientIp()
            .Enrich.WithCorrelationId(addValueIfHeaderAbsence: true)
            .Enrich.WithRequestHeader("Authorize")
            .Enrich.WithProperty("Assembly", $"{name.Name}")
            .Enrich.WithProperty("Assembly", $"{name.Version}")
            .ReadFrom.Configuration(configuration)
            .WriteTo.File(serilogFile, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: NumberExtensions.MB, rollOnFileSizeLimit: true, outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE)
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(Path.Combine(logPath, "Info", "{Date}.log"), outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(Path.Combine(logPath, "Debug", "{Date}.log"), outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(Path.Combine(logPath, "Warning", "{Date}.log"), outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(Path.Combine(logPath, "Error", "{Date}.log"), outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(Path.Combine(logPath, "Fatal", "{Date}.log"), outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE))
            .WriteTo.ExpressionLogsFromConfig(configuration, logPath)
            .WriteTo.Console(outputTemplate: LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE).CreateLogger();

        services = new ServiceCollection();
        mocks = TestHelpers.SetupMocks(services, MockTypes.HydraDBContext);

        services.AddLogging(c => c.AddSerilog(serilogLogger, true));
        services.AddSingleton<LoggerAlertingService>();
        services.AddSingleton<ActionQueueService>();
        services.AddScoped<ThreadCacheServiceProvider>();
        services.AddHealthChecks();

        monitoring = services.UseServiceMonitoring(configuration).UseSettings().Build();

        builder.SetupGet(m => m.Services).Returns(services);

        serviceProvider = services.BuildServiceProvider();

        hostedService = serviceProvider.GetService<IHostedService>()!;

        _= Task.Run(async () =>
        {
            await hostedService.StartAsync(CancellationToken.None);
        });

        loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        logger = loggerFactory.CreateLogger(nameof(WAFStartupTests));
        actionQueueService = serviceProvider.GetService<ActionQueueService>()!;

        logger.LogInformation($"{ nameof(WAFStartupTests)}:Setup");
    }

    [Fact]
    public void TestTextFileTypeMatcher()
    {
        RangeFileTypeMatcher rangeFileTypeMatcher;
        Stream stream;

        rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactTextFileTypeMatcher("{", true), 1, 1024);
        stream = "{".ToStream();

        Assert.True(rangeFileTypeMatcher.Matches(stream));

        rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("{", "}", true), 1, 1024);
        stream = "{".ToStream();

        Assert.False(rangeFileTypeMatcher.Matches(stream));

        rangeFileTypeMatcher = new RangeFileTypeMatcher(new ExactMultiTextFileTypeMatcher("{", "}", true), 1, 1024);
        stream = "{}".ToStream();

        Assert.True(rangeFileTypeMatcher.Matches(stream));
    }

    [Fact(Skip = "Temporary")]
    public void TestConsoleAnimations()
    {
        var right = Console.BufferWidth;

        _ = Task.Run(() =>
        {
            for (var x = 0; x < 1000; x++)
            {
                if (NumberExtensions.ScopeRange(x, 1, 10) > 5)
                {
                    using (ConsolePositioner.Set(right - 50, 1))
                    using (ConsoleColorizer.UseColor(ConsoleColor.Green))
                    {
                        Console.Write("Hello");
                    }
                }
                else
                {
                    using (ConsolePositioner.Set(right - 50, 1))
                    using (ConsoleColorizer.UseColor(ConsoleColor.Green))
                    {
                        Console.Write("Goodbye");
                    }
                }

                Thread.Sleep(1000);
            }
        });
    }

    [Fact(Skip = "Temporary")]
    public void TestMethodBody()
    {
        var method = typeof(WAFStartupTests).GetMethod(nameof(TestMethodBody));
        var calls = method.GetInstructions(i => i.Code.Name == "call" && i.Operand.GetType().IsOneOf(typeof(MethodInfo), typeof(PropertyInfo))).ToList();
        Expression<Func<string, bool>> funcExpression = (s) => s.IsEmpty();
        var func = funcExpression.Compile();

        method = func.Method;
        calls = method.GetInstructions(i => i.Code.Name == "call" && i.Operand.GetType().IsOneOf(typeof(MethodInfo), typeof(PropertyInfo))).ToList();
    }

    [Fact]
    public void TestMemoryMapping()
    {
        var fileInfo = new FileInfo(@"C:\CloudIDEaaS\develop\WebSecurity\DataFilesRaw\iis-errors.data");

        Assert.True(fileInfo.Exists);

        using (var mappedFileInfo = fileInfo.MapFile())
        {
            var content = mappedFileInfo.MapFileGetContent<string>();

            Assert.True(content.Length > 0);

            using (var stream = mappedFileInfo.MapFileGetContent<Stream>())
            {
                Assert.True(stream.Length > 0);
            }
        }
    }

    [Fact/**/(Skip = "Needs work")/**/]
    public void TestKestrelWAFRuleCombos()
    {
        var baseRule = new Rule();
        var combosDictionary = new Dictionary<RuleUniquenessKind, List<RuleUniqueHash>>();
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var loggerAlertingService = serviceProvider.GetService<LoggerAlertingService>()!;
        var cancellationTokenSource = new CancellationTokenSource();
        var applicationLifetimeMock = new Mock<IApplicationLifetime>();
        IList<Rule> allRules;
        List<Rule> subsetRules;
        List<RuleUniqueHash> uniqueCombos;
        List<RuleUniqueHash>? allComboUseCases = null;
        List<RuleUniqueHash>? uniqueUseCases = null;
        List<string> properties;
        string commaSeparatedList;
        WAFMiddleware wafMiddleware;

        applicationLifetimeMock.SetupGet(m => m.ApplicationStopping).Returns(cancellationTokenSource.Token);

        wafMiddleware = CreateWAFMiddleware(contextAccessorMock.Object, loggerAlertingService, applicationLifetimeMock.Object);

        Console.WriteLine("\tLoading rulesets");

        baseRule = LoadRules();

        allRules = baseRule.Rules;

        Console.WriteLine($"\tFound {baseRule.Rules.Count} rules");

        foreach (var kind in EnumUtils.GetValues<RuleUniquenessKind>())
        {
            Console.WriteLine($"\tTesting {kind} use case");

            switch (kind)
            {
                case RuleUniquenessKind.UnaryBinaryPropertyPresence:
                    {
                        var unaryBinaryOperators = new List<string> { "LessThan", "GreaterThan", "LessThanOrEqual", "GreaterThanOrEqual", "IsTrue", "IsFalse" };
                        var paramUser = Expression.Parameter(typeof(WebContext));
                        MicroRulesEngine engine;

                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.GetPropertyPresence().ToString(), kind)).Where(h => h.Rule.Operator != null && h.Rule.Operator.IsOneOf(unaryBinaryOperators)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind, properties.Append("Operator").ToList(), (r) => r == null ? 
                            new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("PropertyPresence", null!), new KeyValuePair<string, object>("IsBinary", null!) } :
                            new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("PropertyPresence", r.GetPropertyPresence()), new KeyValuePair<string, object>("IsBinary", ExpressionType.TryParse(r.Operator, out ExpressionType tUnaryOrBinary) ? tUnaryOrBinary.IsOneOf(ExpressionType.IsFalse, ExpressionType.IsTrue) : null) });

                        engine = new MicroRulesEngine(true, baseRule.Rules.Count);

                        // single method test

                        foreach (var useCase in uniqueUseCases)
                        {
                            var rule = useCase.Rule;
                            ExpressionType tUnaryOrBinary;

                            if (ExpressionType.TryParse(rule.Operator, out tUnaryOrBinary))
                            {
                                var resultExpression = MicroRulesEngine.HandleUnaryBinary(rule, paramUser, tUnaryOrBinary);
                            }
                            else
                            {
                                DebugUtils.Break();
                            }
                        }
                    }
                    break;
                case RuleUniquenessKind.UnaryBinaryNonUnique:
                    {
                        var unaryBinaryOperators = new List<string> { "LessThan", "GreaterThan", "LessThanOrEqual", "GreaterThanOrEqual", "IsTrue", "IsFalse" };
                        var paramUser = Expression.Parameter(typeof(WebContext));
                        MicroRulesEngine engine;

                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        allComboUseCases = allRules.Select(r => new RuleUniqueHash(r, r.Operator, kind)).Where(h => h.Rule.Operator != null && h.Rule.Operator.IsOneOf(unaryBinaryOperators)).Randomize().ToList();

                        engine = new MicroRulesEngine(true, baseRule.Rules.Count);

                        // single method test

                        foreach (var useCase in allComboUseCases)
                        {
                            var rule = useCase.Rule;
                            ExpressionType tUnaryOrBinary;

                            if (ExpressionType.TryParse(rule.Operator, out tUnaryOrBinary))
                            {
                                var resultExpression = MicroRulesEngine.HandleUnaryBinary(rule, paramUser, tUnaryOrBinary);
                            }
                            else
                            {
                                DebugUtils.Break();
                            }
                        }
                    }
                    break;
                case RuleUniquenessKind.UnaryBinary:
                    {
                        var unaryBinaryOperators = new List<string> { "LessThan", "GreaterThan", "LessThanOrEqual", "GreaterThanOrEqual", "IsTrue", "IsFalse" };
                        var paramUser = Expression.Parameter(typeof(WebContext));
                        MicroRulesEngine engine;

                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.Operator, kind)).Where(h => h.Rule.Operator != null && h.Rule.Operator.IsOneOf(unaryBinaryOperators)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind, properties.Append("Operator").ToList());

                        engine = new MicroRulesEngine(true, baseRule.Rules.Count);

                        // single method test

                        foreach (var useCase in uniqueUseCases)
                        {
                            var rule = useCase.Rule;
                            ExpressionType tUnaryOrBinary;

                            if (ExpressionType.TryParse(rule.Operator, out tUnaryOrBinary))
                            {
                                var resultExpression = MicroRulesEngine.HandleUnaryBinary(rule, paramUser, tUnaryOrBinary);
                            }
                            else
                            {
                                DebugUtils.Break();
                            }
                        }
                    }
                    break;
                case RuleUniquenessKind.UniqueInputsForUnaryBinary:
                    {
                        var unaryBinaryOperators = new List<string> { "LessThan", "GreaterThan", "LessThanOrEqual", "GreaterThanOrEqual", "IsTrue", "IsFalse" };
                        var paramUser = Expression.Parameter(typeof(WebContext));
                        MicroRulesEngine engine;

                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.GetPublicPropertyValues().Where(p => p.Key.IsOneOf(properties)).OrderBy(p => p.Key).Select(p => string.Format("{0} is {1} with operator {2}", p.Key, p.Value == null ? "null" : "notnull", r.Operator)).ToMultiLineList(), kind)).Where(h => h.Rule.Operator != null && h.Rule.Operator.IsOneOf(unaryBinaryOperators)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind, properties.Append("Operator").ToList());

                        engine = new MicroRulesEngine(true, baseRule.Rules.Count);

                        // single method test

                        foreach (var useCase in uniqueUseCases)
                        {
                            var rule = useCase.Rule;
                            ExpressionType tUnaryOrBinary;

                            if (ExpressionType.TryParse(rule.Operator, out tUnaryOrBinary))
                            {
                                var resultExpression = MicroRulesEngine.HandleUnaryBinary(rule, paramUser, tUnaryOrBinary);
                            }
                            else
                            {
                                DebugUtils.Break();
                            }
                        }
                    }
                    break;
                case RuleUniquenessKind.UniqueInputs:
                    {
                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.GetPublicPropertyValues().Where(p => p.Key.IsOneOf(properties)).OrderBy(p => p.Key).Select(p => string.Format("{0} is {1}", p.Key, p.Value == null ? "null" : "notnull")).ToMultiLineList(), kind)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind, properties);
                    }
                    break;
                case RuleUniquenessKind.UniqueInputsAndOperators:
                    {
                        properties = new List<string> { "MemberName", "InputMethod", "InputMethod2", "TargetValue", "InputArgument", "InputArgument2" };

                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.GetPublicPropertyValues().Where(p => p.Key.IsOneOf(properties)).OrderBy(p => p.Key).Select(p => string.Format("{0} is {1} with operator {2}", p.Key, p.Value == null ? "null" : "notnull", r.Operator)).ToMultiLineList(), kind)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind, properties.Append("Operator").ToList());
                    }
                    break;
                case RuleUniquenessKind.NullOrNot:
                    {
                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.GetPublicPropertyValues().OrderBy(p => p.Key).Select(p => string.Format("{0} is {1}", p.Key, p.Value == null ? "null" : "notnull")).ToMultiLineList(), kind)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind);
                    }
                    break;
                case RuleUniquenessKind.UniqueOperators:
                    {
                        uniqueCombos = allRules.Select(r => new RuleUniqueHash(r, r.Operator, kind)).Randomize().ToList();
                        uniqueUseCases = uniqueCombos.DistinctBy(r => r.UniqueValue).ToList();

                        commaSeparatedList = uniqueUseCases.BuildCommaSeparated(kind);
                    }
                    break;
                default:
                    {
                        DebugUtils.Break();
                        uniqueUseCases = null!;
                    }
                    break;
            }

            if (uniqueUseCases != null)
            {
                Console.WriteLine($"\t{uniqueUseCases.Count} unique use cases for {kind.ToString()}");

                combosDictionary.AddToDictionaryListCreateIfNotExist(kind, uniqueUseCases);
                subsetRules = uniqueUseCases.Select(h => h.Rule).ToList();
            }
            else if (allComboUseCases != null)
            {
                Console.WriteLine($"\t{allComboUseCases.Count} comprehensive use cases for {kind.ToString()}");

                combosDictionary.AddToDictionaryListCreateIfNotExist(kind, allComboUseCases);
                subsetRules = allComboUseCases.Select(h => h.Rule).ToList();
            }
            else
            {
                DebugUtils.Break();
                subsetRules = null!;
            }    

            subsetRules.ForEach(r => { r.IsChainChild = false; r.ChainChildren?.Clear(); });  // disable chaining since we are using subsets

            baseRule.Rules = subsetRules;

            //compiledRule = new MicroRulesEngine(true, baseRule.Rules.Count).CompileRule<WebContext>(baseRule);
        }

        cancellationTokenSource.Cancel();
    }

    [Fact/**/(Skip = "Needs work")/**/]
    public void TestKestrelWAFSingleSpecificRules()
    {
        var testsPath = @"C:\Users\kenln\Downloads\coreruleset\tests\regression\tests";
        var regressionTestsParser = new RegressionTestsParser();
        var testInputExpectedResults = regressionTestsParser.Parse(testsPath, false);
        var testCaseIndex = 1;
        var testCaseCount = testInputExpectedResults.Count;
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var loggerAlertingService = serviceProvider.GetService<LoggerAlertingService>()!;
        var cancellationTokenSource = new CancellationTokenSource();
        var applicationLifetimeMock = new Mock<IApplicationLifetime>();
        var ruleIds = "901200,901140,901160,911100";
        WAFMiddleware wafMiddleware;

        applicationLifetimeMock.SetupGet(m => m.ApplicationStopping).Returns(cancellationTokenSource.Token);

        wafMiddleware = CreateWAFMiddleware(contextAccessorMock.Object, loggerAlertingService, applicationLifetimeMock.Object, ruleIds);

        Console.WriteLine($"\tFound {testCaseCount} test cases");

        foreach (var pair in testInputExpectedResults)
        {
            var testTitle = pair.Key;
            var expectedResult = pair.Value;
            var httpContext = expectedResult.HttpContext;
            var connection = httpContext.Connection;

            connection.Id = testTitle;
            connection.RemoteIpAddress = IPAddress.Parse(NetworkExtensions.GetRandomIpAddress());
            connection.RemotePort = NumberExtensions.GetRandomIntWithinRange(1, int.MaxValue);
            connection.LocalIpAddress = IPAddress.Parse(NetworkExtensions.GetLocalIPAddress());
            connection.LocalPort = 443;

            contextAccessorMock.SetupGet(m => m.HttpContext).Returns(httpContext);

            Console.WriteLine($"\tExecuting {testCaseIndex} of {testCaseCount} test cases");

            _ = wafMiddleware.Invoke(httpContext);

            testCaseIndex++;
        }

        // todo - add multi-threaded stress test

        loggerAlertingService.Stop();
        cancellationTokenSource.Cancel();
    }


    [Fact/*(Skip = "Needs work")*/]
    public void TestKestrelWAFGeoReport()
    {
        var testCaseIndex = 1;
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var loggerAlertingService = serviceProvider.GetService<LoggerAlertingService>()!;
        var cancellationTokenSource = new CancellationTokenSource();
        var applicationLifetimeMock = new Mock<IApplicationLifetime>();
        var ruleIds = "";
        var targetTestCaseCount = 1000;
        string reportsPath;
        MaxMindProxy maxMindProxy;
        WAFMiddleware wafMiddleware;
        Mock<IHostEnvironment> environmentMock;
        List<IPAddressCity> ipAddressCities;
        int testCaseCount;
        int totalCount;

        environmentMock = new Mock<IHostEnvironment>();
        environmentMock.SetupGet(m => m.ContentRootPath).Returns(contentRootPath);
        reportsPath = configuration["WAFReportsPath"]!;

        if (!Path.IsPathRooted(reportsPath))
        {
            reportsPath = Path.GetFullPath(Path.Combine(testsAssemblyFolder, reportsPath));
        }

        maxMindProxy = new MaxMindProxy(environmentMock.Object);

        applicationLifetimeMock.SetupGet(m => m.ApplicationStopping).Returns(cancellationTokenSource.Token);

        Console.WriteLine($"\tGenerating test data");

        ipAddressCities = LoadIPAddressCities(maxMindProxy);
        totalCount = ipAddressCities.Count;

        ipAddressCities = LoadIPAddressCities(maxMindProxy).Randomize().Take(targetTestCaseCount).ToList();

        testCaseCount = ipAddressCities.Count;

        wafMiddleware = CreateWAFMiddleware(contextAccessorMock.Object, loggerAlertingService, applicationLifetimeMock.Object, ruleIds);

        Console.WriteLine($"\tSelected {testCaseCount} test cases out of {totalCount}");

        foreach (var ipAddressCity in ipAddressCities)
        { 
            var blocked = NumberExtensions.GetRandomIntWithinRange(0, 100) > 95;
            var failCount = blocked ? NumberExtensions.GetRandomIntWithinRange(0, 1000) : 0;
            var ipAddress = ipAddressCity.IpAddress;

            Console.WriteLine($"\tExecuting {testCaseIndex} of {testCaseCount}");
            wafMiddleware.UpdateGeoReport(reportsPath, maxMindProxy, ipAddress.ToString(), blocked, failCount);

            testCaseIndex++;
        }

        // todo - add multi-threaded stress test

        loggerAlertingService.Stop();
        cancellationTokenSource.Cancel();

        Thread.Sleep(1000);
    }

    [Fact/**/(Skip = "Needs work")/**/]
    public void TestKestrelWAF()
    {
        var testsPath = @"C:\Users\kenln\Downloads\coreruleset\tests\regression\tests";
        var regressionTestsParser = new RegressionTestsParser();
        var testInputExpectedResults = regressionTestsParser.Parse(testsPath, false);
        var testCaseIndex = 1;
        var testCaseCount = testInputExpectedResults.Count;
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var loggerAlertingService = serviceProvider.GetService<LoggerAlertingService>()!;
        var cancellationTokenSource = new CancellationTokenSource();
        var applicationLifetimeMock = new Mock<IApplicationLifetime>();
        WAFMiddleware wafMiddleware;

        applicationLifetimeMock.SetupGet(m => m.ApplicationStopping).Returns(cancellationTokenSource.Token);

        wafMiddleware = CreateWAFMiddleware(contextAccessorMock.Object, loggerAlertingService, applicationLifetimeMock.Object);

        Console.WriteLine($"\tFound {testCaseCount} test cases");

        foreach (var pair in testInputExpectedResults)
        {
            var testTitle = pair.Key;
            var expectedResult = pair.Value;
            var httpContext = expectedResult.HttpContext;
            var connection = httpContext.Connection;

            connection.Id = testTitle;
            connection.RemoteIpAddress = IPAddress.Parse(NetworkExtensions.GetRandomIpAddress());
            connection.RemotePort = NumberExtensions.GetRandomIntWithinRange(1, int.MaxValue);
            connection.LocalIpAddress = IPAddress.Parse(NetworkExtensions.GetLocalIPAddress());
            connection.LocalPort = 443;

            contextAccessorMock.SetupGet(m => m.HttpContext).Returns(httpContext);

            Console.WriteLine($"\tExecuting {testCaseIndex} of {testCaseCount} test cases");

            _ = wafMiddleware.Invoke(httpContext);

            testCaseIndex++;
        }

        // todo - add multi-threaded stress test

        loggerAlertingService.Stop();
        cancellationTokenSource.Cancel();
    }

    [Fact]
    public void TestRequestResponseCapture()
    {
        var testsPath = @"C:\Users\kenln\Downloads\coreruleset\tests\regression\tests";
        var regressionTestsParser = new RegressionTestsParser();
        var testInputExpectedResults = regressionTestsParser.Parse(testsPath, false);
        var pair = testInputExpectedResults.Randomize().First();
        var testTitle = pair.Key;
        var httpContext = pair.Value.HttpContext;
        var connection = httpContext.Connection;
        var request = httpContext.Request;
        var response = httpContext.Response;
        var responseBody = httpContext.PrepareResponseBodyForCaptureAndCallNext((c) => Task.CompletedTask);
        var requestBody = request.GetBody();
        var requestLine = request.ToHttpString()!;
        var responseLine = response.ToHttpString()!;
        MaxMindProxy maxMindProxy;
        Mock<IHostEnvironment> environmentMock;
        ConnectionInfo connectionInfo;
        string requestText;
        string responseText;

        environmentMock = new Mock<IHostEnvironment>();
        environmentMock.SetupGet(m => m.ContentRootPath).Returns(contentRootPath);

        maxMindProxy = new MaxMindProxy(environmentMock.Object);

        connection.Id = testTitle;
        connection.RemoteIpAddress = IPAddress.Parse(NetworkExtensions.GetRandomIpAddress());
        connection.RemotePort = NumberExtensions.GetRandomIntWithinRange(1, int.MaxValue);
        connection.LocalIpAddress = IPAddress.Parse(NetworkExtensions.GetLocalIPAddress());
        connection.LocalPort = 443;

        connectionInfo = httpContext.GetConnectionInfo(maxMindProxy, environmentMock.Object);

        requestText = requestLine + "\r\n" + request.Headers.Select(h => h.Key + ":" + h.Value).ToMultiLineList() + "\r\n" + requestBody;
        responseText = responseLine + "\r\n" + response.Headers.Select(h => h.Key + ":" + h.Value).ToMultiLineList() + "\r\n" + responseBody;
    }

    private WAFMiddleware CreateWAFMiddleware(IHttpContextAccessor httpContextAccessor, LoggerAlertingService loggerAlertingService, IApplicationLifetime applicationLifetime, string? ruleIds = null)
    {
        RequestDelegate requestDelegate;
        Mock<ILogger<WAFMiddleware>> loggerMock;
        Mock<IHostEnvironment> environmentMock;
        MaxMindProxy maxMindProxy;
        Mock<IOptions<MemoryCacheOptions>> optionsAccessorMock;
        MemoryCache memoryCache;
        Mock<IOptions<Rule>> optionsRulesetMock;
        Mock<IServiceProvider> serviceProviderMock;
        KestrelWAFGlobalRepository kestrelWAFGlobalRepository;
        KestrelWAFTransactionRepository kestrelWAFTransactionRepository;
        KestrelWAFTransformationsActionsProvider kestrelWAFTransformationsActionsProvider;
        WAFDataFileCache wafDataFileCache;
        WAFMiddleware wafMiddleware;
        Rule baseRule;
        WAFWebSampleDbContext wafWebSampleDbContext;
        Helpers.Mocks mocks;

        requestDelegate = new RequestDelegate((c) => Task.CompletedTask);
        loggerMock = new Mock<ILogger<WAFMiddleware>>();
        environmentMock = new Mock<IHostEnvironment>();
        maxMindProxy = new MaxMindProxy(environmentMock.Object);
        optionsAccessorMock = new Mock<IOptions<MemoryCacheOptions>>();

        optionsAccessorMock.SetupGet(m => m.Value).Returns(new MemoryCacheOptions());

        Console.WriteLine("\tLoading rulesets");

        baseRule = LoadRules();

        if (ruleIds != null)
        {
            var ruleIdsList = ruleIds.Split(",").ToList();
            baseRule.Rules = baseRule.Rules.Where(r => ruleIdsList.Any(i => r.Id == i)).ToList();
        }

        Console.WriteLine($"\tFound {baseRule.Rules.Count} rules");

        memoryCache = new MemoryCache(optionsAccessorMock.Object);
        optionsRulesetMock = new Mock<IOptions<Rule>>();
        serviceProviderMock = new Mock<IServiceProvider>();

        optionsRulesetMock.SetupGet(m => m.Value).Returns(baseRule);

        environmentMock.SetupGet(m => m.ContentRootPath).Returns(contentRootPath);

        mocks = TestHelpers.SetupMocks(services, MockTypes.HydraDBContext);

        wafWebSampleDbContext = mocks.WAFWebSampleDbContext;

        wafDataFileCache = new WAFDataFileCache(configuration, environmentMock.Object, actionQueueService, serviceProvider, loggerFactory.CreateLogger<WAFDataFileCache>());
        kestrelWAFGlobalRepository = new KestrelWAFGlobalRepository(configuration, environmentMock.Object, serviceProvider, actionQueueService, wafDataFileCache, loggerFactory.CreateLogger<KestrelWAFGlobalRepository>());
        kestrelWAFTransactionRepository = new KestrelWAFTransactionRepository(configuration, environmentMock.Object, wafWebSampleDbContext, serviceProvider, httpContextAccessor, kestrelWAFGlobalRepository, actionQueueService, loggerFactory.CreateLogger<KestrelWAFTransactionRepository>());
        kestrelWAFTransformationsActionsProvider = new KestrelWAFTransformationsActionsProvider(configuration, environmentMock.Object, serviceProvider, httpContextAccessor, kestrelWAFGlobalRepository, loggerAlertingService, loggerFactory, loggerFactory.CreateLogger<KestrelWAFTransformationsActionsProvider>());

        serviceProvider.AddScopedPostBuild<ICrsRepository<ITransaction, ITransaction>>((s) => kestrelWAFTransactionRepository);
        serviceProvider.AddSingletonPostBuild<IMaxMindProxy>((s) => new MaxMindProxy(environmentMock.Object));

        serviceProviderMock.Setup(p => p.GetService(typeof(ICrsRepository<IGlobal, IGlobal>))).Returns(kestrelWAFGlobalRepository);
        serviceProviderMock.Setup(p => p.GetService(typeof(ICrsRepository<ITransaction, ITransaction>))).Returns(kestrelWAFTransactionRepository);
        serviceProviderMock.Setup(p => p.GetService(typeof(ITransformationsActionsProvider))).Returns(kestrelWAFTransformationsActionsProvider);

        wafMiddleware = new WAFMiddleware(requestDelegate, maxMindProxy, memoryCache, configuration, environmentMock.Object, applicationLifetime, optionsRulesetMock.Object, serviceProviderMock.Object, loggerMock.Object);

        return wafMiddleware;
    }

    private List<IPAddressCity> LoadIPAddressCities(IMaxMindProxy maxMindProxy)
    {
        var binaryIPAddressCitiesFolder = Path.Combine(testProjectFolder, "BinaryIPAddressCities");
        var binaryIPAddressCitiesDataFilePath = Path.Combine(binaryIPAddressCitiesFolder, "IPAddressCities.dat");  // intended to speed up loading for testing purposes only.
        var binaryIPAddressCitiesDataFile = new FileInfo(binaryIPAddressCitiesDataFilePath);
        var dataFilesFolder = configuration["WAFDataFilesPath"].RemoveEndIfMatches(@"\") + "Testing";
        var citiesCountriesFilePath = Path.Combine(dataFilesFolder, "CitiesCountries.tsv");
        var citiesCountriesContent = File.ReadAllText(citiesCountriesFilePath);
        var citiesCountries = citiesCountriesContent.GetLines().Skip(1).Select(l => new CityCountry(l)).ToList();
        var totalPopulation = citiesCountries.Sum(c => c.Population);
        var ipAddressCities = new List<IPAddressCity>();
        var count = 10_000;
        var index = 0;
        int originalCount;

        if (!binaryIPAddressCitiesDataFile.Exists)
        {
            Console.WriteLine("\tRebuilding binary IPAddressCities file.. please wait");

            for (var x = 0; x < count; x++)
            {
                var ipAddress = IPAddress.Parse(NetworkExtensions.GetRandomIpAddress());
                var cityResponse = maxMindProxy.GetCity(ipAddress);

                if (cityResponse != null)
                {
                    var city = cityResponse.City.Name;
                    var ipAddressCity = new IPAddressCity(ipAddress, city);

                    var populatedCityCountry = citiesCountries.SingleOrDefault(c => c.City == city);

                    if (populatedCityCountry != null)
                    {
                        ipAddressCity.Weight = populatedCityCountry.Population.As<float>() / totalPopulation.As<float>();
                    }
                    else
                    {
                        ipAddressCity.Weight = 10_0000 / totalPopulation.As<float>();
                    }

                    ipAddressCities.Add(ipAddressCity);
                }

                index++;

                if (index.ScopeRange(1000) == 1000)
                {
                    if (cityResponse == null)
                    {
                        ipAddressCities.Add(new IPAddressCity(ipAddress, "{Unknown}"));
                    }

                    Console.WriteLine($"Created {index} of {count} records");
                }
            }

            originalCount = ipAddressCities.Count;
            ipAddressCities = ipAddressCities.SelectMany(i => { var array = new IPAddressCity[(int)(i.Weight * 50000)]; Array.Fill(array, i.DefaultClone()); return array; }).ToList();

            using (var stream = binaryIPAddressCitiesDataFile.OpenWrite())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                formatter.Serialize(stream, ipAddressCities);
            }
        }
        else
        {
            var failedToLoad = false;

            using (var stream = binaryIPAddressCitiesDataFile.OpenRead())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                try
                {
                    ipAddressCities = (List<IPAddressCity>)formatter.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    failedToLoad = true;
                }
            }

            if (failedToLoad)
            {
                binaryIPAddressCitiesDataFile.Delete();
                return LoadIPAddressCities(maxMindProxy);
            }
        }

        return ipAddressCities;
    }

    private Rule LoadRules()
    {
        var rulesetConfig = configuration.GetSection("WAFConfiguration:Ruleset");
        var binaryRulesFolder = Path.Combine(testProjectFolder, "BinaryRules");
        var binaryRulesDataFilePath = Path.Combine(binaryRulesFolder, "Rules.dat");  // intended to speed up rule loading for testing purposes only.
        var binaryRulesDataFile = new FileInfo(binaryRulesDataFilePath);
        var rulesetFile = new FileInfo(rulesetFilePath);
        var baseRule = new Rule();

        if (!binaryRulesDataFile.Exists || binaryRulesDataFile.LastWriteTime < rulesetFile.LastWriteTime)
        {
            Console.WriteLine("\tRebuilding binary rules file");
            Console.WriteLine("\tBinding rulesets");

            ConfigurationBinder.Bind(rulesetConfig, baseRule);

            using (var stream = binaryRulesDataFile.OpenWrite())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                formatter.Serialize(stream, baseRule);
            }
        }
        else
        {
            var failedToLoad = false;

            using (var stream = binaryRulesDataFile.OpenRead())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                try
                {
                    baseRule = (Rule)formatter.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    failedToLoad = true;
                }
            }

            if (failedToLoad)
            {
                binaryRulesDataFile.Delete();
                return LoadRules();
            }
        }

        return baseRule;
    }

    [Fact]
    public void TestSmtp()
    {
        var emailSettings = configuration.Get<EmailSettings>("Monitoring:EmailTransportSettings:0");
        var send = false;
        var smtpClient = new SmtpClient(emailSettings.SmtpHost)
        {
            Port = emailSettings.SmtpPort,
            Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
            EnableSsl = true,
        };

        if (send)
        {
            smtpClient.Send(emailSettings.From, emailSettings.To, "Test", "Test");
        }
    }

    [Fact]
    public void TestRegex()
    {
        var regex = new Regex("0");
        var isMatch = regex.IsMatch("0");
        var value = (object)true;
        var isTrue = value.Equals((object) false);
    }

    [Fact]
    public void TestExpressionLogger()
    {
        string sourceContext;
        string expression;
        LogEvent logEvent;
        bool isTrue;
        CompiledExpression compiled;
        bool canCompile;

        sourceContext = "Utils.Kestrel.KestrelStaticFileMiddleware";
        expression = $"SourceContext like '{sourceContext}'";
        logEvent = new LogEvent(DateTime.Now, LogEventLevel.Debug, new Exception("Client log exception"), new MessageTemplate(LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE, new List<MessageTemplateToken>()), new List<LogEventProperty>
        {
            new LogEventProperty("SourceContext", new ScalarValue(sourceContext.SurroundWith("{", "}")))
        });
        
        compiled = SerilogExpression.Compile(expression);
        canCompile = CoerceIsTrue(compiled(logEvent), out isTrue);

        sourceContext = "Hydra.WebApi.Controllers.User.V1.LogController";
        expression = $"SourceContext like '{ sourceContext }'";
        logEvent = new LogEvent(DateTime.Now, LogEventLevel.Debug, new Exception("Client log exception"), new MessageTemplate(LoggerExtensions.SOURCE_CONTEXT_OUTPUT_TEMPLATE, new List<MessageTemplateToken>()), new List<LogEventProperty>
        {
            new LogEventProperty("SourceContext", new ScalarValue(sourceContext.SurroundWith("{", "}")))
        });

        compiled = SerilogExpression.Compile(expression);
        canCompile = CoerceIsTrue(compiled(logEvent), out isTrue);
    }

    public bool CoerceIsTrue(LogEventPropertyValue? value, out bool boolean)
    {
        if (value is ScalarValue scalarValue && scalarValue.Value is bool flag)
        {
            boolean = flag;
            return true;
        }
        boolean = false;
        return false;
    }

    public void Dispose()
    {
        actionQueueService.Stop();
        serilogLogger.Dispose();
        serviceProvider.Dispose();
        _ = hostedService.StopAsync(CancellationToken.None);
    }
}