using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CppSharp.Parser;
using CppSharp.Parser.AST;
using System.Diagnostics;

namespace CppParser
{
    public static class ParserExtensions
    {
        public static IEnumerable<uint> GetPositions(this TParamCommandComment comment)
        {
            for (var x = 0; x < comment.PositionCount; x++)
            {
                var content = comment.getPosition((uint)x);
                yield return content;
            }
        }

        public static IEnumerable<BlockCommandComment.Argument> GetArguments(this BlockCommandComment comment)
        {
            for (var x = 0; x < comment.ArgumentsCount; x++)
            {
                var argument = comment.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<InlineCommandComment.Argument> GetArguments(this InlineCommandComment comment)
        {
            for (var x = 0; x < comment.ArgumentsCount; x++)
            {
                var argument = comment.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<HTMLStartTagComment.Attribute> GetAttributes(this HTMLStartTagComment comment)
        {
            for (var x = 0; x < comment.AttributesCount; x++)
            {
                var attribute = comment.getAttributes((uint)x);
                yield return attribute;
            }
        }

        public static IEnumerable<InlineContentComment> GetContent(this ParagraphComment comment)
        {
            for (var x = 0; x < comment.ContentCount; x++)
            {
                var content = comment.getContent((uint)x);
                yield return content;
            }
        }

        public static IEnumerable<VerbatimBlockLineComment> GetLines(this VerbatimBlockComment comment)
        {
            for (var x = 0; x < comment.LinesCount; x++)
            {
                var line = comment.getLines((uint)x);
                yield return line;
            }
        }

        public static IEnumerable<Friend> GetFriends(this DeclarationContext context)
        {
            for (var x = 0; x < context.FriendsCount; x++)
            {
                var friend = context.getFriends((uint)x);
                yield return friend;
            }
        }

        public static IEnumerable<TemplateArgument> GetArguments(this FunctionTemplateSpecialization functionTemplate)
        {
            for (var x = 0; x < functionTemplate.ArgumentsCount; x++)
            {
                var argument = functionTemplate.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<Expression> GetArguments(this CXXConstructExpr constructorExpr)
        {
            for (var x = 0; x < constructorExpr.ArgumentsCount; x++)
            {
                var argument = constructorExpr.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<Expression> GetArguments(this CallExpr callExpr)
        {
            for (var x = 0; x < callExpr.ArgumentsCount; x++)
            {
                var argument = callExpr.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<TemplateArgument> GetArguments(this ClassTemplateSpecialization classTemplate)
        {
            for (var x = 0; x < classTemplate.ArgumentsCount; x++)
            {
                var argument = classTemplate.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<ClassTemplateSpecialization> GetSpecializations(this ClassTemplate classTemplate)
        {
            for (var x = 0; x < classTemplate.SpecializationsCount; x++)
            {
                var specialization = classTemplate.getSpecializations((uint)x);
                yield return specialization;
            }
        }

        public static IEnumerable<FunctionTemplateSpecialization> GetSpecializations(this FunctionTemplate functionTemplate)
        {
            for (var x = 0; x < functionTemplate.SpecializationsCount; x++)
            {
                var specialization = functionTemplate.getSpecializations((uint)x);
                yield return specialization;
            }
        }

        public static IEnumerable<VarTemplateSpecialization> GetSpecializations(this VarTemplate varTemplate)
        {
            for (var x = 0; x < varTemplate.SpecializationsCount; x++)
            {
                var specialization = varTemplate.getSpecializations((uint)x);
                yield return specialization;
            }
        }

        public static IEnumerable<ParserDiagnostic> GetDiagnostics(this ParserResult result)
        {
            for (var x = 0; x < result.DiagnosticsCount; x++)
            {
                var diagnostic = result.getDiagnostics((uint)x);
                yield return diagnostic;
            }
        }

        public static IEnumerable<TranslationUnit> GetUnits(this ASTContext context)
        {
            for (var x = 0; x < context.TranslationUnitsCount; x++)
            {
                var unit = context.getTranslationUnits((uint) x);
                yield return unit;
            }
        }

        public static IEnumerable<MacroDefinition> GetMacros(this TranslationUnit unit)
        {
            for (var x = 0; x < unit.MacrosCount; x++)
            {
                var macro = unit.getMacros((uint)x);
                yield return macro;
            }
        }

        public static IEnumerable<Function> GetFunctions(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.FunctionsCount; x++)
            {
                var function = declarationContext.getFunctions((uint) x);
                yield return function;
            }
        }

        public static IEnumerable<Parameter> GetParameters(this Function function)
        {
            for (var x = 0; x < function.ParametersCount; x++)
            {
                var parameter = function.getParameters((uint)x);
                yield return parameter;
            }
        }

        public static IEnumerable<Parameter> GetParameters(this FunctionType functionType)
        {
            for (var x = 0; x < functionType.ParametersCount; x++)
            {
                var parameter = functionType.getParameters((uint)x);
                yield return parameter;
            }
        }

        public static IEnumerable<Declaration> GetParameters(this Template template)
        {
            for (var x = 0; x < template.ParametersCount; x++)
            {
                var parameter = template.getParameters((uint)x);
                yield return parameter;
            }
        }

        public static IEnumerable<Namespace> GetNamespaces(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.NamespacesCount; x++)
            {
                var _namespace = declarationContext.getNamespaces((uint)x);
                yield return _namespace;
            }
        }

        public static IEnumerable<Class> GetClasses(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.ClassesCount; x++)
            {
                var _class = declarationContext.getClasses((uint)x);
                yield return _class;
            }
        }

        public static IEnumerable<Enumeration> GetEnums(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.EnumsCount; x++)
            {
                var _enum = declarationContext.getEnums((uint)x);
                yield return _enum;
            }
        }

        public static IEnumerable<Template> GetTemplates(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.TemplatesCount; x++)
            {
                var template = declarationContext.getTemplates((uint)x);
                yield return template;
            }
        }

        public static IEnumerable<TypedefDecl> GetTypeDefs(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.TypedefsCount; x++)
            {
                var typedef = declarationContext.getTypedefs((uint)x);
                yield return typedef;
            }
        }

        public static IEnumerable<TypeAlias> GetTypeAliases(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.TypeAliasesCount; x++)
            {
                var typeAlias = declarationContext.getTypeAliases((uint)x);
                yield return typeAlias;
            }
        }

        public static IEnumerable<Variable> GetVariables(this DeclarationContext declarationContext)
        {
            for (var x = 0; x < declarationContext.VariablesCount; x++)
            {
                var variable = declarationContext.getVariables((uint)x);
                yield return variable;
            }
        }

        public static IEnumerable<PreprocessedEntity> GetPreprocessedEntities(this Declaration declaration)
        {
            for (var x = 0; x < declaration.PreprocessedEntitiesCount; x++)
            {
                var preprocessedEntity = declaration.getPreprocessedEntities((uint)x);
                yield return preprocessedEntity;
            }
        }

        public static IEnumerable<BlockContentComment> GetBlocks(this FullComment fullComment)
        {
            for (var x = 0; x < fullComment.BlocksCount; x++)
            {
                var block = fullComment.getBlocks((uint)x);
                yield return block;
            }
        }

        public static IEnumerable<BaseClassSpecifier> GetBaseClassSpecifiers(this Class _class)
        {
            for (var x = 0; x < _class.BasesCount; x++)
            {
                var baseClass = _class.getBases((uint)x);
                yield return baseClass;
            }
        }

        public static IEnumerable<Field> GetFields(this Class _class)
        {
            for (var x = 0; x < _class.BasesCount; x++)
            {
                var field = _class.getFields((uint)x);
                yield return field;
            }
        }

        public static IEnumerable<Method> GetMethods(this Class _class)
        {
            for (var x = 0; x < _class.BasesCount; x++)
            {
                var method = _class.getMethods((uint)x);
                yield return method;
            }
        }

        public static IEnumerable<AccessSpecifierDecl> GetSpecifiers(this Class _class)
        {
            for (var x = 0; x < _class.BasesCount; x++)
            {
                var specifier = _class.getSpecifiers((uint)x);
                yield return specifier;
            }
        }

        public static IEnumerable<Enumeration.Item> GetItems(this Enumeration enumeration)
        {
            for (var x = 0; x < enumeration.ItemsCount; x++)
            {
                var item = enumeration.getItems((uint)x);
                yield return item;
            }
        }

        public static IEnumerable<VTableComponent> GetComponents(this VTableLayout vTableLayout)
        {
            for (var x = 0; x < vTableLayout.ComponentsCount; x++)
            {
                var component = vTableLayout.getComponents((uint)x);
                yield return component;
            }
        }

        public static IEnumerable<TemplateArgument> GetArguments(this TemplateSpecializationType type)
        {
            for (var x = 0; x < type.ArgumentsCount; x++)
            {
                var argument = type.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<TemplateArgument> GetArguments(this VarTemplateSpecialization varTemplateSpecialization)
        {
            for (var x = 0; x < varTemplateSpecialization.ArgumentsCount; x++)
            {
                var argument = varTemplateSpecialization.getArguments((uint)x);
                yield return argument;
            }
        }

        public static IEnumerable<TemplateArgument> GetArguments(this DependentTemplateSpecializationType type)
        {
            for (var x = 0; x < type.ArgumentsCount; x++)
            {
                var argument = type.getArguments((uint)x);
                yield return argument;
            }
        }

        public static CppSharp.Parser.AST.Expression GetRealExpression(this CppSharp.Parser.AST.Expression expression)
        {
            switch (expression.Class)
            {
                case StatementClass.BinaryOperator:
                    return BinaryOperator.__CreateInstance(expression.__Instance);
                case StatementClass.CallExprClass:
                    return CallExpr.__CreateInstance(expression.__Instance);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.Template GetRealTemplate(this CppSharp.Parser.AST.Template template)
        {
            switch (template.Kind)
            {
                case DeclarationKind.TemplateTemplateParm:
                    return TemplateTemplateParameter.__CreateInstance(template.__Instance);
                case DeclarationKind.TypeAliasTemplate:
                    return TypeAliasTemplate.__CreateInstance(template.__Instance);
                case DeclarationKind.ClassTemplate:
                    return ClassTemplate.__CreateInstance(template.__Instance);
                case DeclarationKind.FunctionTemplate:
                    return FunctionTemplate.__CreateInstance(template.__Instance);
                case DeclarationKind.VarTemplate:
                    return VarTemplate.__CreateInstance(template.__Instance);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.Class GetRealClass(this CppSharp.Parser.AST.Class _class)
        {
            switch (_class.Kind)
            {
                case DeclarationKind.ClassTemplatePartialSpecialization:
                    return ClassTemplatePartialSpecialization.__CreateInstance(_class.__Instance);
                case DeclarationKind.ClassTemplateSpecialization:
                    return ClassTemplateSpecialization.__CreateInstance(_class.__Instance);
                case DeclarationKind.Class:
                    return _class;
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.PreprocessedEntity GetRealPreprocessedEntity(this CppSharp.Parser.AST.PreprocessedEntity entity)
        {
            switch (entity.Kind)
            {
                case DeclarationKind.MacroDefinition:
                    return CppSharp.Parser.AST.MacroDefinition.__CreateInstance(entity.__Instance);
                case DeclarationKind.MacroExpansion:
                    return CppSharp.Parser.AST.MacroExpansion.__CreateInstance(entity.__Instance);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.Comment GetRealComment(this CppSharp.Parser.AST.Comment comment)
        {
            switch (comment.Kind)
            {
                case CommentKind.FullComment:
                    return CppSharp.Parser.AST.FullComment.__CreateInstance(comment.__Instance);
                case CommentKind.InlineContentComment:
                    return CppSharp.Parser.AST.InlineContentComment.__CreateInstance(comment.__Instance);
                case CommentKind.VerbatimLineComment:
                    return CppSharp.Parser.AST.VerbatimLineComment.__CreateInstance(comment.__Instance);
                case CommentKind.VerbatimBlockLineComment:
                    return CppSharp.Parser.AST.VerbatimBlockLineComment.__CreateInstance(comment.__Instance);
                case CommentKind.BlockCommandComment:
                    return CppSharp.Parser.AST.BlockCommandComment.__CreateInstance(comment.__Instance);
                case CommentKind.ParagraphComment:
                    return CppSharp.Parser.AST.ParagraphComment.__CreateInstance(comment.__Instance);
                case CommentKind.ParamCommandComment:
                    return CppSharp.Parser.AST.ParamCommandComment.__CreateInstance(comment.__Instance);
                case CommentKind.TParamCommandComment:
                    return CppSharp.Parser.AST.TParamCommandComment.__CreateInstance(comment.__Instance);
                case CommentKind.BlockContentComment:
                    return CppSharp.Parser.AST.BlockContentComment.__CreateInstance(comment.__Instance);
                case CommentKind.VerbatimBlockComment:
                    return CppSharp.Parser.AST.VerbatimBlockComment.__CreateInstance(comment.__Instance);
                case CommentKind.HTMLTagComment:
                    return CppSharp.Parser.AST.HTMLTagComment.__CreateInstance(comment.__Instance);
                case CommentKind.HTMLStartTagComment:
                    return CppSharp.Parser.AST.HTMLStartTagComment.__CreateInstance(comment.__Instance);
                case CommentKind.HTMLEndTagComment:
                    return CppSharp.Parser.AST.HTMLEndTagComment.__CreateInstance(comment.__Instance);
                case CommentKind.TextComment:
                    return CppSharp.Parser.AST.TextComment.__CreateInstance(comment.__Instance);
                case CommentKind.InlineCommandComment:
                    return CppSharp.Parser.AST.InlineCommandComment.__CreateInstance(comment.__Instance);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.Type GetRealType(this CppSharp.Parser.AST.Type type)
        {
            switch (type.Kind)
            {
                case TypeKind.Tag:
                    return CppSharp.Parser.AST.TagType.__CreateInstance(type.__Instance);
                case TypeKind.Array:
                    return CppSharp.Parser.AST.ArrayType.__CreateInstance(type.__Instance);
                case TypeKind.Function:
                    return CppSharp.Parser.AST.FunctionType.__CreateInstance(type.__Instance);
                case TypeKind.Pointer:
                    return CppSharp.Parser.AST.PointerType.__CreateInstance(type.__Instance);
                case TypeKind.MemberPointer:
                    return CppSharp.Parser.AST.MemberPointerType.__CreateInstance(type.__Instance);
                case TypeKind.Typedef:
                    return CppSharp.Parser.AST.TypedefType.__CreateInstance(type.__Instance);
                case TypeKind.Attributed:
                    return CppSharp.Parser.AST.AttributedType.__CreateInstance(type.__Instance);
                case TypeKind.Decayed:
                    return CppSharp.Parser.AST.DecayedType.__CreateInstance(type.__Instance);
                case TypeKind.TemplateSpecialization:
                    return CppSharp.Parser.AST.TemplateSpecializationType.__CreateInstance(type.__Instance);
                case TypeKind.DependentTemplateSpecialization:
                    return CppSharp.Parser.AST.DependentTemplateSpecializationType.__CreateInstance(type.__Instance);
                case TypeKind.TemplateParameter:
                    return CppSharp.Parser.AST.TemplateParameterType.__CreateInstance(type.__Instance);
                case TypeKind.TemplateParameterSubstitution:
                    return CppSharp.Parser.AST.TemplateParameterSubstitutionType.__CreateInstance(type.__Instance);
                case TypeKind.InjectedClassName:
                    return CppSharp.Parser.AST.InjectedClassNameType.__CreateInstance(type.__Instance);
                case TypeKind.DependentName:
                    return CppSharp.Parser.AST.DependentNameType.__CreateInstance(type.__Instance);
                case TypeKind.PackExpansion:
                    return CppSharp.Parser.AST.PackExpansionType.__CreateInstance(type.__Instance);
                case TypeKind.Builtin:
                    return CppSharp.Parser.AST.BuiltinType.__CreateInstance(type.__Instance);
                case TypeKind.UnaryTransform:
                    return CppSharp.Parser.AST.UnaryTransformType.__CreateInstance(type.__Instance);
                case TypeKind.Vector:
                    return CppSharp.Parser.AST.VectorType.__CreateInstance(type.__Instance);
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static CppSharp.Parser.AST.Declaration GetRealDeclaration(this CppSharp.Parser.AST.Declaration type)
        {
            if (type == null)
            {
                return null;
            }
            else
            {
                switch (type.Kind)
                {
                    case DeclarationKind.DeclarationContext:
                        return CppSharp.Parser.AST.DeclarationContext.__CreateInstance(type.__Instance);
                    case DeclarationKind.Typedef:
                        return CppSharp.Parser.AST.TypedefDecl.__CreateInstance(type.__Instance);
                    case DeclarationKind.TypeAlias:
                        return CppSharp.Parser.AST.TypeAlias.__CreateInstance(type.__Instance);
                    case DeclarationKind.Parameter:
                        return CppSharp.Parser.AST.Parameter.__CreateInstance(type.__Instance);
                    case DeclarationKind.Function:
                        return CppSharp.Parser.AST.Function.__CreateInstance(type.__Instance);
                    case DeclarationKind.Method:
                        return CppSharp.Parser.AST.Method.__CreateInstance(type.__Instance);
                    case DeclarationKind.Enumeration:
                        return CppSharp.Parser.AST.Enumeration.__CreateInstance(type.__Instance);
                    case DeclarationKind.EnumerationItem:
                        return CppSharp.Parser.AST.Enumeration.Item.__CreateInstance(type.__Instance);
                    case DeclarationKind.Variable:
                        return CppSharp.Parser.AST.Variable.__CreateInstance(type.__Instance);
                    case DeclarationKind.Field:
                        return CppSharp.Parser.AST.Field.__CreateInstance(type.__Instance);
                    case DeclarationKind.AccessSpecifier:
                        return CppSharp.Parser.AST.AccessSpecifierDecl.__CreateInstance(type.__Instance);
                    case DeclarationKind.Class:
                        return CppSharp.Parser.AST.Class.__CreateInstance(type.__Instance);
                    case DeclarationKind.Template:
                        return CppSharp.Parser.AST.Template.__CreateInstance(type.__Instance);
                    case DeclarationKind.TypeAliasTemplate:
                        return CppSharp.Parser.AST.TypeAliasTemplate.__CreateInstance(type.__Instance);
                    case DeclarationKind.ClassTemplate:
                        return CppSharp.Parser.AST.ClassTemplate.__CreateInstance(type.__Instance);
                    case DeclarationKind.ClassTemplateSpecialization:
                        return CppSharp.Parser.AST.ClassTemplateSpecialization.__CreateInstance(type.__Instance);
                    case DeclarationKind.ClassTemplatePartialSpecialization:
                        return CppSharp.Parser.AST.ClassTemplatePartialSpecialization.__CreateInstance(type.__Instance);
                    case DeclarationKind.FunctionTemplate:
                        return CppSharp.Parser.AST.FunctionTemplate.__CreateInstance(type.__Instance);
                    case DeclarationKind.Namespace:
                        return CppSharp.Parser.AST.Namespace.__CreateInstance(type.__Instance);
                    case DeclarationKind.TranslationUnit:
                        return CppSharp.Parser.AST.TranslationUnit.__CreateInstance(type.__Instance);
                    case DeclarationKind.Friend:
                        return CppSharp.Parser.AST.Friend.__CreateInstance(type.__Instance);
                    case DeclarationKind.TemplateTemplateParm:
                        return CppSharp.Parser.AST.TemplateTemplateParameter.__CreateInstance(type.__Instance);
                    case DeclarationKind.NonTypeTemplateParm:
                        return CppSharp.Parser.AST.NonTypeTemplateParameter.__CreateInstance(type.__Instance);
                    case DeclarationKind.VarTemplate:
                        return CppSharp.Parser.AST.VarTemplate.__CreateInstance(type.__Instance);
                    case DeclarationKind.VarTemplateSpecialization:
                        return CppSharp.Parser.AST.VarTemplateSpecialization.__CreateInstance(type.__Instance);
                    case DeclarationKind.VarTemplatePartialSpecialization:
                        return CppSharp.Parser.AST.VarTemplatePartialSpecialization.__CreateInstance(type.__Instance);
                    default:
                        Debugger.Break();
                        return null;
                }
            }
        }
    }
}
