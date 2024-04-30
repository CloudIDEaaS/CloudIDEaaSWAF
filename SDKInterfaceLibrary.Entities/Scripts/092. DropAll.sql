USE [SDKInterfaceLibrary]
GO

/*************** [dbo].[tblSDKHeaderIntegerValue] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderIntegerValue_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderIntegerValue DROP CONSTRAINT [FK_tblSDKHeaderIntegerValue_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderType DROP CONSTRAINT [FK_tblSDKHeaderType_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderVTableLayout] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVTableLayout_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVTableLayout DROP CONSTRAINT [FK_tblSDKHeaderVTableLayout_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderBlockCommandCommentArgument] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderBlockCommandComment_OwningBlockCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockCommandCommentArgument DROP CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderBlockCommandComment_OwningBlockCommandComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockCommandCommentArgument DROP CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderBuiltinType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBuiltinType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBuiltinType DROP CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderBuiltinType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBuiltinType DROP CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderBuiltinTypeExpression] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBuiltinTypeExpression DROP CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderExpression_Expression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBuiltinTypeExpression DROP CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderExpression_Expression]
END

/*************** [dbo].[tblSDKHeaderCallExpr] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderCallExpr_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderCallExpr DROP CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderCallExpr_tblSDKHeaderExpression_Expression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderCallExpr DROP CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderExpression_Expression]
END

/*************** [dbo].[tblSDKHeaderClassLayout] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderClassLayout_tblSDKHeaderVTableLayout_LayoutVTableLayout', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassLayout DROP CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderVTableLayout_LayoutVTableLayout]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassLayout_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassLayout DROP CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderClassTemplate] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplate_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplate DROP CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplate_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplate DROP CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderTemplate_Template]
END

/*************** [dbo].[tblSDKHeaderClassTemplatePartialSpecialization] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplatePartialSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderClassTemplateSpecialization_ClassTemplateSpecialization', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplatePartialSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderClassTemplateSpecialization_ClassTemplateSpecialization]
END

/*************** [dbo].[tblSDKHeaderComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderComment_tblSDKHeaderDeclaration_ParentDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderComment DROP CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderDeclaration_ParentDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderComment DROP CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderCXXConstructExpr] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderCXXConstructExpr DROP CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderExpression_Expression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderCXXConstructExpr DROP CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderExpression_Expression]
END

/*************** [dbo].[tblSDKHeaderExpression] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderExpression_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderExpression DROP CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderExpression_tblSDKHeaderStatement_Statement', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderExpression DROP CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderStatement_Statement]
END

/*************** [dbo].[tblSDKHeaderFunctionTemplate] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionTemplate DROP CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionTemplate DROP CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderTemplate_Template]
END

/*************** [dbo].[tblSDKHeaderHeader] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderHeader_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHeader DROP CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderHeader_tblSDKHeaderNamespace_Namespace', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHeader DROP CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderNamespace_Namespace]
END

If (IndexProperty(Object_Id('tblSDKHeaderHeader'), 'IX_tblSDKHeaderHeader_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderHeader_Name_Index] ON [dbo].[tblSDKHeaderHeader]
END

If (IndexProperty(Object_Id('tblSDKHeaderHeader'), 'IX_tblSDKHeaderHeader_FileName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderHeader_FileName_Index] ON [dbo].[tblSDKHeaderHeader]
END

/*************** [dbo].[tblSDKHeaderHTMLEndTagComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLEndTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLEndTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment]
END

If (IndexProperty(Object_Id('tblSDKHeaderHTMLEndTagComment'), 'IX_tblSDKHeaderHTMLEndTagComment_TagName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderHTMLEndTagComment_TagName_Index] ON [dbo].[tblSDKHeaderHTMLEndTagComment]
END

/*************** [dbo].[tblSDKHeaderHTMLStartTagComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLStartTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLStartTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment]
END

If (IndexProperty(Object_Id('tblSDKHeaderHTMLStartTagComment'), 'IX_tblSDKHeaderHTMLStartTagComment_TagName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderHTMLStartTagComment_TagName_Index] ON [dbo].[tblSDKHeaderHTMLStartTagComment]
END

/*************** [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderHTMLStartTagComment_OwningHTMLStartTagComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLStartTagCommentAttribute DROP CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderHTMLStartTagComment_OwningHTMLStartTagComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLStartTagCommentAttribute DROP CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderFile_HeaderFile]
END

If (IndexProperty(Object_Id('tblSDKHeaderHTMLStartTagCommentAttribute'), 'IX_tblSDKHeaderHTMLStartTagCommentAttribute_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderHTMLStartTagCommentAttribute_Name_Index] ON [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute]
END

/*************** [dbo].[tblSDKHeaderHTMLTagComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderInlineContentComment_InlineContentComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderHTMLTagComment DROP CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderInlineContentComment_InlineContentComment]
END

/*************** [dbo].[tblSDKHeaderInlineCommandCommentArgument] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderInlineCommandComment_OwningInlineCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineCommandCommentArgument DROP CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderInlineCommandComment_OwningInlineCommandComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineCommandCommentArgument DROP CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderMacroDefinition] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderMacroDefinition_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacroDefinition DROP CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderMacroDefinition_tblSDKHeaderPreprocessedEntity_PreprocessedEntity', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacroDefinition DROP CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderPreprocessedEntity_PreprocessedEntity]
END

If (IndexProperty(Object_Id('tblSDKHeaderMacroDefinition'), 'IX_tblSDKHeaderMacroDefinition_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderMacroDefinition_Name_Index] ON [dbo].[tblSDKHeaderMacroDefinition]
END

/*************** [dbo].[tblSDKHeaderPackExpansionType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderPackExpansionType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPackExpansionType DROP CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderPackExpansionType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPackExpansionType DROP CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderParagraphComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderParagraphComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParagraphComment DROP CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderParagraphComment_tblSDKHeaderBlockContentComment_BlockContentComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParagraphComment DROP CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderBlockContentComment_BlockContentComment]
END

/*************** [dbo].[tblSDKHeaderPreprocessedEntity] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderDeclaration_OwningDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPreprocessedEntity DROP CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderDeclaration_OwningDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPreprocessedEntity DROP CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderQualifiedType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderQualifiedType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderQualifiedType DROP CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderType_Type]
END

IF (OBJECT_ID('FK_tblSDKHeaderQualifiedType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderQualifiedType DROP CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderRawComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderRawComment_tblSDKHeaderFullComment_CommentBlockFullComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderRawComment DROP CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFullComment_CommentBlockFullComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderRawComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderRawComment DROP CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderStatement] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderStatement_tblSDKHeaderDeclaration_DeclDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderStatement DROP CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderDeclaration_DeclDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderStatement_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderStatement DROP CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderTemplateTemplateParameter] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderTemplate_Template]
END

/*************** [dbo].[tblSDKHeaderTextComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTextComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTextComment DROP CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTextComment_tblSDKHeaderInlineContentComment_InlineContentComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTextComment DROP CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderInlineContentComment_InlineContentComment]
END

/*************** [dbo].[tblSDKHeaderTParamCommandComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTParamCommandComment DROP CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTParamCommandComment DROP CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment]
END

/*************** [dbo].[tblSDKHeaderTranslationUnit] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTranslationUnit_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTranslationUnit DROP CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTranslationUnit_tblSDKHeaderNamespace_Namespace', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTranslationUnit DROP CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderNamespace_Namespace]
END

If (IndexProperty(Object_Id('tblSDKHeaderTranslationUnit'), 'IX_tblSDKHeaderTranslationUnit_FileName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderTranslationUnit_FileName_Index] ON [dbo].[tblSDKHeaderTranslationUnit]
END

/*************** [dbo].[tblSDKHeaderTypeAliasTemplate] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAliasTemplate DROP CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAliasTemplate DROP CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderTemplate_Template]
END

/*************** [dbo].[tblSDKHeaderTypeDefDecl] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDefDecl DROP CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDefDecl DROP CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl]
END

/*************** [dbo].[tblSDKHeaderVarTemplate] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplate_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplate DROP CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplate_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplate DROP CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderTemplate_Template]
END

/*************** [dbo].[tblSDKHeaderVerbatimBlockComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimBlockComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderBlockCommandComment_BlockCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimBlockComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderBlockCommandComment_BlockCommandComment]
END

/*************** [dbo].[tblSDKHeaderVerbatimLineComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimLineComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderBlockCommandComment_BlockCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimLineComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderBlockCommandComment_BlockCommandComment]
END

/*************** [dbo].[tblSDKHeaderAccessSpecifierDecl] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderClass_OwningClass', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAccessSpecifierDecl DROP CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderClass_OwningClass]
END

IF (OBJECT_ID('FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAccessSpecifierDecl DROP CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAccessSpecifierDecl DROP CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderArrayType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderArrayType_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderArrayType DROP CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderArrayType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderArrayType DROP CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderArrayType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderArrayType DROP CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderBaseClassSpecifier] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderClass_OwningClass', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBaseClassSpecifier DROP CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderClass_OwningClass]
END

IF (OBJECT_ID('FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBaseClassSpecifier DROP CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderType_Type]
END

IF (OBJECT_ID('FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBaseClassSpecifier DROP CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderBlockCommandComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderParagraphComment_ParagraphComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockCommandComment DROP CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderParagraphComment_ParagraphComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockCommandComment DROP CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderBlockContentComment_BlockContentComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockCommandComment DROP CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderBlockContentComment_BlockContentComment]
END

/*************** [dbo].[tblSDKHeaderBlockContentComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_OwningComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockContentComment DROP CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_OwningComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderBlockContentComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockContentComment DROP CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBlockContentComment DROP CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_Comment]
END

/*************** [dbo].[tblSDKHeaderDeclaration] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_OwningDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclaration DROP CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_OwningDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_CompleteDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclaration DROP CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_CompleteDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderDeclaration_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclaration DROP CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderFile_HeaderFile]
END

If (IndexProperty(Object_Id('tblSDKHeaderDeclaration'), 'IX_tblSDKHeaderDeclaration_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderDeclaration_Name_Index] ON [dbo].[tblSDKHeaderDeclaration]
END

/*************** [dbo].[tblSDKHeaderDeclarationContext] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderDeclarationContext_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclarationContext DROP CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderComment_Comment]
END

IF (OBJECT_ID('FK_tblSDKHeaderDeclarationContext_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclarationContext DROP CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderDeclarationContext_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDeclarationContext DROP CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderDeclaration_Declaration]
END

If (IndexProperty(Object_Id('tblSDKHeaderDeclarationContext'), 'IX_tblSDKHeaderDeclarationContext_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderDeclarationContext_Name_Index] ON [dbo].[tblSDKHeaderDeclarationContext]
END

/*************** [dbo].[tblSDKHeaderDependentNameType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderDependentNameType_tblSDKHeaderQualifiedType_DesugaredQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentNameType DROP CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderQualifiedType_DesugaredQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderDependentNameType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentNameType DROP CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderDependentNameType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentNameType DROP CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderDependentTemplateSpecializationType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugaredQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugaredQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDependentTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderFunctionTemplateSpecialization] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunctionTemplate_OwningFunctionTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunctionTemplate_OwningFunctionTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunction_SpecializedFunction', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunction_SpecializedFunction]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderFunctionType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFunctionType_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionType DROP CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunctionType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionType DROP CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunctionType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunctionType DROP CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderInlineCommandComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderIntegerValue_CommandIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineCommandComment DROP CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderIntegerValue_CommandIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineCommandComment DROP CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderInlineContentComment_InlineContentComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineCommandComment DROP CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderInlineContentComment_InlineContentComment]
END

/*************** [dbo].[tblSDKHeaderInlineContentComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_OwningComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineContentComment DROP CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_OwningComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderInlineContentComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineContentComment DROP CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInlineContentComment DROP CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_Comment]
END

/*************** [dbo].[tblSDKHeaderMacro] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderMacro_tblSDKHeaderNamespace_OwningNamespace', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacro DROP CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderNamespace_OwningNamespace]
END

IF (OBJECT_ID('FK_tblSDKHeaderMacro_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacro DROP CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderMacro_tblSDKHeaderPreprocessedEntity_PreprocessedEntity', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacro DROP CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderPreprocessedEntity_PreprocessedEntity]
END

If (IndexProperty(Object_Id('tblSDKHeaderMacro'), 'IX_tblSDKHeaderMacro_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderMacro_Name_Index] ON [dbo].[tblSDKHeaderMacro]
END

/*************** [dbo].[tblSDKHeaderMacroExpansion] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderMacroExpansion_tblSDKHeaderMacroDefinition_DefinitionMacroDefinition', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacroExpansion DROP CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderMacroDefinition_DefinitionMacroDefinition]
END

IF (OBJECT_ID('FK_tblSDKHeaderMacroExpansion_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacroExpansion DROP CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderMacroExpansion_tblSDKHeaderPreprocessedEntity_PreprocessedEntity', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMacroExpansion DROP CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderPreprocessedEntity_PreprocessedEntity]
END

If (IndexProperty(Object_Id('tblSDKHeaderMacroExpansion'), 'IX_tblSDKHeaderMacroExpansion_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderMacroExpansion_Name_Index] ON [dbo].[tblSDKHeaderMacroExpansion]
END

/*************** [dbo].[tblSDKHeaderMemberPointerType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderMemberPointerType_tblSDKHeaderQualifiedType_PointeeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMemberPointerType DROP CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderQualifiedType_PointeeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderMemberPointerType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMemberPointerType DROP CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderMemberPointerType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMemberPointerType DROP CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderNamespace] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNamespace DROP CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderNamespace_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNamespace DROP CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_DeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNamespace DROP CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_DeclarationContext]
END

If (IndexProperty(Object_Id('tblSDKHeaderNamespace'), 'IX_tblSDKHeaderNamespace_Name_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderNamespace_Name_Index] ON [dbo].[tblSDKHeaderNamespace]
END

/*************** [dbo].[tblSDKHeaderParamCommandComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderParamCommandComment_tblSDKHeaderIntegerValue_ParamIndexIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParamCommandComment DROP CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderIntegerValue_ParamIndexIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderParamCommandComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParamCommandComment DROP CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParamCommandComment DROP CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment]
END

/*************** [dbo].[tblSDKHeaderPointerType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderPointerType_tblSDKHeaderQualifiedType_QualifiedPointeeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPointerType DROP CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderQualifiedType_QualifiedPointeeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderPointerType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPointerType DROP CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderPointerType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderPointerType DROP CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTagType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTagType_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTagType DROP CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderDeclaration_Declaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderTagType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTagType DROP CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTagType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTagType DROP CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderQualifiedType_ReplacementQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterSubstitutionType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderQualifiedType_ReplacementQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterSubstitutionType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterSubstitutionType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTypeDefNameDecl] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDefNameDecl DROP CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDefNameDecl DROP CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDefNameDecl DROP CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderTypedefType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypedefType_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypedefType DROP CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderDeclaration_Declaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypedefType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypedefType DROP CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypedefType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypedefType DROP CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTypeTemplateParameter] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderQualifiedType_DefaultArgumentQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderQualifiedType_DefaultArgumentQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter]
END

/*************** [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplate_OwningVarTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplatePartialSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplate_OwningVarTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplatePartialSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplateSpecialization_VarTemplateSpecialization', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplatePartialSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplateSpecialization_VarTemplateSpecialization]
END

/*************** [dbo].[tblSDKHeaderVerbatimBlockLineComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_OwningComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimBlockLineComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_OwningComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimBlockLineComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVerbatimBlockLineComment DROP CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_Comment]
END

/*************** [dbo].[tblSDKHeaderAttributedType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_EquivalentQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAttributedType DROP CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_EquivalentQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_ModifiedQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAttributedType DROP CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_ModifiedQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderAttributedType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAttributedType DROP CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderAttributedType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderAttributedType DROP CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderBinaryOperator] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_LHSExpression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBinaryOperator DROP CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_LHSExpression]
END

IF (OBJECT_ID('FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_RHSExpression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBinaryOperator DROP CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_RHSExpression]
END

IF (OBJECT_ID('FK_tblSDKHeaderBinaryOperator_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBinaryOperator DROP CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_Expression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderBinaryOperator DROP CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_Expression]
END

/*************** [dbo].[tblSDKHeaderClass] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClass DROP CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderClass_tblSDKHeaderClassLayout_LayoutClassLayout', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClass DROP CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderClassLayout_LayoutClassLayout]
END

IF (OBJECT_ID('FK_tblSDKHeaderClass_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClass DROP CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_DeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClass DROP CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_DeclarationContext]
END

If (IndexProperty(Object_Id('tblSDKHeaderClass'), 'IX_tblSDKHeaderClass_ClassName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderClass_ClassName_Index] ON [dbo].[tblSDKHeaderClass]
END

/*************** [dbo].[tblSDKHeaderClassTemplateSpecialization] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_OwningClassTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_OwningClassTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_TemplatedDeclClassTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_TemplatedDeclClassTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClass_Class', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderClassTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClass_Class]
END

/*************** [dbo].[tblSDKHeaderEnumeration] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumeration DROP CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumeration_tblSDKHeaderType_BuiltInType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumeration DROP CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderType_BuiltInType]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumeration_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumeration DROP CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_DeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumeration DROP CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_DeclarationContext]
END

/*************** [dbo].[tblSDKHeaderEnumerationItem] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderEnumerationItem_tblSDKHeaderEnumeration_OwningEnumeration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumerationItem DROP CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderEnumeration_OwningEnumeration]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumerationItem_tblSDKHeaderIntegerValue_ValueIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumerationItem DROP CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderIntegerValue_ValueIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumerationItem_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumerationItem DROP CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderEnumerationItem_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderEnumerationItem DROP CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderFriend] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFriend_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFriend DROP CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_FriendDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFriend DROP CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_FriendDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderFriend_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFriend DROP CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFriend DROP CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderFullComment] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFullComment_tblSDKHeaderComment_OwningComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFullComment DROP CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_OwningComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderFullComment_tblSDKHeaderRawComment_OwningRawComment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFullComment DROP CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderRawComment_OwningRawComment]
END

IF (OBJECT_ID('FK_tblSDKHeaderFullComment_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFullComment DROP CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderFullComment_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFullComment DROP CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_Comment]
END

/*************** [dbo].[tblSDKHeaderInjectedClassNameType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderClass_Class', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInjectedClassNameType DROP CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderClass_Class]
END

IF (OBJECT_ID('FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderQualifiedType_InjectedSpecializationTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInjectedClassNameType DROP CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderQualifiedType_InjectedSpecializationTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInjectedClassNameType DROP CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderInjectedClassNameType DROP CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderMethod] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderMethod_tblSDKHeaderClass_OwningClass', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMethod DROP CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderClass_OwningClass]
END

IF (OBJECT_ID('FK_tblSDKHeaderMethod_tblSDKHeaderQualifiedType_ConversionTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMethod DROP CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderQualifiedType_ConversionTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderMethod_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMethod DROP CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderMethod_tblSDKHeaderFunction_Function', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderMethod DROP CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFunction_Function]
END

/*************** [dbo].[tblSDKHeaderNonTypeTemplateParameter] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderExpression_DefaultArgumentExpression', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNonTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderExpression_DefaultArgumentExpression]
END

IF (OBJECT_ID('FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderIntegerValue_PositionIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNonTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderIntegerValue_PositionIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNonTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderNonTypeTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter]
END

/*************** [dbo].[tblSDKHeaderTemplate] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplate_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplate DROP CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_TemplateDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplate DROP CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_TemplateDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplate_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplate DROP CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplate DROP CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderTemplateParameter] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_DepthIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_DepthIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_IndexIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_IndexIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameter_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameter_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameter DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderTemplateSpecializationType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderTemplate_Template', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderTemplate_Template]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugardQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugardQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateSpecializationType DROP CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTypeDef] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeDef_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDef DROP CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDef_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDef DROP CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDef_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDef DROP CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeDef_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeDef DROP CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl]
END

/*************** [dbo].[tblSDKHeaderUnaryTransformType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_DesugardedQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderUnaryTransformType DROP CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_DesugardedQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_BaseTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderUnaryTransformType DROP CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_BaseTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderUnaryTransformType DROP CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderUnaryTransformType DROP CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderVariable] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVariable_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVariable DROP CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderVariable_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVariable DROP CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderVariable_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVariable DROP CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVariable_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVariable DROP CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderVarTemplateSpecialization] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderDeclaration_OwningDeclaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderDeclaration_OwningDeclaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVarTemplate_TemplateDeclVarTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVarTemplate_TemplateDeclVarTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVariable_Variable', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVarTemplateSpecialization DROP CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVariable_Variable]
END

/*************** [dbo].[tblSDKHeaderVectorType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVectorType_tblSDKHeaderQualifiedType_ElementTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVectorType DROP CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderQualifiedType_ElementTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderVectorType_tblSDKHeaderIntegerValue_NumElementsIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVectorType DROP CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderIntegerValue_NumElementsIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderVectorType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVectorType DROP CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderVectorType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVectorType DROP CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderVTableComponent] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderVTableComponent_tblSDKHeaderVTableLayout_OwningVTableLayout', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVTableComponent DROP CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderVTableLayout_OwningVTableLayout]
END

IF (OBJECT_ID('FK_tblSDKHeaderVTableComponent_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVTableComponent DROP CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderDeclaration_Declaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderVTableComponent_tblSDKHeaderIntegerValue_OffsetIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVTableComponent DROP CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderIntegerValue_OffsetIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderVTableComponent_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderVTableComponent DROP CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderFile_HeaderFile]
END

/*************** [dbo].[tblSDKHeaderDecayedType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_DecayedQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDecayedType DROP CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_DecayedQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_OriginalQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDecayedType DROP CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_OriginalQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_PointeeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDecayedType DROP CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_PointeeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderDecayedType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDecayedType DROP CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderDecayedType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderDecayedType DROP CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderField] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderField_tblSDKHeaderClass_OwningClass', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderField DROP CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderClass_OwningClass]
END

IF (OBJECT_ID('FK_tblSDKHeaderField_tblSDKHeaderIntegerValue_BitWidthIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderField DROP CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderIntegerValue_BitWidthIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderField_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderField DROP CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderField_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderField DROP CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderField_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderField DROP CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderTemplateParameterType] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_DepthIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_DepthIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_IndexIntegerValue', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_IndexIntegerValue]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderTypeTemplateParameter_ParameterTypeTemplateParameter', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderTypeTemplateParameter_ParameterTypeTemplateParameter]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderType_Type', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateParameterType DROP CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderType_Type]
END

/*************** [dbo].[tblSDKHeaderTypeAlias] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTypeAlias_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAlias DROP CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeAlias_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAlias DROP CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeAlias_tblSDKHeaderTemplate_DescribedAliasTemplate', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAlias DROP CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTemplate_DescribedAliasTemplate]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeAlias_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAlias DROP CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderTypeAlias_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTypeAlias DROP CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl]
END

/*************** [dbo].[tblSDKHeaderFunction] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderDeclarationContext_OwningDeclarationContext', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclarationContext_OwningDeclarationContext]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderComment_Comment]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderFunction_InstantiatedFromFunction', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFunction_InstantiatedFromFunction]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderFunction_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderFunction DROP CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclaration_Declaration]
END

If (IndexProperty(Object_Id('tblSDKHeaderFunction'), 'IX_tblSDKHeaderFunction_FunctionName_Index', 'IndexId') IS NOT NULL)
BEGIN
	DROP INDEX [IX_tblSDKHeaderFunction_FunctionName_Index] ON [dbo].[tblSDKHeaderFunction]
END

/*************** [dbo].[tblSDKHeaderParameter] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderFunctionType_OwningFunctionType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunctionType_OwningFunctionType]
END

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderFunction_OwningFunction', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunction_OwningFunction]
END

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderComment_Comment', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderComment_Comment]
END

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderQualifiedType_QualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderQualifiedType_QualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFile_HeaderFile]
END

IF (OBJECT_ID('FK_tblSDKHeaderParameter_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderParameter DROP CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderDeclaration_Declaration]
END

/*************** [dbo].[tblSDKHeaderTemplateArgument] ***************/

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderClassTemplateSpecialization_OwningClassTemplateSpecialization', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderClassTemplateSpecialization_OwningClassTemplateSpecialization]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDependentTemplateSpecializationType_OwningDependentTemplateSpecializationType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDependentTemplateSpecializationType_OwningDependentTemplateSpecializationType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderVarTemplateSpecialization_OwningVarTemplateSpecialization', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderVarTemplateSpecialization_OwningVarTemplateSpecialization]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderTemplateSpecializationType_OwningTemplateSpecializationType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderTemplateSpecializationType_OwningTemplateSpecializationType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFunctionTemplateSpecialization_OwningFunctionTemplateSpecialization', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFunctionTemplateSpecialization_OwningFunctionTemplateSpecialization]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDeclaration_Declaration', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDeclaration_Declaration]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderQualifiedType_TypeQualifiedType', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderQualifiedType_TypeQualifiedType]
END

IF (OBJECT_ID('FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFile_HeaderFile', 'F') IS NOT NULL)
BEGIN
	ALTER TABLE tblSDKHeaderTemplateArgument DROP CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFile_HeaderFile]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderIntegerValue]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderIntegerValue]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVTableLayout]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVTableLayout]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBlockCommandCommentArgument]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBuiltinType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBuiltinType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBuiltinTypeExpression]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderCallExpr]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderCallExpr]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderClassLayout]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderClassLayout]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderClassTemplate]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderClassTemplate]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderClassTemplatePartialSpecialization]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderCXXConstructExpr]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderCXXConstructExpr]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderExpression]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderExpression]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFunctionTemplate]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFunctionTemplate]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderHeader]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderHeader]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderHTMLEndTagComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderHTMLEndTagComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderHTMLStartTagComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderHTMLStartTagComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderHTMLStartTagCommentAttribute]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderHTMLTagComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderHTMLTagComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderInlineCommandCommentArgument]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderMacroDefinition]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderMacroDefinition]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderPackExpansionType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderPackExpansionType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderParagraphComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderParagraphComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderPreprocessedEntity]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderPreprocessedEntity]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderQualifiedType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderQualifiedType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderRawComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderRawComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderStatement]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderStatement]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateTemplateParameter]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTextComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTextComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTParamCommandComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTParamCommandComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTranslationUnit]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTranslationUnit]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeAliasTemplate]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeAliasTemplate]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeDefDecl]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeDefDecl]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVarTemplate]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVarTemplate]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVerbatimBlockComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVerbatimBlockComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVerbatimLineComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVerbatimLineComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderAccessSpecifierDecl]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderArrayType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderArrayType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBaseClassSpecifier]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBaseClassSpecifier]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBlockCommandComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBlockCommandComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBlockContentComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBlockContentComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderDeclaration]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderDeclaration]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderDeclarationContext]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderDeclarationContext]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderDependentNameType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderDependentNameType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderDependentTemplateSpecializationType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFunctionTemplateSpecialization]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFunctionType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFunctionType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderInlineCommandComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderInlineCommandComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderInlineContentComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderInlineContentComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderMacro]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderMacro]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderMacroExpansion]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderMacroExpansion]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderMemberPointerType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderMemberPointerType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderNamespace]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderNamespace]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderParamCommandComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderParamCommandComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderPointerType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderPointerType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTagType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTagType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateParameterSubstitutionType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeDefNameDecl]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeDefNameDecl]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypedefType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypedefType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeTemplateParameter]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeTemplateParameter]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVarTemplatePartialSpecialization]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVerbatimBlockLineComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderAttributedType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderAttributedType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderBinaryOperator]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderBinaryOperator]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderClass]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderClass]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderClassTemplateSpecialization]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderEnumeration]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderEnumeration]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderEnumerationItem]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderEnumerationItem]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFriend]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFriend]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFullComment]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFullComment]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderInjectedClassNameType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderInjectedClassNameType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderMethod]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderMethod]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderNonTypeTemplateParameter]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplate]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplate]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateParameter]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateParameter]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateSpecializationType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateSpecializationType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeDef]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeDef]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderUnaryTransformType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderUnaryTransformType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVariable]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVariable]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVarTemplateSpecialization]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVectorType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVectorType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderVTableComponent]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderVTableComponent]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderDecayedType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderDecayedType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderField]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderField]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateParameterType]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateParameterType]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTypeAlias]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTypeAlias]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderFunction]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderFunction]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderParameter]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderParameter]
END

IF OBJECT_ID (N'[dbo].[tblSDKHeaderTemplateArgument]', N'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[tblSDKHeaderTemplateArgument]
END

