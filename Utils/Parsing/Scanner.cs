using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing
{
    public class Scanner
    {
        private string text;
        private int length;
        private int pos;
        private int startPos;
        private SyntaxKind token;
        private string tokenValue;
        private bool precedingLineBreak;
        private bool hasExtendedUnicodeEscape;
        private bool tokenIsUnterminated;
        private int end;
        private bool skipTrivia;
        private int tokenPos;
        private Dictionary<string, SyntaxKind> textToToken = new Dictionary<string,SyntaxKind>
        {
           { "abstract", SyntaxKind.AbstractKeyword },
           { "any", SyntaxKind.AnyKeyword },
           { "as", SyntaxKind.AsKeyword },
           { "boolean", SyntaxKind.BooleanKeyword },
           { "break", SyntaxKind.BreakKeyword },
           { "case", SyntaxKind.CaseKeyword },
           { "catch", SyntaxKind.CatchKeyword },
           { "class", SyntaxKind.ClassKeyword },
           { "continue", SyntaxKind.ContinueKeyword },
           { "const", SyntaxKind.ConstKeyword },
           { "constructor", SyntaxKind.ConstructorKeyword },
           { "debugger", SyntaxKind.DebuggerKeyword },
           { "declare", SyntaxKind.DeclareKeyword },
           { "default", SyntaxKind.DefaultKeyword },
           { "delete", SyntaxKind.DeleteKeyword },
           { "do", SyntaxKind.DoKeyword },
           { "else", SyntaxKind.ElseKeyword },
           { "enum", SyntaxKind.EnumKeyword },
           { "export", SyntaxKind.ExportKeyword },
           { "extends", SyntaxKind.ExtendsKeyword },
           { "false", SyntaxKind.FalseKeyword },
           { "finally", SyntaxKind.FinallyKeyword },
           { "for", SyntaxKind.ForKeyword },
           { "from", SyntaxKind.FromKeyword },
           { "function", SyntaxKind.FunctionKeyword },
           { "get", SyntaxKind.GetKeyword },
           { "if", SyntaxKind.IfKeyword },
           { "implements", SyntaxKind.ImplementsKeyword },
           { "import", SyntaxKind.ImportKeyword },
           { "in", SyntaxKind.InKeyword },
           { "instanceof", SyntaxKind.InstanceOfKeyword },
           { "interface", SyntaxKind.InterfaceKeyword },
           { "is", SyntaxKind.IsKeyword },
           { "keyof", SyntaxKind.KeyOfKeyword },
           { "let", SyntaxKind.LetKeyword },
           { "module", SyntaxKind.ModuleKeyword },
           { "namespace", SyntaxKind.NamespaceKeyword },
           { "never", SyntaxKind.NeverKeyword },
           { "new", SyntaxKind.NewKeyword },
           { "null", SyntaxKind.NullKeyword },
           { "number", SyntaxKind.NumberKeyword },
           { "package", SyntaxKind.PackageKeyword },
           { "private", SyntaxKind.PrivateKeyword },
           { "protected", SyntaxKind.ProtectedKeyword },
           { "public", SyntaxKind.PublicKeyword },
           { "readonly", SyntaxKind.ReadonlyKeyword },
           { "require", SyntaxKind.RequireKeyword },
           { "global", SyntaxKind.GlobalKeyword },
           { "return", SyntaxKind.ReturnKeyword },
           { "set", SyntaxKind.SetKeyword },
           { "static", SyntaxKind.StaticKeyword },
           { "string", SyntaxKind.StringKeyword },
           { "super", SyntaxKind.SuperKeyword },
           { "switch", SyntaxKind.SwitchKeyword },
           { "symbol", SyntaxKind.SymbolKeyword },
           { "this", SyntaxKind.ThisKeyword },
           { "throw", SyntaxKind.ThrowKeyword },
           { "true", SyntaxKind.TrueKeyword },
           { "try", SyntaxKind.TryKeyword },
           { "type", SyntaxKind.TypeKeyword },
           { "typeof", SyntaxKind.TypeOfKeyword },
           { "undefined", SyntaxKind.UndefinedKeyword },
           { "var", SyntaxKind.VarKeyword },
           { "void", SyntaxKind.VoidKeyword },
           { "while", SyntaxKind.WhileKeyword },
           { "with", SyntaxKind.WithKeyword },
           { "yield", SyntaxKind.YieldKeyword },
           { "async", SyntaxKind.AsyncKeyword },
           { "await", SyntaxKind.AwaitKeyword },
           { "of", SyntaxKind.OfKeyword },
           { "{", SyntaxKind.OpenBraceToken },
           { "}", SyntaxKind.CloseBraceToken },
           { "(", SyntaxKind.OpenParenToken },
           { ")", SyntaxKind.CloseParenToken },
           { "[", SyntaxKind.OpenBracketToken },
           { "]", SyntaxKind.CloseBracketToken },
           { ".", SyntaxKind.DotToken },
           { "...", SyntaxKind.DotDotDotToken },
           { ";", SyntaxKind.SemicolonToken },
           { " },", SyntaxKind.CommaToken },
           { "<", SyntaxKind.LessThanToken },
           { ">", SyntaxKind.GreaterThanToken },
           { "<=", SyntaxKind.LessThanEqualsToken },
           { ">=", SyntaxKind.GreaterThanEqualsToken },
           { "==", SyntaxKind.EqualsEqualsToken },
           { "!=", SyntaxKind.ExclamationEqualsToken },
           { "===", SyntaxKind.EqualsEqualsEqualsToken },
           { "!==", SyntaxKind.ExclamationEqualsEqualsToken },
           { "=>", SyntaxKind.EqualsGreaterThanToken },
           { "+", SyntaxKind.PlusToken },
           { "-", SyntaxKind.MinusToken },
           { "**", SyntaxKind.AsteriskAsteriskToken },
           { "*", SyntaxKind.AsteriskToken },
           { "/", SyntaxKind.SlashToken },
           { "%", SyntaxKind.PercentToken },
           { "++", SyntaxKind.PlusPlusToken },
           { "--", SyntaxKind.MinusMinusToken },
           { "<<", SyntaxKind.LessThanLessThanToken },
           { "</", SyntaxKind.LessThanSlashToken },
           { ">>", SyntaxKind.GreaterThanGreaterThanToken },
           { ">>>", SyntaxKind.GreaterThanGreaterThanGreaterThanToken },
           { "&", SyntaxKind.AmpersandToken },
           { "|", SyntaxKind.BarToken },
           { "^", SyntaxKind.CaretToken },
           { "!", SyntaxKind.ExclamationToken },
           { "~", SyntaxKind.TildeToken },
           { "&&", SyntaxKind.AmpersandAmpersandToken },
           { "||", SyntaxKind.BarBarToken },
           { "?", SyntaxKind.QuestionToken },
           { ",", SyntaxKind.ColonToken },
           { "=", SyntaxKind.EqualsToken },
           { "+=", SyntaxKind.PlusEqualsToken },
           { "-=", SyntaxKind.MinusEqualsToken },
           { "*=", SyntaxKind.AsteriskEqualsToken },
           { "**=", SyntaxKind.AsteriskAsteriskEqualsToken },
           { "/=", SyntaxKind.SlashEqualsToken },
           { "%=", SyntaxKind.PercentEqualsToken },
           { "<<=", SyntaxKind.LessThanLessThanEqualsToken },
           { ">>=", SyntaxKind.GreaterThanGreaterThanEqualsToken },
           { ">>>=", SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken },
           { "&=", SyntaxKind.AmpersandEqualsToken },
           { "|=", SyntaxKind.BarEqualsToken },
           { "^=", SyntaxKind.CaretEqualsToken },
           { "@", SyntaxKind.AtToken }
        };
        private CharacterCodes chCurrent;
        private string currentTokenValue;

        public Scanner(bool skipTrivia)
        {
            this.skipTrivia = skipTrivia;
        }

        public void SetText(string text, int? start = null, int? length = null)
        {
            this.text = text;
            end = length.HasValue ? startPos + length.Value : text.Length;
            SetTextPosition(start ?? 0);
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public int StartPosition
        {
            get
            {
                return startPos;
            }
        }

        public int TokenPosition
        {
            get
            {
                return tokenPos;
            }
        }

        public void SetTextPosition(int pos)
        {
            this.pos = pos;
            startPos = pos;
            tokenPos = pos;
            token = SyntaxKind.Unknown;

            precedingLineBreak = false;

            tokenValue = null;
            hasExtendedUnicodeEscape = false;
            tokenIsUnterminated = false;
        }

        public string CurrentText
        {
            get
            {
                if (text != null)
                {
                    var prefix = string.Empty;

                    if (currentTokenValue == null)
                    {
                        prefix = ((char)chCurrent).ToString();
                    }
                    else if (chCurrent != CharacterCodes.nullCharacter)
                    {
                        prefix = currentTokenValue;
                    }

                    return prefix + text.Right(text.Length - pos);
                }
                else
                {
                    return "[Text not set]";
                }
            }
        }

        public SyntaxKind Scan()
        {
            startPos = pos;
            hasExtendedUnicodeEscape = false;
            precedingLineBreak = false;
            tokenIsUnterminated = false;

            currentTokenValue = null;

            while (true)
            {
                tokenPos = pos;

                if (pos >= end)
                {
                    return SyntaxKind.EndOfFileToken;
                }

                chCurrent = text.CharCodeAt(pos);

                switch (chCurrent) 
                {
                    case CharacterCodes.space:

                        if (skipTrivia)
                        {
                            pos++;
                            continue;
                        }
                        else
                        {
                            while (pos < end && IsWhiteSpaceSingleLine(text.CharCodeAt(pos)))
                            {
                                pos++;
                            }
                        
                            return token = SyntaxKind.WhitespaceTrivia;
                        }

                    case CharacterCodes._1:
                    case CharacterCodes._2:
                    case CharacterCodes._3:
                    case CharacterCodes._4:
                    case CharacterCodes._5:
                    case CharacterCodes._6:
                    case CharacterCodes._7:
                    case CharacterCodes._8:
                    case CharacterCodes._9:
                        tokenValue = ScanNumber();
                        return token = SyntaxKind.NumericLiteral;
                    case CharacterCodes.dot:
                        pos++;
                        return token = SyntaxKind.DotToken;
                    case CharacterCodes.openBracket:
                        pos++;
                        return token = SyntaxKind.OpenBracketToken;
                    case CharacterCodes.closeBracket:
                        pos++;
                        return token = SyntaxKind.CloseBracketToken;
                    case CharacterCodes.openParen:
                        pos++;
                        return token = SyntaxKind.OpenParenToken;
                    case CharacterCodes.closeParen:
                        pos++;
                        return token = SyntaxKind.CloseParenToken;
                    case CharacterCodes.comma:
                        pos++;
                        return token = SyntaxKind.CommaToken;
                    case CharacterCodes.backtick:
                        pos++;
                        return token = SyntaxKind.CLRStringArgCountSpecifier;
                    default:

                        if (IsIdentifierStart(chCurrent))
                        {
                            pos++;

                            while (pos < end && IsIdentifierPart((chCurrent = text.CharCodeAt(pos))))
                            {
                                pos++;
                            }

                            tokenValue = text.Substring(tokenPos, pos - tokenPos);

                            if (chCurrent == CharacterCodes.backslash) 
                            {
                                tokenValue += ScanIdentifierParts();
                            }

                            currentTokenValue = tokenValue;

                            return token = GetIdentifierToken();
                        }
                        else if (IsWhiteSpaceSingleLine(chCurrent)) 
                        {
                            pos++;
                            continue;
                        }
                        else if (IsLineBreak(chCurrent)) 
                        {
                            precedingLineBreak = true;
                            pos++;
                            continue;
                        }

                        Debugger.Break();
                        pos++;

                        return token = SyntaxKind.Unknown;
                }
            }
        }

        private bool IsLineBreak(CharacterCodes ch)
        {
             return ch == CharacterCodes.lineFeed ||
                  ch == CharacterCodes.carriageReturn ||
                  ch == CharacterCodes.lineSeparator ||
                  ch == CharacterCodes.paragraphSeparator;        
        }

        private string ScanNumber()
        {
            var start = pos;
            int end;

            while (IsDigit(text.CharCodeAt(pos))) pos++;
            
            if (text.CharCodeAt(pos) == CharacterCodes.dot) 
            {
                pos++;
                while (IsDigit(text.CharCodeAt(pos))) pos++;
            }
            
            end = pos;

            if (text.CharCodeAt(pos) == CharacterCodes.E || text.CharCodeAt(pos) == CharacterCodes.e) 
            {
                pos++;

                if (text.CharCodeAt(pos) == CharacterCodes.plus || text.CharCodeAt(pos) == CharacterCodes.minus) pos++;

                if (IsDigit(text.CharCodeAt(pos))) 
                {
                    pos++;
                    while (IsDigit(text.CharCodeAt(pos))) pos++;
                    end = pos;
                }
                else 
                {
                    Debugger.Break();
                }
            }

            return text.Substring(start, end - start);
        }

        private bool IsDigit(CharacterCodes ch)
        {
            return ch >= CharacterCodes._0 && ch <= CharacterCodes._9;
        }

        private SyntaxKind GetIdentifierToken()
        {
            var length = tokenValue.Length;

            if (length >= 2 && length <= 11)
            {
                var ch = tokenValue.CharCodeAt(0);

                if (ch >= CharacterCodes.a && ch <= CharacterCodes.z && textToToken.ContainsKey(tokenValue))
                {
                    return (token = textToToken[tokenValue]);
                }
            }

            return (token = SyntaxKind.Identifier);
        }

        private object ScanIdentifierParts()
        {
            throw new NotImplementedException();
        }

        private bool IsIdentifierPart(CharacterCodes ch)
        {
            return ch >= CharacterCodes.A && ch <= CharacterCodes.Z || ch >= CharacterCodes.a && ch <= CharacterCodes.z ||
                ch >= CharacterCodes._0 && ch <= CharacterCodes._9 || ch == CharacterCodes.at || ch == CharacterCodes._;
        }

        private bool IsIdentifierStart(CharacterCodes ch)
        {
            return ch >= CharacterCodes.A && ch <= CharacterCodes.Z || ch >= CharacterCodes.a && ch <= CharacterCodes.z ||
                ch == CharacterCodes.at || ch == CharacterCodes._;
        }

        private object ScanString()
        {
            throw new NotImplementedException();
        }

        private bool IsWhiteSpaceSingleLine(CharacterCodes ch)
        {
            return ch == CharacterCodes.space ||
                ch == CharacterCodes.tab ||
                ch == CharacterCodes.verticalTab ||
                ch == CharacterCodes.formFeed ||
                ch == CharacterCodes.nonBreakingSpace ||
                ch == CharacterCodes.nextLine ||
                ch == CharacterCodes.ogham ||
                ch >= CharacterCodes.enQuad && ch <= CharacterCodes.zeroWidthSpace ||
                ch == CharacterCodes.narrowNoBreakSpace ||
                ch == CharacterCodes.mathematicalSpace ||
                ch == CharacterCodes.ideographicSpace ||
                ch == CharacterCodes.byteOrderMark;
        }

        internal int GetStartPosition()
        {
            return startPos;
        }

        internal string GetTokenValue()
        {
            return tokenValue;
        }

        public bool HasPrecedingLineBreak
        {
            get
            {
                return precedingLineBreak;
            }
        }

        public T LookAhead<T>(Func<T> callback)
        {
            return SpeculationHelper(callback, true);
        }

        public T TryScan<T>(Func<T> callback)
        {
            return SpeculationHelper(callback, false);
        }

        private T SpeculationHelper<T>(Func<T> callback, bool isLookAhead)
        {
            var savePos = pos;
            var saveStartPos = startPos;
            var saveTokenPos = tokenPos;
            var saveToken = token;
            var saveTokenValue = tokenValue;
            var savePrecedingLineBreak = precedingLineBreak;
            var result = callback();

            if (result == null || isLookAhead) 
            {
                pos = savePos;
                startPos = saveStartPos;
                tokenPos = saveTokenPos;
                token = saveToken;
                tokenValue = saveTokenValue;
                precedingLineBreak = savePrecedingLineBreak;
            }

            return result;
        }
    }
}
