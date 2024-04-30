using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TextTemplating;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AnyAscii;
using Pluralize.NET;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Math;



#endif

#if INCLUDE_FARE
using Fare;
#endif

namespace Utils 
{
    public enum QuoteTextType
    {
        None,
        SingleQuote,
        DoubleQuote
    }

    public static class StringExtensions
    {
        public const string REGEX_IDENTIFIER = @"^(?:(~?(?!\d)\w+(?:\.(?!\d)\w+)*)\.)?((?!\d)\w+)$";
        public const string REGEX_IDENTIFIER_MIDSTRING = @"(?:(~?(?!\d)\w+(?:\.(?!\d)\w+)*)\.)?((?!\d)\w+)";
        public const string REGEX_IDENTIFIER_CHARS = @"[\w\d]";
        public const string REGEX_INTEGER_OR_DECIMAL_MIDSTRING = @"((\d+)((\.\d{1,2})?))";

#if !SILVERLIGHT
        private static IPluralize pluralizer;
        private static string BytOrderMarkUtf8;
        private static List<string> insignificantWordList;
        internal static Dictionary<StringBuilder, Stack<TagHandler>> tagHandlerStack;
        private static List<string> insignificantWordListLevel1;
        const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern bool InitAtomTable(uint nSize);
        [DllImport("kernel32.dll")]
        public static extern ushort AddAtom(string lpString);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetAtomName(ushort nAtom, [Out] StringBuilder lpBuffer, int nSize);
        [DllImport("kernel32.dll")]
        public static extern ushort DeleteAtom(ushort nAtom);

        static StringExtensions()
        {
            pluralizer = new Pluralizer();
            tagHandlerStack = new Dictionary<StringBuilder, Stack<TagHandler>>();

            insignificantWordList = InsignificantWords.GetInsignificantWords();
            insignificantWordListLevel1 = InsignificantWords.GetLevel1InsignificantWords();

            BytOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        }
#endif
        public static string CreateRandomString(int length)
        {
            var random = new Random();

            return random.NextStrings(AllowedChars, (length, length), 1).Single();
        }

        public static IDisposable OpenBrace(this StringBuilder builder, int tabCount)
        {
            builder.AppendLineFormatTabIndent(tabCount, "{{");

            return builder.CreateDisposable(() =>
            {
                builder.AppendLineFormatTabIndent(tabCount, "}}");
            });
        }

        public static IDisposable OpenBracket(this StringBuilder builder, int tabCount)
        {
            builder.AppendLineFormatTabIndent(tabCount, "[");

            return builder.CreateDisposable(() =>
            {
                builder.AppendLineFormatTabIndent(tabCount, "]");
            });
        }

        private static IEnumerable<string> NextStrings(this Random random, string allowedChars, (int Min, int Max) length, int count)
        {
            ISet<string> usedRandomStrings = new HashSet<string>();
            (int min, int max) = length;
            char[] chars = new char[max];
            int setLength = allowedChars.Length;

            while (count-- > 0)
            {
                int stringLength = random.Next(min, max + 1);

                for (int i = 0; i < stringLength; ++i)
                {
                    chars[i] = allowedChars[random.Next(setLength)];
                }

                string randomString = new string(chars, 0, stringLength);

                if (usedRandomStrings.Add(randomString))
                {
                    yield return randomString;
                }
                else
                {
                    count++;
                }
            }
        }
        public static T ToObject<T>(this Match match)
        {
            var obj = Activator.CreateInstance<T>();

            foreach (var group in match.Groups.Cast<Group>())
            {
                if (group.Name == "0")
                {
                    continue;
                }
                else
                {
                    var property = group.Name;
                    var value = group.Value;
                    var propertyInfo = typeof(T).GetProperties().Single(p => p.Name.AsCaseless() == property || (p.HasCustomAttribute<JsonPropertyAttribute>() && p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName.AsCaseless() == property));

                    propertyInfo.SetValue(obj, value);
                }
            }

            return obj;
        }

        public static string AsciiOnly(this string str)
        {
            return str.Transliterate();
        }

        public static string MakeHashtag(this string str)
        {
            return "#" + str.CapitalizeWords().Replace(" ", "");
        }

        public static string Flatten(this string str)
        {
            return str.Replace("\r\n", string.Empty).Replace("\n", string.Empty);
        }

        public static string EncryptStringToString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static byte[] EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return array;
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string SingleLineCondense(this string text)
        {
            text = text.RegexReplace("[\r|\n]", " ").Replace("  ", " ");

            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            return text;
        }

        public static string DoubleToSingleQuotes(this string text)
        {
            return text.Replace("\"", "'");
        }

        public static string ReplaceLinesBetween(this string str, string replacement, int startLine, int endLine)
        {
            string str1;
        
            if (!str.IsNullOrEmpty())
            {
                StringBuilder stringBuilder = new StringBuilder();

                using (StringReader stringReader = new StringReader(str))
                {
                    using (StringWriter stringWriter = new StringWriter(stringBuilder))
                    {
                        int num = 1;
                        bool flag = false;
                        while (true)
                        {
                            string str2 = stringReader.ReadLine();
                            string str3 = str2;
                            if (str2 == null)
                            {
                                break;
                            }
                            if ((num < startLine ? true : num > endLine))
                            {
                                stringWriter.WriteLine(str3);
                            }
                            else if (!flag)
                            {
                                stringWriter.WriteLine(replacement);
                                flag = true;
                            }
                            num++;
                        }
                    }
                }
                str1 = stringBuilder.ToString();
            }
            else
            {
                str1 = str;
            }
            return str1;
        }

        public static string DecryptString(string key, byte[] buffer)
        {
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string RightMinusLengthOf(this string text, string right)
        {
            var count = text.Length - right.Length;

            if (text.Length > count)
            {
                return text.Right(count);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string[] Split(this string str, char ch, StringSplitOptions options)
        {
            return str.Split(new char[] { ch }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitNullToEmpty(this string str, string delimiter, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (str.IsNullOrEmpty())
            {
                return new string[0];
            }
            else
            {
                return str.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static string[] SplitNullToEmpty(this string str, char ch, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (str.IsNullOrEmpty())
            {
                return new string[0];
            }
            else
            {
                return str.Split(new char[] { ch }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static bool Contains(this StringBuilder builder, string text)
        {
            return builder.IndexOf(text) != -1;
        }

        public static int IndexOf(this StringBuilder builder, string text)
        {
            if (builder == null || text == null)
                throw new ArgumentNullException();
            if (text.Length == 0)
                return 0;//empty strings are everywhere!
            if (text.Length == 1)//can't beat just spinning through for it
            {
                char c = text[0];
                for (int idx = 0; idx != builder.Length; ++idx)
                    if (builder[idx] == c)
                        return idx;
                return -1;
            }
            int m = 0;
            int i = 0;
            int[] T = KMPTable(text);
            while (m + i < builder.Length)
            {
                if (text[i] == builder[m + i])
                {
                    if (i == text.Length - 1)
                        return m == text.Length ? -1 : m;//match -1 = failure to find conventional in .NET
                    ++i;
                }
                else
                {
                    m = m + i - T[i];
                    i = T[i] > -1 ? T[i] : 0;
                }
            }
            return -1;
        }

        private static int[] KMPTable(string sought)
        {
            int[] table = new int[sought.Length];
            int pos = 2;
            int cnd = 0;
            table[0] = -1;
            table[1] = 0;
            while (pos < table.Length)
                if (sought[pos - 1] == sought[cnd])
                    table[pos++] = ++cnd;
                else if (cnd > 0)
                    cnd = table[cnd];
                else
                    table[pos++] = 0;
            return table;
        }

        public static bool IsValidEmailAddress(this string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        public static bool IsValidUrl(this string source)
        {
            return !source.IsNullWhiteSpaceOrEmpty() && Uri.IsWellFormedUriString(source, UriKind.Absolute);
        }

        public static bool IsNumeric(this string input)
        {
            int number;

            return int.TryParse(input, out number);
        }

        public static string DoTextReplacements(this Dictionary<string, string> textReplacements, string str)
        {
            foreach (var pair in textReplacements)
            {
                str = str.Replace(pair.Key, pair.Value);
            }

            return str;
        }

        public static string ReplaceTokens(this string value, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                value = value.RegexReplace(@"{\s*?" + replacement.Key + @"\s*?}", replacement.Value);
            }

            return value;
        }

        public static String[] SplitCombine(this string str, params char[] separator)
        {
            var parts = str.Split(separator);
            var list = new List<string>();
            var lastPart = string.Empty;
            var separatorText = string.Join(string.Empty, separator);

            foreach (var part in parts)
            {
                var combinedPart = lastPart.AppendIfMissing(separatorText) + part;

                list.Add(combinedPart);
                lastPart = combinedPart;
            }

            return list.ToArray();
        }

        public static bool IsInsignificantWord(this string str, IEnumerable<string> exclusions = null)
        {
            if (exclusions != null)
            {
                return insignificantWordList.Where(w => !exclusions.Any(e => w == e)).Contains(str);
            }
            else
            {
                return insignificantWordList.Contains(str);
            }
        }

        public static bool IsInsignificantWord(this string str, int level, IEnumerable<string> exclusions = null)
        {
            if (exclusions != null)
            {
                return insignificantWordListLevel1.Where(w => !exclusions.Any(e => w == e)).Contains(str);
            }
            else
            {
                return insignificantWordListLevel1.Contains(str);
            }
        }

        public static SecureString ToSecureString(this string str)
        {
            var secureString = new SecureString();

            foreach (var ch in str)
            {
                secureString.AppendChar(ch);
            }

            return secureString;
        }

        public static string ToSingleLine(this string str)
        {
            var line = str.RegexReplace($"\n", @"\n");
            var lineCount = line.GetLineCount();

            Debug.Assert(lineCount <= 1);

            return line;
        }

        public static string ToReportDisplayText(this string str)
        {
            str = str.ToSingleLine().Replace("\"", "\"\"").SurroundWithQuotes();

            return str;
        }

        public static SecureString ToSecureString(this IConfigurationSection configurationSection)
        {
            var secureString = new SecureString();
            var builder = new StringBuilder();

            foreach (var section in configurationSection.GetChildren())
            {
                builder.AppendLine(section.Value);
            }

            foreach (var ch in builder.ToString())
            {
                secureString.AppendChar(ch);
            }

            return secureString;
        }

        public static string Unsecure(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static bool Match(this Regex regex, string input, Action<Match> a)
        {
            if (regex.IsMatch(input))
            {
                a(regex.Match(input));

                return true;
            }

            return false;
        }

#if INCLUDE_FARE
        public static string GetRandomText(string pattern)
        {
            return new Regex(pattern).GetRandomText();
        }

        public static string GetRandomText(this Regex regex)
        {
            var xeger = new Xeger(regex.ToString());
            var generatedString = xeger.Generate();

            return generatedString;
        }
#endif
        public static string GetGroupValue(this Match match, string groupName)
        {
            return match.Groups[groupName].Value;
        }

        public static string Replace(this Match match, string input, string replacement)
        {
            var index = match.Index;
            var length = input.Length;

            return input.Left(index) + replacement + input.Right(length - (index + match.Length));
        }

        public static string Replace(this Match match, string input, string replacement, int indexIncrement)
        {
            var index = match.Index + indexIncrement;
            var length = input.Length;

            return input.Left(index) + replacement + input.Right(length - (index + match.Length));
        }

        public static string Replace(this Group group, string input, string replacement)
        {
            var index = group.Index;
            var length = input.Length;

            return input.Left(index) + replacement + input.Right(length - (index + group.Length));
        }

        public static string GetValue(this Match match, string groupName)
        {
            var value = match.Groups[groupName].Value;

            return value;
        }

        public static string GetGroupValue(this Regex regex, string input, string groupName)
        {
            var match = regex.Match(input);
            var value = match.Groups[groupName].Value;

            return value;
        }

        public static string SlashesToPipes(this string text)
        {
            if (text.Contains("\\"))
            {
                text = text.Replace("\\", "|");
            }

            return text;
        }

        public static string RightAtIndexOf(this string text, char ch)
        {
            if (text.Contains(ch))
            {
                return text.Right(text.Length - text.IndexOf(ch) - 1);
            }
            else
            {
                return text;
            }
        }

        public static string RightAfterIndexOf(this string text, char ch)
        {
            if (text.Contains(ch))
            {
                return text.Right(text.Length - text.IndexOf(ch));
            }
            else
            {
                return text;
            }
        }

        public static string PipesToSlashes(this string text)
        {
            if (text.Contains("|"))
            {
                text = text.Replace("|", "\\");
            }

            return text;
        }

        public static bool IsCrOrLf(this char ch)
        {
            return ch == '\r' || ch == '\n';
        }

        public static bool IsDigit(this char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        public static bool IsCrLfString(this string str)
        {
            return str == "\r" || str == "\n" || str == "\r\n";
        }

        public static bool IsAbsoluteUri(this string str)
        {
            return str.RegexIsMatch(@"/^https?:\/\//i");
        }

        public static string SpaceBlock(this string str, int spaceCount)
        {
            str = str.GetLines().ToMultiLineList(l => l.Prepend(" ".Repeat(spaceCount)));

            return str;
        }

        public static string TrimBlankLines(this string str)
        {
            str = str.GetLines().Where(l => l.Trim().Length > 0).ToMultiLineList();

            return str;
        }

        public static string GetLastLine(this StringBuilder builder)
        {
            try
            {
                var text = builder.ToString();
                var lineCount = text.Trim().GetLineCount();

                if (lineCount > 0)
                {
                    return text.GetLines().ElementAt(lineCount - 1);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Format("Error with GetLastLine: '{0}'", ex.ToString());
            }
        }

        public static int GetLineCount(this StringBuilder builder)
        {
            return builder.ToString().GetLineCount();
        }

        public static int GetLineCount(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }
            else
            {
                return str.Count(c => c == '\n') + 1;
            }
        }

        public static string SpaceIndentLine(this string str, int count)
        {
            return new string(' ', count) + str;
        }

        public static string SpaceIndentLines(this string str, int count, int lineSkip = 0)
        {
            var builder = new StringBuilder();
            var lines = str.GetLines();
            var lineCount = lines.Count();
            var x = 0;

            foreach (var line in lines)
            {
                if (x >= lineSkip)
                {
                    var newLine = new string(' ', count) + line;

                    if (x == lineCount - 1)
                    {
                        builder.Append(newLine);
                    }
                    else
                    {
                        builder.AppendLine(newLine);
                    }
                }
                else
                {
                    if (x == lineCount - 1)
                    {
                        builder.Append(line);
                    }
                    else
                    {
                        builder.AppendLine(line);
                    }
                }

                x++;
            }

            return builder.ToString();
        }

        public static int GetCountOfCharAtStart(this string str, char ch)
        {
            var charStr = ch.ToString();

            return str.RegexGet($"^{charStr}*(?=[^{charStr}])").Length;
        }
        public static string RemoveLines(this string str, params int[] line)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            else
            {
                var builder = new StringBuilder();

                using (var stringReader = new StringReader(str))
                {
                    using (var stringWriter = new StringWriter(builder))
                    {
                        string strLine;
                        var x = 1;

                        while ((strLine = stringReader.ReadLine()) != null)
                        {
                            if (!line.Contains(x))
                            {
                                stringWriter.WriteLine(strLine);
                            }

                            x++;
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static string RemoveLinesBetween(this string str, int startLine, int endLine)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            else
            {
                var builder = new StringBuilder();

                using (var stringReader = new StringReader(str))
                {
                    using (var stringWriter = new StringWriter(builder))
                    {
                        string strLine;
                        var x = 1;

                        while ((strLine = stringReader.ReadLine()) != null)
                        {
                            if (x < startLine || x > endLine)
                            {
                                stringWriter.WriteLine(strLine);
                            }

                            x++;
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static string GetLinesBetween(this string str, int startLine, int endLine)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            else
            {
                var builder = new StringBuilder();

                using (var stringReader = new StringReader(str))
                {
                    using (var stringWriter = new StringWriter(builder))
                    {
                        string strLine;
                        var x = 1;

                        while ((strLine = stringReader.ReadLine()) != null)
                        {
                            if (x >= startLine && x <= endLine)
                            {
                                stringWriter.WriteLine(strLine);
                            }

                            x++;
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static string GetLastLine(this string str)
        {
            var lineIndex = str.GetLineCount();

            return str.GetLine(lineIndex);
        }

        public static string GetFirstLine(this string str)
        {
            var count = str.GetLineCount();

            if (count > 0)
            {
                return str.GetLine(1);
            }

            return null;
        }

        public static string GetLastNonCrlfLine(this string str)
        {
            var lineIndex = str.GetLineCount() - 1;

            return str.GetLine(lineIndex);
        }

        public static string GetLine(this string str, int line)
        {
            return str.GetLinesBetween(line, line);
        }

        public static string GetLinesStartingAt(this StringBuilder builder, int startLine)
        {
            return builder.ToString().GetLinesStartingAt(startLine);
        }

        public static string GetLinesStartingAt(this string str, int startLine)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            else
            {
                var builder = new StringBuilder();

                using (var stringReader = new StringReader(str))
                {
                    using (var stringWriter = new StringWriter(builder))
                    {
                        string strLine;
                        var x = 1;

                        while ((strLine = stringReader.ReadLine()) != null)
                        {
                            if (x >= startLine)
                            {
                                stringWriter.WriteLine(strLine);
                            }

                            x++;
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static int GetStartingTabCount(this string str)
        {
            var tabs = str.TakeWhile(c => c == '\t');

            return tabs.Count();
        }

        public static IEnumerable<string> GetLines(this StringBuilder builder, bool removeEmpty = false)
        {
            return builder.ToString().GetLines(removeEmpty);
        }

        public static IEnumerable<string> GetLines(this string str, bool removeEmpty = false)
        {
            if (str.IsNullOrEmpty())
            {
                yield break;
            }
            else
            {
                using (var stringReader = new StringReader(str))
                {
                    string line;

                    while ((line = stringReader.ReadLine()) != null)
                    {
                        if (removeEmpty)
                        {
                            if (!line.Trim().IsNullOrEmpty())
                            {
                                yield return line;
                            }
                        }
                        else
                        {
                            yield return line;
                        }
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<int, string>> GetLinesWithLineNumbers(this string str, bool removeEmpty = false)
        {
            if (str.IsNullOrEmpty())
            {
                yield break;
            }
            else
            {
                var x = 1;

                using (var stringReader = new StringReader(str))
                {
                    string line;

                    while ((line = stringReader.ReadLine()) != null)
                    {
                        if (removeEmpty)
                        {
                            if (!line.Trim().IsNullOrEmpty())
                            {
                                yield return new KeyValuePair<int, string>(x, line);
                            }
                        }
                        else
                        {
                            yield return new KeyValuePair<int, string>(x, line);
                        }

                        x++;
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<KeyValuePair<int, int>, string>> GetLinesWithLineNumbersAndCharIndexes(this string str, bool removeEmpty = false)
        {
            if (str.IsNullOrEmpty())
            {
                yield break;
            }
            else
            {
                var x = 1;
                var charPos = 0;
                var lastPos = 0;

                using (var stringReader = new StringReader(str))
                {
                    string line;

                    while ((line = stringReader.ReadLine()) != null)
                    {
                        var pos = stringReader.GetPrivateFieldValue<int>("_pos");

                        if (pos < charPos)
                        {
                            DebugUtils.Break();
                        }

                        charPos = pos;

                        while (pos < str.Length && str[charPos].IsCrOrLf())
                        {
                            charPos--;
                        }

                        if (removeEmpty)
                        {
                            if (!line.Trim().IsNullOrEmpty())
                            {
                                yield return new KeyValuePair<KeyValuePair<int, int>, string>(new KeyValuePair<int, int>(x, lastPos), line);
                            }
                        }
                        else
                        {
                            yield return new KeyValuePair<KeyValuePair<int, int>, string>(new KeyValuePair<int, int>(x, lastPos), line);
                        }

                        lastPos = charPos;
                        x++;
                    }
                }
            }
        }

        public static int GetCharCount(this string str, char ch)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }
            else
            {
                return str.Count(c => c == ch);
            }
        }

        public static int GetSubStringCount(this string str, string findText)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }
            else
            {
                var index = str.IndexOf(findText);
                var count = 0;

                while (index != -1)
                {
                    count++;
                    index = str.IndexOf(findText, index + 1);
                }

                return count;
            }
        }

        public static int GetLeadingCharCount(this string str, char ch)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }
            else
            {
                return str.TakeWhile(c => c == ch).Count();
            }
        }

        public static string EmptyToNull(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool ContainsAny(this string str, params string[] substring)
        {
            foreach (var sub in substring)
            {
                if (str.Contains(sub))
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetContainsCount(this string str, params string[] substring)
        {
            var count = 0;

            foreach (var sub in substring)
            {
                if (str.Contains(sub))
                {
                    count = str.GetSubStringCount(sub);
                }
            }

            return count;
        }

        public static IEnumerable<string> GetWhatsContained(this string str, params string[] substring)
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var sub in substring)
            {
                if (str.Contains(sub))
                {
                    var index = str.IndexOf(sub);

                    dictionary.Add(sub, index);
                }
            }

            return dictionary.OrderBy(p => p.Value).Select(p => p.Key);
        }

        public static string AddCrlfIfNone(this string str)
        {
            if (str.RegexIsMatch(@"\n$"))
            {
                return str;
            }
            else
            {
                return str + "\r\n";
            }
        }

        public static string IndentLines(this string str, int tabCount = 1)
        {
            return string.Join("\r\n", str.GetLines().Select(l => "\t".Repeat(tabCount) + l));
        }

        public static string IndentLinesSpaces(this string str, int spaceCount = 1)
        {
            return string.Join("\r\n", str.GetLines().Select(l => " ".Repeat(spaceCount) + l));
        }

        public static bool IsNullWhiteSpaceOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(str);
        }

        public static string GetRandomString(int length)
        {
            var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            string base64;

            provider.GetBytes(bytes);

            base64 = Convert.ToBase64String(bytes);

            return base64.RemoveEnd(base64.Length - length);
        }

        public static string AsDisplayText(this object obj, bool quoteText = false)
        {
            if (obj == null)
            {
                return "null";
            }
            else if (obj is bool)
            {
                return ((bool)obj) ? "true" : "false";
            }
            else if (obj is string && quoteText)
            {
                return "'" + ((string)obj) + "'";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static string AsDisplayText(this object obj, QuoteTextType quoteTextType)
        {
            if (obj == null)
            {
                return "null";
            }
            else if (obj is bool)
            {
                return ((bool)obj) ? "true" : "false";
            }
            else if (obj is string && quoteTextType == QuoteTextType.SingleQuote)
            {
                return "'" + ((string)obj) + "'";
            }
            else if (obj is string && quoteTextType == QuoteTextType.DoubleQuote)
            {
                return "\"" + ((string)obj) + "\"";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static string AsDisplayText(this string str)
        {
            if (str == null)
            {
                return "null";
            }
            else
            {
                return str;
            }
        }

        public static string AsDisplayText(this byte[] bytes)
        {
            if (bytes == null)
            {
                return "null";
            }
            else
            {
                string hex = BitConverter.ToString(bytes).Replace("-", ", ").SurroundWith("{ ", " }");

                return hex;
            }
        }

        public static string ToHexString(this byte b, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x2}", b);
            }
            else
            {
                return string.Format("{0:x2}", b);
            }
        }

        public static string ToHexString(this IntPtr ptr, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x8}", ptr.ToInt64());
            }
            else
            {
                return string.Format("{0:x8}", ptr.ToInt64());
            }
        }

        public static string ToHexString(this UIntPtr ptr, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x8}", ptr.ToUInt64());
            }
            else
            {
                return string.Format("{0:x8}", ptr.ToUInt64());
            }
        }

        public static string ToHexString(this uint n, bool leadingIdentifier = false)
        { 
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x8}", n);
            }
            else
            {
                return string.Format("{0:x8}", n);
            }
        }

        public static string ToHexString(this ulong n, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x8}", n);
            }
            else
            {
                return string.Format("{0:x8}", n);
            }
        }

        public static string ToHexString(this long n, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x16}", n);
            }
            else
            {
                return string.Format("{0:x16}", n);
            }
        }

        public static string ToBinString(this long n)
        {
            return Convert.ToString(n, 2);
        }

        public static string ToBinString(this int n)
        {
            return Convert.ToString(n, 2);
        }

        public static string ToHexString(this short n, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x2}", n);
            }
            else
            {
                return string.Format("{0:x2}", n);
            }
        }

        public static string ToHexString(this int n, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return string.Format("0x{0:x8}", n);
            }
            else
            {
                return string.Format("{0:x8}", n);
            }
        }

        public static long FromHexString(this string text, bool leadingIdentifier = false)
        {
            if (leadingIdentifier)
            {
                return Convert.ToInt64(text, 16);
            }
            else
            {
                return long.Parse(text, System.Globalization.NumberStyles.HexNumber);
            }
        }

        public static string AsDisplayText(this byte[] bytes, int limit)
        {
            if (bytes == null)
            {
                return "null";
            }
            else
            {
                string hex = BitConverter.ToString(bytes, 0, Math.Min(limit, bytes.Length)).Replace("-", ", ").SurroundWith("{ ", " }");

                return hex;
            }
        }

        public static string GetNullTermString(this Encoding encoding, byte[] bytes)
        {
            return encoding.GetString(bytes, 0, bytes.IndexOf<byte>(0));
        }

        public static void AppendLineFormat(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format + "\r\n", args);
        }

        public static void RemoveEnd(this StringBuilder builder, int length)
        {
            builder.Remove(builder.Length - length, length); 
        }

        public static void RemoveTrailingCommaCrlf(this StringBuilder builder)
        {
            builder.RemoveTrailingCrlf();
            builder.RemoveTrailingComma();
        }

        public static void RemoveLastTrailingComma(this StringBuilder builder)
        {
            for (var x = builder.Length - 1; x >= 0; x--)
            {
                if (builder[x].IsWhitespace())
                {
                    continue;
                }

                if (builder[x] == ',')
                {
                    builder.Remove(x, 1);
                }

                return;
            }
        }

        public static void RemoveTrailingCrlf(this StringBuilder builder)
        {
            if (builder[builder.Length - 1] == '\n')
            {
                builder.RemoveEnd(1);
            }

            if (builder[builder.Length - 1] == '\r')
            {
                builder.RemoveEnd(1);
            }
        }

        public static string RemoveTrailingCrlf(this string text)
        {
            var builder = new StringBuilder(text);

            builder.RemoveTrailingCrlf();

            return builder.ToString();
        }

        public static string ToLower(this bool boolValue)
        {
            return boolValue.ToString().ToLower();
        }

        public static void RemoveTrailingComma(this StringBuilder builder)
        {
            if (builder[builder.Length - 1] == ',')
            {
                builder.RemoveEnd(1);
            }
        }

        public static void RemoveEndIfMatches(this StringBuilder builder, string matchText)
        {
            if (builder.ToString().EndsWith(matchText))
            {
                builder.RemoveEnd(matchText.Length);
            }
        }

        public static void AppendLineFormatSpaceIndent(this StringBuilder builder, int count, string format, params object[] args)
        {
            builder.Append(" ".Repeat(count));
            builder.AppendFormat(format + "\r\n", args);
        }

        public static void AppendLineFormatTabIndent(this StringBuilder builder, int count, string format, params object[] args)
        {
            builder.Append('\t'.Repeat(count));
            builder.AppendFormat(format + "\r\n", args);
        }

        public static void AppendLineFormatTabIndent(this StringBuilder builder, string format, params object[] args)
        {
            builder.Append('\t'.Repeat(1));
            builder.AppendFormat(format + "\r\n", args);
        }

#if !SILVERLIGHT
        public static TagHandler AppendTag(this StringBuilder builder, string tag, object attributesOrValue = null)
        {
            Stack<TagHandler> stack = null;
            TagHandler handler;

            if (!tagHandlerStack.ContainsKey(builder))
            {
                stack = new Stack<TagHandler>();
                tagHandlerStack.Add(builder, stack);
            }
            else
            {
                stack = tagHandlerStack[builder];
            }

            handler = AppendTag(builder, stack.Count, tag, attributesOrValue);

            stack.Push(handler);

            handler.Disposed += (sender, e) =>
            {
                var handlerPeek = stack.Peek();

                Debug.Assert(handlerPeek.Tag == handler.Tag);

                stack.Pop();

                if (stack.Count == 0)
                {
                    tagHandlerStack.Remove(builder);
                }
            };

            return handler;
        }

        private static TagHandler AppendTag(this StringBuilder builder, int indent, string tag, object attributesOrValue = null)
        {
            if (attributesOrValue != null)
            {
                var type = attributesOrValue.GetType();

                if (type.Is<string>() || type.Is<DateTime>() || type.IsEnum || type.IsPrimitive)
                {
                    builder.AppendFormatTabIndent(indent, "<{0}>{1}", tag, attributesOrValue.ToString());
                    return new TagHandler(builder, 0, tag);
                }
                else
                {
                    builder.AppendFormatTabIndent(indent, "<{0}", tag);

                    foreach (var property in type.GetProperties())
                    {
                        var propertyName = property.Name;
                        var value = property.GetValue(attributesOrValue, null);

                        builder.AppendFormat(" {0}=\"{1}\"", propertyName, value.ToString());
                    }

                    builder.AppendLine(">");
                }
            }
            else
            {
                builder.AppendLineFormatTabIndent(indent, "<{0}>", tag);
            }

            return new TagHandler(builder, indent, tag);
        }

        public static string ReadNullTermString(this BinaryReader reader)
        {
            return ReadNullTermString(reader, IOExtensions.MAX_PATH);
        }

        public static string ReadNullTermString(this BinaryReader reader, int maxLength)
        {
            var builder = new StringBuilder();

            for (var x = 0; x < maxLength; x++)
            {
                var _byte = reader.ReadByte();

                if (_byte == 0)
                {
                    break;
                }

                builder.Append((char) _byte);
            }

            return builder.ToString();
        }

        public static string ReadNullTermStringLength(this BinaryReader reader, int maxLength)
        {
            var builder = new StringBuilder();

            for (var x = 0; x < maxLength; x++)
            {
                var _byte = reader.ReadByte();

                if (_byte != 0)
                {
                    builder.Append((char)_byte);
                }
            }

            return builder.ToString();
        }

        public static unsafe string ReadWideCharString(uint ptr)
        {
            byte[] bytes;
            var pStartingBytes = (int*)ptr;
            var pBytes = pStartingBytes;
            var length = 0;

            while (((int) pBytes) != 0)
            {
                length++;
                pBytes++;
            }

            bytes = new byte[length];

            Marshal.Copy(new IntPtr(pStartingBytes), bytes, 0, length);

            return UnicodeEncoding.Unicode.GetString(bytes);
        }


        public static string ReadWideCharString(this BinaryReader reader)
        {
            return ReadWideCharString(reader, IOExtensions.MAX_PATH);
        }

        public static unsafe string ReadWideCharString(this BinaryReader reader, int maxLength)
        {
            var bytes = new byte[0];
            var wChar = reader.ReadInt16();

            while (wChar != 0)
            {
                bytes = bytes.Append(BitConverter.GetBytes(wChar));
                wChar = reader.ReadInt16();
            }

            return UnicodeEncoding.Unicode.GetString(bytes);
        }

        public static string ReadUnicodeString(this BinaryReader reader)
        {
            var length = reader.ReadInt16();
            var bytes = reader.ReadBytes(length * 2);

            return UnicodeEncoding.Unicode.GetString(bytes);
        }

        public static string ReadNullTermFourByteAlignedString(this BinaryReader reader)
        {
            var buffer = new List<char>();
            char nextChar;

            do
            {
                nextChar = reader.ReadChar();
                buffer.Add(nextChar);
            }
            while (nextChar != '\0' || reader.BaseStream.Position % 4 != 0);

            return new string(buffer.TakeWhile(b => !b.Equals('\0')).ToArray<char>());
        }

        public static string Pluralize(this string text)
        {
            return pluralizer.Pluralize(text);
        }

        public static bool IsPlural(this string text)
        {
            return pluralizer.IsPlural(text);
        }

        public static string Singularize(this string text)
        {
            return pluralizer.Singularize(text);
        }

        public static bool IsSingular(this string text)
        {
            return pluralizer.IsSingular(text);
        }

#endif
        public static string Capitalize(this string text)
        {
            if (text.Length > 1)
            {
                return char.ToUpper(text[0]) + text.Substring(1);
            }
            else
            {
                return text.ToUpper();
            }
        }

        public static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(new string[] { separator }, options);
        }

        public static string CapitalizeWords(this string text)
        {
            if (text.Length > 1)
            {
                text = text.Replace("-", " ");

                return string.Join(" ", text.SplitToWords().Select(w => w.Capitalize()));
            }

            return text;
        }

        public static float GetCapitalizationRatio(this string text)
        {
            var capitalCount = 0;
            var count = 0;

            if (text.Length > 1)
            {
                foreach (var word in text.SplitToWords())
                {
                    if (word[0].IsUpperCase())
                    {
                        capitalCount++;
                    }

                    count++;
                }
            }

            return capitalCount / count;
        }

        public static string NumberToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static IEnumerable<char> GetNonWordCharacters(this string text)
        {
            return text.Where(c => c != ' ' && !c.IsBetween('a', 'z') && !c.IsBetween('A', 'Z'));
        }

        public static bool HasNonWordCharacters(this string text)
        {
            return text.GetNonWordCharacters().Count() > 0;
        }

        public static bool IsNonWordCharacter(this char ch)
        {
            return ch != ' ' && !ch.IsBetween('a', 'z') && !ch.IsBetween('A', 'Z');
        }

        public static bool IsWhitespace(this char ch)
        {
            return ch <= ' ';
        }

        public static string MakeIdentifier(this string text)
        {
            if (text.Length > 0)
            {
                text = text.Replace(" ", "");
                text = text.Replace("-", " ");

                text = string.Join(string.Empty, text.Where(c => c == '_' || c.IsBetween('0', '9') || c.IsBetween('a', 'z') || c.IsBetween('A', 'Z')));
            }

            if (text.Length > 0 && text[0].IsBetween('0', '9'))
            {
                text = "_" + text;
            }

            return text;
        }

        public static string RemoveNonWordCharacters(this string text, bool includeRemoveSpaces = false)
        {
            if (text.Length > 0)
            {
                if (includeRemoveSpaces)
                {
                    text = text.Replace(" ", "");
                    text = text.Replace("-", "");
                }
                else
                {
                    text = text.Replace("-", " ");
                }

                text = string.Join(string.Empty, text.Where(c => c == ' ' || c.IsBetween('a', 'z') || c.IsBetween('A', 'Z')));
            }

            return text;
        }

        public static string RemoveSpecialCharacters(this string text)
        {
            if (text.Length > 0)
            {
                text = string.Join(string.Empty, text.Where(c => c.IsBetween(' ', '~')));
            }

            return text;
        }

        public static bool HasSpecialCharacters(this string text)
        {
            if (text.Length > 0)
            {
                return text.Any(c => !c.IsBetween(' ', '~'));
            }

            return false;
        }

        public static string ReplaceSpecialCharacters(this string text)
        {
            if (text.Length > 0)
            {
                foreach (var ch in text.Where(c => !c.IsBetween(' ', '~')))
                {
                    switch (ch)
                    {
                        case '\u00ae':
                            break;
                        case '\u2013':
                            text = text.Replace(ch, '-');
                            break;
                        case '\u2019':
                            text = text.Replace(ch, '\'');
                            break;
                        case '\u2014':
                            text = text.Replace(ch, '-');
                            break;
                        case '\u201c':
                        case '\u201d':
                            text = text.Replace(ch, '"');
                            break;
                        default:
                            DebugUtils.Break();
                            break;
                    }
                }
            }

            return text;
        }

        public static bool IsUpperCase(this char ch)
        {
            return ch.IsBetween('A', 'Z') || ch.IsOneOf("!@#$%^&*()_+{}|:\"<>?~".ToCharArray());
        }

        public static string[] RemoveInsignificantWords(this string[] words, IEnumerable<string> exclusions = null)
        {
            if (words.Length > 1)
            {
                if (exclusions != null)
                {
                    return words.Where(w => !w.IsInsignificantWord(exclusions)).ToArray();
                }
                else
                {
                    return words.Where(w => !w.IsInsignificantWord()).ToArray();
                }
            }

            return new string[0];
        }

        public static string RemoveInsignificantWords(this string text, IEnumerable<string> exclusions = null)
        {
            if (text.Length > 1)
            {
                if (exclusions != null)
                {
                    return string.Join(" ", text.SplitToWords().Where(w => !w.IsInsignificantWord(exclusions)));
                }
                else
                {
                    return string.Join(" ", text.SplitToWords().Where(w => !w.IsInsignificantWord()));
                }
            }

            return text;
        }

        public static string RemoveInsignificantWords(this string text, int level, IEnumerable<string> exclusions = null)
        {
            if (text.Length > 1)
            {
                if (exclusions != null)
                {
                    return string.Join(" ", text.SplitToWords().Where(w => !w.IsInsignificantWord(level, exclusions)));
                }
                else
                {
                    return string.Join(" ", text.SplitToWords().Where(w => !w.IsInsignificantWord(level)));
                }
            }

            return text;
        }

        public static string RemoveText(this string text, string replaceWithEmpty)
        {
            if (text.Length > 1)
            {
                return text.Replace(replaceWithEmpty, string.Empty);
            }

            return text;
        }

        public static string RemoveSpaces(this string text)
        {
            return text.RemoveText(" ");
        }


        public static string LowerCase(this string text)
        {
            if (text.Length > 1)
            {
                return char.ToLower(text[0]) + text.Substring(1);
            }
            else
            {
                return text.ToLower();
            }
        }

        public static string AsFormat(this string format, params object[] parms)
        {
            return string.Format(format, parms);
        }

        public static int IndexOfFirstNonWhitespace(this string text)
        {
            var firstChar = text.ToArray<char>().FirstGreaterThan(c => c, ' ', true);

            return text.IndexOf(firstChar);
        }

        public static bool StartsWithAny(this CaselessString text, params string[] parts)
        {
            foreach (var part in parts)
            {
                if (text.StartsWith(part))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool StartsWithAny(this string text, params string[] parts)
        {
            foreach (var part in parts)
            {
                if (text.StartsWith(part))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EndsWithAny(this string text, params string[] parts)
        { 
            foreach (var part in parts)
            {
                if (text.EndsWith(part))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Expressionize(this string str)
        {
            var regex = new Regex(@"(?<expressionstart>\{)(?<symbol>@)(?<expressionend>[^}]*\})");
            var builder = new StringBuilder(str);
            var index = 0;
            var charIndex = 0;

            if (regex.IsMatch(str))
            {
                foreach (var match in regex.Matches(str).Cast<Match>())
                {
                    var symbol = match.Groups["symbol"];
                    var expressionStart = match.Groups["expressionstart"];
                    var expressionEnd = match.Groups["expressionend"];

                    charIndex = symbol.Index;

                    builder.Replace("@", index.ToString(), charIndex, 1);

                    charIndex += symbol.Length + expressionEnd.Length;
                    index++;
                }
            }

            return builder.ToString();
        }

        public static void AppendSpaceIndent(this StringBuilder builder, int count, string text)
        {
            builder.Append(" ".Repeat(count));
            builder.Append(text);
        }

        public static void AppendFormatSpaceIndent(this StringBuilder builder, int count, string format, params object[] args)
        {
            builder.Append(" ".Repeat(count));
            builder.AppendFormat(format, args);
        }

        public static void AppendFormatSpaceIndent(this StringBuilder builder, string leadIfLength, int count, string format, params object[] args)
        {
            if (builder.Length > 0)
            {
                builder.Append(leadIfLength);
            }

            builder.Append(" ".Repeat(count));
            builder.AppendFormat(format, args);
        }

        public static void AppendWithLeadingIfLength(this StringBuilder builder, string lead, string format, params object[] args)
        {
            if (builder.Length > 0)
            {
                builder.Append(lead);
            }

            builder.AppendFormat(format, args);
        }

        public static void AppendLineIfLength(this StringBuilder builder, string text)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine(text);
            }
        }

        public static void AppendLineIfLength(this StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }
        }

        public static void AppendIfLength(this StringBuilder builder, string text)
        {
            if (builder.Length > 0)
            {
                builder.Append(text);
            }
        }

        public static void AppendLineWithLeadingIfLength(this StringBuilder builder, string lead, string format, params object[] args)
        {
            if (builder.Length > 0)
            {
                builder.Append(lead);
            }

            builder.AppendLineFormat(format, args);
        }

        public static void AppendTabIndent(this StringBuilder builder, int count, string text)
        {
            builder.Append('\t'.Repeat(count));
            builder.Append(text);
        }

        public static void AppendFormatTabIndent(this StringBuilder builder, int count, string format, params object[] args)
        {
            builder.Append('\t'.Repeat(count));
            builder.AppendFormat(format, args);
        }

        public static void AppendLineSpaceIndent(this StringBuilder builder, int count, string text)
        {
            builder.Append(" ".Repeat(count));
            builder.AppendLine(text);
        }

        public static void AppendLineTabIndent(this StringBuilder builder, int count, string text)
        {
            builder.Append('\t'.Repeat(count));
            builder.AppendLine(text);
        }

        public static void WriteSpaceIndent(this ILogWriter writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatSpaceIndent(this ILogWriter writer, int count, string format, params object[] args)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteTabIndent(this ILogWriter writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatTabIndent(this ILogWriter writer, int count, string format, params object[] args)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteLineSpaceIndent(this ILogWriter writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.WriteLine(text);
        }

        public static void WriteLineTabIndent(this ILogWriter writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.WriteLine(text);
        }

#if NET8_0_OR_GREATER
        public static void WriteSpaceIndent(this TextTransformation writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatSpaceIndent(this TextTransformation writer, int count, string format, params object[] args)
        {
            writer.Write(" ".Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteLineFormatSpaceIndent(this TextTransformation writer, int count, string format, params object[] args)
        {
            writer.Write(" ".Repeat(count));
            writer.WriteLine(format, args);
        }

        public static void WriteTabIndent(this TextTransformation writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(text);
        }

        public static void WriteFormatTabIndent(this TextTransformation writer, int count, string format, params object[] args)
        {
            writer.Write('\t'.Repeat(count));
            writer.Write(format, args);
        }

        public static void WriteLineSpaceIndent(this TextTransformation writer, int count, string text)
        {
            writer.Write(" ".Repeat(count));
            writer.WriteLine(text);
        }

        public static void WriteLineTabIndent(this TextTransformation writer, int count, string text)
        {
            writer.Write('\t'.Repeat(count));
            writer.WriteLine(text);
        }
#endif
        public static void PrependLineWithLeadingIfLength(this StringBuilder builder, string lead, string format, params object[] args)
        {
            var length = builder.Length;

            builder.InsertFormat(0, format, args);

            if (length > 0)
            {
                builder.Insert(0, lead);
            }
        }

        public static void InsertLineAtCharIndex(this StringBuilder builder, int index)
        {
            builder.Insert(index, "\r\n");
        }

        public static void InsertLineAtLineNumber(this StringBuilder builder, int index)
        {
            var lines = builder.ToString().GetLinesWithLineNumbersAndCharIndexes();
            var line = lines.ElementAt(index - 1);

            builder.Insert(line.Key.Value, "\r\n");
        }

        public static void InsertLineFormatTabIndent(this StringBuilder builder, int index, int count, string format, params object[] args)
        {
            var builderTemp = new StringBuilder();

            builderTemp.AppendLineFormatTabIndent(count, format, args);

            builder.Insert(index, builderTemp.ToString());
        }

        public static void InsertFormat(this StringBuilder builder, int index, string format, params object[] args)
        {
            var text = string.Format(format, args);

            builder.Insert(index, text);
        }

        public static string Repeat(this string value, int count)
        {
            if (value.Length == 1)
            {
                return Repeat(value[0], count);
            }
            else
            {
                var builder = new StringBuilder();

                count.Countdown(n =>
                {
                    builder.Append(value);
                });

                return builder.ToString();
            }
        }

        public static string Repeat(this char value, int count)
        {
            return new string(value, count);
        }

        public static CaselessString AsCaseless(this string str)
        {
            return str;
        }

        public static string PluralizeWords(this string str)
        {
            return string.Join(" ", Regex.Split(str, @"[\s\n\-]").Where(f => f.Trim().Length > 0).Select(s => s.Pluralize()));
        }

        public static string WordsToCamelCase(this string str)
        {
            return string.Join(" ", Regex.Split(str, @"[\s\n\-]").Where(f => f.Trim().Length > 0).Select(s => s.ToCamelCase()));
        }

        public static string WordsToTitleCase(this string str)
        {
            return string.Join(" ", Regex.Split(str, @"[\s\n\-]").Where(f => f.Trim().Length > 0).Select(s => s.ToTitleCase()));
        }

        public static int GetWordCount(this string str, bool removeNonWordCharacters = true)
        {
            var words = str.SplitToWords(removeNonWordCharacters);

            return words.Length;
        }

        public static string[] SplitToWords(this string str, bool removeNonWordCharacters = true)
        {
            if (removeNonWordCharacters)
            {
                str = str.Replace("-", " ");

                if (removeNonWordCharacters)
                {
                    str = str.RemoveNonWordCharacters();
                }
            }

            return Regex.Split(str, @"[\s\n\-]").Where(f => f.Trim().Length > 0).ToArray();
        }

        public static string[] SplitUriPartToWords(this string uriPart)
        {
            return uriPart.Split(".").SelectMany(s => s.Split("/")).SelectMany(s => s.Split("-")).Select(s => HttpUtility.UrlDecode(s)).SelectMany(s => s.SplitTitleCaseIdentifierToWords()).ToArray();
        }

        public static string[] SplitToWords(this Uri uri)
        {
            var words = new List<string>();
            var nameValues = HttpUtility.ParseQueryString(uri.Query);
            var segments = uri.Segments.Select(s => s.RemoveEndIfMatches("/")).Where(s => !s.IsNullOrEmpty());

            words.Add(uri.Host);
            words.AddRange(uri.Host.SplitUriPartToWords());
            words.AddRange(segments);
            words.AddRange(segments.SelectMany(s => s.SplitUriPartToWords()));
            words.AddRange(nameValues.Keys.Cast<string>().SelectMany(s => s.SplitUriPartToWords()));

            return words.Distinct().ToArray();
        }

        public static string RemoveLastWord(this string str)
        {
            var words = str.SplitToWords(false).ToList();

            words.RemoveAt(words.Count - 1);

            if (words.Count > 0)
            {
                var lastWord = words.Last();
                var lastChar = lastWord.Last();

                while (lastChar.IsNonWordCharacter() || lastChar == ' ')
                {
                    lastWord = lastWord.RemoveEnd(1);

                    words[words.Count - 1] = lastWord;

                    if (lastWord.Length > 0)
                    {
                        lastChar = lastWord.Last();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return string.Join(" ", words);
        }

        public static string RemoveFirstWord(this string str)
        {
            var words = str.SplitToWords(false).ToList();

            words.RemoveAt(0);

            if (words.Count > 0)
            {
                var firstWord = words.First();
                var firstChar = firstWord.First();

                while (firstChar.IsNonWordCharacter() || firstChar == ' ')
                {
                    firstWord = firstWord.RemoveStart(1);
                    words[0] = firstWord;

                    if (firstWord.Length > 0)
                    {
                        firstChar = firstWord.First();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return string.Join(" ", words);
        }
        public static string GetLastWord(this string str)
        {
            var words = str.SplitToWords(false).ToList();

            return words.Last();
        }

        public static string GetFirstWord(this string str)
        {
            var words = str.SplitToWords(false).ToList();

            return words.First();
        }

        public static string[] SplitIdentifierToWords(this string str)
        {
            string[] words;
            var startsWithUnderscore = str.First() == '_';

            str = str.Replace("-", " ").Replace("_", " ");

            if (str.IsAllUpper())
            {
                words = Regex.Split(str, @"(?=[ ])").Where(f => f.Trim().Length > 0).Select(w => w.Trim().ToLower().ToTitleCase()).ToArray();
            }
            else
            {
                words = Regex.Split(str, @"(?=[A-Z0-9 ])").Where(f => f.Trim().Length > 0).ToArray();
            }

            if (startsWithUnderscore)
            {
                words[0] = "_" + words[0];
            }

            return words;
        }

        public static bool IsAllUpper(this string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] SplitTitleCaseIdentifierToWords(this string str)
        {
            if (str.IsAllUpper())
            {
                var words = str.SplitIdentifierToWords().Select(w => w.ToTitleCase());

                return words.ToArray();
            }

            str = str.Replace("-", " ").Replace("_", " ");

            return Regex.Split(str, @"(?=[A-Z0-9])").Where(f => f.Trim().Length > 0).ToArray();
        }

        public static bool EndsWith(this CaselessString str, string value)
        {
            return str.String.EndsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool Contains(this CaselessString str, string value)
        {
            return str.String.Contains(value, StringComparison.CurrentCultureIgnoreCase); 
        }

        public static CaselessString Replace(this CaselessString str, string oldValue, string newValue)
        {
            return str.String.Replace(oldValue, newValue);
        }

        public static bool StartsWith(this CaselessString str, string value)
        {
            return str.String.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string UriEncode(this string str)
        {
            return Uri.EscapeUriString(str);
        }

        public static string UriDecode(this string str)
        {
            return Uri.UnescapeDataString(str);
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlEncodeWithBreaks(this string str)
        {
            str = HttpUtility.HtmlEncode(str);
            str = str.RegexReplace(@"\n", "<br>");

            return str;
        }

        public static string HtmlEncodeWithWhitespace(this string str, int spaceCountPerTab = 2)
        {
            str = HttpUtility.HtmlEncode(str);
            str = str.RegexReplace(@"\t", "&nbsp;".Repeat(spaceCountPerTab));
            str = str.RegexReplace(@" ", "&nbsp;");

            return str;
        }

        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static XDocument ParseAsXML(this string str)
        {
            return XDocument.Parse(str);
        }

        public static IEnumerable<string> GetAllWordGroups(this string text)
        {
            var words = text.SplitToWords(false).ToList();
            var maxGroups = words.Count;

            if (text.Contains("-"))
            {
                yield return text;
            }

            for (var groupCount = 1; groupCount <= maxGroups; groupCount++)
            {
                for (var index = 0; index < words.Count - groupCount + 1; index++)
                {
                    var group = words.Skip(index).Take(groupCount);

                    yield return string.Join(" ", group);
                }
            }
        }

        public static IEnumerable<string> GetAllWordGroups(this Uri uri)
        {
            var words = uri.SplitToWords();

            return words;
        }

        public static object ParseAsJson(this string str)
        {
            return JsonExtensions.ReadJson<object>(str);
        }

        public static string RegexEscape(this string str)
        {
            return Regex.Escape(str);
        }

        public static string FormatEscape(this string str)
        {
            return str.RegexReplace(@"(?<!\{)\{(?!\{)", "{{").RegexReplace(@"(?<!\})\}(?!\})", "}}");
        }

        public static string RegexGetMultiline(this string str, string pattern, string groupName)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline);

            if (regex.IsMatch(str))
            {
                var match = regex.Match(str);
                var value = match.Groups[groupName].Value;

                return value;
            }

            return null;
        }

        public static string RegexGet(this string str, string pattern)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                var match = regex.Match(str);

                return match.Value;
            }

            return null;
        }

        public static string RegexGet(this string str, string pattern, string groupName)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                var match = regex.Match(str);
                var value = match.Groups[groupName].Value;

                return value;
            }

            return null;
        }

        public static string[] RegexGetMultiple(this string str, string pattern, string groupName)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                var matches = regex.Matches(str);

                return matches.Cast<Match>().Select(c => c.Groups[groupName].Value).ToArray();
            }

            return null;
        }

        public static string RemoveNewLineAndCarraigeReturns(this string str)
        {
            return str.RegexRemove(@"\n");
        }

        public static string ReplaceInnerQuotesToSingle(this string str)
        {
            return str.RegexReplace("(?<!^)\"(?!$)", "'");
        }

        public static string SurroundWithQuotesIfNeeded(this string str)
        {
            if (!str.StartsWith("\""))
            {
                str = str.Prepend("\"");
            }

            if (!str.EndsWith("\""))
            {
                str = str.Append("\"");
            }

            return str;
        }

        public static Match RegexGetMatch(this string str, string pattern)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                return regex.Match(str);
            }

            return null;
        }

        public static string Join(this IEnumerable<string> list, string stringSeparator = null)
        {
            return string.Join(stringSeparator ?? "", list);
        }

        public static IEnumerable<Match> GetAttributeStringExpressionMatches(this string str)
        {
            return str.RegexGetMatches(@"\{\s*(?<expression>" + StringExtensions.REGEX_IDENTIFIER_MIDSTRING + @")\s*\}");
        }

        public static IEnumerable<Match> RegexGetMatches(this string str, string pattern)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                var matches = regex.Matches(str);

                return matches.Cast<Match>();
            }

            return new Match[0];
        }

        public static string[] RegexSplit(this string str, string pattern)
        {
            var parts = new List<string>();

            if (Regex.IsMatch(str, pattern))
            {
                string fragment;
                var matches = Regex.Matches(str, pattern);
                var lastIndex = 0;

                foreach (Match match in matches)
                {
                    var splitIndex = lastIndex == 0 ? lastIndex : lastIndex + 1;

                    if (match.Index > 0)
                    {
                        fragment = str.Substring(splitIndex, match.Index - splitIndex);
                        parts.Add(fragment);
                    }

                    lastIndex = match.Index;
                }

                if (lastIndex != 0)
                {
                    fragment = str.Substring(lastIndex + 1, str.Length - (lastIndex + 1));
                    parts.Add(fragment);
                }
            }
            else if (str.Length > 0)
            {
                parts.Add(str);
            }

            return parts.ToArray();
        }

        public static bool IsWildcardMatch(this string str, string pattern)
        {
            bool result;
            var regexPattern = Regex.Escape(pattern);

            regexPattern = regexPattern.Replace(@"\*", ".*?");
            regexPattern = regexPattern.Replace(@"\?", ".");

            result = Regex.IsMatch(str, regexPattern, RegexOptions.IgnoreCase);

            return result;
        }

        public static string StripNumberedPrefix(this string str)
        {
            if (str.RegexIsMatch(@"\d*?\. (?<item>.*)"))
            {
                return str.RegexGet(@"\d *?\. (?<item>.*)", "item");
            }

            return str;
        }

        public static bool RegexIsMatch(this StringBuilder builder, string pattern)
        {
            return Regex.IsMatch(builder.ToString(), pattern);
        }

        public static bool RegexIsMatch(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }

        public static bool RegexIsMatchMultiline(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern, RegexOptions.Singleline | RegexOptions.Multiline);
        }

        public static string RemoveTrailingNulls(this string str)
        {
            while (str[str.Length - 1] == '\0')
            {
                str = str.RemoveEnd(1);
            }

            return str;
        }

        public static string RemoveTrailingWhitespace(this string str)
        {
            while (str[str.Length - 1] == '\r' || str[str.Length - 1] == '\n')
            {
                str = str.RemoveEnd(1);
            }

            return str;
        }

        public static string RegexRemove(this string str, string pattern)
        {
            return Regex.Replace(str, pattern, string.Empty);
        }

        public static string RegexReplace(this string str, string pattern, string replace)
        {
            return Regex.Replace(str, pattern, replace);
        }

        public static string RegexInsertAfter(this string str, string pattern, string insert)
        {
            var regex = new Regex(pattern);

            if (regex.IsMatch(str))
            {
                var match = regex.Match(str);

                return str.Insert(match.Index + match.Length, insert);
            }

            return str;
        }

        public static string RemoveEnclosingTag(this string str, string tag)
        {
            return str.RegexRemove(@"\< " + tag + @"[^>]*\>")
                .RegexRemove(@"\</" + tag + @"\>");
        }

        public static string EncloseWithTag(this string str, string tag)
        {
            return str
                .Prepend("<" + tag + ">")
                .Append("</" + tag + ">");
        }

        public static string SurroundWith(this string str, string pre, string post)
        {
            return str
                .Prepend(pre)
                .Append(post);
        }

        public static string SurroundWithTag(this string str, string tagName)
        {
            return str.SurroundWith($"<{ tagName }>", $"</{ tagName }>");
        }

        public static string SurroundWithClass(this string str, string className)
        {
            return str.SurroundWith($"public class { className }\r\n" + "{", "\r\n}");
        }


        public static string SurroundWithIfNotNullOrEmpty(this string str, string pre, string post)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else
            {
                return str
                    .Prepend(pre)
                    .Append(post);
            }
        }

        public static string SurroundWithQuotes(this string str)
        {
            return str.SurroundWith("\"");
        }

        public static string SurroundWithSingleQuotes(this string str)
        {
            return str.SurroundWith("'");
        }

        public static string SurroundWithSlashedQuotes(this string str)
        {
            return str.SurroundWith("\"");
        }

        public static string SurroundWith(this string str, string text)
        {
            return str
                .Prepend(text)
                .Append(text);
        }

        public static XDocument ParseAsXML(this string str, string enclosingTag)
        {
            return XDocument.Parse(str
                .Prepend("<" + enclosingTag + ">")
                .Append("</" + enclosingTag + ">"));
        }

        public static string Append(this string str, string suffix)
        {
            return str + suffix;
        }

        public static string AppendFormat(this string str, string suffix, params object[] args)
        {
            suffix = string.Format(suffix, args);

            return str + suffix;
        }

        public static string SplitJoin(this string str, string separator, Func<string, string> select)
        {
            return string.Join(separator, str.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(select));
        }

        public static string SplitJoin(this string str, string separator, string splitSeparator)
        {
            return string.Join(separator, str.Split(new string[] { splitSeparator }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string AppendIfMissing(this string str, string suffix)
        {
            if (!str.AsCaseless().EndsWith(suffix))
            {
                return str + suffix;
            }
            else
            {
                return str;
            }
        }

        public static string AppendIf(this string str, string suffix, bool condition)
        {
            if (condition)
            {
                return str + suffix;
            }
            else
            {
                return str;
            }
        }

        public static string AppendCrlf(this string str)
        {
            return str.Append("\r\n");
        }

        public static string AppendIfNotNullOrEmpty(this string str, string suffix)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str + suffix;
            }
            else
            {
                return str;
            }
        }

        public static string AppendIfNotNullOrEmpty(this string str, string prefix, string suffix)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str + prefix + suffix;
            }
            else
            {
                return str;
            }
        }

        public static string Append(this string str, string delimiter, string suffix)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str + suffix;
            }
            else
            {
                return str + delimiter + suffix;
            }
        }

        public static string Prepend(this string str, string prefix)
        {
            return prefix + str;
        }

        public static string PrependIfMissing(this string str, string prefix)
        {
            if (!str.AsCaseless().StartsWith(prefix))
            {
                return prefix + str;
            }
            else
            {
                return str;
            }
        }

        public static string PrependIf(this string str, string prefix, bool condition)
        {
            if (condition)
            {
                return prefix + str;
            }
            else
            {
                return str;
            }
        }

        public static string PrependIfNotNullOrEmpty(this string str, string prefix)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return prefix + str;
            }
            else
            {
                return str;
            }
        }

        public static string PrependIfPrefixNotNullOrEmpty(this string str, string prefix)
        {
            if (!prefix.IsNullOrEmpty())
            {
                return prefix + str;
            }
            else
            {
                return str;
            }
        }

        public static string GetString(string str, params object[] parms)
        {
            return str + ", Params:" + parms.ToCommaDelimitedList();
        }

        public static string GetString(string str)
        {
            return str;
        }

        internal static string GetObject(string p)
        {
            return p;
        }

        public static string RemoveQuotes(this string text)
        {
            if (text.StartsWith("\"") || text.StartsWith("'") || text.StartsWith("“"))
            {
                text = text.Remove(0, 1);
            }

            if (text.EndsWith("\"") || text.EndsWith("'") || text.EndsWith("”"))
            {
                text = text.Remove(text.Length - 1);
            }

            return text;
        }

        public static string MakeUserFriendly(this string text)
        {
            text = string.Join(string.Empty, text.Select(c =>
            {
                if (c > 64 && c < 90)
                {
                    return " " + c.ToString();
                }
                else
                {
                    return c.ToString();
                }
            }));

            return text;
        }

        public static string ReverseSlashes(this string text)
        {
            if (text.Contains("\\"))
            {
                text = text.Replace("\\", "/");
            }
            else if (text.Contains("/"))
            {
                text = text.Replace("/", "\\");
            }

            return text;
        }

        public static string ForwardSlashes(this string text)
        {
            if (text.Contains("\\"))
            {
                text = text.Replace("\\", "/");
            }

            return text;
        }

        public static string BackSlashes(this string text)
        {
            if (text.Contains("/"))
            {
                text = text.Replace("/", "\\");
            }

            return text;
        }

        public static string RemoveSurroundingSlashes(this string text)
        {
            if (text.StartsWith("\\") || text.StartsWith("/"))
            {
                text = text.Remove(0, 1);
            }

            if (text.EndsWith("\\") || text.EndsWith("/"))
            {
                text = text.Remove(text.Length - 1);
            }

            return text;
        }

        public static string RemoveSurrounding(this string text, string surrounding)
        {
            if (text.StartsWith(surrounding))
            {
                text = text.Remove(0, 1);
            }

            if (text.EndsWith(surrounding))
            {
                text = text.Remove(text.Length - 1);
            }

            return text;
        }

        public static string RemoveSurrounding(this string text, string start, string end)
        {
            if (text.StartsWith(start))
            {
                text = text.Remove(0, 1);
            }

            if (text.EndsWith(end))
            {
                text = text.Remove(text.Length - 1);
            }

            return text;
        }

        public static string ToKebabCase(this string name)
        {
            var builder = new StringBuilder();
            var identifier = string.Empty;

            foreach (var ch in name)
            {
                if (ch.IsUpperCase())
                {
                    builder.Append("-");
                }

                builder.Append(ch.ToString().ToLower());
            }

            identifier = builder.ToString();

            return identifier;
        }

        public static IEnumerable<string> SplitTitleCaseWords(this string name)
        {
            var builder = new StringBuilder();

            foreach (var ch in name)
            {
                if (ch.IsUpperCase())
                {
                    if (builder.Length > 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                }

                builder.Append(ch.ToString().ToLower());
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }

        public static string ToIdentifier(this string name)
        {
            var builder = new StringBuilder();
            var identifier = string.Empty;
            var goUpper = false;

            foreach (var ch in name)
            {
                var ch2 = ch.ToString();

                if (ch2 == "-")
                {
                    goUpper = true;
                    continue;
                }
                else
                {
                    if (goUpper)
                    {
                        ch2 = ch2.ToUpper();
                        goUpper = false;
                    }
                }

                builder.Append(ch2.ToString());
            }

            identifier = builder.ToString();

            return identifier;
        }

        public static string ToCamelCase(this string text)
        {
            return text.First().ToString().ToLower() + string.Join("", text.Skip(1));
        }

        public static string ToTitleCase(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            return text.First().ToString().ToUpper() + string.Join("", text.Skip(1));
        }

        public static string ToNonTitleCase(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            return text.First().ToString().ToLower() + string.Join("", text.Skip(1));
        }

        public static string RemoveEnd(this string text, int count)
        {
            return text.Remove(text.Length - count);
        }

        public static string RemoveBOM(this string text)
        {
            if (text.StartsWith(BytOrderMarkUtf8))
            {
                text = text.RemoveStart(BytOrderMarkUtf8.Length);
            }

            return text;
        }

        public static bool RegexHasMatch(this IEnumerable<string> inputItems, string pattern, RegexOptions regexOptions)
        {
            return inputItems.Any(i => Regex.IsMatch(i, pattern, regexOptions));
        }

        public static bool RegexHasMatch(this IDictionary<string, string> inputItems, string pattern, RegexOptions regexOptions)
        {
            return inputItems.Any(i => Regex.IsMatch(i.Key, pattern, regexOptions) ||  Regex.IsMatch(i.Value, pattern, regexOptions));
        }

        public static bool RegexHasMatch(this KeyValuePair<string, string> pair, string pattern, RegexOptions regexOptions)
        {
            return Regex.IsMatch(pair.Key, pattern, regexOptions) ||  Regex.IsMatch(pair.Value, pattern, regexOptions);
        }

        public static bool HasBOM(this string text)
        {
            return text.StartsWith(BytOrderMarkUtf8);
        }

        public static string RemoveStartAtLastChar(this string text, char lastChar)
        {
            if (text.IndexOf(lastChar) != -1)
            {
                return text.RightAt(text.LastIndexOf(lastChar));
            }
            else
            {
                return text;
            }
        }

        public static string RemoveStartAfterLastChar(this string text, char lastChar)
        {
            if (text.IndexOf(lastChar) != -1)
            {
                return text.RightAfter(text.LastIndexOf(lastChar));
            }
            else
            {
                return text;
            }
        }

        public static string RemoveEndAfterLastChar(this string text, char lastChar)
        {
            if (text.IndexOf(lastChar) != -1)
            {
                return text.Crop(text.LastIndexOf(lastChar) + 1);
            }
            else
            {
                return text;
            }
        }

        public static string RemoveEndAtLastChar(this string text, char lastChar)
        {
            if (text.IndexOf(lastChar) != -1)
            {
                return text.Crop(text.LastIndexOf(lastChar));
            }
            else
            {
                return text;
            }
        }

        public static string RemoveEndAtFirstChar(this string text, char firstChar)
        {
            if (text.IndexOf(firstChar) != -1)
            {
                return text.Crop(text.IndexOf(firstChar));
            }
            else
            {
                return text;
            }
        }

        public static string RemoveEndAfterFirstChar(this string text, char firstChar)
        {
            if (text.IndexOf(firstChar) != -1)
            {
                return text.Crop(text.IndexOf(firstChar) + 1);
            }
            else
            {
                return text;
            }
        }

        public static string Crop(this string text, string lengthOfString)
        {
            return text.Crop(lengthOfString.Length);
        }

        public static string Crop(this string text, int lengthToTruncateTo, bool useEllipsis = false)
        {
            if (text.Length > lengthToTruncateTo)
            {
                if (useEllipsis)
                {
                    return text.RemoveEnd(text.Length - lengthToTruncateTo + 3) + "...";
                }
                else
                {
                    return text.RemoveEnd(text.Length - lengthToTruncateTo);
                }
            } 
            else
            {
                return text;
            }
        }

        public static string Crop(this string text, string lengthOfString, bool useEllipsis = false)
        {
            return text.Crop(lengthOfString.Length, useEllipsis);
        }

        public static string Crop(this string text, int lengthToTruncateTo)
        {
            if (text.Length > lengthToTruncateTo)
            {
                return text.RemoveEnd(text.Length - lengthToTruncateTo);
            }
            else
            {
                return text;
            }
        }

        public static bool StartsWith(this string text, params string[] args)
        {
            foreach (var arg in args)
            {
                if (text.StartsWith(arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Substitute(this string text, int insertionPoint, int length, string substitute)
        {
            var left = text.Left(insertionPoint);
            var right = text.Right(text.Length - (insertionPoint + length));

            return left + substitute + right;
        }

        public static string Left(this string text, int count)
        {
            if (text.Length > count)
            {
                return text.Substring(0, count);
            }
            else
            {
                return text;
            }
        }

        public static string Right(this string text, int count)
        {
            if (text.Length > count)
            {
                return text.Substring(text.Length - count, count);
            }
            else
            {
                return text;
            }
        }

        public static string RightAtLastIndexOf(this string text, char ch)
        {
            if (text.Contains(ch))
            {
                return text.Right(text.Length - text.LastIndexOf(ch) - 1);
            }
            else
            {
                return text;
            }
        }

        public static string RightAt(this string text, int index)
        {
            return text.Right(text.Length - index);
        }

        public static string RightAfter(this string text, int index)
        {
            if (text.Length <= index)
            {
                return string.Empty;
            }

            return text.Right(text.Length - index - 1);
        }

        public static string RightAt(this string text, bool reverse, Func<int, char, bool> charProcessor)
        {
            if (reverse)
            {
                var length = text.Length;
                var x = text.Length;

                foreach (var ch in text.Reverse())
                {
                    if (charProcessor(x, ch))
                    {
                        return text.Right(length - x);
                    }

                    x--;
                }
            }
            else
            {
                var x = 0;

                foreach (var ch in text)
                {
                    if (charProcessor(x, ch))
                    {
                        return text.Right(x);
                    }

                    x++;
                }
            }

            return text;
        }

        public static string LeftUpToLastIndexOf(this string text, char ch)
        {
            if (text.Contains(ch))
            {
                return text.Left(text.LastIndexOf(ch));
            }
            else
            {
                return text;
            }
        }

        public static string LeftUpToIndexOf(this string text, char ch)
        {
            if (text.Contains(ch))
            {
                return text.Left(text.IndexOf(ch));
            }
            else
            {
                return text;
            }
        }

        public static string RemoveStart(this string text, int count)
        {
            return text.Remove(0, count);
        }

        public static string RemoveStartEnd(this string text, int count)
        {
            return text.RemoveEnd(count).RemoveStart(count);
        }

        public static string RemoveEnd(this string text, string lengthOfString)
        {
            return text.Remove(text.Length - lengthOfString.Length);
        }

        public static string RemoveStartEnd(this string text, string lengthOfString)
        {
            var count = text.Length - lengthOfString.Length;

            return text.RemoveEnd(count).RemoveStart(count);
        }

        public static string RemoveStart(this string text, string lengthOfString)
        {
            return text.Remove(0, lengthOfString.Length);
        }

        public static string RemoveStartAfter(this string text, string remove)
        {
            var index = text.IndexOf(remove);

            return text.Remove(0, index);
        }

        public static string RemoveStartBefore(this string text, string remove)
        {
            var index = text.IndexOf(remove);

            return text.Remove(0, index - 1);
        }


        public static string RemoveStartAfter(this CaselessString caselessString, string remove)
        {
            var index = caselessString.String.ToLower().IndexOf(remove.ToLower());

            return caselessString.String.RightAfter(index + remove.Length);
        }

        public static string RemoveStartIfMatches(this string text, string startString)
        {
            if (text.StartsWith(startString))
            {
                return text.Remove(0, startString.Length);
            }
            else
            {
                return text;
            }
        }

        public static string RemoveEndIfMatches(this string text, params string[] endStrings)
        {
            foreach (var endString in endStrings)
            {
                if (text.EndsWith(endString))
                {
                    text = text.Remove(text.Length - endString.Length);
                }
            }

            return text;
        }

        public static string RemoveEndIfMatches(this string text, string endString)
        {
            if (text.EndsWith(endString))
            {
                return text.Remove(text.Length - endString.Length);
            }
            else
            {
                return text;
            }
        }
    }
}
