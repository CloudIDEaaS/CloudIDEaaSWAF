USE [SDKInterfaceLibrary]
GO

ALTER TABLE [dbo].[tblSDKHeaderIntegerValue] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderIntegerValue_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableLayout_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderBlockCommandComment_OwningBlockCommandComment] FOREIGN KEY([OwningBlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCallExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCallExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderVTableLayout_LayoutVTableLayout] FOREIGN KEY([LayoutVTableLayoutId])
REFERENCES [dbo].[tblSDKHeaderVTableLayout] ([VTableLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderClassTemplateSpecialization_ClassTemplateSpecialization] FOREIGN KEY([ClassTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderClassTemplateSpecialization] ([ClassTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderDeclaration_ParentDeclaration] FOREIGN KEY([ParentDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCXXConstructExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCXXConstructExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderStatement_Statement] FOREIGN KEY([StatementId])
REFERENCES [dbo].[tblSDKHeaderStatement] ([StatementId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHeader] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHeader] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderNamespace_Namespace] FOREIGN KEY([NamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLEndTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLEndTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment] FOREIGN KEY([HTMLTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLTagComment] ([HTMLTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment] FOREIGN KEY([HTMLTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLTagComment] ([HTMLTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderHTMLStartTagComment_OwningHTMLStartTagComment] FOREIGN KEY([OwningHTMLStartTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLStartTagComment] ([HTMLStartTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderInlineCommandComment_OwningInlineCommandComment] FOREIGN KEY([OwningInlineCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineCommandComment] ([InlineCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroDefinition] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroDefinition] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPackExpansionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPackExpansionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParagraphComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParagraphComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderBlockContentComment_BlockContentComment] FOREIGN KEY([BlockContentCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockContentComment] ([BlockContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPreprocessedEntity] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPreprocessedEntity] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderQualifiedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderQualifiedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderRawComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFullComment_CommentBlockFullComment] FOREIGN KEY([CommentBlockFullCommentId])
REFERENCES [dbo].[tblSDKHeaderFullComment] ([FullCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderRawComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderStatement] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderDeclaration_DeclDeclaration] FOREIGN KEY([DeclDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderStatement] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTextComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTextComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTranslationUnit] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTranslationUnit] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderNamespace_Namespace] FOREIGN KEY([NamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAliasTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAliasTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderParagraphComment_ParagraphComment] FOREIGN KEY([ParagraphCommentId])
REFERENCES [dbo].[tblSDKHeaderParagraphComment] ([ParagraphCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderBlockContentComment_BlockContentComment] FOREIGN KEY([BlockContentCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockContentComment] ([BlockContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_CompleteDeclaration] FOREIGN KEY([CompleteDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderQualifiedType_DesugaredQualifiedType] FOREIGN KEY([DesugaredQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugaredQualifiedType] FOREIGN KEY([DesugaredQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunctionTemplate_OwningFunctionTemplate] FOREIGN KEY([OwningFunctionTemplateId])
REFERENCES [dbo].[tblSDKHeaderFunctionTemplate] ([FunctionTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunction_SpecializedFunction] FOREIGN KEY([SpecializedFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType] FOREIGN KEY([ReturnTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderIntegerValue_CommandIntegerValue] FOREIGN KEY([CommandIdIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderNamespace_OwningNamespace] FOREIGN KEY([OwningNamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderMacroDefinition_DefinitionMacroDefinition] FOREIGN KEY([DefinitionMacroDefinitionId])
REFERENCES [dbo].[tblSDKHeaderMacroDefinition] ([MacroDefinitionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderQualifiedType_PointeeQualifiedType] FOREIGN KEY([PointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderIntegerValue_ParamIndexIntegerValue] FOREIGN KEY([ParamIndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderQualifiedType_QualifiedPointeeQualifiedType] FOREIGN KEY([QualifiedPointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderQualifiedType_ReplacementQualifiedType] FOREIGN KEY([ReplacementQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderQualifiedType_DefaultArgumentQualifiedType] FOREIGN KEY([DefaultArgumentQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter] FOREIGN KEY([TemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTemplateParameter] ([TemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplate_OwningVarTemplate] FOREIGN KEY([OwningVarTemplateId])
REFERENCES [dbo].[tblSDKHeaderVarTemplate] ([VarTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplateSpecialization_VarTemplateSpecialization] FOREIGN KEY([VarTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderVarTemplateSpecialization] ([VarTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_EquivalentQualifiedType] FOREIGN KEY([EquivalentQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_ModifiedQualifiedType] FOREIGN KEY([ModifiedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_LHSExpression] FOREIGN KEY([LHSExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_RHSExpression] FOREIGN KEY([RHSExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderClassLayout_LayoutClassLayout] FOREIGN KEY([LayoutClassLayoutId])
REFERENCES [dbo].[tblSDKHeaderClassLayout] ([ClassLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_OwningClassTemplate] FOREIGN KEY([OwningClassTemplateId])
REFERENCES [dbo].[tblSDKHeaderClassTemplate] ([ClassTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_TemplatedDeclClassTemplate] FOREIGN KEY([TemplatedDeclClassTemplateId])
REFERENCES [dbo].[tblSDKHeaderClassTemplate] ([ClassTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClass_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderType_BuiltInType] FOREIGN KEY([BuiltInTypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderEnumeration_OwningEnumeration] FOREIGN KEY([OwningEnumerationId])
REFERENCES [dbo].[tblSDKHeaderEnumeration] ([EnumerationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderIntegerValue_ValueIntegerValue] FOREIGN KEY([ValueIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_FriendDeclaration] FOREIGN KEY([FriendDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderRawComment_OwningRawComment] FOREIGN KEY([OwningRawCommentId])
REFERENCES [dbo].[tblSDKHeaderRawComment] ([RawCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderClass_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderQualifiedType_InjectedSpecializationTypeQualifiedType] FOREIGN KEY([InjectedSpecializationTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderQualifiedType_ConversionTypeQualifiedType] FOREIGN KEY([ConversionTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFunction_Function] FOREIGN KEY([FunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderExpression_DefaultArgumentExpression] FOREIGN KEY([DefaultArgumentExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderIntegerValue_PositionIntegerValue] FOREIGN KEY([PositionIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter] FOREIGN KEY([TemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTemplateParameter] ([TemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_TemplateDeclaration] FOREIGN KEY([TemplateDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_DepthIntegerValue] FOREIGN KEY([DepthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_IndexIntegerValue] FOREIGN KEY([IndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugardQualifiedType] FOREIGN KEY([DesugardQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_DesugardedQualifiedType] FOREIGN KEY([DesugardedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_BaseTypeQualifiedType] FOREIGN KEY([BaseTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVarTemplate_TemplateDeclVarTemplate] FOREIGN KEY([TemplateDeclVarTemplateId])
REFERENCES [dbo].[tblSDKHeaderVarTemplate] ([VarTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVariable_Variable] FOREIGN KEY([VariableId])
REFERENCES [dbo].[tblSDKHeaderVariable] ([VariableId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderQualifiedType_ElementTypeQualifiedType] FOREIGN KEY([ElementTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderIntegerValue_NumElementsIntegerValue] FOREIGN KEY([NumElementsIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderVTableLayout_OwningVTableLayout] FOREIGN KEY([OwningVTableLayoutId])
REFERENCES [dbo].[tblSDKHeaderVTableLayout] ([VTableLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderIntegerValue_OffsetIntegerValue] FOREIGN KEY([OffsetIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_DecayedQualifiedType] FOREIGN KEY([DecayedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_OriginalQualifiedType] FOREIGN KEY([OriginalQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_PointeeQualifiedType] FOREIGN KEY([PointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderIntegerValue_BitWidthIntegerValue] FOREIGN KEY([BitWidthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_DepthIntegerValue] FOREIGN KEY([DepthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_IndexIntegerValue] FOREIGN KEY([IndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderTypeTemplateParameter_ParameterTypeTemplateParameter] FOREIGN KEY([ParameterTypeTemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTypeTemplateParameter] ([TypeTemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTemplate_DescribedAliasTemplate] FOREIGN KEY([DescribedAliasTemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFunction_InstantiatedFromFunction] FOREIGN KEY([InstantiatedFromFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType] FOREIGN KEY([ReturnTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunctionType_OwningFunctionType] FOREIGN KEY([OwningFunctionTypeId])
REFERENCES [dbo].[tblSDKHeaderFunctionType] ([FunctionTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunction_OwningFunction] FOREIGN KEY([OwningFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderClassTemplateSpecialization_OwningClassTemplateSpecialization] FOREIGN KEY([OwningClassTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderClassTemplateSpecialization] ([ClassTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDependentTemplateSpecializationType_OwningDependentTemplateSpecializationType] FOREIGN KEY([OwningDependentTemplateSpecializationTypeId])
REFERENCES [dbo].[tblSDKHeaderDependentTemplateSpecializationType] ([DependentTemplateSpecializationTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderVarTemplateSpecialization_OwningVarTemplateSpecialization] FOREIGN KEY([OwningVarTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderVarTemplateSpecialization] ([VarTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderTemplateSpecializationType_OwningTemplateSpecializationType] FOREIGN KEY([OwningTemplateSpecializationTypeId])
REFERENCES [dbo].[tblSDKHeaderTemplateSpecializationType] ([TemplateSpecializationTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFunctionTemplateSpecialization_OwningFunctionTemplateSpecialization] FOREIGN KEY([OwningFunctionTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderFunctionTemplateSpecialization] ([FunctionTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderQualifiedType_TypeQualifiedType] FOREIGN KEY([TypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

