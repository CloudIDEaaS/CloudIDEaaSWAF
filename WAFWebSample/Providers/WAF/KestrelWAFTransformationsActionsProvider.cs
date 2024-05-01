using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Utils;
using WebSecurity;
using WebSecurity.Interfaces;
using WebSecurity.KestrelWAF;
using WebSecurity.KestrelWAF.RulesEngine;
using WebSecurity.Models;

namespace WAFWebSample.WebApi.Providers.WAF;

public class KestrelWAFTransformationsActionsProvider : ITransformationsActionsProvider
{
    private readonly ILogger<KestrelWAFTransformationsActionsProvider> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment environment;
    private readonly IServiceProvider serviceProvider;
    private readonly ICrsRepository<IGlobal, IGlobal> globalRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly LoggerAlertingService loggerAlertingService;
    private readonly Dictionary<string, ILogger> loggers;
    private bool verboseOutput;

    public KestrelWAFTransformationsActionsProvider(IConfiguration configuration, IHostEnvironment environment, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, ICrsRepository<IGlobal, IGlobal> globalRepository, LoggerAlertingService loggerAlertingService, ILoggerFactory loggerFactory, ILogger<KestrelWAFTransformationsActionsProvider> logger)
    {
        string? wafVerboseOutputConfigValue;

        this.logger = logger;
        this.loggerFactory = loggerFactory;
        this.configuration = configuration;
        this.environment = environment;
        this.serviceProvider = serviceProvider;
        this.globalRepository = globalRepository;
        this.httpContextAccessor = httpContextAccessor;
        this.loggerAlertingService = loggerAlertingService;
        this.loggers = new Dictionary<string, ILogger>();

        wafVerboseOutputConfigValue = configuration["WAFVerboseOutput"];

        if (wafVerboseOutputConfigValue != null)
        {
            this.verboseOutput = bool.Parse(wafVerboseOutputConfigValue);
        }
    }

    public TransformationActionResult HandleTransformationsActions(Rule rule, WebContext webContext, Dictionary<string, string> groupCaptures, ContextMatches contextMatches, JToken transformationsActions, bool failed, HttpContext httpContext)
    {
        var statusCode = HttpStatusCode.OK;
        var theseLoggers = new List<ILogger>();
        var tags = new List<string>();
        var transformations = new List<string>();
        var actions = new List<Action>();
        var controlVariables = new List<ControlVariable>();
        var capture = false;
        var deny = false;
        var block = false;
        var pass = false;
        var chain = false;
        string? id = null;
        string? message = null;
        string? version = null;
        string? severity = null;
        string? skipAfter = null;
        Phase? phase = null;

        foreach (JProperty transAction in transformationsActions)
        {
            switch (transAction.Name)
            {
                case "id":
                    id = transAction.Value.ToObject<string>();
                    break;
                case "logs":
                    {
                        foreach (var logObject in transAction.Value.Children())
                        {
                            var logType = logObject.ToObject<string>();
                            string? logCategory;
                            ILogger logger;

                            if (loggers.ContainsKey(logType))
                            {
                                logger = loggers[logType];
                            }
                            else
                            {
                                switch (logType)
                                {
                                    case "log":
                                        logCategory = "WAFLog";
                                        break;
                                    case "auditlog":
                                        logCategory = "WAFAuditLog";
                                        break;
                                    case "nolog":
                                    case "noauditlog":
                                        continue;
                                    default:
                                        DebugUtils.Break();
                                        logCategory = null;
                                        break;
                                }

                                logger = loggerFactory.CreateLogger(logCategory);

                                loggers.Add(logType, logger);   
                            }

                            theseLoggers.Add(logger);
                        }
                    }
                    break;
                case "capture":
                    capture = true;
                    break;
                case "deny":
                    deny = true;
                    break;
                case "chain":
                    chain = true;
                    break;
                case "pass":
                    pass = true;
                    break;
                case "block":
                    block = true;
                    break;
                case "msg":
                    message = transAction.Value.ToObject<string>();
                    break;
                case "phase":
                    phase = EnumUtils.GetValue<Phase>(transAction.Value.ToObject<string>());
                    break;
                case "ver":
                    version = transAction.Value.ToObject<string>();
                    break;
                case "severity":
                    severity = transAction.Value.ToObject<string>();
                    break;
                case "skipafter":
                    skipAfter = transAction.Value.ToObject<string>();
                    break;
                case "tags":
                    {
                        foreach (var tagObject in transAction.Value.Children())
                        {
                            var tag = tagObject.ToObject<string>();

                            tags.Add(tag);
                        }
                    }
                    break;
                case "transformations":
                    {
                        foreach (var transformationObject in transAction.Value.Children())
                        {
                            var transformation = transformationObject.ToObject<string>();

                            transformations.Add(transformation);
                        }   
                    }
                    break;
                case "followUpActions":
                    {
                        actions.Add(() =>
                        {
                            var rule2 = rule;
                            var contextMatches2 = contextMatches;

                            foreach (var followupAction in transAction.Value.Children().OfType<JProperty>().Select(p => (JProperty)((JObject)p.Value).Children().First()))
                            {
                                switch (followupAction.Name)
                                {
                                    case "setvar":
                                        {
                                            var pattern = @"^(?<model>\w+)\.(?<variable>[\w_0-9-]+)=((?<value>[\w_0-9-]+)|(%\{(?<otherVariable>(?<model2>\w+)\.(?<variable2>[\w_0-9-]+)\}))|(?<value>.+))$";
                                            var expression = followupAction.Value.ToObject<string>();

                                            if (expression.RegexIsMatch(pattern))
                                            {
                                                var match = expression.RegexGetMatch(pattern);
                                                var model = match.GetGroupValue("model");
                                                var variable = match.GetGroupValue("variable");
                                                var value = match.GetGroupValue("value");
                                                var model2 = match.GetGroupValue("model2");
                                                var variable2 = match.GetGroupValue("variable2");

                                                switch (model)
                                                {
                                                    case "TX":
                                                    case "tx":
                                                        {
                                                            var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
                                                            var settings = repository.GetSettings();

                                                            if (value.IsNullOrEmpty() && !variable2.IsNullOrEmpty())
                                                            {
                                                                var var2Value = settings[variable2];

                                                                settings[variable] = var2Value;
                                                            }
                                                            else if (value.RegexIsMatch("%{.*?}"))
                                                            {
                                                                var fullValue = GetValueFromVariables(groupCaptures, value, settings);

                                                                settings[variable] = fullValue;
                                                            }
                                                            else
                                                            {
                                                                settings[variable] = value;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        DebugUtils.Break();
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                pattern = @"^(?<model>\w+)\.(?<variable>[\w_0-9-%\{\}\.]+)=((?<value>[\w_0-9-]+)|(%\{(?<otherVariable>(?<model2>\w+)\.(?<variable2>[\w_0-9-]+)\}))|(?<value>.+))$";

                                                if (expression.RegexIsMatch(pattern))
                                                {
                                                    var match = expression.RegexGetMatch(pattern);
                                                    var model = match.GetGroupValue("model");
                                                    var variable = match.GetGroupValue("variable");
                                                    var varValue = match.GetGroupValue("value");
                                                    var model2 = match.GetGroupValue("model2");
                                                    var variable2 = match.GetGroupValue("variable2");

                                                    switch (model)
                                                    {
                                                        case "TX":
                                                        case "tx":
                                                            {
                                                            }
                                                            break;
                                                        default:
                                                            DebugUtils.Break();
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    DebugUtils.Break();
                                                }
                                            }
                                        }
                                        break;
                                    case "initcol":
                                        {

                                        }
                                        break;
                                    case "logdata":
                                        {
                                            var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
                                            var settings = repository.GetSettings();
                                            var dataExpression = followupAction.Value.ToObject<string>();
                                            var fullValue = GetValueFromVariables(groupCaptures, webContext, dataExpression, settings, contextMatches);
                                        }
                                        break;
                                    case "ctl":
                                        {
                                            var match = followupAction.Value.ToObject<string>().RegexGetMatch(@"(?<variable>\w+?)=(?<value>.+)?(;(?<target>.+?))?$");
                                            var variable = match.GetGroupValue("variable");
                                            var varValue = match.GetGroupValue("value");
                                            var target = match.GetGroupValue("target");

                                            switch (variable)
                                            {
                                                case "forceRequestBodyVariable":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.ForceRequestBodyVariable, varValue, target));
                                                    }
                                                    break;
                                                case "requestBodyProcessor":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.RequestBodyProcessor, varValue, target));
                                                    }
                                                    break;
                                                case "ruleRemoveByTag":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.RuleRemoveByTag, varValue, target));
                                                    }
                                                    break;
                                                case "ruleRemoveTargetByTag":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.RuleRemoveTargetByTag, varValue, target));
                                                    }
                                                    break;
                                                case "ruleRemoveTargetById":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.RuleRemoveTargetById, varValue, target));
                                                    }
                                                    break;
                                                case "auditEngine":
                                                    {
                                                        controlVariables.Add(new ControlVariable(rule, ControlVariableType.AuditEngine, varValue, target));
                                                    }
                                                    break;
                                                default:
                                                    DebugUtils.Break();
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                        DebugUtils.Break();
                                        break;
                                }
                            }
                        });
                    }
                    break;
                case "status":

                    if (failed)
                    {
                        statusCode = (HttpStatusCode)transAction.Value.ToObject<int>();
                    }

                    break;
                default:
                    DebugUtils.Break();
                    break;
            }
        }

        if (actions.Count > 0 && (capture || failed || deny))
        {
            foreach (var action in actions)
            {
                action();
            }
        }

        if (theseLoggers.Count > 0)
        {
            if (verboseOutput || failed) 
            {
                loggerAlertingService.QueueAction(() =>
                {
                    string logMessage;
                    var maxMindProxy = serviceProvider.GetService<IMaxMindProxy>();

                    if (failed)
                    {
                        logMessage = string.Format("WAF violation encountered. Rule: {0}, Message: '{1}', Version: '{2}', Severity: {3}, Phase: {4}, Tags: '{5}', AdditionalInfo: \r\n", id, version, message, phase.ToString(), severity, tags.ToCommaDelimitedList());
                    }
                    else
                    {
                        logMessage = string.Format("WAF message due to VerboseOutput, but rule passed, ignore message. Rule: {0}, Message: '{1}', Version: '{2}', Severity: {3}, Phase: {4}, Tags: '{5}', AdditionalInfo: \r\n", id, message, version, phase.ToString(), severity, tags.ToCommaDelimitedList());
                    }

                    logMessage = httpContext.AppendAdditionalInfo(logMessage, maxMindProxy, environment);

                    foreach (var logger in theseLoggers)
                    {
                        LogSeverity logSeverity;

                        switch (severity)
                        {
                            case "CRITICAL":
                                logSeverity = failed ? LogSeverity.Critical : LogSeverity.Information;
                                break;
                            case "WARNING":
                                logSeverity = failed ? LogSeverity.Warning : LogSeverity.Information;
                                break;
                            case "NOTICE":
                                logSeverity = LogSeverity.Information;
                                break;
                            default:
                                logSeverity = LogSeverity.None;
                                break;
                        }

                        loggerAlertingService.QueueLog(new LogInfo(logMessage, logSeverity, logger));
                    }
                });
            }
        }

        // quality checks

        if (statusCode.IsSuccess() && failed && (deny || block))
        {
            if (block && !deny)
            {
                var logMessage = string.Format("WAF violation encountered for rule {0}, with status code {1} but not denied. Consider adding an error status to config", id, statusCode);
                ILogger logger;

                if (loggers.ContainsKey("log"))
                {
                    logger = loggers["log"];
                }
                else
                {
                    logger = loggerFactory.CreateLogger("WAFLog");
                }

                using (ConsoleColorizer.UseColor(ConsoleColor.DarkYellow))
                {
                    Console.WriteLine(logMessage);
                }

                loggerAlertingService.QueueLog(new LogInfo(logMessage, LogSeverity.Warning, logger));
            }
            else
            {
                DebugUtils.Break();
            }
        }
        
        if (statusCode != HttpStatusCode.OK && (pass || !failed))
        {
            DebugUtils.Break();
        }

        return new TransformationActionResult(statusCode, skipAfter, chain, controlVariables);
    }

    private string GetValueFromVariables(Dictionary<string, string> groupCaptures, WebContext webContext, string pattern, ITransaction settings, ContextMatches contextMatches)
    {
        var matches = pattern.RegexGetMatches("%{.*?}");
        var txReplacementActions = new List<Action>();
        string fullValue = pattern;

        foreach (var match2 in matches)
        {
            var variableMatch = match2.Value.RegexGetMatch(@"%{(?<variable>[\w_0-9-]+)(\.(?<index>[\w_0-9-]+))?}");
            var variable = variableMatch.GetGroupValue("variable");
            var index = variableMatch.GetGroupValue("index");

            switch (variable)
            {
                case "MATCHED_VAR":
                    fullValue = fullValue.Replace(match2.Value, contextMatches.MatchedVar.Value.AsDisplayText());
                    break;
                case "MATCHED_VAR_NAME":
                    fullValue = fullValue.Replace(match2.Value, contextMatches.MatchedVarName.AsDisplayText());
                    break;
                case "MATCHED_VARS":
                    DebugUtils.Break();
                    break;
                case "MATCHED_VAR_NAMES":
                    DebugUtils.Break();
                    break;
                case "TX":
                case "tx":
                    {
                        txReplacementActions.Add(new Action(() =>
                        {
                            fullValue = GetValueFromVariables(groupCaptures, fullValue, settings);
                        }));
                    }
                    break;
                default:
                   {
                        var type = webContext.GetType();
                        var property = type.GetProperties().Where(p => p.HasCustomAttribute<OwaspNameAttribute>()).Single(p => p.GetCustomAttribute<OwaspNameAttribute>().Name.AsCaseless() == variable);
                        var result = (string) property.GetValue(webContext, null)!;

                        if (!index.IsNullOrEmpty())
                        {
                            DebugUtils.Break();
                        }

                        fullValue = result;
                    }
                    break;
            }
        }

        foreach (var action in txReplacementActions)
        {
            action();
        }

        return fullValue;
    }

    private string GetValueFromVariables(Dictionary<string, string> groupCaptures, string pattern, ITransaction settings)
    {
        var matches = pattern.RegexGetMatches("%{.*?}");
        string fullValue = pattern;

        foreach (var match2 in matches)
        {
            var variableMatch = match2.Value.RegexGetMatch(@"%{((?<model>\w+)\.(?<variable>[\w_0-9-]+))|(?<value>.+)}");
            var variableModel = variableMatch.GetGroupValue("model");
            var variableVariable = variableMatch.GetGroupValue("variable");
            var variableValue = variableMatch.GetGroupValue("value");

            if (variableModel.AsCaseless() == "tx" && !variableVariable.IsNullOrEmpty())
            {
                if (variableVariable.RegexIsMatch(@"^\d+?$"))
                {
                    var matchedValue = groupCaptures[variableVariable];

                    fullValue = fullValue.Replace(match2.Value, matchedValue);
                }
                else
                {
                    var matchedValue = settings[variableVariable].ToString();
                    var regexWithOperators = new Regex(@"(?<prefix>.+)(?<number>\d+?)(?<suffix>.+)?$");

                    fullValue = fullValue.Replace(match2.Value, matchedValue);

                    if (regexWithOperators.IsMatch(fullValue))
                    {
                        var matchOperators = regexWithOperators.Match(fullValue);
                        var number = int.Parse(matchOperators.GetGroupValue("number"));
                        var prefix = matchOperators.GetGroupValue("prefix");
                        var suffix = matchOperators.GetGroupValue("suffix");

                        switch (prefix)
                        {
                            case "":
                                break;
                            case "+":
                                number++;
                                break;
                            default:
                                DebugUtils.Break();
                                break;
                        }

                        switch (suffix)
                        {
                            case "":
                                break;
                            default:
                                DebugUtils.Break();
                                break;
                        }

                        fullValue = number.ToString();
                    }
                }
            }
            else
            {
                DebugUtils.Break();
            }
        }

        return fullValue;
    }
}
