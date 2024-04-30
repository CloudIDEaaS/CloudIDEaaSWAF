using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public abstract class ParseResultBase
    {
        public bool HelpDisplayed { get; set; }
        public bool VersionDisplayed { get; set; }
        public bool MessageDisplayed { get; internal set; }

        public bool IsArg(string value, string arg, bool supportColon = true)
        {
            if (supportColon)
            {
                var index = value.IndexOf(':');

                if (index != -1)
                {
                    value = value.Substring(0, index);
                }
            }

            return (string.Compare(value, arg, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

    public class DefaultParseResult : ParseResultBase
    {
    }

    public static class CommandLineParser
    {
        public static void DisplayVersion(this ParseResultBase parseResult, bool inProcess = false)
        {
            var parseResultType = parseResult.GetType();
            var parserAttribute = parseResultType.GetCustomAttribute<CommandLineParserAttribute>();
            var programAssembly = parserAttribute.ProgramAssemblyType.Assembly;
            var assemblyNameParts = programAssembly.GetNameParts();
            var assemblyAttributes = programAssembly.GetAttributes();
            var assemblyName = assemblyNameParts.AssemblyName;

            Console.WriteLine("{0} v{1}", assemblyName, assemblyAttributes.Version);
            Console.WriteLine(assemblyAttributes.Company);
            Console.WriteLine(assemblyAttributes.Copyright);

            if (!inProcess)
            {
                Console.Write("Press any key to continue . . .");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine();
            }

            parseResult.VersionDisplayed = true;
        }

        public static string FormatCommandLineArg(this string str)
        {
            return str.RemoveNewLineAndCarraigeReturns().ReplaceInnerQuotesToSingle().SurroundWithQuotesIfNeeded();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Pulls the keys, values out of a command-line like string in the form of [key]:[value] with optional repeats. Values can either be a with no spaces or in quotes with spaces. </summary>
        ///
        /// <remarks>   Ken, 4/22/2020. 
        /// </remarks>
        ///
        /// <param name="commandLine">  The commandLine to act on. </param>
        ///
        /// <returns>   The command line key values. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Dictionary<string, string> GetCommandLineKeyValues(this string commandLine, out string command)
        {
            var keyValues = new Dictionary<string, string>();
            var pattern = @"((?<command>^" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + ") ?)(?<args>.*)";
            var args = commandLine.RegexGet(pattern, "args");
            Match match;

            command = commandLine.RegexGet(pattern, "command");

            if (args.Length > 0)
            {
                string key;
                string value;

                pattern = @" ?(?<key>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + ")*?:(?<value>([^ \"]+)|(\"[^\"]*?\")+)";

                do
                {
                    match = args.RegexGetMatch(pattern);

                    if (match == null)
                    {
                        break;
                    }

                    key = match.Groups["key"].Value;
                    value = match.Groups["value"].Value;

                    keyValues.Add(key, value);

                    args = match.Replace(args, string.Empty);
                }
                while (args.RegexIsMatch(pattern));
            }
            else
            {
                command = commandLine;
            }

            return keyValues;
        }
        public static Dictionary<string, string> GetCommandLineKeyValues(this string commandLine)
        {
            string command;

            return commandLine.GetCommandLineKeyValues(out command);
        }

        public static void DisplayMessage(this ParseResultBase parseResult, string format, params object[] args)
        {
            Console.WriteLine(format, args);

            Console.Write("Press any key to continue . . .");
            Console.ReadKey();

            parseResult.MessageDisplayed = true;
        }

        public static void DisplayHelp(this ParseResultBase parseResult, bool displayCommandPath = true, bool inProcess = false)
        {
            var parseResultType = parseResult.GetType();
            var parserAttribute = parseResultType.GetCustomAttribute<CommandLineParserAttribute>();
            var switchType = parserAttribute.SwitchType;
            var commandLineDescription = parserAttribute.Description;
            var programAssembly = parserAttribute.ProgramAssemblyType.Assembly;
            var assemblyNameParts = programAssembly.GetNameParts();
            var assemblyAttributes = programAssembly.GetAttributes();
            var assemblyName = assemblyNameParts.AssemblyName;
            var syntaxBuilder = new StringBuilder();
            var switchListingBuilder = new StringBuilder();
            var switchAttributes = new List<CommandLineSwitchAttribute>();
            var switchNames = new List<string>();
            int maxSwitchName;
            int x;

            foreach (var match in commandLineDescription.GetAttributeStringExpressionMatches())
            {
                var expression = match.GetGroupValue("expression");

                switch (expression)
                {
                    case "AssemblyProduct":
                        commandLineDescription = match.Replace(commandLineDescription, assemblyAttributes.Product ?? assemblyName);
                        break;
                    case "AssemblyName":
                        commandLineDescription = match.Replace(commandLineDescription, assemblyName);
                        break;
                    default:
                        DebugUtils.Break();
                        break;
                }
            }

            Console.WriteLine(commandLineDescription);
            Console.WriteLine();

            if (displayCommandPath)
            {
                var callingAssembly = Assembly.GetCallingAssembly();
                var filePath = Path.Combine(Path.GetDirectoryName(callingAssembly.Location), Path.GetFileNameWithoutExtension(callingAssembly.Location) + ".exe");

                Console.WriteLine("Path:");
                Console.WriteLine(filePath);
                Console.WriteLine();
            }

            Console.WriteLine("Syntax:");

            syntaxBuilder.Append(assemblyName);

            foreach (var switchProperty in parserAttribute.SwitchType.GetConstants())
            {
                if (switchProperty.HasCustomAttribute<CommandLineSwitchAttribute>())
                {
                    var switchAttribute = switchProperty.GetCustomAttribute<CommandLineSwitchAttribute>();
                    var switchName = (string)switchProperty.GetValue(null);

                    syntaxBuilder.AppendFormat(" [/{0}]", switchName);

                    switchNames.Add(switchName);
                    switchAttributes.Add(switchAttribute);
                }
            }

            maxSwitchName = switchNames.Max(n => n.Length);
            x = 0;

            foreach (var switchAttribute in switchAttributes)
            {
                var switchName = switchNames.ElementAt(x);
                var padding = maxSwitchName + (switchAttribute.DescriptionLeftPaddingTabCount * 8);
                var attributeDescription = switchAttribute.Description;
                var switchNamePadded = switchName.PadRight(padding, ' ');

                foreach (var match in attributeDescription.GetAttributeStringExpressionMatches())
                {
                    var expression = match.GetGroupValue("expression");
                    var property = switchType.GetProperty(expression);
                    var value = (string)property.GetValue(null);
                    var lines = value.GetLines().Select(l => ' '.Repeat(12 + switchNamePadded.Length) + l);
                    var ending = attributeDescription.RegexGet(match.Value + @"(?<ending>[^\{]*)", "ending");

                    if (value.Trim().Length > 0)
                    {
                        var expressionLines = "\r\n" + lines.Join("\r\n" + " ".Repeat(switchAttribute.DescriptionLeftPaddingTabCount));

                        attributeDescription = match.Replace(attributeDescription, expressionLines);

                        if (ending.Length > 0)
                        {
                            attributeDescription = attributeDescription.Replace(ending, "\r\n" + ' '.Repeat(4 + switchNamePadded.Length) + ending + "\r\n");
                        }
                    }
                    else
                    {
                        attributeDescription = switchAttribute.FallbackDescription;
                    }
                }

                switchListingBuilder.AppendLineFormat(@"{0}/{1}{2}", ' '.Repeat(4), switchNamePadded, attributeDescription);

                x++;
            }

            Console.WriteLine(syntaxBuilder);

            Console.WriteLine();
            Console.WriteLine(switchListingBuilder);

            if (!inProcess)
            {
                Console.Write("Press any key to continue . . .");
                Console.ReadKey();
            }

            parseResult.HelpDisplayed = true;
        }

        public static T ParseArgs<T>(string[] arguments, Action<T, string> parseArg, Action<T, string, string> parseSwitch) where T : ParseResultBase, new()
        {
            ParseResultBase result = typeof(T).CreateInstance<T>();

            for (int i = 0; i < arguments.Length; i++)
            {
                string str = arguments[i];

                if (!string.IsNullOrEmpty(str))
                {
                    int colonIndex;
                    int equalsIndex;
                    int spaceIndex;
                    string colonArgument = null;
                    string equalsArgument = null;
                    string spaceArgument = null;

                    str = str.Trim();

                    if (!string.IsNullOrEmpty(str))
                    {
                        if ((str[0] == '/') || (str[0] == '-'))
                        {
                            str = str.Substring(1);

                            if ((str[0] == '-'))
                            {
                                str = str.Substring(1);
                            }
                        }
                        else
                        {
                            parseArg((T)result, str);
                            continue;
                        }

                        equalsIndex = str.IndexOf('=');
                        colonIndex = str.IndexOf(':');
                        spaceIndex = str.IndexOf(' ');

                        if (equalsIndex != -1)
                        {
                            equalsArgument = str.Right(str.Length - equalsIndex - 1);
                            str = str.Left(equalsIndex);

                            parseSwitch((T)result, str, equalsArgument);
                        }
                        else if (colonIndex != -1)
                        {
                            colonArgument = str.Right(str.Length - colonIndex - 1);
                            str = str.Left(colonIndex);

                            parseSwitch((T)result, str, colonArgument);
                        }
                        else if (spaceIndex != -1)
                        {
                            spaceArgument = str.Right(str.Length - spaceIndex - 1);
                            str = str.Left(spaceIndex);

                            parseSwitch((T)result, str, spaceArgument);
                        }
                        else
                        {
                            parseSwitch((T)result, str, null);
                        }
                    }
                }
            }

            return (T) result;
        }
    }
}
