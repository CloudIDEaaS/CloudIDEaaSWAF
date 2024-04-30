using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.Parsing
{
    public static class TypeSignatureParser
    {
        static readonly Regex TypeNameRegex = new Regex(@"(?<TypeName>((class )|(struct ))?[a-zA-Z0-9_\s\[\]:\*\& ]+)(<(?<InnerTypeName>((class )|(struct ))?[a-zA-Z0-9_,\<\>\s\[\]:\*\& ]+?)>)?(?<Array>(\[\]))?$", RegexOptions.Compiled);
        public const string MethodPattern = @"(\<[^:]*?\>)?\(.*?\)?(\s*const)?$";
        public const string ParensParmsPattern = @"\(.*?\)?(\s*const)?$";
        public const string ParensPattern = @"\(.*?\)";
        public const string ConstructorPattern = @"^((?<namespace>.*?)::)?(?<typeName>.*)::\3$";
        public const string DestructorPattern = @"^((?<namespace>.*?)::)?(?<typeName>.*)::~\3$";
        public const string NamespacePattern = "^((class )|(struct ))?(?<namespace>[^:]*?)::";
        public const string VTablePattern = @"^`(?<member>vftable)'\{for `(?<interface>.*?')\}";
        public const string LambdaPattern = @"\<(?<lambda>lambda_[a-fA-F0-9]*?)\>";
        public const string TypeNumber = @"(?<typedelim>`(?<type>.*?)')::(?<numberdelim>`(?<number>\d*?)')";
        public const string TypeNumber2 = @"(?<delim>`(?<type>.*?)'\{(?<number>\d*?)\}\')";
        static List<string> keywordPatterns;

        static TypeSignatureParser()
        {
            keywordPatterns = new List<string>
            {
                @"\[thunk\]\:",
                "__thiscall ",
                "__stdcall",
                "__cdecl",
                @"^private\: ",
                @"^protected\: ",
                @"^public\: ",
                @"^struct ",
                @"^class ",
                @"^virtual ",
                @"^unsigned ",
                @"^static ",
                @"^\(",
                @"`adjustor{\d*?}' "
            };
        }

        public static bool CanParse(string typeSymbol)
        {
            TypeInformation typeInformation;

            try
            {
                if (TryParse(typeSymbol, false, out typeInformation))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool TryParse(string typeSymbol, bool skipFrontWork, out TypeInformation typeInformation)
        {
            typeInformation = null;

            try
            {
                if (typeSymbol.Contains("?") && typeSymbol.Contains("@"))
                {
                    typeSymbol = DebugUtils.UndecorateSymbolName(typeSymbol);
                }

                if (typeSymbol.StartsWithAny("__GUID", "_GUID", "__IID", "_IID", "__CLASSID", "_CLASSID", "_SID", "_guid", "_DIID"))
                {
                    var guidName = typeSymbol.RegexGet("^_*?[A-U]*?ID(?<guidname>.*)$", "guidname");

                    TypeInformation type = new TypeInformation
                    {
                        IsGuidAddress = true,
                        GuidName = guidName,
                        TypeSymbol = typeSymbol
                    };

                    typeInformation = type;

                    return true;
                }
                else
                {
                    string namespaceName = null;
                    string methodName = null;
                    string memberName = null;
                    string returnType = null;
                    string interfaceType = null;
                    string lambda = null;
                    string typeName = null;
                    string modifiers = null;
                    bool isDestructor = false;
                    bool isConstructor = false;
                    bool isVTableConst = false;
                    var originalTypeSymbol = typeSymbol;
                    Match match;

                    if (!skipFrontWork)
                    {
                        int doubleColonMatchCount;

                        if (typeSymbol.RegexIsMatch(@"const (?<typeornamespace>.*?)::`vftable'\{for `(?<interface>.*?)'\}"))
                        {
                            match = typeSymbol.RegexGetMatch(@"const (?<typeornamespace>.*?)::`vftable'\{for `(?<interface>.*?)'\}");

                            typeName = match.GetGroupValue("typeornamespace");
                            interfaceType = match.GetGroupValue("interface");

                            if (typeName.IsNullOrEmpty())
                            {
                                DebugUtils.Break();
                            }

                            if (interfaceType.IsNullOrEmpty())
                            {
                                DebugUtils.Break();
                            }

                            isVTableConst = true;
                        }
                        else
                        {
                            typeSymbol = PreProcess(typeSymbol, out returnType, out interfaceType, out lambda, out modifiers);
                            doubleColonMatchCount = typeSymbol.RegexGetMatches("::").Count();

                            if (doubleColonMatchCount >= 2)
                            {
                                namespaceName = string.Empty;

                                while (typeSymbol.RegexGetMatches("::").Count() >= 2)
                                {
                                    if (typeSymbol.RegexIsMatch(ParensParmsPattern))
                                    {
                                        var prefix = typeSymbol.RegexRemove(ParensParmsPattern);

                                        if (prefix.RegexIsMatch(DestructorPattern))
                                        {
                                            var destructorMatch = prefix.RegexGetMatch(DestructorPattern);

                                            typeName = destructorMatch.GetGroupValue("typeName");
                                            namespaceName = destructorMatch.GetGroupValue("namespace");

                                            isDestructor = true;
                                            break;
                                        }
                                        else if (prefix.RegexIsMatch(ConstructorPattern))
                                        {
                                            var constructorMatch = prefix.RegexGetMatch(ConstructorPattern);

                                            typeName = constructorMatch.GetGroupValue("typeName");
                                            namespaceName = constructorMatch.GetGroupValue("namespace");

                                            isConstructor = true;
                                            break;
                                        }
                                    }

                                    if (!isDestructor && !isConstructor)
                                    {
                                        Match matchNamespace;

                                        if (methodName == null)
                                        {
                                            var matchMethod = typeSymbol.RegexGetMatch("::(?<method>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + MethodPattern + ")");
                                            var matchMember = typeSymbol.RegexGetMatch("::(?<member>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + ")$");

                                            if (matchMethod != null)
                                            {
                                                methodName = matchMethod.GetGroupValue("method");

                                                typeSymbol = typeSymbol.Remove(matchMethod.Index, matchMethod.Length);
                                            }
                                            else if (matchMember != null)
                                            {
                                                memberName = matchMember.GetGroupValue("member");

                                                typeSymbol = typeSymbol.Remove(matchMember.Index, matchMember.Length);
                                            }
                                        }

                                        matchNamespace = typeSymbol.RegexGetMatch(NamespacePattern);

                                        if (matchNamespace != null && HasBalancedBrackets(typeSymbol) && HasBalancedBrackets(matchNamespace.GetGroupValue("namespace")))
                                        {
                                            namespaceName += matchNamespace.GetGroupValue("namespace") + "::";

                                            if (matchNamespace.Length == typeSymbol.Length)
                                            {
                                                typeSymbol = namespaceName;
                                                namespaceName = string.Empty;
                                            }
                                            else
                                            {
                                                typeSymbol = typeSymbol.Remove(matchNamespace.Index, matchNamespace.Length);
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (doubleColonMatchCount == 1)
                            {
                                var matchMethod = typeSymbol.RegexGetMatch("::(?<method>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + MethodPattern + ")$");
                                var matchMember = typeSymbol.RegexGetMatch("::(?<member>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + ")$");

                                if (matchMethod != null)
                                {
                                    methodName = matchMethod.GetGroupValue("method");

                                    typeSymbol = typeSymbol.Remove(matchMethod.Index, matchMethod.Length);
                                }
                                else if (matchMember != null)
                                {
                                    memberName = matchMember.GetGroupValue("member");

                                    typeSymbol = typeSymbol.Remove(matchMember.Index, matchMember.Length);
                                }
                            }
                            else if (doubleColonMatchCount == 0)
                            {
                                var matchMethod = typeSymbol.RegexGetMatch("(?<method>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + MethodPattern + ")$");
                                var matchMember = typeSymbol.RegexGetMatch("(?<member>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + ")$");

                                if (matchMethod != null)
                                {
                                    methodName = matchMethod.GetGroupValue("method");

                                    typeSymbol = typeSymbol.Remove(matchMethod.Index, matchMethod.Length);
                                }
                                else if (matchMember != null)
                                {
                                    memberName = matchMember.GetGroupValue("member");

                                    typeSymbol = typeSymbol.Remove(matchMember.Index, matchMember.Length);
                                }
                            }
                        }
                    }

                    if (typeName == null && memberName == null)
                    {

                        // Try to match the type to our regular expression.
                        match = TypeNameRegex.Match(typeSymbol);
                    }
                    else if (typeName != null)
                    {
                        match = TypeNameRegex.Match(typeName);
                    }
                    else if (memberName != null)
                    {
                        typeName = null;
                        match = null;
                    }
                    else
                    {
                        match = null;
                        DebugUtils.Break();
                    }

                    // If that fails, the format is incorrect.
                    if (typeName != null && !match.Success)
                    {
                        return false;
                    }

                    bool isArray;
                    string innerTypeFriendlyName;

                    if (match != null)
                    {
                        // Scrub the type name, inner type name, and array '[]' marker (if present).
                        typeName = match.Groups["TypeName"].Value;

                        innerTypeFriendlyName = match.Groups["InnerTypeName"].Value.RemoveStartIfMatches("class ");
                        isArray = !string.IsNullOrWhiteSpace(match.Groups["Array"].Value);
                    }
                    else
                    {
                        isArray = false;
                        innerTypeFriendlyName = null;
                    }

                    // Create the root type information.
                    TypeInformation type = new TypeInformation
                    {
                        TypeName = typeName,
                        IsArray = isArray
                    };

                    // Check if we have an inner type name (in the case of generics).
                    if (!string.IsNullOrWhiteSpace(innerTypeFriendlyName))
                    {
                        // Split each type by the comma character.
                        var innerTypeNames = SplitByComma(innerTypeFriendlyName);

                        // Iterate through all inner type names and attempt to parse them recursively.
                        foreach (string innerTypeName in innerTypeNames)
                        {
                            TypeInformation innerType = null;
                            var trimmedInnerTypeName = innerTypeName.Trim();
                            var success = TryParse(trimmedInnerTypeName, true, out innerType);

                            // If the inner type fails, so does the parent.
                            if (!success)
                                return false;

                            // Success! Add the inner type to the parent.
                            type.InnerTypes.Add(innerType);
                        }
                    }

                    type.TypeSymbol = originalTypeSymbol;

                    if (modifiers != null)
                    {
                        type.Modifiers = modifiers;
                    }

                    if (isVTableConst)
                    {
                        type.IsVTableConst = true;
                    }

                    if (interfaceType != null)
                    {
                        type.InterfaceType = interfaceType;
                    }

                    if (returnType != null)
                    {
                        type.ReturnType = returnType;
                    }

                    if (isDestructor)
                    {
                        type.IsDestructor = true;
                    }

                    if (isConstructor)
                    {
                        type.IsConstructor = true;
                    }

                    if (namespaceName != null)
                    {
                        namespaceName = namespaceName.Trim();
                        namespaceName = namespaceName.RemoveEndIfMatches("::");
                        namespaceName = namespaceName.RemoveStartIfMatches("(");
                        namespaceName = namespaceName.Trim();

                        if (namespaceName.RegexIsMatch("^" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + " "))
                        {
                            //DebugUtils.Break();
                        }

                        type.NamespaceName = namespaceName;
                    }

                    if (methodName != null)
                    {
                        type.MethodName = methodName;
                        type.MemberName = methodName;
                    }
                    else if (memberName != null)
                    {
                        type.MemberName = memberName;
                    }

                    if (lambda != null)
                    {
                        type.Lambda = lambda;
                    }

                    // Return the parsed type information.
                    typeInformation = type;
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();

                return false;
            }

            return true;
        }

        private static bool HasBalancedBrackets(string typeSymbol)
        {
            var angleBracketDepth = 0;

            foreach (var ch in typeSymbol)
            {
                switch (ch)
                {
                    case '<':
                        angleBracketDepth++;
                        break;
                    case '>':
                        angleBracketDepth--;
                        break;
                }
            }

            return angleBracketDepth == 0;
        }

        private static string PreProcess(string typeSymbol, out string returnType, out string interfaceType, out string lambda, out string modifiers)
        {
            returnType = string.Empty;
            interfaceType = string.Empty;
            modifiers = null;
            lambda = null;

            foreach (var pattern in keywordPatterns)
            {
                if (typeSymbol.RegexIsMatch(pattern))
                {
                    var match = typeSymbol.RegexGetMatch(pattern);

                    modifiers += typeSymbol.Substring(match.Index, match.Length) + " ";

                    typeSymbol = typeSymbol.RegexRemove(pattern);
                }
            }

            typeSymbol = typeSymbol.Replace("> >", ">>").Replace(" >", ">").Replace(" *", "*").Replace(" &", "&").Replace("  ", " ").Trim();

            while (typeSymbol.RegexIsMatch(LambdaPattern))
            {
                lambda = typeSymbol.RegexGet(LambdaPattern, "lambda");

                typeSymbol = typeSymbol.RegexReplace(LambdaPattern, "lambda");
            }

            while (typeSymbol.RegexIsMatch(TypeNumber))
            {
                var match = typeSymbol.RegexGetMatch(TypeNumber);
                var typeDelimGroup = match.Groups["typedelim"];
                var numberDelimGroup = match.Groups["numberdelim"];
                var typeGroup = match.Groups["type"];
                var numberGroup = match.Groups["number"];
                var type = typeGroup.Value;
                var number = numberGroup.Value;

                typeSymbol = typeSymbol.Substitute(numberDelimGroup.Index, numberDelimGroup.Length, "_" + number);
                typeSymbol = typeSymbol.Substitute(typeDelimGroup.Index, typeDelimGroup.Length, type);
            }

            while (typeSymbol.RegexIsMatch(TypeNumber2))
            {
                var match = typeSymbol.RegexGetMatch(TypeNumber2);
                var delimGroup = match.Groups["delim"];
                var typeGroup = match.Groups["type"];
                var numberGroup = match.Groups["number"];
                var type = typeGroup.Value;
                var number = numberGroup.Value;

                typeSymbol = typeSymbol.Substitute(delimGroup.Index, delimGroup.Length, type + "_" + number);
            }

            while (typeSymbol.Contains(" "))
            {
                var index = 0;
                var angleBracketDepth = 0;

                var breakFor = false;
                var breakWhile = false;

                foreach (var ch in typeSymbol)
                {
                    switch (ch)
                    {
                        case ' ':
                            {
                                if (angleBracketDepth == 0 && !typeSymbol.Skip(index + 1).SkipWhile(c => c == ' ').FirstOrDefault().Equals('('))
                                {
                                    returnType += typeSymbol.Left(index) + " ";
                                    typeSymbol = typeSymbol.RightAfter(index);
                                    breakFor = true;

                                    break;
                                }
                            }
                            break;
                        case ':':
                            {
                                if (angleBracketDepth == 0 && typeSymbol.Skip(index + 1).FirstOrDefault().Equals(':'))
                                {
                                    var right = typeSymbol.RightAfter(index + 1);
                                    
                                    if (right.RegexIsMatch(VTablePattern))
                                    {
                                        var match = right.RegexGetMatch(VTablePattern);
                                        Group group;

                                        interfaceType = match.GetGroupValue("interface");
                                        group = match.Groups["member"];

                                        typeSymbol = typeSymbol.Left(index + group.Index + group.Length).RemoveText("`");

                                        breakWhile = true;
                                    }

                                    break;
                                }
                            }
                            break;
                        case '(':
                            breakWhile = true;
                            break;
                        case '<':
                            angleBracketDepth++;
                            break;
                        case '>':
                            angleBracketDepth--;
                            break;
                    }

                    if (breakFor || breakWhile)
                    {
                        breakFor = false;
                        break;
                    }

                    index++;
                }

                if (breakWhile || index == typeSymbol.Length)
                {
                    break;
                }
            }
            
            return typeSymbol;
        }

        public static TypeInformation Parse(string typeSymbol, SpecialFunction specialFunction = null)
        {
            TypeInformation typeInformation;

            if (specialFunction != null && !specialFunction.Type.IsNullOrEmpty())
            {
                typeSymbol = specialFunction.Type;
            }

            if (TryParse(typeSymbol, false, out typeInformation))
            {
                typeInformation.SpecialFunction = specialFunction;

                return typeInformation;
            }

            throw new Exception(string.Format("Type signature compiler exception, Error parsing '{0}'", typeSymbol));
        }

        private static IEnumerable<string> SplitByComma(string value)
        {
            var strings = new List<string>();
            var sb = new StringBuilder();
            var level = 0;

            foreach (var c in value)
            {
                if (c == ',' && level == 0)
                {
                    strings.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }

                if (c == '<')
                    level++;

                if (c == '>')
                    level--;
            }

            strings.Add(sb.ToString());

            return strings;
        }

    }
}
