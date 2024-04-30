using Castle.DynamicProxy.Internal;
using Microsoft.PowerShell.Commands;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Utils;
using Utils.IntermediateLanguage;
using WebSecurity.KestrelWAF;
using TypeExtensions = Utils.TypeExtensions;

namespace WebSecurity.KestrelWAF.RulesEngine;

public class MicroRulesEngine
{
    private static readonly ExpressionType[] nestedOperators = new ExpressionType[] {ExpressionType.And, ExpressionType.AndAlso, ExpressionType.Or, ExpressionType.OrElse};
    private static readonly Lazy<MethodInfo> miObjectEquals = new Lazy<MethodInfo>(() => typeof(object).GetMethod("Equals", new[] { typeof(object) })!);
    private static readonly Lazy<MethodInfo> miRegexIsMatch = new Lazy<MethodInfo>(() => typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string), typeof(RegexOptions) })!);
    private static readonly Lazy<MethodInfo> miRegexIEnumerableHasMatch = new Lazy<MethodInfo>(() => typeof(StringExtensions).GetMethod("RegexHasMatch", new[] { typeof(IEnumerable<string>), typeof(string), typeof(RegexOptions) })!);
    private static readonly Lazy<MethodInfo> miRegexIDictionaryHasMatch = new Lazy<MethodInfo>(() => typeof(StringExtensions).GetMethod("RegexHasMatch", new[] { typeof(IDictionary<string, string>), typeof(string), typeof(RegexOptions) })!);
    private static readonly Lazy<MethodInfo> miRegexKeyValuePairHasMatch = new Lazy<MethodInfo>(() => typeof(StringExtensions).GetMethod("RegexHasMatch", new[] { typeof(KeyValuePair<string, string>), typeof(string), typeof(RegexOptions) })!);
    private static readonly Lazy<MethodInfo> miRegexMatch = new Lazy<MethodInfo>(() => typeof(Regex).GetMethod("Match", new[] { typeof(string), typeof(string), typeof(RegexOptions) })!);
    private static readonly Lazy<MethodInfo> miEndsWith = new Lazy<MethodInfo>(() => typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!);
    private static readonly Lazy<MethodInfo> miListContains = new Lazy<MethodInfo>(() => typeof(IList).GetMethod("Contains", new[] { typeof(object) })!);
    private static readonly Lazy<MethodInfo> miStringContains = new Lazy<MethodInfo>(() => typeof(string).GetMethod("Contains", new[] { typeof(string) })!);
    private static readonly Tuple<string, Lazy<MethodInfo>>[] enumerationMethodsByName = new Tuple<string, Lazy<MethodInfo>>[] { Tuple.Create("Any", new Lazy<MethodInfo>(() => GetLinqMethod("Any", 2))), Tuple.Create("All", new Lazy<MethodInfo>(() => GetLinqMethod("All", 2))),};
    private static readonly Lazy<MethodInfo> miIntParse = new Lazy<MethodInfo>(() => typeof(Int32).GetMethod("Parse", new Type[] { typeof(string) })!);
    private static readonly Lazy<MethodInfo> miLongParse = new Lazy<MethodInfo>(() => typeof(Int64).GetMethod("Parse", new Type[] { typeof(string) })!);
    private static readonly Lazy<MethodInfo> miIntTryParse = new Lazy<MethodInfo>(() => typeof(Int32).GetMethod("TryParse", new Type[] { typeof(string), Type.GetType("System.Int32&")! })!);
    private static readonly Lazy<MethodInfo> miFloatTryParse = new Lazy<MethodInfo>(() => typeof(Single).GetMethod("TryParse", new Type[] { typeof(string), Type.GetType("System.Single&")! })!);
    private static readonly Lazy<MethodInfo> miDoubleTryParse = new Lazy<MethodInfo>(() => typeof(Double).GetMethod("TryParse", new Type[] { typeof(string), Type.GetType("System.Double&")! })!);
    private static readonly Lazy<MethodInfo> miDecimalTryParse = new Lazy<MethodInfo>(() => typeof(Decimal).GetMethod("TryParse", new Type[] { typeof(string), Type.GetType("System.Decimal&")! })!);
    private static readonly Lazy<MethodInfo> miAddRuleToStackAndReturnResult = new Lazy<MethodInfo>(() => typeof(MicroRulesEngine).GetMethod("AddRuleToStackAndReturnResult", new[] { typeof(Rule), typeof(WebContext), typeof(Func<WebContext, bool>) })!);
    private static readonly Lazy<MethodInfo> miAddRuleToStackWithMatchAndReturnResult = new Lazy<MethodInfo>(() => typeof(MicroRulesEngine).GetMethod("AddRuleToStackWithMatchAndReturnResult", new[] { typeof(Rule), typeof(WebContext), typeof(Func<WebContext, Match>) })!);
    private static readonly Regex _regexIndexed = new Regex(@"(?'Collection'\w+)\[(?:(?'Index'\d+)|(?:['""](?'Key'\w+)[""']))\]", RegexOptions.Compiled);
    private static string[] logicOperators = new string[] { mreOperator.And.ToString("g"), mreOperator.AndAlso.ToString("g"), mreOperator.Or.ToString("g"), mreOperator.OrElse.ToString("g")};
    private static string[] comparisonOperators = new string[] { mreOperator.Equal.ToString("g"), mreOperator.GreaterThan.ToString("g"), mreOperator.GreaterThanOrEqual.ToString("g"), mreOperator.LessThan.ToString("g"), mreOperator.LessThanOrEqual.ToString("g"), mreOperator.NotEqual.ToString("g") };
    private static string[] hardCodedStringOperators = new string[] { mreOperator.IsMatch.ToString("g"), mreOperator.IsInteger.ToString("g"), mreOperator.IsSingle.ToString("g"), mreOperator.IsDouble.ToString("g"), mreOperator.IsDecimal.ToString("g"), mreOperator.IsInInput.ToString("g") };
    public static ThreadLocal<Stack<Rule>> executedRulesThreadLocal = new ThreadLocal<Stack<Rule>>(() => new Stack<Rule>());
    public static ThreadLocal<RuleStats> ruleStatsThreadLocal = new ThreadLocal<RuleStats>(() => new RuleStats());
    public static ThreadLocal<Dictionary<string, string>> regexGroupCapturesThreadLocal = new ThreadLocal<Dictionary<string, string>>(() => new Dictionary<string, string>());
    public static ThreadLocal<string> lastCallsThreadLocal = new ThreadLocal<string>(() => string.Empty);
    private static bool warnedAboutTryCatch;
    private static bool providedBuildStartMessage;
    public static event TypeMismatchEventHandler TypeMismatchEvent;
    public static event RuleExecutedEventHandler RuleExecutedEvent;
    public static event RuleExecutingEventHandler RuleExecutingEvent;
    public static event HandleMethodAmbiguityEventHandler HandleMethodAmbiguityEvent;
    public static List<string>? WAFDebugBreakOnRules { get; set; }

    public static string LastCalls
    {
        get
        {
            return lastCallsThreadLocal.Value!;
        }

        set
        {
            lastCallsThreadLocal.Value = value;
        }
    }

    public static Stack<Rule> ExecutedRules
    {
        get
        {
            return executedRulesThreadLocal.Value!;
        }
    }

    public static Dictionary<string, string> GroupCaptures
    {
        get
        {
            return regexGroupCapturesThreadLocal.Value!;
        }
    }

    private static void TestBreakOnRulePreExec(string ruleId)
    {
        if (WAFDebugBreakOnRules != null && WAFDebugBreakOnRules.Contains(ruleId))
        {
#if DEBUG
            DebugUtils.Break();
#endif
        }
    }

    public static bool AddRuleToStackAndReturnResult(Rule rule, WebContext webContext, Func<WebContext, bool> func)
    {
        bool success;
        RuleExecutingEventArgs eventArgsExecuting;
        RuleExecutedEventArgs eventArgsExecuted;
        var method = func.Method;
        var calls = method.GetInstructions(i => i.Code.Name.IsOneOf("call", "callvirt") && i.Operand.GetType().IsOneOf(typeof(MethodBase))).ToList();
        var webContextCalls = calls.Where(c => ((MethodBase)c.Operand!).DeclaringType == typeof(WebContext));
        var methodList = webContextCalls.Select(c => ((MethodBase)c.Operand!).GetFullName(true)).ToCommaDelimitedList();
        var id = rule.Id;

#if DEBUG
        TestBreakOnRulePreExec(id);
#endif

        LastCalls = methodList;

        if (VerboseOutput)
        {
            Console.WriteLine($"Calling: { methodList }");
        }

        ExecutedRules.Push(rule);

        eventArgsExecuting = new RuleExecutingEventArgs(rule, RulesExecuted, RulesCount, webContext);

        RuleExecutingEvent(typeof(MicroRulesExtensions), eventArgsExecuting);

        if (eventArgsExecuting.SkipRule)
        {
            return false;
        }

        success = func(webContext);

        eventArgsExecuted = new RuleExecutedEventArgs(rule, RulesExecuted, RulesCount, webContext, success);

        RuleExecutedEvent(typeof(MicroRulesExtensions), eventArgsExecuted);

        RulesExecuted++;

        return eventArgsExecuted.ReturnSuccess;
    }

    public static bool AddRuleToStackWithMatchAndReturnResult(Rule rule, WebContext webContext, Func<WebContext, Match> func)
    {
        RuleExecutingEventArgs eventArgsExecuting;
        RuleExecutedEventArgs eventArgsExecuted;
        Match match;
        var method = func.Method;
        var calls = method.GetInstructions(i => i.Code.Name.IsOneOf("call", "callvirt") && i.Operand.GetType().IsOneOf(typeof(MethodBase))).ToList();
        var webContextCalls = calls.Where(c => ((MethodBase)c.Operand!).DeclaringType == typeof(WebContext));
        var methodList = webContextCalls.Select(c => ((MethodBase)c.Operand!).GetFullName(true)).ToCommaDelimitedList();
        var id = rule.Id;

#if DEBUG
        TestBreakOnRulePreExec(id);
#endif

        LastCalls = methodList;

        if (VerboseOutput)
        {
            Console.WriteLine($"Calling: {methodList}");
        }

        ExecutedRules.Push(rule);

        eventArgsExecuting = new RuleExecutingEventArgs(rule, RulesExecuted, RulesCount, webContext);

        RuleExecutingEvent(typeof(MicroRulesExtensions), eventArgsExecuting);

        match = func(webContext);

        if (match.Success)
        {
            var captures = regexGroupCapturesThreadLocal.Value!;

            captures.Clear();

            foreach (var group in match.Groups.Cast<Group>())
            {
                captures.Add(group.Name, group.Value);
            }
        }

        eventArgsExecuted = new RuleExecutedEventArgs(rule, RulesExecuted, RulesCount, webContext, match.Success);

        RuleExecutedEvent(typeof(MicroRulesExtensions), eventArgsExecuted);

        RulesExecuted++;

        return eventArgsExecuted.ReturnSuccess;
    }

    public Func<T, bool> CompileRule<T>(Rule r)
    {
        var paramUser = Expression.Parameter(typeof(T));
        var expr = GetExpressionForRule(typeof(T), r, paramUser);

        return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
    }

    public static Expression<Func<T, bool>> ToExpression<T>(Rule r, bool useTryCatchForNulls = false)
    {
        var paramUser = Expression.Parameter(typeof(T));
        Expression expr = GetExpressionForRule(typeof(T), r, paramUser, useTryCatchForNulls);

        return Expression.Lambda<Func<T, bool>>(expr, paramUser);
    }

    public static Func<T, bool> ToFunc<T>(Rule r, bool useTryCatchForNulls = true)
    {
        return ToExpression<T>(r, useTryCatchForNulls).Compile();
    }

    public static Expression<Func<object, bool>> ToExpression(Type type, Rule r)
    {
        var paramUser = Expression.Parameter(typeof(object));
        Expression expr = GetExpressionForRule(type, r, paramUser);

        return Expression.Lambda<Func<object, bool>>(expr, paramUser);
    }

    public static Func<object, bool> ToFunc(Type type, Rule r)
    {
        return ToExpression(type, r).Compile();
    }

    public Func<object, bool> CompileRule(Type type, Rule r)
    {
        var paramUser = Expression.Parameter(typeof(object));
        Expression expr = GetExpressionForRule(type, r, paramUser);

        return Expression.Lambda<Func<object, bool>>(expr, paramUser).Compile();
    }

    public Func<T, bool> CompileRules<T>(IEnumerable<Rule> rules)
    {
        var paramUser = Expression.Parameter(typeof(T));
        var expr = BuildNestedExpression(typeof(T), rules, paramUser, ExpressionType.And);
        return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
    }

    public Func<object, bool> CompileRules(Type type, IEnumerable<Rule> rules)
    {
        var paramUser = Expression.Parameter(type);
        var expr = BuildNestedExpression(type, rules, paramUser, ExpressionType.And);
        return Expression.Lambda<Func<object, bool>>(expr, paramUser).Compile();
    }

    protected static Expression GetExpressionForRule(Type type, Rule rule, ParameterExpression parameterExpression, bool useTryCatchForNulls = false)
    {
        ExpressionType nestedOperator;

        if (ExpressionType.TryParse(rule.Operator, out nestedOperator) && nestedOperators.Contains(nestedOperator) && rule.Rules != null && rule.Rules.Any())
        {
            if (rule.Negate)
            {
                return Expression.Not(BuildNestedExpression(type, rule.Rules, parameterExpression, nestedOperator, useTryCatchForNulls));
            }
            else
            {
                return BuildNestedExpression(type, rule.Rules, parameterExpression, nestedOperator, useTryCatchForNulls);
            }
        }
        else
        {
            if (rule.Negate)
            {
                return Expression.Not(BuildRuleExpression(type, rule, parameterExpression, useTryCatchForNulls));
            }
            else
            {
                return BuildRuleExpression(type, rule, parameterExpression, useTryCatchForNulls);
            }
        }
    }

    protected static Expression BuildNestedExpression(Type type, IEnumerable<Rule> rules, ParameterExpression param, ExpressionType defaultOperation, bool useTryCatchForNulls = true)
    {
        var expressions = rules.Where(r => r.Enabled).Select(r => new KeyValuePair<Rule, Expression>(r, GetExpressionForRule(type, r, param, useTryCatchForNulls)));

        return BinaryExpression(expressions, defaultOperation);
    }

    protected static Expression BinaryExpression(IEnumerable<KeyValuePair<Rule, Expression>> ruleExpressionPairs, ExpressionType defaultOperationType)
    {
        Func<Expression, Expression, Expression> defaultBinaryExpression;
        var ruleExpressionPairsList = ruleExpressionPairs.ToList();
        var firstExpression = ruleExpressionPairsList.First().Value;

        switch (defaultOperationType)
        {
            case ExpressionType.Or:
                defaultBinaryExpression = Expression.Or;
                break;
            case ExpressionType.OrElse:
                defaultBinaryExpression = Expression.OrElse;
                break;
            case ExpressionType.AndAlso:
                defaultBinaryExpression = Expression.AndAlso;
                break;
            default:
            case ExpressionType.And:
                defaultBinaryExpression = Expression.And;
                break;
        }

        return ruleExpressionPairsList.Skip(1).Aggregate(firstExpression, (a, p) => p.Key.OuterOperator == null ? defaultBinaryExpression(a, p.Value.NullableToDefault()) : p.Key.OuterOperator.ToBinaryExpression(a, p.Value.NullableToDefault()));
    }

    public static Expression GetProperty(Expression param, string propname)
    {
        Expression propExpression = param;
        String[] childProperties = propname.Split('.');
        var propertyType = param.Type;

        foreach (var childprop in childProperties)
        {
            var isIndexed = _regexIndexed.Match(childprop);
            if (isIndexed.Success)
            {
                var indexType = typeof(int);
                var collectionname = isIndexed.Groups["Collection"].Value;
                var collectionProp = propertyType.GetProperty(collectionname);

                if (collectionProp == null)
                {
                    throw new RulesException($"Cannot find collection property {collectionname} in class {propertyType.Name} (\"{propname}\")");
                }

                var collexpr = Expression.PropertyOrField(propExpression, collectionname);
                Expression expIndex;

                if (isIndexed.Groups["Index"].Success)
                {
                    var index = Int32.Parse(isIndexed.Groups["Index"].Value);
                    expIndex = Expression.Constant(index);
                }
                else
                {
                    expIndex = Expression.Constant(isIndexed.Groups["Key"].Value);
                    indexType = typeof(string);
                }

                var collectionType = collexpr.Type;
                if (collectionType.IsArray)
                {
                    propExpression = Expression.ArrayAccess(collexpr, expIndex);
                    propertyType = propExpression.Type;
                }
                else
                {
                    var getter = collectionType.GetMethod("get_Item", new Type[] { indexType });

                    if (getter == null)
                    {
                        throw new RulesException($"'{collectionname} ({collectionType.Name}) cannot be indexed");
                    }
                    
                    propExpression = Expression.Call(collexpr, getter, expIndex);
                    propertyType = getter.ReturnType;
                }
            }
            else
            {
                var property = propertyType.GetProperty(childprop);

                if (property == null)
                {
                    throw new RulesException($"Cannot find property {childprop} in class {propertyType.Name} (\"{propname}\")");
                }

                propExpression = Expression.PropertyOrField(propExpression, childprop);
                propertyType = property.PropertyType;
            }
        }

        return propExpression;
    }

    public static Expression GetMethod(Expression param, string methodName, params object[] inputArguments)
    {
        Expression methodExpression = param;
        var methodType = param.Type;
        var isIndexed = _regexIndexed.Match(methodName);
        string? genericArgument = null;
        Type? genericArgumentType = null;

        if (methodName.Contains("<"))
        {
            var match = methodName.RegexGetMatch(@"(?<methodName>.*?)\<(?<genericArgument>.*?)\>$");

            methodName = match.GetGroupValue("methodName");
            genericArgument = match.GetGroupValue("genericArgument");

            if (genericArgument.Contains("<"))
            {
                string typeName;
                string innerGenericArg;

                match = genericArgument.RegexGetMatch(@"(?<typeName>.*?)\<(?<genericArgument>.*?)\>$");

                typeName = match.GetGroupValue("typeName");
                innerGenericArg = match.GetGroupValue("genericArgument");

                if (typeName == "System.Collections.Generic.ICollection")
                {
                    genericArgumentType = typeof(ICollection<>);
                    genericArgumentType = genericArgumentType.MakeGenericType(Type.GetType(TypeExtensions.GetPrimitiveTypeFullName(innerGenericArg)));
                }
            }
            else
            {
                genericArgumentType = Type.GetType(TypeExtensions.GetPrimitiveTypeFullName(genericArgument));
            }
        }

        if (isIndexed.Success)
        {
            var indexType = typeof(int);
            var collectionname = isIndexed.Groups["Collection"].Value;
            var collectionProp = methodType.GetProperty(collectionname);

            if (collectionProp == null)
            {
                throw new RulesException($"Cannot find collection method {collectionname} in class {methodType.Name} (\"{methodName}\")");
            }

            var collexpr = Expression.PropertyOrField(methodExpression, collectionname);
            Expression expIndex;

            if (isIndexed.Groups["Index"].Success)
            {
                var index = Int32.Parse(isIndexed.Groups["Index"].Value);
                expIndex = Expression.Constant(index);
            }
            else
            {
                expIndex = Expression.Constant(isIndexed.Groups["Key"].Value);
                indexType = typeof(string);
            }

            var collectionType = collexpr.Type;

            if (collectionType.IsArray)
            {
                methodExpression = Expression.ArrayAccess(collexpr, expIndex);
                methodType = methodExpression.Type;
            }
            else
            {
                var getter = collectionType.GetMethod("get_Item", new Type[] { indexType });

                if (getter == null)
                {
                    throw new RulesException($"'{collectionname} ({collectionType.Name}) cannot be indexed");
                }

                methodExpression = Expression.Call(collexpr, getter, expIndex);
                methodType = getter.ReturnType;
            }
        }
        else
        {
            var multipleMethods = methodType.GetMethods().Where(m => m.Name == methodName).ToList();
            MethodInfo? method = null;

            if (multipleMethods.Count > 1)
            {
                if (inputArguments.Length > 0)
                {
                    if (inputArguments[0] is Expression innerMember)
                    {
                        var type = innerMember.Type;

                        if (inputArguments.Length > 1)
                        {
                            var types = new List<Type> { type };

                            foreach (var arg in inputArguments.Skip(1))
                            {
                                if (((string)arg).RegexIsMatch(@"^\d+$"))
                                {
                                    types.Add(typeof(int));
                                }
                                else
                                {
                                    types.Add(typeof(string));
                                }
                            }

                            method = methodType.GetMethod(methodName, types.ToArray());
                        }
                        else
                        {
                            method = methodType.GetMethod(methodName, [type]);
                        }
                    }
                    else if (inputArguments[1] is Expression innerMember1)
                    {
                        var type = typeof(string);

                        if (inputArguments.Length > 1)
                        {
                            var types = new List<Type> { type };

                            foreach (var arg in inputArguments.Skip(1))
                            {
                                if (arg is Expression expression)
                                {
                                    types.Add(expression.Type);
                                }
                                else if (((string)arg).RegexIsMatch(@"^\d+$"))
                                {
                                    types.Add(typeof(int));
                                }
                                else
                                {
                                    types.Add(typeof(string));
                                }
                            }

                            method = methodType.GetMethod(methodName, types.ToArray());
                        }
                        else
                        {
                            method = methodType.GetMethod(methodName, [type]);
                        }
                    }

                    if (method == null)
                    {
                        var handleMethodAmbiguityEventArgs = new HandleMethodAmbiguityEventArgs(methodName, inputArguments);

                        HandleMethodAmbiguityEvent(typeof(MicroRulesEngine), handleMethodAmbiguityEventArgs);

                        if (handleMethodAmbiguityEventArgs.RearrangedArgs)
                        {
                            return GetMethod(param, methodName, handleMethodAmbiguityEventArgs.InputArguments);
                        }
                        else
                        {
                            DebugUtils.Break();
                        }
                    }
                }
                else
                {
                    DebugUtils.Break();
                }
            }
            else
            {
                method = methodType.GetMethod(methodName);
            }

            if (method == null)
            {
                throw new RulesException($"Cannot find method {methodName} in class {methodType.Name} (\"{methodName}\")");
            }

            if (genericArgumentType != null)
            {
                method = method.MakeGenericMethod(genericArgumentType);
            }

            if (inputArguments != null)
            {
                if (inputArguments.Length == 0)
                {
                    methodExpression = Expression.Call(param, method);
                }
                else if (inputArguments.Length == 1)
                {
                    var inputArgument1 = inputArguments[0];

                    if (inputArgument1 == null)
                    {
                        methodExpression = Expression.Call(param, method);
                    }
                    else
                    {
                        var parmInfo1 = method.GetParameters()[0];

                        if (parmInfo1.ParameterType != typeof(string) && !parmInfo1.ParameterType.IsGenericCollection())
                        {
                            object? value = null;

                            value = Convert.ChangeType(inputArgument1, parmInfo1.ParameterType);

                            inputArgument1 = value;
                        }

                        if (inputArgument1 is Expression innerExpression)
                        {
                            methodExpression = Expression.Call(param, method, innerExpression);
                        }
                        else
                        {
                            methodExpression = Expression.Call(param, method, Expression.Constant(inputArgument1));
                        }
                    }
                }
                else if (inputArguments.Length == 2)
                {
                    var inputArgument1 = inputArguments[0];
                    var inputArgument2 = inputArguments[1];

                    if (inputArgument2 is Expression expression2)
                    {
                        var parmInfo1 = method.GetParameters()[0];

                        if (parmInfo1.ParameterType != typeof(string))
                        {
                            object value = null;

                            value = Convert.ChangeType(inputArgument1, parmInfo1.ParameterType);

                            inputArgument1 = value;
                        }

                        methodExpression = Expression.Call(param, method, Expression.Constant(inputArgument1), expression2);
                    }
                    else if (inputArgument1 is Expression expression1)
                    {
                        var parmInfo2 = method.GetParameters()[0];

                        if (parmInfo2.ParameterType != typeof(string) && !parmInfo2.ParameterType.IsEnumerableGeneric())
                        {
                            object value = null;

                            value = Convert.ChangeType(inputArgument2, parmInfo2.ParameterType);

                            inputArgument1 = value;
                        }

                        methodExpression = Expression.Call(param, method, expression1, Expression.Constant(inputArgument2));
                    }
                    else
                    {
                        methodExpression = Expression.Call(param, method, Expression.Constant(inputArgument1), Expression.Constant(inputArgument2));
                    }
                }
                else if (inputArguments.Length == 3)
                {
                    var inputArgument1 = inputArguments[0];
                    var inputArgument2 = inputArguments[1];
                    var inputArgument3 = inputArguments[2];

                    if (inputArgument1 is Expression expression)
                    {
                        var parmInfo2 = method.GetParameters()[1];
                        var parmInfo3 = method.GetParameters()[2];

                        if (parmInfo2.ParameterType != typeof(string))
                        {
                            object value = null;

                            value = Convert.ChangeType(inputArgument2, parmInfo2.ParameterType);

                            inputArgument2 = value;
                        }

                        if (parmInfo3.ParameterType != typeof(string))
                        {
                            object value = null;

                            value = Convert.ChangeType(inputArgument3, parmInfo3.ParameterType);

                            inputArgument3 = value;
                        }

                        methodExpression = Expression.Call(param, method, expression, Expression.Constant(inputArgument2), Expression.Constant(inputArgument3));
                    }
                    else
                    {
                        DebugUtils.Break();
                    }
                }
                else
                {
                    DebugUtils.Break();
                }
            }
            else
            {
                methodExpression = Expression.Call(param, method);
            }

            methodType = method.ReturnType;
        }

        return methodExpression;
    }

    public static List<RuleMemberInfo> GetMethodInfo<T>(string methodName)
    {
        var methodInfos = new List<RuleMemberInfo>();
        var methodType = typeof(T);
        List<MethodInfo> methods;

        if (methodName.Contains("<"))
        {
            methodName = methodName.LeftUpToIndexOf('<');
        }

        methods = methodType.GetMethods().Where(m => m.Name == methodName).ToList();

        foreach (var method in methods)
        {
            var methodInfo = new RuleMemberInfo(method);

            methodInfos.Add(methodInfo);
        }

        return methodInfos;
    }

    public static RuleMemberInfo GetPropertyInfo<T>(string propertyName)
    {
        var propertyInfo = new List<RuleMemberInfo>();
        var memberType = typeof(T);
        var property = memberType.GetProperty(propertyName);

        return new RuleMemberInfo(property);
    }

    public static RuleMemberInfo GetPropertyInfo(Type memberType, string propertyName)
    {
        var propertyInfo = new List<RuleMemberInfo>();
        var property = memberType.GetProperty(propertyName);

        return new RuleMemberInfo(property);
    }

    private static MethodInfo? IsEnumerableOperator(string oprator)
    {
        return (from tup in enumerationMethodsByName where string.Equals(oprator, tup.Item1, StringComparison.CurrentCultureIgnoreCase) select tup.Item2.Value).FirstOrDefault();
    }

    private static Expression BuildRuleExpression(Type type, Rule rule, Expression param, bool useTryCatch = true)
    {
        Expression propOrMethodExpression;
        Type propOrMethodType;
        var id = rule.Id;

        if (!useTryCatch && !warnedAboutTryCatch)
        {
            using (ConsoleColorizer.UseColor(ConsoleColor.DarkRed))
            {
                Console.WriteLine("******************************** WARNING!!! NOT USING TRY CATCH ********************************");
            }

            warnedAboutTryCatch = true;
        }

        if (MicroRulesEngine.VerboseOutput)
        {
            Console.WriteLine($"Compiling rule: {id}. {RulesCompiled + 1} of {RulesCount} rules");

            RulesCompiled++;
        }
        else if (!providedBuildStartMessage)
        {
            Console.WriteLine($"Compiling {RulesCount} rules");
            providedBuildStartMessage = true;
        }

        if (MicroRulesEngine.BreakOnIds != null)
        {
            if (id.IsOneOf(MicroRulesEngine.BreakOnIds))
            {
                if (MicroRulesEngine.HitCount == 1)
                {
                }
                else if (MicroRulesEngine.IdHitCounts.ContainsKey(id))
                {
                    int count;

                    MicroRulesEngine.IdHitCounts[id]++;

                    count = MicroRulesEngine.IdHitCounts[id];

                    if (count == MicroRulesEngine.HitCount)
                    {
                    }
                }
                else
                {
                    MicroRulesEngine.IdHitCounts.Add(id, 1);
                }
            }
        }

        if (param.Type == typeof(object))
        {
            param = Expression.TypeAs(param, type);
        }

        ExpressionType tUnaryOrBinary;
        MethodCallExpression testMethodExpression;

        if (ExpressionType.TryParse(rule.Operator, out tUnaryOrBinary))
        {
            return HandleUnaryBinary(rule, param, tUnaryOrBinary);
        }

        if (string.IsNullOrEmpty(rule.MemberName)) //check is against the object itself
        {
            if (string.IsNullOrEmpty(rule.InputMethod))
            {
                propOrMethodExpression = param;
                propOrMethodType = propOrMethodExpression.Type;
            }
            else
            {
                propOrMethodExpression = GetMethod(param, rule.InputMethod, rule.InputArgument);
                propOrMethodType = propOrMethodExpression.Type;
            }
        }
        else
        {
            propOrMethodExpression = GetProperty(param, rule.MemberName);
            propOrMethodType = propOrMethodExpression.Type;
        }

        if (useTryCatch)
        {
            propOrMethodExpression = Expression.TryCatch(
                Expression.Block(propOrMethodExpression.Type, propOrMethodExpression),
                Expression.Catch(typeof(NullReferenceException), Expression.Default(propOrMethodExpression.Type)));
        }

        switch (rule.Operator)
        {
            case "Contains":
                {
                    if (propOrMethodType.IsDictionaryGeneric())
                    {
                        Lazy<MethodInfo> miCollectionContains = new Lazy<MethodInfo>(() =>
                        {
                            var type = typeof(EnumerableExtensions);
                            var method = type.GetMethods().Single(m => m.Name == "Contains" && m.GetParameters()[0].ParameterType.Name == "IDictionary`2");

                            method = method.MakeGenericMethod(propOrMethodType.GetGenericArguments()[0]);

                            return method;
                        });

                        testMethodExpression = Expression.Call(miCollectionContains.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue));
                    }
                    else if (propOrMethodType != typeof(string) && propOrMethodType.IsEnumerableGeneric())
                    {
                        Lazy<MethodInfo> miCollectionContains = new Lazy<MethodInfo>(() =>
                        {
                            var type = typeof(System.Linq.Enumerable);
                            var method = type.GetMethods().Single(m => m.Name == "Contains" && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.Name == "IEnumerable`1");

                            method = method.MakeGenericMethod(typeof(string));

                            return method;
                        });

                        testMethodExpression = Expression.Call(miCollectionContains.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue));
                    }
                    else
                    {
                        testMethodExpression = Expression.Call(
                        propOrMethodExpression,
                        miStringContains.Value,
                        Expression.Constant(rule.TargetValue, typeof(string)));
                    }
                    
                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "EndsWith":
                {
                    testMethodExpression = Expression.Call(
                        propOrMethodExpression,
                        miEndsWith.Value,
                        Expression.Constant(rule.TargetValue, typeof(string)));

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "Equals":
                {
                    var objectType = typeof(object);
                    object value = null;

                    if (propOrMethodType == typeof(bool) && rule.TargetValue is string)
                    {
                        value = Convert.ChangeType(((string) rule.TargetValue).ToLower(), propOrMethodType);
                    }
                    else if (propOrMethodType.IsNullable())
                    {
                        var innerType = propOrMethodType.GetAnyInnerType();

                        value = Convert.ChangeType(((string)rule.TargetValue).ToLower(), innerType);
                    }
                    else
                    {
                        value = Convert.ChangeType(rule.TargetValue, propOrMethodType);
                    }

                    testMethodExpression = Expression.Call(
                        Expression.Convert(Expression.Constant(value), objectType),
                        miObjectEquals.Value,
                        Expression.Convert(propOrMethodExpression, objectType));

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "IsMatch":
                {
                    if (propOrMethodExpression.Type == typeof(KeyValuePair<string, string>))
                    {
                        testMethodExpression = Expression.Call(
                            miRegexKeyValuePairHasMatch.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue, typeof(string)),
                            Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions)));

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                    }
                    else if (propOrMethodExpression.Type == typeof(Dictionary<string, string>))
                    {
                        testMethodExpression = Expression.Call(
                            miRegexIDictionaryHasMatch.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue, typeof(string)),
                            Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions)));

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                    }
                    else if (propOrMethodExpression.Type == typeof(IEnumerable<string>))
                    {
                        testMethodExpression = Expression.Call(
                            miRegexIEnumerableHasMatch.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue, typeof(string)),
                            Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions)));

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var matchExpression = Expression.Call(
                            miRegexMatch.Value,
                            propOrMethodExpression,
                            Expression.Constant(rule.TargetValue, typeof(string)),
                            Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions)));

                        return Expression.Call(miAddRuleToStackWithMatchAndReturnResult.Value, Expression.Constant(rule), param, matchExpression.MakeDelegate(param));
                    }
                }
            case "IsInteger":
                {
                    testMethodExpression = Expression.Call(
                        miIntTryParse.Value,
                        propOrMethodExpression,
                        Expression.MakeMemberAccess(null, typeof(Placeholder).GetField("Int"))
                    );

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "IsSingle":
                {
                    testMethodExpression = Expression.Call(
                        miFloatTryParse.Value,
                        propOrMethodExpression,
                        Expression.MakeMemberAccess(null, typeof(Placeholder).GetField("Float"))
                    );

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "IsDouble":
                {
                    testMethodExpression = Expression.Call(
                        miDoubleTryParse.Value,
                        propOrMethodExpression,
                        Expression.MakeMemberAccess(null, typeof(Placeholder).GetField("Double"))
                    );

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "IsDecimal":
                {
                    testMethodExpression = Expression.Call(
                        miDecimalTryParse.Value,
                        propOrMethodExpression,
                        Expression.MakeMemberAccess(null, typeof(Placeholder).GetField("Decimal"))
                    );

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            case "IsInInput":
                {
                    if (rule.Inputs != null)
                    {
                        testMethodExpression = Expression.Call(Expression.Constant(rule.Inputs.ToList()),
                            miListContains.Value,
                            propOrMethodExpression);
                    }
                    else if (rule.InputMethod2 != null)
                    {
                        if (propOrMethodType.IsCollectionType())
                        {
                            var inputMethodExpression = GetMethod(param, rule.InputMethod2);
                            Lazy<MethodInfo> miCollectionContains = new Lazy<MethodInfo>(() =>
                            {
                                var type = typeof(EnumerableExtensions);
                                var method = type.GetMethods().Single(m => m.Name == "ContainsAny" && m.GetParameters()[0].ParameterType.Name == "ICollection`1");

                                method = method.MakeGenericMethod(propOrMethodType.GetGenericArguments()[0]);

                                return method;
                            });

                            testMethodExpression = Expression.Call(miCollectionContains.Value,
                                inputMethodExpression,
                                propOrMethodExpression);
                        }
                        else
                        {
                            var inputMethodExpression = GetMethod(param, rule.InputMethod2);
                            Lazy<MethodInfo> miCollectionContains = new Lazy<MethodInfo>(() =>
                            {
                                var type = typeof(ICollection<>).MakeGenericType(propOrMethodExpression.Type);
                                var method = type.GetMethods().Single(m => m.Name == "Contains");

                                return method;
                            });

                            testMethodExpression = Expression.Call(inputMethodExpression,
                                miCollectionContains.Value,
                                propOrMethodExpression);
                        }
                    }
                    else if (rule.InputMethod != null)
                    {
                        var inputMethodExpression = GetMethod(param, rule.InputMethod, rule.InputArgument);
                        Lazy<MethodInfo> miCollectionContains = new Lazy<MethodInfo>(() =>
                        {
                            var type = typeof(ICollection<>).MakeGenericType(propOrMethodExpression.Type);
                            var method = type.GetMethods().Single(m => m.Name == "Contains");

                            return method;
                        });

                        testMethodExpression = Expression.Call(inputMethodExpression,
                            miCollectionContains.Value,
                            propOrMethodExpression);
                    }
                    else
                    {
                        testMethodExpression = null;
                        DebugUtils.Break();
                    }

                    return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testMethodExpression.MakeDelegate(param));
                }
            default:
                break;
        }

        var enumerableOperation = IsEnumerableOperator(rule.Operator);

        if (enumerableOperation != null)
        {
            var elementType = ElementType(propOrMethodType);
            var lambdaParam = Expression.Parameter(elementType, "lambdaParam");
            testMethodExpression = rule.Rules?.Any() == true
                ? Expression.Call(enumerableOperation.MakeGenericMethod(elementType),
                    propOrMethodExpression,
                    Expression.Lambda(
                        BuildNestedExpression(elementType, rule.Rules, lambdaParam, ExpressionType.AndAlso),
                        lambdaParam)


                )
                : Expression.Call(enumerableOperation.MakeGenericMethod(elementType), propOrMethodExpression);

            return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), testMethodExpression);
        }
        else //Invoke a method on the Property
        {
            var inputs = rule.Inputs.Select(x => x.GetType()).ToArray();
            var methodInfo = propOrMethodType.GetMethod(rule.Operator, inputs);
            List<Expression> expressions = new List<Expression>();

            if (methodInfo == null)
            {
                methodInfo = propOrMethodType.GetMethod(rule.Operator);
                if (methodInfo != null)
                {
                    var parameters = methodInfo.GetParameters();
                    int i = 0;
                    foreach (var item in rule.Inputs)
                    {
                        expressions.Add(MicroRulesEngine.StringToExpression(item, parameters[i].ParameterType));
                        i++;
                    }
                }
            }
            else
            {
                expressions.AddRange(rule.Inputs.Select(Expression.Constant));
            }

            if (methodInfo == null)
            {
                throw new RulesException($"'{rule.Operator}' is not a method of '{propOrMethodType.Name}");
            }

            if (!methodInfo.IsGenericMethod)
            {
                inputs = null; //Only pass in type information to a Generic Method
            }

            var callExpression = Expression.Call(propOrMethodExpression, rule.Operator, inputs, expressions.ToArray());
            TryExpression tryExpression;

            if (useTryCatch)
            {
                tryExpression = Expression.TryCatch(
                Expression.Block(typeof(bool), callExpression),
                Expression.Catch(typeof(NullReferenceException), Expression.Constant(false)));

                return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), tryExpression);
            }
            else
            {
                return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), callExpression);
            }
        }
    }

    public static Expression HandleResolution(Expression expression, MismatchResolution resolution)
    {
        var values = EnumUtils.GetValues<MismatchResolution>();

        foreach (var value in values.Where(v => v != MismatchResolution.NotSet))
        {
            if (resolution.HasFlag(value))
            {
                switch (value)
                {
                    case MismatchResolution.ParseToInt:
                        expression = Expression.Call(miIntParse.Value, expression);
                        break;
                    case MismatchResolution.GetLenth:
                        expression = Expression.Property(expression, "Length");
                        break;
                    case MismatchResolution.CastToLong:
                        expression = Expression.Convert(expression, typeof(long));
                        break;
                    default:
                        DebugUtils.Break();
                        break;
                }
            }
        }

        return expression;
    }

    public static Expression HandleUnaryBinary(Rule rule, Expression param, ExpressionType unaryOrBinaryExpressionType)
    {
        var isBinary = false;
        var propertyPresence = rule.GetPropertyPresence();
        var id = rule.Id;
        var @operator = rule.Operator;
        Expression left;
        Expression right;
        Expression unaryInput;
        BinaryExpression testBinaryExpression;
        UnaryExpression testUnaryExpression;

        switch (unaryOrBinaryExpressionType)
        {
            case ExpressionType.IsFalse:
            case ExpressionType.IsTrue:
                isBinary = false;
                break;
            default:
                isBinary = true;
                break;
        }

        switch (propertyPresence)
        {
            case RulePropertyPresence.MemberName_InputMethod:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();

                    if (isBinary)
                    {
                        left = GetProperty(param, memberName);
                        right = GetMethod(param, inputMethod);
                        
                        left = left.NullableToDefault();
                        right = right.NullableToDefault();

                        if (right.Type != left.Type)
                        {
                            MismatchResolution leftResolution;
                            MismatchResolution rightResolution;
                            var typeMismatchEventArgs = new TypeMismatchEventArgs(rule, right, left);

                            TypeMismatchEvent(typeof(MicroRulesEngine), typeMismatchEventArgs);

                            leftResolution = typeMismatchEventArgs.LeftResolution;
                            rightResolution = typeMismatchEventArgs.RightResolution;

                            if (MismatchResolution.NotSet.IsAllOf(leftResolution, rightResolution))
                            {
                                DebugUtils.Break();
                            }

                            if (leftResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(left, leftResolution);
                            }
                            
                            if (rightResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(right, rightResolution);
                            }
                        }

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var property = GetProperty(param, memberName);

                        unaryInput = GetMethod(param, inputMethod, property);
                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, unaryInput, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
            case RulePropertyPresence.InputMethod_TargetValue:
                {
                    var inputMethod = (string) rule.InputMethod.Assure();

                    if (isBinary)
                    {
                        left = GetMethod(param, inputMethod);
                        
                        if (!rule.HandleTargetValue(param, out right))
                        {
                            DebugUtils.Break();
                        }

                        left = left.NullableToDefault();
                        right = right.NullableToDefault();

                        if (right.Type != left.Type)
                        {
                            MismatchResolution leftResolution;
                            MismatchResolution rightResolution;
                            var typeMismatchEventArgs = new TypeMismatchEventArgs(rule, right, left);

                            TypeMismatchEvent(typeof(MicroRulesEngine), typeMismatchEventArgs);

                            leftResolution = typeMismatchEventArgs.LeftResolution;
                            rightResolution = typeMismatchEventArgs.RightResolution;

                            if (MismatchResolution.NotSet.IsAllOf(leftResolution, rightResolution))
                            {
                                DebugUtils.Break();
                            }

                            if (leftResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(left, leftResolution);
                            }

                            if (rightResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(right, rightResolution);
                            }
                        }

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_InputArgument:
                {
                    var memberName = (string) rule.MemberName.Assure();
                    var inputMethod = (string) rule.InputMethod.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var property = GetProperty(param, memberName);

                        unaryInput = GetMethod(param, inputMethod, inputArgument, property);
                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, unaryInput, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_TargetValue:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var targetValue = rule.TargetValue.Assure();

                    if (isBinary)
                    {
                        left = GetProperty(param, memberName);

                        if (!rule.HandleTargetValue(param, out right))
                        {
                            DebugUtils.Break();
                        }

                        if (right.Type != left.Type)
                        {
                            var typeMismatchEventArgs = new TypeMismatchEventArgs(rule, right, left);
                            MismatchResolution leftResolution;
                            MismatchResolution rightResolution;

                            TypeMismatchEvent(typeof(MicroRulesEngine), typeMismatchEventArgs);

                            leftResolution = typeMismatchEventArgs.LeftResolution;
                            rightResolution = typeMismatchEventArgs.RightResolution;

                            if (MismatchResolution.NotSet.IsAllOf(leftResolution, rightResolution))
                            {
                                DebugUtils.Break();
                            }

                            if (leftResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(left, leftResolution);
                            }

                            if (rightResolution != MismatchResolution.NotSet)
                            {
                                left = HandleResolution(right, rightResolution);
                            }
                        }

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));                    }
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_TargetValue:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.InputMethod_InputMethod2_TargetValue:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();

                    DebugUtils.Break();
                }
                break;
            case RulePropertyPresence.InputMethod_TargetValue_InputArgument:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    if (isBinary)
                    {
                        left = GetMethod(param, inputMethod, inputArgument);

                        if (!rule.HandleTargetValue(param, out right))
                        {
                            DebugUtils.Break();
                        }

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_TargetValue_InputArgument:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_InputMethod2_TargetValue_InputArgument:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    DebugUtils.Break();
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_InputMethod2_InputArgument_InputArgument2:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var property = GetProperty(param, memberName);
                        var innerMethod = GetMethod(param, inputMethod, inputArgument);

                        unaryInput = GetMethod(param, inputMethod2, inputArgument2, innerMethod);
                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, unaryInput, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_TargetValue_InputArgument_InputArgument2:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.InputMethod_InputMethod2_TargetValue_InputArgument:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        //var property = GetProperty(param, memberName);
                        //unaryInput = GetMethod(param, inputMethod);
                        //testUnaryExpression = Expression.MakeUnary(tUnaryOrBinary, unaryInput, null!);

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.InputMethod_InputMethod2_InputArgument_InputArgument2:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    if (isBinary)
                    {
                        left = GetMethod(param, inputMethod, inputArgument);
                        right = GetMethod(param, inputMethod2, inputArgument2);

                        switch (unaryOrBinaryExpressionType)
                        {
                            case ExpressionType.LessThan:
                            case ExpressionType.LessThanOrEqual:
                            case ExpressionType.GreaterThan:
                            case ExpressionType.GreaterThanOrEqual:
                                {
                                    if (left.Type == typeof(string) && right.Type == typeof(string))
                                    {
                                        left = Expression.Call(miIntParse.Value, left);
                                        right  = Expression.Call(miIntParse.Value, right);
                                    }
                                    else
                                    {
                                        DebugUtils.Break();
                                    }
                                }
                                break;
                        }

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var innerMethod = GetMethod(param, inputMethod, inputArgument);
                        var outerMethod = GetMethod(param, inputMethod2, inputArgument2, innerMethod);

                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, outerMethod, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_InputArgument_InputArgument2:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    if (isBinary)
                    {
                        //left = GetMethod(param, inputMethod, inputArgument);

                        //if (!rule.HandleTargetValue(param, out right))
                        //{
                        //    DebugUtils.Break();
                        //}

                        //testBinaryExpression = Expression.MakeBinary(tUnaryOrBinary, left.NullableToDefault(), right.NullableToDefault());

                        //return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var memberInput = GetProperty(param, memberName);
                        
                        unaryInput = GetMethod(param, inputMethod, memberInput, inputArgument, inputArgument2);

                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, unaryInput, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            case RulePropertyPresence.MemberName_InputMethod_InputMethod2_TargetValue_InputArgument_InputArgument2:
                {
                    var memberName = (string)rule.MemberName.Assure();
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    DebugUtils.Break();
                }
                break;
            case RulePropertyPresence.InputMethod_InputMethod2_TargetValue_InputArgument_InputArgument2:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var targetValue = rule.TargetValue.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();
                    var inputArgument2 = rule.InputArgument2.Assure();

                    DebugUtils.Break();
                }
                break;
            case RulePropertyPresence.InputMethod_InputMethod2_InputArgument:
                {
                    var inputMethod = (string)rule.InputMethod.Assure();
                    var inputMethod2 = (string)rule.InputMethod2.Assure();
                    var inputArgument = rule.InputArgument.Assure();

                    if (isBinary)
                    {
                        left = GetMethod(param, inputMethod, inputArgument);
                        right = GetMethod(param, inputMethod2);

                        testBinaryExpression = Expression.MakeBinary(unaryOrBinaryExpressionType, left.NullableToDefault(), right.NullableToDefault());

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testBinaryExpression.MakeDelegate(param));
                    }
                    else
                    {
                        var innerMethod = GetMethod(param, inputMethod, inputArgument);

                        unaryInput = GetMethod(param, inputMethod2, innerMethod);
                        testUnaryExpression = Expression.MakeUnary(unaryOrBinaryExpressionType, unaryInput, null!);

                        return Expression.Call(miAddRuleToStackAndReturnResult.Value, Expression.Constant(rule), param, testUnaryExpression.MakeDelegate(param));
                    }
                }
                break;
            default:
                DebugUtils.Break();
                break;
        }

        return null!;
    }

    private static MethodInfo GetLinqMethod(string name, int numParameter)
    {
        return typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m => m.Name == name && m.GetParameters().Length == numParameter)!;
    }

    private static Expression StringToExpression(object value, Type propType)
    {
        Debug.Assert(propType != null);

        object safevalue;
        Type valuetype = propType;
        var txt = value as string;

        if (value == null)
        {
            safevalue = null;
        }
        else if (txt != null)
        {
            if (txt.ToLower() == "null")
            {
                safevalue = null;
            }
            else if (propType.IsEnum)
            {
                safevalue = Enum.Parse(propType, txt);
            }
            else
            {
                if (propType.Name == "Nullable`1")
                {
                    valuetype = Nullable.GetUnderlyingType(propType);
                }

                safevalue = IsTime(txt, propType) ?? Convert.ChangeType(value, valuetype);
            }
        }
        else
        {
            if (propType.Name == "Nullable`1")
            {
                valuetype = Nullable.GetUnderlyingType(propType);
            }

            safevalue = Convert.ChangeType(value, valuetype);
        }

        return Expression.Constant(safevalue, propType);
    }

    private static readonly Regex reNow = new Regex(@"#NOW([-+])(\d+)([SMHDY])", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    public static List<string>? BreakOnIds { get; set; }
    public static bool VerboseOutput { get; set; }
    public static int HitCount { get; set; }
    public static Dictionary<string, int> IdHitCounts { get; set; }

    public static int RulesCount
    {

        get
        {
            return ruleStatsThreadLocal.Value.RulesCount;
        }

        set
        {
            ruleStatsThreadLocal.Value.RulesCount = value;
        }
    }

    public static int RulesCompiled
    {

        get
        {
            return ruleStatsThreadLocal.Value.RulesCompiled;
        }

        set
        {
            ruleStatsThreadLocal.Value.RulesCompiled = value;
        }
    }
    
    public static int RulesExecuted
    {

        get
        {
            return ruleStatsThreadLocal.Value.RulesExecuted;
        }

        set
        {
            ruleStatsThreadLocal.Value.RulesExecuted = value;
        }
    }

    public MicroRulesEngine(bool verboseOutput = false, int count = 0, int hitCount = 1, List<string>? breakOnIds = null)
    {
        MicroRulesEngine.BreakOnIds = breakOnIds;
        MicroRulesEngine.VerboseOutput = verboseOutput;
        MicroRulesEngine.RulesCount = count;
        MicroRulesEngine.RulesExecuted = 0;
        MicroRulesEngine.RulesCompiled = 0;
        MicroRulesEngine.HitCount = hitCount;
        MicroRulesEngine.IdHitCounts = new Dictionary<string, int>();
    }

    private static DateTime? IsTime(string text, Type targetType)
    {
        if (targetType != typeof(DateTime) && targetType != typeof(DateTime?))
        {
            return null;
        }

        var match = reNow.Match(text);

        if (!match.Success)
        {
            return null;
        }

        var amt = Int32.Parse(match.Groups[2].Value);

        if (match.Groups[1].Value == "-")
        {
            amt = -amt;
        }

        switch (Char.ToUpperInvariant(match.Groups[3].Value[0]))
        {
            case 'S':
                return DateTime.Now.AddSeconds(amt);
            case 'M':
                return DateTime.Now.AddMinutes(amt);
            case 'H':
                return DateTime.Now.AddHours(amt);
            case 'D':
                return DateTime.Now.AddDays(amt);
            case 'Y':
                return DateTime.Now.AddYears(amt);
        }
        // it should not be possible to reach here.	
        throw new ArgumentException();
    }

    private static Type ElementType(Type seqType)
    {
        Type ienum = FindIEnumerable(seqType);

        if (ienum == null)
        {
            return seqType;
        }

        return ienum.GetGenericArguments()[0];
    }

    private static Type FindIEnumerable(Type seqType)
    {
        if (seqType == null || seqType == typeof(string))
        {
            return null;
        }

        if (seqType.IsArray)
        {
            return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
        }

        if (seqType.IsGenericType)
        {
            foreach (Type arg in seqType.GetGenericArguments())
            {
                Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                if (ienum.IsAssignableFrom(seqType))
                {
                    return ienum;
                }
            }
        }

        Type[] ifaces = seqType.GetInterfaces();

        foreach (Type iface in ifaces)
        {
            Type ienum = FindIEnumerable(iface);

            if (ienum != null)
            {
                return ienum;
            }
        }

        if (seqType.BaseType != null && seqType.BaseType != typeof(object))
        {
            return FindIEnumerable(seqType.BaseType);
        }

        return null;
    }
    public enum OperatorType
    {
        InternalString = 1,
        ObjectMethod = 2,
        Comparison = 3,
        Logic = 4
    }
    public class Operator
    {
        public string Name { get; set; }
        public OperatorType Type { get; set; }
        public int NumberOfInputs { get; set; }
        public bool SimpleInputs { get; set; }
    }
    public class Member
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Operator> PossibleOperators { get; set; }
        public static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(Enum),
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }
        public static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        public static List<Member> GetFields(Type type, string memberName = null, string parentPath = null)
        {
            List<Member> toReturn = new List<Member>();

            var fi = new Member
            {
                Name = string.IsNullOrEmpty(parentPath) ? memberName : $"{parentPath}.{memberName}",
                Type = type.ToString()
            };

            fi.PossibleOperators = Member.Operators(type, string.IsNullOrEmpty(fi.Name));
            toReturn.Add(fi);

            if (!Member.IsSimpleType(type))
            {
                var fields = type.GetFields(Member.flags);
                var properties = type.GetProperties(Member.flags);

                foreach (var field in fields)
                {
                    string useParentName = null;
                    var name = Member.ValidateName(field.Name, type, memberName, fi.Name, parentPath, out useParentName);
                    toReturn.AddRange(GetFields(field.FieldType, name, useParentName));
                }

                foreach (var prop in properties)
                {
                    string useParentName = null;
                    var name = Member.ValidateName(prop.Name, type, memberName, fi.Name, parentPath, out useParentName);
                    toReturn.AddRange(GetFields(prop.PropertyType, name, useParentName));
                }
            }
            return toReturn;
        }
        private static string ValidateName(string name, Type parentType, string parentName, string parentPath, string grandparentPath, out string useAsParentPath)
        {
            if (name == "Item" && IsGenericList(parentType))
            {
                useAsParentPath = grandparentPath;
                return parentName + "[0]";
            }
            else
            {
                useAsParentPath = parentPath;
                return name;
            }
        }

        public static bool IsGenericList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            foreach (Type @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        // if needed, you can also return the type used as generic argument
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<Operator> Operators(Type type, bool addLogicOperators = false, bool noOverloads = true)
        {
            List<Operator> operators = new List<Operator>();

            if (addLogicOperators)
            {
                operators.AddRange(logicOperators.Select(x => new Operator() { Name = x, Type = OperatorType.Logic }));
            }

            if (type == typeof(String))
            {
                operators.AddRange(hardCodedStringOperators.Select(x => new Operator() { Name = x, Type = OperatorType.InternalString }));
            }
            else if (Member.IsSimpleType(type))
            {
                operators.AddRange(comparisonOperators.Select(x => new Operator() { Name = x, Type = OperatorType.Comparison }));
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                if (method.ReturnType == typeof(Boolean) && !method.Name.StartsWith("get_") && !method.Name.StartsWith("set_") && !method.Name.StartsWith("_op"))
                {
                    var parameters = method.GetParameters();
                    var op = new Operator()
                    {
                        Name = method.Name,
                        Type = OperatorType.ObjectMethod,
                        NumberOfInputs = parameters.Length,
                        SimpleInputs = parameters.All(x => Member.IsSimpleType(x.ParameterType))
                    };

                    if (noOverloads)
                    {
                        var existing = operators.FirstOrDefault(x => x.Name == op.Name && x.Type == op.Type);

                        if (existing == null)
                        {
                            operators.Add(op);
                        }
                        else if (existing.NumberOfInputs > op.NumberOfInputs)
                        {
                            operators[operators.IndexOf(existing)] = op;
                        }
                    }
                    else
                    {
                        operators.Add(op);
                    }
                }
            }

            return operators;
        }
    }

}

internal static class Placeholder
{
    public static int Int = 0;
    public static float Float = 0.0f;
    public static double Double = 0.0;
    public static decimal Decimal = 0.0m;
}

// Nothing specific to MRE.  Can be moved to a more common location
public static class Extensions
{
    public static void AddRange<T>(this IList<T> collection, IEnumerable<T> newValues)
    {
        foreach (var item in newValues)
        {
            collection.Add(item);
        }
    }
}

public class RulesException : ApplicationException
{
    public RulesException()
    {
    }

    public RulesException(string message) : base(message)
    {
    }

    public RulesException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

//
// Summary:
//     Describes the node types for the nodes of an expression tree.
public enum mreOperator
{
    //
    // Summary:
    //     An addition operation, such as a + b, without overflow checking, for numeric
    //     operands.
    Add = 0,
    //
    // Summary:
    //     A bitwise or logical AND operation, such as (a & b) in C# and (a And b) in Visual
    //     Basic.
    And = 2,
    //
    // Summary:
    //     A conditional AND operation that evaluates the second operand only if the first
    //     operand evaluates to true. It corresponds to (a && b) in C# and (a AndAlso b)
    //     in Visual Basic.
    AndAlso = 3,
    //
    // Summary:
    //     A node that represents an equality comparison, such as (a == b) in C# or (a =
    //     b) in Visual Basic.
    Equal = 13,
    //
    // Summary:
    //     A "greater than" comparison, such as (a > b).
    GreaterThan = 15,
    //
    // Summary:
    //     A "greater than or equal to" comparison, such as (a >= b).
    GreaterThanOrEqual = 16,
    //
    // Summary:
    //     A "less than" comparison, such as (a < b).
    LessThan = 20,
    //
    // Summary:
    //     A "less than or equal to" comparison, such as (a <= b).
    LessThanOrEqual = 21,
    //
    // Summary:
    //     An inequality comparison, such as (a != b) in C# or (a <> b) in Visual Basic.
    NotEqual = 35,
    //
    // Summary:
    //     A bitwise or logical OR operation, such as (a | b) in C# or (a Or b) in Visual
    //     Basic.
    Or = 36,
    //
    // Summary:
    //     A short-circuiting conditional OR operation, such as (a || b) in C# or (a OrElse
    //     b) in Visual Basic.
    OrElse = 37,
    /// <summary>
    /// Checks that a string value matches a Regex expression
    /// </summary>
    IsMatch = 100,
    /// <summary>
    /// Checks that a value can be 'TryParsed' to an Int32
    /// </summary>
    IsInteger = 101,
    /// <summary>
    /// Checks that a value can be 'TryParsed' to a Single
    /// </summary>
    IsSingle = 102,
    /// <summary>
    /// Checks that a value can be 'TryParsed' to a Double
    /// </summary>
    IsDouble = 103,
    /// <summary>
    /// Checks that a value can be 'TryParsed' to a Decimal
    /// </summary>
    IsDecimal = 104,
    /// <summary>
    /// Checks if the value of the property is in the input list
    /// </summary>
    IsInInput = 105
}

public class RuleValue<T>
{
    public T Value { get; set; }
    public List<Rule> Rules { get; set; }
}

public class RuleValueString : RuleValue<string>
{
}