using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Utils.Parsing.Nodes;
using System.Diagnostics;
using Utils;

namespace Utils.Parsing
{
    public class Parser
    {
        private Scanner scanner;
        private object syntaxCursor;
        private ParsingContext parsingContext;
        private Dictionary<string, string> identifiers;
        private delegate T ParseElement<T>();
        private SyntaxKind currentToken;
        private int nodeCount;
        private Stack<INode> nodeStack;
        private Node currentNode;
        private int identifierCount;
        private string sourceText;
        private NodeFlags contextFlags;
        private List<Diagnostic> parseDiagnostics;
        private bool parseErrorBeforeNextFinishedNode;

        public Parser()
        {
            scanner = new Scanner(true);
            nodeStack = new Stack<INode>();
            parseDiagnostics = new List<Diagnostic>();
        }

        public SyntaxTree Parse(MethodInfo method)
        {
            // Char Aggregate[Char](System.Collections.Generic.IEnumerable`1[System.Char], System.Func`3[System.Char,System.Char,System.Char])
            // System.Collections.Generic.Dictionary`2[System.String,System.Collections.Generic.List`1[System.Int32]] MakeDictionaryList[Int32](System.Collections.Generic.List`1[System.Collections.Generic.KeyValuePair`2[System.String,System.Int32]])

            var methodText = method.ToString();
            var syntaxTree = SyntaxTree.Construct<SyntaxTree>(SyntaxKind.SyntaxTree, 0, 0);

            InitializeState(methodText);

            // Prime the scanner

            NextToken();

            Push(syntaxTree);

            syntaxTree.Children = ParseList<Node>(ParsingContext.CLRString, ParseCLRStringMethod).ToChildren();

            return syntaxTree;
        }

        public SyntaxTree Parse(Type type)
        {
            // Char Aggregate[Char](System.Collections.Generic.IEnumerable`1[System.Char], System.Func`3[System.Char,System.Char,System.Char])
            // System.Collections.Generic.Dictionary`2[System.String,System.Collections.Generic.List`1[System.Int32]] MakeDictionaryList[Int32](System.Collections.Generic.List`1[System.Collections.Generic.KeyValuePair`2[System.String,System.Int32]])

            var typeText = type.ToString();
            var syntaxTree = SyntaxTree.Construct<SyntaxTree>(SyntaxKind.SyntaxTree, 0, 0);

            InitializeState(typeText);

            // Prime the scanner

            NextToken();

            Push(syntaxTree);

            syntaxTree.Children = ParseList<Node>(ParsingContext.CLRString, ParseCLRStringType).ToChildren();

            return syntaxTree;
        }

        private SyntaxKind Token
        {
            get
            {
                return currentToken;
            }
        }

        private Node CurrentNode
        {
            get
            {
                if (syntaxCursor == null)
                {
                    return null;
                }

                return currentNode;
            }
        }

        private SyntaxKind NextToken()
        {
            currentToken = scanner.Scan();

            return currentToken;
        }

        private NodeArray<T> ParseList<T>(ParsingContext kind, ParseElement<T> parseElement) where T : Node
        {
            var result = new NodeArray<T>();
            ParsingContext saveParsingContext = parsingContext;

            parsingContext |= (ParsingContext) (1 << (int) kind);

            while (!IsListTerminator(kind))
            {
                if (IsListElement(kind, false))
                {
                    var element = ParseListElement<T>(kind, parseElement);

                    result.Add(element);
                }
            }

            return result;
        }

        private T ParseListElement<T>(ParsingContext kind, ParseElement<T> parseElement)
        {
            var node = this.CurrentNode;

            if (node != null)
            {
                return (T)ConsumeNode(node);
            }

            return parseElement();
        }

        private object ConsumeNode(Node node)
        {
            throw new NotImplementedException();
        }

        private bool IsListElement(ParsingContext parsingContext, bool inErrorRecovery)
        {
            var node = this.CurrentNode;

            if (node != null)
            {
                return true;
            }

            switch (parsingContext)
            {
                case ParsingContext.CLRString:
                    return !(this.Token == SyntaxKind.CloseParenToken) && IsStartOfCLRString(); 
                case ParsingContext.TypeArguments:
                case ParsingContext.TupleElementTypes:
                    return this.Token == SyntaxKind.CommaToken || IsStartOfType();
                case ParsingContext.TypeParameters:
                    return IsIdentifier();
                case ParsingContext.Parameters:
                    return IsStartOfParameter();
            }

            throw new Exception();
        }

        private bool IsStartOfParameter()
        {
            return this.Token == SyntaxKind.DotDotDotToken || IsIdentifierOrPattern() || IsModifierKind(this.Token) || this.Token == SyntaxKind.AtToken || this.Token == SyntaxKind.ThisKeyword;
        }

        private bool IsModifierKind(SyntaxKind syntaxKind)
        {
            throw new NotImplementedException();
        }

        private bool IsIdentifierOrPattern()
        {
            return this.Token == SyntaxKind.OpenBraceToken || this.Token == SyntaxKind.OpenBracketToken || IsIdentifier();
        }

        private bool IsStartOfType()
        {
            switch (this.Token)
            {
                case SyntaxKind.AnyKeyword:
                case SyntaxKind.StringKeyword:
                case SyntaxKind.NumberKeyword:
                case SyntaxKind.BooleanKeyword:
                case SyntaxKind.SymbolKeyword:
                case SyntaxKind.VoidKeyword:
                case SyntaxKind.UndefinedKeyword:
                case SyntaxKind.NullKeyword:
                case SyntaxKind.ThisKeyword:
                case SyntaxKind.TypeOfKeyword:
                case SyntaxKind.NeverKeyword:
                case SyntaxKind.OpenBraceToken:
                case SyntaxKind.OpenBracketToken:
                case SyntaxKind.LessThanToken:
                case SyntaxKind.BarToken:
                case SyntaxKind.AmpersandToken:
                case SyntaxKind.NewKeyword:
                case SyntaxKind.StringLiteral:
                case SyntaxKind.NumericLiteral:
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.ObjectKeyword:
                    return true;
                case SyntaxKind.MinusToken:

                    Debugger.Break();
                    return false;

                    //return lookAhead(nextTokenIsNumericLiteral);
                case SyntaxKind.OpenParenToken:

                    Debugger.Break();
                    return false;

                    // Only consider '(' the start of a type if followed by ')', '...', an identifier, a modifier,
                    // or something that starts a type. We don't want to consider things like '(1)' a type.
                    //return lookAhead(isStartOfParenthesizedOrFunctionType);
                default:
                    return IsIdentifier();
            }
        }

        private bool IsStartOfCLRString()
        {
            switch (this.Token)
            {
                case SyntaxKind.Identifier:
                    return true;
            }

            return false;
        }

        private bool IsListTerminator(ParsingContext kind)
        {
            if (this.Token == SyntaxKind.EndOfFileToken)
            {
                return true;
            }

            switch (kind)
            {
                case ParsingContext.CLRString:
                    return this.Token == SyntaxKind.CloseParenToken;
                case ParsingContext.TypeArguments:
                    return this.Token != SyntaxKind.CommaToken;
                case ParsingContext.TypeParameters:
                    return this.Token == SyntaxKind.CloseBracketToken;
                case ParsingContext.Parameters:
                case ParsingContext.RestProperties:
                    // Tokens other than ')' and ']' (the latter for index signatures) are here for better error recovery
                    return this.Token == SyntaxKind.CloseParenToken || this.Token == SyntaxKind.CloseBracketToken /*|| token == SyntaxKind.OpenBraceToken*/;
                default:
                    Debugger.Break();
                    break;
            }

            return false;
        }

        private CLRStringMethod ParseCLRStringMethod()
        {
            switch (this.Token)
            {
                case SyntaxKind.Identifier:
                    return ParseCLRStringMethod(scanner.StartPosition);
            }

            return null;
        }

        private CLRStringMethod ParseCLRStringMethod(int fullStart)
        {
            using (var clrStringMethod = CreateNode<CLRStringMethod>(SyntaxKind.CLRString, fullStart, NodeStackOperation.Push))
            {
                clrStringMethod.Type = ParseCLRStringType();
                clrStringMethod.Name = ParseCLRStringPropertyName();

                FillCLRStringSignature(clrStringMethod);

                return clrStringMethod;
            }
        }

        private PropertyName ParseCLRStringPropertyName()
        {
            return ParsePropertyNameWorker(false);
        }

        private PropertyName ParsePropertyName()
        {
            return ParsePropertyNameWorker(true);
        }

        private PropertyName ParsePropertyNameWorker(bool allowComputedPropertyNames)
        {
            if (this.Token == SyntaxKind.StringLiteral || this.Token == SyntaxKind.NumericLiteral) 
            {
                return (PropertyName) ParseLiteralNode(/*internName*/ true);
            }
            else if (allowComputedPropertyNames && this.Token == SyntaxKind.OpenBracketToken) 
            {
                Debugger.Break();
                //return parseComputedPropertyName();

                return null;
            }
            
            return (PropertyName) ParseIdentifierName();
        }

        private void SetContextFlag(bool val, NodeFlags flag) 
        {
            if (val) 
            {
                contextFlags |= flag;
            }
            else 
            {
                contextFlags &= ~flag;
            }
        }

        private TypeNode ParseCLRStringType()
        {
            return DoOutsideOfContext(NodeFlags.TypeExcludesFlags, ParseCLRStringTypeWorker);
        }

        private T DoOutsideOfContext<T>(NodeFlags context, Func<T> func)
        {
            var contextFlagsToClear = context & contextFlags;

            if (contextFlagsToClear == 0) 
            {
                T result;

                // clear the requested context flags

                SetContextFlag(/*val*/ false, contextFlagsToClear);
                result = func();

                // restore the context flags we just cleared

                SetContextFlag(/*val*/ true, contextFlagsToClear);
                return result;
            }

            // no need to do anything special as we are not in any of the requested contexts

            return func();
        }

        private TypeNode ParseCLRStringTypeWorker()
        {
            return ParseCLRStringArrayTypeOrHigher();
        }

        private TypeNode ParseCLRStringArrayTypeOrHigher()
        {
            var type = ParseCLRStringNonArrayType();

            while (!scanner.HasPrecedingLineBreak && ParseOptional(SyntaxKind.OpenBracketToken))
            {
                if (IsStartOfType())
                {
                    Debugger.Break();
                }
                else
                {
                    var node = CreateNode<ArrayTypeNode>(SyntaxKind.ArrayType, type.Pos, NodeStackOperation.Add);

                    node.ElementType = type;

                    ParseExpected(SyntaxKind.CloseBracketToken);
                    type = FinishNode(node);

                    if (this.Token == SyntaxKind.OpenBracketToken)
                    {
                        // handle multi-dimensional arrays

                        Debugger.Break();
                    }
                }
            }

            return type;
        }

        private TypeNode ParseCLRStringNonArrayType()
        {
            switch (this.Token)
            {
                //case SyntaxKind.AnyKeyword:
                //case SyntaxKind.StringKeyword:
                //case SyntaxKind.NumberKeyword:
                //case SyntaxKind.BooleanKeyword:
                //case SyntaxKind.SymbolKeyword:
                //case SyntaxKind.UndefinedKeyword:
                //case SyntaxKind.NeverKeyword:
                //case SyntaxKind.ObjectKeyword:
                //    // If these are followed by a dot, then parse these out as a dotted type reference instead.
                //    const node = tryParse(parseKeywordAndNoDot);
                //    return node || parseTypeReference();
                //case SyntaxKind.StringLiteral:
                //case SyntaxKind.NumericLiteral:
                //case SyntaxKind.TrueKeyword:
                //case SyntaxKind.FalseKeyword:
                //    return parseLiteralTypeNode();
                //case SyntaxKind.MinusToken:
                //    return lookAhead(nextTokenIsNumericLiteral) ? parseLiteralTypeNode() : parseTypeReference();
                //case SyntaxKind.VoidKeyword:
                //case SyntaxKind.NullKeyword:
                //    return parseTokenNode<TypeNode>();
                //case SyntaxKind.ThisKeyword: {
                //    const thisKeyword = parseThisTypeNode();
                //    if (this.Token == SyntaxKind.IsKeyword && !scanner.hasPrecedingLineBreak()) {
                //        return parseThisTypePredicate(thisKeyword);
                //    }
                //    else {
                //        return thisKeyword;
                //    }
                //}
                //case SyntaxKind.TypeOfKeyword:
                //    return parseTypeQuery();
                //case SyntaxKind.OpenBraceToken:
                //    return lookAhead(isStartOfMappedType) ? parseMappedType() : parseTypeLiteral();
                //case SyntaxKind.OpenBracketToken:
                //    return parseTupleType();
                //case SyntaxKind.OpenParenToken:
                //    return parseParenthesizedType();
                default:
                    return ParseCLRStringTypeReference();            
            }
        }

        private CLRStringType ParseCLRStringTypeReference()
        {
            var typeName = ParseCLRStringEntityName(false);
            var node = CreateNode<CLRStringType>(SyntaxKind.TypeReference, typeName.Pos);

            node.TypeName = typeName;

            if (!scanner.HasPrecedingLineBreak && this.Token == SyntaxKind.OpenBracketToken)
            {
                if (!LookAhead(NextTokenIsCloseBracket))
                {
                    node.TypeArguments = ParseBracketedList(ParsingContext.TypeArguments, ParseCLRStringType, SyntaxKind.OpenBracketToken, SyntaxKind.CloseBracketToken).ToNodeArray();
                }
            }

            return FinishNode(node);
        }

        private bool NextTokenIsCloseBracket()
        {
            return NextToken() == SyntaxKind.CloseBracketToken;
        }

        private bool NextTokenIsOpenBracket()
        {
            return NextToken() == SyntaxKind.OpenBracketToken;
        }

        private CLRStringEntityName ParseCLRStringEntityName(bool allowReservedWords, DiagnosticMessage diagnosticMessage = null)
        {
            var entityName = (CLRStringEntityName)ParseEntityName(allowReservedWords, diagnosticMessage);

            if (LookAhead(NextTokenIsNumericLiteral))
            {
                NextToken();
                entityName.ArgCount = (NumericLiteral)(TokenNode) ParseLiteralNode();
            }

            return entityName;
        }

        private T LookAhead<T>(Func<T> callback)
        {
            return SpeculationHelper(callback, true);
        }

        private T SpeculationHelper<T>(Func<T> callback, bool isLookAhead)
        {
            var saveToken = currentToken;
            var saveParseErrorBeforeNextFinishedNode = parseErrorBeforeNextFinishedNode;
            var saveContextFlags = contextFlags;
            var result = isLookAhead
                ? scanner.LookAhead(callback)
                : scanner.TryScan(callback);

            Debug.Assert(saveContextFlags == contextFlags);

            if (result == null || isLookAhead) 
            {
                currentToken = saveToken;
                parseErrorBeforeNextFinishedNode = saveParseErrorBeforeNextFinishedNode;
            }

            return result;
        }

        private bool NextTokenIsNumericLiteral()
        {
            return NextToken() == SyntaxKind.NumericLiteral;
        }

        private LiteralExpression ParseLiteralNode(bool internName = false)
        {
             return (LiteralExpression) ParseLiteralLikeNode(this.Token, internName);
        }

        private LiteralExpression ParseLiteralLikeNode(SyntaxKind kind, bool internName)
        {
            var node = (LiteralExpression) CreateNode<TokenNode>(kind);
            var text = scanner.GetTokenValue();
            int tokenPos;

            node.Text = internName ? InternIdentifier(text) : text;

            tokenPos = scanner.TokenPosition;

            NextToken();

            return FinishNode(node);
        }

        private EntityName ParseEntityName(bool allowReservedWords, DiagnosticMessage diagnosticMessage = null)
        {
            var entity = (EntityName) ParseIdentifier();

            while (ParseOptional(SyntaxKind.DotToken))
            {
                using (var node = CreateNode<QualifiedName>(SyntaxKind.QualifiedName, entity.Pos, NodeStackOperation.PushAdd))
                {
                    node.Left = entity;
                    node.Right = ParseRightSideOfDot(allowReservedWords);

                    Add(node.Left);
                    Add(node.Right);

                    entity = (EntityName)FinishNode(node);
                }
            }

            return entity;
        }

        private Identifier ParseRightSideOfDot(bool allowIdentifierNames)
        {
            if (scanner.HasPrecedingLineBreak && TokenIsIdentifierOrKeyword(this.Token))
            {
                Debugger.Break();
            }

            return allowIdentifierNames ? ParseIdentifierName() : ParseIdentifier();
        }

        private Identifier ParseIdentifierName()
        {
            return CreateIdentifier(TokenIsIdentifierOrKeyword(this.Token));
        }

        private bool TokenIsIdentifierOrKeyword(SyntaxKind token)
        {
            return token >= SyntaxKind.Identifier;
        }

        private bool IsStartOfFunctionType()
        {
            if (this.Token == SyntaxKind.LessThanToken) 
            {
                return true;
            }

            return this.Token == SyntaxKind.OpenParenToken; // && LookAhead(IsUnambiguouslyStartOfFunctionType);
        }

        private bool IsUnambiguouslyStartOfFunctionType() 
        {
            NextToken();

            if (this.Token == SyntaxKind.CloseParenToken || this.Token == SyntaxKind.DotDotDotToken) 
            {
                // ( )
                // ( ...
                return true;
            }

            if (SkipParameterStart()) 
            {
                // We successfully skipped modifiers (if any) and an identifier or binding pattern,
                // now see if we have something that indicates a parameter declaration

                if (this.Token == SyntaxKind.ColonToken || this.Token == SyntaxKind.CommaToken ||
                    this.Token == SyntaxKind.QuestionToken || this.Token == SyntaxKind.EqualsToken) 
                {
                    // ( xxx :
                    // ( xxx ,
                    // ( xxx ?
                    // ( xxx =
                    return true;
                }
                else if (this.Token == SyntaxKind.CloseParenToken) 
                {
                    NextToken();

                    if (this.Token == SyntaxKind.EqualsGreaterThanToken) 
                    {
                        // ( xxx ) =>
                        return true;
                    }
                }
            }

            return false;
        }

        private bool SkipParameterStart()
        {
            throw new NotImplementedException();
        }

        private void FillCLRStringSignature(SignatureDeclaration signature)
        {
            signature.TypeParameters = ParseCLRStringTypeParameters();
            signature.Parameters = ParseCLRStringParameterList();
        }

        private NodeArray<ParameterDeclaration> ParseCLRStringParameterList()
        {
            if (ParseExpected(SyntaxKind.OpenParenToken))
            {
                var result = ParseDelimitedList(ParsingContext.Parameters, ParseCLRStringParameter);

                return result;
            }

            return null;
        }

        private NodeArray<T> ParseDelimitedList<T>(ParsingContext kind, ParseElement<T> parseElement, bool? considerSemicolonAsDelimiter = null) where T : Node
        {
            var saveParsingContext = parsingContext;
            var result = new NodeArray<T>();
            var commaStart = -1;

            parsingContext |= (ParsingContext)(1 << (int)kind);

            while (true)
            {
                if (IsListElement(kind, false))
                {
                    result.Add(ParseListElement(kind, parseElement));
                    commaStart = scanner.TokenPosition;

                    if (ParseOptional(SyntaxKind.CommaToken))
                    {
                        continue;
                    }

                    commaStart = -1;

                    if (IsListTerminator(kind))
                    {
                        break;
                    }

                    ParseExpected(SyntaxKind.CommaToken);

                    if (considerSemicolonAsDelimiter.HasValue && considerSemicolonAsDelimiter.Value && this.Token == SyntaxKind.SemicolonToken && !scanner.HasPrecedingLineBreak)
                    {
                        NextToken();
                    }

                    continue;
                }

                if (IsListTerminator(kind))
                {
                    break;
                }

                if (AbortParsingListOrMoveToNextToken(kind))
                {
                    break;
                }
            }

            result.End = GetNodeEnd();
            parsingContext = saveParsingContext;

            return result;
        }

        private int GetNodeEnd()
        {
            return scanner.GetStartPosition();
        }

        private bool AbortParsingListOrMoveToNextToken(ParsingContext kind)
        {
            //ParseErrorAtCurrentToken(ParsingContextErrors(kind));

            Debugger.Break();
            return false;
        }

        private DiagnosticMessage ParsingContextErrors(ParsingContext context)
        {
            Debugger.Break();
            return null;
        }

        private bool ParseOptional(SyntaxKind t)
        {
            if (this.Token == t)
            {
                NextToken();
                return true;
            }
            else
            {
                return false;
            }
        }

        private ParameterDeclaration ParseCLRStringParameter()
        {
            var node = CreateNode<ParameterDeclaration>(SyntaxKind.Parameter);

            node.Type = ParseCLRStringType();

            if (this.Token == SyntaxKind.Identifier)
            {
                node.Name = (BindingName)CreateIdentifier(IsIdentifier());
            }

            return node;
        }

        private NodeArray<TypeParameterDeclaration> ParseCLRStringTypeParameters()
        {
            if (this.Token == SyntaxKind.OpenBracketToken)
            {
                return ParseBracketedList(ParsingContext.TypeParameters, ParseTypeParameter, SyntaxKind.OpenBracketToken, SyntaxKind.CloseBracketToken);
            }

            return null;
        }

        private TypeParameterDeclaration ParseTypeParameter()
        {
            var node = CreateNode<TypeParameterDeclaration>(SyntaxKind.TypeParameter);

            node.Name = (Identifier) ParseIdentifier();

            return FinishNode(node);
        }

        private NodeArray<T> ParseBracketedList<T>(ParsingContext kind, ParseElement<T> parseElement, SyntaxKind open, SyntaxKind close) where T : Node
        {
            if (ParseExpected(open))
            {
                var result = ParseDelimitedList(kind, parseElement);
                
                ParseExpected(close);

                return result;
            }

            Debugger.Break();
            return null;
        }

        private Identifier ParseIdentifier(DiagnosticMessage diagnosticMessage = null)
        {
            return CreateIdentifier(IsIdentifier());
        }

        private Identifier CreateIdentifier(bool isIdentifier, DiagnosticMessage diagnosticMessage = null)
        {
 	        identifierCount++;

            if (isIdentifier)
            {
                var node = CreateNode<Identifier>(SyntaxKind.Identifier);

                if (this.Token != SyntaxKind.Identifier)
                {
                    node.OriginalKeywordKind = this.Token;
                }

                node.Text = InternIdentifier(scanner.GetTokenValue());
                NextToken();

                return FinishNode(node);
            }
            else
            {
                Debugger.Break();
                return null;
            }
        }

        private T FinishNode<T>(T node, int? end = null) where T : Node
        {
            node.End = end == null ? scanner.GetStartPosition() : end.Value;

            return node;
        }

        private string InternIdentifier(string text)
        {
            text = EscapeIdentifier(text);

            return identifiers.AddToDictionaryIfNotExist(text, text);
        }

        private string EscapeIdentifier(string identifier)
        {
            return identifier.Length >= 2 && identifier.CharCodeAt(0) == CharacterCodes._ && identifier.CharCodeAt(1) == CharacterCodes._ ? "_" + identifier : identifier;
        }

        private bool IsIdentifier()
        {
            return this.Token == SyntaxKind.Identifier;
        }

        private TNode CreateNode<TNode>(SyntaxKind kind, NodeStackOperation operation = NodeStackOperation.None) where TNode : Node
        {
            Node node;
            var pos = scanner.GetStartPosition();

            nodeCount++;

            if (kind >= SyntaxKind.FirstNode)
            {
                node = Node.Construct<TNode>(kind, pos, pos);
            }
            else if (kind == SyntaxKind.Identifier)
            {
                node = (TNode)(Node) new Identifier(kind, pos, pos);
            }
            else
            {
                node = (TNode)(Node) new TokenNode(kind, pos, pos);
            }

            switch (operation)
            {
                case NodeStackOperation.Push:
                    node.CurrentDisposer = Push(node);
                    break;
                case NodeStackOperation.Add:
                    Add(node);
                    break;
                case NodeStackOperation.PushAdd:
                    node.CurrentDisposer = PushAdd(node);
                    break;
            }

            return (TNode) node;
        }

        private TNode CreateNode<TNode>(SyntaxKind kind, int pos, NodeStackOperation operation = NodeStackOperation.None) where TNode : Node
        {
            Node node;

            nodeCount++;

            if (kind >= SyntaxKind.FirstNode)
            {
                node = Node.Construct<TNode>(kind, pos, pos);
            }
            else if (kind == SyntaxKind.Identifier)
            {
                node = (TNode)(Node) new Identifier(kind, pos, pos);
            }
            else
            {
                node = (TNode)(Node) new Token(kind, pos, pos);
            }

            switch (operation)
            {
                case NodeStackOperation.Push:
                    node.CurrentDisposer = Push(node);
                    break;
                case NodeStackOperation.Add:
                    Add(node);
                    break;
                case NodeStackOperation.PushAdd:
                    node.CurrentDisposer = PushAdd(node);
                    break;
            }

            return (TNode) node;
        }

        private bool ParseExpected(SyntaxKind kind, bool shouldAdvance = true)
        {
            if (this.Token == kind)
            {
                if (shouldAdvance)
                {
                    NextToken();
                }

                return true;
            }

            Debugger.Break();
            return false;
        }

        private void InitializeState(string methodText, object syntaxCursor = null, SourceKind? sourceKind = null)
        {
            sourceText = methodText;

            parsingContext = 0;
            this.syntaxCursor = syntaxCursor;
            identifiers = CreateMap<string, string>();
            identifierCount = 0;
            nodeCount = 0;

            contextFlags = sourceKind == SourceKind.CLRText ? NodeFlags.CLRString : NodeFlags.None;

            scanner.SetText(methodText);
        }

        private Dictionary<TKey, TValue> CreateMap<TKey, TValue>(Dictionary<TKey, TValue> template = null)
        {
            var map = new Dictionary<TKey, TValue>();

            if (template != null)
            {
                foreach (var pair in template)
                {
                    map.Add(pair.Key, pair.Value);
                }
            }

            return map;
        }

        private IDisposable Push(INode node)
        {
            return nodeStack.PushNode(node);
        }

        private IDisposable PushAdd(INode node)
        {
            return nodeStack.PushAddNode(node);
        }

        private INode Pop()
        {
            return nodeStack.PopNode();
        }

        private INode Peek()
        {
            return nodeStack.PeekNode();
        }

        private void Add(INode node)
        {
            nodeStack.AddNode(node);
        }
    }
}
