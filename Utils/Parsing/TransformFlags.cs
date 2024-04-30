using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing
{
    public enum TransformFlags
    {
        None = 0,

        // Facts
        // - Flags used to indicate that a node or subtree contains syntax that requires transformation.
        TypeScript = 1 << 0,
        ContainsTypeScript = 1 << 1,
        ContainsJsx = 1 << 2,
        ContainsESNext = 1 << 3,
        ContainsES2017 = 1 << 4,
        ContainsES2016 = 1 << 5,
        ES2015 = 1 << 6,
        ContainsES2015 = 1 << 7,
        Generator = 1 << 8,
        ContainsGenerator = 1 << 9,
        DestructuringAssignment = 1 << 10,
        ContainsDestructuringAssignment = 1 << 11,

        // Markers
        // - Flags used to indicate that a subtree contains a specific transformation.
        ContainsDecorators = 1 << 12,
        ContainsPropertyInitializer = 1 << 13,
        ContainsLexicalThis = 1 << 14,
        ContainsCapturedLexicalThis = 1 << 15,
        ContainsLexicalThisInComputedPropertyName = 1 << 16,
        ContainsDefaultValueAssignments = 1 << 17,
        ContainsParameterPropertyAssignments = 1 << 18,
        ContainsSpread = 1 << 19,
        ContainsObjectSpread = 1 << 20,
        ContainsRest = ContainsSpread,
        ContainsObjectRest = ContainsObjectSpread,
        ContainsComputedPropertyName = 1 << 21,
        ContainsBlockScopedBinding = 1 << 22,
        ContainsBindingPattern = 1 << 23,
        ContainsYield = 1 << 24,
        ContainsHoistedDeclarationOrCompletion = 1 << 25,

        HasComputedFlags = 1 << 29, // Transform flags have been computed.

        // Assertions
        // - Bitmasks that are used to assert facts about the syntax of a node and its subtree.
        AssertTypeScript = TypeScript | ContainsTypeScript,
        AssertJsx = ContainsJsx,
        AssertESNext = ContainsESNext,
        AssertES2017 = ContainsES2017,
        AssertES2016 = ContainsES2016,
        AssertES2015 = ES2015 | ContainsES2015,
        AssertGenerator = Generator | ContainsGenerator,
        AssertDestructuringAssignment = DestructuringAssignment | ContainsDestructuringAssignment,

        // Scope Exclusions
        // - Bitmasks that exclude flags from propagating out of a specific context
        //   into the subtree flags of their container.
        NodeExcludes = TypeScript | ES2015 | DestructuringAssignment | Generator | HasComputedFlags,
        ArrowFunctionExcludes = NodeExcludes | ContainsDecorators | ContainsDefaultValueAssignments | ContainsLexicalThis | ContainsParameterPropertyAssignments | ContainsBlockScopedBinding | ContainsYield | ContainsHoistedDeclarationOrCompletion | ContainsBindingPattern | ContainsObjectRest,
        FunctionExcludes = NodeExcludes | ContainsDecorators | ContainsDefaultValueAssignments | ContainsCapturedLexicalThis | ContainsLexicalThis | ContainsParameterPropertyAssignments | ContainsBlockScopedBinding | ContainsYield | ContainsHoistedDeclarationOrCompletion | ContainsBindingPattern | ContainsObjectRest,
        ConstructorExcludes = NodeExcludes | ContainsDefaultValueAssignments | ContainsLexicalThis | ContainsCapturedLexicalThis | ContainsBlockScopedBinding | ContainsYield | ContainsHoistedDeclarationOrCompletion | ContainsBindingPattern | ContainsObjectRest,
        MethodOrAccessorExcludes = NodeExcludes | ContainsDefaultValueAssignments | ContainsLexicalThis | ContainsCapturedLexicalThis | ContainsBlockScopedBinding | ContainsYield | ContainsHoistedDeclarationOrCompletion | ContainsBindingPattern | ContainsObjectRest,
        ClassExcludes = NodeExcludes | ContainsDecorators | ContainsPropertyInitializer | ContainsLexicalThis | ContainsCapturedLexicalThis | ContainsComputedPropertyName | ContainsParameterPropertyAssignments | ContainsLexicalThisInComputedPropertyName,
        ModuleExcludes = NodeExcludes | ContainsDecorators | ContainsLexicalThis | ContainsCapturedLexicalThis | ContainsBlockScopedBinding | ContainsHoistedDeclarationOrCompletion,
        TypeExcludes = ~ContainsTypeScript,
        ObjectLiteralExcludes = NodeExcludes | ContainsDecorators | ContainsComputedPropertyName | ContainsLexicalThisInComputedPropertyName | ContainsObjectSpread,
        ArrayLiteralOrCallOrNewExcludes = NodeExcludes | ContainsSpread,
        VariableDeclarationListExcludes = NodeExcludes | ContainsBindingPattern | ContainsObjectRest,
        ParameterExcludes = NodeExcludes,
        CatchClauseExcludes = NodeExcludes | ContainsObjectRest,
        BindingPatternExcludes = NodeExcludes | ContainsRest,

        // Masks
        // - Additional bitmasks
        TypeScriptClassSyntaxMask = ContainsParameterPropertyAssignments | ContainsPropertyInitializer | ContainsDecorators,
        ES2015FunctionSyntaxMask = ContainsCapturedLexicalThis | ContainsDefaultValueAssignments
    }
}
